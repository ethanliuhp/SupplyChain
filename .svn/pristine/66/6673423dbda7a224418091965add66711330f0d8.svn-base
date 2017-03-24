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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement
{
    public partial class VPersonManageSearchList : SearchList
    {
        private CPersonManage cPersonManage;
        public VPersonManageSearchList(CPersonManage cPersonManage)
        {
            InitializeComponent();
            this.cPersonManage = cPersonManage;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            string code =ClientUtil.ToDateTime(dgSearchResult.Rows[e.RowIndex].Cells["RealOperationDate"].Value).ToShortDateString().Replace("-","");
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;

            cPersonManage.Find(code.ToString(), ClientUtil.ToString(Id));
            
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("Id", "序列码");
            AddColumn("WeatherCondition", "天气状况");
            AddColumn("Post", "岗位类型");
            AddColumn("CreateDate", "日期");
            AddColumn("ConstructSite", "施工部位");
            AddColumn("HandlePerson", "负责人");
            AddColumn("RealOperationDate", "业务日期");
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
            foreach (PersonManage obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["Id"].Value = obj.Id;
                if (obj.WeatherGlass != null)
                {
                    dr.Cells["WeatherCondition"].Value = obj.WeatherGlass.WeatherCondition;
                }
                dr.Cells["Post"].Value = obj.Post;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["ConstructSite"].Value = obj.ConstructSite;
                dr.Cells["HandlePerson"].Value = obj.HandlePersonName;
                dr.Cells["RealOperationDate"].Value = obj.RealOperationDate.ToShortDateString();
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
