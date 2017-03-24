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


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public partial class VInitGWBSDocumentTemplate : TBasicDataView
    {
        List<ProTaskTypeDocumentStencil> listTaskTypeRelaTemplate = new List<ProTaskTypeDocumentStencil>();
        List<string> listTaskTypeIds = new List<string>();

        MWBSManagement model = new MWBSManagement();
        public VInitGWBSDocumentTemplate()
        {
            InitializeComponent();
            InitEvent();
            InitDate();
        }
        void InitDate()
        {
            CurrentProjectInfo project = StaticMethod.GetProjectInfo();
            if (project != null)
            {
                txtName.Text = project.Name;

                IList list = new ArrayList();
                list.Add(project);
                AddProjectInGrid(list, true);
            }

        }
        void InitEvent()
        {

            this.btnSearch.Click += new EventHandler(btnSearch_Click);

            btnInitialWBSDocTemplate.Click += new EventHandler(btnInitialWBSDocTemplate_Click);
        }

        void btnInitialWBSDocTemplate_Click(object sender, EventArgs e)
        {

            List<CurrentProjectInfo> listSelectProject = new List<CurrentProjectInfo>();
            foreach (DataGridViewRow row in gridProject.Rows)
            {
                if (row.Cells[colSelect.Name].Value != null && Convert.ToBoolean(row.Cells[colSelect.Name].Value))
                {
                    CurrentProjectInfo project = row.Tag as CurrentProjectInfo;
                    listSelectProject.Add(project);
                }
            }

            if (listSelectProject.Count == 0)
            {
                MessageBox.Show("请至少选择一个项目！");
                return;
            }

            if (MessageBox.Show("您确认要初始化选中项目的GWBS的文档模板吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            ObjectQuery oq = new ObjectQuery();

            if (listTaskTypeIds.Count == 0)
            {
                //查询有文档模板的任务类型
                listTaskTypeRelaTemplate = model.ObjectQuery(typeof(ProTaskTypeDocumentStencil), oq).OfType<ProTaskTypeDocumentStencil>().ToList();

                foreach (ProTaskTypeDocumentStencil item in listTaskTypeRelaTemplate)
                {
                    if (listTaskTypeIds.Contains(item.ProTaskType.Id) == false)
                    {
                        listTaskTypeIds.Add(item.ProTaskType.Id);
                    }
                }

                //string sqlStr = "select distinct ProTaskType from THD_ProTaskTypeDocumentStencil";

                //DataSet dsTaskType = model.SearchSQL(sqlStr);
                //DataTable dtTaskType = dsTaskType.Tables[0];

                //foreach (DataRow row in dtTaskType.Rows)
                //{
                //    listTaskTypeIds.Add(row["ProTaskType"].ToString());
                //}
            }

            if (listTaskTypeIds.Count == 0)
            {
                MessageBox.Show("项目任务类型尚未配置文档模板信息！");
                return;
            }

            Disjunction dis = null;

            IList listGWBSRelaTemplate = new ArrayList();
            foreach (CurrentProjectInfo project in listSelectProject)
            {
                oq.Criterions.Clear();
                oq.FetchModes.Clear();

                oq.AddCriterion(Expression.Eq("TheProjectGUID", project.Id));
                dis = new Disjunction();
                foreach (string taskTypeId in listTaskTypeIds)
                {
                    dis.Add(Expression.Eq("ProjectTaskTypeGUID.Id", taskTypeId));
                }

                IList listGWBS = model.ObjectQuery(typeof(GWBSTree), oq);

                foreach (GWBSTree wbs in listGWBS)
                {

                    IEnumerable<ProTaskTypeDocumentStencil> listDocStencil = from p in listTaskTypeRelaTemplate
                                                                             where p.ProTaskType.Id == wbs.ProjectTaskTypeGUID.Id
                                                                             select p;


                    foreach (ProTaskTypeDocumentStencil docStencil in listDocStencil)
                    {
                        //生成工程文档验证
                        ProjectDocumentVerify docVerify = new ProjectDocumentVerify();
                        //docVerify.ProjectID = project.Id;
                        docVerify.ProjectName = "知识库";
                        docVerify.ProjectCode = "KB";

                        docVerify.ProjectTask = wbs;
                        docVerify.ProjectTaskName = wbs.Name;

                        if (docStencil.InspectionMark == ProjectTaskTypeCheckFlag.针对项目任务节点)
                        {
                            docVerify.AssociateLevel = ProjectDocumentAssociateLevel.GWBS;
                        }
                        if (docStencil.InspectionMark == ProjectTaskTypeCheckFlag.针对检验批)
                        {
                            docVerify.AssociateLevel = ProjectDocumentAssociateLevel.检验批;
                        }

                        docVerify.DocuemntID = docStencil.ProDocumentMasterID;
                        docVerify.DocumentCode = docStencil.StencilCode;
                        docVerify.DocumentName = docStencil.StencilName;
                        docVerify.DocumentDesc = docStencil.StencilDescription;
                        docVerify.FileSourceURl = null;
                        docVerify.DocumentCategoryCode = docStencil.DocumentCateCode;
                        docVerify.DocumentCategoryName = docStencil.DocumentCateName;
                        docVerify.DocumentWorkflowName = docStencil.ControlWorkflowName;

                        docVerify.AlterMode = docStencil.AlarmMode;

                        docVerify.SubmitState = ProjectDocumentSubmitState.未提交;
                        docVerify.VerifySwitch = ProjectDocumentVerifySwitch.不验证;

                        listGWBSRelaTemplate.Add(docVerify);
                    }
                }
            }

            model.SaveOrUpdate(listGWBSRelaTemplate);

            MessageBox.Show("初始化完成！");
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            if (txtName.Text.Trim() != "")
            {
                objectQuery.AddCriterion(Expression.Like("Name", txtName.Text.Trim(), MatchMode.Anywhere));
            }
            IList listProject = model.ObjectQuery(typeof(CurrentProjectInfo), objectQuery);

            AddProjectInGrid(listProject, false);
        }

        private void AddProjectInGrid(IList listProject, bool isSelect)
        {
            gridProject.Rows.Clear();

            foreach (CurrentProjectInfo project in listProject)
            {
                int index = gridProject.Rows.Add();
                DataGridViewRow row = gridProject.Rows[index];

                if (isSelect)
                    row.Cells[colSelect.Name].Value = true;

                row.Cells[colProjectName.Name].Value = project.Name;

                row.Tag = project;
            }

        }
    }
}
