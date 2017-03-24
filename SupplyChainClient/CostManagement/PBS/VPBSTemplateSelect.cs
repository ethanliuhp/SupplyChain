using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VPBSTemplateSelect : Form
    {
        public PBSTemplate SelectItem { get; set; }

        public VPBSTemplateSelect()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var table = CommonMethod.CommonMethodSrv.GetData("select id, name, code, nodelevel, syscode, fullpath, typecode, typename, typebit, nodetype from thd_pbstemplate where nodelevel=1").Tables[0];
            if (table != null && table.Rows.Count > 0)
            {
                var list = table.Select().Select(a =>
                    new PBSTemplate()
                    {
                        Id = a["id"].ToString(),
                        Code = a["code"].ToString(),
                        Name = a["name"].ToString(),
                        Level = Convert.ToInt32(a["nodelevel"]),
                        SysCode = a["syscode"].ToString(),
                        FullPath = a["fullpath"].ToString(),
                        TypeCode = a["typecode"].ToString(),
                        TypeName = a["typename"].ToString(),
                        TypeBit = a["typebit"].ToString()
                    }
                );
                foreach (var item in list)
                {
                    var index = gvTable.Rows.Add();
                    gvTable.Rows[index].Cells[colCode.Name].Value = item.Code;
                    gvTable.Rows[index].Cells[colName.Name].Value = item.Name;
                    gvTable.Rows[index].Tag = item;
                }
            }
            gvTable.CellDoubleClick += (a, b) =>
            {
                if (b.RowIndex == -1) return;
                SelectItem = gvTable.Rows[b.RowIndex].Tag as PBSTemplate;
                this.Close();
            };
        }
    }
}
