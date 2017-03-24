using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using NHibernate;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VScheduleSetNodeAtrr : TBasicDataView
    {
        private CurrentProjectInfo projectInfo;
        private MProductionMng model = new MProductionMng();
        private WeekScheduleMaster CurWSDMaster;


        private List<WeekScheduleDetail> _rtnListWSDs;

        ///<summary>
        ///
        ///</summary>
        public List<WeekScheduleDetail> RtnWSDs
        {
            get { return this._rtnListWSDs; }
        }

        private List<GWBSTree> _listWBSTree;

        ///<summary>
        ///
        ///</summary>
        public virtual List<GWBSTree> ListWBSTree
        {
            get { return this._listWBSTree; }
        }

        private bool _isChanged;
        ///<summary>
        ///是否发生修改
        ///</summary>
        public bool IsChanged
        {
            get { return this._isChanged; }
        }

        public VScheduleSetNodeAtrr(CurrentProjectInfo _projectInfo, WeekScheduleMaster _CurWSDMaster)
        {
            this.projectInfo = _projectInfo;
      
            this.CurWSDMaster = _CurWSDMaster;

            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            //projectInfo = StaticMethod.GetProjectInfo();
            this._isChanged = false;
            LoadForm();
        }

        private void LoadForm()
        {
            ObjectQuery oq = new ObjectQuery();
            if (!string.IsNullOrEmpty(projectInfo.Id))
                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            oq.AddCriterion(Expression.Or(Expression.Eq("IsFixed", 1), Expression.Eq("ProductionCuringNode", true)));
            _listWBSTree = model.ProductionManagementSrv.GetGWBSTreesOQ(oq).OfType<GWBSTree>().ToList<GWBSTree>();

            ModelToView();
        }

        private bool ModelToView()
        {
            try
            {
                this.dgDetail.Rows.Clear();
                foreach (var item in ListWBSTree)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[this.colProjectTastType.Name, i].Value = item.Name;
                    
                         
                    DataGridViewCellStyle st = new DataGridViewCellStyle();
                    st.BackColor = item.IsFixed == 1 ? Color.Pink : Color.White;

                    this.dgDetail[this.colHTStartDate.Name, i].ReadOnly = item.IsFixed ==1;
                    this.dgDetail[this.colHTEndDate.Name, i].ReadOnly = item.IsFixed == 1;

                    this.dgDetail[this.colHTStartDate.Name, i].Style = st;
                    this.dgDetail[this.colHTEndDate.Name, i].Style = st;

                    this.dgDetail[this.colHTStartDate.Name, i].Value = item.StartPlanBeginDate;
                    this.dgDetail[this.colHTEndDate.Name, i].Value = item.StartPlanEndDate;



                    DataGridViewCellStyle st1 = new DataGridViewCellStyle();
                    st1.BackColor = item.ProductionCuringNode ? Color.Pink : Color.White;

                    this.dgDetail[this.colSCStartDate.Name, i].ReadOnly = !item.ProductionCuringNode;
                    this.dgDetail[this.colSCEndDate.Name, i].ReadOnly = !item.ProductionCuringNode;

                    this.dgDetail[this.colSCStartDate.Name, i].Style = st1;
                    this.dgDetail[this.colSCEndDate.Name, i].Style = st1;
                   
                    this.dgDetail[this.colSCStartDate.Name, i].Value = item.ProCurBeginDate;
                    this.dgDetail[this.colSCEndDate.Name, i].Value = item.ProCurEndDate;

                    this.dgDetail.Rows[i].Tag = item;
                }
                return true;
            }
            catch
            {
                
                return false;
            }
        }

        private bool ViewToModel()
        {
            try
            {
                foreach (DataGridViewRow item in dgDetail.Rows)
                {
                    GWBSTree detail = item.Tag as GWBSTree;
                    if (detail.IsFixed == 1)
                    {
                        detail.StartPlanBeginDate = ClientUtil.ToDateTime(item.Cells[this.colHTStartDate.Name].Value);
                        detail.StartPlanEndDate = ClientUtil.ToDateTime(item.Cells[this.colHTEndDate.Name].Value);

                    }
                    if (detail.ProductionCuringNode)
                    {
                        detail.ProCurBeginDate = ClientUtil.ToDateTime(item.Cells[this.colSCStartDate.Name].Value);
                        detail.ProCurEndDate = ClientUtil.ToDateTime( item.Cells[this.colSCEndDate.Name].Value);

                        WeekScheduleDetail wsdd = CurWSDMaster.Details.ToList<WeekScheduleDetail>().Find(p => p.GWBSTree.Id == detail.Id);
                        if (wsdd != null)
                        {
                            wsdd.PlannedBeginDate = ClientUtil.ToDateTime(item.Cells[this.colHTStartDate.Name].Value);
                            wsdd.PlannedEndDate = ClientUtil.ToDateTime(item.Cells[this.colHTEndDate.Name].Value);

                            wsdd.PlannedDuration = ClientUtil.ToDateTime(item.Cells[this.colHTEndDate.Name].Value).Subtract(ClientUtil.ToDateTime(item.Cells[this.colHTStartDate.Name].Value)).Days + 1;
                            if (this._rtnListWSDs == null)
                                this._rtnListWSDs = new List<WeekScheduleDetail>();
                            this._rtnListWSDs.Add(wsdd);
                        }
                        
                    }
                     if (this._listWBSTree == null)
                                this._listWBSTree = new List<GWBSTree>();
                     this._listWBSTree.Add(detail);
                }
                return true;
            }
            catch
            {
                
                return false;
            }
           
        }

        private void InitEvent()
        {
            this.btnEnter.Click += new EventHandler(btnEnter_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            ViewToModel();
            this._isChanged = true;
            model.ProductionManagementSrv.UpdateByDao(this._listWBSTree);
            this.btnEnter.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnEnter.FindForm().Close(); 
        }
    }
}
