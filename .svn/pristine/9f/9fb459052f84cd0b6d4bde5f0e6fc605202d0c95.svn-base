using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using IRPServiceModel.Domain.Document;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng
{
    public partial class VDocumentCabinetMng : TBasicDataView
    {
        private MDocumentCategory model = null;
        private CurrentProjectInfo projectInfo = null;
        private TreeNode oprNode;//当前操作节点
        private DocumentCategory oprCate;//当前操作文档分类
        private FileCabinet fileCabinet;//文件柜
        public VDocumentCabinetMng()
        {
            InitializeComponent();
            InitData();
        }
        void InitData()
        {
            txtCateAmt.DataSource = (Enum.GetNames(typeof(TransportProtocolsEnum)));
            txtUseState.DataSource = (Enum.GetNames(typeof(UseState)));
            txtUseState.SelectedIndex = 1;
            txtCateAmt.SelectedIndex = 0;
            model = new MDocumentCategory();
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            InitEvent();
            LoadFileCabinet();
        }

        void InitEvent()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click +=new EventHandler(btnDelete_Click);
            btnAdd.Click +=new EventHandler(btnAdd_Click);
            dgDetail.CellClick +=new DataGridViewCellEventHandler(dgDetail_CellClick);
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                if (dgDetail.CurrentRow.Cells[colSende.Name].Value != null)
                {
                    if (dgDetail.Rows[dgDetail.Rows.Count - 1].Cells[colSende.Name].Value != null)
                    {
                        fileCabinet = new FileCabinet();
                        int i = dgDetail.Rows.Add();
                        dgDetail.ClearSelection();
                        dgDetail.CurrentCell = dgDetail.Rows[i].Cells[colCabinetName.Name];
                        dgDetail.Rows[i].Tag = fileCabinet;
                        Clear();
                    }
                    else
                    {
                        MessageBox.Show("表格中有新行，不能再次新增！");
                    }
                }
                else
                {
                    MessageBox.Show("当前信息还未保存！");
                }
            }
            else
            {
                fileCabinet = new FileCabinet();
                int i = dgDetail.Rows.Add();
                dgDetail.Rows[i].Tag = fileCabinet;
                Clear();
            }
        }

        void LoadFileCabinet()
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = model.ObjectQuery(typeof(FileCabinet), oq);
            if (list.Count > 0)
            {
                dgDetail.Rows.Clear();
                foreach (FileCabinet cabinet in list)
                {
                    int i = dgDetail.Rows.Add();
                    dgDetail[colCabinetName.Name, i].Value = cabinet.Name;
                    dgDetail[colCabinetPath.Name, i].Value = cabinet.Path;
                    dgDetail[colPwd.Name, i].Value = cabinet.UserPwd;
                    dgDetail[colSearch.Name, i].Value = cabinet.QueryStr;
                    dgDetail[colSende.Name, i].Value = EnumUtil<TransportProtocolsEnum>.GetDescription(cabinet.TransportProtocal);
                    dgDetail[colUsedState.Name, i].Value = EnumUtil<UseState>.GetDescription(cabinet.UsedState);
                    dgDetail[colUser.Name, i].Value = cabinet.UserName;
                    dgDetail[colYu.Name, i].Value = cabinet.DomainName;
                    dgDetail[colIP.Name, i].Value = cabinet.ServerName;
                    dgDetail.Rows[i].Tag = cabinet;
                }
                if (dgDetail.Rows.Count > 0)
                {
                    dgDetail.CurrentCell = dgDetail.Rows[0].Cells[0];
                    dgDetail_CellClick(dgDetail, new DataGridViewCellEventArgs(0, 0));
                }
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    if (fileCabinet.Id != null)
                    {
                        if ((bool)model.Delete(fileCabinet))
                        {
                            MessageBox.Show("删除成功！");
                            LoadFileCabinet();
                        }
                    }
                    Clear();
                }
            }
        }

        void Clear()
        {
            txtCateYuName.Text = "";
            txtCateName.Text = "";
            txtCatePath.Text = "";
            txtCateSeach.Text = "";
            txtCateIP.Text = "";
            txtCateUser.Text = "";
            txtCatePwd.Text = "";
            txtUseState.SelectedIndex = 1;
            txtCateAmt.SelectedIndex = 0;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgDetail.CurrentRow != null)
                {
                    //if (txtCateName.Text == "")
                    //{
                    //    MessageBox.Show("文件柜名称不能为空！");
                    //    return;
                    //}
                    if (txtCateIP.Text == "")
                    {
                        MessageBox.Show("服务器IP或域名不能为空！");
                        return;
                    }
                    if (txtUseState.Text == "")
                    {
                        MessageBox.Show("使用状态不能为空！");
                        return;
                    }
                    if (txtCateAmt.Text == "")
                    {
                        MessageBox.Show("传输协议不能为空！");
                        return;
                    }
                    fileCabinet.DomainName = txtCateYuName.Text;
                    fileCabinet.Name = txtCateName.Text;
                    fileCabinet.Path = txtCatePath.Text;
                    fileCabinet.QueryStr = txtCateSeach.Text;
                    fileCabinet.ServerName = txtCateIP.Text;
                    fileCabinet.TransportProtocal = EnumUtil<TransportProtocolsEnum>.FromDescription(txtCateAmt.Text);
                    fileCabinet.UsedState = EnumUtil<UseState>.FromDescription(txtUseState.Text);
                    fileCabinet.UserName = txtCateUser.Text;
                    fileCabinet.UserPwd = txtCatePwd.Text;
                    fileCabinet = model.SaveOrUpdatefile(fileCabinet);
                    MessageBox.Show("保存成功！");
                    LoadFileCabinet();
                }
                else
                {
                    MessageBox.Show("请先点击新增！");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存出错：" + exp.Message);
            }
        }

        void dgDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            fileCabinet = dgDetail.CurrentRow.Tag as FileCabinet;
            txtCateYuName.Text = fileCabinet.DomainName;
            txtCateName.Text = fileCabinet.Name;
            txtCatePath.Text = fileCabinet.Path;
            txtCateSeach.Text = fileCabinet.QueryStr;
            txtCateIP.Text = fileCabinet.ServerName;
            txtCateAmt.Text = EnumUtil<TransportProtocolsEnum>.GetDescription(fileCabinet.TransportProtocal);
            txtUseState.Text = EnumUtil<UseState>.GetDescription(fileCabinet.UsedState);
            txtCateUser.Text = fileCabinet.UserName;
            txtCatePwd.Text = fileCabinet.UserPwd;
        }
    }
}
