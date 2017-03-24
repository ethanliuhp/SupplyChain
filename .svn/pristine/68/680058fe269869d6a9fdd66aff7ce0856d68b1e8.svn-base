using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng
{
    public partial class VEditCostSubjectQuota : Form
    {
        private SubjectCostQuota _optCostQuota = null;
        /// <summary>
        /// 操作分科目成本定额
        /// </summary>
        public SubjectCostQuota OptCostQuota
        {
            get { return _optCostQuota; }
            set { _optCostQuota = value; }
        }

        private CostItem _optCostItem = null;
        /// <summary>
        /// 操作成本项
        /// </summary>
        public CostItem OptCostItem
        {
            get { return _optCostItem; }
            set { _optCostItem = value; }
        }

        public MCostItem model;

        public VEditCostSubjectQuota(MCostItem mot)
        {
            model = mot;
            InitializeComponent();
            InitialEvents();
        }

        private void InitialEvents()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSaveAndExit.Click += new EventHandler(btnSaveAndExit_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            txtQuantityUnit.LostFocus += new EventHandler(txtQuantityUnit_LostFocus);
            txtPriceUnit.LostFocus += new EventHandler(txtPriceUnit_LostFocus);

            btnSelectProjectUnit.Click += new EventHandler(btnSelectProjectUnit_Click);
            btnSelectPriceUnit.Click += new EventHandler(btnSelectPriceUnit_Click);

            btnSelectResource.Click += new EventHandler(btnSelectResource_Click);
            btnRemoveResourceType.Click += new EventHandler(btnRemoveResourceType_Click);

            btnSelectSubject.Click += new EventHandler(btnSelectSubject_Click);

            txtQuotaQunatity.TextChanged += new EventHandler(txtQuotaQunatity_TextChanged);
            txtQuantityPrice.TextChanged += new EventHandler(txtQuantityPrice_TextChanged);


            this.Load += new EventHandler(VEditCostSubjectQuota_Load);

            this.gridResourceGroup.CellEndEdit += new DataGridViewCellEventHandler(gridResourceGroup_CellEndEdit);
        }
        /// <summary>
        /// 编辑图号信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridResourceGroup_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridResourceGroup.Rows[e.RowIndex].Cells[DiagramNumber.Name].ColumnIndex)
            {
                Material m = gridResourceGroup.Rows[e.RowIndex].Tag as Material;
                if (gridResourceGroup.Rows[e.RowIndex].Cells[DiagramNumber.Name].Value != null)
                {
                    string s = gridResourceGroup.Rows[e.RowIndex].Cells[DiagramNumber.Name].Value.ToString();
                    if (_optCostQuota != null)
                    {
                        for (int i = _optCostQuota.ListResources.Count - 1; i > -1; i--)
                        {
                            ResourceGroup rg = _optCostQuota.ListResources.ElementAt(i);
                            if (rg.ResourceTypeGUID == m.Id && rg.DiagramNumber != s)
                            {
                                rg.DiagramNumber = s;
                            }
                        }

                    }
                    m.AssistantCode2 = s;
                    gridResourceGroup.Rows[e.RowIndex].Tag = m;
                }
            }
        }

        void txtPriceUnit_LostFocus(object sender, EventArgs e)
        {
            txtQuantityUnit.LostFocus -= new EventHandler(txtQuantityUnit_LostFocus);
            SetStandardUnit(sender);
            txtQuantityUnit.LostFocus += new EventHandler(txtQuantityUnit_LostFocus);
        }

        void txtQuantityUnit_LostFocus(object sender, EventArgs e)
        {
            txtPriceUnit.LostFocus -= new EventHandler(txtPriceUnit_LostFocus);
            SetStandardUnit(sender);
            txtPriceUnit.LostFocus += new EventHandler(txtPriceUnit_LostFocus);
        }

        private void SetStandardUnit(object sender)
        {
            TextBox tbUnit = sender as TextBox;
            string name = tbUnit.Text.Trim();
            if (name != "")
            {
                if (tbUnit.Tag == null || (tbUnit.Tag != null && (tbUnit.Tag as StandardUnit).Name != name))
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", name));
                    IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                    if (list.Count > 0)
                    {
                        tbUnit.Tag = list[0] as StandardUnit;
                    }
                    else
                    {
                        MessageBox.Show("系统目前不存在该计量单位，请检查！");
                        SelectUnit(tbUnit);
                    }
                }
            }
        }

        void txtQuantityPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtQuantityPrice.Text.Trim() != "" && txtQuotaQunatity.Text.Trim() != "")
            {
                try
                {
                    txtWorkAmountPrice.Text = decimal.Round(Convert.ToDecimal(txtQuantityPrice.Text) * Convert.ToDecimal(txtQuotaQunatity.Text), 5).ToString();
                }
                catch
                {
                    MessageBox.Show("数量单价填写格式不正确，请检查！");
                    txtQuantityPrice.Focus();
                }
            }
        }

        void txtQuotaQunatity_TextChanged(object sender, EventArgs e)
        {
            if (txtQuantityPrice.Text.Trim() != "" && txtQuotaQunatity.Text.Trim() != "")
            {
                try
                {
                    txtWorkAmountPrice.Text = decimal.Round(Convert.ToDecimal(txtQuantityPrice.Text) * Convert.ToDecimal(txtQuotaQunatity.Text), 5).ToString();
                }
                catch
                {
                    MessageBox.Show("定额数量填写格式不正确，请检查！");
                    txtQuotaQunatity.Focus();
                }
            }
        }

        void VEditCostSubjectQuota_Load(object sender, EventArgs e)
        {
            if (OptCostQuota != null)
            {
                if (!string.IsNullOrEmpty(OptCostQuota.Id))
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", OptCostQuota.Id));
                    oq.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);

                    OptCostQuota = model.ObjectQuery(typeof(SubjectCostQuota), oq)[0] as SubjectCostQuota;
                }

                txtState.Text = OptCostQuota.State.ToString();

                txtName.Text = OptCostQuota.Name;

                txtAccountSubject.Tag = OptCostQuota.CostAccountSubjectGUID;
                txtAccountSubject.Text = OptCostQuota.CostAccountSubjectName;

                txtQuotaQunatity.Text = OptCostQuota.QuotaProjectAmount.ToString();
                txtQuantityPrice.Text = OptCostQuota.QuotaPrice.ToString();
                txtWorkAmountPrice.Text = OptCostQuota.QuotaMoney.ToString();
                txtWastage.Text = OptCostQuota.Wastage.ToString();

                txtQuantityUnit.Text = OptCostQuota.ProjectAmountUnitName;
                txtQuantityUnit.Tag = OptCostQuota.ProjectAmountUnitGUID;

                txtPriceUnit.Text = OptCostQuota.PriceUnitName;
                txtPriceUnit.Tag = OptCostQuota.PriceUnitGUID;

                txtAssessmentRate.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DecimelTrimEnd0(OptCostQuota.AssessmentRate * 100);

                cbMainResourceFlag.Checked = OptCostQuota.MainResourceFlag;

                List<ResourceGroup> list = OptCostQuota.ListResources.ToList<ResourceGroup>();
                if (list.Count > 0)
                {
                    gridResourceGroup.Rows.Clear();
                    List<Material> listResourceType = new List<Material>();
                    foreach (ResourceGroup item in list)
                    {
                        Material mat = model.GetObjectById(typeof(Material), item.ResourceTypeGUID) as Material;

                        if (mat != null)
                        {

                            int index = gridResourceGroup.Rows.Add();
                            DataGridViewRow row = gridResourceGroup.Rows[index];
                            row.Cells[ResourceName.Name].Value = mat.Name;
                            row.Cells[ResourceSpec.Name].Value = mat.Specification;
                            row.Cells[ResourceQuality.Name].Value = mat.Quality;
                            row.Cells[DiagramNumber.Name].Value = item.DiagramNumber;
                            row.Tag = mat;

                            listResourceType.Add(mat);
                        }
                    }

                    gridResourceGroup.Tag = listResourceType;

                }
            }
            else
            {
                txtState.Text = SubjectCostQuotaState.编制.ToString();

                if (OptCostItem != null)
                {
                    txtQuantityUnit.Text = OptCostItem.ProjectUnitName;
                    txtQuantityUnit.Tag = OptCostItem.ProjectUnitGUID;

                    txtPriceUnit.Text = OptCostItem.PriceUnitName;
                    txtPriceUnit.Tag = OptCostItem.PriceUnitGUID;
                }
            }
        }

        //选择核算科目
        void btnSelectSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.ShowDialog();
            if (frm.SelectAccountSubject != null)
            {
                txtAccountSubject.Text = frm.SelectAccountSubject.Name;
                txtAccountSubject.Tag = frm.SelectAccountSubject;
            }
        }
        //选择资源
        void btnSelectResource_Click(object sender, EventArgs e)
        {
            CommonMaterial materialSelector = new CommonMaterial();
            materialSelector.OpenSelect();

            IList list = materialSelector.Result;
            if (list.Count > 0)
            {
                List<Material> listResourceType = gridResourceGroup.Tag as List<Material>;

                if (listResourceType != null)
                {
                    foreach (Material mat in list)
                    {
                        var query = from m in listResourceType
                                    where m.Id == mat.Id
                                    select m;

                        if (query.Count() == 0)
                            listResourceType.Add(mat);
                    }
                }
                else
                    listResourceType = list.OfType<Material>().ToList();

                gridResourceGroup.Rows.Clear();
                foreach (Material mat in listResourceType)
                {
                    int index = gridResourceGroup.Rows.Add();
                    DataGridViewRow row = gridResourceGroup.Rows[index];
                    row.Cells[ResourceName.Name].Value = mat.Name;
                    row.Cells[ResourceSpec.Name].Value = mat.Specification;
                    row.Cells[ResourceQuality.Name].Value = mat.Quality;

                    row.Tag = mat;
                }

                gridResourceGroup.Tag = listResourceType;

            }
        }
        void btnRemoveResourceType_Click(object sender, EventArgs e)
        {
            if (gridResourceGroup.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要移除的资源类型！");
                return;
            }

            List<int> listIndex = new List<int>();
            foreach (DataGridViewRow row in gridResourceGroup.SelectedRows)
            {
                listIndex.Add(row.Index);
            }
            listIndex.Sort();

            //List<Material> listResourceType = (gridResourceGroup.Tag as IList).OfType<Material>().ToList();
            List<Material> listResourceType = gridResourceGroup.Tag as List<Material>;
            if (listResourceType != null)
            {
                for (int i = listIndex.Count - 1; i > -1; i--)
                {
                    int rowIndex = listIndex[i];
                    Material resourceType = gridResourceGroup.Rows[rowIndex].Tag as Material;
                    if (resourceType != null)
                    {
                        var query = from rt in listResourceType
                                    where rt.Id == resourceType.Id
                                    select rt;
                        if (query.Count() > 0)
                            listResourceType.Remove(query.ElementAt(0));


                        gridResourceGroup.Rows.RemoveAt(rowIndex);

                    }
                }
                gridResourceGroup.Tag = listResourceType;
            }
        }
        //选择工程量计量单位
        void btnSelectProjectUnit_Click(object sender, EventArgs e)
        {
            SelectUnit(txtQuantityUnit);
        }
        //选择价格计量单位
        void btnSelectPriceUnit_Click(object sender, EventArgs e)
        {
            SelectUnit(txtPriceUnit);
        }
        private void SelectUnit(TextBox txt)
        {
            StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
            if (su != null)
            {
                txt.Tag = su;
                txt.Text = su.Name;
                txt.Focus();
            }
        }

        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValideSave())
                    return;
                if (string.IsNullOrEmpty(_optCostQuota.Id))
                {
                    _optCostQuota.State = SubjectCostQuotaState.编制;

                    if (!string.IsNullOrEmpty(OptCostItem.Id))
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", OptCostItem.Id));
                        oq.AddFetchMode("ListQuotas", NHibernate.FetchMode.Eager);
                        IList list = model.ObjectQuery(typeof(CostItem), oq);
                        OptCostItem = list[0] as CostItem;
                    }

                    if (_optCostQuota.TheCostItem == null)
                    {
                        _optCostQuota.TheCostItem = OptCostItem;
                        OptCostItem.ListQuotas.Add(_optCostQuota);
                    }

                    if (!string.IsNullOrEmpty(OptCostItem.Id))//当成本项存在时才保存明细，否则在保存成本项时一起保存明细
                    {
                        IList listTemp = new ArrayList();
                        listTemp.Add(OptCostItem);
                        listTemp = model.SaveOrUpdateCostItem(listTemp);
                        OptCostItem = listTemp[0] as CostItem;

                        _optCostQuota = OptCostItem.ListQuotas.ElementAt(OptCostItem.ListQuotas.Count - 1);
                    }
                }
                else
                {
                    IList listTemp = new ArrayList();
                    listTemp.Add(_optCostQuota);
                    listTemp = model.SaveOrUpdateCostItemQuota(listTemp);
                    _optCostQuota = listTemp[0] as SubjectCostQuota;
                }

                gridResourceGroup.Tag = _optCostQuota.ListResources.ToList();

                MessageBox.Show("保存成功！");
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }
        //保存并退出
        void btnSaveAndExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValideSave())
                    return;
                if (string.IsNullOrEmpty(_optCostQuota.Id))
                {
                    _optCostQuota.State = SubjectCostQuotaState.编制;

                    if (!string.IsNullOrEmpty(OptCostItem.Id))
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", OptCostItem.Id));
                        oq.AddFetchMode("ListQuotas", NHibernate.FetchMode.Eager);

                        oq.AddFetchMode("ProjectUnitGUID", NHibernate.FetchMode.Eager);
                        oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                        IList list = model.ObjectQuery(typeof(CostItem), oq);
                        OptCostItem = list[0] as CostItem;
                    }
                    _optCostQuota.TheCostItem = OptCostItem;
                    OptCostItem.ListQuotas.Add(_optCostQuota);

                    if (!string.IsNullOrEmpty(OptCostItem.Id))//当成本项存在时才保存明细，否则在保存成本项时一起保存明细
                    {
                        IList listTemp = new ArrayList();
                        listTemp.Add(OptCostItem);
                        listTemp = model.SaveOrUpdateCostItem(listTemp);
                        OptCostItem = listTemp[0] as CostItem;

                        _optCostQuota = OptCostItem.ListQuotas.ElementAt(OptCostItem.ListQuotas.Count - 1);
                    }
                }
                else
                {
                    IList listTemp = new ArrayList();
                    listTemp.Add(_optCostQuota);
                    listTemp = model.SaveOrUpdateCostItemQuota(listTemp);
                    _optCostQuota = listTemp[0] as SubjectCostQuota;
                }

                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private bool ValideSave()
        {
            try
            {
                if (_optCostQuota == null)
                {
                    _optCostQuota = new SubjectCostQuota();
                }

                if (string.IsNullOrEmpty(_optCostQuota.TheProjectGUID) || string.IsNullOrEmpty(_optCostQuota.TheProjectName))
                {
                    CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        _optCostQuota.TheProjectGUID = projectInfo.Id;
                        _optCostQuota.TheProjectName = projectInfo.Name;
                    }
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("资源耗用名称不能为空！");
                    txtName.Focus();
                    return false;
                }
                if (txtAccountSubject.Text.Trim() == "" || txtAccountSubject.Tag == null)
                {
                    MessageBox.Show("核算科目不能为空！");
                    txtAccountSubject.Focus();
                    return false;
                }
                //if (cbResourceGroup.Items.Count == 0)
                //{
                //    MessageBox.Show("资源不能为空！");
                //    cbResourceGroup.Focus();
                //    return false;
                //}
                //if (txtQuotaQunatity.Text.Trim() == "")
                //{
                //    MessageBox.Show("定额数量不能为空！");
                //    txtQuotaQunatity.Focus();
                //    return false;
                //}
                //if (txtQuantityPrice.Text.Trim() == "")
                //{
                //    MessageBox.Show("数量单价不能为空！");
                //    txtQuantityPrice.Focus();
                //    return false;
                //}
                //if (txtWorkAmountPrice.Text.Trim() == "")
                //{
                //    MessageBox.Show("工程量单价不能为空！");
                //    txtWorkAmountPrice.Focus();
                //    return false;
                //}
                //if (txtWastage.Text.Trim() == "")
                //{
                //    MessageBox.Show("损耗不能为空！");
                //    txtWastage.Focus();
                //    return false;
                //}
                //if (txtQuantityUnit.Text.Trim() == "")
                //{
                //    MessageBox.Show("工程计量单位不能为空！");
                //    txtQuantityUnit.Focus();
                //    return false;
                //}
                //if (txtAssessmentRate.Text.Trim() == "")
                //{
                //    MessageBox.Show("分摊比例不能为空！");
                //    txtAssessmentRate.Focus();
                //    return false;
                //}


                _optCostQuota.Name = txtName.Text.Trim();
                if (txtAccountSubject.Tag != null)
                {
                    CostAccountSubject subject = txtAccountSubject.Tag as CostAccountSubject;
                    if (subject != null)
                    {
                        _optCostQuota.CostAccountSubjectGUID = subject;
                        _optCostQuota.CostAccountSubjectName = subject.Name;
                    }
                }
                else
                {
                    MessageBox.Show("请选择核算科目！");
                    txtAccountSubject.Focus();
                    return false;
                }

                try
                {
                    decimal QuotaProjectAmount = 0;
                    if (txtQuotaQunatity.Text.Trim() != "")
                        QuotaProjectAmount = Convert.ToDecimal(txtQuotaQunatity.Text);

                    _optCostQuota.QuotaProjectAmount = QuotaProjectAmount;
                }
                catch
                {
                    MessageBox.Show("定额数量格式填写不正确！");
                    txtQuotaQunatity.Focus();
                    return false;
                }

                try
                {
                    decimal QuotaPrice = 0;
                    if (txtQuantityPrice.Text.Trim() != "")
                        QuotaPrice = Convert.ToDecimal(txtQuantityPrice.Text);

                    _optCostQuota.QuotaPrice = QuotaPrice;
                }
                catch
                {
                    MessageBox.Show("数量单价格式填写不正确！");
                    txtQuantityPrice.Focus();
                    return false;
                }

                try
                {
                    decimal QuotaMoney = 0;
                    if (txtWorkAmountPrice.Text.Trim() != "")
                        QuotaMoney = Convert.ToDecimal(txtWorkAmountPrice.Text);

                    _optCostQuota.QuotaMoney = QuotaMoney;
                }
                catch
                {
                    MessageBox.Show("工程量单价格式填写不正确！");
                    txtWorkAmountPrice.Focus();
                    return false;
                }

                try
                {
                    decimal Wastage = 0;
                    if (txtWastage.Text.Trim() != "")
                        Wastage = Convert.ToDecimal(txtWastage.Text);

                    _optCostQuota.Wastage = Wastage;
                }
                catch
                {
                    MessageBox.Show("损耗率格式填写不正确！");
                    txtWastage.Focus();
                    return false;
                }

                try
                {
                    decimal AssessmentRate = 0;
                    if (txtAssessmentRate.Text.Trim() != "")
                        AssessmentRate = Convert.ToDecimal(txtAssessmentRate.Text);

                    _optCostQuota.AssessmentRate = decimal.Round(AssessmentRate / 100, 5);
                }
                catch
                {
                    MessageBox.Show("分摊比例格式填写不正确！");
                    txtAssessmentRate.Focus();
                    return false;
                }

                if (txtQuantityUnit.Text.Trim() != "" && txtQuantityUnit.Tag != null)
                {
                    _optCostQuota.ProjectAmountUnitGUID = txtQuantityUnit.Tag as StandardUnit;
                    if (_optCostQuota.ProjectAmountUnitGUID != null)
                        _optCostQuota.ProjectAmountUnitName = _optCostQuota.ProjectAmountUnitGUID.Name;
                }
                else
                {
                    _optCostQuota.ProjectAmountUnitGUID = null;
                    _optCostQuota.ProjectAmountUnitName = "";
                }

                if (txtPriceUnit.Text.Trim() != "" && txtPriceUnit.Tag != null)
                {
                    _optCostQuota.PriceUnitGUID = txtPriceUnit.Tag as StandardUnit;
                    if (_optCostQuota.PriceUnitGUID != null)
                        _optCostQuota.PriceUnitName = _optCostQuota.PriceUnitGUID.Name;
                }
                else
                {
                    _optCostQuota.PriceUnitGUID = null;
                    _optCostQuota.PriceUnitName = "";
                }

                _optCostQuota.MainResourceFlag = cbMainResourceFlag.Checked;

                if (gridResourceGroup.Tag != null)
                {
                    //List<Material> listResourceType = (gridResourceGroup.Tag as IList).OfType<Material>().ToList();
                    List<Material> listResourceType = gridResourceGroup.Tag as List<Material>;
                    //Material m=new Material;
                    //m.AssistantCode2
                    if (listResourceType != null)
                    {
                        for (int i = _optCostQuota.ListResources.Count - 1; i > -1; i--)
                        {
                            ResourceGroup rg = _optCostQuota.ListResources.ElementAt(i);

                            var query = from r in listResourceType
                                        where r.Id == rg.ResourceTypeGUID
                                        select r;

                            if (query.Count() > 0)//找到表示存在
                            {
                                listResourceType.Remove(query.ElementAt(0));
                            }
                            else//没找到表示删除
                            {
                                _optCostQuota.ListResources.Remove(rg);
                            }
                        }
                        //剩下的表示新增
                        foreach (Material mat in listResourceType)
                        {
                            ResourceGroup rg = new ResourceGroup();
                            rg.ResourceTypeGUID = mat.Id;
                            rg.ResourceTypeCode = mat.Code;
                            rg.ResourceTypeName = mat.Name;
                            rg.ResourceTypeQuality = mat.Quality;
                            rg.ResourceTypeSpec = mat.Specification;
                            rg.DiagramNumber = mat.AssistantCode2;

                            rg.IsCateResource = mat.IfCatResource == 1;

                            rg.ResourceCateId = mat.Category.Id;
                            rg.ResourceCateSyscode = mat.TheSyscode;


                            rg.TheCostQuota = _optCostQuota;
                            _optCostQuota.ListResources.Add(rg);
                        }
                    }
                }
                else
                {
                    _optCostQuota.ListResources.Clear();
                }

                return true;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }
        //取消
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
