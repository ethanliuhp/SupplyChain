using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng
{
    public partial class VLaborSporadicPlanSearchList : SearchList
    {
        private CLaborSporadicMng cLaborSporadic;

        public VLaborSporadicPlanSearchList(CLaborSporadicMng cLaborSporadic)
        {
            InitializeComponent();
            this.cLaborSporadic = cLaborSporadic;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex < 0) return;
            LaborSporadicMaster obj = dgSearchResult.Rows[e.RowIndex].Tag as LaborSporadicMaster;
            if (obj != null)
            {
                string laborState = obj.LaborState;
                if (laborState == EnumLaborType.分包签证.ToString())
                {
                    cLaborSporadic.Find(obj.Code, EnumLaborType.分包签证, obj.Id);
                } if (laborState == EnumLaborType.计时派工.ToString())
                {
                    cLaborSporadic.Find(obj.Code, EnumLaborType.计时派工, obj.Id);
                } if (laborState == "零星用工")
                {
                    cLaborSporadic.Find(obj.Code, EnumLaborType.派工, obj.Id);
                }
                if (laborState == EnumLaborType.代工.ToString())
                {
                    cLaborSporadic.Find(obj.Code, EnumLaborType.代工, obj.Id);
                }
                if (laborState == EnumLaborType.逐日派工.ToString())
                {
                    cLaborSporadic.Find(obj.Code, EnumLaborType.逐日派工, obj.Id);
                }

                //cLaborSporadic.Find(obj.Code,obj.
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单据号");
            AddColumn("State", "状态");
            AddColumn("BearTeamName", "劳务队伍");
            AddColumn("ResourceTypeName", "派工类型");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "制单日期");
            AddColumn("Descript", "备注");
            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        public void RefreshData(IList lst)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();

            foreach (LaborSporadicMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Tag = obj;
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["BearTeamName"].Value = ClientUtil.ToString(obj.BearTeamName);
                dr.Cells["ResourceTypeName"].Value = ClientUtil.ToString(obj.LaborState);
                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                string a = obj.CreateDate.ToString();
                string[] aArray = a.Split(' ');
                string stra = aArray[0];
                dr.Cells["CreateDate"].Value = stra;
                dr.Cells["Descript"].Value = obj.Descript;
            }
            this.ViewShow();
            this.dgSearchResult.AutoResizeColumns();
        }
        public void RemoveRow(string id)
        {
            foreach (DataGridViewRow row in this.dgSearchResult.Rows)
            {
                if (!row.IsNewRow)
                {
                    if (row.Cells["Id"].Value.ToString() == id)
                    {
                        this.dgSearchResult.Rows.Remove(row);
                        break;
                    }
                }
            }
        }
    }
}
