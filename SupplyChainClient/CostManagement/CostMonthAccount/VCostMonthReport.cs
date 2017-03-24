using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using System.IO;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCostMonthReport : TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        MCostAccountSubject subject = new MCostAccountSubject();
        public MSubContractBalance subContractOperate = new MSubContractBalance();
        CostMonthAccountBill costBill = new CostMonthAccountBill();
        MStockMng stockModel = new MStockMng();
        IList list_subject = new ArrayList();
        private bool ifZB = true;    

        #region 基本数据
        private string loginPersonName = ConstObject.LoginPersonInfo.Name;
        private string loginDate = ConstObject.LoginDate.ToShortDateString();
        private CurrentProjectInfo ProjectInfo;
        string costStr = "成本对比表";
        string fourCalStr = "四算对比表";
        string personExptr = "劳务费对比表";
        string materialStr = "直接材料对比表";
        string materialDiffStr = "工程材料量差、价差对比表";
        string modualStr = "模板木枋材料费对比表";
        string turnExpStr = "周转料具费用对比表";
        string machineStr = "机械费用对比表";
        string measuresStr = "措施费对比表";
        string managerStr = "现场管理费对比表";
        string specialSubStr = "专业分包费对比表";
        string feesStr = "规费对比表";
        string taxStr = "税金统计表";
        string othersStr = "其它成本统计表";
        #endregion
        #region 结果数据
        IList list_fourcal = new ArrayList();//四算对比明细集合
        IList list_personexp = new ArrayList();//劳务费对比表
        IList list_material = new ArrayList();//直接材料对比表
        IList list_materialdiff = new ArrayList();//工程材料量差、价差对比表
        IList list_modual = new ArrayList();//模板木枋材料费对比表
        IList list_turnexp = new ArrayList();//周转料具费用对比表
        IList list_machine = new ArrayList();//机械费用对比表
        IList list_measures = new ArrayList();//措施费对比表
        IList list_manager = new ArrayList();//现场管理费对比表
        IList list_specialsub = new ArrayList();//专业分包费对比表
        IList list_fees = new ArrayList();//规费对比表
        IList list_tax = new ArrayList();//税金统计表
        IList list_others = new ArrayList();//其它成本统计表
        #endregion

        public VCostMonthReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
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
            GetWbsRootNode();
            //载入科目
            ObjectQuery oq = new ObjectQuery();
            oq.AddOrder(Order.Asc("Code"));
            list_subject = subject.Mm.GetCostAccountSubjects(typeof(CostAccountSubject), oq);

            this.fGrid_FourCal.Rows = 1;
            this.fGridFees.Rows = 1;
            this.fGridMachine.Rows = 1;
            this.fGridManager.Rows = 1;
            this.fGridMaterial.Rows = 1;
            this.fGridMaterialDiff.Rows = 1;
            this.fGridMeasures.Rows = 1;
            this.fGridModual.Rows = 1;
            this.fGridOthers.Rows = 1;
            this.fGridPersonExp.Rows = 1;
            this.fGridSpecSub.Rows = 1;
            this.fGridTax.Rows = 1;
            this.fGridTurnExp.Rows = 1;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSelectGWBS.Click += new EventHandler(btSelectGWBS_Click);
        }

        void btSelectGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
          
            if (txtAccountRootNode.Tag != null)
            {
                frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            }
            frm.IsCheck = true;//是否有checkbox
          
            frm.IsRootNode = true;//这个参数是否只选择隶属关系的根节点
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

        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = fGrid_FourCal.ExportToExcel(costStr, false, false, true);
            if (fileName == "")
                return;
            FlashScreen.Show("正在导出月度成本核算报表...");
            ApplicationClass excel = new ApplicationClass();  // 1. 创建Excel应用程序对象的一个实例，相当于我们从开始菜单打开Excel应用程序

            int startIndex = fileName.LastIndexOf("\\") + 1;
            int endIndex = fileName.IndexOf(".x");
            costStr = fileName.Substring(startIndex, endIndex - startIndex);

            //主文件对象
            Workbook workbook = excel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet;
            mySheet.Name = fourCalStr;

            string tempName = fileName.Replace(costStr, personExptr);
            this.fGridPersonExp.ExportToExcel(tempName, false, false, false);
            Workbook workbook1 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet1 = workbook1.Sheets[1] as Worksheet;
            mySheet1.Name = personExptr;

            tempName = fileName.Replace(costStr, materialStr);
            this.fGridMaterial.ExportToExcel(tempName, false, false, false);
            Workbook workbook2 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet2 = workbook2.Sheets[1] as Worksheet;
            mySheet2.Name = materialStr;

            tempName = fileName.Replace(costStr, materialDiffStr);
            this.fGridMaterialDiff.ExportToExcel(tempName, false, false, false);
            Workbook workbook3 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet3 = workbook3.Sheets[1] as Worksheet;
            mySheet3.Name = materialDiffStr;

            tempName = fileName.Replace(costStr, modualStr);
            this.fGridModual.ExportToExcel(tempName, false, false, false);
            Workbook workbook4 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet4 = workbook4.Sheets[1] as Worksheet;
            mySheet4.Name = modualStr;

            tempName = fileName.Replace(costStr, turnExpStr);
            this.fGridTurnExp.ExportToExcel(tempName, false, false, false);
            Workbook workbook5 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet5 = workbook5.Sheets[1] as Worksheet;
            mySheet5.Name = turnExpStr;

            tempName = fileName.Replace(costStr, machineStr);
            this.fGridMachine.ExportToExcel(tempName, false, false, false);
            Workbook workbook6 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet6 = workbook6.Sheets[1] as Worksheet;
            mySheet6.Name = machineStr;

            tempName = fileName.Replace(costStr, measuresStr);
            this.fGridMeasures.ExportToExcel(tempName, false, false, false);
            Workbook workbook7 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet7 = workbook7.Sheets[1] as Worksheet;
            mySheet7.Name = measuresStr;

            tempName = fileName.Replace(costStr, managerStr);
            this.fGridManager.ExportToExcel(tempName, false, false, false);
            Workbook workbook8 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet8 = workbook8.Sheets[1] as Worksheet;
            mySheet8.Name = managerStr;

            tempName = fileName.Replace(costStr, specialSubStr);
            this.fGridSpecSub.ExportToExcel(tempName, false, false, false);
            Workbook workbook9 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet9 = workbook9.Sheets[1] as Worksheet;
            mySheet9.Name = specialSubStr;

            tempName = fileName.Replace(costStr, feesStr);
            this.fGridFees.ExportToExcel(tempName, false, false, false);
            Workbook workbook10 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet10 = workbook10.Sheets[1] as Worksheet;
            mySheet10.Name = feesStr;

            tempName = fileName.Replace(costStr, taxStr);
            this.fGridTax.ExportToExcel(tempName, false, false, false);
            Workbook workbook11 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet11 = workbook11.Sheets[1] as Worksheet;
            mySheet11.Name = taxStr;

            tempName = fileName.Replace(costStr, othersStr);
            this.fGridOthers.ExportToExcel(tempName, false, false, false);
            Workbook workbook12 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
               Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet12 = workbook12.Sheets[1] as Worksheet;
            mySheet12.Name = othersStr;

            try
            {
                mySheet12.Copy(Type.Missing, mySheet);
                mySheet11.Copy(Type.Missing, mySheet);
                mySheet10.Copy(Type.Missing, mySheet);
                mySheet9.Copy(Type.Missing, mySheet);
                mySheet8.Copy(Type.Missing, mySheet);
                mySheet7.Copy(Type.Missing, mySheet);
                mySheet6.Copy(Type.Missing, mySheet);
                mySheet5.Copy(Type.Missing, mySheet);
                mySheet4.Copy(Type.Missing, mySheet);
                mySheet3.Copy(Type.Missing, mySheet);
                mySheet2.Copy(Type.Missing, mySheet);
                mySheet1.Copy(Type.Missing, mySheet);
                
                workbook.Save();
               
            }
            catch (Exception e1)
            {
                throw new Exception("导出月度成本核算分析套表出错！");
            }
            finally {
                FlashScreen.Close();
                //关闭工作表和退出Excel
                workbook.Close(false, Type.Missing, Type.Missing);
                workbook1.Close(false, Type.Missing, Type.Missing);
                workbook2.Close(false, Type.Missing, Type.Missing);
                workbook3.Close(false, Type.Missing, Type.Missing);
                workbook4.Close(false, Type.Missing, Type.Missing);
                workbook5.Close(false, Type.Missing, Type.Missing);
                workbook6.Close(false, Type.Missing, Type.Missing);
                workbook7.Close(false, Type.Missing, Type.Missing);
                workbook8.Close(false, Type.Missing, Type.Missing);
                workbook9.Close(false, Type.Missing, Type.Missing);
                workbook10.Close(false, Type.Missing, Type.Missing);
                workbook11.Close(false, Type.Missing, Type.Missing);
                workbook12.Close(false, Type.Missing, Type.Missing);
                //如果报表文件存在，先删除
                if (File.Exists(fileName.Replace(costStr, personExptr)))
                {
                    File.Delete(fileName.Replace(costStr, personExptr));
                }
                if (File.Exists(fileName.Replace(costStr, materialStr)))
                {
                    File.Delete(fileName.Replace(costStr, materialStr));
                }
                if (File.Exists(fileName.Replace(costStr, materialDiffStr)))
                {
                    File.Delete(fileName.Replace(costStr, materialDiffStr));
                }
                if (File.Exists(fileName.Replace(costStr, modualStr)))
                {
                    File.Delete(fileName.Replace(costStr, modualStr));
                }
                if (File.Exists(fileName.Replace(costStr, turnExpStr)))
                {
                    File.Delete(fileName.Replace(costStr, turnExpStr));
                }
                if (File.Exists(fileName.Replace(costStr, machineStr)))
                {
                    File.Delete(fileName.Replace(costStr, machineStr));
                }
                if (File.Exists(fileName.Replace(costStr, measuresStr)))
                {
                    File.Delete(fileName.Replace(costStr, measuresStr));
                }
                if (File.Exists(fileName.Replace(costStr, managerStr)))
                {
                    File.Delete(fileName.Replace(costStr, managerStr));
                }
                if (File.Exists(fileName.Replace(costStr, specialSubStr)))
                {
                    File.Delete(fileName.Replace(costStr, specialSubStr));
                }
                if (File.Exists(fileName.Replace(costStr, feesStr)))
                {
                    File.Delete(fileName.Replace(costStr, feesStr));
                }
                if (File.Exists(fileName.Replace(costStr, taxStr)))
                {
                    File.Delete(fileName.Replace(costStr, taxStr));
                }
                if (File.Exists(fileName.Replace(costStr, othersStr)))
                {
                    File.Delete(fileName.Replace(costStr, othersStr));
                }
                excel.Quit();
                excel = null;
            }
            MessageBox.Show("导出月度成本核算分析表成功！");
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

            IList list = subContractOperate.ObjectQuery(typeof(GWBSTree), objectQuery);
            IList taskList = new ArrayList();
            if (list == null || list.Count == 0)
            {
                txtAccountRootNode.Text = "";
                txtAccountRootNode.Tag = null;
            }
            else
            {
                GWBSTree tGwbs = (GWBSTree)list[0];
                taskList.Add(tGwbs.Id);
                txtAccountRootNode.Text = tGwbs.Name;
                txtAccountRootNode.Tag = taskList;
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
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
            //if (ClientUtil.ToString(this.txtAccountRootNode.Text) == "")
            //{
            //    MessageBox.Show("请选择核算节点！");
            //    return;
            //}
            FlashScreen.Show("正在统计月度成本核算报表...");
            try
            {
                LoadTempleteFile("成本对比表.flx");
                LoadTempleteFile(personExptr + ".flx");
                LoadTempleteFile(materialStr + ".flx");
                LoadTempleteFile(materialDiffStr + ".flx");
                LoadTempleteFile(modualStr + ".flx");
                LoadTempleteFile(turnExpStr + ".flx");
                LoadTempleteFile(machineStr + ".flx");
                LoadTempleteFile(measuresStr + ".flx");
                LoadTempleteFile(managerStr + ".flx");
                LoadTempleteFile(specialSubStr + ".flx");
                LoadTempleteFile(feesStr + ".flx");
                LoadTempleteFile(taxStr + ".flx");
                LoadTempleteFile(othersStr + ".flx");

                //载入数据
                IList taskList = txtAccountRootNode.Tag as ArrayList;
                if (taskList == null) taskList = new ArrayList();
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
                oq.AddCriterion(Expression.Eq("Kjn", Convert.ToInt32(this.cmbYear.Text)));
                oq.AddCriterion(Expression.Eq("Kjy", Convert.ToInt32(cboFiscalMonth.Text)));
                //oq.AddCriterion(Expression.Sql(" instr('" + acctNode.SysCode + "',{alias}.accountrange)>0"));
                IList list = model.CostMonthAccSrv.GetCostMonthAccountBill(oq);
                if (list != null && list.Count > 0)
                {
                    costBill = model.CombCostMonthAccountBill(list);
                    //costBill = (CostMonthAccountBill)list[0];
                    //costBill = model.TransCostMonthAccountBill(costBill, acctNode.Id, 1);
                }
                else
                {
                    costBill = new CostMonthAccountBill();
                }
                costBill = model.TransCostMonthAccountBill(costBill, taskList, 1);
                this.LoadFourCalData();
                this.LoadPersonExpData();
                this.LoadMaterialData();
                this.LoadMaterialDiffData();
                this.LoadModualData();
                this.LoadTurnExpData();
                this.LoadMachineData();
                this.LoadMeasuresData();
                this.LoadManagerData();
                this.LoadSpecialSubData();
                this.LoadFeesData();
                this.LoadTaxData();
                this.LoadOthersData();

                //设置外观
                CommonUtil.SetFlexGridFace(this.fGrid_FourCal);
                CommonUtil.SetFlexGridFace(this.fGridFees);
                CommonUtil.SetFlexGridFace(this.fGridMachine);
                CommonUtil.SetFlexGridFace(this.fGridManager);
                CommonUtil.SetFlexGridFace(this.fGridMaterial);
                CommonUtil.SetFlexGridFace(this.fGridMaterialDiff);
                CommonUtil.SetFlexGridFace(this.fGridMeasures);
                CommonUtil.SetFlexGridFace(this.fGridModual);
                CommonUtil.SetFlexGridFace(this.fGridOthers);
                CommonUtil.SetFlexGridFace(this.fGridPersonExp);
                CommonUtil.SetFlexGridFace(this.fGridSpecSub);
                CommonUtil.SetFlexGridFace(this.fGridTax);
                CommonUtil.SetFlexGridFace(this.fGridTurnExp);
            }
            catch (Exception e1)
            {
                throw new Exception("统计月度成本核算分析套表出错！");
            }
            finally
            {
                FlashScreen.Close();
            }

        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == "成本对比表.flx")
                {                   
                    fGrid_FourCal.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "劳务费对比表.flx")
                {
                    fGridPersonExp.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "直接材料对比表.flx")
                {
                    this.fGridMaterial.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "工程材料量差、价差对比表.flx")
                {
                    this.fGridMaterialDiff.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "模板木枋材料费对比表.flx")
                {
                    this.fGridModual.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "周转料具费用对比表.flx")
                {
                    this.fGridTurnExp.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "机械费用对比表.flx")
                {
                    this.fGridMachine.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "措施费对比表.flx")
                {
                    this.fGridMeasures.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "现场管理费对比表.flx")
                {
                    this.fGridManager.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "专业分包费对比表.flx")
                {
                    this.fGridSpecSub.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "规费对比表.flx")
                {
                    this.fGridFees.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "税金统计表.flx")
                {
                    this.fGridTax.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "其它成本统计表.flx")
                {
                    this.fGridOthers.OpenFile(path + "\\" + modelName);//载入格式
                }
            } else {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        #region  构造数据

        /// <summary>
        /// 写入合计行的数据
        /// </summary>
        /// <param name="flexGrid">二维表对象</param>
        /// <param name="startRow">计算范围的开始行</param>
        /// <param name="endRow">计算范围的结束行</param>
        /// <param name="startCol">计算范围的开始列</param>
        /// <param name="endCol">计算范围的结束列</param>
        private void WriteSumGridData(CustomFlexGrid flexGrid, int startRow, int endRow, int startCol, int endCol)
        {
            flexGrid.Cell(endRow + 1, startCol - 1).Text = "合计：";           
            for (int i = startCol; i <= endCol; i++)
            {
                decimal sumValue = 0;
                for (int t = startRow; t <= endRow; t++)
                {
                    string ifCalSum = ClientUtil.ToString(flexGrid.Cell(t, 1).Tag);
                    if (ifCalSum == "1")
                    {
                        sumValue += ClientUtil.ToDecimal(flexGrid.Cell(t, i).Text);
                    }
                }
                flexGrid.Cell(endRow + 1, i).Text = sumValue + "";
            }
            FlexCell.Range range = flexGrid.Range(endRow + 1, 1, endRow + 1, endCol);
            CommonUtil.SetFlexGridDetailFormat(range);
            flexGrid.Cell(endRow + 1, startCol - 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
        }

        private CostMonthAccDtlConsume AddCostMonthAccDtlConsume(CostMonthAccDtlConsume dtlConsume,CostMonthAccDtlConsume addDtlConsume)
        {
            dtlConsume.CurrIncomeQuantity += addDtlConsume.CurrIncomeQuantity;
            dtlConsume.CurrRealConsumePlanQuantity += addDtlConsume.CurrRealConsumePlanQuantity;
            dtlConsume.CurrIncomeTotalPrice += addDtlConsume.CurrIncomeTotalPrice;
            dtlConsume.CurrRealConsumePlanTotalPrice += addDtlConsume.CurrRealConsumePlanTotalPrice;
            dtlConsume.CurrRealConsumeQuantity += addDtlConsume.CurrRealConsumeQuantity;
            dtlConsume.CurrRealConsumeTotalPrice += addDtlConsume.CurrRealConsumeTotalPrice;
            dtlConsume.CurrResponsiConsumeQuantity += addDtlConsume.CurrResponsiConsumeQuantity;
            dtlConsume.CurrResponsiConsumeTotalPrice += addDtlConsume.CurrResponsiConsumeTotalPrice;
            dtlConsume.SumIncomeQuantity += addDtlConsume.SumIncomeQuantity;
            dtlConsume.SumIncomeTotalPrice += addDtlConsume.SumIncomeTotalPrice;
            dtlConsume.SumRealConsumePlanQuantity += addDtlConsume.SumRealConsumePlanQuantity;
            dtlConsume.SumRealConsumePlanTotalPrice += addDtlConsume.SumRealConsumePlanTotalPrice;
            dtlConsume.SumRealConsumeQuantity += addDtlConsume.SumRealConsumeQuantity;
            dtlConsume.SumRealConsumeTotalPrice += addDtlConsume.SumRealConsumeTotalPrice;
            dtlConsume.SumResponsiConsumeQuantity += addDtlConsume.SumResponsiConsumeQuantity;
            dtlConsume.SumResponsiConsumeTotalPrice += addDtlConsume.SumResponsiConsumeTotalPrice;

            return dtlConsume;
        }

        /// <summary>
        /// 查询此根节点下几层的成本科目,生成月度核算物资消耗对象集合,并排序
        /// </summary>
        /// <param name="rootSubject">查询的成本科目根节点</param>
        /// <param name="queryLevel">向下查询几层</param>
        /// <param name="ifContainSelf">是否包含自己</param>
        private IList GetCostSubjectList(string rootSubjectCode, int queryLevel, bool ifContainSelf)
        {
            CostAccountSubject rootSubject = new CostAccountSubject();
            foreach (CostAccountSubject subject in list_subject)
            {
                if (subject.Code == rootSubjectCode)
                {
                    rootSubject = subject;
                    break;
                }
            }

            IList list = new ArrayList();
            int rootLevel = rootSubject.Level;
            int maxLevel = rootLevel + queryLevel;
            string rootSyscode = rootSubject.SysCode;
            foreach (CostAccountSubject subject in list_subject)
            {
                if (ifContainSelf == true && subject.SysCode.Equals(rootSyscode))
                {
                    CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    dtlConsume.Data2 = "1";//相对层级
                    dtlConsume.CostingSubjectGUID = subject;
                    dtlConsume.CostingSubjectName = subject.Name;
                    dtlConsume.CostSubjectCode = subject.Code;
                    dtlConsume.CostSubjectSyscode = subject.SysCode;
                    list.Add(dtlConsume);
                }
                if (rootSyscode != null && subject.SysCode.Contains(rootSyscode) && subject.Level > rootLevel && subject.Level <= maxLevel)
                {
                    CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    dtlConsume.Data2 = (subject.Level - rootLevel) + "";//相对层级
                    dtlConsume.CostingSubjectGUID = subject;
                    dtlConsume.CostingSubjectName = subject.Name;
                    dtlConsume.CostSubjectCode = subject.Code;
                    dtlConsume.CostSubjectSyscode = subject.SysCode;
                    list.Add(dtlConsume);
                }
            }
            return list;
        }

        private IList DelZeroData(IList list)
        {
            //剔除多余数据
            IList list_temp = new ArrayList();
            foreach (CostMonthAccDtlConsume m_dtlConsume in list)
            {
                if (m_dtlConsume.CurrRealConsumeTotalPrice != 0 || m_dtlConsume.SumRealConsumeTotalPrice != 0 ||
                    m_dtlConsume.CurrIncomeTotalPrice != 0 || m_dtlConsume.SumIncomeTotalPrice != 0)
                {
                    list_temp.Add(m_dtlConsume);
                }
            }

            list.Clear();
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_temp)
            {
                list.Add(m_dtlConsume);
            }
            return list;
        }

        //补充工程量单位
        private IList AddUnitToCostMonthAccDtlConsume(IList list)
        {
            IList unitList = new ArrayList();
            foreach (CostMonthAccountDtl dtl in costBill.Details)//按科目包含关系形成科目+计量单位HashTable
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    Hashtable ht_add = new Hashtable();
                    int currIndex = 0;
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list)
                    {
                        if (dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode)
                            && !unitList.Contains(dtlConsume.CostingSubjectGUID.Id + "-" + dtlConsume.RationUnitName) && m_dtlConsume.Data2 == "2")
                        {
                            if (ClientUtil.ToString(m_dtlConsume.RationUnitName) == ""
                                && !unitList.Contains(m_dtlConsume.CostingSubjectGUID.Id + "-" + m_dtlConsume.RationUnitName))//补充计量单位
                            {
                                m_dtlConsume.RationUnitName = dtlConsume.RationUnitName;
                            }
                            else
                            {
                                CostMonthAccDtlConsume temp = new CostMonthAccDtlConsume();
                                temp.CostingSubjectGUID = m_dtlConsume.CostingSubjectGUID;
                                temp.CostingSubjectName = m_dtlConsume.CostingSubjectName;
                                temp.CostSubjectCode = m_dtlConsume.CostSubjectCode;
                                temp.CostSubjectSyscode = m_dtlConsume.CostSubjectSyscode;
                                temp.Data2 = m_dtlConsume.Data2;
                                temp.RationUnitName = dtlConsume.RationUnitName;
                                ht_add.Add(currIndex, temp);
                            }
                            if (!unitList.Contains(dtlConsume.CostingSubjectGUID.Id + "-" + dtlConsume.RationUnitName))
                            {
                                unitList.Add(dtlConsume.CostingSubjectGUID.Id + "-" + dtlConsume.RationUnitName);
                            }
                        }
                        currIndex++;
                    }
                    if (ht_add.Count >= 1)//补充数据
                    {
                        foreach (int cIndex in ht_add.Keys)
                        {
                            list.Insert(cIndex, (CostMonthAccDtlConsume)ht_add[cIndex]);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region 四算对比表
        private IList CreateFourCalData()
        {
            string FourSubZB = "C5";
            string ZJESubZB = "C511";//直接费
            int levelZB = 0;
            list_fourcal.Clear();
            if (ifZB == false)
            {
                FourSubZB = "C56";
                ZJESubZB = "C561";//直接费
                levelZB = 1;
            }

            //1: 取得科目集合
            IList list_temp = this.GetCostSubjectList(FourSubZB, 2, false);
            int i = 1;
            int tt = 1;
            foreach (CostMonthAccDtlConsume dtlConsume in list_temp)
            {
                if (dtlConsume.CostingSubjectGUID.Level == (3 + levelZB) && dtlConsume.CostSubjectCode.Contains(ZJESubZB) == false)
                {
                    continue;
                }
                if (dtlConsume.CostingSubjectGUID.Level == (2 + levelZB))
                {
                    dtlConsume.Data1 = CommonUtil.GetChineseNumber(tt);
                    tt++;
                }
                else
                {
                    dtlConsume.Data1 = "  " + i;
                    i++;
                }
                list_fourcal.Add(dtlConsume);
            }

            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume four_dtlConsume in list_fourcal)
                    {
                        if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(four_dtlConsume.CostSubjectCode))
                        {
                            this.AddCostMonthAccDtlConsume(four_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            return list_fourcal;
        }

        private void LoadFourCalData()
        {
            list_fourcal = CreateFourCalData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = list_fourcal.Count;

            //插入明细行
            this.fGrid_FourCal.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_FourCal.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_FourCal.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGrid_FourCal.Cell(3, 2).Text = costBill.ProjectName;
            this.fGrid_FourCal.Cell(3, 10).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_fourcal[i];
                fGrid_FourCal.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGrid_FourCal.Cell(dtlStartRowNum + i, 1).Tag = dtlConsume.Data2;
                fGrid_FourCal.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_FourCal.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                fGrid_FourCal.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //当前值
                fGrid_FourCal.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrIncomeTotalPrice);
                fGrid_FourCal.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                fGrid_FourCal.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                fGrid_FourCal.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);

                fGrid_FourCal.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrIncomeTotalPrice != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice) * 100 / dtlConsume.CurrIncomeTotalPrice, 4));
                }
                else {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 8).Text = "0";
                }
                fGrid_FourCal.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice) * 100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 10).Text = "0";
                }
                //累计值
                fGrid_FourCal.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(dtlConsume.SumIncomeTotalPrice);
                fGrid_FourCal.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                fGrid_FourCal.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                fGrid_FourCal.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);

                fGrid_FourCal.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumIncomeTotalPrice != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice) * 100 / dtlConsume.SumIncomeTotalPrice, 4));
                }
                else
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 16).Text = "0";
                }
                fGrid_FourCal.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumResponsiConsumeTotalPrice != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice) * 100 / dtlConsume.SumResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 18).Text = "0";
                }
            }
            //写入合计值
            this.WriteSumGridData(fGrid_FourCal, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 18);
            if (ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 3).Text) != 0)
            {
                fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 8).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 7).Text) * 100 / ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 3).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 4).Text) != 0)
            {
                fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 10).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 9).Text) * 100 / ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 4).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 11).Text) != 0)
            {
                fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 16).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 15).Text) * 100 / ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 11).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 12).Text) != 0)
            {
                fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 18).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 17).Text) * 100 / ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 12).Text), 2) + "";
            }
        }

        #endregion

        #region 劳务费对比表
        private IList CreatePersonExpData()
        {
            string LWFSubZB = "C51101";//劳务费
            if (ifZB == false)
            {
                LWFSubZB = "C56101";
            }
            this.list_personexp.Clear();
            //1: 取得科目集合
            IList list_personexp = this.GetCostSubjectList(LWFSubZB, 2, true);
            //补充工程量单位
            list_personexp = this.AddUnitToCostMonthAccDtlConsume(list_personexp);

            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_personexp)
                    {
                        if (dtlConsume.CostSubjectCode != null && m_dtlConsume.CostSubjectCode == LWFSubZB)
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode))// && dtlConsume.RationUnitName == m_dtlConsume.RationUnitName
                        {
                            if (m_dtlConsume.Data2 == "2")//最底层
                            {
                                if (dtlConsume.RationUnitName == m_dtlConsume.RationUnitName)
                                {
                                    this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                                }
                            }
                            else
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                    }
                }
            }
            list_personexp = this.DelZeroData(list_personexp);
            //增加序号
            int level1 = 1;
            int level2 = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_personexp)
            {
                if (m_dtlConsume.CostSubjectCode == LWFSubZB)
                {
                    continue;
                }
                if (m_dtlConsume.Data2 == "1")
                {
                    m_dtlConsume.Data1 = CommonUtil.GetChineseNumber(level1);
                    level2 = 1;
                    level1++;
                }
                if (m_dtlConsume.Data2 == "2")
                {
                    m_dtlConsume.Data1 = "  " + level2;
                    level2++;
                }
            }
            return list_personexp;
        }

        private void LoadPersonExpData()
        {
            list_personexp = CreatePersonExpData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = this.list_personexp.Count;

            //插入明细行
            this.fGridPersonExp.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridPersonExp.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridPersonExp.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridPersonExp.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridPersonExp.Cell(3, 10).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_personexp[i];

                fGridPersonExp.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridPersonExp.Cell(dtlStartRowNum + i, 1).Tag = dtlConsume.Data2;
                fGridPersonExp.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                if (dtlConsume.Data2 == "2")
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName + "(" + dtlConsume.RationUnitName + ")";
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //当前合同收入
                fGridPersonExp.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrIncomeQuantity);
                if (dtlConsume.CurrIncomeQuantity != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrIncomeTotalPrice / dtlConsume.CurrIncomeQuantity, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 4).Text = "0";
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrIncomeTotalPrice);
                //当前责任成本
                fGridPersonExp.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeQuantity);
                if (dtlConsume.CurrResponsiConsumeQuantity != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice / dtlConsume.CurrResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 7).Text = "0";
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                //当前计划成本
                fGridPersonExp.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanQuantity);
                if (dtlConsume.CurrRealConsumePlanQuantity != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice / dtlConsume.CurrRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 10).Text = "0";
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                //当前实际成本
                fGridPersonExp.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeQuantity);
                if (dtlConsume.CurrRealConsumeQuantity != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumeTotalPrice / dtlConsume.CurrRealConsumeQuantity, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 13).Text = "0";
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                //本期利润
                fGridPersonExp.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrIncomeTotalPrice != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice) * 100 / dtlConsume.CurrIncomeTotalPrice, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 16).Text = "0";
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice) * 100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 18).Text = "0";
                }

                //累计合同收入
                fGridPersonExp.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(dtlConsume.SumIncomeQuantity);
                if (dtlConsume.SumIncomeQuantity != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumIncomeTotalPrice / dtlConsume.SumIncomeQuantity, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 20).Text = "0";
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(dtlConsume.SumIncomeTotalPrice);
                //累计责任成本
                fGridPersonExp.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeQuantity);
                if (dtlConsume.SumResponsiConsumeQuantity != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice / dtlConsume.SumResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 23).Text = "0";
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                //累计计划成本
                fGridPersonExp.Cell(dtlStartRowNum + i, 25).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanQuantity);
                if (dtlConsume.SumRealConsumePlanQuantity != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 26).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice / dtlConsume.SumRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 26).Text = "0";
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 27).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                //累计实际成本
                fGridPersonExp.Cell(dtlStartRowNum + i, 28).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeQuantity);
                if (dtlConsume.SumRealConsumeQuantity != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 29).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumeTotalPrice / dtlConsume.SumRealConsumeQuantity, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 29).Text = "0";
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 30).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
                //累计利润
                fGridPersonExp.Cell(dtlStartRowNum + i, 31).Text = ClientUtil.ToString(dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumIncomeTotalPrice != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 32).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice) * 100 / dtlConsume.SumIncomeTotalPrice, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 32).Text = "0";
                }
                fGridPersonExp.Cell(dtlStartRowNum + i, 33).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumResponsiConsumeTotalPrice != 0)
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 34).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice)* 100 / dtlConsume.SumResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridPersonExp.Cell(dtlStartRowNum + i, 34).Text = "0";
                }
            }
            //写入合计值
            this.WriteSumGridData(fGridPersonExp, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 34);
            if (ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 5).Text) != 0)
            {
                fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 16).Text = decimal.Round(ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 15).Text) * 100 / ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 5).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 8).Text) != 0)
            {
                fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 18).Text = decimal.Round(ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 17).Text) * 100 / ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 8).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 21).Text) != 0)
            {
                fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 32).Text = decimal.Round(ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 31).Text) * 100 / ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 21).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 24).Text) != 0)
            {
                fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 34).Text = decimal.Round(ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 33).Text) * 100 / ClientUtil.ToDecimal(fGridPersonExp.Cell(dtlStartRowNum + dtlCount, 24).Text), 2) + "";
            }

        }
        #endregion

        #region 直接材料对比表
        private IList CreateMaterialData()
        {
            string ZJCLSubZB = "C51102";//直接材料费
            if (ifZB == false)
            {
                ZJCLSubZB = "C56102";
            }
            this.list_material.Clear();
            //1: 取得科目集合
            IList list_material = this.GetCostSubjectList(ZJCLSubZB, 2, true);
            //补充工程量单位
            list_material = this.AddUnitToCostMonthAccDtlConsume(list_material);
            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_material)
                    {
                        if (dtlConsume.CostSubjectCode != null && m_dtlConsume.CostSubjectCode == ZJCLSubZB)
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode))
                        {
                            if (m_dtlConsume.Data2 == "2")//最底层
                            {
                                if (dtlConsume.RationUnitName == m_dtlConsume.RationUnitName)
                                {
                                    this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                                }
                            }
                            else
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                    }
                }
            }

            list_material = this.DelZeroData(list_material);
            //增加序号
            int level1 = 1;
            int level2 = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_material)
            {
                if (m_dtlConsume.Data2 == "1")
                {
                    m_dtlConsume.Data1 = CommonUtil.GetChineseNumber(level1);
                    level2 = 1;
                    level1++;
                }
                if (m_dtlConsume.Data2 == "2")
                {
                    m_dtlConsume.Data1 = "  " + level2;
                    level2++;
                }
            }
            return list_material;
        }

        private void LoadMaterialData()
        {
            list_material = CreateMaterialData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = this.list_material.Count;

            //插入明细行
            this.fGridMaterial.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridMaterial.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridMaterial.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridMaterial.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridMaterial.Cell(3, 10).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_material[i];

                fGridMaterial.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridMaterial.Cell(dtlStartRowNum + i, 1).Tag = dtlConsume.Data2;
                fGridMaterial.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                if (dtlConsume.Data2 == "2")
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName + "(" + dtlConsume.RationUnitName + ")"; ;
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                }
                
                fGridMaterial.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //当前合同收入
                fGridMaterial.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrIncomeQuantity);
                if (dtlConsume.CurrIncomeQuantity != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrIncomeTotalPrice / dtlConsume.CurrIncomeQuantity, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 4).Text = "0";
                }
                fGridMaterial.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrIncomeTotalPrice);
                //当前责任成本
                fGridMaterial.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeQuantity);
                if (dtlConsume.CurrResponsiConsumeQuantity != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice / dtlConsume.CurrResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 7).Text = "0";
                }
                fGridMaterial.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                //当前计划成本
                fGridMaterial.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanQuantity);
                if (dtlConsume.CurrRealConsumePlanQuantity != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice / dtlConsume.CurrRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 10).Text = "0";
                }
                fGridMaterial.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                //当前实际成本
                fGridMaterial.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeQuantity);
                if (dtlConsume.CurrRealConsumeQuantity != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumeTotalPrice / dtlConsume.CurrRealConsumeQuantity, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 13).Text = "0";
                }
                fGridMaterial.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                //本期利润
                fGridMaterial.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrIncomeTotalPrice != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrIncomeTotalPrice, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 16).Text = "0";
                }
                fGridMaterial.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 18).Text = "0";
                }
                //累计合同收入
                fGridMaterial.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(dtlConsume.SumIncomeQuantity);
                if (dtlConsume.SumIncomeQuantity != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumIncomeTotalPrice / dtlConsume.SumIncomeQuantity, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 20).Text = "0";
                }
                fGridMaterial.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(dtlConsume.SumIncomeTotalPrice);
                //累计责任成本
                fGridMaterial.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeQuantity);
                if (dtlConsume.SumResponsiConsumeQuantity != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice / dtlConsume.SumResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 23).Text = "0";
                }
                fGridMaterial.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                //累计计划成本
                fGridMaterial.Cell(dtlStartRowNum + i, 25).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanQuantity);
                if (dtlConsume.SumRealConsumePlanQuantity != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 26).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice / dtlConsume.SumRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 26).Text = "0";
                }
                fGridMaterial.Cell(dtlStartRowNum + i, 27).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                //累计实际成本
                fGridMaterial.Cell(dtlStartRowNum + i, 28).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeQuantity);
                if (dtlConsume.SumRealConsumeQuantity != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 29).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumeTotalPrice / dtlConsume.SumRealConsumeQuantity, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 29).Text = "0";
                }
                fGridMaterial.Cell(dtlStartRowNum + i, 30).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
                //累计利润
                fGridMaterial.Cell(dtlStartRowNum + i, 31).Text = ClientUtil.ToString(dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumIncomeTotalPrice != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 32).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice)* 100 / dtlConsume.SumIncomeTotalPrice, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 32).Text = "0";
                }
                fGridMaterial.Cell(dtlStartRowNum + i, 33).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumResponsiConsumeTotalPrice != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 34).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice) *100 / dtlConsume.SumResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridMaterial.Cell(dtlStartRowNum + i, 34).Text = "0";
                }
            }
            //写入合计值
            if (dtlCount > 0)
            {
                this.WriteSumGridData(fGridMaterial, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 34);
                if (ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 5).Text) != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + dtlCount, 16).Text = decimal.Round(ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 15).Text) * 100 / ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 5).Text), 2) + "";
                }
                if (ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 8).Text) != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + dtlCount, 18).Text = decimal.Round(ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 17).Text) * 100 / ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 8).Text), 2) + "";
                }
                if (ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 21).Text) != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + dtlCount, 32).Text = decimal.Round(ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 31).Text) * 100 / ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 21).Text), 2) + "";
                }
                if (ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 24).Text) != 0)
                {
                    fGridMaterial.Cell(dtlStartRowNum + dtlCount, 34).Text = decimal.Round(ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 33).Text) * 100 / ClientUtil.ToDecimal(fGridMaterial.Cell(dtlStartRowNum + dtlCount, 24).Text), 2) + "";
                }
            }

        }

        #endregion

        #region 工程材料量差、价差对比表
        private IList CreateMaterialDiffData()
        {
            string GCCLGJSubZB = "C5110203";//工程材料费_钢筋
            string GCCLSPTSubZB = "C5110204";//工程材料费_商品砼
            if (ifZB == false)
            {
                GCCLGJSubZB = "C5610201";
                GCCLSPTSubZB = "C5610202";
            }
            this.list_materialdiff.Clear();
            Hashtable ht_temp = new Hashtable();
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    if (dtlConsume.CostSubjectCode != null && (dtlConsume.CostSubjectCode.Contains(GCCLGJSubZB) || dtlConsume.CostSubjectCode.Contains(GCCLSPTSubZB)))
                    {
                        string tempStr = dtlConsume.CostSubjectCode + "-" + dtlConsume.ResourceTypeGUID;
                        if (!ht_temp.Contains(tempStr))
                        {
                            ht_temp.Add(tempStr, dtlConsume);
                        }
                        else
                        {
                            CostMonthAccDtlConsume temp_consume = (CostMonthAccDtlConsume)ht_temp[tempStr];
                            this.AddCostMonthAccDtlConsume(temp_consume, dtlConsume);
                            ht_temp.Remove(tempStr);
                            ht_temp.Add(tempStr, temp_consume);
                        }
                    }
                }
            }
            //1: 取得科目集合
            IList list_materialdiff = this.GetCostSubjectList(GCCLGJSubZB, 2, false);
            IList list_materialdiff_1 = this.GetCostSubjectList(GCCLSPTSubZB, 2, false);
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_materialdiff_1)
            {
                list_materialdiff.Add(m_dtlConsume);
            }
            int maxLevel = 0;//最大层数
            foreach (string linkStr in ht_temp.Keys)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)ht_temp[linkStr];
                string subjectCode = linkStr.Substring(0, linkStr.IndexOf('-'));
                int tt = 1;
                Hashtable ht_insert = new Hashtable();
                foreach (CostMonthAccDtlConsume temp_consume in list_materialdiff)
                {
                    if (ClientUtil.ToInt(temp_consume.Data2) > maxLevel)
                    {
                        maxLevel = ClientUtil.ToInt(temp_consume.Data2);
                    }
                    if (subjectCode.Contains(temp_consume.CostSubjectCode) && ClientUtil.ToString(temp_consume.ResourceTypeGUID) == "")
                    {
                        this.AddCostMonthAccDtlConsume(temp_consume, dtlConsume);
                    }
                    if (subjectCode == temp_consume.CostSubjectCode && ClientUtil.ToString(temp_consume.ResourceTypeGUID) == "")
                    {
                        dtlConsume.Data3 = tt + "";
                        if (!ht_insert.Contains(subjectCode))
                        {
                            ht_insert.Add(subjectCode, dtlConsume);
                        }
                        else
                        {
                            CostMonthAccDtlConsume i_dtlConsume = (CostMonthAccDtlConsume)ht_insert[subjectCode];
                            this.AddCostMonthAccDtlConsume(i_dtlConsume, dtlConsume);
                            ht_insert.Remove(subjectCode);
                            ht_insert.Add(subjectCode, i_dtlConsume);
                        }
                        //list_turnexp.Insert(tt, dtlConsume);
                    }
                    tt++;
                }
            }
            //加入科目下的规格型号
            IList list_allMaterialdiff = new ArrayList();
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_materialdiff)
            {
                list_allMaterialdiff.Add(m_dtlConsume);
                int num = 1;
                if (ClientUtil.ToInt(m_dtlConsume.Data2) == maxLevel)//最大层的科目下加规格型号
                {
                    foreach (string linkStr in ht_temp.Keys)
                    {
                        CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)ht_temp[linkStr];
                        string subjectCode = linkStr.Substring(0, linkStr.IndexOf('-'));
                        if (subjectCode.Contains(m_dtlConsume.CostSubjectCode))
                        {
                            CostMonthAccDtlConsume r_dtlConsume = new CostMonthAccDtlConsume();
                            r_dtlConsume.Data1 = num + ".";
                            r_dtlConsume.Data2 = (maxLevel + 1) + "";//相对层级
                            r_dtlConsume.CostingSubjectName = dtlConsume.ResourceTypeName + "_" + dtlConsume.ResourceTypeSpec + "_" + dtlConsume.ResourceTypeStuff;
                            this.AddCostMonthAccDtlConsume(r_dtlConsume, dtlConsume);
                            list_allMaterialdiff.Add(r_dtlConsume);
                            num++;
                        }
                    }
                }
            }

            //剔除多余数据
            IList list_temp = new ArrayList();
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_allMaterialdiff)
            {
                if (m_dtlConsume.CurrRealConsumeTotalPrice != 0 || m_dtlConsume.CurrResponsiConsumeTotalPrice != 0 || m_dtlConsume.CurrIncomeTotalPrice != 0)
                {
                    list_temp.Add(m_dtlConsume);
                }
            }

            list_allMaterialdiff.Clear();
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_temp)
            {
                list_allMaterialdiff.Add(m_dtlConsume);
            }

            //增加序号
            int level1 = 1;
            int level2 = 1;
            int levelOther = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_allMaterialdiff)
            {
                if (m_dtlConsume.Data2 == "1")
                {
                    m_dtlConsume.Data1 = CommonUtil.GetChineseNumber(level1);
                    level2 = 1;
                    level1++;
                }
                if (m_dtlConsume.Data2 == "2")
                {
                    m_dtlConsume.Data1 = " (" + CommonUtil.GetChineseNumber(level2) + ")";
                    levelOther = 1;
                    level2++;
                }
                if (ClientUtil.ToString(m_dtlConsume.Data2) == "")
                {
                    m_dtlConsume.Data1 = "   " + levelOther + ".";
                    levelOther++;
                }
            }
            return list_allMaterialdiff;
        }

        private void LoadMaterialDiffData()
        {
            list_materialdiff = CreateMaterialDiffData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = this.list_materialdiff.Count;

            //插入明细行
            this.fGridMaterialDiff.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridMaterialDiff.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridMaterialDiff.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridMaterialDiff.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridMaterialDiff.Cell(3, 10).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_materialdiff[i];

                fGridMaterialDiff.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 1).Tag = dtlConsume.Data2;
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //工程量对比
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrIncomeQuantity);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeQuantity);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanQuantity);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeQuantity);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(dtlConsume.CurrIncomeQuantity - dtlConsume.CurrRealConsumeQuantity);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeQuantity - dtlConsume.CurrRealConsumeQuantity);
                //单价对比
                decimal currIncomePrice = 0;
                decimal currRespConsumePrice = 0;
                decimal currRealConsumePrice = 0;
                if (dtlConsume.CurrIncomeQuantity != 0)
                {
                    currIncomePrice = decimal.Round(dtlConsume.CurrIncomeTotalPrice / dtlConsume.CurrIncomeQuantity, 4);
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(currIncomePrice);
                }
                else
                {
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 9).Text = "0";
                }
                if (dtlConsume.CurrResponsiConsumeQuantity != 0)
                {
                    currRespConsumePrice = decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice / dtlConsume.CurrResponsiConsumeQuantity, 4);
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(currRespConsumePrice);
                }
                else
                {
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 10).Text = "0";
                }
                if (dtlConsume.CurrRealConsumePlanQuantity != 0)
                {
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice / dtlConsume.CurrRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 11).Text = "0";
                }
                if (dtlConsume.CurrRealConsumeQuantity != 0)
                {
                    currRealConsumePrice = decimal.Round(dtlConsume.CurrRealConsumeTotalPrice / dtlConsume.CurrRealConsumeQuantity, 4);
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(currRealConsumePrice);
                }
                else
                {
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 12).Text = "0";
                }
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 13).Text = (currIncomePrice - currRealConsumePrice) + "";
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 14).Text = (currRespConsumePrice - currRealConsumePrice) + "";
                //合价对比
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(dtlConsume.CurrIncomeTotalPrice);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);

                //累计工程量对比
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(dtlConsume.SumIncomeQuantity);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeQuantity);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanQuantity);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeQuantity);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 25).Text = ClientUtil.ToString(dtlConsume.SumIncomeQuantity - dtlConsume.SumRealConsumeQuantity);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 26).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeQuantity - dtlConsume.SumRealConsumeQuantity);
                //累计单价对比
                decimal sumIncomePrice = 0;
                decimal sumRespConsumePrice = 0;
                decimal sumRealConsumePrice = 0;
                if (dtlConsume.SumIncomeQuantity != 0)
                {
                    sumIncomePrice = decimal.Round(dtlConsume.SumIncomeTotalPrice / dtlConsume.SumIncomeQuantity, 4);
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 27).Text = ClientUtil.ToString(sumIncomePrice);
                }
                else
                {
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 27).Text = "0";
                }
                if (dtlConsume.SumResponsiConsumeQuantity != 0)
                {
                    sumRespConsumePrice = decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice / dtlConsume.SumResponsiConsumeQuantity, 4);
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 28).Text = ClientUtil.ToString(sumRespConsumePrice);
                }
                else
                {
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 28).Text = "0";
                }
                if (dtlConsume.SumRealConsumePlanQuantity != 0)
                {
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 29).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice / dtlConsume.SumRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 29).Text = "0";
                }
                if (dtlConsume.SumRealConsumeQuantity != 0)
                {
                    sumRealConsumePrice = decimal.Round(dtlConsume.SumRealConsumeTotalPrice / dtlConsume.SumRealConsumeQuantity, 4);
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 30).Text = ClientUtil.ToString(currRealConsumePrice);
                }
                else
                {
                    fGridMaterialDiff.Cell(dtlStartRowNum + i, 30).Text = "0";
                }
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 31).Text = (sumIncomePrice - sumRealConsumePrice) + "";
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 32).Text = (sumRespConsumePrice - sumRealConsumePrice) + "";
                //累计合价对比
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 33).Text = ClientUtil.ToString(dtlConsume.SumIncomeTotalPrice);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 34).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 35).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 36).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 37).Text = ClientUtil.ToString(dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                fGridMaterialDiff.Cell(dtlStartRowNum + i, 38).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice); 
            }
            //写入合计值
            this.WriteSumGridData(fGridMaterialDiff, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 38);

        }

        #endregion

        #region 模板木枋材料费对比表
        private IList CreateModualData()
        {
            this.list_modual.Clear();
            //1: 取得科目集合
            IList list_modual = this.GetCostSubjectList("C5120802", 2, true);
            //补充工程量单位
            list_modual = this.AddUnitToCostMonthAccDtlConsume(list_modual);
            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_modual)
                    {
                        if (dtlConsume.CostSubjectCode != null && m_dtlConsume.CostSubjectCode == "C5120802")
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode) )
                        {
                            if (m_dtlConsume.Data2 == "2")//最底层
                            {
                                if (dtlConsume.RationUnitName == m_dtlConsume.RationUnitName)
                                {
                                    this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                                }
                            }
                            else
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                    }
                }
            }
            list_modual = this.DelZeroData(list_modual);
            //增加序号
            int level1 = 1;
            int levelOther = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_modual)
            {
                if (m_dtlConsume.Data2 == "1")
                {
                    m_dtlConsume.Data1 = CommonUtil.GetChineseNumber(level1);
                    levelOther = 1;
                    level1++;
                }
                if (m_dtlConsume.Data2 == "")
                {
                    m_dtlConsume.Data1 = "  " + levelOther + "";
                    levelOther++;
                }
            }
            return list_modual;
        }

        private void LoadModualData()
        {
            list_modual = CreateModualData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = this.list_modual.Count;

            //插入明细行
            this.fGridModual.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridModual.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridModual.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridModual.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridModual.Cell(3, 10).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_modual[i];

                fGridModual.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridModual.Cell(dtlStartRowNum + i, 1).Tag = dtlConsume.Data2;
                fGridModual.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                if (dtlConsume.Data2 == "2")
                {
                    fGridModual.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName + "(" + dtlConsume.RationUnitName + ")";
                }
                else
                {
                    fGridModual.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                }
                fGridModual.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //当前责任成本
                fGridModual.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeQuantity);
                if (dtlConsume.CurrResponsiConsumeQuantity != 0)
                {
                    fGridModual.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice / dtlConsume.CurrResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridModual.Cell(dtlStartRowNum + i, 4).Text = "0";
                }
                fGridModual.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                //当前计划成本
                fGridModual.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanQuantity);
                if (dtlConsume.CurrRealConsumePlanQuantity != 0)
                {
                    fGridModual.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice / dtlConsume.CurrRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridModual.Cell(dtlStartRowNum + i, 7).Text = "0";
                }
                fGridModual.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                //当前实际成本
                fGridModual.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeQuantity);
                if (dtlConsume.CurrRealConsumeQuantity != 0)
                {
                    fGridModual.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumeTotalPrice / dtlConsume.CurrRealConsumeQuantity, 4));
                }
                else
                {
                    fGridModual.Cell(dtlStartRowNum + i, 10).Text = "0";
                }
                fGridModual.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                //本期利润
                fGridModual.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridModual.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridModual.Cell(dtlStartRowNum + i, 13).Text = "0";
                }
                //累计责任成本
                fGridModual.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeQuantity);
                if (dtlConsume.SumResponsiConsumeQuantity != 0)
                {
                    fGridModual.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice / dtlConsume.SumResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridModual.Cell(dtlStartRowNum + i, 15).Text = "0";
                }
                fGridModual.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                //累计计划成本
                fGridModual.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanQuantity);
                if (dtlConsume.SumRealConsumePlanQuantity != 0)
                {
                    fGridModual.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice / dtlConsume.SumRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridModual.Cell(dtlStartRowNum + i, 18).Text = "0";
                }
                fGridModual.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                //累计实际成本
                fGridModual.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeQuantity);
                if (dtlConsume.SumRealConsumeQuantity != 0)
                {
                    fGridModual.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumeTotalPrice / dtlConsume.SumRealConsumeQuantity, 4));
                }
                else
                {
                    fGridModual.Cell(dtlStartRowNum + i, 21).Text = "0";
                }
                fGridModual.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
                //累计利润
                fGridModual.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridModual.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridModual.Cell(dtlStartRowNum + i, 24).Text = "0";
                }
                
            }
            //写入合计值
            this.WriteSumGridData(fGridModual, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 24);
            if (ClientUtil.ToDecimal(fGridModual.Cell(dtlStartRowNum + dtlCount, 5).Text) != 0)
            {
                fGridModual.Cell(dtlStartRowNum + dtlCount, 13).Text = decimal.Round(ClientUtil.ToDecimal(fGridModual.Cell(dtlStartRowNum + dtlCount, 12).Text) * 100 / ClientUtil.ToDecimal(fGridModual.Cell(dtlStartRowNum + dtlCount, 5).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridModual.Cell(dtlStartRowNum + dtlCount, 16).Text) != 0)
            {
                fGridModual.Cell(dtlStartRowNum + dtlCount, 24).Text = decimal.Round(ClientUtil.ToDecimal(fGridModual.Cell(dtlStartRowNum + dtlCount, 23).Text) * 100 / ClientUtil.ToDecimal(fGridModual.Cell(dtlStartRowNum + dtlCount, 16).Text), 2) + "";
            }
        }

        #endregion

        #region 周转料具费用对比表
        private IList CreateTurnExpData()
        {
            this.list_turnexp.Clear();
            
            //1: 取得科目集合
            IList list_turnexp = this.GetCostSubjectList("C5120803", 2, true);
            IList list_turnexp_1 = this.GetCostSubjectList("C51209", 2, true);
            IList list_turnexp_2 = this.GetCostSubjectList("C5120302", 2, true);
            foreach (CostMonthAccDtlConsume temp_consume in list_turnexp_1)
            {
                list_turnexp.Add(temp_consume);
            } foreach (CostMonthAccDtlConsume temp_consume in list_turnexp_2)
            {
                list_turnexp.Add(temp_consume);
            }
            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_turnexp)
                    {
                        if (dtlConsume.CostSubjectCode != null && (m_dtlConsume.CostSubjectCode == "C5120803" || m_dtlConsume.CostSubjectCode == "C51209" || m_dtlConsume.CostSubjectCode == "C5120302"))
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode))
                        {
                            this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            list_turnexp = this.DelZeroData(list_turnexp);
            //增加序号
            int level1 = 1;
            int level2 = 1;
            int levelOther = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_turnexp)
            {
                if (m_dtlConsume.Data2 == "1")
                {
                    m_dtlConsume.Data1 = CommonUtil.GetChineseNumber(level1);
                    level2 = 1;
                    level1++;
                }
                if (m_dtlConsume.Data2 == "2")
                {
                    m_dtlConsume.Data1 = "  " + level2;
                    level2++;
                }
                if (m_dtlConsume.Data2 == "")
                {
                    m_dtlConsume.Data1 = "  " + levelOther + "";
                    levelOther++;
                }
            }
            return list_turnexp;
        }
        private void LoadTurnExpData()
        {
            list_turnexp = CreateTurnExpData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = this.list_turnexp.Count;

            //插入明细行
            this.fGridTurnExp.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridTurnExp.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridTurnExp.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridTurnExp.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridTurnExp.Cell(3, 10).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_turnexp[i];

                fGridTurnExp.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridTurnExp.Cell(dtlStartRowNum + i, 1).Tag = dtlConsume.Data2;
                fGridTurnExp.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGridTurnExp.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                fGridTurnExp.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //当前责任成本
                fGridTurnExp.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeQuantity);
                if (dtlConsume.CurrResponsiConsumeQuantity != 0)
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice / dtlConsume.CurrResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 4).Text = "0";
                }
                fGridTurnExp.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                //当前计划成本
                fGridTurnExp.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanQuantity);
                if (dtlConsume.CurrRealConsumePlanQuantity != 0)
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice / dtlConsume.CurrRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 7).Text = "0";
                }
                fGridTurnExp.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                //当前实际成本
                fGridTurnExp.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeQuantity);
                if (dtlConsume.CurrRealConsumeQuantity != 0)
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumeTotalPrice / dtlConsume.CurrRealConsumeQuantity, 4));
                }
                else
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 10).Text = "0";
                }
                fGridTurnExp.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                //本期利润
                fGridTurnExp.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 13).Text = "0";
                }

                //累计责任成本
                fGridTurnExp.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeQuantity);
                if (dtlConsume.SumResponsiConsumeQuantity != 0)
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice / dtlConsume.SumResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 15).Text = "0";
                }
                fGridTurnExp.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                //累计计划成本
                fGridTurnExp.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanQuantity);
                if (dtlConsume.SumRealConsumePlanQuantity != 0)
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice / dtlConsume.SumRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 18).Text = "0";
                }
                fGridTurnExp.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                //累计实际成本
                fGridTurnExp.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeQuantity);
                if (dtlConsume.SumRealConsumeQuantity != 0)
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumeTotalPrice / dtlConsume.SumRealConsumeQuantity, 4));
                }
                else
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 21).Text = "0";
                }
                fGridTurnExp.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
                //累计利润
                fGridTurnExp.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridTurnExp.Cell(dtlStartRowNum + i, 24).Text = "0";
                }

            }
            //写入合计值
            this.WriteSumGridData(fGridTurnExp, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 24);
            if (ClientUtil.ToDecimal(fGridTurnExp.Cell(dtlStartRowNum + dtlCount, 5).Text) != 0)
            {
                fGridTurnExp.Cell(dtlStartRowNum + dtlCount, 13).Text = decimal.Round(ClientUtil.ToDecimal(fGridTurnExp.Cell(dtlStartRowNum + dtlCount, 12).Text) * 100 / ClientUtil.ToDecimal(fGridTurnExp.Cell(dtlStartRowNum + dtlCount, 5).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridTurnExp.Cell(dtlStartRowNum + dtlCount, 16).Text) != 0)
            {
                fGridTurnExp.Cell(dtlStartRowNum + dtlCount, 24).Text = decimal.Round(ClientUtil.ToDecimal(fGridTurnExp.Cell(dtlStartRowNum + dtlCount, 23).Text) * 100 / ClientUtil.ToDecimal(fGridTurnExp.Cell(dtlStartRowNum + dtlCount, 16).Text), 2) + "";
            }

        }

        #endregion

        #region 机械费用对比表
        private IList CreateMachineData()
        {
            string JXFSubZB = "C51103";//机械费
            if (ifZB == false)
            {
                JXFSubZB = "C56103";
            }
            this.list_machine.Clear();
            //1: 取得科目集合
            IList list_machine = this.GetCostSubjectList(JXFSubZB, 1, true);
            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_machine)
                    {
                        if (dtlConsume.CostSubjectCode != null && m_dtlConsume.CostSubjectCode == JXFSubZB)
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode))
                        {
                            this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            list_machine = this.DelZeroData(list_machine);
            //增加序号
            int level1 = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_machine)
            {
                m_dtlConsume.Data1 = level1 + "";
                level1++;
            }
            return list_machine;
        }

        private void LoadMachineData()
        {
            list_machine = CreateMachineData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = this.list_machine.Count;

            //插入明细行
            this.fGridMachine.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridMachine.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridMachine.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridMachine.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridMachine.Cell(3, 10).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_machine[i];

                fGridMachine.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridMachine.Cell(dtlStartRowNum + i, 1).Tag = "1";
                fGridMachine.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGridMachine.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                fGridMachine.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //当前责任成本
                fGridMachine.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeQuantity);
                if (dtlConsume.CurrResponsiConsumeQuantity != 0)
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice / dtlConsume.CurrResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 4).Text = "0";
                }
                fGridMachine.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                //当前计划成本
                fGridMachine.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanQuantity);
                if (dtlConsume.CurrRealConsumePlanQuantity != 0)
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice / dtlConsume.CurrRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 7).Text = "0";
                }
                fGridMachine.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                //当前实际成本
                fGridMachine.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeQuantity);
                if (dtlConsume.CurrRealConsumeQuantity != 0)
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumeTotalPrice / dtlConsume.CurrRealConsumeQuantity, 4));
                }
                else
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 10).Text = "0";
                }
                fGridMachine.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                //本期利润
                fGridMachine.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 13).Text = "0";
                }
                //累计责任成本
                fGridMachine.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeQuantity);
                if (dtlConsume.SumResponsiConsumeQuantity != 0)
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice / dtlConsume.SumResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 15).Text = "0";
                }
                fGridMachine.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                //累计计划成本
                fGridMachine.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanQuantity);
                if (dtlConsume.SumRealConsumePlanQuantity != 0)
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice / dtlConsume.SumRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 18).Text = "0";
                }
                fGridMachine.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                //累计实际成本
                fGridMachine.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeQuantity);
                if (dtlConsume.SumRealConsumeQuantity != 0)
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumeTotalPrice / dtlConsume.SumRealConsumeQuantity, 4));
                }
                else
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 21).Text = "0";
                }
                fGridMachine.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
                //累计利润
                fGridMachine.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridMachine.Cell(dtlStartRowNum + i, 24).Text = "0";
                }
            }
            //写入合计值
            this.WriteSumGridData(fGridMachine, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 24);
            if (ClientUtil.ToDecimal(fGridMachine.Cell(dtlStartRowNum + dtlCount, 5).Text) != 0)
            {
                fGridMachine.Cell(dtlStartRowNum + dtlCount, 13).Text = decimal.Round(ClientUtil.ToDecimal(fGridMachine.Cell(dtlStartRowNum + dtlCount, 12).Text) * 100 / ClientUtil.ToDecimal(fGridMachine.Cell(dtlStartRowNum + dtlCount, 5).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridMachine.Cell(dtlStartRowNum + dtlCount, 16).Text) != 0)
            {
                fGridMachine.Cell(dtlStartRowNum + dtlCount, 24).Text = decimal.Round(ClientUtil.ToDecimal(fGridMachine.Cell(dtlStartRowNum + dtlCount, 23).Text) * 100 / ClientUtil.ToDecimal(fGridMachine.Cell(dtlStartRowNum + dtlCount, 16).Text), 2) + "";
            }


        }

        #endregion

        #region 措施费对比表
        private IList CreateMeasureData()
        {
            string CSFSubZB = "C512";//措施费
            if (ifZB == false)
            {
                CSFSubZB = "C562";
            }
            this.list_measures.Clear();
            //1: 取得科目集合
            IList list_measures = this.GetCostSubjectList(CSFSubZB, 3, true);
            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_measures)
                    {
                        if (dtlConsume.CostSubjectCode != null && m_dtlConsume.CostSubjectCode == CSFSubZB)
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode))
                        {
                            this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            list_measures = this.DelZeroData(list_measures);
            //增加序号
            int level1 = 1;
            int level2 = 1;
            int level3 = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_measures)
            {
                if (m_dtlConsume.Data2 == "1")
                {
                    m_dtlConsume.Data1 = CommonUtil.GetChineseNumber(level1);
                    level2 = 1;
                    level1++;
                }
                if (m_dtlConsume.Data2 == "2")
                {
                    m_dtlConsume.Data1 = " (" + CommonUtil.GetChineseNumber(level2) + ")";
                    level3 = 1;
                    level2++;
                }
                if (m_dtlConsume.Data2 == "3")
                {
                    m_dtlConsume.Data1 = "  " + level3 + "";
                    level3++;
                }
            }
            return list_measures;
        }

        private void LoadMeasuresData()
        {
            list_measures = CreateMeasureData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = this.list_measures.Count;

            //插入明细行
            this.fGridMeasures.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridMeasures.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridMeasures.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridMeasures.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridMeasures.Cell(3, 7).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_measures[i];

                fGridMeasures.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridMeasures.Cell(dtlStartRowNum + i, 1).Tag = dtlConsume.Data2;
                fGridMeasures.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGridMeasures.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                fGridMeasures.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //本期数
                fGridMeasures.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                fGridMeasures.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                fGridMeasures.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                fGridMeasures.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridMeasures.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridMeasures.Cell(dtlStartRowNum + i, 7).Text = "0";
                }
                //累计数
                fGridMeasures.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                fGridMeasures.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                fGridMeasures.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
                fGridMeasures.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumResponsiConsumeTotalPrice != 0)
                {
                    fGridMeasures.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice)*100 / dtlConsume.SumResponsiConsumeTotalPrice, 4));
                }
                else {
                    fGridMeasures.Cell(dtlStartRowNum + i, 12).Text = "0";
                }

            }
            //写入合计值
            this.WriteSumGridData(fGridMeasures, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 12);
            if (ClientUtil.ToDecimal(fGridMeasures.Cell(dtlStartRowNum + dtlCount, 3).Text) != 0)
            {
                fGridMeasures.Cell(dtlStartRowNum + dtlCount, 7).Text = decimal.Round(ClientUtil.ToDecimal(fGridMeasures.Cell(dtlStartRowNum + dtlCount, 6).Text) * 100 / ClientUtil.ToDecimal(fGridMeasures.Cell(dtlStartRowNum + dtlCount, 3).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridMeasures.Cell(dtlStartRowNum + dtlCount, 8).Text) != 0)
            {
                fGridMeasures.Cell(dtlStartRowNum + dtlCount, 12).Text = decimal.Round(ClientUtil.ToDecimal(fGridMeasures.Cell(dtlStartRowNum + dtlCount, 11).Text) * 100 / ClientUtil.ToDecimal(fGridMeasures.Cell(dtlStartRowNum + dtlCount, 8).Text), 2) + "";
            }
        }

        #endregion

        #region 现场管理费对比表
        private IList CreateManagerData()
        {
            string XCGLFSubZB = "C513";//现场管理费
            if (ifZB == false)
            {
                XCGLFSubZB = "C563";
            }
            this.list_manager.Clear();

            //1: 取得科目集合
            IList list_manager = this.GetCostSubjectList(XCGLFSubZB, 1, true);
            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_manager)
                    {
                        if (dtlConsume.CostSubjectCode != null && m_dtlConsume.CostSubjectCode == XCGLFSubZB)
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode))
                        {
                            this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            list_manager = this.DelZeroData(list_manager);
            //增加序号
            int level1 = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_manager)
            {
                //if (m_dtlConsume.Data2 == "1")
                //{
                //    m_dtlConsume.Data1 = level1 + "";
                //    level1++;
                //}
                m_dtlConsume.Data1 = level1 + "";
                level1++;
            }
            return list_manager;
        }

        private void LoadManagerData()
        {
            list_manager = CreateManagerData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = this.list_manager.Count;

            //插入明细行
            this.fGridManager.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridManager.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridManager.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridManager.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridManager.Cell(3, 7).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_manager[i];

                fGridManager.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridManager.Cell(dtlStartRowNum + i, 1).Tag = "1";
                fGridManager.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGridManager.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                fGridManager.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //本期数
                fGridManager.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                fGridManager.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                fGridManager.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                fGridManager.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridManager.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridManager.Cell(dtlStartRowNum + i, 7).Text = "0";
                }
                //累计数
                fGridManager.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                fGridManager.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                fGridManager.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
                fGridManager.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumResponsiConsumeTotalPrice != 0)
                {
                    fGridManager.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice)*100 / dtlConsume.SumResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridManager.Cell(dtlStartRowNum + i, 12).Text = "0";
                }

            }
            //写入合计值
            this.WriteSumGridData(fGridManager, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 12);
            if (ClientUtil.ToDecimal(fGridManager.Cell(dtlStartRowNum + dtlCount, 3).Text) != 0)
            {
                fGridManager.Cell(dtlStartRowNum + dtlCount, 7).Text = decimal.Round(ClientUtil.ToDecimal(fGridManager.Cell(dtlStartRowNum + dtlCount, 6).Text) * 100 / ClientUtil.ToDecimal(fGridManager.Cell(dtlStartRowNum + dtlCount, 3).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridManager.Cell(dtlStartRowNum + dtlCount, 8).Text) != 0)
            {
                fGridManager.Cell(dtlStartRowNum + dtlCount, 12).Text = decimal.Round(ClientUtil.ToDecimal(fGridManager.Cell(dtlStartRowNum + dtlCount, 11).Text) * 100 / ClientUtil.ToDecimal(fGridManager.Cell(dtlStartRowNum + dtlCount, 8).Text), 2) + "";
            }
        }

        #endregion

        #region 规费对比表
        private IList CreateFeesData()
        {
            string GFSubZB = "C514";//规费
            if (ifZB == false)
            {
                GFSubZB = "C564";
            }
            this.list_fees.Clear();
            //1: 取得科目集合
            IList list_fees = this.GetCostSubjectList(GFSubZB, 2, true);
            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_fees)
                    {
                        if (dtlConsume.CostSubjectCode != null && m_dtlConsume.CostSubjectCode == GFSubZB)
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode))
                        {
                            this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            list_fees = this.DelZeroData(list_fees);
            //增加序号
            int level1 = 1;
            int level2 = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_fees)
            {
                if (m_dtlConsume.Data2 == "1")
                {
                    m_dtlConsume.Data1 = CommonUtil.GetChineseNumber(level1);
                    level2 = 1;
                    level1++;
                }
                if (m_dtlConsume.Data2 == "2")
                {
                    m_dtlConsume.Data1 = "  " + level2;
                    level2++;
                }
            }
            return list_fees;
        }

        private void LoadFeesData()
        {
            list_fees = CreateFeesData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = this.list_fees.Count;

            //插入明细行
            this.fGridFees.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridFees.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridFees.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridFees.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridFees.Cell(3, 7).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_fees[i];

                fGridFees.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridFees.Cell(dtlStartRowNum + i, 1).Tag = "1";
                fGridFees.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGridFees.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                fGridFees.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //本期数
                fGridFees.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                fGridFees.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                fGridFees.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                fGridFees.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridFees.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridFees.Cell(dtlStartRowNum + i, 7).Text = "0";
                }
                //累计数
                fGridFees.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                fGridFees.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                fGridFees.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
                fGridFees.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumResponsiConsumeTotalPrice != 0)
                {
                    fGridFees.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice)*100 / dtlConsume.SumResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridFees.Cell(dtlStartRowNum + i, 12).Text = "0";
                }

            }
            //写入合计值
            this.WriteSumGridData(fGridFees, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 12);
            if (ClientUtil.ToDecimal(fGridFees.Cell(dtlStartRowNum + dtlCount, 3).Text) != 0)
            {
                fGridFees.Cell(dtlStartRowNum + dtlCount, 7).Text = decimal.Round(ClientUtil.ToDecimal(fGridFees.Cell(dtlStartRowNum + dtlCount, 6).Text) * 100 / ClientUtil.ToDecimal(fGridFees.Cell(dtlStartRowNum + dtlCount, 3).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridFees.Cell(dtlStartRowNum + dtlCount, 8).Text) != 0)
            {
                fGridFees.Cell(dtlStartRowNum + dtlCount, 12).Text = decimal.Round(ClientUtil.ToDecimal(fGridFees.Cell(dtlStartRowNum + dtlCount, 11).Text) * 100 / ClientUtil.ToDecimal(fGridFees.Cell(dtlStartRowNum + dtlCount, 8).Text), 2) + "";
            }
        }

        #endregion

        #region 专业分包费对比表
        private IList CreateSpecialSubData()
        {
            string ZYFBFSubZB = "C51104";//专业分包费
            if (ifZB == false)
            {
                ZYFBFSubZB = "C56104";
            }
            this.list_specialsub.Clear();
            //1: 取得科目集合
            IList list_specialsub = this.GetCostSubjectList(ZYFBFSubZB, 2, true);
            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_specialsub)
                    {
                        if (dtlConsume.CostSubjectCode != null && m_dtlConsume.CostSubjectCode == ZYFBFSubZB)
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode))
                        {
                            this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            list_specialsub = this.DelZeroData(list_specialsub);
            //增加序号
            int level1 = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_specialsub)
            {
                m_dtlConsume.Data1 = level1 + "";
                level1++;
            }
            return list_specialsub;
        }

        private void LoadSpecialSubData()
        {
            list_specialsub = CreateSpecialSubData();
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = this.list_specialsub.Count;

            //插入明细行
            this.fGridSpecSub.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridSpecSub.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridSpecSub.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridSpecSub.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridSpecSub.Cell(3, 10).Text = costBill.Kjn + "年" + costBill.Kjy + "月"; 

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_specialsub[i];

                fGridSpecSub.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridSpecSub.Cell(dtlStartRowNum + i, 1).Tag = "1";
                fGridSpecSub.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGridSpecSub.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                fGridSpecSub.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //当前合同收入
                fGridSpecSub.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrIncomeQuantity);
                if (dtlConsume.CurrIncomeQuantity != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrIncomeTotalPrice / dtlConsume.CurrIncomeQuantity, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 4).Text = "0";
                }
                fGridSpecSub.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.CurrIncomeTotalPrice);
                //当前责任成本
                fGridSpecSub.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeQuantity);
                if (dtlConsume.CurrResponsiConsumeQuantity != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice / dtlConsume.CurrResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 7).Text = "0";
                }
                fGridSpecSub.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice);
                //当前计划成本
                fGridSpecSub.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanQuantity);
                if (dtlConsume.CurrRealConsumePlanQuantity != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice / dtlConsume.CurrRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 10).Text = "0";
                }
                fGridSpecSub.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumePlanTotalPrice);
                //当前实际成本
                fGridSpecSub.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeQuantity);
                if (dtlConsume.CurrRealConsumeQuantity != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumeTotalPrice / dtlConsume.CurrRealConsumeQuantity, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 13).Text = "0";
                }
                fGridSpecSub.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                //本期利润
                fGridSpecSub.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrIncomeTotalPrice != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrIncomeTotalPrice, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 16).Text = "0";
                }
                fGridSpecSub.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice);
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice)*100 / dtlConsume.CurrResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 18).Text = "0";
                }

                //累计合同收入
                fGridSpecSub.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(dtlConsume.SumIncomeQuantity);
                if (dtlConsume.SumIncomeQuantity != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumIncomeTotalPrice / dtlConsume.SumIncomeQuantity, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 20).Text = "0";
                }
                fGridSpecSub.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(dtlConsume.SumIncomeTotalPrice);
                //累计责任成本
                fGridSpecSub.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeQuantity);
                if (dtlConsume.SumResponsiConsumeQuantity != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice / dtlConsume.SumResponsiConsumeQuantity, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 23).Text = "0";
                }
                fGridSpecSub.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice);
                //累计计划成本
                fGridSpecSub.Cell(dtlStartRowNum + i, 25).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanQuantity);
                if (dtlConsume.SumRealConsumePlanQuantity != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 26).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice / dtlConsume.SumRealConsumePlanQuantity, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 26).Text = "0";
                }
                fGridSpecSub.Cell(dtlStartRowNum + i, 27).Text = ClientUtil.ToString(dtlConsume.SumRealConsumePlanTotalPrice);
                //累计实际成本
                fGridSpecSub.Cell(dtlStartRowNum + i, 28).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeQuantity);
                if (dtlConsume.SumRealConsumeQuantity != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 29).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumeTotalPrice / dtlConsume.SumRealConsumeQuantity, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 29).Text = "0";
                }
                fGridSpecSub.Cell(dtlStartRowNum + i, 30).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
                //累计利润
                fGridSpecSub.Cell(dtlStartRowNum + i, 31).Text = ClientUtil.ToString(dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumIncomeTotalPrice != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 32).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice)*100 / dtlConsume.SumIncomeTotalPrice, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 32).Text = "0";
                }
                fGridSpecSub.Cell(dtlStartRowNum + i, 33).Text = ClientUtil.ToString(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice);
                if (dtlConsume.SumResponsiConsumeTotalPrice != 0)
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 34).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice)*100 / dtlConsume.SumResponsiConsumeTotalPrice, 4));
                }
                else
                {
                    fGridSpecSub.Cell(dtlStartRowNum + i, 34).Text = "0";
                }
            }
            //写入合计值
            this.WriteSumGridData(fGridSpecSub, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 34);
            if (ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 5).Text) != 0)
            {
                fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 16).Text = decimal.Round(ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 15).Text) * 100 / ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 5).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 8).Text) != 0)
            {
                fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 18).Text = decimal.Round(ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 17).Text) * 100 / ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 8).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 21).Text) != 0)
            {
                fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 32).Text = decimal.Round(ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 31).Text) * 100 / ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 21).Text), 2) + "";
            }
            if (ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 24).Text) != 0)
            {
                fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 34).Text = decimal.Round(ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 33).Text) * 100 / ClientUtil.ToDecimal(fGridSpecSub.Cell(dtlStartRowNum + dtlCount, 24).Text), 2) + "";
            }
            this.fGridSpecSub.Column(2).AutoFit();
        }

        #endregion

        #region 税金统计表
        private IList CreateTaxData()
        {
            string SJFSubZB = "C515";//税金
            if (ifZB == false)
            {
                SJFSubZB = "C565";
            }
            this.list_tax.Clear();
            //1: 取得科目集合
            IList list_tax = this.GetCostSubjectList(SJFSubZB, 2, true);
            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_tax)
                    {
                        if (dtlConsume.CostSubjectCode != null && m_dtlConsume.CostSubjectCode == SJFSubZB)
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode))
                        {
                            this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            list_tax = this.DelZeroData(list_tax);
            //增加序号
            int level1 = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_tax)
            {
                m_dtlConsume.Data1 = level1 + "";
                level1++;
            }
            return list_tax;
        }

        private void LoadTaxData()
        {
            list_tax = CreateTaxData();
            int dtlStartRowNum = 6;//模板中的行号
            int dtlCount = this.list_tax.Count;

            //插入明细行
            this.fGridTax.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridTax.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridTax.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridTax.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridTax.Cell(3, 4).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_tax[i];

                fGridTax.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridTax.Cell(dtlStartRowNum + i, 1).Tag = "1";
                fGridTax.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGridTax.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                fGridTax.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //本期数
                fGridTax.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                fGridTax.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);

            }
            //写入合计值
            this.WriteSumGridData(fGridTax, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 4);
        }

        #endregion

        #region 其它成本统计表
        private IList CreateOthersData()
        {
            this.list_others.Clear();
            //1: 取得科目集合
            IList list_others = this.GetCostSubjectList("C516", 2, true);
            //结果累加
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_others)
                    {
                        if (dtlConsume.CostSubjectCode != null && m_dtlConsume.CostSubjectCode == "C516")
                        {
                            if (dtlConsume.CostSubjectCode.Equals(m_dtlConsume.CostSubjectCode))
                            {
                                this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                            }
                        }
                        else if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(m_dtlConsume.CostSubjectCode))
                        {
                            this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            list_others = this.DelZeroData(list_others);
            //增加序号
            int level1 = 1;
            foreach (CostMonthAccDtlConsume m_dtlConsume in list_others)
            {
                m_dtlConsume.Data1 = level1 + "";
                level1++;
            }
            return list_others;
        }

        private void LoadOthersData()
        {
            list_others = CreateOthersData();
            int dtlStartRowNum = 6;//模板中的行号
            int dtlCount = this.list_others.Count;

            //插入明细行
            this.fGridOthers.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridOthers.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridOthers.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridOthers.Cell(3, 2).Text = costBill.ProjectName;
            this.fGridOthers.Cell(3, 4).Text = costBill.Kjn + "年" + costBill.Kjy + "月";

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_others[i];

                fGridOthers.Cell(dtlStartRowNum + i, 1).Text = dtlConsume.Data1;
                fGridOthers.Cell(dtlStartRowNum + i, 1).Tag = "1";
                fGridOthers.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGridOthers.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.CostingSubjectName;
                fGridOthers.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //本期数
                fGridOthers.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(dtlConsume.CurrRealConsumeTotalPrice);
                fGridOthers.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(dtlConsume.SumRealConsumeTotalPrice);
            }
            //写入合计值
            this.WriteSumGridData(fGridOthers, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 4);
        }

        #endregion

    }
}