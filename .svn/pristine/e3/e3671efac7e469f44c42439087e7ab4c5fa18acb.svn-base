using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng
{
    public partial class VCompanySupplyQuery : TBasicDataView
    {
        private MDemandMasterPlanMng model = new MDemandMasterPlanMng();

        public VCompanySupplyQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.txtMaterialCategory.rootCatCode = "01";
            this.txtMaterialCategory.rootLevel = "5";
            txtMaterialCategory.IsCheckBox = true;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddMonths(-1);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            InitProject();
        }

        private void InitProject()
        {
            this.listBoxType.HorizontalScrollbar = true;
            IList list = model.DemandPlanSrv.QuerySupplyProjectInfo();
            try
            {
                listBoxType.DataSource = list;
                listBoxType.DisplayMember = "Name";
                listBoxType.SelectedItems.Clear();
            }
            catch (Exception e)
            {
            }
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            reportGrid.ExportToExcel("采购成本统计表", true, false, true);
        }

        private bool LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                reportGrid.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTempleteFile(@"采购成本统计表.flx");

            #region 查询条件处理
            string condition = "";
            //项目
            if (listBoxType.SelectedItems != null && listBoxType.SelectedItems.Count > 0)
            {
                condition += " and (";
                for (int i = 0; i < listBoxType.SelectedItems.Count; i++)
                {
                    CurrentProjectInfo pi = listBoxType.SelectedItems[i] as CurrentProjectInfo;
                    if (i == 0)
                    {
                        condition += "t1.ProjectId='" + pi.Id + "'";
                    }
                    else
                    {
                        condition += " or t1.ProjectId='" + pi.Id + "'";
                    }
                }
                condition += ")";
            }
            if (condition == "")
            {
                MessageBox.Show("请选择项目！");
                return;
            }
            //物资
            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
            }

            //物资分类
            string matName = "";
            if (txtMaterialCategory.Text != "" && txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
            {
                IList lstMaterialCategory = txtMaterialCategory.Result;
                string sExpCode = string.Empty;
                foreach (MaterialCategory oMaterialCategory in lstMaterialCategory)
                {
                    if (matName == "")
                    {
                        matName += oMaterialCategory.Name;
                    }
                    else {
                        matName += "-" + oMaterialCategory.Name;
                    }
                    if (string.IsNullOrEmpty(sExpCode))
                    {
                        sExpCode = "^" + oMaterialCategory.Code;
                    }
                    else
                    {
                        sExpCode += "|^" + oMaterialCategory.Code;
                    }
                }
                if (!string.IsNullOrEmpty(sExpCode))
                {
                    sExpCode = string.Format(" and  regexp_like(t2.materialcode,'{0}')  ", sExpCode);
                }
                condition += sExpCode;
                //MaterialCategory mc = txtMaterialCategory.Result[0] as MaterialCategory; 
                //condition += " and t2.materialcode like '" + mc.Code + "%'";
            }
            #endregion
            IList list = model.DemandPlanSrv.QuerySupplyCostInfo(condition, dtpDateBegin.Value.Date.ToShortDateString(), dtpDateEnd.Value.Date.AddDays(1).ToShortDateString());

            this.reportGrid.Cell(2, 1).Text = "材料类别: " + matName;
            this.reportGrid.Cell(2, 4).Text = "统计日期: " + dtpDateBegin.Value.Date.ToShortDateString() + "到" + dtpDateEnd.Value.Date.ToShortDateString();
            int dtlStartRowNum = 5;//5为模板中的行号
           
            //设置单元格的边框，对齐方式
            int dtlCount = list.Count;
            //CommonUtil.SetFlexGridAutoFit(reportGrid);

            reportGrid.InsertRow(dtlStartRowNum, list.Count);
            reportGrid.FixedRows = 5;
            //设置单元格的边框，对齐方式
            FlexCell.Range range = reportGrid.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, reportGrid.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            foreach (DataDomain domain in list)
            {
                reportGrid.Cell(dtlStartRowNum, 1).Text = ClientUtil.ToString(domain.Name4);
                reportGrid.Cell(dtlStartRowNum, 2).Text = ClientUtil.ToString(domain.Name5);
                reportGrid.Cell(dtlStartRowNum, 3).Text = ClientUtil.ToString(domain.Name7);//本期数量
                reportGrid.Cell(dtlStartRowNum, 4).Text = ClientUtil.ToString(domain.Name9);//本期金额
                reportGrid.Cell(dtlStartRowNum, 5).Text = ClientUtil.ToString(domain.Name8);//累计数量
                reportGrid.Cell(dtlStartRowNum, 6).Text = ClientUtil.ToString(domain.Name10);//累计金额
                reportGrid.Cell(dtlStartRowNum, 7).Text = ClientUtil.ToString(domain.Name6);
                dtlStartRowNum += 1;              
            }
            CommonUtil.SetFlexGridFace(this.reportGrid);
        }
    }
}
