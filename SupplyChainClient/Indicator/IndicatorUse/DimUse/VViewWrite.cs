using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using NHibernate.Criterion;
using NHibernate;

using VirtualMachine.Core;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.BasicData;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Business.Erp.SupplyChain.Client.Indicator.BasicData;


namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VViewWrite : TBasicDataView
    {
        private MCubeManager mCube = new MCubeManager();
        private MBasicData bdModel = new MBasicData();
        private Hashtable ht_dim = new Hashtable();
        private Hashtable ht_cd = new Hashtable();
        private string kjn;
        private string kjy;
        private bool ifScore = false;
        private bool ifJJZB = false;
        private bool ifSonMother = false;

        public VViewWrite(bool ifSaveScore)
        {
            InitializeComponent();
            ifScore = ifSaveScore;
        }

        internal void Start()
        {
            kjn = ConstObject.LoginDate.Year + "";
            kjy = ConstObject.LoginDate.Month + "";
            if (int.Parse(kjy) < 10)
            {
                kjy = "0" + kjy;
            }

            LoadUserViewList();
            InitialControls();
            InitDimension();          
        }

        /// <summary>
        /// 设置控件的一些属性
        /// </summary>
        private void InitialControls()
        {
            btnExcel.Enabled = false;
            btnPreview.Enabled = false;
            btnSubmit.Enabled = false;
            btnSave.Enabled = false;
            btnLoad.Enabled = false;
        }

        /// <summary>
        /// 激活控件的一些属性
        /// </summary>
        private void ActiveControls()
        {
            btnExcel.Enabled = true;
            btnPreview.Enabled = true;
            btnSubmit.Enabled = true;
            btnSave.Enabled = true;
            btnLoad.Enabled = true;
        }

        private void InitDimension() {           
            IList list = mCube.DimManagerSrv.GetDimensionCategorys();
            foreach (DimensionCategory dc in list)
            {
                ht_dim.Add(dc.Id + "", dc);
            }
        }

        //视图中数据的清除
        private void ClearView() {
            ht_cd.Clear();
            ClearFlexCell();
            //dgvCubeData.Columns.Clear();
        }

        //FlexCell控件清除
        private void ClearFlexCell()
        {
            dgvCubeData.Rows = 1;
            dgvCubeData.Cols = 6;
            for (int i = 1; i < 6; i++)
            {
                dgvCubeData.Cell(0, i).Text = "";
                dgvCubeData.Column(i).Locked = false;
            }
        }

        // <summary>
        // 通过登录人的岗位查询视图列表
        // </summary>
        private void LoadUserViewList()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheJob.Id", ConstObject.TheSysRole.Id));
            oq.AddCriterion(Expression.Eq("Main.ViewTypeCode", "1"));
            oq.AddCriterion(Expression.Lt("StateName", int.Parse(kjn + kjy)));
            oq.AddFetchMode("Main", FetchMode.Eager);
            IList list = mCube.ViewService.GetViewDistributeByQuery(oq);
            lstViewSel.DisplayMember = "ViewName";
            foreach (ViewDistribute vd in list)
            {
                lstViewSel.Items.Add(vd);
            }
        }

        private void lstViewSel_MouseClick(object sender, MouseEventArgs e)
        {
            mCube.Clear();
            ViewDistribute vd = lstViewSel.SelectedItem as ViewDistribute;
            if (vd.Main.CubeRegId.Id == "1")
            {
                ifJJZB = true;
            }
            if ("是".Equals(vd.Main.IfDisplaySonMother))
            {
                ifSonMother = true;
            }
            if (vd == null)
            {
                KnowledgeMessageBox.InforMessage("请选择一个模板！");
                return;
            }
            //初始化
            ActiveControls();
            ClearView();

            edtDistributeDate.Text = vd.DistributeDate.ToShortDateString();
            edtSerial.Text = vd.DistributeSerial + "";

            mCube.HtDimension = ht_dim;
            int returnCode = 0;
            if (vd.Main.CubeRegId.Id == "1")
            {
                returnCode = mCube.DisplayCustomFlexCellByJJZB(dgvCubeData, vd.Main, false, true, "");
            }
            else
            {
                returnCode = mCube.DisplayCustomFlexCell(dgvCubeData, vd.Main, false, true, "");
            }
            if (returnCode == -1) {
                KnowledgeMessageBox.InforMessage("此模板中本月未定义数据！");
                return;
            }
            /*          
            //锁定维度列
            for (int k = 0; k < dgvCubeData.Cols - 1; k++)
            {
                dgvCubeData.Column(k).Locked = true;
            }
            */
        }

        private void SaveGridData(FlexCell.Grid grid, Hashtable ht_cd, CubeRegister cr, ViewDistribute vd,int startRow, int endRow, int startCol, int endCol)
        {
            if (vd.Main.CubeRegId.Id == "1")
            {
                for (int row = startRow; row <= endRow; row++)
                {
                    for (int col = startCol; col <= endCol; col=col+3)
                    {
                        String id = grid.Cell(row, col).Tag;
                        CubeData cd = (CubeData)ht_cd[id];
                        string value = grid.Cell(row, col).Text;
                        string sonValue = grid.Cell(row, col+1).Text;
                        string motherValue = grid.Cell(row, col + 2).Text;
                        if (value != null && !"".Equals(value))
                        {
                            cd.Result = double.Parse(value);
                        }
                        if (sonValue != null && !"".Equals(sonValue))
                        {
                            cd.SonValue = double.Parse(sonValue);
                        }
                        if (motherValue != null && !"".Equals(motherValue))
                        {
                            cd.MotherValue = double.Parse(motherValue);
                        }


                        //如果存在分值，写入算出的分值
                        if (ifScore == true)
                        {
                            cd.Plan = mCube.CubeManagerSrv.getSocreByInput(cd)+"";
                        }
                        mCube.CubeManagerSrv.UpdateCubeDataById(cr, cd);
                    }
                }
            }
            else {
                for (int row = startRow; row <= endRow; row++)
                {
                    for (int col = startCol; col <= endCol; col++)
                    {
                        String id = grid.Cell(row, col).Tag;
                        CubeData cd = (CubeData)ht_cd[id];
                        string value = grid.Cell(row, col).Text;
                        if (value != null && !"".Equals(value))
                        {
                            cd.Result = double.Parse(value);
                        }


                        //如果存在分值，写入算出的分值
                        if (ifScore == true)
                        {
                            cd.Plan = mCube.CubeManagerSrv.getSocreByInput(cd)+"";
                        }
                        mCube.CubeManagerSrv.UpdateCubeDataById(cr, cd);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ViewDistribute vd = lstViewSel.SelectedItem as ViewDistribute;
            //取得立方属性
            CubeRegister cr = mCube.CubeManagerSrv.GetCubeRegisterById(vd.Main.CubeRegId.Id);

            Hashtable ht_cd = mCube.HashCubeData;
            int startRow = mCube.StartRowResult;
            int startCol = mCube.StartColResult;
            int endRow = mCube.EndRowResult;
            int endCol = mCube.EndColResult;
            this.SaveGridData(dgvCubeData, ht_cd, cr, vd, startRow, endRow, startCol, endCol);
            
            MessageBox.Show("采集数据保存成功！");

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            ViewDistribute vd = lstViewSel.SelectedItem as ViewDistribute;
            //取得立方属性
            CubeRegister cr = mCube.CubeManagerSrv.GetCubeRegisterById(vd.Main.CubeRegId.Id);

            //保存结果值
            Hashtable ht_cd = mCube.HashCubeData;
            int startRow = mCube.StartRowResult;
            int startCol = mCube.StartColResult;
            int endRow = mCube.EndRowResult;
            int endCol = mCube.EndColResult;

            this.SaveGridData(dgvCubeData, ht_cd, cr, vd, startRow, endRow, startCol, endCol);

            vd.StateName = kjn + kjy;
            mCube.ViewService.SaveViewDistribute(vd);

            lstViewSel.Items.Remove(vd);
             
            MessageBox.Show("采集数据提交成功！");
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            mCube.GeneralPreview(dgvCubeData);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            dgvCubeData.ExportToExcel("采集数据.xls", true, true, true);
            //MessageBox.Show("导出Excel成功！");
        }

        private void dgvCubeData_KeyUp(object Sender, KeyEventArgs e)
        {
            int startRow = mCube.StartRowResult;
            int startCol = mCube.StartColResult;
            int endRow = mCube.EndRowResult;
            int endCol = mCube.EndColResult;
            if (ifJJZB == false)
            {
                mCube.CalculateResult(dgvCubeData, startRow, endRow, startCol, endCol);
            }
            else {
                if (ifSonMother == true)
                {
                    FlexCell.Cell cell = dgvCubeData.ActiveCell;
                    mCube.CalculateResultByJJZB(dgvCubeData, cell);
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dgvCubeData.OpenFile("c:\\珠钢.flx");
            dgvCubeData.Cell(1, 1).Text = "88888888测试";
        }

    }
}