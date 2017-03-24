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
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.Report
{
    public partial class VAllProjectStateReport : TBasicDataView
    {
        MProjectDepartment model = new MProjectDepartment();

        string orgSyscode = "";
        private CurrentProjectInfo ProjectInfo;
        string detailExptr = "公司项目运行状态一览表";
        IList list = new ArrayList();
        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();

        public VAllProjectStateReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-30);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;

            ProjectInfo = StaticMethod.GetProjectInfo();
            if (ProjectInfo != null && ProjectInfo.Code != CommonUtil.CompanyProjectCode)
            {

                this.lblPSelect.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
            }

            this.fGridDetail.Rows = 1;
        }

        private void InitEvents()
        {
            btnQuery.Click+=new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            this.fGridDetail.HyperLinkClick += new FlexCell.Grid.HyperLinkClickEventHandler(btnHyperLinkClick);
        }


        void btnHyperLinkClick(object sender, FlexCell.Grid.HyperLinkClickEventArgs e)
        {
            int currRow = e.Row;
            int currCol = e.Col;
            e.URL = "";
            e.Changed = true;
            string projectID = fGridDetail.Cell(currRow, currCol).Tag as string;
            string projectName = fGridDetail.Cell(currRow, currCol).Text;
            VProjectStateReport vReport = new VProjectStateReport(true, projectID, projectName, startDate, endDate);
            vReport.Width = 1150;
            vReport.Height = 620;
            vReport.ShowDialog();
        }
        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            string opgId = "";
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect();
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
                opgId = info.Id;
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel(detailExptr, false, false, true);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            LoadTempleteFile(detailExptr + ".flx");

            //载入数据
            this.LoadDetailFile();

            //设置外观
            fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == "公司项目运行状态一览表.flx")
                {                   
                    fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
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

            for (int i = startCol; i <= endCol; i++)
            {
                string oldValue = "0";
                for (int t = startRow; t <= endRow; t++)
                {
                    string currValue = flexGrid.Cell(t, i).Text;
                    if (oldValue == "0")
                    {
                        oldValue = currValue;
                    }
                    else
                    {
                        if (oldValue == currValue)
                        {
                            flexGrid.Range(t - 1, i, t, i).Merge();
                        }
                        oldValue = currValue;
                    }
                }
            }
        }

        #endregion

        #region 项目使用状态明细表
       
        private void LoadDetailFile()
        {
            OperationOrgInfo orgInfo = this.txtOperationOrg.Tag as OperationOrgInfo;
            FlashScreen.Show("正在生成[项目使用状态]报告...");

            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            try
            {
                startDate = dtpDateBegin.Value.Date;
                endDate = dtpDateEnd.Value.Date;
                if (info == null)
                {
                    list = model.CurrentSrv.QueryAllProjectStateInfo("", dtpDateBegin.Value.Date, dtpDateEnd.Value.Date);
                }
                else
                {
                    list = model.CurrentSrv.QueryAllProjectStateInfo(info.Id, dtpDateBegin.Value.Date, dtpDateEnd.Value.Date);
                }
                LoadTotalFile();
            }
            catch (Exception e1)
            {
                throw new Exception("生成[项目使用状态]报告异常[" + e1.Message + "]");
            }
            finally
            {
                FlashScreen.Close();
            }
            
        }

        #endregion

        #region 项目使用状态综合表

        private void LoadTotalFile()
        {
            fGridDetail.AutoRedraw = false;
            int dtlStartRowNum = 6;//模板中的行号
            int dtlCount = list.Count;

            //插入明细行
            this.fGridDetail.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridDetail.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridDetail.Cols - 1);
            CommonUtil.SetFlexGridDetailCenter(range);

            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            if (info == null || ClientUtil.ToString(info.Id) == "")
            {
                this.fGridDetail.Cell(1, 1).Text = dtpDateBegin.Value.Date.ToShortDateString() + "到" + dtpDateEnd.Value.Date.ToShortDateString() + " [公司]项目使用状态统计报告";
            }
            else
            {
                this.fGridDetail.Cell(1, 1).Text = dtpDateBegin.Value.Date.ToShortDateString() + "到" + dtpDateEnd.Value.Date.ToShortDateString() + " [" + info.Name + "]项目使用状态统计报告";
            }
            
            int i = 0;
            IList subList = new ArrayList();
            IList yearList = new ArrayList();
            foreach (DataDomain domain in list)
            {
                fGridDetail.Cell(dtlStartRowNum + i, 1).Text = ClientUtil.ToString(domain.Name4);//分公司
                fGridDetail.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(domain.Name3);//类别
                fGridDetail.Cell(dtlStartRowNum + i, 5).Tag = ClientUtil.ToString(domain.Name3);//类别
                fGridDetail.Cell(dtlStartRowNum + i, 3).Text = "";//
                fGridDetail.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name2);//项目名称
                fGridDetail.Cell(dtlStartRowNum + i, 4).Tag = ClientUtil.ToString(domain.Name1);
                fGridDetail.Cell(dtlStartRowNum + i, 4).CellType = FlexCell.CellTypeEnum.HyperLink;
                fGridDetail.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name30);//是否达标
                if (ClientUtil.ToString(domain.Name30) == "×")
                {
                    fGridDetail.Cell(dtlStartRowNum + i, 5).ForeColor = System.Drawing.Color.Red;
                }
                bool ifHaveSub = false;
                foreach (DataDomain data in subList)
                {
                    if (ClientUtil.ToString(domain.Name4) == ClientUtil.ToString(data.Name4))
                    {
                        if (ClientUtil.ToString(domain.Name30) == "×")
                        {
                            data.Name2 = ClientUtil.ToDecimal(data.Name2) + 1 + "";//不合格数量
                        }
                        else
                        {
                            data.Name1 = ClientUtil.ToDecimal(data.Name1) + 1 + "";//合格数量
                        }
                        ifHaveSub = true;
                    }
                }
                if (ifHaveSub == false)
                {
                    DataDomain sub = new DataDomain();
                    sub.Name4 = ClientUtil.ToString(domain.Name4);
                    if (ClientUtil.ToString(domain.Name30) == "×")
                    {
                        sub.Name1 = "0";//合格数量
                        sub.Name2 = "1";//不合格数量
                    }
                    else
                    {
                        sub.Name1 = "1";
                        sub.Name2 = "0";
                    }
                    subList.Add(sub);
                }

                bool ifHaveYear = false;
                foreach (DataDomain data in yearList)
                {
                    if (ClientUtil.ToString(domain.Name3) == ClientUtil.ToString(data.Name3))
                    {
                        if (ClientUtil.ToString(domain.Name30) == "×")
                        {
                            data.Name2 = ClientUtil.ToDecimal(data.Name2) + 1 + "";//不合格数量
                        }
                        else
                        {
                            data.Name1 = ClientUtil.ToDecimal(data.Name1) + 1 + "";//合格数量
                        }
                        ifHaveYear = true;
                    }
                }
                if (ifHaveYear == false)
                {
                    DataDomain sub = new DataDomain();
                    sub.Name3 = ClientUtil.ToString(domain.Name3);
                    if (ClientUtil.ToString(domain.Name30) == "×")
                    {
                        sub.Name1 = "0";//合格数量
                        sub.Name2 = "1";//不合格数量
                    }
                    else
                    {
                        sub.Name1 = "1";
                        sub.Name2 = "0";
                    }
                    yearList.Add(sub);
                }
                
                fGridDetail.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name5);//人员岗位
                fGridDetail.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name6);//工程信息设置
                fGridDetail.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name7);//工程部位合理划分
                fGridDetail.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(domain.Name8);//施工任务合理划分
                fGridDetail.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(domain.Name9);//成本预算信息设置
                fGridDetail.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(domain.Name10);//工长提报工程量
                fGridDetail.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(domain.Name11);//商务工单复核
                fGridDetail.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(domain.Name12);//物资收/领/验等
                fGridDetail.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(domain.Name13);//分包结算单
                fGridDetail.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(domain.Name14);//质量/安全/检查整改
                fGridDetail.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(domain.Name15);//财务费用结算
                fGridDetail.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(domain.Name16);//设备租赁结算
                fGridDetail.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(domain.Name17);//物资实际耗用结算
                fGridDetail.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(domain.Name18);//月度成本分析生成
                i++;
            }
            string str = " 统计项目一共[" + list.Count + "]个，其中";
            foreach (DataDomain domain in subList)
            {
                int okProject = ClientUtil.ToInt(domain.Name1);
                int noProject = ClientUtil.ToInt(domain.Name2);
                str += " [" + ClientUtil.ToString(domain.Name4) + "共" + (okProject + noProject) + "个，合格项目: " + okProject + "个，不合格项目: " + noProject + "] ";
            }
            str += "\r\n 按年份统计，其中";
            foreach (DataDomain domain in yearList)
            {
                int okProject = ClientUtil.ToInt(domain.Name1);
                int noProject = ClientUtil.ToInt(domain.Name2);
                str += " [" + ClientUtil.ToString(domain.Name3) + "共" + (okProject + noProject) + "个，合格项目: " + okProject + "个，不合格项目: " + noProject + "] ";
            }
            this.fGridDetail.Cell(2, 1).Text = str;
            this.fGridDetail.Row(2).AutoFit();

            //格式调整
            for (int t = dtlStartRowNum; t <= dtlStartRowNum + dtlCount; t++)
            {
                fGridDetail.Row(t).AutoFit();
            }
            fGridDetail.Column(4).AutoFit();
            WriteSumGridData(fGridDetail, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 1, 2);
            //补入序号
            string oldValue = "0";
            int tt = 1;
            for (int t = dtlStartRowNum; t <= dtlStartRowNum + dtlCount - 1; t++)
            {
                string currValue = fGridDetail.Cell(t, 5).Tag as string;
                if (oldValue == "0")
                {
                    oldValue = currValue;
                }
                else
                {
                    if (oldValue == currValue)
                    {
                        tt++;
                    }
                    else
                    {
                        tt = 1;
                    }
                    oldValue = currValue;
                }
                fGridDetail.Cell(t, 3).Text = tt + "";
            }

            fGridDetail.AutoRedraw = true;
            fGridDetail.Refresh();

        }
        #endregion
    }
}