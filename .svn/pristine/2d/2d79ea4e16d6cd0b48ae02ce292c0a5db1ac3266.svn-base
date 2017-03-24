using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.MobileManage.FilesUpload;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules
{
    public partial class VWeekAddDailyDetail : TBasicDataViewByMobile
    {
        MWeekPlanMng model = new MWeekPlanMng();
        InspectionRecord record = new InspectionRecord();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        private AutomaticSize automaticSize = new AutomaticSize();

        public VWeekAddDailyDetail(WeekScheduleDetail weekdetail1, InspectionRecord record1)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            record = record1;
            weekdetail = weekdetail1;
            InitEvent();
        }

        private void InitEvent()
        {
            dtpInspectionDate.Value = DateTime.Now;
            VBasicDataOptr.InitWBSCheckRequir(cbInspectionSpecial, true);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnGiveUp.Click += new EventHandler(btnGiveUp_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnFileUpLoad.Click += new EventHandler(btnFileUpLoad_Click);
            showList();
        }

        private void showList()
        {
            if (record.Id != null)
            {
                dtpInspectionDate.Text = ClientUtil.ToString(record.CreateDate);
                cbInspectionSpecial.SelectedItem = ClientUtil.ToString(record.InspectionSpecial);
                string strInspectionConclusion = ClientUtil.ToString(record.InspectionConclusion);
                if (strInspectionConclusion != null)
                {
                    if (strInspectionConclusion.Equals("检查通过"))
                    {
                        rbInspectionConclusionY.Checked = true;
                    }
                    if (strInspectionConclusion.Equals("未通过"))
                    {
                        rbInspectionConclusionN.Checked = true;
                    }
                }
                string strCorrectiveSign = ClientUtil.ToString(record.CorrectiveSign);
                if (strCorrectiveSign.Equals("0"))
                {
                    rbCorrectionCaseY.Checked = true;
                }
                if (strCorrectiveSign.Equals("1"))
                {
                    rbCorrectionCaseN.Checked = true; ;
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        private bool ViewToModel()
        {
            try
            {
                InspectionRecord newRecord = new InspectionRecord();
                if (record.Id != null)
                {
                    newRecord = record;
                }
                else
                {
                    newRecord.GWBSTree = record.GWBSTree;
                    newRecord.GWBSTreeName = record.GWBSTreeName;
                    newRecord.WeekScheduleDetail = record.WeekScheduleDetail;
                    newRecord.ProjectId = record.ProjectId;
                    newRecord.ProjectName = record.ProjectName;
                    newRecord.CorrectiveSign = record.CorrectiveSign;
                    newRecord.InspecPersonOpgSysCode = record.InspecPersonOpgSysCode;
                }
                newRecord.CreateDate = ClientUtil.ToDateTime(this.dtpInspectionDate.Text);  //检查时间
                newRecord.InspectionSpecial = ClientUtil.ToString(this.cbInspectionSpecial.SelectedItem); //检查专业
                newRecord.InspectionContent = ClientUtil.ToString(this.txtInspectionContent.Text); //检查说明
                if (this.rbInspectionConclusionY.Checked)
                {
                    newRecord.InspectionConclusion = "检查通过";
                }
                if (this.rbInspectionConclusionN.Checked)
                {
                    newRecord.InspectionConclusion = "未通过";
                }
                if (this.rbCorrectionCaseY.Checked)
                {
                    newRecord.CorrectiveSign = ClientUtil.ToInt("0");
                }
                if (this.rbCorrectionCaseN.Checked)
                {
                    newRecord.CorrectiveSign = ClientUtil.ToInt("1");
                }
                string strState = weekdetail.TaskCheckState;
                string strInspectionSpecial = ClientUtil.ToString(cbInspectionSpecial.SelectedItem);
                string strStates = "";

                if (this.rbInspectionConclusionY.Checked)//0.未检查，1.检查未通过，2.检查通过
                {
                    if (!string.IsNullOrEmpty(strState))
                    {
                        if (strInspectionSpecial.Equals("工长质检"))
                        {
                            strStates = strState.Substring(1, 6);
                            strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("质检员质检"))
                        {
                            strStates = strState.Substring(0, 1) + "2" + strState.Substring(2, 5);
                            //strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("监理质检"))
                        {
                            strStates = strState.Substring(0, 2) + "2" + strState.Substring(3, 4);
                            //strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("工程进度"))
                        {
                            strStates = strState.Substring(0, 3) + "2" + strState.Substring(4, 3);
                            //strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("安全专业"))
                        {
                            strStates = strState.Substring(0, 4) + "2" + strState.Substring(5, 2);
                            //strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("物资专业"))
                        {
                            strStates = strState.Substring(0, 5) + "2" + strState.Substring(6, 1);
                            //strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("技术专业"))
                        {
                            strStates = strState.Substring(0, 6) + "2";
                            //strStates = "2" + strStates;
                        }
                    }
                    else
                    {
                        if (strInspectionSpecial.Equals("工长质检"))
                        {
                            strStates = "2000000";
                        }
                        if (strInspectionSpecial.Equals("质检员质检"))
                        {
                            strStates = "0200000";
                            //strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("监理质检"))
                        {
                            strStates = "0020000";
                            //strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("工程进度"))
                        {
                            strStates = "0002000";
                            //strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("安全专业"))
                        {
                            strStates = "0000200";
                            //strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("物资专业"))
                        {
                            strStates = "0000020";
                            //strStates = "2" + strStates;
                        }
                        if (strInspectionSpecial.Equals("技术专业"))
                        {
                            strStates = "0000002";
                            //strStates = "2" + strStates;
                        }
                    }
                }
                else if (this.rbInspectionConclusionN.Checked)
                {
                    if (!string.IsNullOrEmpty(strState))
                    {
                        if (strInspectionSpecial.Equals("工长质检"))
                        {
                            strStates = strState.Substring(1, 6);
                            strStates = "1" + strStates;
                        }
                        if (strInspectionSpecial.Equals("质检员质检"))
                        {
                            strStates = strState.Substring(0, 1) + "1" + strState.Substring(2, 5);
                            //strStates = "1" + strStates;
                        }
                        if (strInspectionSpecial.Equals("监理质检"))
                        {
                            strStates = strState.Substring(0, 2) + "1" + strState.Substring(3, 4);
                            //strStates = "1" + strStates;
                        }
                        if (strInspectionSpecial.Equals("工程进度"))
                        {
                            strStates = strState.Substring(0, 3) + "1" + strState.Substring(4, 3);
                            //strStates = "1" + strStates;
                        }
                        if (strInspectionSpecial.Equals("安全专业"))
                        {
                            strStates = strState.Substring(0, 4) + "1" + strState.Substring(5, 2);
                            //strStates = "1" + strStates;
                        }
                        if (strInspectionSpecial.Equals("物资专业"))
                        {
                            strStates = strState.Substring(0, 5) + "1" + strState.Substring(6, 1);
                            //strStates = "1" + strStates;
                        }
                        if (strInspectionSpecial.Equals("技术专业"))
                        {
                            strStates = strState.Substring(0, 6) + "1";
                            //strStates = "1" + strStates;
                        }
                    }
                    else
                    {
                        strStates = "0000000";

                    }
                }
                this.weekdetail.TaskCheckState = strStates;
                newRecord = model.ProductionManagementSrv.SaveOrUpdateByDao(newRecord) as InspectionRecord;
                weekdetail = model.ProductionManagementSrv.SaveOrUpdateByDao(weekdetail) as WeekScheduleDetail;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }

        }


        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ViewToModel()) return;
                //bool flag = false;
                //if (string.IsNullOrEmpty(record.Id))
                //{
                //    flag = true;
                //}
                //record = model.ProductionManagementSrv.SaveOrUpdateByDao(record) as InspectionRecord;
                MessageBox.Show("保存成功！");
                this.Close();
                ClearView();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据保存错误：");
            }

        }

        public void ClearView()
        {

            this.dtpInspectionDate.Text = "";
            this.cbInspectionSpecial.SelectedItem = "";
            this.txtInspectionContent.Text = "";
            this.rbInspectionConclusionY.Checked = true;
            this.rbCorrectionCaseY.Checked = true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        void btnDelete_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (!ViewModel()) return;
                if (model.ProductionManagementSrv.DeleteByDao(record))
                {
                    MessageBox.Show("删除成功！");
                    this.Close();

                }
            }
        }

        private bool ViewModel()
        {
            try
            {
                string strState = weekdetail.TaskCheckState;
                string strInspectionSpecial = ClientUtil.ToString(cbInspectionSpecial.SelectedItem);
                string strStates = "";

                if (!string.IsNullOrEmpty(strState))//0.未检查，1.检查未通过，2.检查通过
                {
                    if (strInspectionSpecial.Equals("工长质检"))
                    {
                        strStates = strState.Substring(1, 6);
                        strStates = "0" + strStates;
                    }
                    if (strInspectionSpecial.Equals("质检员质检"))
                    {
                        strStates = strState.Substring(0, 1) + "0" + strState.Substring(2, 5);
                        //strStates = "1" + strStates;
                    }
                    if (strInspectionSpecial.Equals("监理质检"))
                    {
                        strStates = strState.Substring(0, 2) + "0" + strState.Substring(3, 4);
                        //strStates = "1" + strStates;
                    }
                    if (strInspectionSpecial.Equals("工程进度"))
                    {
                        strStates = strState.Substring(0, 3) + "0" + strState.Substring(4, 3);
                        //strStates = "1" + strStates;
                    }
                    if (strInspectionSpecial.Equals("安全专业"))
                    {
                        strStates = strState.Substring(0, 4) + "0" + strState.Substring(5, 2);
                        //strStates = "1" + strStates;
                    }
                    if (strInspectionSpecial.Equals("物资专业"))
                    {
                        strStates = strState.Substring(0, 5) + "0" + strState.Substring(6, 1);
                        //strStates = "1" + strStates;
                    }
                    if (strInspectionSpecial.Equals("技术专业"))
                    {
                        strStates = strState.Substring(0, 6) + "0";
                        //strStates = "1" + strStates;
                    }
                }
                this.weekdetail.TaskCheckState = strStates;
                //record = model.ProductionManagementSrv.DeleteByDao(record) as InspectionRecord;
                weekdetail = model.ProductionManagementSrv.SaveOrUpdateByDao(weekdetail) as WeekScheduleDetail;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        void btnFileUpLoad_Click(object sender, EventArgs e)
        {
            string fileObjectType = "工程日常检查记录";
            string objectID = ClientUtil.ToString(record.Id);
            string projectID = ClientUtil.ToString(record.ProjectId);
            string documentSort = "0101";
            string documentWorkflow = "";
            VFilesUpLoad vful = new VFilesUpLoad(fileObjectType, objectID, projectID, documentSort, documentWorkflow);
            vful.ShowDialog();
        }


        void btnGiveUp_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VWeekAddDailyDetail_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
