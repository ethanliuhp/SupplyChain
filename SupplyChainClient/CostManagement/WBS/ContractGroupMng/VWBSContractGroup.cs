using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using ImportIntegration;
using System.IO;
using System.Diagnostics;
using com.think3.PLM.Integration.Client.WS;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng
{
    public partial class VWBSContractGroup : TBasicDataView
    {
        public MWBSContractGroup model;
        
        #region 文档操作变量

        string fileObjectType = string.Empty;
        string FileStructureType = string.Empty;

        string userName = string.Empty;
        string jobId = string.Empty;

        #endregion 文档操作

        private bool isUpdate = false;//是否点击修改(契约组)
        public VWBSContractGroup(MWBSContractGroup mot)
        {
            model = mot;       
            InitializeComponent();
            InitData();
            InitForm();
        }
        private void InitData()
        {
            btnAddContractGroup.Enabled = true;
            btnSaveContractGroup.Enabled = false;
            btnUpdateContractGroup.Enabled = false;
            btnPublishContractGroup.Enabled = false;
            btnCancellationContractGroup.Enabled = false;
            btnDeleteContractGroup.Enabled = false;
        }
        private void InitForm()
        {
            InitEvents();

      
            cbContractTypeQuery.Items.Add("");

            IList list = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_ContractGroupType);
            if (list != null)
            {
                foreach (BasicDataOptr bdo in list)
                {
                    cbContractTypeQuery.Items.Add(bdo.BasicName);

                    ContractType.Items.Add(bdo.BasicName);
                }
            }
            cbContractTypeQuery.SelectedIndex = 0;

            dtStartDate.Value = DateTime.Now.AddMonths(-1);
            dtEndDate.Value = DateTime.Now;

            List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
            fileObjectType = listParams[0];
            FileStructureType = listParams[1];
            userName = "admin";
            jobId = "AAAA4DC34F5882C122C3D0FA863D";
            //userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            //jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);

            btnAddContractGroup.Click += new EventHandler(btnAddContractGroup_Click);
            btnUpdateContractGroup.Click += new EventHandler(btnUpdateContractGroup_Click);
            btnDeleteContractGroup.Click += new EventHandler(btnDeleteContractGroup_Click);
            btnSaveContractGroup.Click += new EventHandler(btnSaveContractGroup_Click);
            btnPublishContractGroup.Click += new EventHandler(btnPublishContractGroup_Click);
            btnCancellationContractGroup.Click += new EventHandler(btnCancellationContractGroup_Click);

            btnBrownDocument.Click += new EventHandler(btnBrownDocument_Click);
            btnUpdateContractDetail.Click += new EventHandler(btnUpdateContractDetail_Click);
            btnDeleteContractDetail.Click += new EventHandler(btnDeleteContractDetail_Click);
            btnSaveContractDetail.Click += new EventHandler(btnSaveContractDetail_Click);
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            //cbContractTypeQuery.SelectedIndexChanged +=new EventHandler(cbContractTypeQuery_SelectedIndexChanged);

            gridContractGroup.CellEndEdit += new DataGridViewCellEventHandler(gridContractGroup_CellEndEdit);
            gridContractGroup.CellMouseClick += new DataGridViewCellMouseEventHandler(gridContractGroup_CellMouseClick);
            gridContractGroup.CellDoubleClick += new DataGridViewCellEventHandler(gridContractGroup_CellDoubleClick);

            gridContractGroupDetail.CellEndEdit += new DataGridViewCellEventHandler(gridContractGroupDetail_CellEndEdit);

        }

        //void cbContractTypeQuery_SelectedIndexChanged(object sender,EventArgs e)
        //{
        //    string strContractType = cbContractTypeQuery.SelectedItem.ToString();
        //    if (strContractType == "分包合同" || strContractType == "分包合同补充协议" || strContractType == "分包签证")
        //    {
        //        colBearRange.Visible = true;
        //        colSettleType.Visible = true;
        //        colSingDate.Visible = true;
        //    }
        //    else
        //    {
        //        colSingDate.Visible = false;
        //        colSettleType.Visible = false;
        //        colBearRange.Visible = false;
        //    }
        //}
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.gridContractGroup, true);
        }

        void gridContractGroupDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridContractGroupDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                object value = gridContractGroupDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (value != null)
                {
                    ContractGroupDetail cg = gridContractGroupDetail.Rows[e.RowIndex].Tag as ContractGroupDetail;
                    string colName = gridContractGroupDetail.Columns[e.ColumnIndex].Name;
                    if (colName == ContractDetailDocName.Name)
                    {
                        cg.ContractDocName = value.ToString();
                    }
                    else if (colName == ContractDetailDocDesc.Name)
                    {
                        cg.ContractDocDesc = value.ToString();
                    }
                    else if (colName == ContractDetailRemark.Name)
                    {
                        cg.Remark = value.ToString();
                    }
                }
            }
        }

        void gridContractGroup_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridContractGroup.Rows[e.RowIndex].ReadOnly == false)
            {
                object value = gridContractGroup.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (value != null)
                {
                    string colName = gridContractGroup.Columns[e.ColumnIndex].Name;
                    ContractGroup cg = gridContractGroup.Rows[e.RowIndex].Tag as ContractGroup;
                    if (colName == ContractName.Name)
                    {
                        cg.ContractName = value.ToString();
                    }
                    if (colName == ContractBusinessCode.Name)
                    {
                        cg.ContractNumber = value.ToString();
                    }
                    else if (colName == ContractType.Name)
                    {
                        cg.ContractGroupType = value.ToString();
                    }
                    else if (colName == ContractDesc.Name)
                    {
                        cg.ContractDesc = value.ToString();
                    }
                }
                //string strContractType = ClientUtil.ToString(gridContractGroup.CurrentRow.Cells[ContractType.Name].Value);
                //if (strContractType != null)
                //{
                //    if (strContractType.Equals("工程签证索赔"))
                //    {
                //        colConfirmMoney.Visible = true;
                //        colSubmitMoney.Visible = true;
                //        colProjectVisa.Visible = true;
                //    }
                //    else
                //    {
                //        colConfirmMoney.Visible = false;
                //        colSubmitMoney.Visible = false;
                //        colProjectVisa.Visible = false;
                //    }
                //    if (strContractType == "分包合同" || strContractType == "分包合同补充协议" || strContractType == "分包签证")
                //    {
                //        colBearRange.Visible = true;
                //        colSettleType.Visible = true;
                //        colSingDate.Visible = true;
                //    }
                //    else
                //    {
                //        colSingDate.Visible = false;
                //        colSettleType.Visible = false;
                //        colBearRange.Visible = false;
                //    }
                //}

            }
        }

        void gridContractGroup_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                //明细保存校验
                if (gridContractGroupDetail.Rows.Count > 0)
                {
                    ContractGroup parent = gridContractGroup.Rows[e.RowIndex].Tag as ContractGroup;
                    ContractGroupDetail dtl = gridContractGroupDetail.Rows[0].Tag as ContractGroupDetail;
                    if (dtl.TheContractGroup.Id == parent.Id)//不需要校验或加载明细
                    {
                        return;
                    }
                    else
                    {
                        foreach (DataGridViewRow row in gridContractGroupDetail.Rows)
                        {
                            if (!row.ReadOnly)
                            {
                                if (MessageBox.Show("契约明细尚未保存,要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    btnSaveContractGroup_Click(btnSaveContractGroup, new EventArgs());
                                }
                                break;
                            }
                        }
                    }
                }



                ContractGroup cg = gridContractGroup.Rows[e.RowIndex].Tag as ContractGroup;
                if (string.IsNullOrEmpty(cg.Id))
                {
                    gridContractGroupDetail.Rows.Clear();
                    btnSaveContractGroup.Enabled = true;
                    return;
                }
                else
                {
                    if (!isUpdate)
                    {
                        btnSaveContractGroup.Enabled = false;
                    }
                    if (cg.State == ContractGroupState.制定)
                    {
                        btnUpdateContractGroup.Enabled = true;
                        btnDeleteContractGroup.Enabled = true;
                        btnPublishContractGroup.Enabled = true;

                        btnCancellationContractGroup.Enabled = false;
                    }
                    else if (cg.State == ContractGroupState.发布)
                    {
                        btnUpdateContractGroup.Enabled = false;
                        btnDeleteContractGroup.Enabled = false;
                        btnPublishContractGroup.Enabled = false;

                        btnCancellationContractGroup.Enabled = true;
                    }
                    else if (cg.State == ContractGroupState.作废)
                    {
                        btnUpdateContractGroup.Enabled = false;
                        btnDeleteContractGroup.Enabled = false;
                        btnPublishContractGroup.Enabled = false;
                        btnCancellationContractGroup.Enabled = false;

                        btnBrownDocument.Enabled = false;
                        btnSaveContractDetail.Enabled = false;
                        btnDeleteContractDetail.Enabled = false;
                        btnUpdateContractDetail.Enabled = false;
                    }

                    if (cg.State != ContractGroupState.作废)
                    {
                        btnBrownDocument.Enabled = true;
                        btnSaveContractDetail.Enabled = true;
                        btnDeleteContractDetail.Enabled = true;
                        btnUpdateContractDetail.Enabled = true;
                    }
                }

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", cg.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList list = model.ObjectQuery(typeof(ContractGroup), oq);

                gridContractGroupDetail.Rows.Clear();
                if (list.Count > 0)
                {
                    ContractGroup parent = list[0] as ContractGroup;
                    foreach (ContractGroupDetail dtl in parent.Details)
                    {
                        InsertToFileList(dtl, true);
                    }
                }
            }
        }

        void gridContractGroup_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                string colName = gridContractGroup.Columns[e.ColumnIndex].Name;
                ContractGroup cg = gridContractGroup.Rows[e.RowIndex].Tag as ContractGroup;
                if (colName == ChangeContract.Name)
                {
                    VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);

                    frm.DefaultSelectedContract = cg.ChangeContract;

                    frm.ShowDialog();
                    if (frm.SelectResult.Count > 0)
                    {
                        ContractGroup tempContract = frm.SelectResult[0] as ContractGroup;
                        cg.ChangeContract = tempContract;


                        gridContractGroup.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = GetContractShowText(tempContract);
                        gridContractGroup.Rows[e.RowIndex].Tag = cg;
                    }
                }
            }
        }

        private bool ValideContractGroupDelete()
        {
            try
            {
                if (gridContractGroup.SelectedRows == null || gridContractGroup.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要删除的契约组！");
                    return false;
                }
                foreach (DataGridViewRow row in gridContractGroup.SelectedRows)
                {
                    ContractGroup group = row.Tag as ContractGroup;
                    if (group.State != ContractGroupState.制定)
                    {
                        MessageBox.Show("只能删除‘制定’状态的契约组！");
                        return false;
                    }
                }

                string text = "要删除选择的契约组吗？该操作将连它的所有契约明细和关联文档一并删除！";
                if (MessageBox.Show(text, "删除契约组", MessageBoxButtons.YesNo) == DialogResult.No)
                    return false;

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    break;

                case MainViewState.Browser:

                    break;
            }
        }

        #region 契约操作按钮
        private void btnQuery_Click(object sender, EventArgs e)
        {
            btnAddContractGroup.Enabled = true;
            btnSaveContractGroup.Enabled = true;
            btnUpdateContractGroup.Enabled = true;
            btnPublishContractGroup.Enabled = true;
            btnCancellationContractGroup.Enabled = true;
            btnDeleteContractGroup.Enabled = true;


            bool flag = false;
            if (gridContractGroup.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in gridContractGroup.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    foreach (DataGridViewRow row in gridContractGroupDetail.Rows)
                    {
                        if (!row.ReadOnly)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
            if (flag)
            {
                if (MessageBox.Show("有契约组或明细尚未保存,要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnSaveContractGroup_Click(btnSaveContractGroup, new EventArgs());
                }
            }

            ObjectQuery oq = new ObjectQuery();

            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            string name = txtContractName.Text.Trim();
            //string startCode = txtStartCode.Text.Trim();
            //string endCode = txtEndCode.Text.Trim();
            DateTime startTime = dtStartDate.Value;
            DateTime endTime = dtEndDate.Value;
            string desc = txtContractDesc.Text.Trim();

            if (!string.IsNullOrEmpty(name))
                oq.AddCriterion(Expression.Like("ContractName", name, MatchMode.Anywhere));

            //if (!string.IsNullOrEmpty(startCode))
            //    oq.AddCriterion(Expression.Ge("Code", startCode));

            //if (!string.IsNullOrEmpty(endCode))
            //    oq.AddCriterion(Expression.Le("Code", endCode));

            oq.AddCriterion(Expression.Ge("CreateDate", startTime));
            oq.AddCriterion(Expression.Le("CreateDate", endTime.AddDays(1).AddSeconds(-1)));

            if (!string.IsNullOrEmpty(desc))
                oq.AddCriterion(Expression.Like("ContractDesc", desc, MatchMode.Anywhere));

            if (cbContractTypeQuery.SelectedItem != null && !string.IsNullOrEmpty(cbContractTypeQuery.SelectedItem.ToString()))
                oq.AddCriterion(Expression.Eq("ContractGroupType", cbContractTypeQuery.SelectedItem.ToString()));

            if (txtHandlePerson.Text != "" && txtHandlePerson.Result != null && txtHandlePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("CreatePersonGUID", (txtHandlePerson.Result[0] as PersonInfo).Id));
            }
            //oq.AddCriterion(Expression.Eq("CreatePersonGUID", txtHandlePerson.Tag));

            oq.AddFetchMode("ChangeContract", NHibernate.FetchMode.Eager);
            oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateDate"));
            IList list = model.ObjectQuery(typeof(ContractGroup), oq);

            gridContractGroup.Rows.Clear();
            foreach (ContractGroup cg in list)
            {
                InsertContractInGrid(cg, true, false, false);
            }

            gridContractGroupDetail.Rows.Clear();
        }

        private void btnAddContractGroup_Click(object sender, EventArgs e)
        {
            btnSaveContractGroup.Enabled = true;
            btnUpdateContractGroup.Enabled = false;
            btnPublishContractGroup.Enabled = false;
            btnCancellationContractGroup.Enabled = false;
            btnDeleteContractGroup.Enabled = false;
            //明细校验
            if (gridContractGroupDetail.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowDtl in gridContractGroupDetail.Rows)
                {
                    if (!rowDtl.ReadOnly)
                    {
                        if (MessageBox.Show("契约文档尚未保存,要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            btnSaveContractDetail_Click(btnSaveContractDetail, new EventArgs());
                        }
                        break;
                    }
                }
            }
            gridContractGroupDetail.Rows.Clear();

            ContractGroup cg = new ContractGroup();
            cg.Code = model.GetContractGroupCode();//获得单据号
            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                cg.ProjectId = projectInfo.Id;
                cg.ProjectName = projectInfo.Name;
            }
            cg.CreatePersonGUID = ConstObject.LoginPersonInfo.Id;
            cg.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            cg.CreatePersonSysCode = ConstObject.TheOperationOrg.SysCode;
            cg.State = ContractGroupState.制定;
            //cg = model.SaveOrUpdateContractGroup(cg);

            ////写操作日志
            //LogData log = new LogData();
            //log.BillId = cg.Id;
            //log.BillType = "契约";
            //log.Code = cg.Code;
            //log.OperType = "新增";
            //log.Descript = "";
            //log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
            //log.ProjectName = cg.ProjectName;
            //Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);

            InsertContractInGrid(cg, false, false, true);

            gridContractGroup.BeginEdit(false);
        }

        private void InsertContractInGrid(ContractGroup cg, bool isReadOnly, bool isSelected, bool setCurrentCell)
        {
            int index = gridContractGroup.Rows.Add();
            DataGridViewRow row = gridContractGroup.Rows[index];
            row.Cells[ContractName.Name].Value = cg.ContractName;
            row.Cells[ContractCode.Name].Value = cg.Code;
            row.Cells[ContractBusinessCode.Name].Value = cg.ContractNumber;
            if (cg.ChangeContract != null)
                row.Cells[ChangeContract.Name].Value = GetContractShowText(cg.ChangeContract);
            row.Cells[ContractVersion.Name].Value = cg.ContractVersion;
            row.Cells[ContractType.Name].Value = cg.ContractGroupType;
            row.Cells[ContractState.Name].Value = cg.State.ToString();
            row.Cells[ContractCreatePerson.Name].Value = cg.CreatePersonName;
            row.Cells[ContractCreateDate.Name].Value = cg.CreateDate.ToString();
            row.Cells[ContractDesc.Name].Value = cg.ContractDesc;
            row.Cells[colSubmitMoney.Name].Value = cg.SubmitMoney;
            row.Cells[colProjectVisa.Name].Value = cg.ProjectVisa;
            row.Cells[colConfirmMoney.Name].Value = cg.ConfirmMoney;
            row.Cells[colBearRange.Name].Value = cg.BearRange;
            row.Cells[colSettleType.Name].Value = cg.SettleType;
            //if (cg.SingDate < ClientUtil.ToDateTime("1900-1-1"))
            //{
            //    row.Cells[colSingDate.Name].Value = ClientUtil.ToDateTime("1900-1-1");
            //}
            //else
            //{
            //    row.Cells[colSingDate.Name].Value = cg.SingDate;
            //}
            if (cg.SingDate > ClientUtil.ToDateTime("1900-01-01"))
           {
                row.Cells[colSingDate.Name].Value = cg.SingDate;
            }

            row.Tag = cg;
            row.ReadOnly = isReadOnly;
            row.Selected = isSelected;
            if (setCurrentCell)
                gridContractGroup.CurrentCell = row.Cells[ContractName.Name];
        }

        private void btnUpdateContractGroup_Click(object sender, EventArgs e)
        {
            btnAddContractGroup.Enabled = true;
            btnDeleteContractGroup.Enabled = true;
            btnSaveContractGroup.Enabled = true;
            btnCancellationContractGroup.Enabled = false;
            btnPublishContractGroup.Enabled = false;
            if (gridContractGroup.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的行！");
                return;
            }
            if (gridContractGroup.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }
            ContractGroup cg = gridContractGroup.SelectedRows[0].Tag as ContractGroup;
            if (cg.State != ContractGroupState.制定)
            {
                MessageBox.Show("请选择状态为‘制定’的契约进行修改！");
                return;
            }
            isUpdate = true;
            btnSaveContractGroup.Enabled = true;
            btnPublishContractGroup.Enabled = true;

            gridContractGroup.CurrentCell = gridContractGroup.SelectedRows[0].Cells[ContractName.Name];
            gridContractGroup.SelectedRows[0].ReadOnly = false;
            gridContractGroup.SelectedRows[0].Selected = false;
            gridContractGroup.BeginEdit(false);
        }

        private void btnDeleteContractGroup_Click(object sender, EventArgs e)
        {
            btnAddContractGroup.Enabled = true;
            btnCancellationContractGroup.Enabled = true;
            btnUpdateContractGroup.Enabled = true;
            btnPublishContractGroup.Enabled = true;
            btnSaveContractGroup.Enabled = false;
            if (gridContractGroup.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的行！");
                return;
            }
            try
            {

                IList list = new List<ContractGroup>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridContractGroup.SelectedRows)
                {
                    ContractGroup cg = row.Tag as ContractGroup;
                    if (cg.State == ContractGroupState.制定)
                    {
                        if (!string.IsNullOrEmpty(cg.Id))
                        {
                            list.Add(cg);
                        }
                        listRowIndex.Add(row.Index);
                    }
                }

                if (listRowIndex.Count == 0)
                {
                    MessageBox.Show("选择中没有符合删除的行，请选择状态为‘制定’契约！");
                    return;
                }

                if (MessageBox.Show("删除后不能恢复，您确认要删除选择的契约吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                if (list.Count > 0)
                {
                    
                    ObjectQuery oq = new ObjectQuery();
                    Disjunction dis = new Disjunction();
                    foreach (ContractGroup cg in list)
                    {
                        dis.Add(Expression.Eq("TheContractGroup.Id", cg.Id));
                    }
                    oq.AddCriterion(dis);
                    IList listContractDtl = model.ObjectQuery(typeof(ContractGroupDetail), oq);

                    List<string> listDocId = new List<string>();
                    foreach (ContractGroupDetail dtl in listContractDtl)
                    {
                        if (!string.IsNullOrEmpty(dtl.ContractDocGUID))
                            listDocId.Add(dtl.ContractDocGUID);
                    }

                    if (listDocId.Count > 0)
                    {
                        //删除契约文档
                        PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DeleteDocumentByCustom(listDocId.ToArray()
                            , "1", null, userName, jobId, null);
                        if (es != null)
                        {
                            MessageBox.Show("删除文档时出错，错误信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    //删除契约
                    model.DeleteContractGroup(list);

                    foreach (ContractGroup cg in list)
                    {
                        LogData log = new LogData();
                        log.BillId = cg.Id;
                        log.BillType = "契约";
                        log.Code = cg.Code;
                        log.OperType = "删除";
                        log.Descript = "";
                        log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = cg.ProjectName;
                        Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);
                    }
                }


                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridContractGroup.Rows.RemoveAt(listRowIndex[i]);
                }

                gridContractGroupDetail.Rows.Clear();

                gridContractGroup.ClearSelection();

                MessageBox.Show("删除成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void btnSaveContractGroup_Click(object sender, EventArgs e)
        {
            btnAddContractGroup.Enabled = true;
            btnUpdateContractGroup.Enabled = true;
            btnDeleteContractGroup.Enabled = true;
            btnPublishContractGroup.Enabled = true;
            btnCancellationContractGroup.Enabled = true;
            try
            {
                foreach (DataGridViewRow var in this.gridContractGroup.Rows)
                {
                    if (!var.ReadOnly)
                    {
                        if (var.Cells[ContractName.Name].Value == null)
                        {
                            MessageBox.Show("契约名称不能为空！");
                            gridContractGroup.CurrentCell = var.Cells[ContractName.Name];
                            return;
                        }
                        if (var.Cells[ContractType.Name].Value == null)
                        {
                            MessageBox.Show("契约类型不能为空！");
                            gridContractGroup.CurrentCell = var.Cells[ContractType.Name];
                            return;
                        }
                    }
                }

                IList listParent = new ArrayList();
                foreach (DataGridViewRow row in gridContractGroup.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        ContractGroup tempParent = row.Tag as ContractGroup;

                        //ContractGroup parent = model.GetObjectById(typeof(ContractGroup), tempParent.Id) as ContractGroup;
                        //parent.ContractName = tempParent.ContractName;
                        //parent.ContractNumber = tempParent.ContractNumber;
                        //parent.ContractGroupType = tempParent.ContractGroupType;
                        //parent.ChangeContract = tempParent.ChangeContract;
                        //parent.ContractDesc = tempParent.ContractDesc;
                        tempParent.ProjectVisa = ClientUtil.ToDecimal(row.Cells[colProjectVisa.Name].Value);
                        tempParent.ConfirmMoney = ClientUtil.ToDecimal(row.Cells[colConfirmMoney.Name].Value);
                        tempParent.SubmitMoney = ClientUtil.ToDecimal(row.Cells[colSubmitMoney.Name].Value);
                        tempParent.SingDate = ClientUtil.ToDateTime(row.Cells[colSingDate.Name].Value);
                        tempParent.SettleType = ClientUtil.ToString(row.Cells[colSettleType.Name].Value);
                        tempParent.BearRange = ClientUtil.ToString(row.Cells[colBearRange.Name].Value);

                        string strUnit = "天";
                        Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Name", strUnit));
                        IList lists = model.LaborSporadicSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                        if (lists != null && lists.Count > 0)
                        {
                            Unit = lists[0] as StandardUnit;
                        }
                        tempParent.ProjectUnitName = strUnit;
                        tempParent.ProjectUnit = Unit as StandardUnit;

                        string strUnitPrice = "元";
                        Application.Resource.MaterialResource.Domain.StandardUnit UnitPrice = null;
                        ObjectQuery objectquery = new ObjectQuery();
                        objectquery.AddCriterion(Expression.Eq("Name", strUnitPrice));
                        IList list = model.LaborSporadicSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), objectquery);
                        if (list != null && list.Count > 0)
                        {
                            UnitPrice = list[0] as StandardUnit;
                        }
                        tempParent.PriceUnitName = strUnitPrice;
                        tempParent.PriceUnit = UnitPrice as StandardUnit;
                        listParent.Add(tempParent);
                    }
                }

                listParent = model.SaveOrUpdateContractGroup(listParent);
                if (listParent == null || listParent.Count <= 0) return;
                ContractGroup grop = listParent[0] as ContractGroup;

                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridContractGroupDetail.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        listRowIndex.Add(row.Index);
                    }
                }

                //上传文档
                if (listRowIndex.Count > 0)
                {

                    for (int i = 0; i < listRowIndex.Count; i++)
                    {
                        int rowIndex = listRowIndex[i];
                        DataGridViewRow row = gridContractGroupDetail.Rows[rowIndex];
                        if (row.Cells[ContractDetailDocName.Name].Value == null)
                        {
                            MessageBox.Show("契约文档名称不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            gridContractGroupDetail.CurrentCell = row.Cells[ContractDetailDocName.Name];
                            row.ReadOnly = false;
                            row.Selected = false;
                            gridContractGroupDetail.BeginEdit(false);

                            return;
                        }
                    }

                    int rowCount = listRowIndex.Count;

                    progressBarDocUpload.Minimum = 0;
                    progressBarDocUpload.Maximum = rowCount >= 10 ? rowCount + 1 : 11;
                    progressBarDocUpload.Value = 1;

                    //使用单个上传模式
                    List<byte[]> listFileBytes = new List<byte[]>();
                    List<string> listFileNames = new List<string>();
                    List<PLMWebServices.DictionaryObjectInfo> listDicKeyValue = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo>();

                    for (int i = 0; i < listRowIndex.Count; i++)
                    {
                        int rowIndex = listRowIndex[i];
                        DataGridViewRow row = gridContractGroupDetail.Rows[rowIndex];

                        ContractGroupDetail dtl = row.Tag as ContractGroupDetail;
                        //ContractGroup grop = gridContractGroup.SelectedRows[0].Tag as ContractGroup;
                        dtl.TheContractGroup = grop;

                        listFileBytes.Clear();
                        listFileNames.Clear();
                        listDicKeyValue.Clear();

                        string filePath = dtl.ContractDocURL;
                        if (string.IsNullOrEmpty(dtl.Id) && !string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                        {
                            FileInfo file = new FileInfo(filePath);

                            #region 上传文件

                            FileStream fileStream = file.OpenRead();
                            int FileLen = (int)file.Length;
                            Byte[] FileData = new Byte[FileLen];
                            //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                            fileStream.Read(FileData, 0, FileLen);

                            listFileBytes.Add(FileData);


                            string fileName = dtl.ContractDocName;
                            listFileNames.Add(fileName + Path.GetExtension(file.Name));

                            object fileDesc = dtl.ContractDocDesc;

                            List<string> listNames = new List<string>();
                            List<object> listValues = new List<object>();

                            //listNames.Add("Code");
                            //listValues.Add(fileName);

                            //listNames.Add("DOCUMENTNUMBER");
                            //listValues.Add(fileName);

                            listNames.Add("Name");
                            listValues.Add(fileName);

                            listNames.Add("DOCUMENTTITLE");
                            listValues.Add(fileName);

                            listNames.Add("DOCUMENTDESCRIPTION");
                            listValues.Add(fileDesc);

                            PLMWebServices.DictionaryObjectInfo dic = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo();
                            dic.InfoNames = listNames.ToArray();
                            dic.InfoValues = listValues.ToArray();

                            listDicKeyValue.Add(dic);


                            string[] listFileIds = null;
                            PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.AddDocumentByCustom(out listFileIds, listFileBytes.ToArray(),
                                listFileNames.ToArray(), fileObjectType, "1", listDicKeyValue.ToArray(), null, userName, jobId, null);

                            if (es != null)
                            {
                                MessageBox.Show("文件“" + fileName + "”上传到服务器失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            #endregion 上传文件

                            #region 更新对象关联文档信息
                            if (listFileIds != null)
                            {
                                string fileId = listFileIds[0];
                                dtl.ContractDocGUID = fileId;
                                dtl.ContractDocURL = string.Empty;
                            }
                            #endregion
                        }

                        IList listDtl = new ArrayList();
                        listDtl.Add(dtl);
                        listDtl = model.SaveOrUpdateContractGroup(listDtl);

                        dtl = listDtl[0] as ContractGroupDetail;

                        row.Tag = dtl;
                        row.ReadOnly = true;


                        if (rowCount < 10)
                            progressBarDocUpload.Value += (int)Math.Floor((decimal)10 / rowCount);
                        else
                            progressBarDocUpload.Value += 1;

                    }
                }

                progressBarDocUpload.Value = progressBarDocUpload.Maximum;

                foreach (ContractGroup parent in listParent)
                {

                    LogData log = new LogData();
                    log.BillId = parent.Id;
                    log.BillType = "契约";
                    log.Code = parent.Code;
                    if (parent.Version == 0)
                    {
                        log.OperType = "新增";
                        log.Descript = "";
                    }
                    else
                    {
                        log.OperType = "修改";
                        log.Descript = "更新契约文档";
                    }
                    log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = parent.ProjectName;
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);

                    bool flag = false;
                    foreach (DataGridViewRow row in gridContractGroup.Rows)
                    {
                        ContractGroup cg = row.Tag as ContractGroup;
                        if (cg.Id != null)
                        {
                            if (cg.Id == parent.Id)
                            {
                                row.Tag = parent;
                                row.ReadOnly = true;
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (!flag)
                    {
                        InsertContractInGrid(parent,true,true,true);
                    }
                }
                List<DataGridViewRow> rowList = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in gridContractGroup.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        rowList.Add(row);
                    }
                }
                //indexList.Reverse();
                foreach(DataGridViewRow row in rowList)
                {
                    gridContractGroup.Rows.Remove(row);
                }
                MessageBox.Show("保存成功！");

                btnSaveContractGroup.Enabled = false;
                btnCancellationContractGroup.Enabled = false;

                isUpdate = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }

            progressBarDocUpload.Value = 0;
        }
        //原来的保存契约
        private void saveContractMaster()
        {
            try
            {
                ContractGroup parent = null;
                if (gridContractGroupDetail.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in gridContractGroupDetail.Rows)
                    {
                        if (!row.ReadOnly)
                        {
                            ContractGroupDetail dtl = row.Tag as ContractGroupDetail;
                            if (parent == null)
                            {
                                parent = dtl.TheContractGroup;

                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                                IList listParent = model.ObjectQuery(typeof(ContractGroup), oq);
                                if (listParent.Count > 0)
                                    parent = listParent[0] as ContractGroup;

                                //设置父对象修改的属性


                                if (string.IsNullOrEmpty(dtl.Id))
                                {
                                    parent.Details.Add(dtl);
                                }
                                else
                                {
                                    foreach (ContractGroupDetail d in parent.Details)
                                    {
                                        if (d.Id == dtl.Id)
                                        {
                                            d.ContractDocName = dtl.ContractDocName;


                                            d.ContractDocDesc = dtl.ContractDocDesc;
                                            d.Remark = dtl.Remark;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(dtl.Id))
                                {
                                    parent.Details.Add(dtl);
                                }
                                else
                                {
                                    foreach (ContractGroupDetail d in parent.Details)
                                    {
                                        if (d.Id == dtl.Id)
                                        {
                                            d.ContractDocName = dtl.ContractDocName;
                                            d.ContractDocDesc = dtl.ContractDocDesc;
                                            d.Remark = dtl.Remark;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                IList list = new List<ContractGroup>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridContractGroup.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        ContractGroup cg = row.Tag as ContractGroup;
                        if (!string.IsNullOrEmpty(cg.Id))
                        {
                            if (parent != null && cg.Id == parent.Id)
                            {
                                parent.ContractName = cg.ContractName;
                                parent.ContractGroupType = cg.ContractGroupType;
                                parent.ContractDesc = cg.ContractDesc;


                                parent.ContractNumber = cg.ContractNumber;
                                parent.ChangeContract = cg.ChangeContract;

                                cg = parent;
                            }
                            else
                            {
                                ContractGroup temp = model.GetObjectById(typeof(ContractGroup), cg.Id) as ContractGroup;
                                temp.ContractName = cg.ContractName;
                                temp.ContractGroupType = cg.ContractGroupType;
                                temp.ContractDesc = cg.ContractDesc;


                                temp.ContractNumber = cg.ContractNumber;
                                temp.ChangeContract = cg.ChangeContract;
                                cg = temp;
                            }
                        }
                        list.Add(cg);

                        listRowIndex.Add(row.Index);
                    }
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("当前没有要保存的数据！");
                    return;
                }


                list = model.SaveOrUpdateContractGroup(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];

                    ContractGroup cg = list[i] as ContractGroup;

                    //写操作日志
                    LogData log = new LogData();
                    log.BillId = cg.Id;
                    log.BillType = "契约";
                    log.Code = cg.Code;
                    log.OperType = "修改";
                    log.Descript = "";
                    log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = cg.ProjectName;



                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);


                    gridContractGroup.Rows[rowIndex].Tag = cg;
                    gridContractGroup.Rows[rowIndex].ReadOnly = true;
                }

                foreach (DataGridViewRow row in gridContractGroupDetail.Rows)
                {
                    row.ReadOnly = true;
                }

                MessageBox.Show("保存成功！");

            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void btnPublishContractGroup_Click(object sender, EventArgs e)
        {
            btnAddContractGroup.Enabled = true;
            btnUpdateContractGroup.Enabled = true;
            btnSaveContractGroup.Enabled = true;


            btnCancellationContractGroup.Enabled = true;
            btnDeleteContractGroup.Enabled = true;
            if (gridContractGroup.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要发布的行！");
                return;
            }
            IList list = new List<ContractGroup>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridContractGroup.SelectedRows)
            {
                ContractGroup cg = row.Tag as ContractGroup;
                if (!string.IsNullOrEmpty(cg.Id) && cg.State == ContractGroupState.制定)
                {
                    ContractGroup temp = model.GetObjectById(typeof(ContractGroup), cg.Id) as ContractGroup;
                    temp.ContractGroupType = cg.ContractGroupType;
                    temp.ContractDesc = cg.ContractDesc;
                    temp.ContractNumber = cg.ContractNumber;
                    temp.ChangeContract = cg.ChangeContract;


                    temp.State = ContractGroupState.发布;
                    temp.ContractVersion = model.GetServerTime().ToString();




                    list.Add(temp);

                    listRowIndex.Add(row.Index);
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("选择中没有需要发布的契约组，请选择状态为‘制定’的契约组！");
                return;
            }

            try
            {
                list = model.SaveOrUpdateContractGroup(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridContractGroup.Rows[rowIndex].Tag = list[i];
                    gridContractGroup.Rows[rowIndex].Cells[ContractState.Name].Value = (list[i] as ContractGroup).State.ToString();


                    gridContractGroup.Rows[rowIndex].Cells[ContractVersion.Name].Value = (list[i] as ContractGroup).ContractVersion;
                    gridContractGroup.Rows[rowIndex].ReadOnly = true;
                }

                MessageBox.Show("发布成功！");
                btnSaveContractGroup.Enabled = false;
                btnDeleteContractGroup.Enabled = false;
                btnUpdateContractGroup.Enabled = false;
                btnPublishContractGroup.Enabled = false;
                isUpdate = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发布失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void btnCancellationContractGroup_Click(object sender, EventArgs e)
        {
            btnAddContractGroup.Enabled = true;
            btnUpdateContractGroup.Enabled = true;
            btnSaveContractGroup.Enabled = true;
            btnPublishContractGroup.Enabled = true;
            btnDeleteContractGroup.Enabled = true;
            if (gridContractGroup.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要作废的行！");
                return;
            }
            IList list = new List<ContractGroup>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridContractGroup.SelectedRows)
            {
                ContractGroup cg = row.Tag as ContractGroup;
                if (!string.IsNullOrEmpty(cg.Id) && cg.State == ContractGroupState.发布)
                {
                    ContractGroup temp = model.GetObjectById(typeof(ContractGroup), cg.Id) as ContractGroup;
                    temp.ContractGroupType = cg.ContractGroupType;


                    temp.ContractDesc = cg.ContractDesc;
                    temp.ContractNumber = cg.ContractNumber;


                    temp.ChangeContract = cg.ChangeContract;
                    temp.State = ContractGroupState.作废;

                    list.Add(temp);

                    listRowIndex.Add(row.Index);
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("选择中没有符合作废的契约组，请选择状态为‘发布’的契约组！");
                return;
            }

            try
            {
                list = model.SaveOrUpdateContractGroup(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];

                    gridContractGroup.Rows[rowIndex].Tag = list[i];
                    gridContractGroup.Rows[rowIndex].Cells["ContractState"].Value = (list[i] as ContractGroup).State.ToString();
                    gridContractGroup.Rows[rowIndex].ReadOnly = true;
                }

                MessageBox.Show("作废设置成功！");

                btnSaveContractGroup.Enabled = false;
                btnUpdateContractGroup.Enabled = false;
                btnDeleteContractGroup.Enabled = false;
                btnPublishContractGroup.Enabled = false;
                btnCancellationContractGroup.Enabled = false;

                btnBrownDocument.Enabled = false;
                btnSaveContractDetail.Enabled = false;
                btnDeleteContractDetail.Enabled = false;
                btnUpdateContractDetail.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("作废设置失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        #endregion

        #region 契约文档操作

        //新增
        private void btnAddContractDetail_Click(object sender, EventArgs e)
        {
            if (gridContractGroup.SelectedRows.Count == 0 || gridContractGroup.SelectedRows.Count > 1)
            {
                MessageBox.Show("请选择一条所属契约！");
                return;
            }

            ContractGroup parent = gridContractGroup.SelectedRows[0].Tag as ContractGroup;
            if (parent.State != ContractGroupState.制定)
            {
                MessageBox.Show("该契约状态为‘" + parent.State + "’,请选择状态为‘制定’的契约添加文档！");
                return;
            }

            ContractGroupDetail cg = new ContractGroupDetail();
            cg.Code = model.GetContractGroupDetailCode(parent.Code, gridContractGroupDetail.Rows.Count + 1);
            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                cg.ProjectId = projectInfo.Id;
                cg.ProjectName = projectInfo.Name;
            }
            cg.CreatePersonGUID = ConstObject.LoginPersonInfo.Id;
            cg.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            cg.CreatePersonSysCode = ConstObject.TheOperationOrg.SysCode;
            cg.TheContractGroup = parent;

            int index = gridContractGroupDetail.Rows.Add();
            DataGridViewRow row = gridContractGroupDetail.Rows[index];
            row.Cells["ContractDetailCode"].Value = cg.Code;
            row.Cells["ContractDetailName"].Value = cg.Name;
            row.Cells["ContractDetailType"].Value = cg.ContractType;
            row.Cells["ContractDetailOwner"].Value = cg.CreatePersonName;
            row.Cells["ContractDetailCreateDate"].Value = cg.CreateTime.ToString();
            row.Cells["ContractDetailRemark"].Value = cg.Remark;
            row.Tag = cg;

            gridContractGroupDetail.CurrentCell = row.Cells["ContractDetailName"];
            gridContractGroupDetail.BeginEdit(false);
        }
        //浏览选择文档
        void btnBrownDocument_Click(object sender, EventArgs e)
        {
            if (gridContractGroup.SelectedRows.Count == 0 || gridContractGroup.SelectedRows.Count > 1)
            {
                MessageBox.Show("请选择一条所属契约！");
                return;
            }

            ContractGroup parent = gridContractGroup.SelectedRows[0].Tag as ContractGroup;
            if (parent.State != ContractGroupState.制定 && parent.State != ContractGroupState.发布)
            {
                MessageBox.Show("该契约状态为‘" + parent.State + "’,请选择状态为‘制定’或‘发布’的契约添加文档！");
                return;
            }

            //选择文件
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "所有文件(*.*)|*.*";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] strFiles = openFileDialog1.FileNames;
                int iCount = strFiles.Length;
                for (int i = 0; i < iCount; i++)
                {
                    string filePath = strFiles[i];
                    ContractGroupDetail cg = new ContractGroupDetail();
                    cg.Code = model.GetContractGroupDetailCode(parent.Code, gridContractGroupDetail.Rows.Count + 1);
                    CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        cg.ProjectId = projectInfo.Id;
                        cg.ProjectName = projectInfo.Name;
                    }
                    cg.CreatePersonGUID = ConstObject.LoginPersonInfo.Id;
                    cg.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                    cg.CreatePersonSysCode = ConstObject.TheOperationOrg.SysCode;
                    cg.TheContractGroup = parent;

                    string fileName = Path.GetFileName(filePath);
                    cg.ContractDocName = fileName.Substring(0, fileName.IndexOf("."));
                    cg.ContractDocURL = filePath;

                    InsertToFileList(cg, false);
                }
            }
        }

        private void InsertToFileList(ContractGroupDetail cg, bool isReadOnly)
        {
            int index = gridContractGroupDetail.Rows.Add();
            DataGridViewRow row = gridContractGroupDetail.Rows[index];
            row.Cells[ContractDetailDocName.Name].Value = cg.ContractDocName;
            row.Cells[ContractDetailDocDesc.Name].Value = cg.ContractDocDesc;
            row.Cells[ContractDetailRemark.Name].Value = cg.Remark;
            row.Cells[ContractDetailURL.Name].Value = cg.ContractDocURL;
            row.Cells[ContractDetailOwner.Name].Value = cg.CreatePersonName;

            row.Cells[ContractDetailCreateDate.Name].Value = cg.CreateTime.ToString();

            row.Tag = cg;

            row.ReadOnly = isReadOnly;

            gridContractGroupDetail.CurrentCell = row.Cells[ContractDetailDocName.Name];
            gridContractGroupDetail.BeginEdit(false);
        }

        private void UpdateDocument(ContractGroupDetail cg)
        {
            foreach (DataGridViewRow row in gridContractGroupDetail.Rows)
            {
                ContractGroupDetail docTemp = row.Tag as ContractGroupDetail;
                if (docTemp.Id == cg.Id)
                {
                    row.Cells[ContractDetailDocName.Name].Value = cg.ContractDocName;
                    row.Cells[ContractDetailDocDesc.Name].Value = cg.ContractDocDesc;
                    row.Cells[ContractDetailRemark.Name].Value = cg.Remark;
                    row.Cells[ContractDetailURL.Name].Value = cg.ContractDocURL;
                    row.Cells[ContractDetailOwner.Name].Value = cg.CreatePersonName;

                    row.Cells[ContractDetailCreateDate.Name].Value = cg.CreateTime.ToString();

                    row.Tag = cg;

                    gridContractGroupDetail.CurrentCell = row.Cells[1];
                    break;
                }
            }
        }

        //修改
        private void btnUpdateContractDetail_Click(object sender, EventArgs e)
        {
            if (gridContractGroupDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的行！");
                return;
            }
            if (gridContractGroupDetail.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }
            ContractGroupDetail dtl = gridContractGroupDetail.SelectedRows[0].Tag as ContractGroupDetail;
            //foreach (DataGridViewRow row in gridContractGroup.SelectedRows)
            //{
            //    ContractGroup cg = row.Tag as ContractGroup;
            //    if (dtl.TheContractGroup.Id == cg.Id && cg.State != ContractGroupState.制定)
            //    {
            //        MessageBox.Show("此文档不允许修改，只能修改状态为‘制定’契约下的文档！");
            //        return;
            //    }
            //}

            gridContractGroupDetail.CurrentCell = gridContractGroupDetail.SelectedRows[0].Cells[ContractDetailDocName.Name];
            gridContractGroupDetail.SelectedRows[0].ReadOnly = false;
            gridContractGroupDetail.SelectedRows[0].Selected = false;
            gridContractGroupDetail.BeginEdit(false);
        }
        //保存
        private void btnSaveContractDetail_Click(object sender, EventArgs e)
        {
            try
            {
                //foreach (DataGridViewRow var in this.gridContractGroup.Rows)
                //{
                //    if (!var.ReadOnly)
                //    {
                //        if (var.Cells[ContractName.Name].Value == null)
                //        {
                //            MessageBox.Show("契约名称不能为空！");
                //            gridContractGroup.CurrentCell = var.Cells[ContractName.Name];
                //            return;
                //        }
                //        if (var.Cells[ContractType.Name].Value == null)
                //        {
                //            MessageBox.Show("契约类型不能为空！");
                //            gridContractGroup.CurrentCell = var.Cells[ContractType.Name];
                //            return;
                //        }
                //    }
                //}

                ContractGroup tempParent = null;

                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridContractGroupDetail.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        if (tempParent == null)
                        {
                            ContractGroupDetail dtl = row.Tag as ContractGroupDetail;
                            tempParent = dtl.TheContractGroup;
                        }
                        listRowIndex.Add(row.Index);
                    }
                }

                if (tempParent == null)
                {
                    MessageBox.Show("当前没有要保存的数据！");
                    return;
                }

                ContractGroup parent = null;
                if (tempParent.Id == null)
                {
                    if (gridContractGroup.SelectedRows.Count > 2 || gridContractGroup.SelectedRows.Count <= 0)
                    {
                        MessageBox.Show("请选择一条契约组！");
                        return;
                    }
                    MessageBox.Show("当前契约组还未保存，请先保存契约组！");
                    return;
                    //parent = gridContractGroup.SelectedRows[0].Tag as ContractGroup;
                    //IList list = new ArrayList();
                    //list.Add(parent);
                    //parent = model.SaveOrUpdateContractGroup(list)[0] as ContractGroup;
                }
                //上传文档
                if (listRowIndex.Count > 0)
                {

                    for (int i = 0; i < listRowIndex.Count; i++)
                    {
                        int rowIndex = listRowIndex[i];
                        DataGridViewRow row = gridContractGroupDetail.Rows[rowIndex];
                        if (row.Cells[ContractDetailDocName.Name].Value == null)
                        {
                            MessageBox.Show("契约文档名称不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            gridContractGroupDetail.CurrentCell = row.Cells[ContractDetailDocName.Name];
                            row.ReadOnly = false;
                            row.Selected = false;
                            gridContractGroupDetail.BeginEdit(false);

                            return;
                        }
                    }

                    int rowCount = listRowIndex.Count;

                    progressBarDocUpload.Minimum = 0;
                    progressBarDocUpload.Maximum = rowCount >= 10 ? rowCount + 1 : 11;
                    progressBarDocUpload.Value = 1;

                    //显示进度条，使用单个上传模式
                    for (int i = 0; i < listRowIndex.Count; i++)
                    {
                        int rowIndex = listRowIndex[i];
                        DataGridViewRow row = gridContractGroupDetail.Rows[rowIndex];

                        ContractGroupDetail dtl = row.Tag as ContractGroupDetail;
                        //dtl.TheContractGroup = parent;

                        string filePath = dtl.ContractDocURL;
                        if (string.IsNullOrEmpty(dtl.Id) && !string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                        {
                            #region
                            //IList listDoc = new List<ProObjectRelaDocument>();
                            List<PLMWebServices.ProjectDocument> listDocument = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();

                            PLMWebServices.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();

                            FileInfo file = new FileInfo(filePath);
                            FileStream fileStream = file.OpenRead();
                            int FileLen = (int)file.Length;
                            Byte[] FileData = new Byte[FileLen];
                            ////将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                            fileStream.Read(FileData, 0, FileLen);
                            if (FileData.Length == 0)
                            {
                                MessageBox.Show("该文件长度为0,请检查!");
                                return;
                            }
                            doc.ExtendName = Path.GetExtension(filePath); //文档扩展名*******************************
                            doc.FileDataByte = FileData; //文件二进制流
                            doc.FileName = file.Name;//文件名称

                            CurrentProjectInfo projectInfo = new CurrentProjectInfo();
                            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                            doc.ProjectCode = projectInfo.Code; //所属项目代码*
                            doc.ProjectName = projectInfo.Name; //所属项目名称*

                            doc.Category = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.CategoryNode();
                            doc.Category.CategoryCode = "";//"CSFL";//文档分类代码
                            doc.Category.CategoryName = "";//"测试分类"; //文档分类名称


                            List<string> listDocParam = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
                            string docObjTypeName = listDocParam[0];
                            string docCateLinkTypeName = listDocParam[2];

                            doc.CategoryRelaDocType = docCateLinkTypeName;//文档分类类型
                            doc.ObjectTypeName = docObjTypeName;//文档对象类型

                            doc.State = Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentState.编制;//文档状态
                            doc.DocType = Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentInfoType.文本;//文档信息类型

                            doc.Name = dtl.ContractDocName;//文档名称
                            doc.Description = dtl.ContractDocDesc;//文档说明
                            doc.Title = dtl.ContractDocName;//文档标题
                            doc.Code = "";
                            //doc.CategorySysCode = "";//文档分类层次码
                            //doc.Code = txtDocumentCode.Text;//文档代码
                            //doc.KeyWords = txtDocumentKeywords.Text;//文档关键字
                            //doc.Author = txtDocumentAuthor.Text;//文档作者
                            //string docObjTypeName= StaticMethod.GetMBPUploadFileParams()[0];
                            //doc.OwnerID = "";//责任人
                            //doc.OwnerName = "";//责任人名称
                            //doc.OwnerOrgSysCode = "";// 责任人组织层次码
                            //doc.Revision = "";//文档版次
                            //doc.Version = "";//文档版本
                            //doc.ExtendInfoNames = "";//扩展属性名
                            //doc.ExtendInfoValues = "";//扩展属性值

                            listDocument.Add(doc);
                            PLMWebServices.ProjectDocument[] resultList = null;
                            PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.AddDocumentByIRP(listDocument.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentSaveMode.一个文件生成一个文档对象, null, userName, jobId, null, out resultList);
                            if (es != null)
                            {
                                MessageBox.Show("文件“" + file.Name + "”上传到服务器失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            #endregion

                            #region 更新对象关联文档信息
                            if (resultList != null)
                            {
                                ///string fileId = resultList[0];
                                dtl.ContractDocGUID = resultList[0].EntityID;
                                dtl.ContractDocURL = string.Empty;
                            }
                            #endregion
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(dtl.ContractDocGUID))
                            {
                                PLMWebServices.ProjectDocument[] resultDoc = null;
                                List<string> fileIds = new List<string>();
                                fileIds.Add(dtl.ContractDocGUID);

                                PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectDocumentByIRP(fileIds.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentQueryVersion.最新版本,
                                    null, userName, jobId, null, out resultDoc);
                                if (es != null)
                                {
                                    MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                PLMWebServices.ProjectDocument pd = resultDoc[0];
                                pd.Title = pd.Name = dtl.ContractDocName;
                                pd.Description = dtl.ContractDocDesc;
                                PLMWebServices.ProjectDocument[] resultUpdateDocList = null;
                                List<PLMWebServices.ProjectDocument> updateDocList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
                                updateDocList.Add(pd);
                                PLMWebServices.ErrorStack es1 = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.UpdateDocumentByIRP(updateDocList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentUpdateMode.添加一个新版次文件,
                                    null, userName, jobId, null, out resultUpdateDocList);
                                if (es1 != null)
                                {
                                    MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es1), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                        IList listDtl = new ArrayList();
                        listDtl.Add(dtl);
                        listDtl = model.SaveOrUpdateContractGroup(listDtl);

                        dtl = listDtl[0] as ContractGroupDetail;

                        row.Tag = dtl;
                        row.ReadOnly = true;


                        if (rowCount < 10)
                            progressBarDocUpload.Value += (int)Math.Floor((decimal)10 / rowCount);
                        else
                            progressBarDocUpload.Value += 1;

                    }
                }

                progressBarDocUpload.Value = progressBarDocUpload.Maximum;

                //ContractGroup parent = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", tempParent.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                IList listParent = model.ObjectQuery(typeof(ContractGroup), oq);
                parent = listParent[0] as ContractGroup;
                parent.ContractName = tempParent.ContractName;
                parent.ContractNumber = tempParent.ContractNumber;
                parent.ContractGroupType = tempParent.ContractGroupType;
                parent.ChangeContract = tempParent.ChangeContract;
                parent.ContractDesc = tempParent.ContractDesc;

                parent = model.SaveOrUpdateContractGroup(parent);


                LogData log = new LogData();
                log.BillId = parent.Id;
                log.BillType = "契约";
                log.Code = parent.Code;
                log.OperType = "修改";
                log.Descript = "更新契约文档";
                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                log.ProjectName = parent.ProjectName;
                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);


                foreach (DataGridViewRow row in gridContractGroup.Rows)
                {
                    ContractGroup cg = row.Tag as ContractGroup;
                    if (cg.Id == parent.Id)
                    {
                        row.Tag = parent;
                        row.ReadOnly = true;

                        break;
                    }
                }

                gridContractGroupDetail.Rows.Clear();
                if (parent.Details != null && parent.Details.Count > 0)
                {
                    foreach (ContractGroupDetail dtl in parent.Details)
                    {
                        InsertToFileList(dtl, true);
                    }
                }


                MessageBox.Show("保存成功！");



            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }


            progressBarDocUpload.Value = 0;

        }
        //删除
        private void btnDeleteContractDetail_Click(object sender, EventArgs e)
        {
            if (gridContractGroupDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的行！");
                return;
            }
            try
            {

                IList list = new List<ContractGroupDetail>();
                List<int> listRowIndex = new List<int>();
                List<string> listDocId = new List<string>();

                foreach (DataGridViewRow row in gridContractGroupDetail.SelectedRows)
                {
                    ContractGroupDetail cg = row.Tag as ContractGroupDetail;
                    if (!string.IsNullOrEmpty(cg.Id))
                    {
                        list.Add(cg);
                        if (!string.IsNullOrEmpty(cg.ContractDocGUID))
                            listDocId.Add(cg.ContractDocGUID);
                    }
                    listRowIndex.Add(row.Index);
                }

                if (list.Count > 0)
                {
                    //foreach (DataGridViewRow rowGroup in gridContractGroup.Rows)
                    //{
                    //    ContractGroup parent = rowGroup.Tag as ContractGroup;
                    //    if ((list[0] as ContractGroupDetail).TheContractGroup.Id == parent.Id && parent.State != ContractGroupState.制定 && parent.State != ContractGroupState.发布)
                    //    {
                    //        MessageBox.Show("此契约文档不允许删除，只能删除状态为‘制定’或‘发布’的契约下的文档！");
                    //        return;
                    //    }
                    //}
                }
                else
                {
                    listRowIndex.Sort();
                    for (int i = listRowIndex.Count - 1; i > -1; i--)
                    {
                        gridContractGroupDetail.Rows.RemoveAt(listRowIndex[i]);
                    }

                    gridContractGroupDetail.ClearSelection();

                    return;
                }

                if (MessageBox.Show("删除后不能恢复，您确认要删除选择的契约文档吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                if (listDocId.Count > 0)
                {
                    List<string> docIds = listDocId;//new List<string>();
                    List<PLMWebServices.ProjectDocument> proDocList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
                    PLMWebServices.ProjectDocument[] reultProdocList = null;
                    PLMWebServices.ProjectDocument[] resultProDoc = null;

                    PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectDocumentByIRP(docIds.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentQueryVersion.所有版本,
                        null, userName, jobId, null, out resultProDoc);
                    if (es != null)
                    {
                        MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    for (int i = 0; i < resultProDoc.Length; i++)
                    {
                        PLMWebServices.ProjectDocument doc = resultProDoc[i];
                        doc.State = Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentState.作废;
                        proDocList.Add(doc);
                    }

                    PLMWebServices.ErrorStack es1 = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.UpdateDocumentByIRP(proDocList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentUpdateMode.添加一个新版次文件,
                        null, userName, jobId, null, out reultProdocList);
                    if (es1 != null)
                    {
                        MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ////删除IRP文档信息
                    //PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DeleteDocumentByCustom(listDocId.ToArray()
                    //    , "1", null, userName, jobId, null);
                    //if (es != null)
                    //{

                    //    MessageBox.Show("删除文档时出错，错误信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}
                }

                model.DeleteContractGroupDetail(list);

                ContractGroupDetail cgDtl = list[0] as ContractGroupDetail;
                LogData log = new LogData();
                log.BillId = cgDtl.TheContractGroup.Id;
                log.BillType = "契约";
                log.Code = cgDtl.TheContractGroup.Code;
                log.OperType = "修改";
                log.Descript = "删除契约文档";
                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                log.ProjectName = cgDtl.TheContractGroup.ProjectName;
                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridContractGroupDetail.Rows.RemoveAt(listRowIndex[i]);
                }

                gridContractGroupDetail.ClearSelection();

                MessageBox.Show("删除成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //下载文档
        void btnDownLoadDocument_Click(object sender, EventArgs e)
        {
            if (gridContractGroupDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要下载的契约文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridContractGroupDetail.Focus();
                return;
            }

            //List<string> listFileIds = new List<string>();
            //List<PLMWebServices.ProjectDocument> listDocument = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
            //PLMWebServices.ProjectDocument[] listResult = null;
            IList listDocument = new ArrayList();
            foreach (DataGridViewRow row in gridContractGroupDetail.SelectedRows)
            {
                ContractGroupDetail doc = row.Tag as ContractGroupDetail;
                PLMWebServices.ProjectDocument proDoc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
                proDoc.EntityID = doc.ContractDocGUID;
                proDoc.Name = doc.ContractDocName;
                listDocument.Add(proDoc);
            }
            VDocumentDownload vdd = new VDocumentDownload(listDocument, "IRP");
            vdd.ShowDialog();

            #region 注释
            //try
            //{
            //    PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByIRP(listDocument.ToArray(), null, userName, jobId, null, out listResult);
            //    if (es != null)
            //    {
            //        MessageBox.Show("文件下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    if (listResult != null)
            //    {
            //        string selectedDir = string.Empty;
            //        for (int i = 0; i < listResult.Length; i++)
            //        {
            //            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //            saveFileDialog1.Filter = "All files(*.*)|*.*";
            //            saveFileDialog1.RestoreDirectory = true;
            //            saveFileDialog1.FileName = listResult[i].Code + listResult[i].FileName;

            //            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //            {
            //                //进行赋值 
            //                string filename = saveFileDialog1.FileName;
            //                string strName = Path.GetFileName(filename);
            //                if (listResult[i].FileDataByte != null)
            //                {
            //                    CreateFileFromByteAarray(listResult[i].FileDataByte, filename);

            //                    selectedDir = saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.LastIndexOf("\\"));
            //                }
            //                else
            //                {
            //                    MessageBox.Show("下载的文件：" + strName + " 为空,无法下载！");
            //                }
            //                //CreateFileFromByteAarray(listResult[i].FileDataByte, filename);

            //                //selectedDir = saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.LastIndexOf("\\"));
            //            }
            //        }
            //        if (Directory.Exists(selectedDir))
            //        {
            //            Process.Start(selectedDir);
            //        }
            //    }

            #region 注释

            //           object[] listFileBytes = null;
            //           string[] listFileNames = null;

            //           PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByCustom(out listFileBytes, out listFileNames, listFileIds.ToArray(),
            //null, userName, jobId, null);

            //           if (es != null)
            //           {
            //               MessageBox.Show("文件下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //               return;
            //           }

            //           if (listFileBytes != null)
            //           {
            //               string selectedDir = string.Empty;
            //               for (int i = 0; i < listFileBytes.Length; i++)
            //               {
            //                   byte[] by = listFileBytes[i] as byte[];
            //                   if (by != null && by.Length > 0)
            //                   {
            //                       SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            //                       saveFileDialog1.Filter = "All files(*.*)|*.*";
            //                       saveFileDialog1.RestoreDirectory = true;
            //                       saveFileDialog1.FileName = listFileNames[i];

            //                       if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //                       {
            //                           //进行赋值 
            //                           string filename = saveFileDialog1.FileName;
            //                           CreateFileFromByteAarray(by, filename);

            //                           selectedDir = saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.LastIndexOf("\\"));
            //                       }
            //                   }
            //               }
            //               if (Directory.Exists(selectedDir))
            //               {
            //                   Process.Start(selectedDir);
            //               }
            //           }
            #endregion
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message.IndexOf("未将对象引用设置到对象的实例") > -1)
            //    {
            //        MessageBox.Show("下载失败，不存在的文档对象！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    else
            //        MessageBox.Show("下载失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            #endregion
        }
        public static void CreateFileFromByteAarray(byte[] stream, string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fs.Write(stream, 0, stream.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetExceptionMessage(PLMWebServices.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServices.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "；\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if (msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1)
            {
                msg = "已存在同名文档，请重命名该文档名称.";
            }

            return msg;
        }
        //预览
        void btnOpenDocument_Click(object sender, EventArgs e)
        {

            if (gridContractGroupDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要打开的契约文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridContractGroupDetail.Focus();
                return;
            }

            List<PLMWebServices.ProjectDocument> listDocument = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
            PLMWebServices.ProjectDocument[] listResult = null;

            foreach (DataGridViewRow row in gridContractGroupDetail.SelectedRows)
            {
                ContractGroupDetail doc = row.Tag as ContractGroupDetail;
                PLMWebServices.ProjectDocument proDoc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
                proDoc.EntityID = doc.ContractDocGUID;
                listDocument.Add(proDoc);
            }

            try
            {
                PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByIRP(listDocument.ToArray(), null, userName, jobId, null, out listResult);
                if (es != null)
                {
                    MessageBox.Show("文件下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<string> errorList = new List<string>();
                List<string> listFileFullPaths = new List<string>();
                if (listResult != null)
                {
                    string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                    if (!Directory.Exists(fileFullPath))
                        Directory.CreateDirectory(fileFullPath);

                    for (int i = 0; i < listResult.Length; i++)
                    {
                        string fileName = listResult[i].FileName;

                        if (listResult[i].FileDataByte == null || listResult[i].FileDataByte.Length <= 0 || fileName == null)
                        {
                            string strName = listResult[i].Code + listResult[i].Name;
                            errorList.Add(strName);
                            continue;
                        }

                        string tempFileFullPath = fileFullPath + @"\\" + fileName;

                        CreateFileFromByteAarray(listResult[i].FileDataByte, tempFileFullPath);

                        listFileFullPaths.Add(tempFileFullPath);
                    }
                }
                foreach (string fileFullPath in listFileFullPaths)
                {
                    FileInfo file = new FileInfo(fileFullPath);

                    //定义一个ProcessStartInfo实例
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    //设置启动进程的初始目录
                    info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
                    //设置启动进程的应用程序或文档名
                    info.FileName = file.Name;
                    //设置启动进程的参数
                    info.Arguments = "";
                    //启动由包含进程启动信息的进程资源
                    try
                    {
                        System.Diagnostics.Process.Start(info);
                    }
                    catch (System.ComponentModel.Win32Exception we)
                    {
                        MessageBox.Show(this, we.Message);
                    }
                }
                if (errorList != null && errorList.Count > 0)
                {
                    string str = "";
                    foreach (string s in errorList)
                    {
                        str += (s + ";");
                    }
                    MessageBox.Show(str + "这" + errorList.Count + "个文件，无法预览，文件不存在或未指定格式！");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("未将对象引用设置到对象的实例") > -1)
                {
                    MessageBox.Show("操作失败，不存在的文档对象！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion 文档操作

        private string GetContractShowText(ContractGroup item)
        {
            string text = "";
            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.ContractName) && !string.IsNullOrEmpty(item.ContractNumber))
                {
                    text = item.ContractName + "_" + item.ContractNumber;
                }
                else if (!string.IsNullOrEmpty(item.ContractName))
                {
                    text = item.ContractName;
                }
                else if (!string.IsNullOrEmpty(item.ContractNumber))
                {
                    text = item.ContractNumber;
                }
            }
            return text;
        }
    }
}
