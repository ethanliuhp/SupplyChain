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
using System.IO;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.Controls;

namespace Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng
{
    public partial class VProDepart : TBasicDataView
    {
        //CurrentProjectInfo projectInfo = null;
        private CurrentProjectInfo projectInfo;
        private MOperationJob operationmodel;
        private MPersonOnJob personmodel;
        public MProjectDepartment model;
        string strProjectName = null;

        string[] picList = null;

        private int picIndex = -1;
        /// <summary>
        /// 项目部基本信息
        /// </summary>
        public CurrentProjectInfo ProjectInfo
        {
            get { return projectInfo; }
            set { projectInfo = value; }
        }
        public VProDepart(MOperationJob mod, MPersonOnJob poj, MProjectDepartment pdt, string strName)
        {

            InitializeComponent();
            operationmodel = mod;
            personmodel = poj;
            model = pdt;
            strProjectName = strName;
            InitDate();
            InitEvents();
            InitForm();
            RefreshControls(0);
        }
        private void InitDate()
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
            //txtState.DataSource = (Enum.GetNames(typeof(EnumProjectInfoState)));
            //资金来源  
            txtMoneySource.DataSource = (Enum.GetNames(typeof(EnumSourcesOfFunding)));
            //资金到位状况 （int 0未到位 1到位）
            txtMoneyStates.Items.AddRange(new object[] { "到位", "未到位" });
            //施工阶段
            //txtConstractStage.Items.AddRange(new object[] { "施工准备", "基础施工", "主体结构", "装饰安装施工", "收尾阶段" });
            VBasicDataOptr.InitProjectConstractStage(this.txtConstractStage, false);

            VBasicDataOptr.InitProjectDepartType(colDepartType, false);
            VBasicDataOptr.InitProjectLivel(txtQuanlityTarget, false);
            VBasicDataOptr.InitProjectSafty(txtSaftyTarget, false);
            VBasicDataOptr.InitBasicFrom(txtBace, false);
            VBasicDataOptr.InitStructFrom(txtStructType, false);

            //timerPic.Enabled = false;
            //timerPic.Interval = 5000;

            cmbTaxType.SelectedIndex = 1;

        }

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public void RefreshControls(int i)
        {
            if (strProjectName == "项目基本信息维护")
            {
                //控制自身控件
                if (i == 1)
                {
                    ObjectLock.Unlock(pnlFloor, true);
                    txtCode.Enabled = false;
                    txtProjectName.Enabled = false;

                    btnAdd.Enabled = true;
                    txtContracte.Enabled = true;
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = true;
                    btnCHAdd.Enabled = true;
                    btnCHDelete.Enabled = true;
                    btnCHSave.Enabled = true;
                    btnCHUpdate.Enabled = true;
                    btnEffect.Enabled = true;
                    btnFlat.Enabled = true;
                    #region 取费
                    btnSelectSelFeeTemplate.Enabled = true;
                    //btnSaveSelFeeTemplate.Enabled = true;
                    colRate.ReadOnly = false;
                    #endregion

                    #region 成本分析参数设置
                    this.dgMachineCostParame.AllowUserToAddRows = true;
                    this.dgMachineCostParame.AllowUserToDeleteRows = true;
                    this.dgSumAreaParame.AllowUserToAddRows = true;
                    this.dgSumAreaParame.AllowUserToDeleteRows = true;
                    #endregion
                }
                else
                {
                    ObjectLock.Lock(pnlFloor, true);
                    txtContracte.Enabled = false;
                    #region 成本分析参数设置
                    this.dgMachineCostParame.AllowUserToAddRows = false;
                    this.dgMachineCostParame.AllowUserToDeleteRows = false;
                    this.dgSumAreaParame.AllowUserToAddRows = false;
                    this.dgSumAreaParame.AllowUserToDeleteRows = false;
                    #endregion
                }
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                btnAdd.Enabled = false;
                txtContracte.ReadOnly = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCHAdd.Enabled = false;
                btnCHDelete.Enabled = false;
                btnCHSave.Enabled = false;
                btnCHUpdate.Enabled = false;
                btnEffect.Enabled = false;
                btnFlat.Enabled = false;
            }

        }

        private void InitEvents()
        {
            //客户信息
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            //工期信息
            btnCHAdd.Click += new EventHandler(btnCHAdd_Click);
            btnCHDelete.Click += new EventHandler(btnCHDelete_Click);
            btnCHSave.Click += new EventHandler(btnCHSave_Click);
            btnCHUpdate.Click += new EventHandler(btnCHUpdate_Click);
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            this.linkMapPoint.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkMapPoint_LinkClicked);

            btnEffect.Click += new EventHandler(btnEffect_Click);
            btnFlat.Click += new EventHandler(btnFlat_Click);
            timerPic.Tick += new EventHandler(timerPic_Tick);
            btnPrevious.Click += new EventHandler(btnPrevious_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
            btnStart.Click += new EventHandler(btnStart_Click);
            btnStop.Click += new EventHandler(btnStop_Click);

            picVirtual.Click += new EventHandler(picVirtual_Click);
            #region 取费
            btnSelectSelFeeTemplate.Click += new EventHandler(btnSelectSelFeeTemplate_Click);
            //btnSaveSelFeeTemplate.Click+=new EventHandler(btnSaveSelFeeTemplate_Click);
            gridSelFeeDetial.CellEndEdit += new DataGridViewCellEventHandler(gridSelFeeDetial_CellEndEdit);
            #endregion

            dgMachineCostParame.CellDoubleClick += new DataGridViewCellEventHandler(dgMachineCostParame_CellDoubleClick);
            dgMachineCostParame.CellEndEdit += new DataGridViewCellEventHandler(dgMachineCostParame_CellEndEdit);
            dgMachineCostParame.MouseUp += new MouseEventHandler(dgMachineCostParame_MouseUp);

            dgSumAreaParame.CellEndEdit += new DataGridViewCellEventHandler(dgSumAreaParame_CellEndEdit);
            dgSumAreaParame.MouseUp += new MouseEventHandler(dgSumAreaParame_MouseUp);

            contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
        }

        string menuitemType = "";
        void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == ItemDel.Name)
            {
                contextMenuStrip1.Hide();
                if (MessageBox.Show("确定删除选中的所有行吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (menuitemType == "dgSumAreaParame")
                    {
                        //if (dgSumAreaParame.SelectedRows != null && dgSumAreaParame.SelectedRows.Count > 0)
                        //{
                        //    foreach (DataGridViewRow item in dgSumAreaParame.SelectedRows)
                        //    {
                        //        dgSumAreaParame.Rows.Remove(item);
                        //    }
                        //}
                        //else 
                        if (!dgSumAreaParame.CurrentRow.IsNewRow)
                        {
                            dgSumAreaParame.Rows.Remove(dgSumAreaParame.CurrentRow);
                        }
                    }
                    else if (menuitemType == "dgMachineCostParame")
                    {
                        //if (dgMachineCostParame.SelectedRows != null && dgMachineCostParame.SelectedRows.Count > 0)
                        //{
                        //    foreach (DataGridViewRow item in dgMachineCostParame.SelectedRows)
                        //    {
                        //        dgMachineCostParame.Rows.Remove(item);
                        //    }
                        //}
                        //else 
                        if (!dgMachineCostParame.CurrentRow.IsNewRow)
                        {
                            dgMachineCostParame.Rows.Remove(dgMachineCostParame.CurrentRow);
                        }
                    }
                }
            }
        }

        void dgSumAreaParame_MouseUp(object sender, MouseEventArgs e)
        {
            //如果是在开始结束时间上点击右键时，显示右键菜单
            if (e.Button == MouseButtons.Right)
            {
                if (!dgSumAreaParame.ReadOnly)
                {
                    menuitemType = "dgSumAreaParame";
                    contextMenuStrip1.Show(dgSumAreaParame, new Point(e.X, e.Y));
                }
            }
        }

        void dgMachineCostParame_MouseUp(object sender, MouseEventArgs e)
        {
            //如果是在开始结束时间上点击右键时，显示右键菜单
            if (e.Button == MouseButtons.Right)
            {
                if (!dgMachineCostParame.ReadOnly)
                {
                    menuitemType = "dgMachineCostParame";
                    contextMenuStrip1.Show(dgMachineCostParame, new Point(e.X, e.Y));
                }
            }
        }

        void dgMachineCostParame_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgMachineCostParame.Rows[e.RowIndex].IsNewRow)
            //{
            //    return;
            //}
            //var rowdata = dgMachineCostParame.Rows[e.RowIndex];
            //dgMachineCostParame.EndEdit();
            //var curColName = dgMachineCostParame.Columns[e.ColumnIndex].Name;
            //if (string.IsNullOrEmpty(rowdata.Cells[colSubjectCode.Name].Value + "") && curColName == colSubjectCode.Name)
            //{
            //    MessageBox.Show(@"所在行的“核算科目”不能为空");
            //    dgMachineCostParame[colSubjectCode.Name, e.RowIndex].Selected = true;
            //    dgMachineCostParame.BeginEdit(true);
            //    return;
            //}
            //if (curColName == colDuration.Name)
            //{
            //    if (string.IsNullOrEmpty(rowdata.Cells[colDuration.Name].Value + ""))
            //    {
            //        MessageBox.Show(@"所在行的“工期”不能为空");
            //        dgMachineCostParame[colSubjectCode.Name, e.RowIndex].Selected = true;
            //        dgMachineCostParame.BeginEdit(true);
            //        return;
            //    }
            //    else
            //    {
            //        decimal val = 0;
            //        if (decimal.TryParse(rowdata.Cells[colDuration.Name].Value + "", out val) == false)
            //        {
            //            MessageBox.Show(@"请填写有效的“工期”");
            //            dgMachineCostParame[colDuration.Name, e.RowIndex].Selected = true;
            //            dgMachineCostParame.BeginEdit(true);
            //            return;
            //        }
            //    }
            //}
            //if (string.IsNullOrEmpty(rowdata.Cells[colActualentryDate.Name].Value + "") && curColName == colActualentryDate.Name)
            //{
            //    MessageBox.Show(@"所在行的“实际进场日期”不能为空");
            //    dgMachineCostParame[colActualentryDate.Name, e.RowIndex].Selected = true;
            //    dgMachineCostParame.BeginEdit(true);
            //    return;
            //}
            //if (string.IsNullOrEmpty(rowdata.Cells[colActualoutDate.Name].Value + "") && curColName == colActualoutDate.Name)
            //{
            //    MessageBox.Show(@"所在行的“实际出场日期”不能为空");
            //    dgMachineCostParame[colActualoutDate.Name, e.RowIndex].Selected = true;
            //    dgMachineCostParame.BeginEdit(true);
            //    return;
            //}

        }

        void dgSumAreaParame_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgSumAreaParame.Rows[e.RowIndex].IsNewRow)
            //{
            //    return;
            //}
            //var rowdata = dgSumAreaParame.Rows[e.RowIndex];
            //var curColName = dgSumAreaParame.Columns[e.ColumnIndex].Name;

            //if (curColName == colYear.Name)
            //{
            //    if (string.IsNullOrEmpty(rowdata.Cells[colYear.Name].Value + ""))
            //    {
            //        MessageBox.Show(@"所在行的“年”不能为空");
            //        dgSumAreaParame[colYear.Name, e.RowIndex].Selected = true;
            //        dgSumAreaParame.BeginEdit(true);
            //        return;
            //    }
            //    else
            //    {
            //        int val = 0;
            //        string str = row.Cells[colYear.Name].Value + "";
            //        if (str.Length != 4 || int.TryParse(str, out  val) == false || val < 2000)
            //        {
            //            MessageBox.Show(@"请填写有效的“年”");
            //            dgSumAreaParame[colYear.Name, e.RowIndex].Selected = true;
            //            dgSumAreaParame.BeginEdit(true);
            //            return;
            //        }
            //    }

            //}
            //if (string.IsNullOrEmpty(rowdata.Cells[colMonth.Name].Value + "") && curColName == colMonth.Name)
            //{
            //    MessageBox.Show(@"所在行的“月”不能为空");
            //    dgSumAreaParame[colMonth.Name, e.RowIndex].Selected = true;
            //    dgSumAreaParame.BeginEdit(true);
            //    return;
            //}
            //if (curColName == colConstructionArea.Name)
            //{
            //    if (string.IsNullOrEmpty(rowdata.Cells[colConstructionArea.Name].Value + ""))
            //    {
            //        MessageBox.Show(@"所在行的“累计建筑面积”不能为空");
            //        dgSumAreaParame[colConstructionArea.Name, e.RowIndex].Selected = true;
            //        dgSumAreaParame.BeginEdit(true);
            //        return;
            //    }
            //    else
            //    {
            //        decimal val = 0;
            //        if (decimal.TryParse(rowdata.Cells[colConstructionArea.Name].Value + "", out  val) == false)
            //        {
            //            MessageBox.Show(@"请填写有效的“累计建筑面积”");
            //            dgSumAreaParame[colConstructionArea.Name, e.RowIndex].Selected = true;
            //            dgSumAreaParame.BeginEdit(true);
            //            return;
            //        }
            //    }
            //}
        }

        void dgMachineCostParame_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var curColName = dgMachineCostParame.Columns[e.ColumnIndex].Name;
            if (e.RowIndex >= 0 && (curColName == colSubjectCode.Name || curColName == colSubjectName.Name))
            {
                //双击核算科目
                VSelectCostAccountSubject frm = new VSelectCostAccountSubject("C51103");//默认只显示："施工机械费"及其子孙节点
                frm.ShowDialog();
                CostAccountSubject cost = frm.SelectAccountSubject;
                if (cost != null)
                {
                    dgMachineCostParame[colSubjectCode.Name, e.RowIndex].Tag = cost.Id;
                    dgMachineCostParame[colSubjectCode.Name, e.RowIndex].Value = cost.Code;
                    dgMachineCostParame[colSubjectName.Name, e.RowIndex].Value = cost.Name;
                }
            }
        }
        //显示图片
        void picVirtual_Click(object sender, EventArgs e)
        {
            timerPic.Enabled = false;
            if (picList == null || picList.Count() == 0) return;
            VShowPictrue frm = new VShowPictrue(picIndex, picList);
            frm.ShowDialog();
        }
        //暂停
        void btnStop_Click(object sender, EventArgs e)
        {
            timerPic.Enabled = false;
        }
        //播放
        void btnStart_Click(object sender, EventArgs e)
        {
            timerPic.Enabled = true;
        }
        //下一张
        void btnNext_Click(object sender, EventArgs e)
        {
            timerPic.Enabled = false;
            //picIndex += 1;
            picIndex -= 1;
            ShowPic(picIndex);
        }
        //上一张
        void btnPrevious_Click(object sender, EventArgs e)
        {
            timerPic.Enabled = false;
            //picIndex -= 1;
            picIndex += 1;
            ShowPic(picIndex);
        }
        //定时器 循环播放
        void timerPic_Tick(object sender, EventArgs e)
        {
            try
            {
                picIndex--;
                //if (picIndex >= picList.Count())
                //{
                //    picIndex = 0;
                //}

                ShowPic(picIndex);

            }
            catch (Exception)
            {
                timerPic.Enabled = false;
            }

        }

        void ShowPic(int indexPic)
        {
            if (picIndex < 0)
            {
                picIndex = picList.Count() - 1;
                indexPic = picIndex;
            }
            //if (picIndex >= picList.Count())
            //{
            //    picIndex = 0;
            //}

            string path = picList[indexPic];
            path = path.Substring(1, path.Length - 2);
            picVirtual.Image = Image.FromStream(System.Net.WebRequest.Create(path).GetResponse().GetResponseStream());


            if (picIndex == 0 || picIndex == picList.Count() - 1)
            {
                if (picList.Count() == 1)
                {
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                }
                else
                {
                    if (picIndex == picList.Count() - 1) //(picIndex == 0)
                    {
                        btnPrevious.Enabled = false;
                        btnNext.Enabled = true;
                    }
                    else if (picIndex == 0)
                    {
                        btnNext.Enabled = false;
                        btnPrevious.Enabled = true;
                    }
                }
            }
            else
            {
                if (!(btnPrevious.Enabled == true && btnNext.Enabled == true))
                {
                    btnPrevious.Enabled = true;
                    btnNext.Enabled = true;
                }
            }
        }

        //上传/修改平面图
        void btnFlat_Click(object sender, EventArgs e)
        {
            selectPic("flat");
        }
        //上传/修改效果图
        void btnEffect_Click(object sender, EventArgs e)
        {
            selectPic("effect");
        }

        void selectPic(string str)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "标签|*.jpg;*.png;*.gif";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                if (str == "effect")
                {
                    txtEffectPic.Text = path;
                    picEffect.Image = Image.FromFile(path);
                }
                else
                {
                    txtFlatPic.Text = path;
                    picFlat.Image = Image.FromFile(path);
                }

            }
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabPage2")
            {
                LoadPerson();
            }
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            try
            {
                if (strProjectName == "项目基本信息维护")
                {
                    base.ModifyView();
                    InitForm();
                    RefreshControls(1);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据修改错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }


        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                projectInfo = model.CurrentSrv.GetProjectById(projectInfo.Id);
                InitForm();
                RefreshControls(0);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (gridSelFeeDetial.Visible == true)
                {
                    this.gridSelFeeDetial.EndEdit();
                    this.gridSelFeeDetial_CellEndEdit(this.gridSelFeeDetial, new DataGridViewCellEventArgs(this.gridSelFeeDetial.CurrentCell.ColumnIndex, this.gridSelFeeDetial.CurrentRow.Index));
                }
                bool flag = false;
                if (string.IsNullOrEmpty(projectInfo.Id))
                {
                    flag = true;
                }

                if (txtProName.Text.Trim() == "")
                {
                    MessageBox.Show("工程名称不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProName.Focus();
                    return false;
                }

                #region
                if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
                {
                    string errMsg = "";
                    string msg = VaildateRates(txtVatRate);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        errMsg += "增资税率" + msg;
                        MessageBox.Show(errMsg);
                        this.txtVatRate.FindForm();
                        return false;
                    }

                    msg = VaildateRates(txtMeasuresFeeRatio);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        errMsg += "措施费比值" + msg;
                        MessageBox.Show(errMsg);
                        this.txtMeasuresFeeRatio.FindForm();
                        return false;
                    }
                    msg = VaildateRates(txtFeesratio);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        errMsg += "规费比值" + msg;
                        MessageBox.Show(errMsg);
                        this.txtFeesratio.Focus();
                        return false;
                    }
                    msg = VaildateRates(txtManagementFeeRatio);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        errMsg += "管理费比值" + msg;
                        MessageBox.Show(errMsg);
                        this.txtManagementFeeRatio.Focus();
                        return false;
                    }
                    msg = VaildateRates(txtTConstructionRatio);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        errMsg += "临时建设费测定比值" + msg;
                        MessageBox.Show(errMsg);
                        this.txtTConstructionRatio.Focus();
                        return false;
                    }
                    msg = VaildateRates(txtElectricRatio);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        errMsg += "电费测定比值" + msg;
                        MessageBox.Show(errMsg);
                        this.txtElectricRatio.Focus();
                        return false;
                    }
                }
                #endregion

                projectInfo.Name = ClientUtil.ToString(txtProjectName.Text);
                //ProjectInfo.ManagerDepart = ClientUtil.ToString(txtProName.Text);
                projectInfo.ManagerDepart = ClientUtil.ToString(txtProName.Text);
                projectInfo.ProjectType = ClientUtil.ToInt(txtProjectType.SelectedIndex);
                projectInfo.ContractType = ClientUtil.ToInt(txtContractWay.SelectedIndex);
                projectInfo.ProjectLocationProvince = ClientUtil.ToString(txtProvince.Text);
                projectInfo.ProjectLocationCity = ClientUtil.ToString(txtCity.Text);
                projectInfo.ProjectLocationDescript = ClientUtil.ToString(txtAddressExplain.Text);
                projectInfo.ContractArea = ClientUtil.ToString(txtContracte.Text);
                projectInfo.StructureType = txtStructType.SelectedIndex;
                projectInfo.BuildingHeight = ClientUtil.ToDecimal(txtProjectHeight.Text);
                projectInfo.GroundLayers = ClientUtil.ToDecimal(txtGroundNum.Text);
                projectInfo.UnderGroundLayers = ClientUtil.ToDecimal(txtUnderNum.Text);
                projectInfo.HandlePersonName = ClientUtil.ToString(this.txtHandlePerson.Text);
                projectInfo.UnderGroundArea = ClientUtil.ToDecimal(txtUnderArea.Text);
                projectInfo.TheGroundArea = ClientUtil.ToDecimal(txtGoundArea.Text);
                projectInfo.BuildingArea = ClientUtil.ToDecimal(txtProjectArea.Text);
                projectInfo.WallProjectArea = ClientUtil.ToDecimal(txtExterWallArea.Text);
                projectInfo.ProjectLifeCycle = EnumUtil<EnumProjectLifeCycle>.FromDescription(txtLifeCycleState.SelectedItem);
                projectInfo.CreateDate = txtCreateDate.Value;
                projectInfo.BaseForm = ClientUtil.ToString(txtBace.Text);

                projectInfo.MapPoint = ClientUtil.ToString(txtMapPoint.Text.Trim());
                projectInfo.ContractIncome = ClientUtil.ToDecimal(txtContractIncome.Text);//自营合同收入
                projectInfo.ResponsCost = ClientUtil.ToDecimal(txtResponsCost.Text);//自营责任成本
                projectInfo.RiskLimits = ClientUtil.ToDecimal(txtRiskLimits.Text);//自营风险额度
                projectInfo.Inproportion = ClientUtil.ToDecimal(txtProportion.Text);//自营责任上缴比例

                projectInfo.BeginDate = txtStartDate.Value;
                projectInfo.EndDate = txtCompleteDate.Value;
                projectInfo.QuanlityTarget = ClientUtil.ToString(txtQuanlityTarget.Text);
                projectInfo.QualityReword = ClientUtil.ToString(txtQualityReword.Text);
                projectInfo.SaftyTarget = ClientUtil.ToString(txtSaftyTarget.Text);
                projectInfo.SaftyReword = ClientUtil.ToString(txtSaftyReword.Text);
                projectInfo.ProjecRewordt = ClientUtil.ToString(txtProReword.Text);

                projectInfo.SourcesOfFunding = EnumUtil<EnumSourcesOfFunding>.FromDescription(txtMoneySource.SelectedItem);
                projectInfo.IsFundsAvailabed = txtMoneyStates.SelectedIndex;
                projectInfo.ProjectCost = ClientUtil.ToDecimal(txtProjectCost.Text) * 10000;
                projectInfo.RealPerMoney = ClientUtil.ToDecimal(txtRealPreMoney.Text) * 10000;
                projectInfo.CivilContractMoney = ClientUtil.ToDecimal(txtConstractMoney.Text) * 10000;
                projectInfo.InstallOrderMoney = ClientUtil.ToDecimal(txtInstallOrderMoney.Text) * 10000;
                projectInfo.ContractCollectRatio = ClientUtil.ToDecimal(txtCollectProport.Text);
                projectInfo.ResProportion = ClientUtil.ToDecimal(txtTurnProport.Text);
                projectInfo.BigModualGroundUpPrice = ClientUtil.ToDecimal(txtGroundPrice.Text);
                projectInfo.BigModualGroundDownPrice = ClientUtil.ToDecimal(txtUnderPrice.Text);
                projectInfo.Descript = ClientUtil.ToString(txtExplain.Text);

                projectInfo.ConstractStage = txtConstractStage.Text;
                projectInfo.AprroachDate = txtInsDate.Value;
                projectInfo.RealKGDate = txtrealKGDate.Value;
                projectInfo.TaxType = cmbTaxType.SelectedIndex;

                #region 效果图
                if (txtEffectPic.Text.Trim() != "")
                {
                    string filePath = txtEffectPic.Text;
                    FileInfo file = new FileInfo(filePath);
                    if (file.Exists)
                    {
                        string fileName = Path.GetFileName(filePath);
                        FileStream fileStream = file.OpenRead();
                        int FileLen = (int)file.Length;
                        Byte[] FileData = new Byte[FileLen];
                        //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                        fileStream.Read(FileData, 0, FileLen);
                        Hashtable ht = model.UpdatePicture(fileName, FileData, projectInfo.Code);
                        if (ht != null)
                        {
                            foreach (System.Collections.DictionaryEntry h in ht)
                            {
                                projectInfo.EffectPicFileCabinetId = h.Key.ToString();
                                projectInfo.EffectPicPath = h.Value.ToString();
                            }
                        }
                    }
                }
                #endregion
                #region 平面图
                if (txtFlatPic.Text.Trim() != "")
                {
                    string filePath = txtFlatPic.Text;
                    FileInfo file = new FileInfo(filePath);
                    if (file.Exists)
                    {
                        string fileName = Path.GetFileName(filePath);
                        FileStream fileStream = file.OpenRead();
                        int FileLen = (int)file.Length;
                        Byte[] FileData = new Byte[FileLen];
                        //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                        fileStream.Read(FileData, 0, FileLen);
                        Hashtable ht = model.UpdatePicture(fileName, FileData, projectInfo.Code);
                        if (ht != null)
                        {
                            foreach (System.Collections.DictionaryEntry h in ht)
                            {
                                projectInfo.FlatPicFileCabinetId = h.Key.ToString();
                                projectInfo.FlatPicPath = h.Value.ToString();
                            }
                        }
                    }
                }
                #endregion

                #region 增值税率，累计面积，分摊参数设置
                if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
                {
                    projectInfo.VatRate = GetPercentValue(txtVatRate.Text.Trim());
                    projectInfo.MeasuresFeeRatio = GetPercentValue(txtMeasuresFeeRatio.Text.Trim());
                    projectInfo.ManagementFeeRatio = GetPercentValue(txtManagementFeeRatio.Text.Trim());
                    projectInfo.FeesRatio = GetPercentValue(txtFeesratio.Text.Trim());
                    projectInfo.TConstructionRatio = GetPercentValue(txtTConstructionRatio.Text.Trim());
                    projectInfo.ElectricRatio = GetPercentValue(txtElectricRatio.Text.Trim());
                    projectInfo.ListSumAreaParame.Clear();
                    dgSumAreaParame.EndEdit();
                    foreach (DataGridViewRow row in dgSumAreaParame.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            SumAreaParame sumParame = null;
                            if (row.Tag == null || string.IsNullOrEmpty((row.Tag as SumAreaParame).Id))
                            {
                                sumParame = new SumAreaParame();
                            }
                            else
                            {
                                sumParame = row.Tag as SumAreaParame;
                            }
                            if (string.IsNullOrEmpty(row.Cells[colYear.Name].Value + ""))
                            {
                                MessageBox.Show("第" + (row.Index + 1) + "行的“年”不能为空");
                                //row.Cells[colYear.Name].Selected = true;
                                //dgSumAreaParame.BeginEdit(true);
                                return false;
                            }
                            else
                            {
                                int val = 0;
                                string str = row.Cells[colYear.Name].Value + "";
                                if (str.Length != 4 || int.TryParse(str, out  val) == false || val < 2000)
                                {
                                    MessageBox.Show(@"请填写有效的“年”");
                                    //row.Cells[colYear.Name].Selected = true;
                                    //dgSumAreaParame.BeginEdit(true);
                                    return false;
                                }
                            }
                            if (string.IsNullOrEmpty(row.Cells[colMonth.Name].Value + ""))
                            {
                                MessageBox.Show("第" + (row.Index + 1) + "行的“月”不能为空");
                                //row.Cells[colMonth.Name].Selected = true;
                                //dgSumAreaParame.BeginEdit(true);
                                return false;
                            }
                            if (string.IsNullOrEmpty(row.Cells[colConstructionArea.Name].Value + ""))
                            {
                                MessageBox.Show("第" + (row.Index + 1) + "行的“累计建筑面积”不能为空");
                                //row.Cells[colConstructionArea.Name].Selected = true;
                                //dgSumAreaParame.BeginEdit(true);
                                return false;
                            }
                            else
                            {
                                decimal val = 0;
                                if (decimal.TryParse(row.Cells[colConstructionArea.Name].Value + "", out  val) == false)
                                {
                                    MessageBox.Show(@"请填写有效的“累计建筑面积”");
                                    //row.Cells[colConstructionArea.Name].Selected = true;
                                    //dgSumAreaParame.BeginEdit(true);
                                    return false;
                                }
                            }
                            sumParame.Year = ClientUtil.ToInt(row.Cells[colYear.Name].Value + "");
                            sumParame.Month = ClientUtil.ToInt(row.Cells[colMonth.Name].Value);
                            sumParame.ConstructionArea = ClientUtil.ToDecimal(row.Cells[colConstructionArea.Name].Value);
                            sumParame.ProjectId = projectInfo.Id;
                            sumParame.Resconfig = projectInfo;

                            projectInfo.ListSumAreaParame.Add(sumParame);
                        }
                    }
                    dgMachineCostParame.EndEdit();
                    foreach (DataGridViewRow row in dgMachineCostParame.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            MachineCostParame machineCostParame = null;
                            if (row.Tag == null || string.IsNullOrEmpty((row.Tag as MachineCostParame).Id))
                            {
                                machineCostParame = new MachineCostParame();
                            }
                            else
                            {
                                machineCostParame = row.Tag as MachineCostParame;
                            }
                            if (string.IsNullOrEmpty(row.Cells[colSubjectCode.Name].Value + "") || string.IsNullOrEmpty(row.Cells[colSubjectName.Name].Value + ""))
                            {
                                MessageBox.Show("第" + (row.Index + 1) + "行的“核算科目”不能为空");
                                //row.Cells[colSubjectCode.Name].Selected = true;
                                //dgMachineCostParame.BeginEdit(true);
                                return false;
                            }
                            if (string.IsNullOrEmpty(row.Cells[colDuration.Name].Value + ""))
                            {
                                MessageBox.Show("第" + (row.Index + 1) + "行的“工期”不能为空");
                                //row.Cells[colDuration.Name].Selected = true;
                                //dgMachineCostParame.BeginEdit(true);
                                return false;
                            }
                            else
                            {
                                decimal val = 0;
                                if (decimal.TryParse(row.Cells[colDuration.Name].Value + "", out val) == false)
                                {
                                    MessageBox.Show(@"请填写有效的“工期”");
                                    //row.Cells[colDuration.Name].Selected = true;
                                    //dgMachineCostParame.BeginEdit(true);
                                    return false;
                                }
                            }
                            if (string.IsNullOrEmpty(row.Cells[colActualentryDate.Name].Value + ""))
                            {
                                MessageBox.Show("第" + (row.Index + 1) + "行的“实际进场日期”不能为空");
                                //row.Cells[colActualentryDate.Name].Selected = true;
                                //dgMachineCostParame.BeginEdit(true);
                                return false;
                            }
                            if (string.IsNullOrEmpty(row.Cells[colActualoutDate.Name].Value + ""))
                            {
                                MessageBox.Show("第" + (row.Index + 1) + "行的“实际出场日期”不能为空");
                                //row.Cells[colActualoutDate.Name].Selected = true;
                                //dgMachineCostParame.BeginEdit(true);
                                return false;
                            }
                            machineCostParame.SubjectId = ClientUtil.ToString(row.Cells[colSubjectCode.Name].Tag);
                            machineCostParame.SubjectCode = ClientUtil.ToString(row.Cells[colSubjectCode.Name].Value);
                            machineCostParame.SubjectName = ClientUtil.ToString(row.Cells[colSubjectName.Name].Value);
                            machineCostParame.Duration = ClientUtil.ToInt(row.Cells[colDuration.Name].Value);
                            machineCostParame.ActualentryDate = ClientUtil.ToDateTime(row.Cells[colActualentryDate.Name].Value);
                            machineCostParame.ActualoutDate = ClientUtil.ToDateTime(row.Cells[colActualoutDate.Name].Value);
                            machineCostParame.ProjectId = projectInfo.Id;
                            machineCostParame.Resconfig = projectInfo;

                            projectInfo.ListMachineCostParame.Add(machineCostParame);
                        }
                    }
                }
                #endregion

                projectInfo = model.CurrentSrv.SaveCurrentProjectInfo(projectInfo);

                //插入日志
                txtCode.Text = projectInfo.Code;
                //更新Caption
                LogData log = new LogData();
                log.BillId = projectInfo.Id;
                log.BillType = "项目部基本信息";
                log.Code = projectInfo.Code;
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = projectInfo.Name;
                log.OperType = "修改";
                StaticMethod.InsertLogData(log);
                MessageBox.Show("保存成功！");
                RefreshControls(0);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }



        void btnAdd_Click(object sender, EventArgs e)
        {
            ObjectLock.Unlock(dgDetail, true);

        }
        void btnUpdate_Click(object sender, EventArgs e)
        {
            ObjectLock.Unlock(dgDetail, true);

        }
        void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                ProRelationUnit unit = new ProRelationUnit();
                if (var.Tag != null)
                {
                    unit = var.Tag as ProRelationUnit;
                    if (unit.Id == null)
                    {
                        projectInfo.ProRelationUnitdetails.Remove(unit);
                    }
                }
                unit.LinkPhone = ClientUtil.ToString(var.Cells[colLinkPhone.Name].Value);
                unit.LinkPersonName = ClientUtil.ToString(var.Cells[colLinkPerson.Name].Value);
                unit.LinkPerson = var.Cells[colLinkPerson.Name].Tag as PersonInfo;
                unit.UnitName = ClientUtil.ToString(var.Cells[colDepartName.Name].Value);
                unit.UnitType = ClientUtil.ToString(var.Cells[colDepartType.Name].Value);
                projectInfo.AddproRelationUnitDetail(unit);
            }
            projectInfo = model.CurrentSrv.SaveCurrentProjectInfo(projectInfo);
            MessageBox.Show("保存成功");
            ObjectLock.Lock(dgDetail, true);
            dgDetail.Rows.Clear();
            LoadCustom();
        }
        void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow.IsNewRow == true || dgDetail.CurrentRow.Tag == null)
            {
                return;
            }
            ProRelationUnit unit = dgDetail.CurrentRow.Tag as ProRelationUnit;
            if (unit.Id == null)
            {
                dgDetail.Rows.Remove(dgDetail.CurrentRow);
                return;
            }
            projectInfo.ProRelationUnitdetails.Remove(unit);
            projectInfo = model.CurrentSrv.SaveCurrentProjectInfo(projectInfo);
            MessageBox.Show("删除成功");
            ObjectLock.Lock(dgDetail, true);
            dgDetail.Rows.Clear();
            LoadCustom();
        }

        void btnCHAdd_Click(object sender, EventArgs e)
        {
            ObjectLock.Unlock(dgNodes, true);
        }
        void btnCHDelete_Click(object sender, EventArgs e)
        {
            if (dgNodes.CurrentRow.IsNewRow == true || dgNodes.CurrentRow.Tag == null)
            {
                return;
            }
            PeriodNode periodNode = new PeriodNode();
            periodNode = dgNodes.CurrentRow.Tag as PeriodNode;
            if (periodNode.Id == null)
            {
                dgNodes.Rows.Remove(dgNodes.CurrentRow);
                return;
            }

            projectInfo.Details.Remove(periodNode);
            projectInfo = model.CurrentSrv.SaveCurrentProjectInfo(projectInfo);
            MessageBox.Show("删除成功");
            ObjectLock.Lock(dgNodes, true);
            dgNodes.Rows.Clear();
            LoadPlan();
        }
        void btnCHSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgNodes.Rows)
            {
                if (var.IsNewRow) break;
                PeriodNode unit = new PeriodNode();
                if (var.Tag != null)
                {
                    unit = var.Tag as PeriodNode;
                    if (unit.Id == null)
                    {
                        projectInfo.Details.Remove(unit);
                    }
                }
                unit.PeriodRequey = ClientUtil.ToString(var.Cells[colGQRequired.Name].Value);
                unit.PerNode = ClientUtil.ToString(var.Cells[colMain.Name].Value);
                projectInfo.AddDetail(unit);
            }
            projectInfo = model.CurrentSrv.SaveCurrentProjectInfo(projectInfo);
            MessageBox.Show("保存成功");
            ObjectLock.Lock(dgNodes, true);
            dgNodes.Rows.Clear();
            LoadPlan();
        }
        void btnCHUpdate_Click(object sender, EventArgs e)
        {
            ObjectLock.Unlock(dgNodes, true);
        }


        private void InitForm()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            projectInfo = model.CurrentSrv.GetProjectById(projectInfo.Id);
            //LoadManager();
            LoadProject();
            LoasdOrder();
            LoadCustom();
            LoadPlan();
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
            {
                LoadSumAreaParame();
                LoadMachineCostParame();

                txtVatRate.Text = (projectInfo.VatRate * 100).ToString();
                txtMeasuresFeeRatio.Text = (projectInfo.MeasuresFeeRatio * 100).ToString();
                txtFeesratio.Text = (projectInfo.FeesRatio * 100).ToString();
                txtManagementFeeRatio.Text = (projectInfo.ManagementFeeRatio * 100).ToString();
                this.txtTConstructionRatio.Text = (projectInfo.TConstructionRatio * 100).ToString();
                this.txtElectricRatio.Text = (projectInfo.ElectricRatio * 100).ToString();
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage6);
                //dgSumAreaParame.Hide();
                //dgMachineCostParame.Hide();
                txtVatRate.Hide();
                customLabel52.Hide();
                label21.Hide();
            }

            #region 取费
            LoadSelFee();
            #endregion
        }
        #region 取费
        public void LoadSelFee()
        {
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.老项目)
            {
                tabControl1.TabPages.Remove(tabPageSelFee);
            }
            else
            {
                int iRow = 0;
                gridSelFeeDetial.Rows.Clear();
                gridSelFormula.Rows.Clear();
                txtSelFeeTemplate.Text = ProjectInfo.SelFeeTemplateName;
                txtSelFeeTemplate.Tag = ProjectInfo.SelFeeTemplateMaster;
                foreach (SelFeeDtl oSelFeeDtl in ProjectInfo.SelFeeDetails)//取费明细
                {
                    iRow = gridSelFeeDetial.Rows.Add();
                    //gridSelFeeDetial[colSpecialType.Name, iRow].Value = oSelFeeDtl.SpecialTypeName;
                    gridSelFeeDetial[colSpecialType.Name, iRow].Value = oSelFeeDtl.SpecialType;
                    gridSelFeeDetial[this.colAccountSubjectName.Name, iRow].Value = oSelFeeDtl.AccountSubjectName;
                    gridSelFeeDetial[this.colAccountSubjectCode.Name, iRow].Value = oSelFeeDtl.AccountSubjectCode;
                    gridSelFeeDetial[this.colRate.Name, iRow].Value = oSelFeeDtl.Rate;
                    gridSelFeeDetial[this.colBeginMoney.Name, iRow].Value = oSelFeeDtl.BeginMoney;
                    gridSelFeeDetial[this.colEndMoney.Name, iRow].Value = oSelFeeDtl.EndMoney;
                    gridSelFeeDetial[this.colMainAccSubjectCode.Name, iRow].Value = oSelFeeDtl.MainAccSubjectCode;
                    gridSelFeeDetial[this.colMainAccSubjectName.Name, iRow].Value = oSelFeeDtl.MainAccSubjectName;
                    gridSelFeeDetial.Rows[iRow].Tag = oSelFeeDtl;
                }
                foreach (SelFeeFormula oSelFeeFormula in ProjectInfo.SelFeeFormulas)//取费公式
                {
                    iRow = gridSelFormula.Rows.Add();
                    gridSelFormula[colAccountSubjectCodeFormula.Name, iRow].Value = oSelFeeFormula.AccountSubjectCode;
                    gridSelFormula[this.colAccountSubjectNameFormula.Name, iRow].Value = oSelFeeFormula.AccountSubjectName;
                    gridSelFormula[this.colFormula.Name, iRow].Value = oSelFeeFormula.Formula;
                    gridSelFormula.Rows[iRow].Tag = oSelFeeFormula;
                }
            }
        }
        public void btnSelectSelFeeTemplate_Click(object sender, EventArgs e)
        {
            VSelFeeTemplateSelect oVSelFeeTemplateSelect = new VSelFeeTemplateSelect();
            oVSelFeeTemplateSelect.StartPosition = FormStartPosition.CenterScreen;
            oVSelFeeTemplateSelect.ShowDialog();
            if (oVSelFeeTemplateSelect.SelectTemplate != null)
            {
                SelFeeDtl oSelFeeDtl = null;
                SelFeeFormula oSelFeeFormula = null;
                ProjectInfo.SelFeeFormulas.Clear();
                ProjectInfo.SelFeeDetails.Clear();
                ProjectInfo.SelFeeTemplateMaster = oVSelFeeTemplateSelect.SelectTemplate;
                ProjectInfo.SelFeeTemplateName = oVSelFeeTemplateSelect.SelectTemplate.Name;
                foreach (SelFeeTemplateDtl oSelFeeTemplateDtl in ProjectInfo.SelFeeTemplateMaster.SelFeeTemplateDetails)
                {
                    oSelFeeDtl = new SelFeeDtl();
                    oSelFeeDtl.SpecialType = oSelFeeTemplateDtl.SpecialType;
                    //oSelFeeDtl.SpecialTypeName = oSelFeeTemplateDtl.TempData;
                    oSelFeeDtl.AccountSubject = oSelFeeTemplateDtl.AccountSubject;
                    oSelFeeDtl.AccountSubjectCode = oSelFeeTemplateDtl.AccountSubjectCode;
                    oSelFeeDtl.AccountSubjectName = oSelFeeTemplateDtl.AccountSubjectName;
                    oSelFeeDtl.Rate = oSelFeeTemplateDtl.Rate;
                    oSelFeeDtl.BeginMoney = oSelFeeTemplateDtl.BeginMoney;
                    oSelFeeDtl.EndMoney = oSelFeeTemplateDtl.EndMoney;
                    oSelFeeDtl.MainAccSubject = oSelFeeTemplateDtl.MainAccSubject;
                    oSelFeeDtl.MainAccSubjectCode = oSelFeeTemplateDtl.MainAccSubjectCode;
                    oSelFeeDtl.MainAccSubjectName = oSelFeeTemplateDtl.MainAccSubjectName;
                    oSelFeeDtl.Descript = oSelFeeTemplateDtl.Descript;
                    //oSelFeeDtl.ProjectInfo = ProjectInfo;
                    //ProjectInfo.SelFeeDetails.Add(oSelFeeDtl);
                    ProjectInfo.AddSelFeeDetails(oSelFeeDtl);
                }
                foreach (SelFeeTempFormula oSelFeeTempFormula in ProjectInfo.SelFeeTemplateMaster.SelFeeTempFormulas)
                {
                    oSelFeeFormula = new SelFeeFormula();
                    oSelFeeFormula.AccountSubject = oSelFeeTempFormula.AccountSubject;
                    oSelFeeFormula.AccountSubjectCode = oSelFeeTempFormula.AccountSubjectCode;
                    oSelFeeFormula.AccountSubjectName = oSelFeeTempFormula.AccountSubjectName;
                    oSelFeeFormula.Formula = oSelFeeTempFormula.Formula;
                    oSelFeeFormula.FormulaCode = oSelFeeTempFormula.FormulaCode;
                    oSelFeeFormula.Descript = oSelFeeTempFormula.Descript;

                    ProjectInfo.AddSelFeeFormulas(oSelFeeFormula);
                    //oSelFeeFormula.ProjectInfo = ProjectInfo;
                    //ProjectInfo.SelFeeFormulas.Add(oSelFeeFormula);
                }
                LoadSelFee();
            }
        }

        public void btnSaveSelFeeTemplate_Click(object sender, EventArgs e)
        {

        }
        public void gridSelFeeDetial_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (colRate.Name == gridSelFeeDetial.Columns[e.ColumnIndex].Name)
            {
                string sRate = ClientUtil.ToString(gridSelFeeDetial[colRate.Name, e.RowIndex].Value);
                if (!CommonMethod.VeryValid(sRate))
                {
                    MessageBox.Show("请输入数值");
                    gridSelFeeDetial[colRate.Name, e.RowIndex].Selected = true;
                    gridSelFeeDetial.BeginEdit(true);
                }
                else
                {
                    SelFeeDtl oSelFeeDtl = gridSelFeeDetial.Rows[e.RowIndex].Tag as SelFeeDtl;
                    oSelFeeDtl.Rate = ClientUtil.ToDecimal(sRate);
                }
            }
        }
        #endregion
        void LoadPerson()
        {
            //string condition = ConstObject.TheLogin.TheOperationOrgInfo.Id;
            if (StaticMethod.GetProjectInfo().Code == CommonUtil.CompanyProjectCode)
            {
                return;
            }
            string condition = StaticMethod.GetProjectInfo().OwnerOrg.Id;
            DataSet ds = model.CurrentSrv.LoadPerson(condition);
            DataTable dt = ds.Tables[0];

            dgPerson.Rows.Clear();
            Hashtable ht = new Hashtable();
            int index = 0;
            foreach (DataRow row in dt.Rows)
            {
                bool flag = false;
                if (ht != null && ht.Count > 0)
                {
                    foreach (System.Collections.DictionaryEntry obj in ht)
                    {
                        DataGridViewRow dr = obj.Key as DataGridViewRow;
                        if (dr.Cells[colDepart.Name].Value.ToString() == row["opgname"].ToString() && dr.Cells[colPost.Name].Value.ToString() == row["opjname"].ToString() && dr.Cells[colPerson.Name].Value.ToString() == row["pername"].ToString())
                        {
                            flag = true;
                            index = Convert.ToInt32(obj.Value);
                        }
                    }
                    if (flag)
                    {
                        if (!dgPerson.Rows[index].Cells[colrole.Name].Value.ToString().Contains(row["rolename"].ToString()))
                        {
                            dgPerson.Rows[index].Cells[colrole.Name].Value += "/" + row["rolename"].ToString();
                        }
                    }
                }
                if (!flag || ht == null || ht.Count == 0)
                {
                    index = dgPerson.Rows.Add();
                    dgPerson.Rows[index].Cells[colDepart.Name].Value = row["opgname"].ToString();
                    dgPerson.Rows[index].Cells[colPost.Name].Value = row["opjname"].ToString();
                    dgPerson.Rows[index].Cells[colPerson.Name].Value = row["pername"].ToString();
                    dgPerson.Rows[index].Cells[colrole.Name].Value = row["rolename"].ToString();
                    ht.Add(dgPerson.Rows[index], index.ToString());
                    flag = false;

                }
            }

        }

        void LoadManager()
        {
            //通过业务组织查询岗位，通过岗位查询角色和人员
            //ObjectQuery oqy = new ObjectQuery();
            //oqy.AddCriterion(Expression.Eq("Id", projectInfo.Id));
            //IList projects = model.CurrentSrv.GetCurrentProjectInfo(oqy);
            //if (projects.Count == 0) return;
            //foreach(CurrentProjectInfo info in projects)
            //{
            //    OperationOrg org = info.OwnerOrg;

            //    int i = dgPerson.Rows.Add();
            //    dgPerson[colDepart.Name, i].Value = org.Name;
            dgPerson.Rows.Clear();
            OperationOrg currNode = projectInfo.OwnerOrg;
            if (currNode == null) return;
            IList lst = operationmodel.GetOperationJob(currNode);//获得岗位
            //IList lst = operationmodel.GetOperationJob(org);//获得岗位
            if (lst.Count == 0) return;
            int k = 0;
            foreach (OperationJob ojob in lst)
            {
                IList lstPerson = personmodel.GetOnJobPersonList(ojob.Id);
                if (lstPerson.Count > 0)
                {
                    int i = dgPerson.Rows.Add();
                    ObjectQuery oqy = new ObjectQuery();
                    oqy.AddCriterion(Expression.Eq("Id", projectInfo.Id));
                    IList projects = model.CurrentSrv.GetCurrentProjectInfo(oqy);
                    if (projects.Count == 0) return;
                    CurrentProjectInfo info = projects[0] as CurrentProjectInfo;

                    dgPerson[colDepart.Name, i].Value = info.OwnerOrg.Name;
                    dgPerson[colPost.Name, i].Value = ojob.Name;
                    dgPerson[colPost.Name, i].Tag = ojob;
                    //通过岗位找人员和角色
                    foreach (PersonOnJob p in lstPerson)
                    {
                        dgPerson.DefaultCellStyle.WrapMode = DataGridViewTriState.True;//自动换行
                        this.dgPerson.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;//自动调节高度
                        dgPerson[colPerson.Name, i].Value += p.StandardPerson.Name + "(" + p.StandardPerson.Code + ")" + "\n";
                        k++;
                    }
                    foreach (OperationJobWithRole obj in ojob.JobWithRole)
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", obj.OperationRole.Id));
                        IList cacheRoleList = new ArrayList();
                        cacheRoleList = operationmodel.OpeJobSrv.GetOperationRole(oq);//获得角色
                        foreach (OperationRole role in cacheRoleList)
                        {
                            dgPerson[colrole.Name, i].Value += role.RoleName + "/";
                        }
                    }
                    string strPerson = ClientUtil.ToString(dgPerson[colPerson.Name, i].Value);
                    if (strPerson != "")
                    {
                        dgPerson[colPerson.Name, i].Value = strPerson.Substring(0, strPerson.Length - 1);
                    }
                    string strrole = ClientUtil.ToString(dgPerson[colrole.Name, i].Value);
                    if (strrole != "")
                    {
                        dgPerson[colrole.Name, i].Value = strrole.Substring(0, strrole.Length - 1);
                    }
                }
            }
            label3.Text = "共计：【" + k.ToString() + "】人";
        }
        //工程简介
        void LoadProject()
        {
            txtProjectName.Text = ClientUtil.ToString(ProjectInfo.Name);
            txtProName.Text = ClientUtil.ToString(ProjectInfo.ManagerDepart);
            txtCode.Text = ClientUtil.ToString(ProjectInfo.Code);
            txtProjectType.SelectedIndex = projectInfo.ProjectType;//工程类型
            txtContractWay.SelectedIndex = projectInfo.ContractType;//承包方式
            txtProvince.Text = ClientUtil.ToString(projectInfo.ProjectLocationProvince);
            txtCity.Text = ClientUtil.ToString(projectInfo.ProjectLocationCity);
            txtAddressExplain.Text = ClientUtil.ToString(projectInfo.ProjectLocationDescript);
            txtContracte.Text = ClientUtil.ToString(projectInfo.ContractArea); //承包范围

            txtStructType.SelectedIndex = ClientUtil.ToInt(projectInfo.StructureType);//结构类型
            txtProjectHeight.Text = ClientUtil.ToString(projectInfo.BuildingHeight);//建筑高度
            txtGroundNum.Text = ClientUtil.ToString(projectInfo.GroundLayers);//地上层数
            this.txtHandlePerson.Text = ClientUtil.ToString(projectInfo.HandlePersonName);//项目经理
            txtUnderNum.Text = ClientUtil.ToString(projectInfo.UnderGroundLayers);//地下层数
            txtGoundArea.Text = ClientUtil.ToString(projectInfo.TheGroundArea);//地上面积
            txtUnderArea.Text = ClientUtil.ToString(projectInfo.UnderGroundArea);//地下面积
            txtProjectArea.Text = ClientUtil.ToString(projectInfo.BuildingArea);//建筑面积
            txtExterWallArea.Text = ClientUtil.ToString(projectInfo.WallProjectArea);//投影面积
            txtLifeCycleState.SelectedItem = EnumUtil<EnumProjectLifeCycle>.GetDescription(projectInfo.ProjectLifeCycle);//生命周期状态
            txtMapPoint.Text = ClientUtil.ToString(projectInfo.MapPoint);//地图坐标
            if (projectInfo.CreateDate > ClientUtil.ToDateTime("2000-01-01"))
            {
                txtCreateDate.Value = projectInfo.CreateDate;//开工时间
            }
            txtBace.Text = projectInfo.BaseForm;//基础形式
            txtContractIncome.Text = ClientUtil.ToString(projectInfo.ContractIncome);//自营合同收入
            txtResponsCost.Text = ClientUtil.ToString(projectInfo.ResponsCost);//自营责任成本
            txtRiskLimits.Text = ClientUtil.ToString(projectInfo.RiskLimits);//自营风险额度
            txtProportion.Text = ClientUtil.ToString(projectInfo.Inproportion);//自营责任上缴比例

            if (!string.IsNullOrEmpty(projectInfo.EffectPicPath))
            {
                string path = StaticMethod.GetPicturePath(projectInfo.EffectPicFileCabinetId, projectInfo.EffectPicPath);

                if (path != "")
                    picEffect.Image = Image.FromFile(path);

            }
            if (!string.IsNullOrEmpty(projectInfo.FlatPicPath))
            {
                string path = StaticMethod.GetPicturePath(projectInfo.FlatPicFileCabinetId, projectInfo.FlatPicPath);

                if (path != "")
                    picFlat.Image = Image.FromFile(path);

            }

            string str = TransUtil.SendRequest(projectInfo.Code);
            if (str != "")
            {
                str = str.Substring(1, str.Length - 2);
            }
            if (str != "")
            {
                picList = str.Split(',');
                timerPic.Enabled = false;

                picIndex = picList.Count() - 1;
                ShowPic(picIndex);
            }
            else
            {
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnStart.Enabled = false;
                btnStop.Enabled = false;
            }

        }
        //合同摘要
        void LoasdOrder()
        {
            if (projectInfo.BeginDate > ClientUtil.ToDateTime("2000-01-01"))
            {
                txtStartDate.Value = projectInfo.BeginDate;
            }
            if (projectInfo.EndDate > ClientUtil.ToDateTime("2000-01-01"))
            {
                txtCompleteDate.Value = projectInfo.EndDate;
            }
            txtQuanlityTarget.Text = ClientUtil.ToString(projectInfo.QuanlityTarget);//质量目标
            txtQualityReword.Text = ClientUtil.ToString(projectInfo.QualityReword);//质量奖惩
            txtSaftyTarget.Text = ClientUtil.ToString(projectInfo.SaftyTarget);//安全目标
            txtSaftyReword.Text = ClientUtil.ToString(projectInfo.SaftyReword);//安全奖惩
            txtProReword.Text = ClientUtil.ToString(projectInfo.ProjecRewordt);//工期奖惩

            txtMoneySource.Text = EnumUtil<EnumSourcesOfFunding>.GetDescription(projectInfo.SourcesOfFunding);//资金来源
            txtMoneyStates.SelectedIndex = projectInfo.IsFundsAvailabed;//资金到位情况
            txtProjectCost.Text = ClientUtil.ToString(projectInfo.ProjectCost / 10000);//工程造价
            txtRealPreMoney.Text = ClientUtil.ToString(projectInfo.RealPerMoney / 10000);//实际预算总金额
            txtConstractMoney.Text = ClientUtil.ToString(projectInfo.CivilContractMoney / 10000);//土建合同金额
            txtInstallOrderMoney.Text = ClientUtil.ToString(projectInfo.InstallOrderMoney / 10000);//安装合同金额
            txtCollectProport.Text = ClientUtil.ToString(projectInfo.ContractCollectRatio);//合同收款比例
            txtTurnProport.Text = ClientUtil.ToString(projectInfo.ResProportion);//责任上缴比例
            txtGroundPrice.Text = ClientUtil.ToString(projectInfo.BigModualGroundUpPrice);
            txtUnderPrice.Text = ClientUtil.ToString(projectInfo.BigModualGroundDownPrice);
            txtExplain.Text = ClientUtil.ToString(projectInfo.Descript);//备注信息（项目说明）

            cmbTaxType.SelectedIndex = projectInfo.TaxType;
        }

        void LoadCustom()
        {
            dgDetail.Rows.Clear();
            foreach (ProRelationUnit unit in projectInfo.ProRelationUnitdetails)
            {
                int j = dgDetail.Rows.Add();
                dgDetail[colDepartType.Name, j].Value = unit.UnitType;
                dgDetail[colDepartName.Name, j].Value = unit.UnitName;
                dgDetail[colLinkPerson.Name, j].Value = unit.LinkPersonName;
                dgDetail[colLinkPerson.Name, j].Tag = unit.LinkPerson as PersonInfo;
                dgDetail[colLinkPhone.Name, j].Value = unit.LinkPhone;
                dgDetail.Rows[j].Tag = unit;
            }
        }
        //施工策划
        void LoadPlan()
        {
            dgNodes.Rows.Clear();

            txtConstractStage.Text = projectInfo.ConstractStage;//施工情况
            if (projectInfo.AprroachDate > ClientUtil.ToDateTime("2000-1-1"))
            {
                txtInsDate.Value = ClientUtil.ToDateTime(projectInfo.AprroachDate);//进场日期
            }
            if (projectInfo.RealKGDate > ClientUtil.ToDateTime("2000-1-1"))
            {
                txtrealKGDate.Value = ClientUtil.ToDateTime(projectInfo.RealKGDate);//实际开工日期
            }
            foreach (PeriodNode node in projectInfo.Details)
            {
                int i = dgNodes.Rows.Add();
                dgNodes[colMain.Name, i].Value = node.PerNode;//主要工期控制点
                dgNodes[colGQRequired.Name, i].Value = node.PeriodRequey;
                dgNodes.Rows[i].Tag = node;
            }
        }

        void LoadSumAreaParame()
        {
            dgSumAreaParame.Rows.Clear();
            int j = 0;
            foreach (SumAreaParame parame in projectInfo.ListSumAreaParame)
            {
                j = dgSumAreaParame.Rows.Add();
                dgSumAreaParame[colYear.Name, j].Value = parame.Year;
                dgSumAreaParame[colMonth.Name, j].Value = parame.Month.ToString();
                dgSumAreaParame[colConstructionArea.Name, j].Value = parame.ConstructionArea;
                dgSumAreaParame.Rows[j].Tag = parame;
            }
        }

        void LoadMachineCostParame()
        {
            dgMachineCostParame.Rows.Clear();
            int j = 0;
            foreach (MachineCostParame parame in projectInfo.ListMachineCostParame)
            {
                j = dgMachineCostParame.Rows.Add();
                dgMachineCostParame[colSubjectCode.Name, j].Tag = parame.SubjectId;
                dgMachineCostParame[colSubjectCode.Name, j].Value = parame.SubjectCode;
                dgMachineCostParame[colSubjectName.Name, j].Value = parame.SubjectName;
                dgMachineCostParame[colDuration.Name, j].Value = parame.Duration;
                dgMachineCostParame[colActualentryDate.Name, j].Value = parame.ActualentryDate;
                dgMachineCostParame[colActualoutDate.Name, j].Value = parame.ActualoutDate;
                dgMachineCostParame.Rows[j].Tag = parame;
            }

        }
        public void linkMapPoint_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //this.lbllink_LinkClicked.Links[0].LinkData = "http://www.sina.com";
            //System.Diagnostics.Process.Start(e.Link.LinkData.ToString());    
            linkMapPoint.Links[0].LinkData = "http://api.map.baidu.com/lbsapi/getpoint/";
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }


        /// <summary>
        /// 验证测定比值非空，取值范围在0~100之间
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        private string VaildateRates(CustomEdit editor)
        {
            var str = editor.Text.Trim();
            if (string.IsNullOrEmpty(str))
            {
                return "不能为空！";
            }
            else
            {
                var decimalVal = ClientUtil.ToDecimal(str);
                if (decimalVal < 0 && decimalVal > 100)
                {
                    return "的值必须在0到100之间！"; ;
                }
                return "";
            }
        }

        /// <summary>
        /// 将值转换成百分比的小数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private decimal GetPercentValue(string str)
        {
            var decimalVal = ClientUtil.ToDecimal(str);
            return Math.Round(decimalVal / 100, 4);
        }
    }
}
