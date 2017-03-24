using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using System.Collections;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VElementAndPBSRelation : TBasicDataView
    {
        private Elements opElement = null;
        private ElementPBSRelation opRelation = null;
        MPBSTree model = new MPBSTree();
        public VElementAndPBSRelation(Elements element)
        {
            InitializeComponent();
            opElement = element;
            InitData();
            InitEvents();
        }
        void InitEvents()
        {
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            btnSelectPBS.Click += new EventHandler(btnSelectPBS_Click);
        }
        void InitData()
        {
            ShowData();
            cbRelationType.Items.AddRange(new object[] { "参照"});
            cbRelationType.SelectedIndex = 0;
        }
        void ShowData()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ElementId", opElement.Id));
            IList list = model.ObjectQuery(typeof(ElementPBSRelation), oq);
            dgRelation.Rows.Clear();
            foreach (ElementPBSRelation relation in list)
            {
                int index = dgRelation.Rows.Add();
                dgRelation[pbsName.Name, index].Value = relation.PbsName;
                dgRelation[relationType.Name, index].Value = relation.RelationType;
                dgRelation.Rows[index].Tag = relation;
            }
        }

        //新增
        void btnAdd_Click(object sender, EventArgs e)
        {
            Clear();
            opRelation = new ElementPBSRelation();
        }
        //修改
        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgRelation.Rows.Count == 0 || dgRelation.CurrentRow.Tag == null) return;
            ElementPBSRelation relation = dgRelation.CurrentRow.Tag as ElementPBSRelation;
            opRelation = relation;
            txtPBSNode.Text = relation.PbsName;
            txtPBSNode.Tag = relation.PbsId;
            cbRelationType.Text = relation.RelationType;
        }
        //删除
        void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgRelation.Rows.Count == 0 || dgRelation.CurrentRow.Tag == null) return;
            try
            {
                if (MessageBox.Show("确定删除？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ElementPBSRelation relation = dgRelation.CurrentRow.Tag as ElementPBSRelation;
                    model.Delete(relation);
                    dgRelation.Rows.Remove(dgRelation.CurrentRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除出错：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isNew = true;
                if (opRelation==null|| opRelation.Id == null)
                {
                    opRelation = new ElementPBSRelation();
                    opRelation.ElementId = opElement.Id;
                    opRelation.ElementName = opElement.Name;
                }
                else
                {
                    isNew = false;
                }
                opRelation.PbsId = txtPBSNode.Tag.ToString();
                opRelation.PbsName = txtPBSNode.Text;
                opRelation.RelationType = cbRelationType.Text;

                IList list = new ArrayList();
                list.Add(opRelation);
                opRelation = model.SaveOrUpdate(list)[0] as ElementPBSRelation;
                int index = -1;
                if (isNew)
                {
                    index = dgRelation.Rows.Add();
                }
                else
                {
                    index = dgRelation.CurrentRow.Index;
                }
                dgRelation[pbsName.Name, index].Value = opRelation.PbsName;
                dgRelation[relationType.Name, index].Value = opRelation.RelationType;
                dgRelation.Rows[index].Tag = opRelation;
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存出错：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //取消
        void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        //选择PBS节点
        void btnSelectPBS_Click(object sender, EventArgs e)
        {
            VSelectPBSNode frm = new VSelectPBSNode();
            frm.SelectMethod = SelectNodeMethod.零散节点选择;
            frm.IsSelectSingleNode = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                List<TreeNode> listSelectNode = frm.SelectResult;
                PBSTree pbs = listSelectNode[0].Tag as PBSTree;
                txtPBSNode.Text = pbs.Name;
                txtPBSNode.Tag = pbs.Id;
                //List<PBSTree> listPBS = new List<PBSTree>();
                //foreach (TreeNode tn in listSelectNode)
                //{
                //    PBSTree pbs = tn.Tag as PBSTree;
                //    listPBS.Add(pbs);
                //    cbRelaPBS.Items.Add(pbs);
                //}

                //cbRelaPBS.DisplayMember = "Name";
                //cbRelaPBS.ValueMember = "Id";

                //List<PBSTree> listOld = cbRelaPBS.Tag as List<PBSTree>;
                //if (listOld != null && listOld.Count > 0)
                //{
                //    listPBS.AddRange(listOld);
                //}
                //cbRelaPBS.Tag = listPBS;

                //cbRelaPBS.SelectedIndex = 0;
            }
        }
        void Clear()
        {
            txtPBSNode.Text = "";
            txtPBSNode.Tag = null;
            cbRelationType.Text = "参照";
        }
    }
}
