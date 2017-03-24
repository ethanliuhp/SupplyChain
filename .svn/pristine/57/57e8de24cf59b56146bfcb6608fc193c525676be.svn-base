using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Properties;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using FlexCell;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VScheduleBak : TMasterDetailView
    {
        private MProductionMng model = new MProductionMng();
        private Hashtable detailHashtable = new Hashtable();
        private ProductionScheduleMaster CurBillMaster;
        private ProductionScheduleDetail ChildRootNode;
        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";
        private EnumScheduleType enumScheduleType;

        private int startImageCol = 1, endImageCol = 19;

        public VScheduleBak(EnumScheduleType enumScheduleType)
        {
            InitializeComponent();
            this.enumScheduleType = enumScheduleType;
            InitEvents();
            InitData();
        }

        private void InitEvents()
        {
            btnGWBS.Click += new EventHandler(btnGWBS_Click);
            cboScheduleType.SelectedIndexChanged += new EventHandler(cboScheduleType_SelectedIndexChanged);
            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnInvalid.Click += new EventHandler(btnInvalid_Click);
            btnPublish.Click += new EventHandler(btnPublish_Click);

            btnExportToMPP.Click += new EventHandler(btnExportToMPP_Click);
            btnImportFromMPP.Click += new EventHandler(btnImportFromMPP_Click);
        }

        void btnImportFromMPP_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "项目 (*.MPP)|*.MPP";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName;
                List<ProductionScheduleDetail> list = MSProjectUtil.ReadMPP(fileName);
                updateSchedule(list);
            }
        }

        private void updateSchedule(List<ProductionScheduleDetail> list)
        {
            if (list == null || list.Count == 0) return;
            foreach (ProductionScheduleDetail detail in list)
            {
                for (int i = 1; i <= flexGrid.Rows; i++)
                {
                    if (flexGrid.Cell(i, 0) == null) continue;
                    string detailId = flexGrid.Cell(i, 0).Tag;
                    if (detail.Id == detailId)
                    {
                        //计划开始日期
                        flexGrid.Cell(i, endImageCol + 1).Text = detail.PlannedBeginDate.ToShortDateString();
                        //计划结束日期
                        flexGrid.Cell(i, endImageCol + 2).Text = detail.PlannedEndDate.ToShortDateString();
                        //工期
                        flexGrid.Cell(i, endImageCol + 4).Text = detail.PlannedDuration.ToString();
                        break;
                    }
                }
            }
        }

        void btnExportToMPP_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofg = new OpenFileDialog();
            SaveFileDialog sfd = new SaveFileDialog();
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.Filter = "项目 (*.MPP)|*.MPP";
            sfd.RestoreDirectory = true;
            sfd.FileName = CurBillMaster.ScheduleName;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                if (System.IO.File.Exists(fileName))
                {
                    IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
                    MSProjectUtil mSProjectUtil = new MSProjectUtil();
                    mSProjectUtil.UpdateMPP(fileName, list);
                }
                else
                {
                    IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
                    MSProjectUtil.CreateMPP(fileName, list,this.Handle );
                }
            }


        }

        void btnPublish_Click(object sender, EventArgs e)
        {
            int activeRowIndex = flexGrid.ActiveCell.Row;
            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
            if (detailId != null && !detailId.Equals(""))
            {
                ProductionScheduleDetail detail = (ProductionScheduleDetail)detailHashtable[detailId];
                int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
                if (detail == null)
                {
                    MessageBox.Show("没有找到进度计划。");
                    return;
                }
                if (MessageBox.Show("确定要发布【" + detail.GWBSTreeName + "】和其下的进度计划吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    try
                    {
                        model.ProductionManagementSrv.PublishScheduleDetail(detailId);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("发布出错。\n" + ex.Message);
                    }
                }
            }
        }

        void btnInvalid_Click(object sender, EventArgs e)
        {
            int activeRowIndex = flexGrid.ActiveCell.Row;
            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
            if (detailId != null && !detailId.Equals(""))
            {
                ProductionScheduleDetail detail = (ProductionScheduleDetail)detailHashtable[detailId];

                if (detail == null)
                {
                    MessageBox.Show("没有找到进度计划。");
                    return;
                }
                if (MessageBox.Show("确定要作废【" + detail.GWBSTreeName + "】和其下的进度计划吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    try
                    {
                        //int childs = 1;
                        //model.ProductionManagementSrv.InvalidScrollSchdulePlanDtl(detail, out childs);
                        //for (int i = 0; i <= childs; i++)
                        //{
                        //    flexGrid.Row(activeRowIndex).Delete();
                        //}
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("作废出错。\n" + ex.Message);
                    }
                }
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtScheduleName.Text))
            {
                MessageBox.Show("请输入计划名称。");
                txtScheduleName.Focus();
                return;
            }
            SaveSchedule();
        }

        private bool SaveSchedule()
        {
            try
            {
                CurBillMaster = model.ProductionManagementSrv.GetSchedulesById(CurBillMaster.Id);
                ViewToMaster();
                ViewToDetails();
                CurBillMaster = model.ProductionManagementSrv.SaveOrUpdateByDao(CurBillMaster) as ProductionScheduleMaster;
                MessageBox.Show("保存成功。");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存出错。\n" + ex.Message);
            }
            return false;
        }

        private void ViewToMaster()
        {
            CurBillMaster.ScheduleTypeDetail = cboScheduleType.SelectedItem + "";
            CurBillMaster.ScheduleCaliber = cboScheduleCaliber.SelectedItem + "";
            CurBillMaster.ScheduleName = txtScheduleName.Text;
            CurBillMaster.Descript = txtRemark.Text;
        }

        private IList ViewToDetails()
        {
            IList list = new ArrayList();
            for (int i = 1; i < flexGrid.Rows; i++)
            {
                string detailId = flexGrid.Cell(i, 0).Tag;
                if (detailId == null || detailId.Equals("")) continue;
                ProductionScheduleDetail detail = null;
                foreach (ProductionScheduleDetail tempDetail in CurBillMaster.Details)
                {
                    if (detailId == tempDetail.Id)
                    {
                        detail = tempDetail;
                        break;
                    }
                }
                //计划开始时间
                string PlannedBeginDateStr = flexGrid.Cell(i, endImageCol + 1).Text;
                if (PlannedBeginDateStr != null && !PlannedBeginDateStr.Equals(""))
                {
                    detail.PlannedBeginDate = DateTime.Parse(PlannedBeginDateStr);
                }
                else
                {
                    detail.PlannedBeginDate = new DateTime(1900, 1, 1);
                }
                //计划结束时间
                string PlannedEndDateStr = flexGrid.Cell(i, endImageCol + 2).Text;
                if (PlannedEndDateStr != null && !PlannedEndDateStr.Equals(""))
                {
                    detail.PlannedEndDate = DateTime.Parse(PlannedEndDateStr);
                }
                else
                {
                    detail.PlannedEndDate = new DateTime(1900, 1, 1);
                }
                //工期计量单位
                detail.ScheduleUnit = flexGrid.Cell(i, endImageCol + 3).Text;
                //计划工期
                detail.PlannedDuration = flexGrid.Cell(i, endImageCol + 4).IntegerValue;
                //实际开始时间
                string actualBeginDate = flexGrid.Cell(i, endImageCol + 5).Text;
                if (actualBeginDate != null && !actualBeginDate.Equals(""))
                {
                    detail.ActualBeginDate = DateTime.Parse(actualBeginDate);
                }
                else
                {
                    detail.ActualBeginDate = new DateTime(1900, 1, 1);
                }

                //实际结束时间
                string actualEndDate = flexGrid.Cell(i, endImageCol + 6).Text;
                if (actualEndDate != null && !actualEndDate.Equals(""))
                {
                    detail.ActualEndDate = DateTime.Parse(actualEndDate);
                }
                else
                {
                    detail.ActualEndDate = new DateTime(1900, 1, 1);
                }
                //实际工期
                detail.ActualDuration = flexGrid.Cell(i, endImageCol + 7).IntegerValue;
                //计划说明
                detail.TaskDescript = flexGrid.Cell(i, endImageCol + 8).Text;
                CurBillMaster.AddDetail(detail);
            }
            return list;
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            int activeRowIndex = flexGrid.ActiveCell.Row;
            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
            if (detailId != null && !detailId.Equals(""))
            {
                ProductionScheduleDetail detail = (ProductionScheduleDetail)detailHashtable[detailId];
                if (detail == null)
                {
                    MessageBox.Show("选择计划明细不存在(或已被其他操作员删除),请重载该计划！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (detail.Level == 1)
                {
                    MessageBox.Show("根节点不能删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("确定要删除【" + detail.GWBSTreeName + "】及其下属滚动计划吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        FlashScreen.Show("正在执行删除操作,请稍候......");

                        int childs = 0;
                        string errMsg = "";

                        IList list = model.ProductionManagementSrv.DeleteScheduleDetail(detail,  childs,  errMsg);

                        errMsg = list[0] as string;
                        childs = Convert.ToInt32(list[1]);

                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            FlashScreen.Close();
                            MessageBox.Show(errMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        //flexGrid.AutoRedraw = false;

                        int rowIndexs = activeRowIndex + childs;
                        for (int i = rowIndexs; i >= activeRowIndex; i--)
                        {
                            flexGrid.Row(i).Delete();
                        }
                        //flexGrid.AutoRedraw = true;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除失败.\n" + ex.Message);
                        return;
                    }
                    finally
                    {
                        FlashScreen.Close();
                    }

                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        void flexGrid_Click(object Sender, EventArgs e)
        {
            if (flexGrid.ActiveCell.ImageKey == imageExpand)
            {
                flexGrid.ActiveCell.SetImage(imageCollapse);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, true);
                }

            }
            else if (flexGrid.ActiveCell.ImageKey == imageCollapse)
            {
                flexGrid.ActiveCell.SetImage(imageExpand);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, false);
                }
            }
        }

        private void SetRowVisible(int row1, int row2, bool value)
        {
            flexGrid.AutoRedraw = false;
            for (int i = row1; i <= row2; i++)
            {
                flexGrid.Row(i).Visible = value;
                if (value)
                {
                    for (int j = startImageCol; j <= endImageCol; j++)
                    {
                        if (flexGrid.Cell(i, j).ImageKey != null && !flexGrid.Cell(i, j).ImageKey.Equals(""))
                        {
                            flexGrid.Cell(i, j).SetImage(imageCollapse);
                            break;
                        }
                    }
                }
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        void cboScheduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scheduleType = cboScheduleType.SelectedItem as string;
            try
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ScheduleType", this.enumScheduleType));
                oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
                oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", scheduleType));
                ProductionScheduleMaster master = model.ProductionManagementSrv.GetSchedulesByType(oq);
                if (master == null)
                {
                    master = new ProductionScheduleMaster();
                    NewMaster(master);
                    CurBillMaster = model.ProductionManagementSrv.NewSchedule(master) as ProductionScheduleMaster;
                    ChildRootNode = CurBillMaster.GetChildRootNode();
                }
                else
                {
                    CurBillMaster = master;
                    ChildRootNode = CurBillMaster.GetChildRootNode();
                    FreshMasterInfo(master);
                }
                FillFlex();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询进度计划出错。\n" + ex.Message);
                return;
            }
        }

        private void NewMaster(ProductionScheduleMaster master)
        {
            master.CreateDate = DateTime.Now;
            master.HandlePerson = ConstObject.LoginPersonInfo;
            master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            master.OperOrgInfo = ConstObject.TheOperationOrg;
            master.OperOrgInfoName = ConstObject.TheOperationOrg.Name;
            master.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
            //归属项目
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                master.ProjectId = projectInfo.Id;
                master.ProjectName = projectInfo.Name;
            }
            master.DocState = DocumentState.Edit;
            master.ScheduleType = this.enumScheduleType;
            master.ScheduleCaliber = cboScheduleCaliber.SelectedItem.ToString();
            master.ScheduleTypeDetail = cboScheduleType.SelectedItem + "";
        }

        private void FreshMasterInfo(ProductionScheduleMaster master)
        {
            cboScheduleCaliber.SelectedItem = master.ScheduleCaliber;
            txtScheduleName.Text = master.ScheduleName;
            txtRemark.Text = master.Descript;
        }

        private void InitData()
        {
            InitFlexGrid(5);

            //进度计划口径
            VBasicDataOptr.InitScheduleCaliber(cboScheduleCaliber, false);
            if (cboScheduleCaliber.Items.Count > 0)
            {
                cboScheduleCaliber.SelectedIndex = 0;
            }

            if (enumScheduleType == EnumScheduleType.总进度计划)
            {
                //总进度计划类型
                VBasicDataOptr.InitScheduleType(cboScheduleType, false);
                if (cboScheduleType.Items.Count > 0)
                {
                    cboScheduleType.SelectedIndex = 0;
                }

                btnPubSchedule.Visible = false;
            }
            else if (enumScheduleType == EnumScheduleType.总滚动进度计划)
            {
                //总滚动进度计划类型
                VBasicDataOptr.InitScheduleTypeRolling(cboScheduleType, false);
                if (cboScheduleType.Items.Count > 0)
                {
                    cboScheduleType.SelectedIndex = 0;
                }

                btnPubSchedule.Visible = true;
            }
        }

        void btnGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree frm = new VSelectGWBSTree(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;

            if (list != null && list.Count > 0)
            {
                TreeNode treeNode = list[0];
                if (CurBillMaster != null)
                {
                    CurBillMaster = model.ProductionManagementSrv.GetSchedulesById(CurBillMaster.Id);
                }
                //判断工程任务节点是否可以添加
                if (!CanAddChild(treeNode, CurBillMaster)) return;

                ProductionScheduleDetail parentNode = FindParentNode(treeNode, CurBillMaster);
                if (parentNode == null) parentNode = ChildRootNode;
                try
                {
                    CurBillMaster = SaveChilds(treeNode, parentNode, CurBillMaster);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("生成进度计划出错。\n" + ex.Message);
                    return;
                }

                //AddRow(startRow, treeNode, ChildRootNode, treeNode.Level + 1);
                FillFlex();
            }
        }

        private ProductionScheduleMaster SaveChilds(TreeNode treeNode, ProductionScheduleDetail parentNode, ProductionScheduleMaster master)
        {
            IList detailList = new ArrayList();
            try
            {
                AddChild(treeNode, parentNode, ref detailList, master);
            }
            catch
            {
                model.ProductionManagementSrv.DeleteByDao(detailList);
                detailList.Clear();
                MessageBox.Show("生成进度计划明细出错。");
            }
            if (detailList.Count > 0)
            {
                return model.ProductionManagementSrv.SaveSchedule(master, detailList);
            }
            else
            {
                return master;
            }
        }

        private void AddChild(TreeNode treeNode, ProductionScheduleDetail parentNode, ref IList detailList, ProductionScheduleMaster master)
        {
            ProductionScheduleDetail detail = new ProductionScheduleDetail();
            detail.GWBSTree = treeNode.Tag as GWBSTree;
            if (detail.GWBSTree != null)
            {
                detail.GWBSTreeName = detail.GWBSTree.Name;
                detail.OrderNo = (int)detail.GWBSTree.OrderNo;
            }
            detail.ParentNode = parentNode;
            detail.Level = parentNode.Level + 1;
            detail.Master = master;
            detail.State = EnumScheduleDetailState.编辑;
            detail = model.ProductionManagementSrv.SaveOrUpdateByDao(detail) as ProductionScheduleDetail;
            detail.SysCode = parentNode.SysCode + detail.Id + ".";

            //detail = model.ProductionManagementSrv.SaveOrUpdateByDao(detail) as ProductionScheduleDetail;
            //master.AddDetail(detail);
            detailList.Add(detail);
            foreach (TreeNode tn in treeNode.Nodes)
            {
                AddChild(tn, detail, ref detailList, master);
            }
        }

        /// <summary>
        /// 判断工程任务节点是否可以添加
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="master"></param>
        /// <returns></returns>
        private bool CanAddChild(TreeNode treeNode, ProductionScheduleMaster master)
        {
            GWBSTree gwbsTree = treeNode.Tag as GWBSTree;
            foreach (ProductionScheduleDetail detail in master.Details)
            {
                if (detail.GWBSTree == null) continue;
                if (gwbsTree.Id == detail.GWBSTree.Id)
                {
                    MessageBox.Show("【" + gwbsTree.Name + "】已经存在，不能添加！");
                    return false;
                }
            }
            foreach (TreeNode tn in treeNode.Nodes)
            {
                return CanAddChild(tn, master);
            }
            return true;
        }

        /// <summary>
        /// 查找当前工程任务的父节点
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="master"></param>
        /// <returns></returns>
        private ProductionScheduleDetail FindParentNode(TreeNode treeNode, ProductionScheduleMaster master)
        {
            TreeNode tn = treeNode.Parent;
            if (tn == null) return null;
            GWBSTree gwbsTree = tn.Tag as GWBSTree;
            if (gwbsTree == null) return null;
            foreach (ProductionScheduleDetail detail in master.Details)
            {
                if (detail.GWBSTree == null) continue;
                if (gwbsTree.Id == detail.GWBSTree.Id)
                {
                    return detail;
                }
            }
            return null;
        }

        private void FillFlex()
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;
            flexGrid.Column(endImageCol + 3).Locked = true;
            detailHashtable.Clear();
            IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
            if (list != null && list.Count > 0)
            {
                flexGrid.Rows = list.Count + 1;
                int rowIndex = 1;
                foreach (ProductionScheduleDetail detail in list)
                {
                    if (detail.State == EnumScheduleDetailState.失效)
                    {
                        flexGrid.Rows = flexGrid.Rows - 1;
                        continue;
                    }
                    flexGrid.Cell(rowIndex, 0).Tag = detail.Id;
                    detailHashtable.Add(detail.Id, detail);
                    flexGrid.Cell(rowIndex, detail.Level).SetImage(imageCollapse);
                    FlexCell.Range rangeTemp = flexGrid.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    rangeTemp.Merge();
                    //rangeTemp.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    flexGrid.Cell(rowIndex, detail.Level + 1).Text = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName;
                    //计划开始时间
                    if (detail.PlannedBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text = ""; //"计划开始时间";//20
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text = detail.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                    }
                    //计划结束时间
                    if (detail.PlannedEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划结束时间";//21
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = detail.PlannedEndDate.ToShortDateString();
                    }
                    //工期计量单位                    
                    //flexGrid.Cell(rowIndex, endImageCol + 3).Text =detail.ScheduleUnit; //"";//22
                    flexGrid.Cell(rowIndex, endImageCol + 3).Text = "天";


                    //计划工期
                    flexGrid.Cell(rowIndex, endImageCol + 4).Text = detail.PlannedDuration == 0 ? "" : detail.PlannedDuration.ToString(); //"计划工期";//23
                    //实际开始时间
                    if (detail.ActualBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 5).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 5).Text = detail.ActualBeginDate.ToShortDateString();
                    }
                    //实际结束时间
                    if (detail.ActualEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 6).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 6).Text = detail.ActualEndDate.ToShortDateString();
                    }
                    //实际工期
                    flexGrid.Cell(rowIndex, endImageCol + 7).Text = detail.ActualDuration == 0 ? "" : detail.ActualDuration.ToString();
                    //计划说明
                    flexGrid.Cell(rowIndex, endImageCol + 8).Text = detail.TaskDescript; //"计划说明";//27

                    rowIndex = rowIndex + 1;
                }
            }
            //1-19列的背景色
            FlexCell.Range range = flexGrid.Range(1, startImageCol, flexGrid.Rows - 1, endImageCol);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private int NodesCount(TreeNode treeNode)
        {
            int result = 0;
            if (treeNode != null)
            {
                result = treeNode.Nodes.Count;
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    result += NodesCount(tn);
                }
            }
            return result;
        }

        private void InitFlexGrid(int rows)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = false;

            flexGrid.Rows = rows;
            flexGrid.Cols = 28;//其中0列隐藏 1-19 为放置图片列 20-27为数据列

            flexGrid.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGrid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = FlexCell.ScrollBarsEnum.Vertical;
            flexGrid.BackColorBkg = SystemColors.Control;
            flexGrid.DefaultFont = new Font("Tahoma", 8);

            FlexCell.Range range;

            for (int i = startImageCol; i <= endImageCol; i++)
            {
                flexGrid.Column(i).TabStop = false;
                flexGrid.Column(i).Width = 20;
            }

            range = flexGrid.Range(0, startImageCol, 0, endImageCol);
            range.Merge();
            flexGrid.Cell(0, 1).Text = "任务名称";
            flexGrid.Cell(0, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;

            // 加载图片
            flexGrid.Images.Add(Resources.ImageExpend, imageExpand);
            flexGrid.Images.Add(Resources.ImageFold, imageCollapse);

            flexGrid.Cell(0, endImageCol + 1).Text = "计划开始时间";//20
            flexGrid.Cell(0, endImageCol + 2).Text = "计划结束时间";//21
            flexGrid.Cell(0, endImageCol + 3).Text = "工期计量单位";//22
            flexGrid.Cell(0, endImageCol + 4).Text = "计划工期";//23
            flexGrid.Cell(0, endImageCol + 5).Text = "实际开始时间";//24
            flexGrid.Cell(0, endImageCol + 6).Text = "实际结束时间";//25
            flexGrid.Cell(0, endImageCol + 7).Text = "实际工期";//26
            flexGrid.Cell(0, endImageCol + 8).Text = "计划说明";//27

            flexGrid.Column(endImageCol + 1).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 2).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 3).CellType = FlexCell.CellTypeEnum.ComboBox;
            flexGrid.Column(endImageCol + 5).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 6).CellType = FlexCell.CellTypeEnum.Calendar;

            flexGrid.Column(endImageCol + 4).Mask = FlexCell.MaskEnum.Digital;
            flexGrid.Column(endImageCol + 7).Mask = FlexCell.MaskEnum.Digital;

            FlexCell.ComboBox cb = flexGrid.ComboBox(endImageCol + 3);
            flexGrid.ComboBox(endImageCol + 3).Locked = true;
            try
            {
                IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ScheduleUnit);
                if (list != null && list.Count > 0)
                {
                    foreach (BasicDataOptr bd in list)
                    {
                        cb.Items.Add(bd.BasicName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取工期计量单位出错。");
            }

            // Refresh
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
    }
}
