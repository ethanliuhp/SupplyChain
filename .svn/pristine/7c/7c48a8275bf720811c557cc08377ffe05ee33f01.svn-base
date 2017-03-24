using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource;
using System.Collections;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Core;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;

namespace Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng
{
    public partial class VProjectDepart : TBasicDataView
    {
        CurrentProjectInfo projectInfo = null;
        private TreeNode currNode;
        private MOperationJob uim;
        private MPersonOnJob modelpoj;
        private IList cacheRoleList = new ArrayList();
        
        //private CurrentProjectInfo oprNode = null;
        public MProjectDepartment model;
        private IList lstInstance;

        private CurrentProjectInfo curBillManage;

        /// <summary>
        /// 项目部基本信息
        /// </summary>
        public CurrentProjectInfo CurBillManage
        {
            get { return curBillManage; }
            set { curBillManage = value; }
        }
        string strType = "";
        public VProjectDepart(MOperationJob mod, MPersonOnJob poj, MProjectDepartment pdt,string strName)
        {
            uim = mod;
            modelpoj = poj;
            model = pdt;
            strType = strName;
            InitializeComponent();
            InitEvents();
            InitForm();
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            GetOperationJob(projectInfo);
            LoadManager();
            RefreshControls("btnSave");
            if (strType.Equals("项目基本信息维护"))
            {
                this.btnSave.Visible = true;
                this.btnUpdate.Visible = true;
            }
            if (strType.Equals("项目基本信息查询"))
            {
                this.btnUpdate.Visible = false;
                this.btnSave.Visible = false;
            }
        }
        private void InitEvents()
        {
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.dgManage1.SelectionChanged +=new EventHandler(dgManage1_SelectionChanged);
        }

        private void InitForm()
        {
            //承包方式
            //txtContractWay.DataSource = (Enum.GetNames(typeof(EnumContractType)));
            VBasicDataOptr.InitContractWay(txtContractWay, false);
            //项目生命周期  
            txtLifeCycleState.DataSource = (Enum.GetNames(typeof(EnumProjectLifeCycle)));
            //项目类型   
            //txtProjectType.DataSource = (Enum.GetNames(typeof(EnumProjectType)));
            VBasicDataOptr.InitProjectType(txtProjectType, false);
            //状态   
            txtState.DataSource = (Enum.GetNames(typeof(EnumProjectInfoState)));
            //资金来源  
            txtMoneySource.DataSource = (Enum.GetNames(typeof(EnumSourcesOfFunding)));
            //资金到位状况 （int 0未到位 1到位）
            txtMoneyStates.Items.AddRange(new object[] { "到位", "未到位"});
            //施工阶段
            //txtConstractStage.Items.AddRange(new object[] { "施工准备", "基础施工", "主体结构","装饰安装施工","收尾阶段" });
            VBasicDataOptr.InitProjectConstractStage(txtConstractStage, false);

            VBasicDataOptr.InitBasicFrom(txtBace, false);
            VBasicDataOptr.InitStructFrom(txtStructType, false);
        }

        void btnUpdate_Click(object sender,EventArgs e)
        {
            //所有控件都为可编辑
            RefreshControls("btnUpdate");
        }

        void LoadManager()
        {
            txtGoundArea.Text = ClientUtil.ToString(projectInfo.TheGroundArea);
            txtUnderArea.Text = ClientUtil.ToString(projectInfo.UnderGroundArea);
            txtCollectProport.Text = ClientUtil.ToString(projectInfo.ContractCollectRatio * 100);
            txtBace.Text = projectInfo.BaseForm;
            txtConstractMoney.Text = ClientUtil.ToString(projectInfo.CivilContractMoney/10000);
            txtExterWallArea.Text = ClientUtil.ToString(projectInfo.WallProjectArea);
            txtTurnProport.Text = ClientUtil.ToString(projectInfo.ResProportion * 100);
            txtGroundPrice.Text = ClientUtil.ToString(projectInfo.BigModualGroundUpPrice);
            txtUnderPrice.Text = ClientUtil.ToString(projectInfo.BigModualGroundDownPrice);
            DateTime dtTime = Convert.ToDateTime("1900-1-1 00:00:00");
            if (DateTime.Compare(projectInfo.BeginDate,dtTime) > 0)
            {
                txtStartDate.Text = projectInfo.BeginDate.ToShortDateString();
            }
            if (DateTime.Compare(projectInfo.EndDate, dtTime) > 0)
            {
                txtCompleteDate.Text = projectInfo.EndDate.ToShortDateString();
            }
            if (projectInfo.RealKGDate > dtTime)
            {
                txtrealKGDate.Value = projectInfo.RealKGDate;
            }
            txtName.Text = projectInfo.OwnerOrgName;
            txtMoneySource.SelectedItem = EnumUtil<EnumSourcesOfFunding>.GetDescription(projectInfo.SourcesOfFunding);
            txtMoneyStates.SelectedIndex = projectInfo.IsFundsAvailabed;//资金到位情况
            //ProjectInfoState
            txtState.SelectedItem = EnumUtil<EnumProjectInfoState>.GetDescription(projectInfo.ProjectInfoState);
            txtDescript.Text = projectInfo.Descript;
            //txtProjectType.SelectedItem = EnumUtil<EnumProjectType>.GetDescription(projectInfo.ProjectType);
            txtProjectType.SelectedIndex = projectInfo.ProjectType;//项目类型
            txtExplain.Text = projectInfo.ProjectTypeDescript;
            txtLifeCycleState.SelectedItem = EnumUtil<EnumProjectLifeCycle>.GetDescription(projectInfo.ProjectLifeCycle);
            if (projectInfo.HandlePerson != null)
            {
                txtHandelPerson.Result.Clear();
                txtHandelPerson.Result.Add(projectInfo.HandlePerson);
                txtHandelPerson.Value = projectInfo.HandlePersonName;
                txtHandelPerson.Enabled = false;
            }
            //txtStructType.SelectedItem = EnumUtil<EnumStructureType>.GetDescription(projectInfo.StructureType);
            txtStructType.SelectedIndex = projectInfo.StructureType;
            txtStructExcplain.Text = projectInfo.StructureTypeDescript;
            txtProjectArea.Text = ClientUtil.ToString(projectInfo.BuildingArea);
            txtProjectHeight.Text = ClientUtil.ToString(projectInfo.BuildingHeight);
            txtSubcontractProject.Text = projectInfo.SubProjectDescript;
            txtProjectCost.Text = ClientUtil.ToString(projectInfo.ProjectCost/10000);
            txtAddressExplain.Text = projectInfo.ProjectLocationDescript;
            txtCity.Text = projectInfo.ProjectLocationCity;
            txtProvince.Text = projectInfo.ProjectLocationProvince;
            txtUnderNum.Text = ClientUtil.ToString(projectInfo.UnderGroundLayers);
            txtGroundNum.Text = ClientUtil.ToString(projectInfo.GroundLayers);
            if (DateTime.Compare(projectInfo.CreateDate, dtTime) > 0)
            {
                txtCreateDate.Text = projectInfo.CreateDate.ToShortDateString();
            }
            //txtContractWay.SelectedItem = EnumUtil<EnumContractType>.GetDescription(projectInfo.ContractType);
            txtContractWay.SelectedIndex = projectInfo.ContractType;//承包方式
            txtContractRange.Text = projectInfo.ContractArea;
            txtLength.Text = ClientUtil.ToString(projectInfo.Length);
            //txtMeasure.Text = ClientUtil.ToString(projectInfo.MeasureAccuracy);//测量精度
            txtCode.Text = projectInfo.Code;
            txtName.Text = projectInfo.Name;
            txtConstractStage.Text = projectInfo.ConstractStage;
            txtManagerDepart.Text = ClientUtil.ToString(projectInfo.ManagerDepart);
        }

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public void RefreshControls(string btn)
        {
            //控制自身控件
            if (btn.Equals("btnUpdate"))
            {
                ObjectLock.Unlock(pnlFloor, true);
                txtContractWay.Enabled = true;
                txtStructType.Enabled = true;
                txtLifeCycleState.Enabled = true;
                txtProjectType.Enabled = true;
                txtState.Enabled = true;
                txtMoneySource.Enabled = true;
                txtMoneyStates.Enabled = true;
                txtConstractStage.Enabled = true;
                txtBace.Enabled = true;
                txtHandelPerson.Enabled = true;
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                txtContractWay.Enabled = false;
                txtStructType.Enabled = false;
                txtLifeCycleState.Enabled = false;
                txtProjectType.Enabled = false;
                txtState.Enabled = false;
                txtMoneySource.Enabled = false;
                txtMoneyStates.Enabled = false;
                txtConstractStage.Enabled = false;
                txtBace.Enabled = false;
                txtHandelPerson.Enabled = false;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            //将控件的信息保存
            projectInfo.TheGroundArea = ClientUtil.ToDecimal(txtGoundArea.Text);
            projectInfo.UnderGroundArea = ClientUtil.ToDecimal(txtUnderArea.Text);
            projectInfo.ContractCollectRatio = ClientUtil.ToDecimal(txtCollectProport.Text)/100;
            projectInfo.BaseForm = txtBace.Text;
            projectInfo.CivilContractMoney = ClientUtil.ToDecimal(txtConstractMoney.Text) * 10000;
            projectInfo.WallProjectArea = ClientUtil.ToDecimal(txtExterWallArea.Text);
            projectInfo.ResProportion = ClientUtil.ToDecimal(txtTurnProport.Text)/100;
            projectInfo.BigModualGroundUpPrice = ClientUtil.ToDecimal(txtGroundPrice.Text);
            projectInfo.BigModualGroundDownPrice = ClientUtil.ToDecimal(txtUnderPrice.Text);
            projectInfo.BeginDate = Convert.ToDateTime(txtStartDate.Text);
            projectInfo.EndDate = Convert.ToDateTime(txtCompleteDate.Text);
            projectInfo.OwnerOrgName = txtName.Text;
            projectInfo.SourcesOfFunding = EnumUtil<EnumSourcesOfFunding>.FromDescription(txtMoneySource.SelectedItem);
            projectInfo.IsFundsAvailabed = txtMoneyStates.SelectedIndex;//资金到位情况
            projectInfo.ProjectInfoState = EnumUtil<EnumProjectInfoState>.FromDescription(txtState.SelectedItem);
            projectInfo.Descript = txtDescript.Text;
            //projectInfo.ProjectType = EnumUtil<EnumProjectType>.FromDescription(txtProjectType.SelectedItem);
            projectInfo.ProjectType = ClientUtil.ToInt(txtProjectType.SelectedIndex);
            projectInfo.ProjectTypeDescript = txtExplain.Text;
            projectInfo.ProjectLifeCycle = EnumUtil<EnumProjectLifeCycle>.FromDescription(txtLifeCycleState.SelectedItem);
            projectInfo.RealKGDate = Convert.ToDateTime(txtrealKGDate.Value.Date);
            string strUnit = "平方米";
            Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Name", strUnit));
            IList lists = model.CurrentSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
            if (lists != null && lists.Count > 0)
            {
                Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                projectInfo.DefaultAreaUnit = Unit;
                projectInfo.DefaultAreaUnitName = Unit.Name;
            }
            string strUnit1 = "元";
            Application.Resource.MaterialResource.Domain.StandardUnit Unit1 = null;
            ObjectQuery oq1 = new ObjectQuery();
            oq1.AddCriterion(Expression.Eq("Name", strUnit1));
            IList lists1 = model.CurrentSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq1);
            if (lists1 != null && lists1.Count > 0)
            {
                Unit1 = lists1[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                projectInfo.DefaultPriceUnit = Unit1;
                projectInfo.DefaultPriceUnitName = Unit1.Name;
            }

            string strUnit2 = "吨";
            Application.Resource.MaterialResource.Domain.StandardUnit Unit2 = null;
            ObjectQuery oq2 = new ObjectQuery();
            oq2.AddCriterion(Expression.Eq("Name", strUnit2));
            IList lists2 = model.CurrentSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq2);
            if (lists2 != null && lists2.Count > 0)
            {
                Unit2 = lists2[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                projectInfo.DefaultWeightUnit = Unit2;
                projectInfo.DefaultWeightUnitName = Unit2.Name;
            }

            string strUnit11 = "立方米";
            Application.Resource.MaterialResource.Domain.StandardUnit Unit11 = null;
            ObjectQuery oq11 = new ObjectQuery();
            oq11.AddCriterion(Expression.Eq("Name", strUnit11));
            IList lists11 = model.CurrentSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq11);
            if (lists11 != null && lists11.Count > 0)
            {
                Unit11 = lists11[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                projectInfo.DefaultVolumeUnit = Unit11;
                projectInfo.DefaultVolumeUnitName = Unit11.Name;
            }

            string strUnit12 = "米";
            Application.Resource.MaterialResource.Domain.StandardUnit Unit12 = null;
            ObjectQuery oq12 = new ObjectQuery();
            oq12.AddCriterion(Expression.Eq("Name", strUnit12));
            IList lists12 = model.CurrentSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq12);
            if (lists12 != null && lists12.Count > 0)
            {
                Unit12 = lists12[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                projectInfo.DefaultLengthUnit = Unit12;
                projectInfo.DefaultLengthUnitName = Unit12.Name;
            }
            projectInfo.HandlePersonName = txtHandelPerson.Text;
            projectInfo.HandlePerson = txtHandelPerson.Result[0] as PersonInfo;
            projectInfo.StructureType = txtStructType.SelectedIndex;
            //projectInfo.StructureType = EnumUtil<EnumStructureType>.FromDescription(txtStructType.SelectedItem);
            projectInfo.StructureTypeDescript = txtStructExcplain.Text;
            projectInfo.BuildingArea = ClientUtil.ToDecimal(txtProjectArea.Text);
            projectInfo.BuildingHeight = ClientUtil.ToDecimal(txtProjectHeight.Text);
            projectInfo.SubProjectDescript = txtSubcontractProject.Text;
            projectInfo.ProjectCost = ClientUtil.ToDecimal(txtProjectCost.Text) * 10000;
            projectInfo.ProjectLocationDescript = txtAddressExplain.Text;
            projectInfo.ProjectLocationCity = txtCity.Text;
            projectInfo.ManagerDepart = txtManagerDepart.Text;
            projectInfo.ProjectLocationProvince = txtProvince.Text;
            projectInfo.UnderGroundLayers = ClientUtil.ToDecimal(txtUnderNum.Text);
            projectInfo.GroundLayers = ClientUtil.ToDecimal(txtGroundNum.Text);
            projectInfo.CreateDate = Convert.ToDateTime(txtCreateDate.Text);
            //projectInfo.ContractType = EnumUtil<EnumContractType>.FromDescription(txtContractWay.SelectedItem);
            projectInfo.ContractType = ClientUtil.ToInt(txtContractWay.SelectedIndex);
            projectInfo.ContractArea = txtContractRange.Text;
            projectInfo.Length = ClientUtil.ToDecimal(txtLength.Text);
            //projectInfo.MeasureAccuracy = ClientUtil.ToDecimal(txtMeasure.Text);
            projectInfo.Code = txtCode.Text;
            projectInfo.Name = txtName.Text;
            projectInfo.ConstractStage = txtConstractStage.Text;
            projectInfo = model.CurrentSrv.SaveCurrentProjectInfo(projectInfo);
            MessageBox.Show("保存成功！");
            RefreshControls("btnSave");
        }
    
        private void GetOperationJob(CurrentProjectInfo tn)
        {
            try
            {
                OperationOrg currNode = null;
                currNode = tn.OwnerOrg;
                if (currNode == null) return;
                IList lst = uim.GetOperationJob(currNode);
                BindPostList(lst);
                if (lst.Count > 0)
                {
                    OperationJob job = this.dgManage1.Rows[0].Tag as OperationJob;
                    this.LoadPersonList(job);
                    tvwCategory_SelectionChanged(dgManage2, new EventArgs());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("装载岗位列表出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void BindPostList(IList Post)
        {
            foreach (object arr in Post)
            {
                OperationJob m = arr as OperationJob;
                int i = dgManage1.Rows.Add();
                try
                {
                    AddNewPost(m, i);
                }
                catch (Exception e)
                {
                    MessageBox.Show("装载岗位出错：" + e.Message);
                }
            }
        }

        private void AddNewPost(OperationJob m, int i)
        {
            DataGridViewRow r = dgManage1.Rows[i];
            r.Tag = m;
            r.Cells[colPostCode.Name].Value = m.Code.ToString();
            r.Cells[colPostName.Name].Value = m.Name.ToString();
            r.Cells[colCreateDate.Name].Value = m.CreatedDate.ToShortDateString();
            //r.Cells[colRealPersonNum.Name].Value = m.Persons.ToString();
            //r.Cells[colPostPersonNum.Name].Value = m.Incumbents.ToString();
            r.Cells[colNo.Name].Value = m.OrderNo.ToString();

        }

        private void LoadPersonList(OperationJob job)
        {
            try
            {
                if (job != null)
                {
                    IList lst = modelpoj.GetOnJobPersonList(job.Id);
                    if (lst == null) return;
                    BindEmpList(lst);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("加载人员列表出错：" + ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private void BindEmpList(IList OnJob)
        {
            this.dgManage3.Rows.Clear();
            foreach (PersonOnJob arr in OnJob)
            {
                int i = this.dgManage3.Rows.Add();
                try
                {
                    AddNewPerson(arr, i);
                }
                catch (Exception e)
                {
                    MessageBox.Show("装载人员列表出错：" + e.Message);
                    break;
                }
            }
        }

        private void AddNewPerson(PersonOnJob m, int i)
        {
            DataGridViewRow r = this.dgManage3.Rows[i];
            r.Tag = m;
            r.Cells[colPersonNo.Name].Value = m.StandardPerson.Code;
            r.Cells[colPersonName.Name].Value = m.StandardPerson.Name;
            //r.Cells[colPersonSex.Name].Value = (m.StandardPerson.Sex == 0) ? "男" : "女";
            r.Cells[colBirthDate.Name].Value = m.StandardPerson.Birthday.ToShortDateString();
            r.Cells[colStartDate.Name].Value = m.BeginDate.ToShortDateString();
            r.Cells[colEndDate.Name].Value = m.EndDate.ToShortDateString();
        }
        private void LoadOperationRole()
        {
            ObjectQuery oq = new ObjectQuery();
            try
            {
                cacheRoleList = uim.OpeJobSrv.GetOperationRole(oq);
                ShowRole(cacheRoleList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询角色出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        private void ShowRole(IList roleList)
        {
            if (roleList == null || roleList.Count == 0) return;
            
            foreach (OperationRole obj in roleList)
            {
                if (obj != null)
                {
                    int rowIndex = dgManage2.Rows.Add();
                    DataGridViewRow dr = dgManage2.Rows[rowIndex];
                    dr.Tag = obj;
                    dr.Cells[colRoleName.Name].Value = obj.RoleName;
                    dr.Cells[colRoleDescript.Name].Value = obj.Descript;
                }
            }
        }

        void tvwCategory_SelectionChanged(object sender, EventArgs e)
        {
            this.dgManage2.Rows.Clear();
            DataGridViewRow dr = dgManage1.CurrentRow;
            if (dr == null) return;
            Application.Resource.PersonAndOrganization.OrganizationResource.Domain.OperationJob job = dr.Tag as Application.Resource.PersonAndOrganization.OrganizationResource.Domain.OperationJob;
            if (job == null) return;
            foreach (OperationJobWithRole obj in job.JobWithRole)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", obj.OperationRole.Id));
                cacheRoleList = uim.OpeJobSrv.GetOperationRole(oq);
                ShowRole(cacheRoleList);
            }
        }

        void dgManage1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgManage1.CurrentRow;
            if (dr == null) return;
            dgManage2.Rows.Clear();
            dgManage3.Rows.Clear();
            Application.Resource.PersonAndOrganization.OrganizationResource.Domain.OperationJob job = dr.Tag as Application.Resource.PersonAndOrganization.OrganizationResource.Domain.OperationJob;
            if (job == null) return;
            foreach (OperationJobWithRole obj in job.JobWithRole)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", obj.OperationRole.Id));
                cacheRoleList = uim.OpeJobSrv.GetOperationRole(oq);
                ShowRole(cacheRoleList);
                this.LoadPersonList(job);
            }
        }
    }
}
