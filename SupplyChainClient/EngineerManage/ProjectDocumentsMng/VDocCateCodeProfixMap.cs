using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.IO;
using System.Data.OleDb;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;


namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocCateCodeProfixMap : TBasicDataView
    {

        MDocumentMng model = new MDocumentMng();

        public VDocCateCodeProfixMap()
        {
            InitializeComponent();
            InitEvent();
            InitDate();
        }

        void InitDate()
        {
            ObjectQuery oq = new ObjectQuery();

            IList list = model.ObjectQuery(typeof(DocCateCodeProfixMap), oq);
            if (list.Count > 0)
            {
                DocCateCodeProfixMap map = list[0] as DocCateCodeProfixMap;
                txtKBCateCodeProfix.Text = map.KBDocCateCodeProfix;
                txtMBP_IRPCateCodeProfix.Text = map.MBP_IRPDocCateCodeProfix;
                txtKBCateCodeProfix.Tag = map;
            }

            btnSearch_Click(btnSearch, new EventArgs());

        }

        void InitEvent()
        {

            this.btnSaveProfixMap.Click += new EventHandler(btnSaveProfixMap_Click);

            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnAddObjTypeDefCate.Click += new EventHandler(btnAddObjTypeDefCate_Click);
            btnDelObjTypeDefCateConfig.Click += new EventHandler(btnDelObjTypeDefCateConfig_Click);
            btnSaveObjTypeDefCateConfig.Click += new EventHandler(btnSaveObjTypeDefCateConfig_Click);

            gridConfig.CellDoubleClick += new DataGridViewCellEventHandler(gridConfig_CellDoubleClick);
            gridConfig.CellEndEdit += new DataGridViewCellEventHandler(gridConfig_CellEndEdit);

            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            //右键复制一条明细
            tsmiCopy.Click += new EventHandler(tsmiCopy_Click);
        }

        void tsmiDel_Click(object sender, EventArgs e)
        {
            btnDelObjTypeDefCateConfig_Click(btnDelObjTypeDefCateConfig, new EventArgs());
        }

        //复制
        void tsmiCopy_Click(object sender, EventArgs e)
        {
            if (gridConfig.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要复制的行！");
                return;
            }

            List<ObjTypeDefaultCateConfig> listConfig = new List<ObjTypeDefaultCateConfig>();
            foreach (DataGridViewRow row in gridConfig.SelectedRows)
            {
                ObjTypeDefaultCateConfig config = row.Tag as ObjTypeDefaultCateConfig;

                listConfig.Add(config);
            }

            foreach (ObjTypeDefaultCateConfig config in listConfig)
            {
                ObjTypeDefaultCateConfig item = new ObjTypeDefaultCateConfig();
                item.ObjTypeName = config.ObjTypeName;
                item.ObjTypeDesc = config.ObjTypeDesc;
                item.ObjTypeAttributeName = config.ObjTypeAttributeName;
                item.ObjTypeAttributeDesc = config.ObjTypeAttributeDesc;
                item.ObjTypeAttributeValue = config.ObjTypeAttributeValue;
                item.DocCateName = config.DocCateName;
                item.DocCateCode = config.DocCateCode;

                int index = gridConfig.Rows.Add();
                DataGridViewRow row = gridConfig.Rows[index];

                row.Cells[colObjTypeName.Name].Value = item.ObjTypeName;
                row.Cells[colObjTypeDesc.Name].Value = item.ObjTypeDesc;
                row.Cells[colObjTypeAttName.Name].Value = item.ObjTypeAttributeName;
                row.Cells[colObjTypeAttDesc.Name].Value = item.ObjTypeAttributeDesc;
                row.Cells[colObjTypeAttValue.Name].Value = item.ObjTypeAttributeValue;
                row.Cells[colCateCode.Name].Value = item.DocCateCode;
                row.Cells[colCateName.Name].Value = item.DocCateName;

                row.Tag = item;
            }

            gridConfig.CurrentCell = gridConfig.Rows[gridConfig.Rows.Count - listConfig.Count].Cells[colObjTypeName.Name];
        }

        //文档分类编码前缀映射配置
        void btnSaveProfixMap_Click(object sender, EventArgs e)
        {
            if (txtKBCateCodeProfix.Text.Trim() == "")
            {
                MessageBox.Show("请输入“知识库分类编码前缀”！");
                txtKBCateCodeProfix.Focus();
                return;
            }
            if (txtMBP_IRPCateCodeProfix.Text.Trim() == "")
            {
                MessageBox.Show("请输入“项目管理IRP分类编码前缀”！");
                txtMBP_IRPCateCodeProfix.Focus();
                return;
            }

            string KBCateCodeProfix = txtKBCateCodeProfix.Text.Trim();
            string IRPCateCodeProfix = txtMBP_IRPCateCodeProfix.Text.Trim();

            DocCateCodeProfixMap map = null;
            if (txtKBCateCodeProfix.Tag == null)
            {
                map = new DocCateCodeProfixMap();
            }
            else
            {
                map = txtKBCateCodeProfix.Tag as DocCateCodeProfixMap;
            }
            map.KBDocCateCodeProfix = KBCateCodeProfix;
            map.MBP_IRPDocCateCodeProfix = IRPCateCodeProfix;

            map = model.SaveOrUpdate(map) as DocCateCodeProfixMap;

            txtKBCateCodeProfix.Tag = map;

            MessageBox.Show("保存成功！");
        }

        // 查询
        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery objectQuery = new ObjectQuery();

            if (txtObjTypeName.Text.Trim() != "")
            {
                objectQuery.AddCriterion(Expression.Like("ObjTypeName", txtObjTypeName.Text.Trim(), MatchMode.Anywhere));
            }
            if (txtObjTypeDesc.Text.Trim() != "")
            {
                objectQuery.AddCriterion(Expression.Like("ObjTypeDesc", txtObjTypeDesc.Text.Trim(), MatchMode.Anywhere));
            }
            if (txtObjTypeAttName.Text.Trim() != "")
            {
                objectQuery.AddCriterion(Expression.Like("ObjTypeAttributeName", txtObjTypeAttName.Text.Trim(), MatchMode.Anywhere));
            }
            if (txtObjTypeAttDesc.Text.Trim() != "")
            {
                objectQuery.AddCriterion(Expression.Like("ObjTypeAttributeDesc", txtObjTypeAttDesc.Text.Trim(), MatchMode.Anywhere));
            }
            if (txtObjTypeAttValue.Text.Trim() != "")
            {
                objectQuery.AddCriterion(Expression.Like("ObjTypeAttributeValue", txtObjTypeAttValue.Text.Trim(), MatchMode.Anywhere));
            }
            objectQuery.AddOrder(Order.Asc("ObjTypeName"));
            objectQuery.AddOrder(Order.Asc("ObjTypeAttributeName"));

            IList listConfig = model.ObjectQuery(typeof(ObjTypeDefaultCateConfig), objectQuery);

            AddConfigInfoInGrid(listConfig);
        }

        void gridConfig_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string colName = gridConfig.Columns[e.ColumnIndex].Name;

            if (colName == colCateCode.Name || colName == colCateName.Name)
            {
                VDocumentSortSelectByIRP vdss = new VDocumentSortSelectByIRP();
                vdss.ShowDialog();
                PLMWebServices.CategoryNode cate = vdss.ResultCate;
                if (cate != null)
                {
                    gridConfig.Rows[e.RowIndex].Cells[colCateCode.Name].Value = cate.CategoryCode;
                    gridConfig.Rows[e.RowIndex].Cells[colCateName.Name].Value = cate.CategoryName;

                    ObjTypeDefaultCateConfig config = gridConfig.Rows[e.RowIndex].Tag as ObjTypeDefaultCateConfig;
                    config.DocCateCode = cate.CategoryCode;
                    config.DocCateName = cate.CategoryName;

                    gridConfig.CurrentCell = colName == colCateCode.Name ? gridConfig.Rows[e.RowIndex].Cells[colCateName.Name] : gridConfig.Rows[e.RowIndex].Cells[colCateCode.Name];
                }
            }
        }

        void gridConfig_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            object tempValue = gridConfig.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            string value = "";
            if (tempValue != null)
                value = tempValue.ToString().Trim();

            ObjTypeDefaultCateConfig config = gridConfig.Rows[e.RowIndex].Tag as ObjTypeDefaultCateConfig;
            string colName = gridConfig.Columns[e.ColumnIndex].Name;

            if (colName == colObjTypeName.Name)
            {
                config.ObjTypeName = value.ToLower();
            }
            else if (colName == colObjTypeDesc.Name)
            {
                config.ObjTypeDesc = value;
            }
            else if (colName == colObjTypeAttName.Name)
            {
                config.ObjTypeAttributeName = value.ToLower();
            }
            else if (colName == colObjTypeAttDesc.Name)
            {
                config.ObjTypeAttributeDesc = value;
            }
            else if (colName == colObjTypeAttValue.Name)
            {
                config.ObjTypeAttributeValue = value;
            }
            else if (colName == colCateCode.Name)
            {
                config.DocCateCode = value;
            }
            else if (colName == colCateName.Name)
            {
                config.DocCateName = value;
            }

            gridConfig.Rows[e.RowIndex].Tag = config;
        }
        //新增
        void btnAddObjTypeDefCate_Click(object sender, EventArgs e)
        {
            int index = gridConfig.Rows.Add();
            DataGridViewRow row = gridConfig.Rows[index];

            ObjTypeDefaultCateConfig config = new ObjTypeDefaultCateConfig();
            row.Tag = config;

            gridConfig.CurrentCell = row.Cells[colObjTypeName.Name];
        }
        //删除
        void btnDelObjTypeDefCateConfig_Click(object sender, EventArgs e)
        {
            if (gridConfig.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要复制的行！");
                return;
            }
            if (MessageBox.Show("删除后不能恢复,确定要删除选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IList listConfig = new ArrayList();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridConfig.SelectedRows)
                {
                    ObjTypeDefaultCateConfig config = row.Tag as ObjTypeDefaultCateConfig;

                    if (!string.IsNullOrEmpty(config.Id))
                        listConfig.Add(config);

                    listRowIndex.Add(row.Index);
                }

                if (listConfig.Count > 0)
                    model.Delete(listConfig);

                listRowIndex.Sort();

                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridConfig.Rows.RemoveAt(listRowIndex[i]);
                }
            }
        }

        //对象类型关联文档分类配置
        void btnSaveObjTypeDefCateConfig_Click(object sender, EventArgs e)
        {

            IList listConfig = new ArrayList();

            foreach (DataGridViewRow row in gridConfig.Rows)
            {
                ObjTypeDefaultCateConfig config = row.Tag as ObjTypeDefaultCateConfig;
                listConfig.Add(config);
            }

            if (listConfig.Count > 0)
            {
                listConfig = model.SaveOrUpdate(listConfig);

                AddConfigInfoInGrid(listConfig);
            }

            MessageBox.Show("保存成功！");
        }

        private void AddConfigInfoInGrid(IList listConfig)
        {
            gridConfig.Rows.Clear();

            foreach (ObjTypeDefaultCateConfig item in listConfig)
            {
                int index = gridConfig.Rows.Add();
                DataGridViewRow row = gridConfig.Rows[index];

                row.Cells[colObjTypeName.Name].Value = item.ObjTypeName;
                row.Cells[colObjTypeDesc.Name].Value = item.ObjTypeDesc;
                row.Cells[colObjTypeAttName.Name].Value = item.ObjTypeAttributeName;
                row.Cells[colObjTypeAttDesc.Name].Value = item.ObjTypeAttributeDesc;
                row.Cells[colObjTypeAttValue.Name].Value = item.ObjTypeAttributeValue;
                row.Cells[colCateCode.Name].Value = item.DocCateCode;
                row.Cells[colCateName.Name].Value = item.DocCateName;

                row.Tag = item;
            }
        }
    }
}
