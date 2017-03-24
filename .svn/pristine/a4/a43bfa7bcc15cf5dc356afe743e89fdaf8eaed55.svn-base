using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Microsoft.Win32;
using System.Diagnostics;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.IO;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Runtime.InteropServices;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng
{
    public partial class VProjectBusinessInfo : TBasicDataView
    {
        private MProjectDepartment model = new MProjectDepartment();
        private static Hashtable startedProcessHash = new Hashtable();

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        
        public VProjectBusinessInfo()
        {
            InitializeComponent();
            InitEvent();
            InitDate();
        }

        private void InitDate()
        {
            //项目状态
            this.cmbProjectCurState.Items.Clear();
            cmbProjectCurState.Items.Insert(0,"在建");
            cmbProjectCurState.Items.Insert(1,"所有");
            cmbProjectCurState.SelectedIndex = 0;
            //VBasicDataOptr.InitProjectConstractStage(cbProjectStage, false);
            VBasicDataOptr.InitProjectType(cbProjectType, false);
            cbProjectType.Text = "建筑工程";
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo(); //归属项目
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                this.btnOperationOrg.Visible = false;
            }
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnEnterMBP.Click += new EventHandler(btnEnterMBP_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
        }

        public override void ViewShow()
        {
            base.ViewShow();
            QueryProjectInfo();
        }

        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect();
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("Id", info.Id));
                //OperationOrgInfo org = model.ResourceRequirePlanSrv.ObjectQuery(typeof(OperationOrgInfo), oq)[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
            }

        }

        void btnEnterMBP_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgDetail.CurrentRow;
            if (dr == null)
            {
                MessageBox.Show("请选择一个项目。");
            }
            CurrentProjectInfo cpi = dr.Tag as CurrentProjectInfo;
            StartMBP(cpi.Id);
        }

        private void StartMBP(string projectId)
        {
            object oPID=startedProcessHash[projectId];
            int processId = 0;
            if (oPID != null)
            {
                processId = int.Parse(oPID.ToString());
            }
            if (processId > 0)
            {
                //进程已经启动
                try
                {
                    Process oldProcess = Process.GetProcessById(processId);
                    if (oldProcess != null)
                    {
                        SetForegroundWindow(oldProcess.MainWindowHandle);
                        return;
                    }
                }
                catch
                { }
            }
            else
            { 
                //关闭其它已经启动的项目进程
                foreach (int proId in startedProcessHash.Values)
                {
                    try
                    {
                        Process tempProcess = Process.GetProcessById(proId);
                        if (tempProcess != null)
                        {
                            tempProcess.Kill();
                        }
                    }
                    catch
                    {
                    }
                    
                }
            }
            StartNewProcess(projectId);
        }

        private void StartNewProcess(string projectId)
        {
            string userName = ConstObject.TheLogin.ThePerson.Code;
            StandardPerson person = model.CurrentSrv.GetStandardPerson(ConstObject.TheLogin.ThePerson.Id);
            string password = person.Password;
            string groupId = ConstObject.IRPMenuName;
            string roleId = ConstObject.TheLogin.TheSysRole.Id;
            string loginDate = ConstObject.LoginDate.ToShortDateString();

            string installPath="";
            try
            {
                installPath = GetInstallPath();
            }
            catch
            {
                MessageBox.Show("权限不足。");
                return;
            }
            if (string.IsNullOrEmpty(installPath))
            {
                MessageBox.Show("未正确安装项目业务基础模块，请与系统管理员联系。");
                return;
            }

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = installPath;
            info.Arguments = "autologin=true username=\"" + userName + "\" password=\"" + password + "\" groupId=\"" + groupId + "\" roleId=\"" + roleId + "\" loginDate=\"" + loginDate + "\" projectId=\"" + projectId+"\"";
            info.UseShellExecute=true;
            info.WorkingDirectory = Path.GetDirectoryName(info.FileName);
            Process p = new Process();
            p.StartInfo = info;
            p.Start();
            if (!startedProcessHash.Contains(projectId))
            {
                startedProcessHash.Add(projectId, p.Id);
            }
        }

        private string GetInstallPath()
        {
            RegistryKey rkroot = Registry.CurrentUser;
            RegistryKey rkInstallPath = rkroot.OpenSubKey("Software\\CSCEC\\MBP");
            string installPath = rkInstallPath.GetValue("install") + "";
            return installPath;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            QueryProjectInfo();
        }

        private void QueryProjectInfo()
        {
            
            string currProject = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("ProjectCurrState", 0));
            //增加数据权限
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo(); //归属项目
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                oq.AddCriterion(Expression.Eq("Id", projectInfo.Id));
            }
            else{
                string opgSyscode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                string belongOrgId = model.CurrentSrv.GetBelongOperationOrg(opgSyscode);
                if (belongOrgId != "")
                {
                    oq.AddCriterion(Expression.Like("OwnerOrgSysCode", belongOrgId, MatchMode.Anywhere));
                }
            }

            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            if (info != null)
            {
                oq.AddCriterion(Expression.Like("OwnerOrgSysCode", info.SysCode, MatchMode.Anywhere));
            }
            string projectName = txtName.Text;
            if (!string.IsNullOrEmpty(projectName))
            {
                oq.AddCriterion(Expression.Like("Name", projectName, MatchMode.Anywhere));
            }
            string projectType = cbProjectType.Text;
            if (!string.IsNullOrEmpty(projectType))
            {
                oq.AddCriterion(Expression.Eq("ProjectType", (int)VirtualMachine.Component.Util.EnumUtil<EnumProjectType>.FromDescription(projectType)));
            }
            string projectStage = cbProjectStage.Text;
            if (!string.IsNullOrEmpty(projectStage))
            {
                oq.AddCriterion(Expression.Eq("ConstractStage", projectStage));
            }
            if (cmbProjectCurState.SelectedIndex == 0)//在建
            {
                oq.AddCriterion(Expression.Eq("ProjectCurrState",0 ));  
            }
            try
            {
                dgDetail.Rows.Clear();
                oq.AddCriterion(Expression.Eq("OwnerOrg.State", 1));
                IList projectLst = model.CurrentSrv.GetCurrentProjectInfo(oq);
                ShowProject(projectLst);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询项目失败。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void ShowProject(IList projectLst)
        {
            if (projectLst == null || projectLst.Count == 0) return;
            foreach (CurrentProjectInfo obj in projectLst)
            {
                if (obj.Name.Trim() == "")
                    continue;
                int i = dgDetail.Rows.Add();
                DataGridViewRow dr = dgDetail.Rows[i];
                dr.Tag = obj;
                dr.Cells[ColNo.Name].Value = i+1;
                dr.Cells[ColProjectName.Name].Value = obj.Name;//工程名称
                dr.Cells[this.ColProjectCode.Name].Value = obj.Code;//项目编码
                dr.Cells[ColProManage.Name].Value = obj.HandlePersonName;//项目经理
                string type = Enum.GetName(typeof(EnumProjectType), obj.ProjectType);
                dr.Cells[ColProjectType.Name].Value = type;//项目类型
                dr.Cells[ColProStage.Name].Value = obj.ConstractStage;//施工阶段
                dr.Cells[ColProjectCost.Name].Value = obj.ProjectCost;//工程花费
                dr.Cells[ColtheGroundArea.Name].Value = obj.BuildingArea;//建筑面积
                dr.Cells[ColBuildingHeight.Name].Value = obj.BuildingHeight;//建筑高度
                dr.Cells[ColGroundLayers.Name].Value = obj.GroundLayers;//地上层数
                dr.Cells[colUnderLayers.Name].Value = obj.UnderGroundLayers;//地下层数
            }
            dgDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }
    }
}
