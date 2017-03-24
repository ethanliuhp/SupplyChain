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
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentsListBak : TBasicDataView
    {
        //IList downDocList = new List<PLMWebServices.ProjectDocument>();
        MGWBSTree model = new MGWBSTree();
        string userName = string.Empty;
        string jobId = string.Empty;
        string isIRPOrKB = string.Empty;

        public VDocumentsListBak()
        {
            InitializeComponent();
            InitEvent();
            InitcomboBoxData();
            IntData();
            LoadProject();
            //LoadCategory();
        }
        private void InitEvent()
        {
            //this.btnSelectProject.Click += new EventHandler(btnSelectProject_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExportFileData.Click += new EventHandler(btnExportFileData_Click);
            this.btnUpNewDocument.Click += new EventHandler(btnUpNewDocument_Click);
            this.dgvDocumentList.CellMouseDown += new DataGridViewCellMouseEventHandler(dgvDocumentList_CellMouseDown);
            this.cmsDocumentList.ItemClicked += new ToolStripItemClickedEventHandler(cmsDocumentList_ItemClicked);
            this.lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            this.lnkInverse.Click += new EventHandler(lnkInverse_Click);
            this.cmbProject.SelectedIndexChanged += new EventHandler(cmbProject_SelectedIndexChanged);
            //this.tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            this.btnEmpty.Click += new EventHandler(btnEmpty_Click);
        }
        //清除查询条件
        void btnEmpty_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要清除不可编辑的查询条件", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                txtHandlePerson.Text = "";
            }
        }

        /// <summary>
        /// 给复选框默认值
        /// </summary>
        void InitcomboBoxData()
        {
            //this.cmbDocumentLibrary.Items.AddRange(new object[] { "知识库" });
            //文档信息类型
            foreach (string infoType in Enum.GetNames(typeof(PLMWebServices.DocumentInfoType)))
            {
                cmbDocumentInforType.Items.Add(infoType);
            }
            //文档状态
            foreach (string infoType in Enum.GetNames(typeof(PLMWebServices.DocumentState)))
            {
                cmbDocumentStatus.Items.Add(infoType);
            }
            //版本
            foreach (string infotype in Enum.GetNames(typeof(PLMWebServices.DocumentQueryVersion)))
            {
                cmbDocumentVersion.Items.Add(infotype);
            }
            //this.cmbDocumentInforType.Items.AddRange(new object[] { "文本", "图片", "音频", "视频", "信息模型", "程序" });
            //this.cmbDocumentVersion.Items.AddRange(new object[] { "最后版本", "所有版本" });
            //this.cmbDocumentStatus.Items.AddRange(new object[] { "编辑", "提交", "发布 ", "作废" });
            this.cmbDocumentVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDocumentVersion.SelectedIndex = 0;
        }
        void IntData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            //ObjectLock.Lock(new Object[] { txtProjectName });
        }

        /// <summary>
        /// 加载文档库
        /// </summary>
        void LoadProject()
        {
            //PersonOnJob p = new PersonOnJob();//new Application.Resource.PersonAndOrganization.HumanResource.Domain.PersonOnJob();
            //p.OperationJob.OperationOrg
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("StandardPerson.Id", Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Id));
            oq.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("OperationJob.OperationOrg", NHibernate.FetchMode.Eager);
            IEnumerable<PersonOnJob> jobList = model.ObjectQuery(typeof(PersonOnJob), oq).OfType<PersonOnJob>();

            var orgId = (from j in jobList
                         select j.OperationJob.OperationOrg.Id).Distinct().ToList();

            oq.Criterions.Clear();
            Disjunction dis = new Disjunction();
            foreach (string operationOrgId in orgId)
            {
                dis.Add(Expression.Eq("OwnerOrg.Id", operationOrgId));
            }
            oq.AddCriterion(dis);
            IList projectList = model.ObjectQuery(typeof(CurrentProjectInfo), oq);

            CurrentProjectInfo extProject = new CurrentProjectInfo();
            extProject.Code = "KB";
            extProject.Name = "知识库";
            projectList.Add(extProject);

            cmbProject.DataSource = projectList;
            cmbProject.DisplayMember = "Name";

            cmbProject.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProject.SelectedIndex = 0;
        }
        /// <summary>
        /// 更改文档库事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadCategory();
        }

        #region 文档分类
        /// <summary>
        /// 加载文档分类
        /// </summary>
        //void LoadCategory()
        //{
        //    if (cmbProject.Text != "知识库")
        //    {
        //        List<PLMWebServices.CategoryNode> list = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.CategoryNode>();
        //        tvwCategory.Nodes.Clear();

        //        PLMWebServices.CategoryNode[] resultList = null;

        //        PLMWebServices.ErrorStack es = StaticMethod.GetDocumentCategoryByIRP(StaticMethod.ProjectDocumentCategoryTypeEnum.CLASSIRPDOCUMENT, Application.Business.Erp.SupplyChain.Client.PLMWebServices.CategoryQueryModeEnum.树方式,
        //            null, null, userName, jobId, null, out resultList);
                
        //        if (es != null)
        //        {
        //            MessageBox.Show("分类树加载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        list.AddRange(resultList);

        //        if (list.Count > 0 && list != null)
        //        {
        //            IEnumerable<PLMWebServices.CategoryNode> listCate = list.OfType<PLMWebServices.CategoryNode>();

        //            var queryCate = from c in listCate
        //                            where c.ParentNode == null
        //                            select c;
        //            if (queryCate != null && queryCate.Count() > 0)
        //            {
        //                for (int i = 0; i < queryCate.Count(); i++)
        //                {
        //                    PLMWebServices.CategoryNode rootNode = queryCate.ElementAt(i);

        //                    TreeNode root = new TreeNode();
        //                    root.Text = rootNode.CategoryName;
        //                    root.Tag = rootNode;

        //                    LoadChildNodeByIRP(rootNode, root, listCate);

        //                    tvwCategory.Nodes.Add(root);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //MessageBox.Show("未找到数据！");
        //        }
        //    }
        //    else
        //    {
        //        List<PLMWebServicesByKB.CategoryNode> list = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.CategoryNode>();
        //        tvwCategory.Nodes.Clear();

        //        PLMWebServicesByKB.CategoryNode[] resultList = null;

        //        PLMWebServicesByKB.ErrorStack es = StaticMethod.GetDocumentCategoryByKB(StaticMethod.ProjectDocumentCategoryTypeEnum.CLASSDOCUMENT, Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.CategoryQueryModeEnum.树方式,
        //            null, null, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId, null, out resultList);

        //        if (es != null)
        //        {
        //            MessageBox.Show("分类树加载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        list.AddRange(resultList);

        //        if (list.Count > 0 && list != null)
        //        {
        //            IEnumerable<PLMWebServicesByKB.CategoryNode> listCate = list.OfType<PLMWebServicesByKB.CategoryNode>();

        //            var queryCate = from c in listCate
        //                            where c.ParentNode == null
        //                            select c;
        //            if (queryCate != null && queryCate.Count() > 0)
        //            {
        //                for (int i = 0; i < queryCate.Count(); i++)
        //                {
        //                    PLMWebServicesByKB.CategoryNode rootNode = queryCate.ElementAt(i);

        //                    TreeNode root = new TreeNode();
        //                    root.Text = rootNode.CategoryName;
        //                    root.Tag = rootNode;

        //                    LoadChildNodeByKB(rootNode, root, listCate);

        //                    tvwCategory.Nodes.Add(root);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //MessageBox.Show("未找到数据！");
        //        }
        //    }
        //}
        //private void LoadChildNodeByIRP(PLMWebServices.CategoryNode parentNode, TreeNode treeParentNode, IEnumerable<PLMWebServices.CategoryNode> listCate)
        //{
        //    var queryCate = from c in listCate
        //                    where c.ParentNode != null && c.ParentNode.EntityID == parentNode.EntityID
        //                    select c;

        //    if (queryCate != null)
        //    {
        //        foreach (PLMWebServices.CategoryNode child in queryCate)
        //        {
        //            TreeNode childNode = new TreeNode();
        //            childNode.Text = child.CategoryName;
        //            childNode.Tag = child;

        //            treeParentNode.Nodes.Add(childNode);

        //            LoadChildNodeByIRP(child, childNode, listCate);
        //        }
        //    }
        //}
        //private void LoadChildNodeByKB(PLMWebServicesByKB.CategoryNode parentNode, TreeNode treeParentNode, IEnumerable<PLMWebServicesByKB.CategoryNode> listCate)
        //{
        //    var queryCate = from c in listCate
        //                    where c.ParentNode != null && c.ParentNode.EntityID == parentNode.EntityID
        //                    select c;

        //    if (queryCate != null)
        //    {
        //        foreach (PLMWebServicesByKB.CategoryNode child in queryCate)
        //        {
        //            TreeNode childNode = new TreeNode();
        //            childNode.Text = child.CategoryName;
        //            childNode.Tag = child;

        //            treeParentNode.Nodes.Add(childNode);

        //            LoadChildNodeByKB(child, childNode, listCate);
        //        }
        //    }
        //}

        //void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    if (cmbProject.Text != "知识库")
        //    {
        //        #region 项目管理查询


        //        //PLMWebServices.ProjectDocument[] list = null;
        //        //List<PLMWebServices.ProjectDocument> documentList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();

        //        ////CurrentProjectInfo projectInfo = txtProjectName.Tag as CurrentProjectInfo;
        //        //CurrentProjectInfo projectInfo = cmbProject.SelectedItem as CurrentProjectInfo;


        //        //PLMWebServices.ErrorStack es = StaticMethod.

        //        //if (es != null)
        //        //{
        //        //    MessageBox.Show("查询失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        //    return;
        //        //}

        //        //dgvDocumentList.Rows.Clear();
        //        //if (list != null)
        //        //{
        //        //    //documentList
        //        //    documentList.AddRange(list);
        //        //    if (documentList.Count > 0 && documentList != null)
        //        //    {
        //        //        foreach (PLMWebServices.ProjectDocument doc in documentList)
        //        //        {
        //        //            //【文档名称】、【文档扩展名】、【文档信息类型】、【文档代码】、【文档版本号】、【创建时间】、【责任人】、【文档状态】。
        //        //            //int rowIndex = this.dgvDocumentList.Rows.Add();
        //        //            //dgvDocumentList[colDocumentName.Name, rowIndex].Value = doc.Name;
        //        //            //dgvDocumentList[colDocumentExpandedName.Name, rowIndex].Value = doc.ExtendName;
        //        //            //dgvDocumentList[colDocumentInforType.Name, rowIndex].Value = doc.DocType.ToString();
        //        //            //dgvDocumentList[colDocumentCode.Name, rowIndex].Value = doc.Code;
        //        //            //dgvDocumentList[colDocumentVersion.Name, rowIndex].Value = doc.Version;
        //        //            //dgvDocumentList[colCreateTime.Name, rowIndex].Value = doc.CreateTime;
        //        //            //dgvDocumentList[colDutyPerson.Name, rowIndex].Value = doc.OwnerName;
        //        //            //dgvDocumentList[colDocumentStatus.Name, rowIndex].Value = doc.State.ToString();
        //        //            //this.dgvDocumentList.Rows[rowIndex].Tag = doc;
        //        //            AddDgvDocumentListByIRP(doc);
        //        //        }
        //        //        isIRPOrKB = "IRP";
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    MessageBox.Show("未找到数据！");
        //        //}
        //        #endregion
        //    }
        //    else
        //    {
        //        #region 知识库查询  PLMWebServicesByKB

        //        //PLMWebServicesByKB.ProjectDocument[] list = null;
        //        //List<PLMWebServicesByKB.ProjectDocument> documentList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();

        //        ////CurrentProjectInfo projectInfo = txtProjectName.Tag as CurrentProjectInfo;
        //        //CurrentProjectInfo projectInfo = cmbProject.SelectedItem as CurrentProjectInfo;

                

               
                

        //        //PLMWebServicesByKB.ErrorStack es = StaticMethod.GetProjectDocumentByKB(null, cmbProject.Text, txtDocumentCode.Text.Trim(),
        //        //    txtDocumentName.Text.Trim(), txtDocumentExpandedName.Text.Trim(), docInfoType, docVersion, null, null, dtpDateBegin.Value, dtpDateEnd.Value.Date.AddDays(1).AddSeconds(-1),
        //        //    txtHandlePerson.Text.Trim() == "" ? null : txtHandlePerson.Text.Trim(), docState, txtKeywordSearch.Text, null,
        //        //    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId, null, out list);

        //        //if (es != null)
        //        //{
        //        //    MessageBox.Show("查询失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        //    return;
        //        //}

        //        //dgvDocumentList.Rows.Clear();
        //        //if (list != null)
        //        //{
        //        //    //documentList
        //        //    documentList.AddRange(list);
        //        //    if (documentList.Count > 0 && documentList != null)
        //        //    {
        //        //        foreach (PLMWebServicesByKB.ProjectDocument doc in documentList)
        //        //        {
        //        //            //【文档名称】、【文档扩展名】、【文档信息类型】、【文档代码】、【文档版本号】、【创建时间】、【责任人】、【文档状态】。
        //        //            //int rowIndex = this.dgvDocumentList.Rows.Add();
        //        //            //dgvDocumentList[colDocumentName.Name, rowIndex].Value = doc.Name;
        //        //            //dgvDocumentList[colDocumentExpandedName.Name, rowIndex].Value = doc.ExtendName;
        //        //            //dgvDocumentList[colDocumentInforType.Name, rowIndex].Value = doc.DocType.ToString();
        //        //            //dgvDocumentList[colDocumentCode.Name, rowIndex].Value = doc.Code;
        //        //            //dgvDocumentList[colDocumentVersion.Name, rowIndex].Value = doc.Version;
        //        //            //dgvDocumentList[colCreateTime.Name, rowIndex].Value = doc.CreateTime;
        //        //            //dgvDocumentList[colDutyPerson.Name, rowIndex].Value = doc.OwnerName;
        //        //            //dgvDocumentList[colDocumentStatus.Name, rowIndex].Value = doc.State.ToString();
        //        //            //this.dgvDocumentList.Rows[rowIndex].Tag = doc;
        //        //            AddDgvDocumentListByKB(doc);
        //        //        }
        //        //        isIRPOrKB = "KB";
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    MessageBox.Show("未找到数据！");
        //        //}

        //        #endregion
        //    }
        //}
        #endregion

        /// <summary>
        /// 选择文档库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void btnSelectProject_Click(object sender, EventArgs e)
        //{
        //    VDepartSelector frm = new VDepartSelector(1);
        //    CurrentProjectInfo extProject = new CurrentProjectInfo();
        //    extProject.Code = "KB";
        //    extProject.Name = "知识库";
        //    frm.ListExtendProject.Add(extProject);
        //    frm.ShowDialog();

        //    if (frm.Result != null && frm.Result.Count > 0)
        //    {
        //        CurrentProjectInfo selectProject = frm.Result[0] as CurrentProjectInfo;
        //        if (selectProject != null)
        //        {
        //            txtProjectName.Text = selectProject.Name;
        //            txtProjectName.Tag = selectProject;
        //        }
        //    }
        //}

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            //if (cmb.Text.Trim() == "")
            //{
            //    MessageBox.Show("请选择一个文档库！");
            //    txtProjectName.Focus();
            //    return;
            //}

            if (txtDocumentExpandedName.Text.Trim() != "" && txtDocumentExpandedName.Text.Trim().IndexOf(".") != 0)
            {
                MessageBox.Show("文档扩展名必须以‘．’打头，例如‘．doc’！");
                txtDocumentExpandedName.Focus();
                return;
            }

            FlashScreen.Show("正在查询加载数据，请稍候......");

            if (cmbProject.Text != "知识库")
            {
                #region 项目管理查询
                PLMWebServices.ProjectDocument[] list = null;
                List<PLMWebServices.ProjectDocument> documentList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();

                //CurrentProjectInfo projectInfo = txtProjectName.Tag as CurrentProjectInfo;
                CurrentProjectInfo projectInfo = cmbProject.SelectedItem as CurrentProjectInfo;

                PLMWebServices.DocumentInfoType? docInfoType = null;

                foreach (PLMWebServices.DocumentInfoType type in Enum.GetValues(typeof(PLMWebServices.DocumentInfoType)))
                {
                    if (type.ToString() == cmbDocumentInforType.Text.Trim())
                    {
                        docInfoType = type;
                        break;
                    }
                }

                PLMWebServices.DocumentState? docState = null;
                foreach (PLMWebServices.DocumentState state in Enum.GetValues(typeof(PLMWebServices.DocumentState)))
                {
                    if (state.ToString() == cmbDocumentStatus.Text.Trim())
                    {
                        docState = state;
                        break;
                    }
                }
                PLMWebServices.DocumentQueryVersion docVersion = 0;
                foreach (PLMWebServices.DocumentQueryVersion qversion in Enum.GetValues(typeof(PLMWebServices.DocumentQueryVersion)))
                {
                    if (qversion.ToString() == cmbDocumentVersion.Text.Trim())
                    {
                        docVersion = qversion;
                        break;
                    }
                }

                PLMWebServices.ErrorStack es = StaticMethod.GetProjectDocumentByIRP(null, cmbProject.Text, txtDocumentCode.Text.Trim(),
                    txtDocumentName.Text.Trim(), txtDocumentExpandedName.Text.Trim(), docInfoType, docVersion, null, null, dtpDateBegin.Value, dtpDateEnd.Value.Date.AddDays(1).AddSeconds(-1),
                    txtHandlePerson.Text.Trim() == "" ? null : txtHandlePerson.Text.Trim(), docState, txtKeywordSearch.Text, null, userName, jobId, null, out list);

                if (es != null)
                {
                    FlashScreen.Close();
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
                        foreach (PLMWebServices.ProjectDocument doc in documentList)
                        {
                            //【文档名称】、【文档扩展名】、【文档信息类型】、【文档代码】、【文档版本号】、【创建时间】、【责任人】、【文档状态】。
                            //int rowIndex = this.dgvDocumentList.Rows.Add();
                            //dgvDocumentList[colDocumentName.Name, rowIndex].Value = doc.Name;
                            //dgvDocumentList[colDocumentExpandedName.Name, rowIndex].Value = doc.ExtendName;
                            //dgvDocumentList[colDocumentInforType.Name, rowIndex].Value = doc.DocType.ToString();
                            //dgvDocumentList[colDocumentCode.Name, rowIndex].Value = doc.Code;
                            //dgvDocumentList[colDocumentVersion.Name, rowIndex].Value = doc.Version;
                            //dgvDocumentList[colCreateTime.Name, rowIndex].Value = doc.CreateTime;
                            //dgvDocumentList[colDutyPerson.Name, rowIndex].Value = doc.OwnerName;
                            //dgvDocumentList[colDocumentStatus.Name, rowIndex].Value = doc.State.ToString();
                            //this.dgvDocumentList.Rows[rowIndex].Tag = doc;
                            AddDgvDocumentListByIRP(doc);
                        }
                        isIRPOrKB = "IRP";
                    }
                }
                else
                {
                    FlashScreen.Close();
                    MessageBox.Show("未找到数据！");
                }
                #endregion
            }
            else
            {
                #region 知识库查询  PLMWebServicesByKB

                PLMWebServicesByKB.ProjectDocument[] list = null;
                List<PLMWebServicesByKB.ProjectDocument> documentList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();

                //CurrentProjectInfo projectInfo = txtProjectName.Tag as CurrentProjectInfo;
                CurrentProjectInfo projectInfo = cmbProject.SelectedItem as CurrentProjectInfo;

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

                PLMWebServicesByKB.ErrorStack es = StaticMethod.GetProjectDocumentByKB(null, cmbProject.Text, txtDocumentCode.Text.Trim(),
                    txtDocumentName.Text.Trim(), txtDocumentExpandedName.Text.Trim(), docInfoType, docVersion, null, null, dtpDateBegin.Value, dtpDateEnd.Value.Date.AddDays(1).AddSeconds(-1),
                    txtHandlePerson.Text.Trim() == "" ? null : txtHandlePerson.Text.Trim(), docState, txtKeywordSearch.Text, null,
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId, null, out list);

                if (es != null)
                {
                    FlashScreen.Close();
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
                            //int rowIndex = this.dgvDocumentList.Rows.Add();
                            //dgvDocumentList[colDocumentName.Name, rowIndex].Value = doc.Name;
                            //dgvDocumentList[colDocumentExpandedName.Name, rowIndex].Value = doc.ExtendName;
                            //dgvDocumentList[colDocumentInforType.Name, rowIndex].Value = doc.DocType.ToString();
                            //dgvDocumentList[colDocumentCode.Name, rowIndex].Value = doc.Code;
                            //dgvDocumentList[colDocumentVersion.Name, rowIndex].Value = doc.Version;
                            //dgvDocumentList[colCreateTime.Name, rowIndex].Value = doc.CreateTime;
                            //dgvDocumentList[colDutyPerson.Name, rowIndex].Value = doc.OwnerName;
                            //dgvDocumentList[colDocumentStatus.Name, rowIndex].Value = doc.State.ToString();
                            //this.dgvDocumentList.Rows[rowIndex].Tag = doc;
                            AddDgvDocumentListByKB(doc);
                        }
                        isIRPOrKB = "KB";
                    }
                }
                else
                {
                    FlashScreen.Close();
                    MessageBox.Show("未找到数据！");
                }

                #endregion
            }
            FlashScreen.Close();
        }
        void AddDgvDocumentListByIRP(PLMWebServices.ProjectDocument doc)
        {
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
        void AddDgvDocumentListByKB(PLMWebServicesByKB.ProjectDocument doc)
        {
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

        #region 得到选中文档的集合
        /// <summary>
        /// 得到选中文档的集合
        /// </summary>
        /// <returns></returns>
        IList GetCheckedDocument()
        {
            IList selctedDocList = new ArrayList();
            if (isIRPOrKB != "KB")
            {
                foreach (DataGridViewRow row in this.dgvDocumentList.Rows)
                {
                    if ((bool)row.Cells["select"].EditedFormattedValue)
                    {
                        PLMWebServices.ProjectDocument doc = row.Tag as PLMWebServices.ProjectDocument;
                        selctedDocList.Add(doc);
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in this.dgvDocumentList.Rows)
                {
                    if ((bool)row.Cells["select"].EditedFormattedValue)
                    {
                        PLMWebServicesByKB.ProjectDocument doc = row.Tag as PLMWebServicesByKB.ProjectDocument;
                        selctedDocList.Add(doc);
                    }
                }
            }
            return selctedDocList;
        }
        #endregion

        /// <summary>
        /// 导出归档资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExportFileData_Click(object sender, EventArgs e)
        {
            IList downDocList = GetCheckedDocument();
            if (downDocList.Count > 0 && downDocList != null)
            {
                VDocumentDownload vdd = new VDocumentDownload(downDocList, isIRPOrKB);
                vdd.ShowDialog();
            }
            else
            {
                MessageBox.Show("未选择文档！");
            }
        }

        #region 右键菜单
        /// <summary>
        /// dgvDocumentList_CellMouseDown  显示右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgvDocumentList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    if (dgvDocumentList.Rows[e.RowIndex].Selected == false)
                    {
                        dgvDocumentList.ClearSelection();
                        dgvDocumentList.Rows[e.RowIndex].Selected = true;
                    }
                    //if (dgvDocumentList.SelectedRows.Count == 1)
                    //{
                    //    dgvDocumentList.CurrentCell = dgvDocumentList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    //}
                    cmsDocumentList.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }
        /// <summary>
        /// cmsDocumentList_ItemClicked  点击菜单项事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmsDocumentList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            IList pdList = new ArrayList();
            if (isIRPOrKB != "KB")
            {
                PLMWebServices.ProjectDocument doc = dgvDocumentList.SelectedRows[0].Tag as PLMWebServices.ProjectDocument;
                pdList.Add(doc);
            }
            else
            {
                PLMWebServicesByKB.ProjectDocument doc = dgvDocumentList.SelectedRows[0].Tag as PLMWebServicesByKB.ProjectDocument;
                pdList.Add(doc);
            }

            if (e.ClickedItem.Text.Trim() == "查看文档详细信息")
            {
                VDocumentDetail vdd = new VDocumentDetail(pdList, isIRPOrKB);
                vdd.ShowDialog();
            }
            if (e.ClickedItem.Text.Trim() == "下载文档")
            {
                //IList onlyDocument = new ArrayList();
                //onlyDocument.Add(doc);
                VDocumentDownload vdd = new VDocumentDownload(pdList, isIRPOrKB);
                vdd.ShowDialog();
            }
            if (e.ClickedItem.Text.Trim() == "上传文档新版本")
            {
                VDocumentUpload vdu = new VDocumentUpload(pdList, isIRPOrKB);
                vdu.ShowDialog();
                //PLMWebServices.ProjectDocument resultDocument = vdu.ResultDocument;
                IList resultDocList = vdu.ResultListDoc;

                if (resultDocList != null && resultDocList.Count > 0)
                {
                    if (isIRPOrKB != "KB")
                    {
                        PLMWebServices.ProjectDocument resultDocument = resultDocList[0] as PLMWebServices.ProjectDocument;
                        int rowIndex = dgvDocumentList.SelectedRows[0].Index;
                        dgvDocumentList[colDocumentName.Name, rowIndex].Value = resultDocument.Name;
                        dgvDocumentList[colDocumentExpandedName.Name, rowIndex].Value = resultDocument.ExtendName;
                        dgvDocumentList[colDocumentInforType.Name, rowIndex].Value = resultDocument.DocType.ToString();
                        dgvDocumentList[colDocumentCode.Name, rowIndex].Value = resultDocument.Code;
                        dgvDocumentList[colDocumentVersion.Name, rowIndex].Value = resultDocument.Version;
                        dgvDocumentList[colCreateTime.Name, rowIndex].Value = resultDocument.CreateTime;
                        dgvDocumentList[colDutyPerson.Name, rowIndex].Value = resultDocument.OwnerName;
                        dgvDocumentList[colDocumentStatus.Name, rowIndex].Value = resultDocument.State.ToString();
                        this.dgvDocumentList.Rows[rowIndex].Tag = resultDocument;
                    }
                    else
                    {
                        PLMWebServicesByKB.ProjectDocument resultDocument = resultDocList[0] as PLMWebServicesByKB.ProjectDocument;
                        int rowIndex = dgvDocumentList.SelectedRows[0].Index;
                        dgvDocumentList[colDocumentName.Name, rowIndex].Value = resultDocument.Name;
                        dgvDocumentList[colDocumentExpandedName.Name, rowIndex].Value = resultDocument.ExtendName;
                        dgvDocumentList[colDocumentInforType.Name, rowIndex].Value = resultDocument.DocType.ToString();
                        dgvDocumentList[colDocumentCode.Name, rowIndex].Value = resultDocument.Code;
                        dgvDocumentList[colDocumentVersion.Name, rowIndex].Value = resultDocument.Version;
                        dgvDocumentList[colCreateTime.Name, rowIndex].Value = resultDocument.CreateTime;
                        dgvDocumentList[colDutyPerson.Name, rowIndex].Value = resultDocument.OwnerName;
                        dgvDocumentList[colDocumentStatus.Name, rowIndex].Value = resultDocument.State.ToString();
                        this.dgvDocumentList.Rows[rowIndex].Tag = resultDocument;
                    }
                }
            }
        }
        #endregion

        #region 上传新文档
        /// <summary>
        /// 上传新文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUpNewDocument_Click(object sender, EventArgs e)
        {
            if (cmbProject.Text != "")
            {
                CurrentProjectInfo cpi = cmbProject.SelectedItem as CurrentProjectInfo;//txtProjectName.Tag as CurrentProjectInfo;
                VDocumentUpload vdu = new VDocumentUpload(cpi, null, null);
                vdu.ShowDialog();
            }
            else
            {
                MessageBox.Show("请选择一个文档库！");
            }
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

        private string GetExceptionMessage(PLMWebServices.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServices.ErrorStack esTemp = es.InnerErrorStack;
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

    }
}
