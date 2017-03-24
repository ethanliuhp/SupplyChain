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
using System.Runtime.InteropServices;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Microsoft.Win32;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng
{

    public partial class VProjectMaterialStateMng : TBasicDataView
    {
        IList lstProjectState = null;
        private MProjectDepartment model = new MProjectDepartment();
        private static Hashtable startedProcessHash = new Hashtable();
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        CProjectDepartment_ExecType execType;
        public VProjectMaterialStateMng(CProjectDepartment_ExecType execType)
        {
            InitializeComponent();
            this.execType = execType;
            InitEvent();
            InitDate();
        }

        private void InitDate()
        {
            VBasicDataOptr.InitProjectConstractStage(cbProjectStage, false);
            VBasicDataOptr.InitProjectType(cbProjectType, false);
            cbProjectType.Text = "建筑工程";
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo(); //归属项目
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                this.btnOperationOrg.Visible = false;
            }

            lstProjectState = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTSTATE);
            cmbType.Items.Clear();
            DataGridViewComboBoxColumn oCmbColumn = null;
            // System.Drawing.SystemColors.Control;
            if (execType == CProjectDepartment_ExecType.ProjectStateMng)
            {
                oCmbColumn = new DataGridViewComboBoxColumn()
                {
                    Name = "colProjectinfoState",
                    HeaderText = "工程状态",
                    Width = 100,
                    ReadOnly = false
                };

                lblName.Text = "工程状态";
                foreach (BasicDataOptr oBasicDataOptr in lstProjectState)
                {
                    cmbType.Items.Add(oBasicDataOptr.BasicName);
                    oCmbColumn.Items.Add(oBasicDataOptr.BasicName);
                }
                cmbType.Items.Insert(0, "");
            }
            else if (execType == CProjectDepartment_ExecType.ProjectBusinessStateMng)
            {
                oCmbColumn = new DataGridViewComboBoxColumn()
                {
                    Name = "colProjectCurrState",
                    HeaderText = "商务状态",
                    Width = 100,
                    ReadOnly = false
                };
                lblName.Text = "商务状态";
                foreach (string sName in Enum.GetNames(typeof(EnumProjectCurrState)))
                {
                    cmbType.Items.Add(sName);
                    oCmbColumn.Items.Add(sName);
                }
                cmbType.Items.Insert(0, "");
            }
            if (oCmbColumn != null)
            {
                dgDetail.Columns.Insert(ColProjectType.Index + 1, oCmbColumn);
                oCmbColumn.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                oCmbColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                oCmbColumn.FlatStyle = FlatStyle.Popup;
            }
            QueryProjectInfo();
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
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
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
            }

        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            QueryProjectInfo();
        }

        private void QueryProjectInfo()
        {

            string currProject = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            StringBuilder oBuilder = new StringBuilder();
            if (cmbType.SelectedIndex > 0)
            {
                if (execType == CProjectDepartment_ExecType.ProjectBusinessStateMng)
                {
                    int iProjectCurrState = (int)Enum.Parse(typeof(EnumProjectCurrState), ClientUtil.ToString(cmbType.SelectedItem.ToString()));
                    oBuilder.AppendFormat(" and t.projectcurrstate={0} ", iProjectCurrState);
                }
                else if (execType == CProjectDepartment_ExecType.ProjectStateMng)
                {
                    int iProjectState = this.GetProjectStateValue(ClientUtil.ToString(cmbType.SelectedItem.ToString()));
                    oBuilder.AppendFormat(" and projectinfostate={0} ", iProjectState);
                }
            }
            //增加数据权限
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo(); //归属项目
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                oBuilder.AppendFormat(" and t.id='{0}' ", projectInfo.Id);
            }
            else
            {
                string opgSyscode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                string belongOrgId = model.CurrentSrv.GetBelongOperationOrg(opgSyscode);
                if (belongOrgId != "")
                {
                    oBuilder.AppendFormat(" and t.ownerorgsyscode like '%{0}%' ", belongOrgId);
                }
            }

            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            if (info != null)
            {
                oBuilder.AppendFormat(" and t.ownerorgsyscode like '%{0}%' ", info.SysCode);
            }
            string projectName = txtName.Text;
            if (!string.IsNullOrEmpty(projectName))
            {
                oBuilder.AppendFormat(" and t.projectname like '%{0}%' ", projectName);
            }
            string projectType = cbProjectType.Text;
            if (!string.IsNullOrEmpty(projectType))
            {
                oBuilder.AppendFormat(" and t.projecttype={0}", (int)VirtualMachine.Component.Util.EnumUtil<EnumProjectType>.FromDescription(projectType));
            }
            try
            {
                DataSet ds = model.CurrentSrv.GetCurrentProjectInfo(oBuilder.ToString());
                ShowProject(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询项目失败。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void ShowProject(DataSet dsProjectInfos)
        {
            dgDetail.Rows.Clear();
            if (dsProjectInfos == null || dsProjectInfos.Tables.Count == 0 || dsProjectInfos.Tables[0].Rows.Count == 0) return;
            DataTable oTable = dsProjectInfos.Tables[0];
            string sName = string.Empty;
            string type = string.Empty;
            foreach (DataRow oRow in oTable.Rows)
            {
                sName = ClientUtil.ToString(oRow["name"]);
                if (string.IsNullOrEmpty(sName))
                    continue;
                int i = dgDetail.Rows.Add();
                DataGridViewRow dr = dgDetail.Rows[i];
                dr.Tag = oRow;
                dr.Cells[ColNo.Name].Value = i + 1;
                dr.Cells[ColProjectName.Name].Value = sName;//工程名称
                dr.Cells[this.ColProjectCode.Name].Value = ClientUtil.ToString(oRow["code"]);//项目编码
                dr.Cells[ColProManage.Name].Value = ClientUtil.ToString(oRow["HandlePersonName"]);  //项目经理
                type = Enum.GetName(typeof(EnumProjectType), ClientUtil.ToInt(oRow["ProjectType"]));
                dr.Cells[ColProjectType.Name].Value = type;//项目类型
                if (this.execType == CProjectDepartment_ExecType.ProjectBusinessStateMng)
                {
                    dr.Cells["colProjectCurrState"].Value = Enum.GetName(typeof(EnumProjectCurrState), ClientUtil.ToInt(oRow["projectcurrstate"]));
                }
                else if (this.execType == CProjectDepartment_ExecType.ProjectStateMng)
                {
                    dr.Cells["colProjectinfoState"].Value = GetProjectStateName(ClientUtil.ToInt(oRow["projectinfostate"]));
                }
                dr.Cells[ColProStage.Name].Value = ClientUtil.ToString(oRow["ConstractStage"]);//obj.ConstractStage;//施工阶段
                dr.Cells[ColProjectCost.Name].Value = ClientUtil.ToDecimal(oRow["ProjectCost"]);// obj.ProjectCost;//工程花费
                dr.Cells[ColtheGroundArea.Name].Value = ClientUtil.ToDecimal(oRow["BuildingArea"]);// obj.BuildingArea;//建筑面积
                dr.Cells[ColBuildingHeight.Name].Value = ClientUtil.ToDecimal(oRow["BuildingHeight"]);//obj.BuildingHeight;//建筑高度
                dr.Cells[ColGroundLayers.Name].Value = ClientUtil.ToDecimal(oRow["GroundLayers"]);//obj.GroundLayers;//地上层数
                dr.Cells[colUnderLayers.Name].Value = ClientUtil.ToDecimal(oRow["UnderGroundLayers"]);//obj.UnderGroundLayers;//地下层数
                dr.Cells[RankingNote.Name].Value = oRow["materialnote"] + "";   // 物资说明
            }
        }
        public void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dgDetail.EndEdit();
                string sSQLTemp = string.Empty;

                IList lstCurProjectSQL = new ArrayList();
                DataRow oDataRow = null;
                int iProjectCurrState = -1;
                int iProjectInfoState = -1;
                string sID = string.Empty;
                if (CProjectDepartment_ExecType.ProjectStateMng == execType)
                {
                    sSQLTemp = " update resconfig set  projectinfostate={0} where id='{1}'";
                    foreach (DataGridViewRow oRow in dgDetail.Rows)
                    {
                        if (oRow.Tag != null)
                        {
                            oDataRow = oRow.Tag as DataRow;
                            iProjectInfoState = GetProjectStateValue(ClientUtil.ToString(oRow.Cells["colProjectinfoState"].Value));
                            if (iProjectInfoState != ClientUtil.ToInt(oDataRow["projectinfostate"]))
                            {
                                sID = ClientUtil.ToString(oDataRow["id"]);
                                if (!string.IsNullOrEmpty(sID))
                                {
                                    lstCurProjectSQL.Add(string.Format(sSQLTemp, iProjectInfoState, sID));
                                }
                            }
                        }
                    }
                }
                else if (CProjectDepartment_ExecType.ProjectMaterialStateMng == execType)
                {
                    sSQLTemp = " update resconfig set materialnote='{1}'  where id='{0}'";
                    foreach (DataGridViewRow oRow in dgDetail.Rows)
                    {
                        if (oRow.Tag != null)
                        {
                            oDataRow = oRow.Tag as DataRow;
                            var projectTypeNote = oRow.Cells[RankingNote.Name].Value + "";

                            if (projectTypeNote != oDataRow["materialnote"] + "")
                            {
                                sID = ClientUtil.ToString(oDataRow["id"]);
                                if (!string.IsNullOrEmpty(sID))
                                {
                                    lstCurProjectSQL.Add(string.Format(sSQLTemp, sID, projectTypeNote));
                                }
                            }
                        }
                    }
                }
                else { }
                if (lstCurProjectSQL.Count > 0)
                {
                    model.CurrentSrv.SaveUpdateCurrentProjectInfo(lstCurProjectSQL);
                    QueryProjectInfo();
                }
                else
                {
                    throw new Exception("请修改项目信息后保存.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("修改失败:{0}", ex.Message));
            }

        }
        public int GetProjectStateValue(string sName)
        {
            int iResult = 0;
            if (lstProjectState != null && lstProjectState.Count > 0)
            {
                string[] arr = lstProjectState.OfType<BasicDataOptr>().Where(a => a.BasicName == sName).Select(a => a.BasicCode).ToArray();
                if (arr != null && arr.Length > 0)
                {
                    iResult = ClientUtil.ToInt(arr[0]);
                }
            }
            return iResult;
        }
        public string GetProjectStateName(int iValue)
        {
            string sResult = string.Empty;
            string sCode = iValue.ToString();
            if (lstProjectState != null && lstProjectState.Count > 0)
            {
                string[] arr = lstProjectState.OfType<BasicDataOptr>().Where(a => a.BasicCode == sCode).Select(a => a.BasicName).ToArray();
                if (arr != null && arr.Length > 0)
                {
                    sResult = arr[0];
                }
            }
            return sResult;
        }


    }
}
