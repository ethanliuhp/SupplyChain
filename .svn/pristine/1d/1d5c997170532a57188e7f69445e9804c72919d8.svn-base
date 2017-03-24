using System;
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
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng
{
    public partial class VEditCostWorkForce : Form
    {
        private CostWorkForce _optCostWorkForce;
        ///<summary></summary>
        public virtual CostWorkForce OptCostWorkForce
        {
            set { this._optCostWorkForce = value; }
            get { return this._optCostWorkForce; }
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

        public VEditCostWorkForce(MCostItem mot)
        {
            model = mot;
            InitializeComponent();
            InitialEvents();
        }

        #region 注册事件
        private void InitialEvents()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSaveAndExit.Click += new EventHandler(btnSaveAndExit_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSelectResource.Click += new EventHandler(btnSelectResource_Click);
 
            this.txtMaxQutity.Leave += new EventHandler(txtMaxQutity_Leave);
            this.txtMinQutity.Leave += new EventHandler(txtMinQutity_Leave);
            this.Load += new EventHandler(VEditCostWorkForce_Load);

        }
        #endregion

        #region 每日完成工作量 与 单位工作量所需工日 的联动关系
        void txtMinQutity_Leave(object sender, EventArgs e)
        {

            string msg = "";
            decimal MinQutity = GetMaxOrMinQutity(false, ref msg);
            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
                this.txtMinQutity.Focus();

                return;
            }
            decimal MaxQutity = GetMaxOrMinQutity(true, ref msg);
            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
                this.txtMaxQutity.Focus();

                return;
            }


            if (MinQutity <= 0)
            {
                MessageBox.Show("每工日完成工程量下限必须大于0！");
                this.txtMinQutity.Focus();

                return;
            }

            if (MaxQutity > 0 && MinQutity > MaxQutity)
            {
                MessageBox.Show("每工日完成工程量上限必须大于每工日完成工程量下限！");
                this.txtMinQutity.Focus();

                return;
            }

            this.txtMaxWorkdays.Text = ClientUtil.ToString(((decimal)1 / MinQutity).ToString("####################0.######"));
        }

        void txtMaxQutity_Leave(object sender, EventArgs e)
        {
            string msg = "";
            decimal MaxQutity = GetMaxOrMinQutity(true, ref msg);
            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
                this.txtMaxQutity.Focus();
                return;
            }
            decimal MinQutity = GetMaxOrMinQutity(false, ref msg);
            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
                this.txtMinQutity.Focus();
                return;
            }

            if (MaxQutity <= 0)
            {
                MessageBox.Show("每工日完成工程量上限必须大于0！");
                this.txtMinQutity.Focus();
                return;
            }

            if (MinQutity > 0 && MinQutity > MaxQutity)
            {
                MessageBox.Show("每工日完成工程量上限必须大于每工日完成工程量下限！");
                this.txtMinQutity.Focus();
                return;
            }

            this.txtMinWorkdays.Text = ClientUtil.ToString(((decimal)1 / MaxQutity).ToString("##################0.######"));
        }

        private decimal GetMaxOrMinQutity(bool isMax, ref string msg)
        {
            if (isMax)
            {
                decimal MaxQutity = 0;
                try
                {
                    if (txtMaxQutity.Text.Trim() != "")
                        MaxQutity = Convert.ToDecimal(txtMaxQutity.Text);
                    return MaxQutity;
                }
                catch
                {
                    msg = "每工日完成工程量上限格式填写不正确！";
                    return MaxQutity;
                }
            }
            else
            {
                decimal MinQutity = 0;
                try
                {
                    
                    if (txtMinQutity.Text.Trim() != "")
                        MinQutity = Convert.ToDecimal(txtMinQutity.Text);

                    return MinQutity;
                }
                catch
                {
                    msg = "每工日完成工程量下限格式填写不正确！";
                    txtMinQutity.Focus();
                    return MinQutity;
                }
            }
        }
        #endregion


        void VEditCostWorkForce_Load(object sender, EventArgs e)
        {
            if (OptCostWorkForce != null)
            {
                if (!string.IsNullOrEmpty(OptCostWorkForce.Id))
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", OptCostWorkForce.Id));
                    
                    OptCostWorkForce = model.ObjectQuery(typeof(CostWorkForce), oq)[0] as CostWorkForce;
                }

                this.txtName.Text = OptCostWorkForce.Name;
                this.txtState.Text = ClientUtil.ToString(OptCostWorkForce.State);
                this.txtResourceType.Text = OptCostWorkForce.ResourceTypeName;
                this.txtResourceType.Tag = OptCostWorkForce.ResourceTypeGUID;
                this.txtMaxQutity.Text = ClientUtil.ToString(OptCostWorkForce.MaxQutity);
                this.txtMinQutity.Text = ClientUtil.ToString(OptCostWorkForce.MinQutity);
                this.txtMaxWorkdays.Text = ClientUtil.ToString(OptCostWorkForce.MaxWorkdays);
                this.txtMinWorkdays.Text =ClientUtil.ToString( OptCostWorkForce.MinWorkdays);

            }
            else
            {
                txtState.Text = CostWorkForceState.编制.ToString();

            }
        }

      
        //选择资源
        void btnSelectResource_Click(object sender, EventArgs e)
        {
            CommonMaterial materialSelector = new CommonMaterial();
            materialSelector.materialCatCode = "03";
            materialSelector.OpenSelect();

            IList list = materialSelector.Result;
            if (list.Count > 0)
            {
                Material mtl = list[0] as Material;
                this.txtResourceType.Text = mtl.Name;
                this.txtResourceType.Tag = mtl.Id;

                this.txtResourceTypeHide2.Text =  mtl.Code;
                this.txtResourceTypeHide1.Text=mtl.Specification;
                this.txtResourceTypeHide1.Tag =  mtl.Stuff;
            }
        }
 
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValideSave())
                    return;
                if (string.IsNullOrEmpty(_optCostWorkForce.Id))
                {
                    _optCostWorkForce.State = CostWorkForceState.编制;

                    if (!string.IsNullOrEmpty(OptCostItem.Id))
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", OptCostItem.Id));
                        oq.AddFetchMode("ListCostWorkForce", NHibernate.FetchMode.Eager);
                        IList list = model.ObjectQuery(typeof(CostItem), oq);
                        OptCostItem = list[0] as CostItem;
                    }

                    if (_optCostWorkForce.TheCostItem == null)
                    {
                        _optCostWorkForce.TheCostItem = OptCostItem;
                    }

                   
                }
                else
                {
                    IList listTemp = new ArrayList();
                    listTemp.Add(_optCostWorkForce);
                    listTemp = model.SaveOrUpdateCostWorkForce(listTemp);
                    _optCostWorkForce = listTemp[0] as CostWorkForce;
                }


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
                if (string.IsNullOrEmpty(_optCostWorkForce.Id))
                {
                    _optCostWorkForce.State = CostWorkForceState.编制;

                    if (!string.IsNullOrEmpty(OptCostItem.Id))
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", OptCostItem.Id));
                        oq.AddFetchMode("ListCostWorkForce", NHibernate.FetchMode.Eager);
                        IList list = model.ObjectQuery(typeof(CostItem), oq);
                        OptCostItem = list[0] as CostItem;
                    }
                    _optCostWorkForce.TheCostItem = OptCostItem;
                    OptCostItem.ListCostWorkForce.Add(_optCostWorkForce);

                    if (!string.IsNullOrEmpty(OptCostItem.Id))//当成本项存在时才保存明细，否则在保存成本项时一起保存明细
                    {
                        IList listTemp = new ArrayList();
                        listTemp.Add(OptCostItem);
                        listTemp = model.SaveOrUpdateCostItem(listTemp);
                        OptCostItem = listTemp[0] as CostItem;

                        _optCostWorkForce = OptCostItem.ListCostWorkForce.ElementAt(OptCostItem.ListCostWorkForce.Count - 1);
                    }
                }
                else
                {
                    IList listTemp = new ArrayList();
                    listTemp.Add(_optCostWorkForce);
                    listTemp = model.SaveOrUpdateCostWorkForce(listTemp);
                    _optCostWorkForce = listTemp[0] as CostWorkForce;
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
                if (_optCostWorkForce == null)
                {
                    _optCostWorkForce = new CostWorkForce();
                }

               
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("劳动力定额名称不能为空！");
                    txtName.Focus();
                    return false;
                }
                //if (txtAccountSubject.Text.Trim() == "" || txtAccountSubject.Tag == null)
                //{
                //    MessageBox.Show("核算科目不能为空！");
                //    txtAccountSubject.Focus();
                //    return false;
                //}
              

                _optCostWorkForce.Name = txtName.Text.Trim();

                //if (txtAccountSubject.Tag != null)
                //{
                //    CostAccountSubject subject = null;
                //    CostAccountSubject subject1 = txtAccountSubject.Tag as CostAccountSubject;

                //    ObjectQuery oq = new ObjectQuery();
                //    oq.AddCriterion(Expression.Eq("Id", subject1.Id));
                //    IList list = model.ObjectQuery(typeof(CostAccountSubject), oq);

                //    if (list != null && list.Count > 0)
                //        subject = list[0] as CostAccountSubject;

                //    if (subject != null)
                //    {
                //        _optCostWorkForce.CostAccountSubjectGuid = subject;
                //        _optCostWorkForce.CostAccountSubjectName = subject.Name;
                //        _optCostWorkForce.CostAccountSubjectCode = subject.Code;
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("请选择核算科目！");
                //    txtAccountSubject.Focus();
                //    return false;
                //}

                if (txtResourceType.Tag != null)
                {
                    string mtlcode = txtResourceType.Tag as string;
                    if (mtlcode != null)
                    {
                        _optCostWorkForce.ResourceTypeGUID = mtlcode;
                        _optCostWorkForce.ResourceTypeName = txtResourceType.Text;

                        _optCostWorkForce.ResourceTypeCode = this.txtResourceTypeHide2.Text;
                        _optCostWorkForce.ResourceTypeStuff = this.txtResourceTypeHide1.Tag as string;
                        _optCostWorkForce.ResourceTypeSpec = this.txtResourceTypeHide1.Text;

                    }
                }
                else
                {
                    MessageBox.Show("请选择资源名称！");
                    txtResourceType.Focus();
                    return false;
                }

                try
                {
                    decimal MaxQutity = 0;
                    if (txtMaxQutity.Text.Trim() != "")
                        MaxQutity = Convert.ToDecimal(txtMaxQutity.Text);

                    _optCostWorkForce.MaxQutity = MaxQutity;
                    this.txtMinWorkdays.Text = ClientUtil.ToString(((decimal)1 / MaxQutity).ToString("##################0.######"));
                }
                catch
                {
                    MessageBox.Show("每工日完成工程量上限格式填写不正确！");
                    txtMaxQutity.Focus();
                    return false;
                }

                try
                {
                    decimal MinQutity = 0;
                    if (txtMinQutity.Text.Trim() != "")
                        MinQutity = Convert.ToDecimal(txtMinQutity.Text);

                    _optCostWorkForce.MinQutity = MinQutity;
                    this.txtMaxWorkdays.Text = ClientUtil.ToString(((decimal)1 / MinQutity).ToString("####################0.######"));
                    
                }
                catch
                {
                    MessageBox.Show("每工日完成工程量下限格式填写不正确！");
                    txtMinQutity.Focus();
                    return false;
                }
                _optCostWorkForce.MaxWorkdays = ClientUtil.ToDecimal(this.txtMaxWorkdays.Text);
                _optCostWorkForce.MinWorkdays = ClientUtil.ToDecimal(this.txtMinWorkdays.Text);

                _optCostWorkForce.State = EnumUtil<CostWorkForceState>.FromDescription(this.txtState.Text);

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
