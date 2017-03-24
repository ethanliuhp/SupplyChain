using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VGWBSTreeNodeEdit : TBasicDataView
    {
        private MProductionMng model = new MProductionMng();

        private bool  _isChanged;
        ///<summary>
        ///是否发生修改
        ///</summary>
        public bool  IsChanged
        {
            get { return this._isChanged; }
        }
        private GWBSTree _setNodeGWBSTree;

        ///<summary>    
        ///设置的工程任务节点
        ///</summary>
        public GWBSTree SetNodeGWBSTree
        {
            get { return this._setNodeGWBSTree; }
            set { this._setNodeGWBSTree = value; }
        }

        public VGWBSTreeNodeEdit(GWBSTree gwbs_setTree)
        {

            InitializeComponent();
            this._setNodeGWBSTree = gwbs_setTree;
            InitForm();
            InitEvent();
        }

        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnEnter.Click += new EventHandler(btnEnter_Click);
            this.cbIsFixed.CheckedChanged += new EventHandler(cbIsFixed_CheckedChanged);
            this.cbIsProCur.CheckedChanged += new EventHandler(cbIsProCur_CheckedChanged);
        }

        void cbIsProCur_CheckedChanged(object sender, EventArgs e)
        {

            this.dtGDEndTime.Enabled = this.cbIsProCur.Checked;
            this.dtGDStartTime.Enabled = this.cbIsProCur.Checked;

        }

        void cbIsFixed_CheckedChanged(object sender, EventArgs e)
        {
            this.dtHTEndTime.Enabled = this.cbIsFixed.Checked;
            this.dtHTStartTime.Enabled = this.cbIsFixed.Checked;
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            this._setNodeGWBSTree.IsFixed = this.cbIsFixed.Checked ? 1 : 0;
            this._setNodeGWBSTree.StartPlanBeginDate = this.cbIsFixed.Checked ? (DateTime?)this.dtHTStartTime.Value : null;
            this._setNodeGWBSTree.StartPlanEndDate = this.cbIsFixed.Checked ? (DateTime?)this.dtHTEndTime.Value : null;

            this._setNodeGWBSTree.ProductionCuringNode = this.cbIsProCur.Checked;
            this._setNodeGWBSTree.ProCurBeginDate = this.cbIsProCur.Checked ?  (DateTime?)this.dtGDStartTime.Value : null;
            this._setNodeGWBSTree.ProCurEndDate = this.cbIsProCur.Checked ? (DateTime?)this.dtGDEndTime.Value : null;

            try
            {
                model.ProductionManagementSrv.UpdateByDao(this._setNodeGWBSTree);
                this._isChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR:" + ex.Message);
                return;
            }

            this.btnEnter.FindForm().Close();

        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnEnter.FindForm().Close();
        }

        private void InitForm()
        {
            this._isChanged = false;
            this.txtGWBSTreeName.Text = this._setNodeGWBSTree.Name;
            this.cbIsFixed.Checked = this._setNodeGWBSTree.IsFixed == 1;
            this.dtHTStartTime.Value = ClientUtil.ToDateTime(this._setNodeGWBSTree.StartPlanBeginDate);
            this.dtHTEndTime.Value = ClientUtil.ToDateTime(this._setNodeGWBSTree.StartPlanEndDate);

            this.cbIsProCur.Checked = this._setNodeGWBSTree.ProductionCuringNode;
            this.dtGDStartTime.Value = ClientUtil.ToDateTime(this._setNodeGWBSTree.ProCurBeginDate);
            this.dtGDEndTime.Value = ClientUtil.ToDateTime(this._setNodeGWBSTree.ProCurEndDate);
        }
    }
}
