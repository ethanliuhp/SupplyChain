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
using Application.Business.Erp.SupplyChain.Client.Basic;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.IO;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng
{
    public partial class VAcceptance : TBasicDataView
    {
        public MAcceptanceInspectionMng model = new MAcceptanceInspectionMng();
        private AcceptanceInspection acceptanceInspection;

        public VAcceptance()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            VBasicDataOptr.InitWBSCheckRequir(cbCheckRequir, true);
            cbCheckConclusion.Items.AddRange(new object[] { "检查通过", "罚款后检查通过", "检查不通过" });
            colDocumentType.Items.AddRange(new object[] { "照片", "视频" });
            RefreshControls(MainViewState.Browser);
            txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
            dtpCheckDate.Value = ConstObject.TheLogin.LoginDate;
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            //检查记录
            btnAdd.Click += new EventHandler(btnAddInspectionRecord_Click);
            btnUpdate.Click += new EventHandler(btnModifyInspectionRecord_Click);
            btnDelete.Click += new EventHandler(btnDeleteInspectionRecord_Click);
            btnSave.Click += new EventHandler(btnSaveInspectionRecord_Click);
            dgDetail.SelectionChanged += new EventHandler(dgDetail_SelectionChanged);
            dgDetail.CellClick += new DataGridViewCellEventHandler(dgDetail_CellClick);
        }

        #region 检查记录操作事件及方法
        void dgDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            AcceptanceInspection master = dgDetail.CurrentRow.Tag as AcceptanceInspection;
            master = model.AcceptanceInspectionSrv.GetAcceptanceInspectionById(master.Id);

            if (tabControl1.SelectedTab.Name.Equals("验收检查记录明细信息"))
            {
                if (dgDetail.Rows.Count > 0 && dgDetail.CurrentRow != null)
                {
                    this.EditInspectionRecord();
                }
            }

            else if (tabControl1.SelectedTab.Name.Equals("相关文件信息"))
            {
                //if (dgDetail.Rows.Count > 0 && dgDetail.CurrentRow != null)
                //{
                //    if (dgSitePictureVideo.Rows.Count > 0)
                //        dgSitePictureVideo.Rows.Clear();
                //    if (master != null)
                //    {
                //        foreach (SitePictureVideo theSitePictureVideo in master.SitePictureVideos)
                //        {
                //            int rowIndex = dgSitePictureVideo.Rows.Add();
                //            dgSitePictureVideo[colDocuName.Name, rowIndex].Value = theSitePictureVideo.DocumentName;
                //            dgSitePictureVideo[colDocuURL.Name, rowIndex].Value = theSitePictureVideo.DocumentURL;
                //            dgSitePictureVideo[colDocumentType.Name, rowIndex].Value = theSitePictureVideo.Type;
                //            dgSitePictureVideo[colDocuContent.Name, rowIndex].Value = theSitePictureVideo.ContentNotes;
                //            dgSitePictureVideo[colDocuDate.Name, rowIndex].Value = theSitePictureVideo.ShootingDate;
                //            dgSitePictureVideo[colPerson.Name, rowIndex].Tag = theSitePictureVideo.ShootingPerson;
                //            dgSitePictureVideo[colPerson.Name, rowIndex].Value = theSitePictureVideo.ShootingPersonName;
                //            dgSitePictureVideo[colState.Name, rowIndex].Value = 1;
                //            dgSitePictureVideo.Rows[rowIndex].Tag = theSitePictureVideo;
                //        }
                //    }
                //}
            }
        }
        void btnSaveInspectionRecord_Click(object sender, EventArgs e)
        {
            //校验数据
            if (cbCheckRequir.Text == "")
            {
                MessageBox.Show("请选择检查专业！");
                return;
            }
            if (txtCheckPerson.Text == "")
            {
                MessageBox.Show("请选择检查人！");
                return;
            }
            if (cbCheckConclusion.Text == "")
            {
                MessageBox.Show("请选择检查结论！");
                return;
            }
            this.SaveInspectionRecord();
        }
        /// <summary>
        /// 删除检查记录单和该检查记录单下的整改通知和现场照片和视频文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDeleteInspectionRecord_Click(object sender, EventArgs e)
        {
            //删除检查记录
            if (dgDetail.Rows.Count > 0 && dgDetail.CurrentRow != null)
            {
                try
                {
                    DataGridViewRow theCurrentRow = dgDetail.CurrentRow;
                    AcceptanceInspection master = theCurrentRow.Tag as AcceptanceInspection;

                    if (MessageBox.Show("是否要删除当前检查记录？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    if (master.Id != null)
                    {
                        master = model.AcceptanceInspectionSrv.GetAcceptanceInspectionById(master.Id);
                        model.AcceptanceInspectionSrv.DeleteByDao(master);
                        dgDetail.Rows.Remove(theCurrentRow);
                    }
                    else
                    {
                        //MessageBox.Show("当前检查记录未保存，是否先保存？");
                        dgDetail.Rows.Remove(theCurrentRow);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除数据出错" + ex.ToString());
                }

            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnModifyInspectionRecord_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count > 0 && dgDetail.SelectedRows != null)
            {
                if (tabControl1.SelectedTab.Name.Equals("验收检查记录明细信息"))
                {
                    Clear();
                    //txtInspectionRecord.Tag = dgDetail.CurrentRow.Tag;
                    //txtCode.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colCode.Name].Value);
                    //txtCheckPerson.Value = dgDetail.CurrentRow.Cells[colCheckPerson.Name].Value;
                    //txtCheckPerson.Tag = dgDetail.CurrentRow.Cells[colCheckPerson.Name].Tag;
                    //cbCheckConclusion.SelectedItem = dgDetail.CurrentRow.Cells[colCheckConclusion.Name].Value;
                    //cbCheckRequir.SelectedItem = dgDetail.CurrentRow.Cells[colCheckSpecial.Name].Value;
                    //txtContent.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colContent.Name].Value);
                    //txtCheckStatus.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colCheckStatus.Name].Value);
                    dtpCheckDate.Value = DateTime.Now;
                    this.EditInspectionRecord();
                    txtCode.Focus();
                    RefreshControls(MainViewState.Modify);
                }
            }
        }
        //新增检查记录(根据工程项目任务新增:检查专业不同)
        void btnAddInspectionRecord_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count <= 0 && dgDetail.SelectedRows == null)
            {
                MessageBox.Show("请先选择工程任务明细");
                return;
            }
            else
            {
                //当前检查记录生成不同检查专业的检查记录
                AcceptanceInspection Record = dgDetail.Rows[0].Tag as AcceptanceInspection;
                if (Record != null)
                {
                    int i = dgDetail.Rows.Add();
                    AcceptanceInspection InspectionRecord = new AcceptanceInspection();
                    DataGridViewRow newRow = dgDetail.Rows[i];
                    dgDetail[colCheckPerson.Name, i].Value = ConstObject.LoginPersonInfo.Name;
                    dgDetail[colCheckPerson.Name, i].Tag = ConstObject.LoginPersonInfo;
                    dgDetail.Rows[i].Tag = InspectionRecord;
                    txtInspectionRecord.Tag = InspectionRecord;
                }
            }
        }

        void dgDetail_SelectionChanged(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                AcceptanceInspection master = dgDetail.CurrentRow.Tag as AcceptanceInspection;
                if (master != null)
                    master = model.AcceptanceInspectionSrv.GetAcceptanceInspectionById(master.Id);


                if (tabControl1.SelectedTab.Name.Equals("验收检查记录明细信息"))
                {
                    if (dgDetail.Rows.Count > 0 && dgDetail.CurrentRow != null)
                    {
                        this.EditInspectionRecord();
                    }
                }
            }
        }

        /// <summary>
        /// 在编辑框显示当前检查记录详细信息
        /// </summary>
        private void EditInspectionRecord()
        {
            if (tabControl1.SelectedTab.Name.Equals("验收检查记录明细信息"))
            {
                DataGridViewRow theCurrentRow = dgDetail.CurrentRow;
                Clear();
                if (theCurrentRow.Tag != null)
                {
                    AcceptanceInspection master = theCurrentRow.Tag as AcceptanceInspection;
                    txtInspectionRecord.Tag = master;
                    
                    txtCode.Text = master.Code;
                    if (master.InspectionPerson != null)
                    {
                        txtCheckPerson.Result.Clear();
                        txtCheckPerson.Result.Add(master.InspectionPerson);
                        txtCheckPerson.Text = master.InspectionPersonName;
                    }
                    cbCheckRequir.Text = master.InspectionSpecial;
                    cbCheckConclusion.Text = master.InspectionConclusion;
                    dtpCheckDate.Value = master.InspectionDate;
                    txtContent.Text = master.InspectionContent;
                    txtCheckStatus.Text = master.InspectionStatus;
                    txtCheckPerson.Result.Clear();
                    txtCheckPerson.Result.Add(dgDetail.CurrentRow.Cells[colCheckPerson.Name].Tag);
                    txtCheckPerson.Value = dgDetail.CurrentRow.Cells[colCheckPerson.Name].Value;
                    dtpCheckDate.Value = DateTime.Now;
                }
            }
        }
        /// <summary>
        /// 保存更新检查记录
        /// </summary>
        /// <returns></returns>
        private void SaveInspectionRecord()
        {
            try
            {
                AcceptanceInspection master = txtInspectionRecord.Tag as AcceptanceInspection;
                if (master.Id == null)
                    master = new AcceptanceInspection();
                master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                master.HandlePerson = ConstObject.LoginPersonInfo;
                master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                master.ProjectId = projectInfo.Id;
                master.ProjectName = projectInfo.Name;
                //检查人
                if (txtCheckPerson.Text != "")
                {
                    PersonInfo CheckPerson = txtCheckPerson.Tag as PersonInfo;
                    master.InspectionPerson = txtCheckPerson.Tag as PersonInfo;
                    master.InspectionPersonName = txtCheckPerson.Text;
                    //master.InspecPersonOpgSysCode=CheckPerson.
                }
                master.InsLotGUID = txtInspectionLotSCode.Tag as InspectionLot;
                master.InspectionSpecial = cbCheckRequir.Text;//检查专业
                master.InspectionConclusion = cbCheckConclusion.Text;//检查结论
                master.InspectionContent = txtContent.Text;//检查内容说明
                master.InspectionStatus = txtCheckStatus.Text;//检查结论
                master.InspectionDate = dtpCheckDate.Value.Date;
                string strSpecial = ClientUtil.ToString(cbCheckRequir.SelectedItem);//获得检查专业
                string strConclusion = ClientUtil.ToString(cbCheckConclusion.Text);
                string strState = ClientUtil.ToString((txtInspectionLotSCode.Tag as InspectionLot).AccountStatus);//获得检验批的验收状态
                if (strSpecial.Equals("工长质检"))
                {
                    if (strConclusion.Equals("检查通过") || strConclusion.Equals("罚款后检查通过"))
                    {
                        strState = "1" + strState.Substring(1, 6);
                    }
                    if (strConclusion.Equals("检查不通过"))
                    {
                        strState = "X" + strState.Substring(1, 6);
                    }
                }
                if (strSpecial.Equals("质检员质检"))
                {
                    if (strConclusion.Equals("检查通过") || strConclusion.Equals("罚款后检查通过"))
                    {
                        strState = strState.Substring(0, 1) + "1" + strState.Substring(2, 5);
                    }
                    if (strConclusion.Equals("检查不通过"))
                    {
                        strState = strState.Substring(0, 1) + "X" + strState.Substring(2, 5);
                    }
                }
                if (strSpecial.Equals("工程进度"))
                {
                    if (strConclusion.Equals("检查通过") || strConclusion.Equals("罚款后检查通过"))
                    {
                        strState = strState.Substring(0, 2) + "1" + strState.Substring(3, 4);
                    }
                    if (strConclusion.Equals("检查不通过"))
                    {
                        strState = strState.Substring(0, 2) + "X" + strState.Substring(3, 4);
                    }
                }
                if (strSpecial.Equals("安全专业"))
                {
                    if (strConclusion.Equals("检查通过") || strConclusion.Equals("罚款后检查通过"))
                    {
                        strState = strState.Substring(0, 3) + "1" + strState.Substring(4, 3);
                    }
                    if (strConclusion.Equals("检查不通过"))
                    {
                        strState = strState.Substring(0, 3) + "X" + strState.Substring(4, 3);
                    }
                }
                if (strSpecial.Equals("物资专业"))
                {
                    if (strConclusion.Equals("检查通过") || strConclusion.Equals("罚款后检查通过"))
                    {
                        strState = strState.Substring(0, 4) + "1" + strState.Substring(5, 2);
                    }
                    if (strConclusion.Equals("检查不通过"))
                    {
                        strState = strState.Substring(0, 4) + "X" + strState.Substring(5, 2);
                    }
                }
                if (strSpecial.Equals("技术专业"))
                {
                    if (strConclusion.Equals("检查通过") || strConclusion.Equals("罚款后检查通过"))
                    {
                        strState = strState.Substring(0, 5) + "1" + strState.Substring(6, 1);
                    }
                    if (strConclusion.Equals("检查不通过"))
                    {
                        strState = strState.Substring(0, 5) + "X" + strState.Substring(6, 1);
                    }
                }
                if (strSpecial.Equals("监理质检"))
                {
                    if (strConclusion.Equals("检查通过") || strConclusion.Equals("罚款后检查通过"))
                    {
                        strState = strState.Substring(0, 6) + "1";
                    }
                    if (strConclusion.Equals("检查不通过"))
                    {
                        strState = strState.Substring(0, 6) + "X";
                    }
                }
                master.InsTemp = strState;
                master = model.AcceptanceInspectionSrv.SaveAcceptanceInspection(master) as AcceptanceInspection;
                UpdateCurrentRow(master);
                MessageBox.Show("保存成功！");
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据错误" + ex.ToString());
                return;
            }
        }
        private void UpdateCurrentRow(AcceptanceInspection obj)
        {
            txtInspectionRecord.Tag = null;
            txtInspectionRecord.Tag = obj;
            DataGridViewRow theCurrentRow = dgDetail.CurrentRow;
            txtCode.Text = obj.Code;
            theCurrentRow.Cells[colCode.Name].Value = obj.Code;
            theCurrentRow.Cells[colCheckSpecial.Name].Value = obj.InspectionSpecial;
            theCurrentRow.Cells[colCheckConclusion.Name].Value = obj.InspectionConclusion;
            theCurrentRow.Cells[colContent.Name].Value = obj.InspectionContent;
            theCurrentRow.Cells[colCheckStatus.Name].Value = obj.InspectionStatus;
            theCurrentRow.Cells[colDeductionSign.Name].Value = obj.DeductionSign;
            theCurrentRow.Cells[colCorrectiveSign.Name].Value = obj.CorrectiveSign;
            if (obj.InspectionPerson != null)
            {
                theCurrentRow.Cells[colCheckPerson.Name].Tag = obj.InspectionPerson;
                theCurrentRow.Cells[colCheckPerson.Name].Value = obj.InspectionPersonName;
            }
            theCurrentRow.Tag = obj;
        }

      
        #endregion

        void btnSearch_Click(object sender, EventArgs e)
        {
            VInspectionLotSelect vss = new VInspectionLotSelect();
            vss.ShowDialog();
            IList list = vss.Result;
            if (list == null || list.Count == 0) return;
            dgDetail.Rows.Clear();
            foreach (InspectionLot detail in list)
            {
                txtInspectionLotSCode.Text = detail.Code;
                txtInspectionLotSCode.Tag = detail;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("InsLotGUID.Id", detail.Id));
                IList temp_list = model.AcceptanceInspectionSrv.GetAcceptanceInspection(oq);
                if (temp_list.Count > 0)
                {
                    foreach (AcceptanceInspection Record in temp_list)
                    {
                        int rowIndex = dgDetail.Rows.Add();
                        dgDetail[colCode.Name, rowIndex].Tag = Record.Code;

                        dgDetail[colCode.Name, rowIndex].Value = Record.Code;
                        dgDetail[colCheckSpecial.Name, rowIndex].Value = Record.InspectionSpecial;
                        dgDetail[colCheckConclusion.Name, rowIndex].Value = Record.InspectionConclusion;
                        dgDetail[colContent.Name, rowIndex].Value = Record.InspectionContent;
                        dgDetail[colCheckStatus.Name, rowIndex].Value = Record.InspectionStatus;
                        dgDetail[colCheckPerson.Name, rowIndex].Value = Record.InspectionPersonName;
                        dgDetail[colCheckPerson.Name, rowIndex].Tag = Record.InspectionPerson;
                        dgDetail.Rows[rowIndex].Tag = Record;
                    }
                }
                else
                {
                    int rowIndex = dgDetail.Rows.Add();
                    //构造检查记录
                    AcceptanceInspection record = new AcceptanceInspection();
                    record.InspectionPerson = detail.HandlePerson;
                    record.InspectionPersonName = detail.HandlePersonName;
                    dgDetail[colCheckPerson.Name, rowIndex].Value = detail.HandlePersonName;
                    dgDetail[colCheckPerson.Name, rowIndex].Tag = detail.HandlePerson;
                    dgDetail.Rows[rowIndex].Tag = record;
                }
            }
        }


        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgDetail_SelectionChanged(sender, new EventArgs());
        }

        private void Clear()
        {
            txtCode.Tag = null;
            txtCode.Text = "";
            txtContent.Text = "";
            dtpCheckDate.Text = "";
            txtContent.Text = "";
            txtCheckStatus.Text = "";
            txtCheckPerson.Result.Clear();
            txtCheckPerson.Tag = null;
            txtCheckPerson.Text = "";
            cbCheckConclusion.Text = "";
            cbCheckRequir.Text = "";
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:
                    //按钮
                    btnSaveDocument.Enabled = true;
                    btnSave.Enabled = true;
                    btnInventory.Enabled = false;
                    //界面输入控件
                    txtCode.Enabled = true;
                    txtCheckPerson.Enabled = true;
                    cbCheckRequir.Enabled = true;
                    cbCheckConclusion.Enabled = true;
                    dtpCheckDate.Enabled = true;
                    txtContent.Enabled = true;
                    txtCheckStatus.Enabled = true;
                    break;
                case MainViewState.Browser:
                    //按钮
                    btnSaveDocument.Enabled = false;
                    btnSave.Enabled = false;
                    btnInventory.Enabled = true;
                    //界面输入控件
                    txtCode.Enabled = false;
                    txtCheckPerson.Enabled = false;
                    cbCheckRequir.Enabled = false;
                    cbCheckConclusion.Enabled = false;
                    dtpCheckDate.Enabled = false;
                    txtContent.Enabled = false;
                    txtCheckStatus.Enabled = false;
                    break;
            }
        }
    }
}
