using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.SupplyMng;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VGenerateExecDemandPlan : TMasterDetailView
    {
        public MRollingDemandPlan model;
        public MDailyPlanMng MDailyPlan = new MDailyPlanMng();
        public MLaborDemandPlanMng MLaborDemandPlan = new MLaborDemandPlanMng();
        public MMonthlyPlanMng MMonthlyPlan = new MMonthlyPlanMng();
        public MDemandMasterPlanMng MDemandMasterPlan = new MDemandMasterPlanMng();
        private Hashtable ht_matcat = new Hashtable();

        string frontBillType = ResourceRequirePlanType.��������ƻ�.ToString();

        private CurrentProjectInfo projectInfo = null;

        RemandPlanType optPlanType = RemandPlanType.�ڵ�����ƻ�;

        /// <summary>
        /// ��ǰ�����ճ�����ƻ�
        /// </summary>
        DailyPlanMaster theDailyPlanMaster = null;
        /// <summary>
        /// �¶�����ƻ�
        /// </summary>
        MonthlyPlanMaster theMonthPlanMaster = null;
        int AccountYear = DateTime.Now.Year;
        int AccountMonth = DateTime.Now.Month;

        /// <summary>
        /// �ڵ�����ƻ�
        /// </summary>
        MonthlyPlanMaster theNodePlanMaster = null;
        /// <summary>
        /// ��������ƻ�
        /// </summary>
        LaborDemandPlanMaster theLaborPlanMaster = null;
        /// <summary>
        /// ������ƻ�
        /// </summary>
        DemandMasterPlanMaster theMasterPlanMaster = null;

        public VGenerateExecDemandPlan(MRollingDemandPlan mot)
        {
            model = mot;

            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            projectInfo = StaticMethod.GetProjectInfo();

            InitEvents();

            //����ִ������ƻ�
            foreach (string s in Enum.GetNames(typeof(RemandPlanType)))
            {
                cbPlanType.Items.Add(s);
            }
            if (cbPlanType.Items.Count > 0)
                cbPlanType.SelectedIndex = 0;

            ht_matcat = model.Mm.GetFirstMatInfo();
        }

        private void InitEvents()
        {
            btnSelectScrollPlan.Click += new EventHandler(btnSelectScrollPlan_Click);

            cbPlanType.SelectedIndexChanged += new EventHandler(cbPlanType_SelectedIndexChanged);

            gridMasterPlan.CellValidating += new DataGridViewCellValidatingEventHandler(gridMasterPlan_CellValidating);
            gridMasterPlan.CellDoubleClick += new DataGridViewCellEventHandler(gridMasterPlan_CellDoubleClick);

            gridNodePlan.CellValidating += new DataGridViewCellValidatingEventHandler(gridNodePlan_CellValidating);
            gridNodePlan.CellDoubleClick += new DataGridViewCellEventHandler(gridNodePlan_CellDoubleClick);

            gridMonthPlan.CellValidating += new DataGridViewCellValidatingEventHandler(gridMonthPlan_CellValidating);
            gridMonthPlan.CellDoubleClick += new DataGridViewCellEventHandler(gridMonthPlan_CellDoubleClick);

            gridDayPlan.CellDoubleClick += new DataGridViewCellEventHandler(gridDayPlan_CellDoubleClick);
            gridDayPlan.CellValidating += new DataGridViewCellValidatingEventHandler(gridDayPlan_CellValidating);

            gridServicePlan.CellDoubleClick += new DataGridViewCellEventHandler(gridServicePlan_CellDoubleClick);
            gridServicePlan.CellValidating += new DataGridViewCellValidatingEventHandler(gridServicePlan_CellValidating);
        }

        void gridMasterPlan_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridMasterPlan.Columns[e.ColumnIndex].Name;
                        DemandMasterPlanDetail dtl = gridMasterPlan.Rows[e.RowIndex].Tag as DemandMasterPlanDetail;
                        if (colName == colSumQuantity.Name)//�ƻ�������
                        {
                            if (value.ToString() != "")
                            {
                                dtl.Quantity = ClientUtil.ToDecimal(value);
                            }
                            else
                                dtl.Quantity = 0;
                        }
                        else if (colName == colSumQuantityUnit.Name)
                        {
                            //string name = value.ToString();
                            //object oo= gridMasterPlan.Rows[e.RowIndex].Cells[colSumQuantityUnit.Name].Value;
                            //if (name != "")
                            //{
                            //    if (string.IsNullOrEmpty(dtl.MatStandardUnitName) || (dtl.MatStandardUnitName != name))
                            //    {
                            //        ObjectQuery oq = new ObjectQuery();
                            //        oq.AddCriterion(Expression.Eq("Name", name));
                            //        IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                            //        if (list.Count > 0)
                            //        {
                            //            dtl.MatStandardUnit = list[0] as StandardUnit;
                            //            dtl.MatStandardUnitName = dtl.MatStandardUnit.Name;
                            //        }
                            //        else
                            //        {
                            //            MessageBox.Show("ϵͳĿǰ�����ڸü�����λ�����飡");
                            //            e.Cancel = true;
                            //        }
                            //    }
                            //}
                        }
                        else if (colName == colSumRemark.Name)
                        {
                            dtl.Descript = value.ToString();
                        }
                        else if (colName == colSumDiagramNumber.Name)
                        {
                            if (value.ToString() != "")
                            {
                                dtl.DiagramNumber = ClientUtil.ToString(value);
                            }
                        }

                    }
                    catch
                    {
                        MessageBox.Show("�����ʽ����ȷ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }
        }
        void gridMasterPlan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                DemandMasterPlanDetail dtl = gridMasterPlan.Rows[e.RowIndex].Tag as DemandMasterPlanDetail;
                string colName = gridMasterPlan.Columns[e.ColumnIndex].Name;
                if (colName == colSumResourceName.Name || colName == colSumResourceCode.Name || colName == colSumSpec.Name)
                {
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.OpenSelect();

                    IList list = materialSelector.Result;
                    if (list.Count > 0)
                    {
                        Material mat = list[0] as Material;
                        dtl.MaterialResource = mat;
                        dtl.MaterialCode = mat.Code;
                        dtl.MaterialName = mat.Name;
                        dtl.MaterialSpec = mat.Specification;
                        dtl.MaterialStuff = mat.Quality;


                        MaterialCategory cate = model.GetObjectById(typeof(MaterialCategory), mat.Category.Id) as MaterialCategory;
                        dtl.MaterialCategory = cate;
                        if (cate != null)
                            dtl.MaterialCategoryName = cate.Name;
                        else
                            dtl.MaterialCategoryName = "";

                        gridMasterPlan.Rows[e.RowIndex].Cells[colSumResourceName.Name].Value = dtl.MaterialName;
                        gridMasterPlan.Rows[e.RowIndex].Cells[colSumResourceCode.Name].Value = dtl.MaterialCode;
                        gridMasterPlan.Rows[e.RowIndex].Cells[colSumSpec.Name].Value = dtl.MaterialSpec;

                        gridMasterPlan.Rows[e.RowIndex].Tag = dtl;

                        gridMasterPlan.CurrentCell = gridMasterPlan.Rows[e.RowIndex].Cells[colSumResourceCode.Name];
                        gridMasterPlan.CurrentCell = gridMasterPlan.Rows[e.RowIndex].Cells[colSumResourceName.Name];
                    }
                }
                else if (colName == colSumQuantityUnit.Name)
                {
                    StandardUnit su = UCL.Locate("������λά��", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        dtl.MatStandardUnit = su;
                        dtl.MatStandardUnitName = su.Name;

                        gridMasterPlan.Rows[e.RowIndex].Cells[colSumQuantityUnit.Name].Value = su.Name;

                        gridMasterPlan.CurrentCell = gridMasterPlan.Rows[e.RowIndex].Cells[0];
                        gridMasterPlan.CurrentCell = gridMasterPlan.Rows[e.RowIndex].Cells[colSumQuantityUnit.Name];
                    }
                }
            }
        }

        void gridNodePlan_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridNodePlan.Columns[e.ColumnIndex].Name;
                        MonthlyPlanDetail dtl = gridNodePlan.Rows[e.RowIndex].Tag as MonthlyPlanDetail;
                        if (colName == colNodeQuantity.Name)//�ƻ�������
                        {
                            if (value.ToString() != "")
                            {
                                dtl.Quantity = ClientUtil.ToDecimal(value);
                            }
                            else
                                dtl.Quantity = 0;
                        }
                        else if (colName == colNodeQuantityUnit.Name)
                        {
                            //string name = value.ToString();
                            //if (name != "")
                            //{
                            //    if (string.IsNullOrEmpty(dtl.MatStandardUnitName) || (dtl.MatStandardUnitName != name))
                            //    {
                            //        ObjectQuery oq = new ObjectQuery();
                            //        oq.AddCriterion(Expression.Eq("Name", name));
                            //        IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                            //        if (list.Count > 0)
                            //        {
                            //            dtl.MatStandardUnit = list[0] as StandardUnit;
                            //            dtl.MatStandardUnitName = dtl.MatStandardUnit.Name;
                            //        }
                            //        else
                            //        {
                            //            MessageBox.Show("ϵͳĿǰ�����ڸü�����λ�����飡");
                            //            e.Cancel = true;
                            //        }
                            //    }
                            //}
                        }
                        else if (colName == colNodeRemark.Name)
                        {
                            dtl.Descript = value.ToString();
                        }
                        else if (colName == colNodeDiagramNumber.Name)
                        {
                            if (value.ToString() != "")
                            {
                                dtl.DiagramNumber = ClientUtil.ToString(value);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("�����ʽ����ȷ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }
        }
        void gridNodePlan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                MonthlyPlanDetail dtl = gridNodePlan.Rows[e.RowIndex].Tag as MonthlyPlanDetail;
                string colName = gridNodePlan.Columns[e.ColumnIndex].Name;
                if (colName == colNodeResourceName.Name || colName == colNodeResourceCode.Name || colName == colNodeMaterialSpec.Name)
                {
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.OpenSelect();

                    IList list = materialSelector.Result;
                    if (list.Count > 0)
                    {
                        Material mat = list[0] as Material;
                        dtl.MaterialResource = mat;
                        dtl.MaterialCode = mat.Code;
                        dtl.MaterialName = mat.Name;
                        dtl.MaterialSpec = mat.Specification;
                        dtl.MaterialStuff = mat.Quality;

                        MaterialCategory cate = model.GetObjectById(typeof(MaterialCategory), mat.Category.Id) as MaterialCategory;
                        dtl.MaterialCategory = cate;
                        if (cate != null)
                            dtl.MaterialCategoryName = cate.Name;
                        else
                            dtl.MaterialCategoryName = "";

                        gridNodePlan.Rows[e.RowIndex].Cells[colNodeResourceName.Name].Value = dtl.MaterialName;
                        gridNodePlan.Rows[e.RowIndex].Cells[colNodeResourceCode.Name].Value = dtl.MaterialCode;
                        gridNodePlan.Rows[e.RowIndex].Cells[colNodeMaterialSpec.Name].Value = dtl.MaterialSpec;

                        gridNodePlan.Rows[e.RowIndex].Tag = dtl;

                        gridNodePlan.CurrentCell = gridNodePlan.Rows[e.RowIndex].Cells[colNodeResourceCode.Name];
                        gridNodePlan.CurrentCell = gridNodePlan.Rows[e.RowIndex].Cells[colNodeResourceName.Name];
                    }
                }
                else if (colName == colNodeQuantityUnit.Name)
                {
                    StandardUnit su = UCL.Locate("������λά��", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        dtl.MatStandardUnit = su;
                        dtl.MatStandardUnitName = su.Name;

                        gridNodePlan.Rows[e.RowIndex].Cells[colNodeQuantityUnit.Name].Value = su.Name;

                        gridNodePlan.CurrentCell = gridNodePlan.Rows[e.RowIndex].Cells[0];
                        gridNodePlan.CurrentCell = gridNodePlan.Rows[e.RowIndex].Cells[colNodeQuantityUnit.Name];
                    }
                }
            }
        }

        void gridMonthPlan_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridMonthPlan.Columns[e.ColumnIndex].Name;
                        MonthlyPlanDetail dtl = gridMonthPlan.Rows[e.RowIndex].Tag as MonthlyPlanDetail;
                        if (colName == colMonthQuantity.Name)//�ƻ�������
                        {
                            if (value.ToString() != "")
                            {
                                dtl.Quantity = ClientUtil.ToDecimal(value);
                            }
                            else
                                dtl.Quantity = 0;
                        }
                        else if (colName == colMonthQuantityUnit.Name)
                        {
                            //string name = value.ToString();
                            //if (name != "")
                            //{
                            //    if (string.IsNullOrEmpty(dtl.MatStandardUnitName) || (dtl.MatStandardUnitName != name))
                            //    {
                            //        ObjectQuery oq = new ObjectQuery();
                            //        oq.AddCriterion(Expression.Eq("Name", name));
                            //        IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                            //        if (list.Count > 0)
                            //        {
                            //            dtl.MatStandardUnit = list[0] as StandardUnit;
                            //            dtl.MatStandardUnitName = dtl.MatStandardUnit.Name;
                            //        }
                            //        else
                            //        {
                            //            MessageBox.Show("ϵͳĿǰ�����ڸü�����λ�����飡");
                            //            e.Cancel = true;
                            //        }
                            //    }
                            //}
                        }
                        else if (colName == colMonthRemark.Name)
                        {
                            dtl.Descript = value.ToString();
                        }
                        else if (colName == colMonthDiagramNumber.Name)
                        {
                            if (value.ToString() != "")
                            {
                                dtl.DiagramNumber = ClientUtil.ToString(value);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("�����ʽ����ȷ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }

        }
        void gridMonthPlan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                MonthlyPlanDetail dtl = gridMonthPlan.Rows[e.RowIndex].Tag as MonthlyPlanDetail;
                string colName = gridMonthPlan.Columns[e.ColumnIndex].Name;
                if (colName == colMonthResourceName.Name || colName == colMonthResourceCode.Name || colName == colMonthMaterialSpec.Name)
                {
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.OpenSelect();

                    IList list = materialSelector.Result;
                    if (list.Count > 0)
                    {
                        Material mat = list[0] as Material;
                        dtl.MaterialResource = mat;
                        dtl.MaterialCode = mat.Code;
                        dtl.MaterialName = mat.Name;
                        dtl.MaterialSpec = mat.Specification;
                        dtl.MaterialStuff = mat.Quality;

                        MaterialCategory cate = model.GetObjectById(typeof(MaterialCategory), mat.Category.Id) as MaterialCategory;
                        dtl.MaterialCategory = cate;
                        if (cate != null)
                            dtl.MaterialCategoryName = cate.Name;
                        else
                            dtl.MaterialCategoryName = "";

                        gridMonthPlan.Rows[e.RowIndex].Cells[colMonthResourceName.Name].Value = dtl.MaterialName;
                        gridMonthPlan.Rows[e.RowIndex].Cells[colMonthResourceCode.Name].Value = dtl.MaterialCode;
                        gridMonthPlan.Rows[e.RowIndex].Cells[colMonthMaterialSpec.Name].Value = dtl.MaterialSpec;

                        gridMonthPlan.Rows[e.RowIndex].Tag = dtl;

                        gridMonthPlan.CurrentCell = gridMonthPlan.Rows[e.RowIndex].Cells[colMonthResourceCode.Name];
                        gridMonthPlan.CurrentCell = gridMonthPlan.Rows[e.RowIndex].Cells[colMonthResourceName.Name];
                    }
                }
                else if (colName == colMonthQuantityUnit.Name)
                {
                    StandardUnit su = UCL.Locate("������λά��", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        dtl.MatStandardUnit = su;
                        dtl.MatStandardUnitName = su.Name;

                        gridMonthPlan.Rows[e.RowIndex].Cells[colMonthQuantityUnit.Name].Value = su.Name;

                        gridMonthPlan.CurrentCell = gridMonthPlan.Rows[e.RowIndex].Cells[0];
                        gridMonthPlan.CurrentCell = gridMonthPlan.Rows[e.RowIndex].Cells[colMonthQuantityUnit.Name];
                    }
                }
                else if (colName == colMonthUsedTeam.Name)
                {
                    CommonSupplier Supplier = new CommonSupplier();
                    DataGridViewCell cell = this.gridMonthPlan[e.ColumnIndex, e.RowIndex];
                    object tempValue = cell.EditedFormattedValue;
                    if (tempValue != null && !tempValue.Equals(""))
                    {
                        Supplier.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-";
                        Supplier.OpenSelect();
                    }
                    else
                    {
                        Supplier.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-";
                        Supplier.OpenSelect();
                    }

                    IList list = Supplier.Result;
                    foreach (SupplierRelationInfo Suppliers in list)
                    {
                        this.gridMonthPlan.CurrentRow.Cells[colMonthUsedTeam.Name].Tag = Suppliers;
                        this.gridMonthPlan.CurrentRow.Cells[colMonthUsedTeam.Name].Value = Suppliers.SupplierInfo.Name;
                        //this.txtCode.Focus();
                    }
                }

            }
        }

        void gridDayPlan_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridDayPlan.Columns[e.ColumnIndex].Name;
                        DailyPlanDetail dtl = gridDayPlan.Rows[e.RowIndex].Tag as DailyPlanDetail;
                        if (colName == colDayQuantity.Name)//�ƻ�������
                        {
                            if (value.ToString() != "")
                            {
                                dtl.Quantity = ClientUtil.ToDecimal(value);
                            }
                            else
                                dtl.Quantity = 0;
                        }
                        else if (colName == colDayQuantityUnit.Name)
                        {
                            //string name = value.ToString();
                            //if (name != "")
                            //{
                            //    if (string.IsNullOrEmpty(dtl.MatStandardUnitName) || (dtl.MatStandardUnitName != name))
                            //    {
                            //        ObjectQuery oq = new ObjectQuery();
                            //        oq.AddCriterion(Expression.Eq("Name", name));
                            //        IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                            //        if (list.Count > 0)
                            //        {
                            //            dtl.MatStandardUnit = list[0] as StandardUnit;
                            //            dtl.MatStandardUnitName = dtl.MatStandardUnit.Name;
                            //        }
                            //        else
                            //        {
                            //            MessageBox.Show("ϵͳĿǰ�����ڸü�����λ�����飡");
                            //            e.Cancel = true;
                            //        }
                            //    }
                            //}
                        }
                        else if (colName == colDayRemark.Name)
                        {
                            dtl.Descript = value.ToString();
                        }
                        else if (colName == colDayDiagramNumber.Name)
                        {
                            if (value.ToString() != "")
                            {
                                dtl.DiagramNumber = ClientUtil.ToString(value);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("�����ʽ����ȷ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }

                }
            }

        }
        void gridDayPlan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                DailyPlanDetail dtl = gridDayPlan.Rows[e.RowIndex].Tag as DailyPlanDetail;
                string colName = gridDayPlan.Columns[e.ColumnIndex].Name;
                if (colName == colDayResourceName.Name || colName == colDayResourceCode.Name || colName == colDaySpec.Name)
                {
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.OpenSelect();

                    IList list = materialSelector.Result;
                    if (list.Count > 0)
                    {
                        Material mat = list[0] as Material;
                        dtl.MaterialResource = mat;
                        dtl.MaterialCode = mat.Code;
                        dtl.MaterialName = mat.Name;
                        dtl.MaterialSpec = mat.Specification;
                        dtl.MaterialStuff = mat.Quality;

                        MaterialCategory cate = model.GetObjectById(typeof(MaterialCategory), mat.Category.Id) as MaterialCategory;
                        dtl.MaterialCategory = cate;
                        if (cate != null)
                            dtl.MaterialCategoryName = cate.Name;
                        else
                            dtl.MaterialCategoryName = "";

                        gridDayPlan.Rows[e.RowIndex].Cells[colDayResourceName.Name].Value = dtl.MaterialName;
                        gridDayPlan.Rows[e.RowIndex].Cells[colDayResourceCode.Name].Value = dtl.MaterialCode;
                        gridDayPlan.Rows[e.RowIndex].Cells[colDaySpec.Name].Value = dtl.MaterialSpec;

                        gridDayPlan.Rows[e.RowIndex].Tag = dtl;

                        gridDayPlan.CurrentCell = gridDayPlan.Rows[e.RowIndex].Cells[colDayResourceCode.Name];
                        gridDayPlan.CurrentCell = gridDayPlan.Rows[e.RowIndex].Cells[colDayResourceName.Name];
                    }
                }
                else if (colName == colDayQuantityUnit.Name)
                {
                    StandardUnit su = UCL.Locate("������λά��", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        dtl.MatStandardUnit = su;
                        dtl.MatStandardUnitName = su.Name;

                        gridDayPlan.Rows[e.RowIndex].Cells[colDayQuantityUnit.Name].Value = su.Name;

                        gridDayPlan.CurrentCell = gridDayPlan.Rows[e.RowIndex].Cells[0];
                        gridDayPlan.CurrentCell = gridDayPlan.Rows[e.RowIndex].Cells[colDayQuantityUnit.Name];
                    }
                }
                    //ʹ�ö�����֧��˫���¼�
                else if (colName == colDayUsedTeam.Name)
                {
                    CommonSupplier Supplier = new CommonSupplier();
                    DataGridViewCell cell = this.gridDayPlan[e.ColumnIndex, e.RowIndex];
                    object tempValue = cell.EditedFormattedValue;
                    if (tempValue != null && !tempValue.Equals(""))
                    {
                        Supplier.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-";
                        Supplier.OpenSelect();
                    }
                    else
                    {
                        Supplier.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-";
                        Supplier.OpenSelect();
                    }

                    IList list = Supplier.Result;
                    foreach (SupplierRelationInfo Suppliers in list)
                    {
                        this.gridDayPlan.CurrentRow.Cells[colDayUsedTeam.Name].Tag = Suppliers;
                        this.gridDayPlan.CurrentRow.Cells[colDayUsedTeam.Name].Value = Suppliers.SupplierInfo.Name;
                        //this.txtCode.Focus();
                    }
                }
            }
        }

        void gridServicePlan_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridServicePlan.Columns[e.ColumnIndex].Name;
                        LaborDemandPlanDetail dtl = gridServicePlan.Rows[e.RowIndex].Tag as LaborDemandPlanDetail;
                        if (colName == colServiceProjectQuantity.Name)//Ԥ�ƹ�����
                        {
                            if (value.ToString() != "")
                            {
                                dtl.Quantity = ClientUtil.ToDecimal(value);
                            }
                            else
                                dtl.Quantity = 0;
                        }
                        else if (colName == colServiceProjectQnyUnit.Name)
                        {
                            //string name = value.ToString();
                            //if (name != "")
                            //{
                            //    if (string.IsNullOrEmpty(dtl.MatStandardUnitName) || (dtl.MatStandardUnitName != name))
                            //    {
                            //        ObjectQuery oq = new ObjectQuery();
                            //        oq.AddCriterion(Expression.Eq("Name", name));
                            //        IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                            //        if (list.Count > 0)
                            //        {
                            //            dtl.MatStandardUnit = list[0] as StandardUnit;
                            //            dtl.MatStandardUnitName = dtl.MatStandardUnit.Name;
                            //        }
                            //        else
                            //        {
                            //            MessageBox.Show("ϵͳĿǰ�����ڸü�����λ�����飡");
                            //            e.Cancel = true;
                            //        }
                            //    }
                            //}
                        }
                        else if (colName == colServiceMainWorkContent.Name)
                        {
                            dtl.MainJobDescript = value.ToString();
                        }
                        else if (colName == colServiceQuanlityRequire.Name)
                        {
                            dtl.QualitySafetyRequirement = value.ToString();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("�����ʽ����ȷ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }
        }
        void gridServicePlan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                LaborDemandPlanDetail dtl = gridServicePlan.Rows[e.RowIndex].Tag as LaborDemandPlanDetail;
                string colName = gridServicePlan.Columns[e.ColumnIndex].Name;
                if (colName == colServiceProjectQnyUnit.Name)
                {
                    StandardUnit su = UCL.Locate("������λά��", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        dtl.MatStandardUnit = su;
                        dtl.MatStandardUnitName = su.Name;

                        gridServicePlan.Rows[e.RowIndex].Cells[colServiceProjectQnyUnit.Name].Value = su.Name;

                        gridServicePlan.CurrentCell = gridServicePlan.Rows[e.RowIndex].Cells[0];
                        gridServicePlan.CurrentCell = gridServicePlan.Rows[e.RowIndex].Cells[colServiceProjectQnyUnit.Name];
                    }
                }
            }
        }

        void cbPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbPlanType.SelectedIndexChanged -= new EventHandler(cbPlanType_SelectedIndexChanged);

            RemandPlanType prevPlanType = optPlanType;
            if ((theDailyPlanMaster != null || theLaborPlanMaster != null || theMasterPlanMaster != null || theMonthPlanMaster != null || theNodePlanMaster != null)
                &&
                MessageBox.Show("�Ƿ�Ҫ���浱ǰ��" + optPlanType + "��?", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (SaveView() == false)
                {
                    cbPlanType.Text = optPlanType.ToString();
                    cbPlanType.SelectedIndexChanged += new EventHandler(cbPlanType_SelectedIndexChanged);
                    return;
                }
            }
            else
            {
                theDailyPlanMaster = null;
                theLaborPlanMaster = null;
                theMasterPlanMaster = null;
                theMonthPlanMaster = null;
                theNodePlanMaster = null;
            }


            optPlanType = VirtualMachine.Component.Util.EnumUtil<RemandPlanType>.FromDescription(cbPlanType.SelectedItem.ToString());

            ClearView();

            if (optPlanType == RemandPlanType.�ճ�����ƻ�)
            {
                if (tabControlPlan.TabPages.Contains(tabPage�ճ�����ƻ�) == false)
                    tabControlPlan.TabPages.Add(tabPage�ճ�����ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�ڵ�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�ڵ�����ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage��������ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage��������ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�����ܼƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�����ܼƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�¶�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�¶�����ƻ�);
            }
            else if (optPlanType == RemandPlanType.�¶�����ƻ�)
            {
                if (tabControlPlan.TabPages.Contains(tabPage�ճ�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�ճ�����ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�ڵ�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�ڵ�����ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage��������ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage��������ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�����ܼƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�����ܼƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�¶�����ƻ�) == false)
                    tabControlPlan.TabPages.Add(tabPage�¶�����ƻ�);

                VGenerateRequirePlanSetYearMonth frm = new VGenerateRequirePlanSetYearMonth();
                frm.ShowDialog();
                if (frm.IsOk)
                {
                    AccountYear = frm.AccountYear;
                    AccountMonth = frm.AccountMonth;
                }
                else
                {
                    MessageBox.Show("��ȡ���ˡ�" + optPlanType + "���ı��ƣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    optPlanType = prevPlanType;
                    cbPlanType.Text = prevPlanType.ToString();
                }
            }
            else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
            {
                if (tabControlPlan.TabPages.Contains(tabPage�ճ�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�ճ�����ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�ڵ�����ƻ�) == false)
                    tabControlPlan.TabPages.Add(tabPage�ڵ�����ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage��������ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage��������ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�����ܼƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�����ܼƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�¶�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�¶�����ƻ�);

            }
            else if (optPlanType == RemandPlanType.��������ƻ�)
            {
                if (tabControlPlan.TabPages.Contains(tabPage�ճ�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�ճ�����ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�ڵ�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�ڵ�����ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage��������ƻ�) == false)
                    tabControlPlan.TabPages.Add(tabPage��������ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�����ܼƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�����ܼƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�¶�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�¶�����ƻ�);
            }
            else if (optPlanType == RemandPlanType.�����ܼƻ�)
            {
                if (tabControlPlan.TabPages.Contains(tabPage�ճ�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�ճ�����ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�ڵ�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�ڵ�����ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage��������ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage��������ƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�����ܼƻ�) == false)
                    tabControlPlan.TabPages.Add(tabPage�����ܼƻ�);
                if (tabControlPlan.TabPages.Contains(tabPage�¶�����ƻ�) == true)
                    tabControlPlan.TabPages.Remove(tabPage�¶�����ƻ�);
            }

            cbPlanType.SelectedIndexChanged += new EventHandler(cbPlanType_SelectedIndexChanged);

        }

        private bool ValidSaveCurrBill()
        {
            if (optPlanType == RemandPlanType.�����ܼƻ�)
            {
                if (theMasterPlanMaster != null && MessageBox.Show("�Ƿ�Ҫ���浱ǰ��" + optPlanType + "��?", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return SaveView();
                }
            }
            else if (optPlanType == RemandPlanType.�ճ�����ƻ�)
            {
                if (theDailyPlanMaster != null && MessageBox.Show("�Ƿ�Ҫ���浱ǰ��" + optPlanType + "��?", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return SaveView();
                }
            }
            else if (optPlanType == RemandPlanType.�¶�����ƻ�)
            {
                if (theMonthPlanMaster != null && MessageBox.Show("�Ƿ�Ҫ���浱ǰ��" + optPlanType + "��?", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return SaveView();
                }
            }
            else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
            {
                if (theNodePlanMaster != null && MessageBox.Show("�Ƿ�Ҫ���浱ǰ��" + optPlanType + "��?", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return SaveView();
                }
            }
            else if (optPlanType == RemandPlanType.��������ƻ�)
            {
                if (theLaborPlanMaster != null && MessageBox.Show("�Ƿ�Ҫ���浱ǰ��" + optPlanType + "��?", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return SaveView();
                }
            }
            return true;
        }

        void btnSelectScrollPlan_Click(object sender, EventArgs e)
        {
            VSelectRollingDemandPlan frm = new VSelectRollingDemandPlan(new MRollingDemandPlan());
            frm.FilterPlanType = ResourceRequirePlanType.��������ƻ�;
            frm.FilterPlanState = ResourceRequirePlanDetailState.����;
            frm.PlanType = optPlanType;
            frm.ShowDialog();

            if (frm.isOK)
            {
                IList listResult = frm.SelectResult;

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (ResourceRequirePlanDetail dtl in listResult)
                {
                    dis.Add(Expression.Eq("Id", dtl.Id));
                }
                oq.AddCriterion(dis);
                oq.AddFetchMode("MaterialResource.Category", NHibernate.FetchMode.Eager);
                listResult = model.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);

                DateTime serverTime = model.GetServerTime();

                ResourceRequirePlan frontBill = model.GetObjectById(typeof(ResourceRequirePlan), (listResult[0] as ResourceRequirePlanDetail).TheResourceRequirePlan.Id) as ResourceRequirePlan;
                string frontBillId = frontBill.Id;
                string frontBillCode = frontBill.Code;

                if (optPlanType == RemandPlanType.�����ܼƻ�)
                {
                    #region ����������ƻ�
                    if (theMasterPlanMaster == null)
                    {
                        theMasterPlanMaster = new DemandMasterPlanMaster();
                        theMasterPlanMaster.Code = model.GetCode(typeof(DemandMasterPlanMaster), projectInfo.Id);
                        //������Ϣ
                        theMasterPlanMaster.CreatePerson = ConstObject.LoginPersonInfo;
                        theMasterPlanMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                        theMasterPlanMaster.CreateDate = serverTime;
                        theMasterPlanMaster.CreateYear = serverTime.Year;
                        theMasterPlanMaster.CreateMonth = serverTime.Month;

                        theMasterPlanMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//��¼������
                        theMasterPlanMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                        theMasterPlanMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;

                        theMasterPlanMaster.HandlePerson = ConstObject.LoginPersonInfo;
                        theMasterPlanMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                        theMasterPlanMaster.DocState = DocumentState.Edit;

                        theMasterPlanMaster.ProjectId = projectInfo.Id;
                        theMasterPlanMaster.ProjectName = projectInfo.Name;

                        theMasterPlanMaster.ForwardBillType = frontBillType;
                        theMasterPlanMaster.ForwardBillId = frontBillId;
                        theMasterPlanMaster.ForwardBillCode = frontBillCode;

                        theMasterPlanMaster.RealOperationDate = serverTime;

                    }

                    foreach (ResourceRequirePlanDetail dtl in listResult)
                    {
                        if (IsExistsPlanDetail(dtl) == false)
                        {
                            DemandMasterPlanDetail planDtl = new DemandMasterPlanDetail();

                            planDtl.UsedPart = dtl.TheGWBSTaskGUID;
                            planDtl.UsedPartName = dtl.TheGWBSTaskName;

                            planDtl.MaterialResource = dtl.MaterialResource;
                            planDtl.MaterialCode = dtl.MaterialCode;
                            planDtl.MaterialName = dtl.MaterialName;
                            planDtl.MaterialSpec = dtl.MaterialSpec;
                            planDtl.Quantity = dtl.PlanRequireQuantity;
                            //planDtl.UsedPart = dtl.u

                            planDtl.DiagramNumber = dtl.DiagramNumber;

                            if (dtl.MaterialResource != null)
                            {
                                string matCode = dtl.MaterialCode.Substring(0, 4);
                                DataDomain domain = (DataDomain)ht_matcat[matCode];
                                if (domain != null)
                                {
                                    MaterialCategory currCat = new MaterialCategory();
                                    currCat.Id = domain.Name1.ToString(); ;
                                    currCat.Name = domain.Name2.ToString();
                                    currCat.Version = 1;
                                    //planDtl.MaterialCategory = dtl.MaterialResource.Category;
                                    //planDtl.MaterialCategoryName = dtl.MaterialResource.Category.Name;
                                    planDtl.MaterialCategory = currCat;
                                    planDtl.MaterialCategoryName = currCat.Name;
                                }
                            }

                            planDtl.Quantity = dtl.PlanRequireQuantity;

                            planDtl.MatStandardUnit = dtl.QuantityUnitGUID;
                            planDtl.MatStandardUnitName = dtl.QuantityUnitName;

                            planDtl.ForwardDetailId = dtl.Id;

                            theMasterPlanMaster.AddDetail(planDtl);

                            AddMasterPlanDetailInGrid(planDtl, false, false);
                        }
                    }
                    #endregion
                }
                else if (optPlanType == RemandPlanType.�ճ�����ƻ�)
                {
                    #region �����ճ�����ƻ�

                    if (theDailyPlanMaster == null)
                    {
                        theDailyPlanMaster = new DailyPlanMaster();
                        theDailyPlanMaster.Code = model.GetCode(typeof(DailyPlanMaster), projectInfo.Id);
                        //������Ϣ
                        theDailyPlanMaster.CreatePerson = ConstObject.LoginPersonInfo;
                        theDailyPlanMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                        theDailyPlanMaster.CreateDate = serverTime;
                        theDailyPlanMaster.CreateYear = serverTime.Year;
                        theDailyPlanMaster.CreateMonth = serverTime.Month;
                        theDailyPlanMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//��¼������
                        theDailyPlanMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                        theDailyPlanMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                        theDailyPlanMaster.HandlePerson = ConstObject.LoginPersonInfo;
                        theDailyPlanMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                        theDailyPlanMaster.DocState = DocumentState.Edit;

                        theDailyPlanMaster.ProjectId = projectInfo.Id;
                        theDailyPlanMaster.ProjectName = projectInfo.Name;

                        theDailyPlanMaster.ForwardBillType = frontBillType;
                        theDailyPlanMaster.ForwardBillId = frontBillId;
                        theDailyPlanMaster.ForwardBillCode = frontBillCode;

                        theDailyPlanMaster.RealOperationDate = serverTime;
                    }

                    //��ϸ��Ϣ
                    foreach (ResourceRequirePlanDetail dtl in listResult)
                    {
                        if (IsExistsPlanDetail(dtl) == false)
                        {
                            DailyPlanDetail planDtl = new DailyPlanDetail();

                            planDtl.UsedPart = dtl.TheGWBSTaskGUID;
                            planDtl.UsedPartName = dtl.TheGWBSTaskName;
                            planDtl.ProjectTaskSysCode = dtl.TheGWBSSysCode;

                            planDtl.MaterialResource = dtl.MaterialResource;
                            planDtl.MaterialCode = dtl.MaterialCode;
                            planDtl.MaterialName = dtl.MaterialName;
                            planDtl.MaterialSpec = dtl.MaterialSpec;
                            planDtl.MaterialStuff = dtl.MaterialStuff;
                            //planDtl.QualityStandard = dtl.MaterialResource.;

                            planDtl.DiagramNumber = dtl.DiagramNumber;

                            if (dtl.MaterialResource != null)
                            {
                                string matCode = dtl.MaterialCode.Substring(0, 4);
                                DataDomain domain = (DataDomain)ht_matcat[matCode];
                                if (domain != null)
                                {
                                    MaterialCategory currCat = new MaterialCategory();
                                    currCat.Id = domain.Name1.ToString();
                                    currCat.Name = domain.Name2.ToString();
                                    currCat.Version = 1;
                                    //planDtl.MaterialCategory = dtl.MaterialResource.Category;
                                    //planDtl.MaterialCategoryName = dtl.MaterialResource.Category.Name;
                                    planDtl.MaterialCategory = currCat;
                                    planDtl.MaterialCategoryName = currCat.Name;
                                }

                                planDtl.Manufacturer = dtl.MaterialResource.Factory;

                            }

                            planDtl.Quantity = dtl.PlanRequireQuantity;
                            planDtl.MatStandardUnit = dtl.QuantityUnitGUID;
                            planDtl.MatStandardUnitName = dtl.QuantityUnitName;

                            planDtl.ForwardDetailId = dtl.Id;


                            theDailyPlanMaster.AddDetail(planDtl);

                            AddDayPlanDetailInGrid(planDtl, false, false);
                        }
                    }

                    #endregion
                }
                else if (optPlanType == RemandPlanType.�¶�����ƻ�)
                {
                    #region �����¶�����ƻ�

                    if (theMonthPlanMaster == null)
                    {
                        theMonthPlanMaster = new MonthlyPlanMaster();
                        theMonthPlanMaster.Code = model.GetCode(typeof(MonthlyPlanMaster), projectInfo.Id);
                        //������Ϣ
                        theMonthPlanMaster.CreatePerson = ConstObject.LoginPersonInfo;
                        theMonthPlanMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                        theMonthPlanMaster.CreateDate = serverTime;
                        theMonthPlanMaster.CreateYear = serverTime.Year;
                        theMonthPlanMaster.CreateMonth = serverTime.Month;
                        theMonthPlanMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//��¼������
                        theMonthPlanMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                        theMonthPlanMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                        theMonthPlanMaster.HandlePerson = ConstObject.LoginPersonInfo;
                        theMonthPlanMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                        theMonthPlanMaster.DocState = DocumentState.Edit;

                        theMonthPlanMaster.RealOperationDate = serverTime;

                        //��ݺ��·�
                        theMonthPlanMaster.Year = AccountYear;
                        theMonthPlanMaster.Month = AccountMonth;

                        theMonthPlanMaster.ProjectId = projectInfo.Id;
                        theMonthPlanMaster.ProjectName = projectInfo.Name;

                        theMonthPlanMaster.ForwardBillType = frontBillType;
                        theMonthPlanMaster.ForwardBillId = frontBillId;
                        theMonthPlanMaster.ForwardBillCode = frontBillCode;


                        theMonthPlanMaster.MonthePlanType = "�¶ȼƻ�";
                    }

                    //��ϸ��Ϣ
                    foreach (ResourceRequirePlanDetail dtl in listResult)
                    {
                        if (IsExistsPlanDetail(dtl) == false)
                        {
                            MonthlyPlanDetail planDtl = new MonthlyPlanDetail();

                            planDtl.UsedPart = dtl.TheGWBSTaskGUID;
                            planDtl.UsedPartName = dtl.TheGWBSTaskName;
                            planDtl.ProjectTaskSysCode = dtl.TheGWBSSysCode;

                            planDtl.MaterialResource = dtl.MaterialResource;
                            planDtl.MaterialCode = dtl.MaterialCode;
                            planDtl.MaterialName = dtl.MaterialName;
                            planDtl.MaterialSpec = dtl.MaterialSpec;
                            planDtl.MaterialStuff = dtl.MaterialStuff;

                            planDtl.DiagramNumber = dtl.DiagramNumber;

                            if (dtl.MaterialResource != null)
                            {
                                string matCode = dtl.MaterialCode.Substring(0, 4);
                                DataDomain domain = (DataDomain)ht_matcat[matCode];
                                if (domain != null)
                                {
                                    MaterialCategory currCat = new MaterialCategory();
                                    currCat.Id = domain.Name1.ToString(); ;
                                    currCat.Name = domain.Name2.ToString();
                                    currCat.Version = 1;
                                    //planDtl.MaterialCategory = dtl.MaterialResource.Category;
                                    //planDtl.MaterialCategoryName = dtl.MaterialResource.Category.Name;
                                    planDtl.MaterialCategory = currCat;
                                    planDtl.MaterialCategoryName = currCat.Name;
                                }

                                planDtl.Manufacturer = dtl.MaterialResource.Factory;
                            }



                            planDtl.Quantity = dtl.PlanRequireQuantity;

                            planDtl.MatStandardUnit = dtl.QuantityUnitGUID;
                            planDtl.MatStandardUnitName = dtl.QuantityUnitName;

                            planDtl.ForwardDetailId = dtl.Id;

                            theMonthPlanMaster.AddDetail(planDtl);

                            AddMonthPlanDetailInGrid(planDtl, false, false);
                        }
                    }
                    #endregion
                }
                else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
                {
                    #region ���ɽڵ�����ƻ�

                    if (theNodePlanMaster == null)
                    {
                        theNodePlanMaster = new MonthlyPlanMaster();
                        theNodePlanMaster.Code = model.GetCode(typeof(MonthlyPlanMaster), projectInfo.Id);
                        //������Ϣ
                        theNodePlanMaster.CreatePerson = ConstObject.LoginPersonInfo;
                        theNodePlanMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                        theNodePlanMaster.CreateDate = serverTime;
                        theNodePlanMaster.CreateYear = serverTime.Year;
                        theNodePlanMaster.CreateMonth = serverTime.Month;
                        theNodePlanMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//��¼������
                        theNodePlanMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                        theNodePlanMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                        theNodePlanMaster.HandlePerson = ConstObject.LoginPersonInfo;
                        theNodePlanMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                        theNodePlanMaster.DocState = DocumentState.Edit;

                        theNodePlanMaster.ProjectId = projectInfo.Id;
                        theNodePlanMaster.ProjectName = projectInfo.Name;

                        theNodePlanMaster.ForwardBillType = frontBillType;
                        theNodePlanMaster.ForwardBillId = frontBillId;
                        theNodePlanMaster.ForwardBillCode = frontBillCode;

                        theNodePlanMaster.RealOperationDate = serverTime;
                        theNodePlanMaster.MonthePlanType = "�ڵ�ƻ�";
                    }

                    //��ϸ��Ϣ
                    foreach (ResourceRequirePlanDetail dtl in listResult)
                    {
                        if (IsExistsPlanDetail(dtl) == false)
                        {
                            MonthlyPlanDetail planDtl = new MonthlyPlanDetail();

                            planDtl.UsedPart = dtl.TheGWBSTaskGUID;
                            planDtl.UsedPartName = dtl.TheGWBSTaskName;
                            planDtl.ProjectTaskSysCode = dtl.TheGWBSSysCode;

                            planDtl.MaterialResource = dtl.MaterialResource;
                            planDtl.MaterialCode = dtl.MaterialCode;
                            planDtl.MaterialName = dtl.MaterialName;
                            planDtl.MaterialSpec = dtl.MaterialSpec;
                            planDtl.MaterialStuff = dtl.MaterialStuff;

                            planDtl.DiagramNumber = dtl.DiagramNumber;

                            if (dtl.MaterialResource != null)
                            {
                                string matCode = dtl.MaterialCode.Substring(0, 4);
                                DataDomain domain = (DataDomain)ht_matcat[matCode];
                                if (domain != null)
                                {
                                    MaterialCategory currCat = new MaterialCategory();
                                    currCat.Id = domain.Name1.ToString(); ;
                                    currCat.Name = domain.Name2.ToString();
                                    currCat.Version = 1;
                                    //planDtl.MaterialCategory = dtl.MaterialResource.Category;
                                    //planDtl.MaterialCategoryName = dtl.MaterialResource.Category.Name;
                                    planDtl.MaterialCategory = currCat;
                                    planDtl.MaterialCategoryName = currCat.Name;
                                }

                                planDtl.Manufacturer = dtl.MaterialResource.Factory;
                            }

                            planDtl.Quantity = dtl.PlanRequireQuantity;

                            planDtl.MatStandardUnit = dtl.QuantityUnitGUID;
                            planDtl.MatStandardUnitName = dtl.QuantityUnitName;

                            planDtl.ForwardDetailId = dtl.Id;

                            theNodePlanMaster.AddDetail(planDtl);

                            AddNodePlanDetailInGrid(planDtl, false, false);
                        }
                    }

                    #endregion
                }
                else if (optPlanType == RemandPlanType.��������ƻ�)
                {
                    #region ������������ƻ�

                    if (theLaborPlanMaster == null)
                    {
                        theLaborPlanMaster = new LaborDemandPlanMaster();
                        theLaborPlanMaster.Code = model.GetCode(typeof(LaborDemandPlanMaster), projectInfo.Id);
                        //������Ϣ
                        theLaborPlanMaster.CreatePerson = ConstObject.LoginPersonInfo;
                        theLaborPlanMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                        theLaborPlanMaster.CreateDate = serverTime;
                        theLaborPlanMaster.ReportTime = serverTime;
                        theLaborPlanMaster.CreateYear = serverTime.Year;
                        theLaborPlanMaster.CreateMonth = serverTime.Month;
                        theLaborPlanMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//��¼������
                        theLaborPlanMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                        theLaborPlanMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                        theLaborPlanMaster.HandlePerson = ConstObject.LoginPersonInfo;
                        theLaborPlanMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                        theLaborPlanMaster.DocState = DocumentState.Edit;

                        theLaborPlanMaster.ProjectId = projectInfo.Id;
                        theLaborPlanMaster.ProjectName = projectInfo.Name;

                        theLaborPlanMaster.ForwardBillType = frontBillType;
                        theLaborPlanMaster.ForwardBillId = frontBillId;
                        theLaborPlanMaster.ForwardBillCode = frontBillCode;

                        theLaborPlanMaster.RealOperationDate = serverTime;

                    }
                    //��ϸ��Ϣ
                    foreach (ResourceRequirePlanDetail dtl in listResult)
                    {
                        if (IsExistsPlanDetail(dtl) == false)
                        {
                            LaborDemandPlanDetail planDtl = new LaborDemandPlanDetail();

                            //planDtl.MaterialResource = dtl.MaterialResource;
                            //planDtl.MaterialCode = dtl.MaterialCode;
                            //planDtl.MaterialName = dtl.MaterialName;
                            //planDtl.MaterialSpec = dtl.MaterialSpec;

                            planDtl.LaborRankInTime = ClientUtil.ToDateTime(dtl.PlanBeginApproachDate);
                            planDtl.UsedRankType = ClientUtil.ToString(dtl.ServiceType);

                            planDtl.ProjectTask = dtl.TheGWBSTaskGUID;
                            planDtl.ProjectTaskName = dtl.TheGWBSTaskName;
                            planDtl.ProjectTaskSysCode = dtl.TheGWBSSysCode;

                            planDtl.Quantity = dtl.PlanRequireQuantity;

                            planDtl.ForwardDetailId = dtl.Id;

                            theLaborPlanMaster.AddDetail(planDtl);

                            AddServicePlanDetailInGrid(planDtl, false, false);
                        }
                    }
                    #endregion
                }
            }
        }

        private bool IsExistsPlanDetail(ResourceRequirePlanDetail dtl)
        {
            if (optPlanType == RemandPlanType.�����ܼƻ�)
            {
                foreach (DataGridViewRow row in gridMasterPlan.Rows)
                {
                    DemandMasterPlanDetail planDtl = row.Tag as DemandMasterPlanDetail;
                    if (planDtl.ForwardDetailId == dtl.Id)
                    {
                        return true;
                    }
                }
            }
            else if (optPlanType == RemandPlanType.�ճ�����ƻ�)
            {
                foreach (DataGridViewRow row in gridDayPlan.Rows)
                {
                    DailyPlanDetail planDtl = row.Tag as DailyPlanDetail;
                    if (planDtl.ForwardDetailId == dtl.Id)
                    {
                        return true;
                    }
                }
            }
            else if (optPlanType == RemandPlanType.�¶�����ƻ�)
            {
                foreach (DataGridViewRow row in gridMonthPlan.Rows)
                {
                    MonthlyPlanDetail planDtl = row.Tag as MonthlyPlanDetail;
                    if (planDtl.ForwardDetailId == dtl.Id)
                    {
                        return true;
                    }
                }
            }
            else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
            {
                foreach (DataGridViewRow row in gridNodePlan.Rows)
                {
                    MonthlyPlanDetail planDtl = row.Tag as MonthlyPlanDetail;
                    if (planDtl.ForwardDetailId == dtl.Id)
                    {
                        return true;
                    }
                }
            }
            else if (optPlanType == RemandPlanType.��������ƻ�)
            {
                foreach (DataGridViewRow row in gridServicePlan.Rows)
                {
                    LaborDemandPlanDetail planDtl = row.Tag as LaborDemandPlanDetail;
                    if (planDtl.ForwardDetailId == dtl.Id)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void LoadPlanDetail()
        {
            if (optPlanType == RemandPlanType.�����ܼƻ�)
            { }
            else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
            { }
            else if (optPlanType == RemandPlanType.�¶�����ƻ�)
            { }
            else if (optPlanType == RemandPlanType.�ճ�����ƻ�)
            {
            }
            else if (optPlanType == RemandPlanType.��������ƻ�)
            { }
        }

        private void AddMasterPlanDetailInGrid(DemandMasterPlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridMasterPlan.Rows.Add();
            DataGridViewRow row = gridMasterPlan.Rows[index];

            row.Cells[colSumResourceName.Name].Value = dtl.MaterialName;
            row.Cells[colSumResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[colSumSpec.Name].Value = dtl.MaterialSpec;

            row.Cells[colSumUsedPart.Name].Value = dtl.UsedPartName;
            row.Cells[colSumUsedPart.Name].Tag = dtl.UsedPart;

            //row.Cells[colSumUsedPart.Name].Value = dtl.UsedPartName;
            //row.Cells[colSumUsedPart.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.ProjectTaskName, dtl.ProjectTaskSysCode);

            row.Cells[colSumQuantity.Name].Value = dtl.Quantity;
            
            row.Cells[colSumQuantityUnit.Name].Value = dtl.MatStandardUnitName;
            row.Cells[colSumQuantityUnit.Name].Tag = dtl.MatStandardUnit;

            row.Cells[colSumRemark.Name].Value = dtl.Descript;

            row.Cells[colQuerlity.Name].Value = dtl.QualityStandard;

            row.Cells[colSumDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridMasterPlan.CurrentCell = row.Cells[0];
        }

        private void AddDayPlanDetailInGrid(DailyPlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridDayPlan.Rows.Add();
            DataGridViewRow row = gridDayPlan.Rows[index];

            row.Cells[colDayTaskName.Name].Value = dtl.UsedPartName;
            row.Cells[colDayTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.ProjectTaskName, dtl.ProjectTaskSysCode);

            row.Cells[colDayResourceName.Name].Value = dtl.MaterialName;
            row.Cells[colDayResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[colDaySpec.Name].Value = dtl.MaterialSpec;

            row.Cells[colDayUsedTeam.Name].Value = dtl.UsedRankName;
            row.Cells[colDayUsedTeam.Name].Tag = dtl.UsedRank;

            row.Cells[colDayQuantity.Name].Value = dtl.Quantity;
            row.Cells[colDayQuantityUnit.Name].Value = dtl.MatStandardUnitName;

            row.Cells[colEnterDate.Name].Value = dtl.ApproachDate;

            row.Cells[colDayRemark.Name].Value = dtl.Descript;

            row.Cells[colDayQuality.Name].Value = dtl.QualityStandard;

            row.Cells[colDayDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridDayPlan.CurrentCell = row.Cells[0];

        }

        private void AddMonthPlanDetailInGrid(MonthlyPlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridMonthPlan.Rows.Add();
            DataGridViewRow row = gridMonthPlan.Rows[index];

            row.Cells[colMonthTaskName.Name].Value = dtl.UsedPartName;
            row.Cells[colMonthTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.ProjectTaskName, dtl.ProjectTaskSysCode);

            row.Cells[colMonthResourceName.Name].Value = dtl.MaterialName;
            row.Cells[colMonthResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[colMonthMaterialSpec.Name].Value = dtl.MaterialSpec;

            row.Cells[colMonthUsedTeam.Name].Value = dtl.UsedRankName;
            row.Cells[colMonthUsedTeam.Name].Tag = dtl.UsedRank;

            row.Cells[colMonthQuantity.Name].Value = dtl.Quantity;
            row.Cells[colMonthQuantityUnit.Name].Value = dtl.MatStandardUnitName;

            row.Cells[colMonthRemark.Name].Value = dtl.Descript;
            //������׼
            row.Cells[colMonthQuality.Name].Value = dtl.QualityStandard;

            row.Cells[colMonthDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridMonthPlan.CurrentCell = row.Cells[0];

        }

        private void AddNodePlanDetailInGrid(MonthlyPlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridNodePlan.Rows.Add();
            DataGridViewRow row = gridNodePlan.Rows[index];

            row.Cells[colNodeTaskName.Name].Value = dtl.UsedPartName;
            row.Cells[colNodeTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.ProjectTaskName, dtl.ProjectTaskSysCode);

            row.Cells[colNodeResourceName.Name].Value = dtl.MaterialName;
            row.Cells[colNodeResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[colNodeMaterialSpec.Name].Value = dtl.MaterialSpec;

            row.Cells[colNodeUsedTeam.Name].Value = dtl.UsedRankName;
            row.Cells[colNodeUsedTeam.Name].Tag = dtl.UsedRank;

            row.Cells[colNodeQuantity.Name].Value = dtl.Quantity;

            row.Cells[colNodeQuantityUnit.Name].Value = dtl.MatStandardUnitName;

            row.Cells[colNodeRemark.Name].Value = dtl.Descript;

            row.Cells[colNodeQuality.Name].Value = dtl.QualityStandard;

            row.Cells[colNodeDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridNodePlan.CurrentCell = row.Cells[0];

        }

        private void AddServicePlanDetailInGrid(LaborDemandPlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridServicePlan.Rows.Add();
            DataGridViewRow row = gridServicePlan.Rows[index];

            row.Cells[colServiceTaskName.Name].Value = dtl.ProjectTaskName;
            row.Cells[colServiceTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.ProjectTaskName, dtl.ProjectTaskSysCode);

            row.Cells[colServiceBearType.Name].Value = dtl.UsedRankType;
            row.Cells[colServiceProjectQuantity.Name].Value = dtl.Quantity;
            row.Cells[colServiceProjectQnyUnit.Name].Value = dtl.ProjectQuantityUnitName;

            if (dtl.LaborRankInTime != null)
                row.Cells[colServiceBearIncomeDate.Name].Value = StaticMethod.GetShowDateTimeStr(dtl.LaborRankInTime.Value, false);
            row.Cells[colServiceMainWorkContent.Name].Value = dtl.MainJobDescript;
            row.Cells[colServiceQuanlityRequire.Name].Value = dtl.QualitySafetyRequirement;

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridServicePlan.CurrentCell = row.Cells[0];
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);
        }
        /// <summary>
        /// ����������,(����״̬�����¼������е�����)
        /// </summary>
        /// <param name="code"></param>
        public void Start(string code)
        {
            try
            {
                if (code == "��")
                    RefreshState(MainViewState.Browser);
            }
            catch (Exception e)
            {
                MessageBox.Show("��ͼ��������" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// ����������,(����״̬�����¼������е�����)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="GUID"></param>
        public void Start(RemandPlanType planType, string code, string GUID)
        {
            try
            {
                optPlanType = planType;

                if (optPlanType == RemandPlanType.�ճ�����ƻ�)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList list = model.ObjectQuery(typeof(DailyPlanMaster), oq);
                    if (list.Count > 0)
                    {
                        theDailyPlanMaster = list[0] as DailyPlanMaster;
                        ModelToView();
                        RefreshState(MainViewState.Browser);
                    }
                }
                else if (optPlanType == RemandPlanType.�¶�����ƻ�)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList list = model.ObjectQuery(typeof(MonthlyPlanMaster), oq);
                    if (list.Count > 0)
                    {
                        theMonthPlanMaster = list[0] as MonthlyPlanMaster;
                        ModelToView();
                        RefreshState(MainViewState.Browser);
                    }
                }
                else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList list = model.ObjectQuery(typeof(MonthlyPlanMaster), oq);
                    if (list.Count > 0)
                    {
                        theNodePlanMaster = list[0] as MonthlyPlanMaster;
                        ModelToView();
                        RefreshState(MainViewState.Browser);
                    }
                }
                else if (optPlanType == RemandPlanType.��������ƻ�)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList list = model.ObjectQuery(typeof(LaborDemandPlanMaster), oq);
                    if (list.Count > 0)
                    {
                        theLaborPlanMaster = list[0] as LaborDemandPlanMaster;
                        ModelToView();
                        RefreshState(MainViewState.Browser);
                    }
                }
                else if (optPlanType == RemandPlanType.�����ܼƻ�)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList list = model.ObjectQuery(typeof(DemandMasterPlanMaster), oq);
                    if (list.Count > 0)
                    {
                        theMasterPlanMaster = list[0] as DemandMasterPlanMaster;
                        ModelToView();
                        RefreshState(MainViewState.Browser);
                    }
                }

                if (optPlanType == RemandPlanType.�ճ�����ƻ�)
                {
                    if (tabControlPlan.TabPages.Contains(tabPage�ճ�����ƻ�) == false)
                        tabControlPlan.TabPages.Add(tabPage�ճ�����ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�ڵ�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�ڵ�����ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage��������ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage��������ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�����ܼƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�����ܼƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�¶�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�¶�����ƻ�);
                }
                else if (optPlanType == RemandPlanType.�¶�����ƻ�)
                {
                    if (tabControlPlan.TabPages.Contains(tabPage�ճ�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�ճ�����ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�ڵ�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�ڵ�����ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage��������ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage��������ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�����ܼƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�����ܼƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�¶�����ƻ�) == false)
                        tabControlPlan.TabPages.Add(tabPage�¶�����ƻ�);
                }
                else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
                {
                    if (tabControlPlan.TabPages.Contains(tabPage�ճ�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�ճ�����ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�ڵ�����ƻ�) == false)
                        tabControlPlan.TabPages.Add(tabPage�ڵ�����ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage��������ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage��������ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�����ܼƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�����ܼƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�¶�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�¶�����ƻ�);

                }
                else if (optPlanType == RemandPlanType.��������ƻ�)
                {
                    if (tabControlPlan.TabPages.Contains(tabPage�ճ�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�ճ�����ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�ڵ�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�ڵ�����ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage��������ƻ�) == false)
                        tabControlPlan.TabPages.Add(tabPage��������ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�����ܼƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�����ܼƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�¶�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�¶�����ƻ�);
                }
                else if (optPlanType == RemandPlanType.�����ܼƻ�)
                {
                    if (tabControlPlan.TabPages.Contains(tabPage�ճ�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�ճ�����ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�ڵ�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�ڵ�����ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage��������ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage��������ƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�����ܼƻ�) == false)
                        tabControlPlan.TabPages.Add(tabPage�����ܼƻ�);
                    if (tabControlPlan.TabPages.Contains(tabPage�¶�����ƻ�) == true)
                        tabControlPlan.TabPages.Remove(tabPage�¶�����ƻ�);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("��ͼ��������" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        //�������
        private void ClearView()
        {
            ClearControl(pnlFloor);
        }

        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }

            //�Զ���ؼ����
            if (c is CustomEdit || c is TextBox)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

        /// <summary>
        /// �½�
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();

                ClearView();

                theDailyPlanMaster = null;
                theLaborPlanMaster = null;
                theMasterPlanMaster = null;
                theMonthPlanMaster = null;
                theNodePlanMaster = null;

                RefreshControls(MainViewState.Modify);

                txtPlanName.Focus();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// �޸�
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            base.ModifyView();

            ClearView();

            if (optPlanType == RemandPlanType.�ڵ�����ƻ� && theNodePlanMaster.DocState == DocumentState.Edit)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", theNodePlanMaster.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                theNodePlanMaster = model.ObjectQuery(typeof(MonthlyPlanMaster), oq)[0] as MonthlyPlanMaster;

                ModelToView();
                RefreshControls(MainViewState.Modify);
                return true;
            }
            if (optPlanType == RemandPlanType.��������ƻ� && theLaborPlanMaster.DocState == DocumentState.Edit)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", theLaborPlanMaster.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                theLaborPlanMaster = model.ObjectQuery(typeof(LaborDemandPlanMaster), oq)[0] as LaborDemandPlanMaster;

                ModelToView();
                RefreshControls(MainViewState.Modify);
                return true;
            }
            if (optPlanType == RemandPlanType.�ճ�����ƻ� && theDailyPlanMaster.DocState == DocumentState.Edit)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", theDailyPlanMaster.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                theDailyPlanMaster = model.ObjectQuery(typeof(DailyPlanMaster), oq)[0] as DailyPlanMaster;

                ModelToView();
                RefreshControls(MainViewState.Modify);
                return true;
            }
            if (optPlanType == RemandPlanType.�����ܼƻ� && theMasterPlanMaster != null && theMasterPlanMaster.DocState == DocumentState.Edit)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", theMasterPlanMaster.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                theMasterPlanMaster = model.ObjectQuery(typeof(DemandMasterPlanMaster), oq)[0] as DemandMasterPlanMaster;

                ModelToView();
                RefreshControls(MainViewState.Modify);
                return true;
            }
            if (optPlanType == RemandPlanType.�¶�����ƻ� && theMonthPlanMaster.DocState == DocumentState.Edit)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", theMonthPlanMaster.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                theMonthPlanMaster = model.ObjectQuery(typeof(MonthlyPlanMaster), oq)[0] as MonthlyPlanMaster;

                ModelToView();
                RefreshControls(MainViewState.Modify);
                return true;
            }

            string message = "�˵�״̬Ϊ���ƶ�״̬�������޸ģ�";
            MessageBox.Show(message);
            return false;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                return this.SaveOrSubmitBill(1);
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// �ύ
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                return this.SaveOrSubmitBill(2);
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        //[optrType=1 ����][optrType=2 �ύ]
        private bool SaveOrSubmitBill(int optrType)
        {
            if (!ValidView())
                return false;

            LogData log = new LogData();


            if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
            {
                if (optrType == 2)
                {
                    theNodePlanMaster.DocState = DocumentState.InAudit;
                }
                if (string.IsNullOrEmpty(theNodePlanMaster.Id))
                {
                    if (optrType == 2)
                    {
                        log.OperType = "�����ύ";
                    }
                    else
                    {
                        log.OperType = "��������";
                    }
                }
                else
                {
                    if (optrType == 2)
                    {
                        log.OperType = "�޸��ύ";
                    }
                    else
                    {
                        log.OperType = "�޸ı���";
                    }
                }
                foreach (DataGridViewRow var in this.gridNodePlan.Rows)
                {
                    if (var.IsNewRow) break;
                    MonthlyPlanDetail curBillDtl = new MonthlyPlanDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as MonthlyPlanDetail;
                        if (curBillDtl.Id == null)
                        {
                            theNodePlanMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.QualityStandard = ClientUtil.ToString(var.Cells[colNodeQuality.Name].Value);
                    theNodePlanMaster.AddDetail(curBillDtl);
                }


                theNodePlanMaster = MDailyPlan.DailyPlanSrv.SaveOrUpdateByDao(theNodePlanMaster) as MonthlyPlanMaster;

                log.BillId = theNodePlanMaster.Id;
                log.BillType = optPlanType + "��";
                log.Code = theNodePlanMaster.Code;
                log.Descript = "";
                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                log.ProjectName = theNodePlanMaster.ProjectName;

                this.ViewCaption = ViewName + "-" + theNodePlanMaster.Code;
            }
            else if (optPlanType == RemandPlanType.�¶�����ƻ�)
            {
                if (optrType == 2)
                {
                    theMonthPlanMaster.DocState = DocumentState.InAudit;
                }
                if (string.IsNullOrEmpty(theMonthPlanMaster.Id))
                {
                    if (optrType == 2)
                    {
                        log.OperType = "�����ύ";
                    }
                    else
                    {
                        log.OperType = "��������";
                    }
                }
                else
                {
                    if (optrType == 2)
                    {
                        log.OperType = "�޸��ύ";
                    }
                    else
                    {
                        log.OperType = "�޸ı���";
                    }
                }
                foreach (DataGridViewRow var in this.gridMonthPlan.Rows)
                {
                    if (var.IsNewRow) break;
                    MonthlyPlanDetail curBillDtl = new MonthlyPlanDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as MonthlyPlanDetail;
                        if (curBillDtl.Id == null)
                        {
                            theMonthPlanMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.UsedRank = var.Cells[colMonthUsedTeam.Name].Tag as Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass.SupplierRelationInfo;
                    curBillDtl.UsedRankName = ClientUtil.ToString(var.Cells[colMonthUsedTeam.Name].Value);
                    curBillDtl.QualityStandard = ClientUtil.ToString(var.Cells[colMonthQuality.Name].Value);
                    theMonthPlanMaster.AddDetail(curBillDtl);
                }

                theMonthPlanMaster = MDailyPlan.DailyPlanSrv.SaveOrUpdateByDao(theMonthPlanMaster) as MonthlyPlanMaster;

                log.BillId = theMonthPlanMaster.Id;
                log.BillType = optPlanType + "��";
                log.Code = theMonthPlanMaster.Code;
                log.Descript = "";
                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                log.ProjectName = theMonthPlanMaster.ProjectName;

                this.ViewCaption = ViewName + "-" + theMonthPlanMaster.Code;
            }

            else if (optPlanType == RemandPlanType.��������ƻ�)
            {
                if (optrType == 2)
                {
                    theLaborPlanMaster.DocState = DocumentState.InAudit;
                }
                if (string.IsNullOrEmpty(theLaborPlanMaster.Id))
                {
                    if (optrType == 2)
                    {
                        log.OperType = "�����ύ";
                    }
                    else
                    {
                        log.OperType = "��������";
                    }
                }
                else
                {
                    if (optrType == 2)
                    {
                        log.OperType = "�޸��ύ";
                    }
                    else
                    {
                        log.OperType = "�޸ı���";
                    }
                }

                theLaborPlanMaster = MDailyPlan.DailyPlanSrv.SaveOrUpdateByDao(theLaborPlanMaster) as LaborDemandPlanMaster;

                log.BillId = theLaborPlanMaster.Id;
                log.BillType = optPlanType + "��";
                log.Code = theLaborPlanMaster.Code;
                log.Descript = "";
                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                log.ProjectName = theLaborPlanMaster.ProjectName;

                this.ViewCaption = ViewName + "-" + theLaborPlanMaster.Code;
            }
            else if (optPlanType == RemandPlanType.�ճ�����ƻ�)
            {
                if (optrType == 2)
                {
                    theDailyPlanMaster.DocState = DocumentState.InAudit;
                }
                if (string.IsNullOrEmpty(theDailyPlanMaster.Id))
                {
                    if (optrType == 2)
                    {
                        log.OperType = "�����ύ";
                    }
                    else
                    {
                        log.OperType = "��������";
                    }
                }
                else
                {
                    if (optrType == 2)
                    {
                        log.OperType = "�޸��ύ";
                    }
                    else
                    {
                        log.OperType = "�޸ı���";
                    }
                }
                //theDailyPlanMaster.Details
                foreach (DataGridViewRow var in this.gridDayPlan.Rows)
                {
                    if (var.IsNewRow) break;
                    DailyPlanDetail curBillDtl = new DailyPlanDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as DailyPlanDetail;
                        if (curBillDtl.Id == null)
                        {
                            theDailyPlanMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.UsedRank = var.Cells[colDayUsedTeam.Name].Tag as Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass.SupplierRelationInfo;
                    curBillDtl.UsedRankName = ClientUtil.ToString(var.Cells[colDayUsedTeam.Name].Value);
                    curBillDtl.QualityStandard = ClientUtil.ToString(var.Cells[colDayQuantity.Name].Value);
                    if (var.Cells[colEnterDate.Name].Value != null)
                    {
                        curBillDtl.ApproachDate = ClientUtil.ToDateTime(var.Cells[colEnterDate.Name].Value);
                    }
                    theDailyPlanMaster.AddDetail(curBillDtl);
                }


                theDailyPlanMaster = MDailyPlan.DailyPlanSrv.SaveOrUpdateByDao(theDailyPlanMaster) as DailyPlanMaster;

                log.BillId = theDailyPlanMaster.Id;
                log.BillType = optPlanType + "��";
                log.Code = theDailyPlanMaster.Code;
                log.Descript = "";
                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                log.ProjectName = theDailyPlanMaster.ProjectName;

                this.ViewCaption = ViewName + "-" + theDailyPlanMaster.Code;
            }
            else if (optPlanType == RemandPlanType.�����ܼƻ�)
            {
                if (optrType == 2)
                {
                    theMasterPlanMaster.DocState = DocumentState.InAudit;
                }
                if (string.IsNullOrEmpty(theMasterPlanMaster.Id))
                {
                    if (optrType == 2)
                    {
                        log.OperType = "�����ύ";
                    }
                    else
                    {
                        log.OperType = "��������";
                    }
                }
                else
                {
                    if (optrType == 2)
                    {
                        log.OperType = "�޸��ύ";
                    }
                    else
                    {
                        log.OperType = "�޸ı���";
                    }
                }
                foreach (DataGridViewRow var in this.gridMasterPlan.Rows)
                {
                    if (var.IsNewRow) break;
                    DemandMasterPlanDetail curBillDtl = new DemandMasterPlanDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as DemandMasterPlanDetail;
                        if (curBillDtl.Id == null)
                        {
                            theMasterPlanMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.QualityStandard = ClientUtil.ToString(var.Cells[colQuerlity.Name].Value);
                    theMasterPlanMaster.AddDetail(curBillDtl);
                }


                theMasterPlanMaster = MDailyPlan.DailyPlanSrv.SaveOrUpdateByDao(theMasterPlanMaster) as DemandMasterPlanMaster;

                log.BillId = theMasterPlanMaster.Id;
                log.BillType = optPlanType + "��";
                log.Code = theMasterPlanMaster.Code;
                log.Descript = "";
                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                log.ProjectName = theMasterPlanMaster.ProjectName;

                this.ViewCaption = ViewName + "-" + theMasterPlanMaster.Code;
            }

            StaticMethod.InsertLogData(log);


            return true;
        }

        /// <summary>
        /// ��������ǰУ������
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            string planName = txtPlanName.Text.Trim();
            if (planName == "")
            {
                MessageBox.Show("�����롰" + cbPlanType.SelectedItem.ToString() + "�����ƣ�");
                txtPlanName.Focus();
                return false;
            }
            string planDesc = this.txtPlanDesc.Text.Trim();

            if (optPlanType == RemandPlanType.�ճ�����ƻ�)
            {
                theDailyPlanMaster.PlanName = planName;
                theDailyPlanMaster.Descript = planDesc;
                theDailyPlanMaster.Special = "����";
            }
            else if (optPlanType == RemandPlanType.�¶�����ƻ�)
            {
                theMonthPlanMaster.PlanName = planName;
                theMonthPlanMaster.Descript = planDesc;
                theMonthPlanMaster.Special = "����";
            }
            else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
            {
                theNodePlanMaster.PlanName = planName;
                theNodePlanMaster.Descript = planDesc;
                theNodePlanMaster.Special = "����";
            }
            else if (optPlanType == RemandPlanType.��������ƻ�)
            {
                theLaborPlanMaster.PlanName = planName;
                theLaborPlanMaster.Descript = planDesc;
            }
            else if (optPlanType == RemandPlanType.�����ܼƻ�)
            {
                theMasterPlanMaster.PlanName = planName;
                theMasterPlanMaster.Descript = planDesc;
                theMasterPlanMaster.Special = "����";
            }

            return true;
        }

        //��ʾ����
        private bool ModelToView()
        {
            try
            {
                if (optPlanType == RemandPlanType.�ճ�����ƻ�)
                {
                    txtPlanName.Text = theDailyPlanMaster.PlanName;
                    txtPlanDesc.Text = theDailyPlanMaster.Descript;

                    if (theDailyPlanMaster.Details.Count > 0)
                    {
                        foreach (DailyPlanDetail dtl in theDailyPlanMaster.Details)
                        {
                            AddDayPlanDetailInGrid(dtl, false, false);
                        }
                    }
                }
                else if (optPlanType == RemandPlanType.�¶�����ƻ�)
                {
                    txtPlanName.Text = theMonthPlanMaster.PlanName;
                    txtPlanDesc.Text = theMonthPlanMaster.Descript;

                    if (theMonthPlanMaster.Details.Count > 0)
                    {
                        foreach (MonthlyPlanDetail dtl in theMonthPlanMaster.Details)
                        {
                            AddMonthPlanDetailInGrid(dtl, false, false);
                        }
                    }
                }
                else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
                {
                    txtPlanName.Text = theNodePlanMaster.PlanName;
                    txtPlanDesc.Text = theNodePlanMaster.Descript;

                    if (theNodePlanMaster.Details.Count > 0)
                    {
                        foreach (MonthlyPlanDetail dtl in theNodePlanMaster.Details)
                        {
                            AddNodePlanDetailInGrid(dtl, false, false);
                        }
                    }
                }
                else if (optPlanType == RemandPlanType.��������ƻ�)
                {
                    txtPlanName.Text = theLaborPlanMaster.PlanName;
                    txtPlanDesc.Text = theLaborPlanMaster.Descript;

                    if (theLaborPlanMaster.Details.Count > 0)
                    {
                        foreach (LaborDemandPlanDetail dtl in theLaborPlanMaster.Details)
                        {
                            AddServicePlanDetailInGrid(dtl, false, false);
                        }
                    }
                }
                else if (optPlanType == RemandPlanType.�����ܼƻ�)
                {
                    txtPlanName.Text = theMasterPlanMaster.PlanName;
                    txtPlanDesc.Text = theMasterPlanMaster.Descript;

                    if (theMasterPlanMaster.Details.Count > 0)
                    {
                        foreach (DemandMasterPlanDetail dtl in theMasterPlanMaster.Details)
                        {
                            AddMasterPlanDetailInGrid(dtl, false, false);
                        }
                    }
                }


                return true;
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("����ӳ�����" + StaticMethod.ExceptionMessage(e));
                //return false;
            }
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                if (optPlanType == RemandPlanType.�ճ�����ƻ�)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theDailyPlanMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    IList list = model.ObjectQuery(typeof(DailyPlanMaster), oq);
                    theDailyPlanMaster = list[0] as DailyPlanMaster;

                    if (theDailyPlanMaster.DocState == DocumentState.Edit)
                    {
                        if (!MDailyPlan.DailyPlanSrv.DeleteByDao(theDailyPlanMaster))
                            return false;

                        LogData log = new LogData();
                        log.BillId = theDailyPlanMaster.Id;
                        log.BillType = optPlanType + "��";
                        log.Code = theDailyPlanMaster.Code;
                        log.OperType = "ɾ��";
                        log.Descript = "";
                        log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = theDailyPlanMaster.ProjectName;
                        StaticMethod.InsertLogData(log);

                        ClearView();
                        return true;
                    }
                    MessageBox.Show("�˵�״̬Ϊ��" + ClientUtil.GetDocStateName(theDailyPlanMaster.DocState) + "��������ɾ����");
                }
                else if (optPlanType == RemandPlanType.�¶�����ƻ�)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theMonthPlanMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    IList list = model.ObjectQuery(typeof(MonthlyPlanMaster), oq);
                    theMonthPlanMaster = list[0] as MonthlyPlanMaster;


                    if (theMonthPlanMaster.DocState == DocumentState.Edit)
                    {
                        if (!MDailyPlan.DailyPlanSrv.DeleteByDao(theMonthPlanMaster))
                            return false;

                        LogData log = new LogData();
                        log.BillId = theMonthPlanMaster.Id;
                        log.BillType = optPlanType + "��";
                        log.Code = theMonthPlanMaster.Code;
                        log.OperType = "ɾ��";
                        log.Descript = "";
                        log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = theMonthPlanMaster.ProjectName;
                        StaticMethod.InsertLogData(log);

                        ClearView();
                        return true;
                    }
                    MessageBox.Show("�˵�״̬Ϊ��" + ClientUtil.GetDocStateName(theMonthPlanMaster.DocState) + "��������ɾ����");
                }
                else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theNodePlanMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    IList list = model.ObjectQuery(typeof(MonthlyPlanMaster), oq);
                    theNodePlanMaster = list[0] as MonthlyPlanMaster;


                    if (theNodePlanMaster.DocState == DocumentState.Edit)
                    {
                        if (!MDailyPlan.DailyPlanSrv.DeleteByDao(theNodePlanMaster))
                            return false;

                        LogData log = new LogData();
                        log.BillId = theNodePlanMaster.Id;
                        log.BillType = optPlanType + "��";
                        log.Code = theNodePlanMaster.Code;
                        log.OperType = "ɾ��";
                        log.Descript = "";
                        log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = theNodePlanMaster.ProjectName;
                        StaticMethod.InsertLogData(log);

                        ClearView();
                        return true;
                    }
                    MessageBox.Show("�˵�״̬Ϊ��" + ClientUtil.GetDocStateName(theNodePlanMaster.DocState) + "��������ɾ����");
                }
                else if (optPlanType == RemandPlanType.��������ƻ�)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theLaborPlanMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    IList list = model.ObjectQuery(typeof(LaborDemandPlanMaster), oq);
                    theLaborPlanMaster = list[0] as LaborDemandPlanMaster;

                    if (theLaborPlanMaster.DocState == DocumentState.Edit)
                    {
                        if (!MDailyPlan.DailyPlanSrv.DeleteByDao(theLaborPlanMaster))
                            return false;

                        LogData log = new LogData();
                        log.BillId = theLaborPlanMaster.Id;
                        log.BillType = optPlanType + "��";
                        log.Code = theLaborPlanMaster.Code;
                        log.OperType = "ɾ��";
                        log.Descript = "";
                        log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = theLaborPlanMaster.ProjectName;
                        StaticMethod.InsertLogData(log);

                        ClearView();
                        return true;
                    }
                    MessageBox.Show("�˵�״̬Ϊ��" + ClientUtil.GetDocStateName(theLaborPlanMaster.DocState) + "��������ɾ����");
                }
                else if (optPlanType == RemandPlanType.�����ܼƻ�)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theMasterPlanMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    IList list = model.ObjectQuery(typeof(DemandMasterPlanMaster), oq);
                    theMasterPlanMaster = list[0] as DemandMasterPlanMaster;
                    if (theMasterPlanMaster.DocState == DocumentState.Edit)
                    {
                        if (!MDailyPlan.DailyPlanSrv.DeleteByDao(theMasterPlanMaster))
                            return false;

                        LogData log = new LogData();
                        log.BillId = theMasterPlanMaster.Id;
                        log.BillType = optPlanType + "��";
                        log.Code = theMasterPlanMaster.Code;
                        log.OperType = "ɾ��";
                        log.Descript = "";
                        log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = theMasterPlanMaster.ProjectName;
                        StaticMethod.InsertLogData(log);

                        ClearView();
                        return true;
                    }
                    MessageBox.Show("�˵�״̬Ϊ��" + ClientUtil.GetDocStateName(theMasterPlanMaster.DocState) + "��������ɾ����");
                }

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("����ɾ������" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                ClearView();

                switch (ViewState)
                {
                    case MainViewState.AddNew:
                        theDailyPlanMaster = null;
                        theLaborPlanMaster = null;
                        theMasterPlanMaster = null;
                        theMonthPlanMaster = null;
                        theNodePlanMaster = null;
                        break;
                    case MainViewState.Modify:

                        if (optPlanType == RemandPlanType.�ճ�����ƻ�)
                        {
                            if (!string.IsNullOrEmpty(theDailyPlanMaster.Id))
                            {
                                //������������
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Id", theDailyPlanMaster.Id));
                                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                                theDailyPlanMaster = model.ObjectQuery(typeof(DailyPlanMaster), oq)[0] as DailyPlanMaster;

                                ModelToView();
                            }
                        }
                        else if (optPlanType == RemandPlanType.�¶�����ƻ�)
                        {
                            if (!string.IsNullOrEmpty(theMonthPlanMaster.Id))
                            {
                                //������������
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Id", theMonthPlanMaster.Id));
                                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                                theMonthPlanMaster = model.ObjectQuery(typeof(MonthlyPlanMaster), oq)[0] as MonthlyPlanMaster;

                                ModelToView();
                            }
                        }
                        else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
                        {
                            if (!string.IsNullOrEmpty(theNodePlanMaster.Id))
                            {
                                //������������
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Id", theNodePlanMaster.Id));
                                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                                theNodePlanMaster = model.ObjectQuery(typeof(MonthlyPlanMaster), oq)[0] as MonthlyPlanMaster;

                                ModelToView();
                            }
                        }
                        else if (optPlanType == RemandPlanType.��������ƻ�)
                        {
                            if (!string.IsNullOrEmpty(theLaborPlanMaster.Id))
                            {
                                //������������
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Id", theLaborPlanMaster.Id));
                                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                                theLaborPlanMaster = model.ObjectQuery(typeof(LaborDemandPlanMaster), oq)[0] as LaborDemandPlanMaster;

                                ModelToView();
                            }
                        }
                        else if (optPlanType == RemandPlanType.�����ܼƻ�)
                        {
                            if (!string.IsNullOrEmpty(theMasterPlanMaster.Id))
                            {
                                //������������
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Id", theMasterPlanMaster.Id));
                                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                                theMasterPlanMaster = model.ObjectQuery(typeof(DemandMasterPlanMaster), oq)[0] as DemandMasterPlanMaster;

                                ModelToView();
                            }
                        }

                        break;
                    default:
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݳ�������" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// ˢ��
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                if (ViewState == MainViewState.Modify)
                {
                    if (MessageBox.Show("��ǰ���㵥���ڱ༭״̬����Ҫ�����޸���", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!SaveView())
                        {
                            return;
                        }
                    }
                }

                if (optPlanType == RemandPlanType.�ճ�����ƻ�)
                {
                    //���¼�������
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theDailyPlanMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    theDailyPlanMaster = model.ObjectQuery(typeof(DailyPlanMaster), oq)[0] as DailyPlanMaster;
                }
                else if (optPlanType == RemandPlanType.�¶�����ƻ�)
                {
                    //���¼�������
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theMonthPlanMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    theMonthPlanMaster = model.ObjectQuery(typeof(MonthlyPlanMaster), oq)[0] as MonthlyPlanMaster;
                }
                else if (optPlanType == RemandPlanType.�ڵ�����ƻ�)
                {
                    //���¼�������
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theNodePlanMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    theNodePlanMaster = model.ObjectQuery(typeof(MonthlyPlanMaster), oq)[0] as MonthlyPlanMaster;
                }
                else if (optPlanType == RemandPlanType.��������ƻ�)
                {
                    //���¼�������
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theLaborPlanMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    theLaborPlanMaster = model.ObjectQuery(typeof(LaborDemandPlanMaster), oq)[0] as LaborDemandPlanMaster;
                }
                else if (optPlanType == RemandPlanType.�����ܼƻ�)
                {
                    //���¼�������
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theMasterPlanMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    theMasterPlanMaster = model.ObjectQuery(typeof(DemandMasterPlanMaster), oq)[0] as DemandMasterPlanMaster;
                }


                ModelToView();

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception e)
            {
                MessageBox.Show("����ˢ�´���" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:
                    cbPlanType.Enabled = true;
                    btnSelectScrollPlan.Enabled = true;

                    txtPlanName.ReadOnly = false;
                    txtPlanDesc.ReadOnly = false;

                    tabControlPlan.Enabled = true;
                    break;

                case MainViewState.Browser:
                    cbPlanType.Enabled = false;
                    btnSelectScrollPlan.Enabled = false;

                    txtPlanName.ReadOnly = true;
                    txtPlanDesc.ReadOnly = true;

                    tabControlPlan.Enabled = false;
                    break;
            }
        }
    }
}
