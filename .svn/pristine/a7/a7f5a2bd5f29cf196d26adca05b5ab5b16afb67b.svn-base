using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.BasicData.UnitMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;


namespace Application.Business.Erp.SupplyChain.Client.BasicData.UnitMng
{
    public partial class VUnitMng : TBasicDataView
    {
        private MUnitMng model = new MUnitMng();
        private UnitMaster curBillMaster;
        //private GWBSTree oprNode = null;
        //private MGWBSTree treemodel;

        //private List<GWBSDetail> listCopyNodeDetail = new List<GWBSDetail>();

        //有权限的GWBSTree
        private IList lstInstance;
        /// <summary>
        /// 当前单据
        /// </summary>
        public UnitMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        public VUnitMng()
        {
            //MGWBSTree mot
            //treemodel = mot;
            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            InitEvents();
            txtCategory.CheckBoxes = false;
            LoadLeftWindowTree();
        }

        private void InitEvents()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            this.dgMessage.CellDoubleClick += new DataGridViewCellEventHandler(dgMessage_CellDoubleClick);
            this.dgMessage.CellValidating += new DataGridViewCellValidatingEventHandler(dgMessage_CellValidating);
            txtCategory.AfterSelect += new TreeViewEventHandler(txtCategory_AfterSelect);
            dgMessage.CellEndEdit += new DataGridViewCellEventHandler(dgMessage_CellEndEdit);
        }

        private void LoadLeftWindowTree()
        {
            txtCategory.Nodes.Add("PBS");
            txtCategory.Nodes.Add("工程WBE");
        }

        private void txtCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string strName = txtCategory.SelectedNode.Text;
                //string strType = txtCategory.SelectedNode.

                curBillMaster = model.UnitSrv.GetUnitBillTypeNameById(strName);
                if (curBillMaster != null)
                {
                    dgMessage.Rows.Clear();
                    foreach (UnitDetail var in curBillMaster.Details)
                    {
                        int i = this.dgMessage.Rows.Add();
                        this.dgMessage[colCostId.Name, i].Value = curBillMaster.BillName;
                        this.dgMessage[colUnitType.Name, i].Value = var.DimensionName;
                        this.dgMessage[colUnitName.Name, i].Value = var.UnitName;
                        dgMessage.Rows[i].Tag = var;
                    }
                }
                else
                {
                    curBillMaster = new UnitMaster();
                    dgMessage.Rows.Clear();
                    //dgMessage[colCostId.Name, 0].Tag = oprNode;
                    dgMessage[colCostId.Name, 0].Value = strName;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }


        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMessage_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //判断最后一行是不是新行，不是就添加一行
            //if (this.dgMessage[colCostId.Name, e.RowIndex].Value == "")
            //{ }
            //else
            //{
            //    dgMessage.Rows.Add();
            //}
            dgMessage[colCostId.Name, e.RowIndex].Tag = dgMessage[colCostId.Name, 0].Tag;
            dgMessage[colCostId.Name, e.RowIndex].Value = dgMessage[colCostId.Name, 0].Value;
        }

        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMessage_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string strName = txtCategory.SelectedNode.Text;

            if (this.dgMessage.Columns[e.ColumnIndex].Name.Equals(colUnitType.Name))
            {
                string strflag = ClientUtil.ToString(this.dgMessage.CurrentRow.Cells[colUnitType.Name].Value);
                Dimension ds = UCL.Locate("计量单位量纲选择", StandardUnitExcuteType.SelectDimension) as Dimension;
                if (ds != null)
                {
                    if (strflag.Equals(""))
                    {
                        int i = dgMessage.Rows.Add();

                        this.dgMessage[colUnitType.Name, i].Tag = ds;
                        this.dgMessage[colUnitType.Name, i].Value = ds.Name;
                        this.dgMessage[colCostId.Name, i].Value = strName;
                        i++;
                    }
                    else
                    {
                        this.dgMessage.CurrentRow.Cells[colUnitType.Name].Tag = ds;
                        this.dgMessage.CurrentRow.Cells[colUnitType.Name].Value = ds.Name;
                        this.dgMessage.CurrentRow.Cells[colCostId.Name].Value = strName;
                    }
                }
            }
            if (this.dgMessage.Columns[e.ColumnIndex].Name.Equals(colUnitName.Name))
            {
                StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                if (su != null)
                {
                    this.dgMessage.CurrentRow.Cells[colUnitName.Name].Tag = su;
                    this.dgMessage.CurrentRow.Cells[colUnitName.Name].Value = su.Name;
                    this.dgMessage.CurrentRow.Cells[colCostId.Name].Value = strName;
                }
            }
            dgMessage.EndEdit();
            dgMessage_CellEndEdit(sender, e);
            this.btnSave.Focus();
        }
        void dgMessage_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ViewToModel()) return;
                curBillMaster = model.UnitSrv.SaveUnit(curBillMaster);
                //更新Caption
                this.ViewCaption = "计量单位配置" + "-" + curBillMaster.Code;

                MessageBox.Show("保存成功！");
                return;
            }
            catch (Exception err)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(err));
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                curBillMaster = model.UnitSrv.GetUnitById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    //foreach (UnitDetail dtl in curBillMaster.Details)
                    //{
                    //    if (dtl.SupplyLeftQuantity > 0)
                    //    {
                    //        MessageBox.Show("信息被引用，不可删除！");
                    //        return;
                    //    }
                    //}
                    if (!model.UnitSrv.DeleteByDao(curBillMaster)) return;
                    dgMessage.Rows.Clear();
                }
                else
                {
                    MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(err));
            }
        }


        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            string validMessage = "";
            if (this.dgMessage.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }

            dgMessage.EndEdit();
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }

            //明细表数据校验
            foreach (DataGridViewRow dr in dgMessage.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow)
                {
                    //if (dgMessage[colUnitType.Name, 1].Value == "")
                    //{
                        break;
                    //}
                }   

                if (dr.Cells[colCostId.Name].Value == null)
                {
                    MessageBox.Show("业务单据不允许为空！");
                    dgMessage.CurrentCell = dr.Cells[colCostId.Name];
                    return false;
                }
                if (dr.Cells[colUnitType.Name].Value == null)
                {
                    MessageBox.Show("计量单位类型不允许为空！");
                    dgMessage.CurrentCell = dr.Cells[colUnitType.Name];
                    return false;
                }
                if (dr.Cells[colUnitName.Name].Value == null)
                {
                    MessageBox.Show("计量单位不允许为空！");
                    dgMessage.CurrentCell = dr.Cells[colUnitName.Name];
                    return false;
                }
            }
            for (int i = 0; i < dgMessage.Rows.Count - 2; i++)
            {
                for (int j = dgMessage.Rows.Count - 1; j > i; j--)
                {
                    if (dgMessage[1, i].Value.Equals(dgMessage[1, j].Value))
                    {
                        MessageBox.Show("计量单位类型不可重复！");
                        return false;
                    }
                }
            }
            dgMessage.Update();
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.OperOrgInfoName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
                foreach (DataGridViewRow var in this.dgMessage.Rows)
                {
                    if (var.IsNewRow) break;
                    UnitDetail curBillDtl = new UnitDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as UnitDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.UnitId = var.Cells[colUnitName.Name].Tag as StandardUnit;
                    curBillDtl.UnitName = ClientUtil.ToString(var.Cells[colUnitName.Name].Value);
                    curBillDtl.DimensionId = var.Cells[colUnitType.Name].Tag as Dimension;
                    curBillDtl.DimensionName = ClientUtil.ToString(var.Cells[colUnitType.Name].Value);
                    curBillMaster.BillTypeName = ClientUtil.ToString(var.Cells[colCostId.Name].Value);
                    curBillMaster.BillName = ClientUtil.ToString(var.Cells[colCostId.Name].Value);
                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }
    }
}

