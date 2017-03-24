using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng
{
    public partial class VAcceptanceInspectionSelect : TBasicDataView
    {
        private MAcceptanceInspectionMng model = new MAcceptanceInspectionMng();

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VAcceptanceInspectionSelect()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            this.btnSearchProject.Click +=new EventHandler(btnSearchProject_Click);
            VBasicDataOptr.InitWBSCheckRequir(txtSpecail, true);
           
        }
        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void btnOK_Click(object sender,EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (dgDetail.Rows.Count > 0 && this.dgDetail.SelectedRows == null)
                {
                    MessageBox.Show("请选择一条检验批信息！");
                    return;
                }
                AcceptanceInspection detail = dgDetail.CurrentRow.Tag as AcceptanceInspection;
                result.Add(detail);
            }
            this.Close();
        }

        void btnCancel_Click(object seender,EventArgs e)
        {
            this.Close();
        }

        //选择检验批
        void btnSearchProject_Click(object sender, EventArgs e)
        {
            VInspectionLotSelect vss = new VInspectionLotSelect();
            vss.ShowDialog();
            IList list = vss.Result;
            if (list == null || list.Count == 0) return;
            foreach (InspectionLot detail in list)
            {
                txtProjectTask.Text = detail.Code;
                txtProjectTask.Tag = detail;
            }
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        //查询
        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Ge("CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", dtpDateEnd.Value.AddDays(1).Date));
            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, txtCodeEnd.Text));
            }

            if (txtCreatePerson.Text != "" && txtCreatePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("CreatePerson", txtCreatePerson.Result[0] as PersonInfo));
            }
            try
            {
                list = model.AcceptanceInspectionSrv.GetAcceptanceInspection(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }

        }

        /// <summary>
        /// 显示主表及明细信息
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgDetail.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (AcceptanceInspection detail in masterList)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = detail;
                dgDetail[colCode.Name, rowIndex].Value = detail.Code;
                dgDetail[colInsLotCode.Name, rowIndex].Value = detail.InsLotCode;
                dgDetail[colInspectionConclusion.Name, rowIndex].Value = detail.InspectionConclusion;
                dgDetail[colInspectionContent.Name, rowIndex].Value = detail.InspectionContent;
                dgDetail[colInspectionSituation.Name, rowIndex].Value = detail.InspectionStatus;
                dgDetail[colInspectionSpecail.Name, rowIndex].Value = detail.InspectionSpecial;
                dgDetail[colCreateDate.Name, rowIndex].Value = detail.CreateDate.ToShortDateString();
                dgDetail[colCreatePerson.Name, rowIndex].Value = detail.CreatePersonName;
                dgDetail[colHandlePerson.Name, rowIndex].Value = detail.HandlePersonName;    
            }
            if (dgDetail.Rows.Count == 0) return;
            dgDetail.CurrentCell = dgDetail[1, 0];
        }

    }
}
