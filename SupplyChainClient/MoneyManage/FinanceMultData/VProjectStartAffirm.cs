using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Iesi.Collections.Generic;
using NHibernate.Criterion;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VProjectStartAffirm : TBasicDataView
    {
        private MFinanceMultData mOperate;
        private MOperationOrg mOperationOrg;
        private MOperationJob mOperationJob;
        private CurrentProjectInfo selectProject;

        public VProjectStartAffirm()
        {
            InitializeComponent();

            InitEvents();

            LoadProjects();
        }

        private void LoadProjects()
        {
            if (mOperate == null)
            {
                mOperate = new MFinanceMultData();
            }

            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectCurrState", 10));
            dgMaster.DataSource = mOperate.CurrentProjectSrv.GetCurrentProjectInfo(objQuery);

            ClearProjectInfo();
        }

        private void LoadStandardJob()
        {
            if (mOperate == null)
            {
                mOperate = new MFinanceMultData();
            }

            lstJobs.DataSource = mOperate.CurrentProjectSrv.GetStandardOperationJob().OfType<OperationJob>().ToList();
            lstJobs.DisplayMember = "Name";
            lstJobs.ValueMember = "Id";
        }

        private bool ValidInput()
        {
            if (selectProject == null)
            {
                MessageBox.Show("请要确认项目");
                return false;
            }

            if (string.IsNullOrEmpty(txtProjectName.Text.Trim()))
            {
                MessageBox.Show("请输入项目名称");
                return false;
            }

            if (string.IsNullOrEmpty(txtProjectMng.Text.Trim()))
            {
                MessageBox.Show("请输入项目经理");
                return false;
            }

            if (string.IsNullOrEmpty(txtProjectProv.Text.Trim()))
            {
                MessageBox.Show("请输入工程地点所在省份");
                return false;
            }

            if (string.IsNullOrEmpty(txtProjectCity.Text.Trim()))
            {
                MessageBox.Show("请输入工程地点所在市");
                return false;
            }

            if (string.IsNullOrEmpty(txtProjectAddress.Text.Trim()))
            {
                MessageBox.Show("请输入工程详细地点");
                return false;
            }

            decimal tmp = 0;
            if (!decimal.TryParse(txtProjectHeight.Text.Trim(), out tmp) || tmp <= 0)
            {
                MessageBox.Show("请输入建筑高度");
                return false;
            }

            if (!decimal.TryParse(txtProjectArea.Text.Trim(), out tmp) || tmp <= 0)
            {
                MessageBox.Show("请输入建筑面积");
                return false;
            }

            if (string.IsNullOrEmpty(txtProjectOrg.Text.Trim()))
            {
                MessageBox.Show("请选择该项目隶属组织");
                return false;
            }

            return MessageBox.Show("您确认项目信息无误并启动该项目？", "项目启动确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                   DialogResult.Yes;
        }

        private void ClearProjectInfo()
        {
            txtProjectAddress.Text = "";
            txtProjectArea.Text = "";
            txtProjectCity.Text = "";
            txtProjectHeight.Text = "";
            txtProjectMng.Text = "";
            txtProjectName.Text = "";
            txtProjectProv.Text = "";
            txtProjectOrg.Text = "";

            selectProject = null;
            txtProjectOrg.Tag = null;
            lstJobs.DataSource = null;
        }

        private PersonInfo GetPersonInfoByName(string pName)
        {
            if (string.IsNullOrEmpty(pName))
            {
                return null;
            }

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Name", pName));

            var list = mOperate.PersonManager.GetPersonInfo(objectQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }
            else
            {
                return list[0] as PersonInfo;
            }
        }

        private List<OperationJob> CopyFromStandardJob()
        {
            if (selectProject == null)
            {
                return null;
            }

            var jobs = lstJobs.DataSource as List<OperationJob>;
            if (jobs == null || jobs.Count == 0)
            {
                return null;
            }

            var jobList = new List<OperationJob>();
            foreach (var jb in jobs)
            {
                var orgJob = new OperationJob();
                orgJob.Author = ConstObject.TheLogin.ThePerson;
                orgJob.AuthorCode = orgJob.Author.Code;
                orgJob.Code = jb.Code;
                orgJob.CreatedDate = DateTime.Now.Date;
                orgJob.Descript = "标准岗位复制";
                orgJob.Name = jb.Name;
                orgJob.OrderNo = jb.OrderNo;
                orgJob.Persons = jb.Persons;
                orgJob.Incumbents = jb.Incumbents;
                orgJob.JobWithRole = new HashedSet<OperationJobWithRole>();

                foreach (var jbRole in jb.JobWithRole)
                {
                    var orgJobRole = new OperationJobWithRole();
                    orgJobRole.OperationJob = orgJob;
                    orgJobRole.OperationJobName = orgJob.Name;
                    orgJobRole.OperationRole = jbRole.OperationRole;
                    orgJobRole.OperationRoleName = jbRole.OperationRoleName;

                    orgJob.JobWithRole.Add(orgJobRole);
                }

                jobList.Add(orgJob);
            }

            return jobList;
        }

        private void InitEvents()
        {
            dgMaster.BorderStyle = BorderStyle.FixedSingle;
            dgMaster.AutoGenerateColumns = false;
            dgMaster.RowPostPaint += dgMaster_RowPostPaint;
            dgMaster.CellClick += dgMaster_CellClick;

            btnProjectOrg.Click += btnProjectOrg_Click;
            btnSaveAndAffirm.Click += btnSaveAndAffirm_Click;
        }

        private void dgMaster_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var dt = dgMaster.Rows[e.RowIndex].DataBoundItem as CurrentProjectInfo;
                if (dt != null)
                {
                    dgMaster.Rows[e.RowIndex].Cells[colProjectAddress.Name].Value =
                        string.Concat(
                            string.IsNullOrEmpty(dt.ProjectLocationProvince) ? "" : dt.ProjectLocationProvince + "省",
                            string.IsNullOrEmpty(dt.ProjectLocationCity) ? "" : dt.ProjectLocationCity + "市",
                            dt.ProjectLocationDescript);
                }
            }
        }

        private void dgMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectProject = dgMaster.Rows[e.RowIndex].DataBoundItem as CurrentProjectInfo;
                if (selectProject != null)
                {
                    txtProjectAddress.Text = selectProject.ProjectLocationDescript;
                    txtProjectArea.Text = selectProject.BuildingArea.ToString("N2");
                    txtProjectCity.Text = selectProject.ProjectLocationCity;
                    txtProjectHeight.Text = selectProject.BuildingHeight.ToString();
                    txtProjectMng.Text = selectProject.HandlePersonName;
                    txtProjectName.Text = selectProject.Name;
                    txtProjectProv.Text = selectProject.ProjectLocationProvince;
                }

                LoadStandardJob();
            }
        }

        private void btnSaveAndAffirm_Click(object sender, EventArgs e)
        {
            if(!ValidInput())
            {
                return;
            }

            if (mOperationOrg == null)
            {
                mOperationOrg = new MOperationOrg();
            }

            var pOrgInfo = txtProjectOrg.Tag as OperationOrgInfo;
            var pOrg = mOperationOrg.GetOperationOrgById(pOrgInfo.Id);
            var newOrg = new OperationOrg();
            newOrg.Code = mOperationOrg.GetNextOPGCode();
            newOrg.Level = pOrgInfo.Level + 1;
            newOrg.Name = selectProject.Name;
            newOrg.IsAccountOrg = true;
            newOrg.AuthorCode = ConstObject.TheLogin.ThePerson.Code;
            newOrg.Describe = string.Format("项目[{0}]确认启动时生成", selectProject.Name);
            newOrg.CreateDate = DateTime.Now.Date;
            newOrg.ParentNode = pOrg;
            newOrg.CategoryNodeType = NodeType.LeafNode;
            newOrg.OperationType = "fgsxmb";

            try
            {
                selectProject.Name = txtProjectName.Text.Trim();
                selectProject.HandlePersonName = txtProjectMng.Text.Trim();
                selectProject.HandlePerson = GetPersonInfoByName(selectProject.HandlePersonName);
                selectProject.ProjectLocationProvince = txtProjectProv.Text.Trim();
                selectProject.ProjectLocationCity = txtProjectCity.Text.Trim();
                selectProject.ProjectLocationDescript = txtProjectAddress.Text.Trim();
                selectProject.BuildingHeight = Convert.ToDecimal(txtProjectHeight.Text.Trim());
                selectProject.BuildingArea = Convert.ToDecimal(txtProjectArea.Text.Trim());
                selectProject.ProjectCurrState = 0;
                selectProject.ProjectInfoState = EnumProjectInfoState.新项目;

                selectProject = mOperate.CurrentProjectSrv.AffirmProject(newOrg, selectProject, CopyFromStandardJob());

                //标准岗位复制及角色
                if (selectProject != null && !string.IsNullOrEmpty(selectProject.Id))
                {
                    MessageBox.Show("项目信息确认成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("项目启动确认失败：" + ex.Message);
            }

            LoadProjects();
        }

        private void btnProjectOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(false);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtProjectOrg.Tag = info;
                txtProjectOrg.Text = info.Name;
            }
        }
    }
}
