using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VTemplateSelect : TBasicDataView
    {
        /// <summary>
        /// 服务
        /// </summary>
        public IPBSTreeSrv service;
        /// <summary>
        /// 所有的结构类型
        /// </summary>
        public List<PBSTemplateType> TypeList { get; set; }
        /// <summary>
        /// 选择的类型
        /// </summary>
        public PBSTemplateType SelectItem { get; set; }
        public VTemplateSelect()
        {
            InitializeComponent();
            TypeList = new List<PBSTemplateType>();
            service = new MPBSTree().Service;
            Init();
        }

        private void Init()
        {
            var oq = new ObjectQuery();
            oq.AddOrder(Order.Asc("CreateTime"));
            var result = service.ObjectQuery(typeof(PBSTemplateType), oq);
            if (result == null || result.Count == 0) return;
            foreach (PBSTemplateType item in result)
                TypeList.Add(item);
            LoadList(TypeList);

            // 事件
            txtKey.KeyDown += (a, b) =>
            {
                if (b.KeyCode != Keys.Enter) return;
                Search(txtKey.Text);
            };
            gvTable.CellDoubleClick += (a, b) => { SelectItem = gvTable.Rows[b.RowIndex].Tag as PBSTemplateType; this.Close(); };
        }

        /// <summary>
        /// 根据关键字搜索指定的结构类型
        /// </summary>
        /// <param name="key"></param>
        private void Search(string key)
        {
            var result = TypeList.Where(a => a.Code.Contains(key) || a.Name.Contains(key) || a.CodeBit.Contains(key));
            LoadList(result);
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        private void LoadList(IEnumerable<PBSTemplateType> list)
        {
            gvTable.Rows.Clear();
            foreach (var item in list)
            {
                var rowIndex = gvTable.Rows.Add();
                var row = gvTable.Rows[rowIndex];
                row.Cells[colCode.Name].Value = item.Code;
                row.Cells[colName.Name].Value = item.Name;
                row.Tag = item;
            }
        }
    }
}
