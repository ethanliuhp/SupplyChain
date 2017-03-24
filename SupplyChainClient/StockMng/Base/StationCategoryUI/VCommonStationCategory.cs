using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Util;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Base.StationCategoryUI
{
    public partial class VCommonStationCategory : TBasicDataView
    {
        //private static IMStationCategory theMStationCategory = StaticMethod.GetRefModule(typeof(MStationCategory)) as IMStationCategory;
        MStationCategory theMStationCategory = new MStationCategory();
        private IList result = new ArrayList();
        private StationCategory theStationCategory = new StationCategory();
        private Hashtable hashtableStationKind = new Hashtable();
        public IList Result
        {
            get { return result; }
            set { result = value; }
        }


        public VCommonStationCategory()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
        }

        private void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnQuery.Click += new EventHandler(btnQuery_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            this.dgCK.SelectionChanged += new EventHandler(dgCK_SelectionChanged);
        }

        void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null) return;  //根节点不显示

            theStationCategory = e.Node.Tag as StationCategory;
            ViewStationCategory();
        }
        private void ClearAll()
        {
            this.txtCurrentPath.Text = "";
            this.txtSysCode.Text = "";
            this.txtCategoryCode.Text = "";
            this.txtCategoryName.Text = "";
            this.txtCategoryDescript.Text = "";
            //this.chkState.Checked = true;
        }

        private void ViewStationCategory()
        {
            try
            {
                ClearAll();

                txtCurrentPath.Text = this.tvwCategory.SelectedNode.FullPath.ToString();
                txtSysCode.Text = theStationCategory.SysCode;
                txtCategoryCode.Text = theStationCategory.Code;
                txtCategoryCode.Tag = theStationCategory;
                txtCategoryName.Text = theStationCategory.Name;
                txtCategoryDescript.Text = theStationCategory.Describe;

                //this.txtAddress.Text = theStationCategory.Address;
                //this.txtCapability.Text = theStationCategory.Capability.ToString();
                //this.cboStationKind.Text = ClientUtil.ToString(hashtableStationKind[theStationCategory.StationKind]);
                //this.txtUsableCapability.Text = theStationCategory.UsableCapability.ToString();
                //this.txtUsedPrice.Text = theStationCategory.UsedPrice.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show("显示分类出错：" + ExceptionUtil.ExceptionMessage(e)); ;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            dgCK.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            if (txtName.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("Name", "%" + txtName.Text.Trim() + "%"));
            }
            if (txtCode.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("Code", "%" + this.txtCode.Text.Trim() + "%"));
            }
            //if (LoginInfomation.LoginInfo.TheOperationOrgInfo.CkRight == "1")
            //{
            //    ICriterion exp1 = Expression.Eq("OperOrgInfo.Id", ConstObject.TheOperationOrg.Id);
            //    ICriterion exp2 = Expression.Eq("CategoryNodeType", "0");
            //    oq.AddCriterion(Expression.Or(exp1, exp2));
            //}
            if (ClientUtil.ToString(txtName.Text) == "" && ClientUtil.ToString(txtCode.Text) == "")
            {
                MessageBox.Show("请输入查询条件！");
                return;
            }
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = MStationCategory.theStationCategorySrv.GetObjectsByCondition(typeof(StationCategory), oq);
            int tt = 0;
            foreach (StationCategory cate in list)
            {
                int i = this.dgCK.Rows.Add();
                DataGridViewRow r = dgCK.Rows[i];
                r.Tag = cate;
                r.Cells["code"].Value = cate.Code;
                r.Cells["name"].Value = cate.Name;
                r.Cells["level"].Value = cate.ParentNode.Name;
                tt++;
            }
            if (tt > 0)
            {
                dgCK_SelectionChanged(this.dgCK, new EventArgs());
            }
        }

        void dgCK_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgCK.SelectedRows.Count == 0) return;
            DataGridViewRow selectRow = this.dgCK.SelectedRows[0];
            StationCategory cate = selectRow.Tag as StationCategory;
            if (cate == null) return;

            txtSysCode.Text = cate.SysCode;
            txtCategoryCode.Text = cate.Code;
            txtCategoryCode.Tag = cate;
            txtCategoryName.Text = cate.Name;
            txtCategoryDescript.Text = cate.Describe;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            if ((this.tvwCategory.SelectedNode.Tag as StationCategory).Level == 1 && (txtCategoryCode.Tag as StationCategory).Level == 1)
            {
                MessageBox.Show("请选择仓库信息！");
                return;
            }
            //if ((this.tvwCategory.SelectedNode.Tag as StationCategory).State == 1)
            //{
            //    this.result.Add(this.tvwCategory.SelectedNode.Tag);
            //}
            if ((this.txtCategoryCode.Tag as StationCategory).State == 1)
            {
                this.result.Add(this.txtCategoryCode.Tag);
            }
            this.Close();
        }
        public void OpenSelectView(ObjectQuery oq, IWin32Window owner)
        {
            this.ShowDialog(owner);
        }
        private void InitForm()
        {
            hashtableStationKind.Add("仓库", 0);
            hashtableStationKind.Add("货位", 1);

            hashtableStationKind.Add(0, "仓库");
            hashtableStationKind.Add(1, "货位");

            LoadStationCategoryTree();
        }
        private void LoadStationCategoryTree()
        {
            int i = 0;
            Hashtable hashtable = new Hashtable();
            Hashtable old_hashtable = new Hashtable();
            try
            {
                i = 0;
                tvwCategory.Nodes.Clear();

                IList list = new ArrayList();
                //if (LoginInfomation.LoginInfo.TheOperationOrgInfo.CkRight == "1")
                //{
                //    ObjectQuery oq = new ObjectQuery();
                //    ICriterion exp1 = Expression.Eq("OperOrgInfo.Id", ConstObject.TheOperationOrg.Id);
                //    ICriterion exp2 = Expression.Eq("CategoryNodeType", "0");
                //    oq.AddCriterion(Expression.Or(exp1, exp2));

                //    list = MStationCategory.theStationCategorySrv.GetObjectsByCondition(typeof(StationCategory), oq);
                //}
                //else
                //{
                list = theMStationCategory.GetStationCategory();
                //}

                foreach (object o in list)
                {
                    StationCategory node = o as StationCategory;
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = node.Id.ToString();
                    tnTmp.Text = "[" + node.Code + "]" + node.Name;
                    tnTmp.Tag = node;
                    old_hashtable.Add(tnTmp.Name, tnTmp);
                }

                foreach (object o in list)
                {
                    i++;
                    StationCategory childNode = o as StationCategory;
                    if (childNode.StationKind == 1) continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = "[" + childNode.Code + "]" + childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        if (hashtable[childNode.ParentNode.Id.ToString()] != null)
                        {
                            tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                            tnp.Nodes.Add(tnTmp);
                        }
                        else
                        {
                            tnp = old_hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                            if (tnp != null)
                            {
                                tnp.Nodes.Add(tnTmp);
                            }
                        }
                    }
                    else
                    {
                        tvwCategory.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("装载分类树出错：" + i.ToString() + ExceptionUtil.ExceptionMessage(e)); ;
            }
        }

    }
}