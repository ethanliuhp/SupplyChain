using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.Base.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.StockManage.Base.StationCategoryUI;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    public partial class StkStationCategory : UCCommonBase
    {
        private IList result = new ArrayList();

        virtual public IList Result
        {
            set { result = value; }
            get { return result; }
        }

        public void OpenSelect()
        {
            textBox.Text = "";
            result.Clear();
            pnlSelect_Click(this.pnlSelect, new EventArgs());
        }
        public void OpenSelect(string condition)
        {
            this.textBox.Text = condition;
            result.Clear();
            pnlSelect_Click(this.pnlSelect, new EventArgs());
        }
        private static IStationCategorySrv theStationCategorySrv;
        public StkStationCategory()
        {
            InitializeComponent();
            InitForm();
        }
        private void InitForm()
        {
            InitEvents();
        }
        private void InitEvents()
        {
            this.pnlSelect.Click += new EventHandler(pnlSelect_Click);
            this.textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            this.textBox.DoubleClick += new EventHandler(textBox_DoubleClick);
        }

        void textBox_DoubleClick(object sender, EventArgs e)
        {
            if (this.Enabled)
                pnlSelect_Click(this.pnlSelect, new EventArgs());
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pnlSelect_Click(this.pnlSelect, new EventArgs());
            }
        }
        void pnlSelect_Click(object sender, EventArgs e)
        {
            if (theStationCategorySrv == null)
                theStationCategorySrv = StaticMethod.GetService("StationCategorySrv") as IStationCategorySrv;

            //ObjectQuery oq = new ObjectQuery();
            object obj = new Object();
            string a = ConstObject.TheOperationOrg.Name;
            //if (this.textBox.Text.Trim() != "")
            //{
            //    SimpleExpression exp1 = Expression.Like("Name", this.textBox.Text.Trim() + "%");
            //    SimpleExpression exp2 = Expression.Eq("Code", this.textBox.Text.Trim());
            //    oq.AddCriterion(Expression.Or(exp1, exp2));
            //    IList list = theStationCategorySrv.GetStationCategory(typeof(StationCategory), oq);
            //    if (list.Count == 1)
            //    {
            //        StationCategory aa = list[0] as StationCategory;
            //        if (aa.State == 1)
            //        {
            //            this.textBox.Text = ClientUtil.ToString(aa.Name);
            //            result = list;
            //            return;
            //        }
            //    }
            //    obj = UCL.Locate("²Ö¿â±àÂëÎ¬»¤", StationCategoryExcuteType.CommonSelect, oq, this.FindForm());
            //}
            //else
            {
                obj = UCL.Locate("²Ö¿â±àÂëÎ¬»¤", StationCategoryExcuteType.CommonSelect, null, this.FindForm());
            }

            result = obj as IList;
            if (result != null && result.Count == 1)
            {
                StationCategory bb = result[0] as StationCategory;
                this.textBox.Text = ClientUtil.ToString(bb.Name);
            }
        }
    }
}
