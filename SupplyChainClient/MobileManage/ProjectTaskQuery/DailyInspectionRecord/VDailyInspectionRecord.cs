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
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.DailyInspectionRecord
{
    public partial class VDailyInspectionRecord : TBasicToolBarByMobile
    {
        MProjectTaskQuery model = new MProjectTaskQuery();
        private AutomaticSize automaticSize = new AutomaticSize();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        InspectionRecord record = new InspectionRecord();
        IList list = new ArrayList();
        IList lists = new ArrayList();
        private int pageIndex = 0;
        private int i = 0;

        public VDailyInspectionRecord(IList list, int pageIndex)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            this.list = list;
            this.pageIndex = pageIndex;
            InitData();
            InitEvent();
            Contents();
        }

        public void InitEvent()
        {
            //VBasicDataOptr.InitWBSCheckRequir(cbInspectionSpecial, true);
            cbInspectionSpecial.Items.Clear();
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
            foreach (BasicDataOptr b in list)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = b.BasicName;
                li.Value = b.BasicCode;
                cbInspectionSpecial.Items.Add(li);
            }
            cbInspectionConclusion.Items.AddRange(new object[] { "通过", "不通过" });
            cbCorrectiveSign.Items.AddRange(new object[] { "未整改", "无需整改"});
            btnTheFirst.Click += new EventHandler(btnTheFirst_Click);
            btnBack.Click += new EventHandler(btnBack_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
            btnTheLast.Click += new EventHandler(btnTheLast_Click);
        }

        public void Contents()
        {
            this.功能菜单1Item.Visible = true;
            this.功能菜单1Item.Text = "放弃编辑";
            this.功能菜单2Item.Visible = true;
            this.功能菜单2Item.Text = "提交记录";
            this.功能菜单3Item.Visible = true;
            this.功能菜单3Item.Text = "删除记录";
            this.功能菜单4Item.Visible = true;
            this.功能菜单4Item.Text = "保存记录";
            this.功能菜单5Item.Visible = true;
            this.功能菜单5Item.Text = "新增记录";
        }

        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
            VDailyInspectionResult v_dresult = new VDailyInspectionResult(list, pageIndex, lists, i);
            v_dresult.ShowDialog();
            i = v_dresult.i;

            if (weekdetail != null)
            {
                record = lists[i - 1] as InspectionRecord;
            }
            if (weekdetail == null)
            {
                record = list[i - 1] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            }
            
            txtBearTeamName.Text = ClientUtil.ToString(record.BearTeamName);
            dtpInspectionDate.Value = record.CreateDate;
            cbInspectionSpecial.SelectedItem = ClientUtil.ToString(record.InspectionSpecial);
            string strInspectionConslusion = ClientUtil.ToString(record.InspectionConclusion);
            if (strInspectionConslusion.Equals("通过"))
            {
                cbInspectionConclusion.SelectedItem = strInspectionConslusion;
            }
            if (strInspectionConslusion.Equals("不通过"))
            {
                cbInspectionConclusion.SelectedItem = strInspectionConslusion;
            }
            string strCorrectiveSign = ClientUtil.ToString(record.CorrectiveSign);
            if (strCorrectiveSign.Equals("0"))
            {
                cbCorrectiveSign.SelectedItem = "无需整改";
            }
            if (strCorrectiveSign.Equals("1"))
            {
                cbCorrectiveSign.SelectedItem = "未整改";
            }
            txtInspectionPerson.Text = ClientUtil.ToString(record.CreatePersonName);

            lblRecord.Text = "第【" + i + "】条";
        }

        public override void 功能菜单1Item_Click(object sender, EventArgs e)
        {
            this.txtBearTeamName.Text = "";
            this.txtInspectionPerson.Text = "";
            this.dtpInspectionDate.Text = "";
            this.cbInspectionSpecial.Text = "";
            this.cbInspectionConclusion.Text = "";
            this.cbCorrectiveSign.Text = "";
            this.txtInspectionPerson.Text = "";
            this.txtDocState.Text = "";
        }

        public override void 功能菜单2Item_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ViewToModel()) return;
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "提交成功！";
                v_show.ShowDialog();
            }
            catch (Exception ex)
            {
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据保存错误！";
                v_show.ShowDialog();
            }
        }

        public override void 功能菜单3Item_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (!DeleteModel()) return;
                if (model.DeleteByDao(record))
                {
                    VMessageBox v_show = new VMessageBox();
                    v_show.txtInformation.Text = "删除成功！";
                    v_show.ShowDialog();
                }
            }
            if (weekdetail != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("GWBSTreeName", weekdetail.GWBSTreeName));
                lists = model.GetInspectionRecord(oq);
                if (lists == null || lists.Count == 0) return;
                i = 1;
                record = lists[0] as InspectionRecord;

                lblRecordTotal.Text = "共【" + lists.Count + "】条";
            }
            if (weekdetail == null)
            {
                ObjectQuery oq = new ObjectQuery();
                list = model.GetInspectionRecord(oq);
                if (list == null || list.Count == 0) return;
                i = 1;
                record = list[0] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);

                lblRecordTotal.Text = "共【" + list.Count + "】条";
            }

            txtBearTeamName.Text = ClientUtil.ToString(record.BearTeamName);
            dtpInspectionDate.Value = record.CreateDate;
            cbInspectionSpecial.Text = ClientUtil.ToString(record.InspectionSpecial);
            string strInspectionConslusion = ClientUtil.ToString(record.InspectionConclusion);
            if (strInspectionConslusion.Equals("通过"))
            {
                cbInspectionConclusion.Text = strInspectionConslusion;
            }
            if (strInspectionConslusion.Equals("不通过"))
            {
                cbInspectionConclusion.Text = strInspectionConslusion;
            }
            string strCorrectiveSign = ClientUtil.ToString(record.CorrectiveSign);
            if (strCorrectiveSign.Equals("0"))
            {
                cbCorrectiveSign.Text = "无需整改";
            }
            if (strCorrectiveSign.Equals("1"))
            {
                cbCorrectiveSign.Text = "未整改";
            }
            txtInspectionPerson.Text = ClientUtil.ToString(record.CreatePersonName);

            lblRecord.Text = "第【" + 1 + "】条";
            
        }

        public override void 功能菜单4Item_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ViewToModel()) return;
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "保存成功！";
                v_show.ShowDialog();
            }
            catch (Exception ex)
            {
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据保存错误！";
                v_show.ShowDialog();
            }
        }

        public override void 功能菜单5Item_Click(object sender, EventArgs e)
        {
            this.txtBearTeamName.Text = "";
            this.txtInspectionPerson.Text = "";
            this.dtpInspectionDate.Text = "";
            this.cbInspectionSpecial.Text = "";
            this.cbInspectionConclusion.Text = "";
            this.cbCorrectiveSign.Text = "";
            this.txtInspectionPerson.Text = "";
            this.txtDocState.Text = "";
            record.Id = null;
        }

        private bool ViewToModel()
        {
            InspectionRecord master = new InspectionRecord();
            if (record.Id != null)
            {
                master = record;
            }
            else
            {
                master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                master.HandlePerson = ConstObject.LoginPersonInfo;
                master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                master.ProjectId = projectInfo.Id;
                master.ProjectName = projectInfo.Name;

                //工程任务
                if (txtProjectTask.Tag != null)
                {
                    master.GWBSTree = record.WeekScheduleDetail.GWBSTree as GWBSTree;
                    master.GWBSTreeName = txtProjectTask.Text;
                }
            }

            master.InspectionSpecial = ClientUtil.ToString(cbInspectionSpecial.Text);
            System.Web.UI.WebControls.ListItem li = cbInspectionSpecial.SelectedItem as System.Web.UI.WebControls.ListItem;
            master.InspectionSpecialCode = li.Value;

            master.CreateDate = dtpInspectionDate.Value.Date;
            master.InspectionConclusion = ClientUtil.ToString(cbInspectionConclusion.Text);
            //master.WeekScheduleDetail = weekdetail;
            if (cbCorrectiveSign.Text.Equals("无需整改"))
            {
                master.CorrectiveSign = ClientUtil.ToInt("0");
            }
            if (cbCorrectiveSign.Text.Equals("未整改"))
            {
                master.CorrectiveSign = ClientUtil.ToInt("1");
            }
            record = model.SaveOrUpdateByDao(master) as InspectionRecord;

            WeekScheduleDetail detail = record.WeekScheduleDetail as WeekScheduleDetail;
            //    WeekScheduleDetail weekDetail = dgWeekDetail.CurrentRow.Tag as WeekScheduleDetail;
            string strCheckRequtre = ClientUtil.ToString(record.GWBSTree.CheckRequire);
            string strSpecail = ClientUtil.ToString(record.InspectionSpecialCode);
            if (strSpecail.Equals("001"))
            {
                if (detail.TaskCheckState != null)
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = "2" + strWeek.Substring(1, 11);
                    }
                    else
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = "1" + strWeek.Substring(1, 11);
                    }
                }
                else
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        detail.TaskCheckState = "2" + strCheckRequtre.Substring(1, 11);
                    }
                    else
                    {
                        detail.TaskCheckState = strCheckRequtre;
                    }
                }
            }
            else if (strSpecail.Equals("002"))
            {
                if (detail.TaskCheckState != null)
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 1) + "2" + strWeek.Substring(2, 10);
                    }
                    else
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 1) + "1" + strWeek.Substring(2, 10);
                    }
                }
                else
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        detail.TaskCheckState = strCheckRequtre.Substring(0, 1) + "2" + strCheckRequtre.Substring(2, 10);
                    }
                    else
                    {
                        detail.TaskCheckState = strCheckRequtre;
                    }
                }
            }
            else if (strSpecail.Equals("003"))
            {
                if (detail.TaskCheckState != null)
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 2) + "2" + strWeek.Substring(3, 9);
                    }
                    else
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 2) + "1" + strWeek.Substring(3, 9);
                    }
                }
                else
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        detail.TaskCheckState = strCheckRequtre.Substring(0, 2) + "2" + strCheckRequtre.Substring(3, 9);
                    }
                    else
                    {
                        detail.TaskCheckState = strCheckRequtre;
                    }
                }
                //weekdetail = model.UpdateWeekScheduleDetail(weekdetail);
            }
            else if (strSpecail.Equals("004"))
            {
                if (detail.TaskCheckState != null)
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 3) + "2" + strWeek.Substring(4, 8);
                    }
                    else
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 3) + "1" + strWeek.Substring(4, 8);
                    }
                }
                else
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        detail.TaskCheckState = strCheckRequtre.Substring(0, 3) + "2" + strCheckRequtre.Substring(4, 8);
                    }
                    else
                    {
                        detail.TaskCheckState = strCheckRequtre;
                    }
                }
                //weekdetail = model.UpdateWeekScheduleDetail(weekdetail);
            }
            else if (strSpecail.Equals("005"))
            {
                if (detail.TaskCheckState != null)
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 4) + "2" + strWeek.Substring(5, 7);
                    }
                    else
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 4) + "1" + strWeek.Substring(5, 7);
                    }
                }
                else
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        detail.TaskCheckState = strCheckRequtre.Substring(0, 4) + "2" + strCheckRequtre.Substring(5, 7);
                    }
                    else
                    {
                        detail.TaskCheckState = strCheckRequtre;
                    }
                }
                //weekdetail = model.UpdateWeekScheduleDetail(weekdetail);
            }
            else if (strSpecail.Equals("006"))
            {
                if (detail.TaskCheckState != null)
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 5) + "2" + strWeek.Substring(6, 6);
                    }
                    else
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 5) + "1" + strWeek.Substring(6, 6);
                    }
                }
                else
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        detail.TaskCheckState = strCheckRequtre.Substring(0, 5) + "2" + strCheckRequtre.Substring(6, 6);
                    }
                    else
                    {
                        detail.TaskCheckState = strCheckRequtre;
                    }
                }
                //weekdetail = model.UpdateWeekScheduleDetail(weekdetail);
            }
            else if (strSpecail.Equals("007"))
            {
                if (detail.TaskCheckState != null)
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 6) + "2" + strWeek.Substring(7, 5);
                    }
                    else
                    {
                        string strWeek = ClientUtil.ToString(detail.TaskCheckState);
                        detail.TaskCheckState = strWeek.Substring(0, 6) + "1" + strWeek.Substring(7, 5);
                    }
                }
                else
                {
                    if (record.InspectionConclusion == "通过")
                    {
                        detail.TaskCheckState = strCheckRequtre.Substring(0, 6) + "2" + strCheckRequtre.Substring(7, 5);
                    }
                    else
                    {
                        detail.TaskCheckState = strCheckRequtre;
                    }
                }
                //weekdetail = model.UpdateWeekScheduleDetail(weekdetail);
            }

            detail = model.UpdateWeekScheduleDetail(detail);

            return true;
        }

        private bool DeleteModel()
        {
            try
            {
                string strState = ClientUtil.ToString(weekdetail.TaskCheckState);
                string strInspectionSpecial = ClientUtil.ToString(cbInspectionSpecial.Text);
                string strStates = "";

                if (strState != null)
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

                    this.weekdetail.TaskCheckState = strStates;
                    WeekScheduleDetail detail = model.SaveOrUpdateByDao(weekdetail) as WeekScheduleDetail;
                }
                return true;
            }
            catch (Exception e)
            {
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据错误！" + ExceptionUtil.ExceptionMessage(e);
                v_show.ShowDialog();
                return false;
            }
        }

        void btnTheFirst_Click(object sender, EventArgs e)
        {
            i = 1;
            i = pageIndex;
            InitData();
            btnBack.Enabled = false;
            btnTheFirst.Enabled = false;
            btnNext.Enabled = true;
            btnTheLast.Enabled = true;
            
            lblRecord.Text = "第【" + i + "】条";
        }

        void btnBack_Click(object sender, EventArgs e)
        {
            if (i - 1 == 0) return;
            if (i - 1 == 1)
            {
                btnBack.Enabled = false;
                btnTheFirst.Enabled = false;

            }
            btnNext.Enabled = true;
            btnTheLast.Enabled = true;

            if (weekdetail != null)
            {
                record = lists[i - 2] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.WeekScheduleDetail.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.WeekScheduleDetail.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            }
            if (weekdetail == null)
            {
                record = list[i - 2] as InspectionRecord;
                txtProjectTask.Text = ClientUtil.ToString(record.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            }
            txtBearTeamName.Text = ClientUtil.ToString(record.BearTeamName);
            dtpInspectionDate.Value = record.CreateDate;
            cbInspectionSpecial.Text = ClientUtil.ToString(record.InspectionSpecial);
            string strInspectionConslusion = ClientUtil.ToString(record.InspectionConclusion);
            if (strInspectionConslusion.Equals("通过"))
            {
                cbInspectionConclusion.Text = strInspectionConslusion;
            }
            if (strInspectionConslusion.Equals("不通过"))
            {
                cbInspectionConclusion.Text = strInspectionConslusion;
            }
            string strCorrectiveSign = ClientUtil.ToString(record.CorrectiveSign);
            if (strCorrectiveSign.Equals("0"))
            {
                cbCorrectiveSign.Text = "无需整改";
            }
            if (strCorrectiveSign.Equals("1"))
            {
                cbCorrectiveSign.Text = "未整改";
            }
            txtInspectionPerson.Text = ClientUtil.ToString(record.CreatePersonName);
            i--;
            lblRecord.Text = "第【" + i + "】条";
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            if (weekdetail != null)
            {
                if (i + 1 > lists.Count) return;
                if (i + 1 == lists.Count)
                {
                    btnNext.Enabled = false;
                    btnTheLast.Enabled = false;

                }
                btnBack.Enabled = true;
                btnTheFirst.Enabled = true;
                record = lists[i] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.WeekScheduleDetail.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.WeekScheduleDetail.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            }
            if (weekdetail == null)
            {
                if (i + 1 > list.Count) return;
                if (i + 1 == list.Count)
                {
                    btnNext.Enabled = false;
                    btnTheLast.Enabled = false;

                }
                btnBack.Enabled = true;
                btnTheFirst.Enabled = true;
                record = list[i] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            }
            txtBearTeamName.Text = ClientUtil.ToString(record.BearTeamName);
            dtpInspectionDate.Value = record.CreateDate;
            cbInspectionSpecial.Text = ClientUtil.ToString(record.InspectionSpecial);
            string strInspectionConslusion = ClientUtil.ToString(record.InspectionConclusion);
            if (strInspectionConslusion.Equals("通过"))
            {
                cbInspectionConclusion.Text = strInspectionConslusion;
            }
            if (strInspectionConslusion.Equals("不通过"))
            {
                cbInspectionConclusion.Text = strInspectionConslusion;
            }
            string strCorrectiveSign = ClientUtil.ToString(record.CorrectiveSign);
            if (strCorrectiveSign.Equals("0"))
            {
                cbCorrectiveSign.Text = "无需整改";
            }
            if (strCorrectiveSign.Equals("1"))
            {
                cbCorrectiveSign.Text = "未整改";
            }
            txtInspectionPerson.Text = ClientUtil.ToString(record.CreatePersonName);
            i++;

            lblRecord.Text = "第【" + i + "】条";
        }

        void btnTheLast_Click(object sender, EventArgs e)
        {
            btnBack.Enabled = true;
            btnTheFirst.Enabled = true;
            btnNext.Enabled = false;
            btnTheLast.Enabled = false;
            record = lists[lists.Count - 1] as InspectionRecord;

            txtProjectTask.Text = ClientUtil.ToString(record.WeekScheduleDetail.GWBSTreeName);
            txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.WeekScheduleDetail.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);

            txtBearTeamName.Text = ClientUtil.ToString(record.BearTeamName);
            dtpInspectionDate.Value = record.CreateDate;
            cbInspectionSpecial.Text = ClientUtil.ToString(record.InspectionSpecial);
            string strInspectionConslusion = ClientUtil.ToString(record.InspectionConclusion);
            if (strInspectionConslusion.Equals("通过"))
            {
                cbInspectionConclusion.Text = strInspectionConslusion;
            }
            if (strInspectionConslusion.Equals("不通过"))
            {
                cbInspectionConclusion.Text = strInspectionConslusion;
            }
            string strCorrectiveSign = ClientUtil.ToString(record.CorrectiveSign);
            if (strCorrectiveSign.Equals("0"))
            {
                cbCorrectiveSign.Text = "无需整改";
            }
            if (strCorrectiveSign.Equals("1"))
            {
                cbCorrectiveSign.Text = "未整改";
            }
            txtInspectionPerson.Text = ClientUtil.ToString(record.CreatePersonName);

            lblRecord.Text = "第【" + lists.Count + "】条";
        }

        void InitData()
        {
            weekdetail = list[0] as WeekScheduleDetail;
            
            if (weekdetail != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("GWBSTreeName", weekdetail.GWBSTreeName));
                lists = model.GetInspectionRecord(oq);
                if (lists == null || lists.Count == 0) return;
                i = 1;
                record = lists[0] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.WeekScheduleDetail.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.WeekScheduleDetail.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);

                lblRecordTotal.Text = "共【" + lists.Count + "】条";
            }
            if (weekdetail == null)
            {
                if (list == null || list.Count == 0) return;
                i = 1;
                record = list[0] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);

                lblRecordTotal.Text = "共【" + list.Count + "】条";
            }
            txtBearTeamName.Text = ClientUtil.ToString(record.BearTeamName);
            dtpInspectionDate.Value = record.CreateDate;
            cbInspectionSpecial.Text = ClientUtil.ToString(record.InspectionSpecial);
            string strInspectionConslusion = ClientUtil.ToString(record.InspectionConclusion);
            if (strInspectionConslusion.Equals("通过"))
            {
                cbInspectionConclusion.Text = strInspectionConslusion;
            }
            if (strInspectionConslusion.Equals("不通过"))
            {
                cbInspectionConclusion.Text = strInspectionConslusion;
            }
            string strCorrectiveSign = ClientUtil.ToString(record.CorrectiveSign);
            if (strCorrectiveSign.Equals("0"))
            {
                cbCorrectiveSign.Text = "无需整改";
            }
            if (strCorrectiveSign.Equals("1"))
            {
                cbCorrectiveSign.Text = "未整改";
            }
            txtInspectionPerson.Text = ClientUtil.ToString(record.CreatePersonName);

            lblRecord.Text = "第【" + i + "】条";
            
        }

        private void VDailyInspectionRecord_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

    }
}