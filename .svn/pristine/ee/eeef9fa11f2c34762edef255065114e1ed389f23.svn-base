using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VPBSTreeBakNewBatchUpdateName_New : TBasicDataView
    {
        PBSTree CurrPBSTree = null;
        MPBSTree model = null;
        Hashtable ht = new Hashtable();
        CurrentProjectInfo currProject = null;
        Color UpdateColor = System.Drawing.Color.Blue;
       public  bool IsSave = false;
        public VPBSTreeBakNewBatchUpdateName_New(PBSTree oPBSTree, MPBSTree model)
        {
            InitializeComponent();
            this.model = model;
            this.CurrPBSTree = oPBSTree;
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
            if (CurrPBSTree != null)
            {
                this.txtPBSTreeSearch.Tag = CurrPBSTree;
                this.txtPBSTreeSearch.Text = CurrPBSTree.Name;
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
             VSelectPBSNode frm = new VSelectPBSNode();
            frm.IsSelectSingleNode = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode oNode=frm.SelectResult[0];
                if (oNode != null && oNode.Tag != null)
                {
                    CurrPBSTree = oNode.Tag as PBSTree;
                    if (CurrPBSTree != null)
                    {
                        this.txtPBSTreeSearch.Tag = CurrPBSTree;
                        this.txtPBSTreeSearch.Text = CurrPBSTree.Name;
                    }
                    else
                    {
                        this.txtPBSTreeSearch.Tag = null;
                        this.txtPBSTreeSearch.Text = "";
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
                string sParentID = string.IsNullOrEmpty(txtPBSTreeSearch.Text) ? null : (txtPBSTreeSearch.Tag as PBSTree).Id;
                DataSet ds = model.GetPBSTreesByInstanceSql(sProjectID, sParentID, sName);
                IList<PBSTree> lstPBSTree = null;
                int iRow = 0;
                DataGridViewRow oRow=null;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {//t1.name,t1.id,t1.fullpath,t1.code
                    lstPBSTree = ds.Tables[0].Rows.OfType<DataRow>().Select(a => new PBSTree()
                                    {
                                        Id = ClientUtil.ToString(a["id"]),
                                        Name = ClientUtil.ToString(a["name"]),
                                        Code = ClientUtil.ToString(a["code"]),
                                        FullPath = ClientUtil.ToString(a["fullpath"]),
                                        Level = ClientUtil.ToInt(a["tlevel"]),
                                        TheProjectGUID=sProjectID,
                                        Describe = ClientUtil.ToString(a["fullpath"])
                                    }).ToList();
             
                    if (lstPBSTree != null && lstPBSTree.Count > 0)
                    {
                        foreach (PBSTree oPBSTree in lstPBSTree)
                        {
                            iRow=gridGWBTree.Rows.Add();
                           oRow= gridGWBTree.Rows[iRow];
                           oRow.Cells[this.colCode.Name].Value = oPBSTree.Code;
                           oRow.Cells[this.colFullPath.Name].Value = oPBSTree.FullPath;
                           oRow.Cells[this.colName.Name].Value = oPBSTree.Name;
                           oRow.Tag = oPBSTree;
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
                IList lst = ht.Values.OfType<PBSTree>().OrderBy(a=>a.Level).ToList();
                foreach (PBSTree oTemp in lst)
                {
                    var var = lst.OfType<PBSTree>().Where(a => (a.Level > oTemp.Level && a.Id != oTemp.Id && a.FullPath.StartsWith(oTemp.FullPath)));
                    if (var.Count() > 0)
                    {
                        foreach (PBSTree o in var)
                        {
                            o.Describe = oTemp.FullPath + o.Describe.Substring(oTemp.Describe.Length);
                        }
                    }
                }
                model.SaveBatchPBSTreeName(lst);
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
                PBSTree oPBSTree = null;
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
                    oPBSTree = oRow.Tag as PBSTree;
                    if (!ht.ContainsKey(oPBSTree.Id)) { ht.Add(oPBSTree.Id,oPBSTree); }
                    oPBSTree.Name = oPBSTree.Name.Replace(sOldValue, sNewValue);
                    sFullPath = oPBSTree.FullPath;
                    oPBSTree.FullPath = string.Format("{0}{1}", sFullPath.Substring(0, sFullPath.LastIndexOf("\\") + 1), oPBSTree.Name);
                    oRow.Cells[colName.Name].Value = oPBSTree.Name;
                    oRow.Cells[colFullPath.Name].Value = oPBSTree.FullPath;
                    oRow.Cells[colName.Name].Style.ForeColor = oRow.Cells[colFullPath.Name].Style.ForeColor = UpdateColor;
                    BatchUpdateFullPath(oPBSTree, sFullPath+"\\");
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
                    //oCell.Selected = true;
                    MessageBox.Show("节点名称不能为空");
                
                }
                else
                {
                    PBSTree oPBSTreeTemp = gridGWBTree.Rows[e.RowIndex].Tag as PBSTree;
                    string sName=ClientUtil.ToString( oCell.EditedFormattedValue);
                    string sFullPath=oPBSTreeTemp.FullPath;
                    if (!string.Equals(oPBSTreeTemp.Name, sName))
                    {
                        oPBSTreeTemp.Name = sName;
                        oPBSTreeTemp.FullPath = string.Format("{0}{1}", sFullPath.Substring(0, sFullPath.LastIndexOf("\\") + 1), sName);
                        gridGWBTree[this.colFullPath.Name, e.RowIndex].Value = oPBSTreeTemp.FullPath;
                        gridGWBTree[this.colFullPath.Name, e.RowIndex].Style.ForeColor = UpdateColor;
                        gridGWBTree[this.colName.Name, e.RowIndex].Style.ForeColor = UpdateColor;
                        if (!ht.ContainsKey(oPBSTreeTemp.Id))
                        {
                            ht.Add(oPBSTreeTemp.Id, oPBSTreeTemp);
                            btnSave.Visible = true;
                        }
                        BatchUpdateFullPath(oPBSTreeTemp, sFullPath+"\\");
                    }
                }
            }
        }
        public void gridGWBTree_CellValidating(object sender,  DataGridViewCellValidatingEventArgs e)
        {
            if (gridGWBTree.Columns[e.ColumnIndex] == colName)
            {
               // DataGridViewCell oCell=gridGWBTree[colName.Name, e.RowIndex];
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
        public void BatchUpdateFullPath(PBSTree oPBSTreeParent,string sOldFullPath)
        {
            PBSTree oPBSTreeTemp = null;
            int iLen=sOldFullPath.Length-1;
            IList<DataGridViewRow> lstRows = gridGWBTree.Rows.OfType<DataGridViewRow>().Where(a => (a.Tag!=null && (a.Tag as PBSTree).FullPath.StartsWith(sOldFullPath))).ToList();
            if (lstRows != null && lstRows.Count > 0)
            {
                foreach (DataGridViewRow oRow in lstRows)
                {
                    oPBSTreeTemp = oRow.Tag as PBSTree;
                    if (string.Equals(oPBSTreeTemp.Id, oPBSTreeParent.Id)) continue;
                    oPBSTreeTemp.FullPath = string.Format("{0}{1}", oPBSTreeParent.FullPath, oPBSTreeTemp.FullPath.Substring(iLen));
                    oRow.Cells[colFullPath.Name].Value = oPBSTreeTemp.FullPath;
                    oRow.Cells[colFullPath.Name].Style.ForeColor = UpdateColor;
                }
            }
        }



         
    }
}
