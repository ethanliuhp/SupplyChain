using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI
{
    public partial class VGWBSConfirmNode : Form
    {
        private GWBSTaskConfirm _GWBSTaskConfirm;
        private MProductionMng model = new MProductionMng();

        public VGWBSConfirmNode(GWBSTaskConfirm _GWBSTaskConfirm)
        {
            InitializeComponent();
            InitEvent();
            this._GWBSTaskConfirm = _GWBSTaskConfirm;
        }

        private void InitEvent()
        {
            btnExit.Click += new EventHandler(btnExit_Click);
            Load += new EventHandler(VGWBSConfirmNode_Load);
        }

        void VGWBSConfirmNode_Load(object sender, EventArgs e)
        {
            if (_GWBSTaskConfirm.Id == null)
            {
                foreach (GWBSTaskConfirmNode node in _GWBSTaskConfirm.NodeDetails)
                {
                    int rowIndex = dgDetail.Rows.Add();
                    DataGridViewRow dr = dgDetail.Rows[rowIndex];
                    dr.Cells[colGWBSTree.Name].Value = node.GWBSTreeName;
                    dr.Cells[colProgress.Name].Value = node.Progress;
                }
            }
            else
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("GWBSTaskConfirm.Id", _GWBSTaskConfirm.Id));
                try
                {
                    IList list = model.ProductionManagementSrv.GetGWBSTaskConfirmNode(oq);
                    ShowList(list);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询生产节点出错。\n"+ExceptionUtil.ExceptionMessage(ex));
                }
            }
        }

        private void ShowList(IList nodeList)
        {
            if (nodeList == null || nodeList.Count == 0) return;
            foreach (GWBSTaskConfirmNode node in nodeList)
            {
                int rowIndex = dgDetail.Rows.Add();
                DataGridViewRow dr = dgDetail.Rows[rowIndex];
                dr.Cells[colGWBSTree.Name].Value = node.GWBSTreeName;
                dr.Cells[colProgress.Name].Value = node.Progress;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
