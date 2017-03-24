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
    public partial class VViewQuery : TBasicDataView
    {
        private MCubeManager mCube = new MCubeManager();
        private MBasicData bdModel = new MBasicData();
        private Hashtable ht_dim = new Hashtable();
        private Hashtable ht_cd = new Hashtable();
        private bool ifScore = false;

        public VViewQuery(bool ifSaveScore)
        {
            InitializeComponent();
            ifScore = ifSaveScore;
        }

        internal void Start()
        {
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
        }

        /// <summary>
        /// 激活控件的一些属性
        /// </summary>
        private void ActiveControls()
        {
            btnExcel.Enabled = true;
            btnPreview.Enabled = true;
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
            if (vd == null)
            {
                KnowledgeMessageBox.InforMessage("请选择一个模板！");
                return;
            }
            //初始化
            ActiveControls();
            ClearView();

            IList time_style = mCube.GetCurrTimeList(vd.Main);
            cboTime.DataSource = time_style;
            cboTime.DisplayMember = "Name";
            cboTime.ValueMember = "Id";

            edtDistributeDate.Text = vd.DistributeDate.ToShortDateString();
            edtSerial.Text = vd.DistributeSerial + "";

            mCube.HtDimension = ht_dim;
            if (vd.Main.CubeRegId.Id == "1")
            {
                mCube.DisplayCustomFlexCellByJJZB(dgvCubeData, vd.Main, false, false, "");
            }
            else
            {
                mCube.DisplayCustomFlexCell(dgvCubeData, vd.Main, false, false, "");
            }

            /*          
            //锁定维度列
            for (int k = 0; k < dgvCubeData.Cols - 1; k++)
            {
                dgvCubeData.Column(k).Locked = true;
            }
            */
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

        private void btnQuery_Click(object sender, EventArgs e)
        {
            ViewDistribute vd = lstViewSel.SelectedItem as ViewDistribute;
            ClearView();
            string time_str = cboTime.Text;
            if (vd.Main.CubeRegId.Id == "1")
            {
                mCube.DisplayCustomFlexCellByJJZB(dgvCubeData, vd.Main, false, false,time_str);
            }
            else
            {
                mCube.DisplayCustomFlexCell(dgvCubeData, vd.Main, false, false,time_str);
            }
        }

    }
}