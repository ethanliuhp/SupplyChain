using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSTreeBatchUpdateName_New : TBasicDataView
    { 
        GWBSTree CurrGWBSTree = null;
        MGWBSTree model = null;
        Hashtable ht = new Hashtable();
        CurrentProjectInfo currProject = null;
        Color UpdateColor = System.Drawing.Color.Blue;
      public   bool IsSave = false;
        public VGWBSTreeBatchUpdateName_New( GWBSTree oGWBSTree,MGWBSTree model)
        {
            InitializeComponent();
            this.model = model;
            this.CurrGWBSTree = oGWBSTree;
            InitialForm();
        }
        public void InitialForm()
        {
            InitalData();
            IntialEvent();
        }
        public void InitalData()
        {
            currProject = StaticMethod.GetProjectInfo();
            if (CurrGWBSTree != null)
            {
                this.txtGWBSTreeSearch.Tag = CurrGWBSTree;
                this.txtGWBSTreeSearch.Text = CurrGWBSTree.Name;
                btnQuery_Click(null,null);
            }
             
             
          
        }
        public void IntialEvent()
        {
            this.btnSelectSearch.Click+=new EventHandler(btnSelectSearch_Click);
            this.btnQuery.Click+=new EventHandler(btnQuery_Click);
            this.btnSave.Click+=new EventHandler(btnSave_Click);
            this.btnReplace.Click+=new EventHandler(btnReplace_Click);
            this.chkAllSelect.CheckedChanged+=new EventHandler(chkAllSelect_CheckedChanged);
            this.chkUnSelect.CheckedChanged+=new EventHandler(chkUnSelect_CheckedChanged);
            this.gridGWBTree.CellContentClick += new DataGridViewCellEventHandler(gridGWBTree_CellContentClick);
            this.gridGWBTree.CellEndEdit += new DataGridViewCellEventHandler(gridGWBTree_CellEndEdit);
           this.gridGWBTree.CellValidating += new DataGridViewCellValidatingEventHandler(gridGWBTree_CellValidating);
        }
        public void btnSelectSearch_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsSelectSingleNode = true;
            frm.ShowDialog();
            if (frm.isOK && frm.SelectResult != null && frm.SelectResult.Count > 0)
            {
                TreeNode oNode=frm.SelectResult[0];
                if (oNode != null && oNode.Tag != null)
                {
                    CurrGWBSTree = oNode.Tag as GWBSTree;
                    if (CurrGWBSTree != null)
                    {
                        this.txtGWBSTreeSearch.Tag = CurrGWBSTree;
                        this.txtGWBSTreeSearch.Text = CurrGWBSTree.Name;
                    }
                    else
                    {
                        this.txtGWBSTreeSearch.Tag = null;
                        this.txtGWBSTreeSearch.Text = "";
                    }
                }
            }
        }
        public void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                gridGWBTree.Rows.Clear();
                string sProjectID = currProject.Id;
                string sName = txtNameSearch.Text.Trim();
                string sParentID = string.IsNullOrEmpty(txtGWBSTreeSearch.Text) ? null : (txtGWBSTreeSearch.Tag as GWBSTree).Id;
                DataSet ds = model.GetGWBSTreesByInstanceSql(sProjectID, sParentID, sName);
                IList<GWBSTree> lstGWBSTree = null;
                int iRow = 0;
                DataGridViewRow oRow=null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {//t1.name,t1.id,t1.fullpath,t1.code
                    lstGWBSTree = ds.Tables[0].Rows.OfType<DataRow>().Select(a => new GWBSTree()
                                    {
                                        Id = ClientUtil.ToString(a["id"]),
                                        Name = ClientUtil.ToString(a["name"]),
                                        Code = ClientUtil.ToString(a["code"]),
                                        FullPath = ClientUtil.ToString(a["fullpath"]),
                                        Level = ClientUtil.ToInt(a["tlevel"]),
                                        TheProjectGUID=sProjectID,
                                        Describe = ClientUtil.ToString(a["fullpath"]),//临时
                                        SpecialType = ClientUtil.ToString(a["name"])//临时
                                    }).ToList();
             
                    if (lstGWBSTree != null && lstGWBSTree.Count > 0)
                    {
                        foreach (GWBSTree oGWBSTree in lstGWBSTree)
                        {
                            iRow=gridGWBTree.Rows.Add();
                           oRow= gridGWBTree.Rows[iRow];
                           oRow.Cells[this.colCode.Name].Value = oGWBSTree.Code;
                           oRow.Cells[this.colFullPath.Name].Value = oGWBSTree.FullPath;
                           oRow.Cells[this.colName.Name].Value = oGWBSTree.Name;
                           oRow.Tag = oGWBSTree;
                        }
                    }
                }
                btnSave.Visible = false;
                ht.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        public void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                gridGWBTree.EndEdit();
                if (ht == null || ht.Count == 0) throw new Exception("请修改后再保存");
                IList lst = ht.Values.OfType<GWBSTree>().OrderBy(a=>a.Level).ToList();
                foreach (GWBSTree oTemp in lst)
                {
                    var var = lst.OfType<GWBSTree>().Where(a => (a.Level > oTemp.Level && a.Id != oTemp.Id && a.FullPath.StartsWith(oTemp.FullPath)));
                    if (var.Count() > 0)
                    {
                        foreach (GWBSTree o in var)
                        {
                            o.Describe = oTemp.FullPath + o.Describe.Substring(oTemp.Describe.Length);
                        }
                    }
                }
                model.SaveBatchGWBSTreeName(lst);
                IsSave = true;
                this.btnQuery_Click(null,null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:"+ExceptionUtil.ExceptionMessage(ex));
            }
            
        }
        public void btnReplace_Click(object sender, EventArgs e)
        {
            try
            {
                gridGWBTree.EndEdit();
                GWBSTree oGWBSTree = null;
                string sFullPath = string.Empty;
                string sOldValue = txtOldName.Text;
                string sNewValue = txtNewName.Text;
                if (sOldValue == sNewValue) throw new Exception("替换[原名称内容]与替换后内容相同");
                if (string.IsNullOrEmpty(sOldValue)) { this.txtOldName.Select(); throw new Exception("替换[原名称内容]不能为空"); }
                if ( this.gridGWBTree.Rows.Count==0) throw new Exception("当前查询的施工任务集合为空");
                IList<DataGridViewRow> lstRow = this.gridGWBTree.Rows.OfType<DataGridViewRow>().Where(a =>ClientUtil.ToBool(a.Cells[colSelect.Name].Value)).ToList();
                if (lstRow == null || lstRow.Count == 0) throw new Exception("请勾选需要被替换的施工任务");
                lstRow = lstRow.Where(a => ClientUtil.ToString(a.Cells[colName.Name].Value).Contains(sOldValue)).ToList();
                if(lstRow==null || lstRow.Count==0) throw new Exception(string.Format("没有找到施工任务名称包含[{0}]记录",sOldValue));
                foreach (DataGridViewRow oRow in lstRow)
                {
                    oGWBSTree = oRow.Tag as GWBSTree;
                    if (!ht.ContainsKey(oGWBSTree.Id)) { ht.Add(oGWBSTree.Id,oGWBSTree); }
                    oGWBSTree.Name = oGWBSTree.Name.Replace(sOldValue, sNewValue);
                    sFullPath = oGWBSTree.FullPath;
                    oGWBSTree.FullPath = string.Format("{0}{1}", sFullPath.Substring(0, sFullPath.LastIndexOf("\\") + 1), oGWBSTree.Name);
                    oRow.Cells[colName.Name].Value = oGWBSTree.Name;
                    oRow.Cells[colFullPath.Name].Value = oGWBSTree.FullPath;
                    oRow.Cells[colName.Name].Style.ForeColor = oRow.Cells[colFullPath.Name].Style.ForeColor = UpdateColor;
                    BatchUpdateFullPath(oGWBSTree, sFullPath+"\\");
                    btnSave.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("替换失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        public void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllSelect.Checked)
            {
                foreach (DataGridViewRow oRow in this.gridGWBTree.Rows)
                {
                    oRow.Cells[colSelect.Name].Value = true;
                }
                this.chkUnSelect.CheckedChanged -= this.chkUnSelect_CheckedChanged;
                this.chkUnSelect.Checked = false;
                this.chkUnSelect.CheckedChanged += this.chkUnSelect_CheckedChanged;
            }
            
        }
        public void chkUnSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnSelect.Checked)
            {
                foreach (DataGridViewRow oRow in this.gridGWBTree.Rows)
                {
                    oRow.Cells[colSelect.Name].Value = !ClientUtil.ToBool(oRow.Cells[colSelect.Name].Value);
                }
                this.chkAllSelect.CheckedChanged -= this.chkAllSelect_CheckedChanged;
                this.chkAllSelect.Checked = false;
                this.chkAllSelect.CheckedChanged += this.chkAllSelect_CheckedChanged;
            }
        }
        public void gridGWBTree_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.chkUnSelect.CheckedChanged -= this.chkUnSelect_CheckedChanged;
            this.chkAllSelect.CheckedChanged -= this.chkAllSelect_CheckedChanged;
            this.chkUnSelect.Checked = false;
            this.chkAllSelect.Checked = this.gridGWBTree.Rows.Count == this.gridGWBTree.Rows.OfType<DataGridViewRow>().Where(a => ClientUtil.ToBool(a.Cells[colSelect.Name].EditedFormattedValue)).Count();
            this.chkUnSelect.CheckedChanged += this.chkUnSelect_CheckedChanged;
            this.chkAllSelect.CheckedChanged += this.chkAllSelect_CheckedChanged;
        }
        public void gridGWBTree_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (gridGWBTree.Columns[e.ColumnIndex] == colName)
            {
                DataGridViewCell oCell = gridGWBTree[colName.Name, e.RowIndex];
                if (string.IsNullOrEmpty(ClientUtil.ToString(oCell.Value)))
                {
                   // oCell.Selected = true;
                    MessageBox.Show("节点名称不能为空");
                }
                else
                {
                    GWBSTree oGWBSTreeTemp = gridGWBTree.Rows[e.RowIndex].Tag as GWBSTree;
                    string sName=ClientUtil.ToString( oCell.EditedFormattedValue);
                    string sFullPath=oGWBSTreeTemp.FullPath;
                    if (!string.Equals(oGWBSTreeTemp.Name, sName))
                    {
                        oGWBSTreeTemp.Name = sName;
                        oGWBSTreeTemp.FullPath = string.Format("{0}{1}", sFullPath.Substring(0, sFullPath.LastIndexOf("\\") + 1), sName);
                        gridGWBTree[this.colFullPath.Name, e.RowIndex].Value = oGWBSTreeTemp.FullPath;
                        gridGWBTree[this.colFullPath.Name, e.RowIndex].Style.ForeColor = UpdateColor;
                        gridGWBTree[this.colName.Name, e.RowIndex].Style.ForeColor = UpdateColor;
                        if (!ht.ContainsKey(oGWBSTreeTemp.Id))
                        {
                            ht.Add(oGWBSTreeTemp.Id, oGWBSTreeTemp);
                            btnSave.Visible = true;
                        }
                        BatchUpdateFullPath(oGWBSTreeTemp, sFullPath+"\\");
                    }
                }
            }
        }
        public void gridGWBTree_CellValidating(object sender,  DataGridViewCellValidatingEventArgs e)
        {
            if (gridGWBTree.Columns[e.ColumnIndex] == colName)
            {
                if (string.IsNullOrEmpty(ClientUtil.ToString(e.FormattedValue)))
                {
                    e.Cancel = true;
                    //gridGWBTree.CancelEdit();
                    MessageBox.Show("节点名称不能为空");
                    gridGWBTree.Focus();
                    gridGWBTree.CurrentCell = gridGWBTree[colName.Name, e.RowIndex];
                }
                else
                {
                    e.Cancel = false;
                }
            }
        }
        public void BatchUpdateFullPath(GWBSTree oGWBSTreeParent,string sOldFullPath)
        {
            GWBSTree oGWBSTreeTemp = null;
            int iLen=sOldFullPath.Length-1;
            IList<DataGridViewRow> lstRows = gridGWBTree.Rows.OfType<DataGridViewRow>().Where(a => (a.Tag!=null && (a.Tag as GWBSTree).FullPath.StartsWith(sOldFullPath))).ToList();
            if (lstRows != null && lstRows.Count > 0)
            {
                foreach (DataGridViewRow oRow in lstRows)
                {
                    oGWBSTreeTemp = oRow.Tag as GWBSTree;
                    if (string.Equals(oGWBSTreeTemp.Id, oGWBSTreeParent.Id)) continue;
                    oGWBSTreeTemp.FullPath = string.Format("{0}{1}", oGWBSTreeParent.FullPath, oGWBSTreeTemp.FullPath.Substring(iLen));
                    oRow.Cells[colFullPath.Name].Value = oGWBSTreeTemp.FullPath;
                    oRow.Cells[colFullPath.Name].Style.ForeColor = UpdateColor;
                }
            }
        }

         
    }
}
