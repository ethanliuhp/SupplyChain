using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VBillSubjectUpdate : TBasicDataView
    {
        public CSubContractBalance cBalance;
        private MStockMng stockmodel = new MStockMng();
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        public MSubContractBalance model = new MSubContractBalance();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        private Hashtable subject_ht = new Hashtable();//科目名称集合

        public VBillSubjectUpdate()
        {
            InitializeComponent();
            InitForm();
            RefreshControls(MainViewState.Browser);
        }

        public VBillSubjectUpdate(CSubContractBalance c)
        {
            InitializeComponent();
            cBalance = c;
            InitForm();
            RefreshControls(MainViewState.Browser);
        }

        private void InitForm()
        {
            InitEvents();
            DateTime serverTime = model.GetServerTime();
            dtpBalanceDateBegin.Value = serverTime.Date.AddMonths(-1);
            projectInfo = StaticMethod.GetProjectInfo();
            subject_ht = model.SubBalSrv.GetCostSubjectNameList();
            btnAutoSynUpdate.Visible = (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目);
            
        }

        private void InitEvents()
        {
            btnSelect.Click += new EventHandler(btnSelect_Click);
            btnBalanceSelect.Click += new EventHandler(btnBalanceSelect_Click);
            this.btnGWBSSelect.Click += new EventHandler(btnGWBSSelect_Click);
            this.btnExcelBill.Click += new EventHandler(btnExcelBill_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnSelectCostSubject.Click += new EventHandler(btnSelectSubject_Click);
            this.btnRSelSubject.Click += new EventHandler(btnRSelSubject_Click);
            this.btnRSave.Click += new EventHandler(btnRSave_Click);
            this.btnRQuery.Click += new EventHandler(btnRQuery_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnRSelectCode.Click+=new EventHandler(btnRSelectCode_Click);
            dgDetail.CellDoubleClick+=new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            #region 资源调整 资源和科目批量调整 及 选择行
            dgBalanceDtl.CurrentCellDirtyStateChanged += dgBalanceDtl_CurrentCellDirtyStateChanged;
            this.btnGWBSDetial.Click += btnGWBSDetial_Click;
            dgBalanceDtl.CellEndEdit += dgBalanceDtl_CellValueChanged;
            dgBalanceDtl.CellDoubleClick += dgBalanceDtl_CellContentDoubleClick;
            //dgBalanceDtl.CellValueChanged += dgBalanceDtl_CellValueChanged;
            lnkSelectAll.Click += LinkClick_click;
            lnkUnSelected.Click += LinkClick_click;
            btnSelectBatchSubject.Click += btnSelectBatchSubject_Click;
            btnBatchSetSubject.Click += btnBatchSetSubject_Click;
        
            btnSelectBatchResource.Click += btnSelectBatchResource_Click;
            btnBatchSetResource.Click += btnBatchSetResource_Click;
            txtSelectBatchSubject.tbTextChanged += txtSelectBatchSubject_LostFocus;
            btnAutoSynUpdate.Click += btnAutoSynUpdate_Click;
            #endregion
        }

         #region 资源调整 资源和科目批量调整 及 选择行
        private void dgBalanceDtl_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.dgBalanceDtl.IsCurrentCellDirty) //有未提交的更//改
            {
                this.dgBalanceDtl.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        void btnGWBSDetial_Click(object sender, EventArgs e)
        {
            VSelectGWBSDetail frm = new VSelectGWBSDetail(new MGWBSTree());
            frm.ShowDialog();
            if (frm.IsOk && frm.SelectGWBSDetail != null && frm.SelectGWBSDetail.Count>0)
            {
                GWBSDetail oGWBSDetail = frm.SelectGWBSDetail[0];
                txtGWBSDetial.Text = oGWBSDetail.Name;
                txtGWBSDetial.Tag = oGWBSDetail;
                txtBalance.Text = oGWBSDetail.TheGWBS.Name;
                txtBalance.Tag = oGWBSDetail.TheGWBS;
            }
        }
        void dgBalanceDtl_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < dgBalanceDtl.Rows.Count && e.ColumnIndex > -1 && e.ColumnIndex < dgBalanceDtl.Columns.Count)
            {
                DataGridViewColumn oColumn = dgBalanceDtl.Columns[e.ColumnIndex];
                if (oColumn == colSelect || oColumn==colOkSubjectName||oColumn==colOKResourceTypeName)
                {
                    SetControl();
                }

               
            }
        }
     
        void dgBalanceDtl_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //this.dgBalanceDtl.CellValueChanged -= dgBalanceDtl_CellValueChanged;
            if (e.RowIndex > -1 && e.RowIndex < dgBalanceDtl.Rows.Count && e.ColumnIndex > -1 && e.ColumnIndex < dgBalanceDtl.Columns.Count)
            {
                DataGridViewColumn oColumn = dgBalanceDtl.Columns[e.ColumnIndex];
                if (oColumn == colSelect)
                {
                    SetControl();
                }
                else if (oColumn == colOKResourceTypeName)
                {
                    //DataGridViewCell oCell = dgBalanceDtl[e.RowIndex, e.ColumnIndex];
                    DataGridViewCell oCell = dgBalanceDtl[e.ColumnIndex, e.RowIndex];
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.OpenSelect();
                    if (materialSelector.Result != null && materialSelector.Result.Count > 0)
                    {
                        Material oMaterial=materialSelector.Result[0] as Material;
                        oCell.Tag = oMaterial;
                        oCell.Value = oMaterial.Name;
                        SetControl();
                    }
                }

            }
           // this.dgBalanceDtl.CellValueChanged += dgBalanceDtl_CellValueChanged;
        }
        void LinkClick_click(object sender, EventArgs e)
        {
            if (sender is LinkLabel)
            {
               // dgBalanceDtl.CellValueChanged -= dgBalanceDtl_CellValueChanged;
                LinkLabel lbl = sender as LinkLabel;
                if (lbl.Text == "全选")
                {
                    foreach (DataGridViewRow oRow in dgBalanceDtl.Rows)
                    {
                        oRow.Cells[colSelect.Name].Value = true;
                    }
                    SetControl();
                }
                else if (lbl.Text == "反选")
                {
                    foreach (DataGridViewRow oRow in dgBalanceDtl.Rows )
                    {
                        oRow.Cells[colSelect.Name].Value = !ClientUtil.ToBool(oRow.Cells[colSelect.Name].FormattedValue);
                    }
                    SetControl();
                }
               // dgBalanceDtl.CellValueChanged += dgBalanceDtl_CellValueChanged;
            }
        }
        void btnSelectBatchSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.IsLeafSelect = true;
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                txtSelectBatchSubject.Tag = cost;
                txtSelectBatchSubject.Text = cost.Name;
                SetControl();
            }
        }
        void btnBatchSetSubject_Click(object sender, EventArgs e)
        {
            string sCostSubject=txtSelectBatchSubject.Text.Trim() ;
            if (string.IsNullOrEmpty(sCostSubject))
            {
                MessageBox.Show("无法进行替换:请输入或者选择核算科目");
                txtSelectBatchSubject.Select ();
                return;
            }
            else
            {
                if (!subject_ht.Contains(sCostSubject))
                {
                    MessageBox.Show(string.Format("无法进行替换:请输入或者选择核算科目无效[{0}]", sCostSubject));
                    txtSelectBatchSubject.Select();
                    return;
                }
                else
                {
                    if (dgBalanceDtl.Rows.Count == 0)
                    {
                        MessageBox.Show("无法进行替换:请查询后在进行替换相应的分包结算单的资源耗用中的核算科目");
                        return;
                    }
                    IEnumerable<DataGridViewRow> lstRow = dgBalanceDtl.Rows.OfType<DataGridViewRow>().Where(a => ClientUtil.ToBool(a.Cells[colSelect.Name].FormattedValue));
                    if (lstRow != null && lstRow.Count() > 0)
                    {
                      //  this.dgBalanceDtl.CellValueChanged -= dgBalanceDtl_CellValueChanged;
                        foreach (DataGridViewRow oRow in lstRow)
                        {
                            oRow.Cells[colOkSubjectName.Name].Value = sCostSubject;
                        }
                       // this.dgBalanceDtl.CellValueChanged += dgBalanceDtl_CellValueChanged;
                    }
                    else
                    {
                        MessageBox.Show("无法进行替换:请勾选需要进行替换相应的分包结算单的资源耗用");
                        return;
                    }
                }
            }
       
        }
        
        void btnSelectBatchResource_Click(object sender, EventArgs e)
        {
            CommonMaterial materialSelector = new CommonMaterial();
            materialSelector.OpenSelect();
            if (materialSelector.Result != null && materialSelector.Result.Count > 0)
            {
                Material oMaterial = materialSelector.Result[0] as Material;
                this.txtSelectBatchResource.Tag = oMaterial;
                this.txtSelectBatchResource.Text = oMaterial.Name;
                SetControl();
            }
        }
        void btnBatchSetResource_Click(object sender, EventArgs e)
        {
            Material oMaterial = txtSelectBatchResource.Tag as Material;
            if (oMaterial==null)
            {
                MessageBox.Show("无法进行替换:请选择替换后的资源");
                txtSelectBatchSubject.Select();
                return;
            }
            else
            {
               
                    if (dgBalanceDtl.Rows.Count == 0)
                    {
                        MessageBox.Show("无法进行替换:请查询后在进行替换相应的分包结算单的资源耗用中的资源名称");
                        return;
                    }
                    IEnumerable<DataGridViewRow> lstRow = dgBalanceDtl.Rows.OfType<DataGridViewRow>().Where(a => ClientUtil.ToBool(a.Cells[colSelect.Name].FormattedValue));
                    if (lstRow != null && lstRow.Count() > 0)
                    {
                        //this.dgBalanceDtl.CellValueChanged -= dgBalanceDtl_CellValueChanged;
                        DataGridViewCell oCell = null;
                        foreach (DataGridViewRow oRow in lstRow)
                        {
                           oCell= oRow.Cells[this.colOKResourceTypeName.Name];
                           oCell.Value = oMaterial.Name;
                           oCell.Tag = oMaterial;
                        }
                       // this.dgBalanceDtl.CellValueChanged += dgBalanceDtl_CellValueChanged;
                    }
                    else
                    {
                        MessageBox.Show("无法进行替换:请勾选需要进行替换相应的分包结算单的资源耗用");
                        return;
                    }
                
            }

        }
        public void SetControl()
        {

            bool IsExist = dgBalanceDtl.Rows.Count > 0 && dgBalanceDtl.Rows.OfType<DataGridViewRow>().ToList().Exists(a => ClientUtil.ToBool(a.Cells[colSelect.Name].FormattedValue));
            btnBatchSetSubject.Enabled = !string.IsNullOrEmpty(txtSelectBatchSubject.Text) && subject_ht.Contains(txtSelectBatchSubject.Text) && IsExist;
            btnBatchSetResource.Enabled = (!string.IsNullOrEmpty(this.txtSelectBatchResource.Text.Trim())) && txtSelectBatchResource.Tag != null && IsExist;
            IsExist = dgBalanceDtl.Rows.Count > 0 && dgBalanceDtl.Rows.OfType<DataGridViewRow>().ToList().Exists(a => a.Cells[colOKResourceTypeName.Name].Tag != null || (a.Cells[colOKResourceTypeName.Name].FormattedValue != null));
            btnSave.Enabled = IsExist;
        }
        public void txtSelectBatchSubject_LostFocus(object sender, EventArgs e)
        {
            SetControl();
        }
        public void btnAutoSynUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code == CommonUtil.CompanyProjectCode)
                {
                    MessageBox.Show("公司和分公司机关的分包结算单与预算体系上的科目和资源不能进行同步");
                    return;
                }
                else
                {
                    string sProjectId = projectInfo.Id;
                    int iCount = 0, iResouceCount = 0, iSubjectCount = 0;
                    //获取需要同步记录条数
                    DataDomain data = model.SubBalSrv.GetAutoSynCount(sProjectId);
                    //iCount,iResouceCount, iSubjectCount
                    if (data == null || ClientUtil.ToInt(data.Name1) == 0)
                    {
                        MessageBox.Show("预算体系与分包结算单耗用直接没有数据需要同步");
                        return;
                    }
                    else
                    {
                        iCount = ClientUtil.ToInt(data.Name1);
                        iResouceCount = ClientUtil.ToInt(data.Name2);
                        iSubjectCount = ClientUtil.ToInt(data.Name3);
                        if (MessageBox.Show(string.Format("预算体系与分包结算单耗用需要同步数据条数{0}条(其中物资{1}条，科目{2}条)", iCount, iResouceCount, iSubjectCount), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            model.SubBalSrv.ExeAutoSyn(sProjectId);
                            MessageBox.Show("同步成功");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("分包结算单耗用与预算体系同步失败:",ExceptionUtil.ExceptionMessage(ex)));
            }
        }
       #endregion
        void btnSelectSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.IsLeafSelect = false;
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                this.txtCostSubject.Text = cost.Name;
                this.txtCostSubject.Tag = cost;
            }
        }

        void btnRSelSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.IsLeafSelect = false;
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                this.txtRSubject.Text = cost.Name;
                this.txtRSubject.Tag = cost;
            }
        }

        void btnBalanceSelect_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtBalance.Text = task.Name;
                    txtBalance.Tag = task;
                }
            }
        }

        void btnGWBSSelect_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    this.txtUsedPart.Text = task.Name;
                    txtUsedPart.Tag = task;
                }
            }
        }

        void btnExcelBill_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgBalanceDtl, true);
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSelectBalOrg_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vces = new VContractExcuteSelector();
            vces.StartPosition = FormStartPosition.CenterScreen;
            vces.ShowDialog();
            if (vces.Result != null && vces.Result.Count > 0)
            {
                SubContractProject scp = vces.Result[0] as SubContractProject;
            }
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vces = new VContractExcuteSelector();
            vces.StartPosition = FormStartPosition.CenterScreen;
            vces.ShowDialog();
            if (vces.Result != null && vces.Result.Count > 0)
            {
                SubContractProject scp = vces.Result[0] as SubContractProject;
                txtBalanceName.Text = scp.BearerOrgName;
                txtBalanceName.Tag = scp;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            IList okList = new ArrayList();
            List<DataDomain> okListResource = null ;
            foreach (DataGridViewRow var in this.dgBalanceDtl.Rows)
            {
                string dtlsubjectid = "";
                DataDomain dDomain = new DataDomain();
                if (var.Tag != null)
                {
                    dtlsubjectid = var.Tag as string;
                }
                string okSubjectName = ClientUtil.ToString(var.Cells[this.colOkSubjectName.Name].Value);
                if (ClientUtil.ToString(okSubjectName) != "")
                {
                    if (subject_ht.Contains(okSubjectName) == false)
                    {
                        MessageBox.Show("核算科目名称[" + okSubjectName + "]无效，请核对！");
                        return;
                    }
                    dDomain.Name1 = dtlsubjectid;
                    dDomain.Name2 = ClientUtil.ToString(subject_ht[okSubjectName]);
                    okList.Add(dDomain);
                }
            }
            //t.resourcetypeguid,t.resourcetypename,t.resourcetypestuff,resourcetypespec,t.resourcesyscode
            Material oMaterial=null;
            DataDomain oDataDomain = null;
            okListResource = new List<DataDomain>();
            foreach (DataGridViewRow oRow in dgBalanceDtl.Rows.OfType<DataGridViewRow>().
                 Where(a => a.Cells[colOKResourceTypeName.Name].Tag != null))
            {
                oMaterial = oRow.Cells[colOKResourceTypeName.Name].Tag as Material;
                oDataDomain = new DataDomain() { 
                    Name1 = oRow .Tag,
                    Name2 = oMaterial.Id,
                    Name3=oMaterial.Name,
                    Name4=oMaterial.Stuff,
                    Name5=oMaterial.Specification,
                    Name6=oMaterial.TheSyscode
                };
                okListResource.Add(oDataDomain);
            }
            if (okList.Count > 0 || okListResource.Count>0)
            {
                model.SubBalSrv.UpdateBillSubjectInfo(okList,okListResource, 1);
                if (okList.Count > 0 && okListResource.Count > 0)
                {
                    MessageBox.Show("核算科目和资源名称调整成功,请重新查询！");
                }
                else if (okList.Count > 0)
                {
                    MessageBox.Show("核算科目调整成功,请重新查询！");
                }
                else 
                {
                    MessageBox.Show("核算科目调整成功,请重新查询！");
                }
            }
            
            else
            {
                MessageBox.Show("无核算科目和资源名称调整信息！");
            }
        }

        void btnRSave_Click(object sender, EventArgs e)
        {
            IList okList = new ArrayList();
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                string dtlid = "";
                DataDomain dDomain = new DataDomain();
                if (var.Tag != null)
                {
                    dtlid = var.Tag as string;
                }
                string okSubjectName = ClientUtil.ToString(var.Cells[this.colRSubjectName.Name].Value);
                if (ClientUtil.ToString(okSubjectName) != "")
                {
                    if (subject_ht.Contains(okSubjectName) == false)
                    {
                        MessageBox.Show("核算科目名称[" + okSubjectName + "]无效，请核对！");
                        return;
                    }
                    dDomain.Name1 = dtlid;
                    dDomain.Name2 = ClientUtil.ToString(subject_ht[okSubjectName]);
                    okList.Add(dDomain);
                }
            }
            if (okList.Count > 0)
            {
                model.SubBalSrv.UpdateBillSubjectInfo(okList,null, 2);
                MessageBox.Show("核算科目调整成功,请重新查询！");
            }
            else {
                MessageBox.Show("无核算科目调整信息！");
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            //this.dgBalanceDtl.CellValueChanged -= dgBalanceDtl_CellValueChanged;
            string condition = "";
            projectInfo = StaticMethod.GetProjectInfo();
            if (this.txtCodeBegin.Text != "")
            {
                condition = condition + " and t1.Code like '%" + this.txtCodeBegin.Text + "%'";
            }
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.CreateDate>='" + dtpBalanceDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpBalanceDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.CreateDate>=to_date('" + dtpBalanceDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpBalanceDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }
            //物资
            if (!txtMaterial.Text.Trim().Equals("") && txtMaterial.Result != null)
            {
                condition += " and t3.Resourcetypename like '%" + txtMaterial.Text + "%'";
            }
            // GWBS
            GWBSTree selectGWBS = txtBalance.Tag as GWBSTree;
            if (this.txtBalance.Text != "" && selectGWBS != null)
            {
                condition += " and t2.balancetasksyscode like '%" + selectGWBS.Id + "%'";

            }
            if (!string.IsNullOrEmpty(this.txtGWBSDetial.Text) && this.txtGWBSDetial.Tag != null)
            {
                condition += string.Format(" and t2.balancetaskdtlguid='{0}' ",( txtGWBSDetial.Tag as GWBSDetail).Id);
            }
            // 科目
            CostAccountSubject selectCostSub = txtCostSubject.Tag as CostAccountSubject;
            if (this.txtCostSubject.Text != "" && selectCostSub != null)
            {
                condition += " and t3.balancesubjectsyscode like '%" + selectCostSub.Id + "%'";

            }
            if (txtBalanceName.Text != "")
            {
                condition = condition + " and t1.subcontractunitname like '%" + txtBalanceName.Text + "%'";
            }

            condition = condition + " and t1.projectid ='" + projectInfo.Id + "'";
            DataSet ds = model.SubBalSrv.SubContractBalanceQuery(condition);
            this.dgBalanceDtl.Rows.Clear();

            DataTable dt = ds.Tables[0];

            decimal sumMoney = 0;
            foreach (DataRow row in dt.Rows)
            {
                int i = dgBalanceDtl.Rows.Add();
                dgBalanceDtl[colBalanceCode.Name, i].Value = row["code"];
                dgBalanceDtl.Rows[i].Tag = row["dtlsubjectid"];
                dgBalanceDtl[colSubContractUnit.Name, i].Value = ClientUtil.ToString(row["subcontractunitname"]);
                dgBalanceDtl[colBalanceBillingTime.Name, i].Value = ClientUtil.ToDateTime(row["createdate"]).ToShortDateString();
                dgBalanceDtl[colBalanceTaskName.Name, i].Value = ClientUtil.ToString(row["dtltaskname"]);
                dgBalanceDtl[colBalanceTaskDtlName.Name, i].Value = ClientUtil.ToString(row["balancetaskdtlname"]);
                dgBalanceDtl[colFontBillType.Name, i].Value = Enum.GetName(typeof(FrontBillType), Convert.ToInt32(row["fontbilltype"].ToString()));
                dgBalanceDtl[DtlHandlePerson.Name, i].Value = ClientUtil.ToString(row["HandlePersonName"].ToString());
                dgBalanceDtl[colBalacneQuantity.Name, i].Value = ClientUtil.ToDecimal(row["balancequantity"]);
                dgBalanceDtl[colBalancePrice.Name, i].Value = ClientUtil.ToDecimal(row["balanceprice"]);
                dgBalanceDtl[colBalanceTotalPrice.Name, i].Value = ClientUtil.ToDecimal(row["balancetotalprice"]);

                sumMoney += ClientUtil.ToDecimal(row["balancetotalprice"]);
                dgBalanceDtl[colUsedAccount.Name, i].Value = ClientUtil.ToString(row["usedescript"]);
                dgBalanceDtl[CostName.Name, i].Value = ClientUtil.ToString(row["costname"]);
                dgBalanceDtl[ResourceTypeName.Name, i].Value = ClientUtil.ToString(row["resourcetypename"]);
                dgBalanceDtl[ResourceTypeSpec.Name, i].Value = ClientUtil.ToString(row["resourcetypespec"]);
                dgBalanceDtl[BalanceSubjectName.Name, i].Value = ClientUtil.ToString(row["balancesubjectname"]);
                dgBalanceDtl[this.colSubjectCode.Name, i].Value = ClientUtil.ToString(row["balancesubjectcode"]);
            }
            this.dgBalanceDtl.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            if (dt != null && dt.Rows.Count > 0)
            {
                SetControl();
            }
          //  this.dgBalanceDtl.CellValueChanged -= dgBalanceDtl_CellValueChanged;
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < this.dgDetail.Rows.Count && e.ColumnIndex > -1 && e.ColumnIndex < dgDetail.Columns.Count)
            {
                DataGridViewColumn oColumn = dgDetail.Columns[e.ColumnIndex];
                if (oColumn == colRSubjectName)
                {
                    DataGridViewCell oCell = dgDetail[e.ColumnIndex, e.RowIndex];
                    VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                    frm.IsLeafSelect = true;
                    frm.ShowDialog();
                    CostAccountSubject cost = frm.SelectAccountSubject;
                    if (cost != null)
                    {
                        oCell.Value = cost.Name;
                        dgDetail.CurrentCell = dgDetail[colSubjectNameDtl.Name, e.RowIndex];
                    }
                }
            }
        }
        void btnRQuery_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                condition += "and t1.ProjectId = '" + projectInfo.Id + "' and t1.stockoutmanner = 20 ";
                //if (this.txtCodeBegin.Text != "")
                //{
                //    condition = condition + " and t1.Code like '%" + this.txtCodeBegin.Text + "%' ";
                //}
                if (this.txtRCode.Text != "")
                {
                    if (string.Equals(txtRCode.Text, ClientUtil.ToString(txtRCode.Tag)))
                    {
                        condition = condition + string.Format(" and t1.Code ='{0}' ", txtRCode.Text);
                    }
                    else
                    {
                        condition = condition + string.Format(" and t1.Code like '%{0}%' ", txtRCode.Text);
                    }
                }
                if (txtSupplier.Result != null && txtSupplier.Result.Count > 0)
                {
                    condition = condition + string.Format(" and t1.supplierrelation = '{0}' ", (txtSupplier.Result[0] as SupplierRelationInfo).Id);
                }
                if (dtpDateBegin.Value <= dtpDateEnd.Value)
                {
                    if (StaticMethod.IsUseSQLServer())
                    {
                        condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                    }
                    else
                    {
                        condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                    }
                }
                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
                }
                if (this.txtSpec.Text != "")
                {
                    condition = condition + " and t2.MaterialSpec like '%" + this.txtSpec.Text + "%'";
                }
                // 科目
                CostAccountSubject selectCostSub = this.txtRSubject.Tag as CostAccountSubject;
                if (this.txtRSubject.Text != "" && selectCostSub != null)
                {
                    condition += "and t2.subjectsyscode like '%" + selectCostSub.Id + "%'";

                }
                IList catResult = txtMaterialCategory.Result;
                if (catResult != null && catResult.Count > 0)
                {
                    MaterialCategory mc = catResult[0] as MaterialCategory;
                    condition = condition + " and t2.materialcode like '" + mc.Code + "%'";
                }

                if (!string.IsNullOrEmpty(txtUsedPart.Text))
                {
                    GWBSTree usedPart = txtUsedPart.Tag as GWBSTree;
                    if (usedPart != null)
                    {
                        condition += " and t2.usedpartsyscode like '%" + usedPart.Id + "%'";
                    }
                }

                #endregion

                DataSet dataSet = stockmodel.StockOutSrv.StockOutQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = ClientUtil.ToString(dataRow["code"]);
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = ClientUtil.ToString(dataRow["SupplierRelationName"]);
                    object isLimited = dataRow["IsLimited"];
                    if (isLimited == null || isLimited.ToString() == "") isLimited = "0";

                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["mxid"]);

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    dgDetail[colMaterialCode.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MaterialCode"]);
                    dgDetail[colMaterialName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MaterialName"]);
                    dgDetail[colSpec.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MaterialSpec"]);
                    dgDetail[colUsedPart.Name, rowIndex].Value = ClientUtil.ToString(dataRow["UsedPartName"]);
                    dgDetail[colSubjectNameDtl.Name, rowIndex].Value = ClientUtil.ToString(dataRow["subjectname"]);
                    dgDetail[colQuantity.Name, rowIndex].Value = dataRow["Quantity"];
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["price"];
                    dgDetail[colMoney.Name, rowIndex].Value = dataRow["Money"];

                    dgDetail[colUnit.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MatStandardUnitName"]);
                    dgDetail[colCreateDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToShortDateString();
                    dgDetail[colCreatePersonName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["CreatePersonName"]);
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"];
                    dgDetail[colDiagramNum.Name, rowIndex].Value = ClientUtil.ToString(dataRow["DiagramNumber"]);
                    dgDetail[this.colSubjectNameDtl.Name, rowIndex].Value = ClientUtil.ToString(dataRow["subjectname"]);
                }

                this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
        void btnRSelectCode_Click(object sender, EventArgs e)
        {
            VBillSubjectUpdateSelectResource oSelector = new VBillSubjectUpdateSelectResource();
            oSelector.ShowDialog();
            if (!string.IsNullOrEmpty(oSelector.BillCode))
            {
                txtRCode.Text = oSelector.BillCode;
                txtRCode.Tag = oSelector.BillCode;
            }
        }
    }
}
