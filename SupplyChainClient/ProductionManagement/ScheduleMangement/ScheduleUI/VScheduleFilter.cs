using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using FlexCell;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VScheduleFilter : TBasicDataView
    {
        private DateTime defaultTime = new DateTime(1900, 1, 1);
        private ObjectQuery _tempOQ;

        ///<summary>
        ///
        ///</summary>
        public ObjectQuery TempOQ
        {
            get { return this._tempOQ; }
        }
        public VScheduleFilter()
        {
            InitializeComponent();
            InitForm();
            this.InitEvent();
        }

        private void InitForm()
        {
            this.dtpDateBegin.Checked = false;
            this.dtpDateEnd.Checked = false;

            InitComboBox();
            cbPlanSetStiu.Text = "请选择...";
        }

        private void InitComboBox()
        {
            if (cbPlanSetStiu.Items.Count == 0)
            {
                cbPlanSetStiu.Items.Add("请选择...");
                cbPlanSetStiu.Items.Add("已设置");
                cbPlanSetStiu.Items.Add("已设置未审核");
                cbPlanSetStiu.Items.Add("已审核");
                cbPlanSetStiu.Items.Add("未设置");
           
            }
        }

     


        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.dtpDateBegin.ValueChanged += new EventHandler(dtpDateBegin_ValueChanged);
            this.dtpDateEnd.ValueChanged += new EventHandler(dtpDateEnd_ValueChanged);
        }

        void dtpDateEnd_ValueChanged(object sender, EventArgs e)
        {

            if (this.dtpDateBegin.Checked  && this.dtpDateBegin.Value.Date > this.dtpDateEnd.Value.Date)
            {
                MessageBox.Show("开始时间不能小于结束时间，请重置");
                this.dtpDateEnd.Value = this.dtpDateBegin.Value;
                this.dtpDateEnd.Checked = false;
                this.dtpDateEnd.Focus();
            }
        }

        void dtpDateBegin_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtpDateBegin.Checked && this.dtpDateBegin.Value.Date > this.dtpDateEnd.Value.Date)
            {
                MessageBox.Show("开始时间不能小于结束时间，请重置");
                this.dtpDateBegin.Value = this.dtpDateEnd.Value;
                this.dtpDateBegin.Checked = false;
                this.dtpDateBegin.Focus();
            }

        }
        void btnOK_Click(object sender, EventArgs e)
        {
            _tempOQ = new ObjectQuery();
            if (this.txtLevel.Text.Trim() != "")
            {
                this._tempOQ.AddCriterion(Expression.Le("Level", ClientUtil.ToInt(this.txtLevel.Text)));
            }
            if (this.dtpDateBegin.Checked && !this.dtpDateEnd.Checked)
            {
                DateTime beginTime = dtpDateBegin.Value.Date;
            

                Disjunction dis = new Disjunction();
                dis.Add(Expression.And(Expression.Le("PlannedBeginDate", beginTime), Expression.Ge("PlannedEndDate", beginTime)));
 
                _tempOQ.AddCriterion(dis);
            }
            if (!this.dtpDateBegin.Checked && this.dtpDateEnd.Checked)
            {
                DateTime endTime = dtpDateEnd.Value.Date;


                Disjunction dis = new Disjunction();
                dis.Add(Expression.And(Expression.Le("PlannedBeginDate", endTime), Expression.Ge("PlannedEndDate", endTime)));

                _tempOQ.AddCriterion(dis);
            }

            if (this.dtpDateBegin.Checked  && this.dtpDateEnd.Checked )
            { 
                DateTime beginTime = dtpDateBegin.Value.Date;
                DateTime endTime = dtpDateEnd.Value.Date;

                 Disjunction dis = new Disjunction();
                 dis.Add(Expression.And(Expression.Le("PlannedBeginDate", beginTime), Expression.Ge("PlannedEndDate", beginTime)));
                 dis.Add(Expression.And(Expression.Le("PlannedBeginDate", endTime), Expression.Ge("PlannedEndDate", endTime)));
                 dis.Add(Expression.And(Expression.Gt("PlannedBeginDate", beginTime), Expression.Lt("PlannedEndDate", endTime)));
                 _tempOQ.AddCriterion(dis);

            }
            if (cbPlanSetStiu.Text == "请选择...")
            {
                Disjunction dis = new Disjunction();
                dis.Add(Expression.Eq("State", DocumentState.Edit));
                dis.Add(Expression.Or(Expression.Eq("State", DocumentState.InExecute), Expression.Eq("Level", 1)));

                _tempOQ.AddCriterion(dis);
            }



            if (cbPlanSetStiu.Text != "请选择...")
            {
                
                switch (cbPlanSetStiu.Text)
                {
                    case "已设置":
                        Disjunction dis1 = new Disjunction();
                        dis1.Add(Expression.Or(Expression.Not(Expression.Eq("PlannedBeginDate", defaultTime)), Expression.Not(Expression.Eq("PlannedEndDate", defaultTime))));
                        dis1.Add(Expression.Eq("Level", 1));
                        this._tempOQ.AddCriterion(dis1);
                        break;
                    case "已设置未审核":
                        Disjunction dis2 = new Disjunction();
                        dis2.Add(Expression.Or(Expression.Not(Expression.Eq("PlannedBeginDate", defaultTime)), Expression.Not(Expression.Eq("PlannedEndDate", defaultTime))));
                        //dis2.Add(Expression.Eq("Level", 1));
                     
                        this._tempOQ.AddCriterion(dis2);

                        this._tempOQ.AddCriterion(Expression.Or(Expression.Not(Expression.Eq("State", DocumentState.InExecute)),Expression.Eq("Level", 1)));


                        break;
                    case "已审核":
                        Disjunction dis3 = new Disjunction();
                        dis3.Add(Expression.Eq("State", DocumentState.InExecute));
                        dis3.Add(Expression.Eq("Level", 1));
                        this._tempOQ.AddCriterion(dis3);
                        break;
                    case "未设置":
                        Disjunction dis4 = new Disjunction();
                        dis4.Add(Expression.And(Expression.Eq("PlannedBeginDate", defaultTime), Expression.Eq("PlannedEndDate", defaultTime)));
                        dis4.Add( Expression.Eq("Level", 1));
                        this._tempOQ.AddCriterion(dis4);
                        break;
                }
            }

            this.btnOK.FindForm().Close();
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }

    }
}
