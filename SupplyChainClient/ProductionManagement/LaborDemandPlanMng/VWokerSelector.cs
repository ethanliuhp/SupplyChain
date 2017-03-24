using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng
{
    public partial class VWokerSelector : TBasicDataView
    {
        MLaborDemandPlanMng model = new MLaborDemandPlanMng();
        private int totalRecords = 0;
        private LaborDemandPlanDetail curBillDetail;
        /// <summary>
        /// 当前明细单据
        /// </summary>
        public LaborDemandPlanDetail CurBillDetail
        {
            get { return curBillDetail; }
            set { curBillDetail = value; }
        }

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VWokerSelector()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
            InitData();
        }

        public void InitData()
        {
            //给工种添加下拉列表框的信息
            VBasicDataOptr.InitWokerType(colWokerType, false);
            //dgDetail.ContextMenuStrip = cmsDg;
        }

        private void InitForm()
        {
            this.Title = "工种信息引用";
        }

        private void InitEvent()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            Load += new EventHandler(VWokerTypeSelector_Load);
        }

        void VWokerTypeSelector_Load(object sender, EventArgs e)
        {
            foreach (LaborDemandWorkerType obj in Result)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = obj;
                dgDetail[colPersonNum.Name, rowIndex].Value = obj.PeopleNum;
                dgDetail[colWokerType.Name, rowIndex].Value = obj.WorkerType;
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
