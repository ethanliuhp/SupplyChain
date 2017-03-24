using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Main;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VPBSNodeAdd : Form
    {
        /// <summary>
        /// 需要加载的模版类型的父节点
        /// </summary>
        public PBSTemplate ParentTemplate { get; set; }
        /// <summary>
        /// 获取或设置窗口提示消息
        /// </summary>
        public bool EnableSelect { get; set; }
        /// <summary>
        /// 提供数据库服务的接口
        /// </summary>
        private IPBSTreeSrv service = new MPBSTree().Service;

        #region 需要返回的数据
        /// <summary>
        /// 确认的子节点名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 确认的子节点编码
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 确认的子节点描述
        /// </summary>
        public string NodeRemark { get; set; }
        /// <summary>
        /// 确认的子节点模版节点类型
        /// </summary>
        public PBSTemplate NodeTemplate { get; set; }
        /// <summary>
        /// 获取或设置要添加子节点的数量
        /// </summary>
        public int Number;
        #endregion

        public VPBSNodeAdd(PBSTemplate pbs)
        {
            InitializeComponent();
            EnableSelect = true;
            ParentTemplate = pbs;
            txtCount.Text = "1";
            Init();
            if (!EnableSelect) return;
            cbType.SelectedValueChanged += (a, b) => SelectItem((PBSTemplate)((ComboBox)a).SelectedItem);
            SelectItem((PBSTemplate)cbType.SelectedItem);

            btnSubmit.Click += (a, b) =>
            {
                if (!ValidateData()) return;
                NodeName = txtName.Text;
                NodeCode = txtCode.Text;
                NodeRemark = txtRemark.Text;
                NodeTemplate = (PBSTemplate)cbType.SelectedItem;
                this.Close();
            };
        }

        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="template"></param>
        private void SelectItem(PBSTemplate template)
        {
            if (template == null) return;
            txtName.Text = template.Name;
            txtCode.Text = template.Code;
        }

        /// <summary>
        /// 页面初始化
        /// </summary>
        private void Init()
        {
            if (ParentTemplate == null)
            {
                MessageBox.Show("无PBS模板信息！");
                return;
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentId", ParentTemplate.Id));
            oq.AddOrder(Order.Asc("Sort"));
            var list = service.ObjectQuery(typeof(PBSTemplate), oq).Select<PBSTemplate>();
            if (list == null || list.Count == 0)
            {
                EnableSelect = false;
                return;
            }
            cbType.DisplayMember = "Name";
            cbType.DataSource = list;
        }

        /// <summary>
        /// 验证输入数据
        /// </summary>
        private bool ValidateData()
        {
            if (!int.TryParse(txtCount.Text, out Number))
            {
                MessageBox.Show("添加数量只能输入数字...");
                return false;
            }
            if (Number == 0)
            {
                MessageBox.Show("添加数量不能为0...");
                return false;
            }
            if (cbType.SelectedItem == null)
            {
                MessageBox.Show("请选择结构类型...");
                return false;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("请输入结构名称...");
                return false;
            }
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                MessageBox.Show("请输入结构编码...");
                return false;
            }
            return true;
        }
    }
}
