using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
//测试
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VSelectDocumentsList : TBasicDataView
    {
        PLMWebServicesByKB.ProjectDocument[] list = null;
        //IList downDocList = new List<PLMWebServicesByKB.ProjectDocument>();

        string isAJudge = string.Empty;//为空上传模版  不为空修改模版

        private IList resultList;

        public IList ResultList
        {
            get { return resultList; }
            set { resultList = value; }
        }
        string userName = string.Empty;
        string jobId = string.Empty;

        public VSelectDocumentsList()
        {
            InitializeComponent();
            InitEvent();
            InitcomboBoxData();
            IntData();
        }

        public VSelectDocumentsList(string s)
        {
            InitializeComponent();
            isAJudge = s;
            InitEvent();
            InitcomboBoxData();
            IntData();
            if (isAJudge != "")
            {
                lnkCheckAll.Enabled = false;
                //lnkInverse.Enabled = false;
            }
        }

        private void InitEvent()
        {
            this.btnSelectProject.Click += new EventHandler(btnSelectProject_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            //this.btnExportFileData.Click += new EventHandler(btnExportFileData_Click);
            //this.btnUpNewDocument.Click += new EventHandler(btnUpNewDocument_Click);
            //this.dgvDocumentList.CellMouseDown += new DataGridViewCellMouseEventHandler(dgvDocumentList_CellMouseDown);
            //this.cmsDocumentList.ItemClicked += new ToolStripItemClickedEventHandler(cmsDocumentList_ItemClicked);
            this.lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            this.lnkInverse.Click += new EventHandler(lnkInverse_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
        }

        #region 选择责任人
        /// <summary>
        /// 选择责任人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelectProject_Click(object sender, EventArgs e)
        {
            VDepartSelector frm = new VDepartSelector("1");

            frm.ShowDialog();

            if (frm.Result != null && frm.Result.Count > 0)
            {
                CurrentProjectInfo selectProject = frm.Result[0] as CurrentProjectInfo;
                if (selectProject != null)
                {
                    txtProjectName.Text = selectProject.Name;
                    txtProjectName.Tag = selectProject;
                }
            }
        }
        #endregion


        /// <summary>
        /// 给复选框默认值
        /// </summary>
        void InitcomboBoxData()
        {
            //this.cmbDocumentLibrary.Items.AddRange(new object[] { "知识库" });
            //文档信息类型
            foreach (string infoType in Enum.GetNames(typeof(PLMWebServicesByKB.DocumentInfoType)))
            {
                cmbDocumentInforType.Items.Add(infoType);
            }
            //文档状态
            foreach (string infoType in Enum.GetNames(typeof(PLMWebServicesByKB.DocumentState)))
            {
                cmbDocumentStatus.Items.Add(infoType);
            }
            //版本
            foreach (string infotype in Enum.GetNames(typeof(PLMWebServicesByKB.DocumentQueryVersion)))
            {
                cmbDocumentVersion.Items.Add(infotype);
            }
            //this.cmbDocumentLibrary.DropDownStyle = ComboBoxStyle.DropDownList;
            //this.cmbDocumentInforType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDocumentVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            //this.cmbDocumentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDocumentVersion.SelectedIndex = 0;
            //cmbDocumentLibrary.SelectedIndex = 0;
            cmbDocumentInforType.SelectedIndex = 0;
            cmbDocumentStatus.SelectedIndex = 0;
            //cmbDocumentInforType.Text = "图片";
        }
        void IntData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            //if (txtProjectName.Text.Trim() == "" || txtProjectName.Tag == null)
            //{
            //    MessageBox.Show("请选择一个文档库！");
            //    txtProjectName.Focus();
            //    return;
            //}
            List<PLMWebServicesByKB.ProjectDocument> documentList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();

            CurrentProjectInfo projectInfo = txtProjectName.Tag as CurrentProjectInfo;

            PLMWebServicesByKB.DocumentInfoType? docInfoType = null;

            foreach (PLMWebServicesByKB.DocumentInfoType type in Enum.GetValues(typeof(PLMWebServicesByKB.DocumentInfoType)))
            {
                if (type.ToString() == cmbDocumentInforType.Text.Trim())
                {
                    docInfoType = type;
                    break;
                }
            }

            PLMWebServicesByKB.DocumentState? docState = null;
            foreach (PLMWebServicesByKB.DocumentState state in Enum.GetValues(typeof(PLMWebServicesByKB.DocumentState)))
            {
                if (state.ToString() == cmbDocumentStatus.Text.Trim())
                {
                    docState = state;
                    break;
                }
            }
            PLMWebServicesByKB.DocumentQueryVersion docVersion = 0;
            foreach (PLMWebServicesByKB.DocumentQueryVersion qversion in Enum.GetValues(typeof(PLMWebServicesByKB.DocumentQueryVersion)))
            {
                if (qversion.ToString() == cmbDocumentVersion.Text.Trim())
                {
                    docVersion = qversion;
                    break;
                }
            }

            PLMWebServicesByKB.ErrorStack es = StaticMethod.GetProjectDocumentByKB(null, null, txtDocumentCode.Text.Trim(),
                txtDocumentName.Text.Trim(), txtDocumentExpandedName.Text.Trim(), docInfoType, docVersion, null, null, dtpDateBegin.Value, dtpDateEnd.Value.Date.AddDays(1).AddSeconds(-1),
                txtHandlePerson.Text.Trim() == "" ? null : txtHandlePerson.Text.Trim(), docState, txtKeywordSearch.Text, null, StaticMethod.KB_System_UserName, StaticMethod.KB_System_JobId, null, out list);

            if (es != null)
            {
                MessageBox.Show("查询失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dgvDocumentList.Rows.Clear();
            if (list != null)
            {
                //documentList
                documentList.AddRange(list);
                if (documentList.Count > 0 && documentList != null)
                {
                    foreach (PLMWebServicesByKB.ProjectDocument doc in documentList)
                    {
                        //【文档名称】、【文档扩展名】、【文档信息类型】、【文档代码】、【文档版本号】、【创建时间】、【责任人】、【文档状态】。
                        int rowIndex = this.dgvDocumentList.Rows.Add();
                        dgvDocumentList[colDocumentName.Name, rowIndex].Value = doc.Name;
                        dgvDocumentList[colDocumentExpandedName.Name, rowIndex].Value = doc.ExtendName;
                        dgvDocumentList[colDocumentInforType.Name, rowIndex].Value = doc.DocType.ToString();
                        dgvDocumentList[colDocumentCode.Name, rowIndex].Value = doc.Code;
                        dgvDocumentList[colDocumentVersion.Name, rowIndex].Value = doc.Version;
                        dgvDocumentList[colCreateTime.Name, rowIndex].Value = doc.CreateTime;
                        dgvDocumentList[colDutyPerson.Name, rowIndex].Value = doc.OwnerName;
                        dgvDocumentList[colDocumentStatus.Name, rowIndex].Value = doc.State.ToString();
                        this.dgvDocumentList.Rows[rowIndex].Tag = doc;
                    }
                }
                lblCount.Text = documentList.Count.ToString();
            }
            else
            {
                MessageBox.Show("未找到数据！");
            }

        }


        void btnOK_Click(object sender, EventArgs e)
        {
            resultList = GetCheckedDocument();
            if (isAJudge != "")
            {
                if (resultList.Count == 0)
                {
                    MessageBox.Show("请选择一个模版！");
                    return;
                }
                if (resultList.Count > 1)
                {
                    MessageBox.Show("只能选择一个模版！");
                    return;
                }
            }
            this.Close();
        }


        #region 得到选中文档的集合
        /// <summary>
        /// 得到选中文档的集合
        /// </summary>
        /// <returns></returns>
        IList GetCheckedDocument()
        {
            IList selctedDocList = new ArrayList();
            foreach (DataGridViewRow row in this.dgvDocumentList.Rows)
            {
                if ((bool)row.Cells[select.Name].EditedFormattedValue)
                {
                    PLMWebServicesByKB.ProjectDocument doc = row.Tag as PLMWebServicesByKB.ProjectDocument;
                    selctedDocList.Add(doc);
                }
            }
            return selctedDocList;
        }
        #endregion

        #region 全选 反选
        void lnkCheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in dgvDocumentList.Rows)
            {
                var.Cells["select"].Value = true;
            }
        }
        void lnkInverse_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in dgvDocumentList.Rows)
            {
                var.Cells["select"].Value = false;
            }
        }
        #endregion

        /// <summary>
        /// 错误提示
        /// </summary>
        /// <param name="es"></param>
        /// <returns></returns>
        private string GetExceptionMessage(PLMWebServicesByKB.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServicesByKB.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "；\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if ((msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1) || msg.IndexOf("违反唯一约束") > -1)
            {
                msg = "已存在同名文档，请重命名该文档名称.";
            }

            return msg;
        }


        #region 导出归档资料(注释)
        ///// <summary>
        ///// 导出归档资料
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void btnExportFileData_Click(object sender, EventArgs e)
        //{
        //    downDocList = GetCheckedDocument();
        //    if (downDocList.Count > 0 && downDocList != null)
        //    {
        //        VDocumentDownload vdd = new VDocumentDownload(downDocList);
        //        vdd.ShowDialog();
        //    }
        //    else
        //    {
        //        MessageBox.Show("未选择文档！");
        //    }
        //}
        #endregion

        #region 右键菜单（注释）
        ///// <summary>
        ///// dgvDocumentList_CellMouseDown  显示右键菜单
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void dgvDocumentList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (e.RowIndex >= 0)
        //        {
        //            if (dgvDocumentList.Rows[e.RowIndex].Selected == false)
        //            {
        //                dgvDocumentList.ClearSelection();
        //                dgvDocumentList.Rows[e.RowIndex].Selected = true;
        //            }
        //            //if (dgvDocumentList.SelectedRows.Count == 1)
        //            //{
        //            //    dgvDocumentList.CurrentCell = dgvDocumentList.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //            //}
        //            cmsDocumentList.Show(MousePosition.X, MousePosition.Y);
        //        }
        //    }
        //}
        ///// <summary>
        ///// cmsDocumentList_ItemClicked  点击菜单项事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void cmsDocumentList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    PLMWebServicesByKB.ProjectDocument doc = dgvDocumentList.SelectedRows[0].Tag as PLMWebServicesByKB.ProjectDocument;
        //    if (e.ClickedItem.Text.Trim() == "查看文档详细信息")
        //    {
        //        VDocumentDetail vdd = new VDocumentDetail(doc);
        //        vdd.ShowDialog();
        //    }
        //    if (e.ClickedItem.Text.Trim() == "下载文档")
        //    {
        //        IList onlyDocument = new ArrayList();
        //        onlyDocument.Add(doc);
        //        VDocumentDownload vdd = new VDocumentDownload(onlyDocument);
        //        vdd.ShowDialog();
        //    }
        //    if (e.ClickedItem.Text.Trim() == "上传文档新版本")
        //    {
        //        VDocumentUpload vdu = new VDocumentUpload(doc);
        //        vdu.ShowDialog();
        //        PLMWebServicesByKB.ProjectDocument resultDocument = vdu.ResultDocument;
        //        if (resultDocument != null)
        //        {
        //            int rowIndex = dgvDocumentList.SelectedRows[0].Index;
        //            dgvDocumentList[colDocumentName.Name, rowIndex].Value = resultDocument.Name;
        //            dgvDocumentList[colDocumentExpandedName.Name, rowIndex].Value = resultDocument.ExtendName;
        //            dgvDocumentList[colDocumentInforType.Name, rowIndex].Value = resultDocument.DocType.ToString();
        //            dgvDocumentList[colDocumentCode.Name, rowIndex].Value = resultDocument.Code;
        //            dgvDocumentList[colDocumentVersion.Name, rowIndex].Value = resultDocument.Version;
        //            dgvDocumentList[colCreateTime.Name, rowIndex].Value = resultDocument.CreateTime;
        //            dgvDocumentList[colDutyPerson.Name, rowIndex].Value = resultDocument.OwnerName;
        //            dgvDocumentList[colDocumentStatus.Name, rowIndex].Value = resultDocument.State.ToString();
        //            this.dgvDocumentList.Rows[rowIndex].Tag = resultDocument;
        //        }
        //    }
        //}
        #endregion

        #region 上传新文档（注释）
        ///// <summary>
        ///// 上传新文档
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void btnUpNewDocument_Click(object sender, EventArgs e)
        //{
        //    VDocumentUpload vdu = new VDocumentUpload(null);
        //    vdu.ShowDialog();
        //    //IList resultList = vdu.ResultList;
        //    //foreach (ProObjectRelaDocument doc in resultList)
        //    //{
        //    //    int rowIndex = this.dgvDocumentList.Rows.Add();
        //    //    dgvDocumentList[colDocumentName.Name, rowIndex].Value = doc.DocumentName;
        //    //    //dgOftenWords[userName.Name, rowIndex].Value = ow.UserName;
        //    //    this.dgvDocumentList.Rows[rowIndex].Tag = doc;
        //    //}
        //}
        #endregion

    }
}
