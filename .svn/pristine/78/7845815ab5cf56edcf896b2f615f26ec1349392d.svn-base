using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VImportPlanTaskDetail : Form
    {
        public bool isOK = false;

        /// <summary>
        ///缺省选择的GWBS
        /// </summary>
        public GWBSTree DefaultSelectedGWBS = null;

        private Dictionary<Dictionary<Material, StandardUnit>, decimal> _SelectResult = new Dictionary<Dictionary<Material, StandardUnit>, decimal>();
        /// <summary>
        /// 选择的资源类型集合
        /// </summary>
        public Dictionary<Dictionary<Material, StandardUnit>, decimal> SelectResult
        {
            get { return _SelectResult; }
            set { _SelectResult = value; }
        }

        CurrentProjectInfo projectInfo = null;

        public MRollingDemandPlan model = new MRollingDemandPlan();

        public VImportPlanTaskDetail()
        {
            InitializeComponent();

            InitForm();

        }

        private void InitForm()
        {
            InitEvent();

            projectInfo = StaticMethod.GetProjectInfo();

        }

        private void InitEvent()
        {
            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            cbAllSelect.Click += new EventHandler(cbAllSelect_Click);
            gridResourceRequireDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridResourceRequireDetail_CellValidating);

            this.Load += new EventHandler(VImportPlanTaskDetail_Load);
            this.FormClosing += new FormClosingEventHandler(VImportPlanTaskDetail_FormClosing);
        }

        void gridResourceRequireDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object tempValue = e.FormattedValue;
                if (tempValue != null)
                {
                    string value = "";
                    if (tempValue != null)
                        value = tempValue.ToString().Trim();
                    try
                    {
                        string colName = gridResourceRequireDetail.Columns[e.ColumnIndex].Name;

                        //数据格式校验
                        if (colName == ResourceNum.Name)//合同
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("输入格式不正确！");
                        e.Cancel = true;
                    }
                }
            }
        }

        void cbAllSelect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                row.Cells[colSelect.Name].Value = cbAllSelect.Checked;
            }
        }

        void VImportPlanTaskDetail_Load(object sender, EventArgs e)
        {
            if (DefaultSelectedGWBS != null)
            {
                txtTaskName.Text = GetFullPath(DefaultSelectedGWBS);


                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("CostingFlag", 1));
                oq.AddCriterion(Expression.Like("TheGWBSSysCode", DefaultSelectedGWBS.SysCode, MatchMode.Start));

                //oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);

                IList listDtl = model.ObjectQuery(typeof(GWBSDetail), oq);
                if (listDtl.Count > 0)
                {
                    oq.Criterions.Clear();
                    Disjunction dis = new Disjunction();
                    foreach (GWBSDetail dtl in listDtl)
                    {
                        dis.Add(Expression.Eq("TheGWBSDetail.Id", dtl.Id));
                    }
                    oq.AddCriterion(dis);
                    oq.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);

                    IEnumerable<GWBSDetailCostSubject> listSubject = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq).OfType<GWBSDetailCostSubject>();

                    var queryGroup = from s in listSubject
                                     where !string.IsNullOrEmpty(s.ResourceTypeGUID)
                                     group s by new
                                     {
                                         s.ResourceTypeGUID,
                                         s.ProjectAmountUnitGUID
                                     }
                                         into g
                                         select new
                                         {
                                             g.Key.ResourceTypeGUID,
                                             g.Key.ProjectAmountUnitGUID,
                                             planQuantity = g.Sum(s => s.PlanWorkAmount)
                                         };


                    //if (queryGroup != null && queryGroup.Count() > 0)
                    //{
                    //    oq.Criterions.Clear();

                    //    Disjunction dis = new Disjunction();

                    //    foreach (var obj in queryGroup)
                    //    {
                    //        dis.Add(Expression.Eq("Id", obj.ResourceTypeGUID));
                    //    }
                    //    oq.AddCriterion(dis);
                    //    oq.AddCriterion(Expression.Like("Code", "I1", MatchMode.Start));

                    //    IList listMat = model.ObjectQuery(typeof(Material), oq);

                    //    AddResourceTypeInGrid(listMat);
                    //}


                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();
                    dis = new Disjunction();
                    foreach (var obj in queryGroup)
                    {
                        dis.Add(Expression.Eq("Id", obj.ResourceTypeGUID));
                    }
                    oq.AddCriterion(dis);
                    oq.AddCriterion(Expression.Like("Code", "I1", MatchMode.Start));
                    IEnumerable<Material> listMat = model.ObjectQuery(typeof(Material), oq).OfType<Material>();

                    listMat = from s in listMat
                              orderby s.Specification ascending
                              orderby s.Quality ascending
                              orderby s.Name ascending
                              select s;

                    foreach (Material mat in listMat)
                    {
                        var query = from m in queryGroup
                                    orderby m.planQuantity ascending
                                    orderby m.ProjectAmountUnitGUID.Name ascending
                                    where m.ResourceTypeGUID == mat.Id
                                    select m;

                        if (query.Count() > 0)
                        {
                            foreach (var obj in query)
                            {
                                int index = gridResourceRequireDetail.Rows.Add();
                                DataGridViewRow row = gridResourceRequireDetail.Rows[index];

                                row.Cells[ResourceName.Name].Value = mat.Name;
                                row.Cells[ResourceQuality.Name].Value = mat.Quality;
                                row.Cells[ResourceSpec.Name].Value = mat.Specification;

                                row.Cells[ResourceNum.Name].Value = StaticMethod.DecimelTrimEnd0(obj.planQuantity);

                                row.Cells[QuantityUnit.Name].Value = obj.ProjectAmountUnitGUID.Name;
                                row.Cells[QuantityUnit.Name].Tag = obj.ProjectAmountUnitGUID;

                                row.Tag = mat;
                            }
                        }
                    }
                }
            }
        }

        private string GetFullPath(GWBSTree wbs)
        {
            string path = string.Empty;

            path = wbs.Name;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wbs.Id));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(GWBSTree), oq);

            wbs = list[0] as GWBSTree;

            CategoryNode parent = wbs.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;

                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = model.ObjectQuery(typeof(GWBSTree), oq);

                parent = (list[0] as CategoryNode).ParentNode;
            }

            return path;
        }

        private void AddResourceTypeInGrid(IList listMat)
        {
            foreach (Material mat in listMat)
            {
                int index = gridResourceRequireDetail.Rows.Add();
                DataGridViewRow row = gridResourceRequireDetail.Rows[index];

                row.Cells[ResourceName.Name].Value = mat.Name;
                row.Cells[ResourceQuality.Name].Value = mat.Quality;
                row.Cells[ResourceSpec.Name].Value = mat.Specification;

                row.Tag = mat;
            }
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            bool isSelect = false;
            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                if (row.Cells[colSelect.Name].Value != null && (bool)row.Cells[colSelect.Name].Value == true)
                {
                    isSelect = true;
                    break;
                }
            }
            if (isSelect == false)
            {
                MessageBox.Show("请选择资源类型！");
                gridResourceRequireDetail.Focus();
                return;
            }

            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                if (row.Cells[colSelect.Name].Value != null && (bool)row.Cells[colSelect.Name].Value == true)
                {
                    Material mat = row.Tag as Material;
                    StandardUnit standUnit = row.Cells[QuantityUnit.Name].Tag as StandardUnit;

                    decimal matQuantity = 0;
                    try
                    {
                        matQuantity = ClientUtil.ToDecimal(row.Cells[ResourceNum.Name].Value);
                    }
                    catch { }

                    Dictionary<Material, StandardUnit> dicKey = new Dictionary<Material, StandardUnit>();
                    dicKey.Add(mat, standUnit);

                    if (SelectResult.Keys.Contains(dicKey) == false)
                    {
                        SelectResult.Add(dicKey, matQuantity);
                    }
                }
            }

            isOK = true;
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }

        void VImportPlanTaskDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isOK)
                SelectResult.Clear();
        }

    }
}
