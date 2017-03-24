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
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.OBS;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng
{
    public partial class VRectificationNoticeQuery : TBasicDataView
    {
        private MRectificationNoticeMng model = new MRectificationNoticeMng();

        public VRectificationNoticeQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            comMngType.Items.Clear();
            comMngType1.Items.Clear();
            Array tem = Enum.GetValues(typeof(DocumentState));
            for (int i = 0; i < tem.Length; i++)
            {
                DocumentState s = (DocumentState)tem.GetValue(i);
                int k = (int)s;
                if (k != 0)
                {
                    string strNewName = ClientUtil.GetDocStateName(k);
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = strNewName;
                    li.Value = k.ToString();
                    comMngType.Items.Add(li);
                    comMngType1.Items.Add(li);
                }
            }
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick+=new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            txtAccepType.Items.AddRange(new object[] { "工程日常检查",  "专业检查" });
            txtAccepTypeBill.Items.AddRange(new object[] { "工程日常检查", "专业检查" });
            btnSelect.Click +=new EventHandler(btnSelect_Click);
            btnSelectBill.Click+=new EventHandler(btnSelectBill_Click);
            dgMaster.SelectionChanged+=new EventHandler(dgMaster_SelectionChanged);
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(this.colCode.Name))
            {
                return;
            }
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VRectificationNoticeMng vRN = new VRectificationNoticeMng();
                    vRN.Start(billId);
                    vRN.ShowDialog();
                }
            }
        }
        //单据选择队伍
        void btnSelectBill_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;

            txtSuppilerBill.Text = engineerMaster.BearerOrgName;
            txtSuppilerBill.Tag = engineerMaster;
        }
        //明细选择队伍
        void btnSelect_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;

            txtSuppiler.Text = engineerMaster.BearerOrgName;
            txtSuppiler.Tag = engineerMaster;
        }
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                //DailyPlanMaster master = model.DailyPlanSrv.GetDailyPlanByCode(dgvCell.Value.ToString());
                RectificationNoticeMaster master = dgMaster.Rows[e.RowIndex].Tag as RectificationNoticeMaster;
                VRectificationNoticeMng vmro = new VRectificationNoticeMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
            else if (dgvCell.OwningColumn.Name == colDocument.Name)
            {
                RectificationNoticeMaster master = dgMaster.Rows[e.RowIndex].Tag as RectificationNoticeMaster;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", master.Id));
                IList listObj = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                if (listObj != null && listObj.Count > 0)
                {
                    oq.Criterions.Clear();
                    Disjunction dis = new Disjunction();
                    foreach (ProObjectRelaDocument obj in listObj)
                    {
                        dis.Add(Expression.Eq("Id", obj.DocumentGUID));
                    }
                    oq.AddCriterion(dis);
                    IList docList = model.ObjectQuery(typeof(DocumentMaster), oq);
                    if (docList != null && docList.Count > 0)
                    {
                        VDocumentsDownloadOrOpen frm = new VDocumentsDownloadOrOpen(docList);
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("未找到相关文档！");
                    }
                }
                else
                {
                    MessageBox.Show("未找到相关文档！");
                }
            }
        }
        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                RectificationNoticeMaster master = model.RectificationNoticeSrv.GetRectificationNoticeByCode(dgvCell.Value.ToString());
                VRectificationNoticeMng vmro = new VRectificationNoticeMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                //单据号
                if (this.txtCodeBegin.Text != "")
                {
                    condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询     
                }
                //制单日期
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }

                //制单人
                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
                }

                //检查类型
                if (txtAccepType.SelectedItem != null)
                {
                    condition += "and t1.INSPECTIONTYPE = '" + txtAccepType.SelectedIndex + "'";
                }
                //供应商
                if (txtSuppiler.Text != "")
                {
                    condition += "and t1.SupplierUnitName = '" + txtSuppiler.Text + "' and t1.SupplierUnit = '" + (txtSuppiler.Tag as SubContractProject).Id + "'";
                }
                if (comMngType1.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType1.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                #endregion
                DataSet dataSet = model.RectificationNoticeSrv.RectificationNoticeQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    if (ClientUtil.ToDateTime(dataRow["RequiredDate"]) > ClientUtil.ToDateTime("1900-1-1"))
                    {
                        dgDetail[colInsCompleteDate.Name, rowIndex].Value = ClientUtil.ToString(ClientUtil.ToDateTime(dataRow["RequiredDate"]).ToShortDateString());
                    }
                    if (ClientUtil.ToDateTime(dataRow["RectDate"]) > ClientUtil.ToDateTime("1900-1-1"))
                    {
                        dgDetail[colInsConcluDate.Name, rowIndex].Value = ClientUtil.ToString(ClientUtil.ToDateTime(dataRow["RectDate"]).ToShortDateString());
                    }
                    string strName = dataRow["RectConclusion"].ToString();
                    if (strName.Equals("0"))
                    {
                        dgDetail[colInsConclusion.Name, rowIndex].Value = "整改中";
                    }
                    if (strName.Equals("1"))
                    {
                        dgDetail[colInsConclusion.Name, rowIndex].Value = "整改未通过";
                    }
                    if (strName.Equals("2"))
                    {
                        dgDetail[colInsConclusion.Name, rowIndex].Value = "整改通过";
                    }
                    dgDetail[colInsHandlePerson.Name, rowIndex].Value = dataRow["HandlePersonName"].ToString();
                    dgDetail[colInsQuestion.Name, rowIndex].Value = dataRow["QuestionState"].ToString();
                    dgDetail[colInsQuestionCode.Name, rowIndex].Value = dataRow["ProblemCode"].ToString();
                    dgDetail[colInsRequired.Name, rowIndex].Value = dataRow["Rectrequired"].ToString();
                    dgDetail[colInsResult.Name, rowIndex].Value = dataRow["RectContent"].ToString();
                    if (ClientUtil.ToDateTime(dataRow["RectSendDate"]) > ClientUtil.ToDateTime("1900-1-1"))
                    {
                        dgDetail[colInsSendDate.Name, rowIndex].Value = ClientUtil.ToString(ClientUtil.ToDateTime(dataRow["RectSendDate"]).ToShortDateString());
                    }
                    dgDetail[colInsSupplier.Name, rowIndex].Value = dataRow["SupplierUnitName"].ToString();
                    string strType = dataRow["InspectionType"].ToString();
                    if (strType.Equals("0"))
                    {
                        dgDetail[colInsType.Name, rowIndex].Value = "工程日常检查";
                    }
                    if (strType.Equals("1"))
                    {
                        dgDetail[colInsType.Name, rowIndex].Value = "专业检查";
                    } 
                    dgDetail[colCreateDate.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                }
                FlashScreen.Close();
                lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
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
        /// <summary>
        /// 主从表显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchBill_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            //单据
            if (txtCodeBeginBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, NHibernate.Criterion.MatchMode.Anywhere));
            }
            //业务时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //创建人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            //检查类型

            if (this.txtAccepTypeBill.SelectedItem != null)
            {
                int i = 0;
                if (this.txtAccepTypeBill.Text == "专业检查")
                {
                    i = 1;
                }
                if (this.txtAccepTypeBill.Text == "工程日常检查")
                {
                    i = 0;
                }
                objectQuery.AddCriterion(Expression.Eq("InspectionType", i));
            }

            //供货商
            if (this.txtSuppilerBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("SupplierUnitName", txtSuppilerBill.Text, MatchMode.Anywhere)); ;
            }
            if (comMngType.Text != "")
            {
                System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                //int values = ClientUtil.ToInt(li.Value);
                objectQuery.AddCriterion(Expression.Eq("DocState", (DocumentState)Convert.ToInt32(li.Value)));
            }
            try
            {
                list = model.RectificationNoticeSrv.GetRectificationNotice(objectQuery);
                dgDetail.Rows.Clear();
                dgMaster.Rows.Clear();
                dgDetailBill.Rows.Clear();
                ShowMasterList(list);
                FlashScreen.Close();
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
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (RectificationNoticeMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;
                int strType = master.InspectionType;   //检查类型
                if (strType ==0)
                {
                    dgMaster[colInsTypeBill.Name, rowIndex].Value = "工程日常检查";
                }
                if (strType == 1)
                {
                    dgMaster[colInsTypeBill.Name, rowIndex].Value = "专业检查";
                }
                dgMaster[colInsSupplierBill.Name, rowIndex].Value = master.SupplierUnitName; //受检承担单位
                dgMaster[colCreateDateMaster.Name, rowIndex].Value = master.CreateDate.ToShortDateString();//业务日期
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.RealOperationDate.ToString();   //制单日期
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;  //制单人
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);   //状态
                dgMaster[colDocument.Name, rowIndex].Value = "查看文档";
            }
            lblRecordTotalBill.Text = "共【" + dgMaster.Rows.Count + "】条记录";
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetailBill.Rows.Clear();
            RectificationNoticeMaster master = dgMaster.CurrentRow.Tag as RectificationNoticeMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl;
            foreach (RectificationNoticeDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colInsHandlePersonBill.Name, rowIndex].Value = master.HandlePersonName;                            //受检管理责任者
                dgDetailBill[colInsQuestionCodeBill.Name, rowIndex].Value = dtl.ProblemCode;                          //问题代码
                dgDetailBill[colInsQuestionBill.Name, rowIndex].Value = dtl.QuestionState;                               //存在问题说明
                dgDetailBill[colInsRequiredBill.Name, rowIndex].Value = dtl.Rectrequired;                                        //整改要求
                dgDetailBill[colInsResultBill.Name, rowIndex].Value = dtl.RectContent;                                     //整改措施和结果说明
                if (dtl.RequiredDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgDetailBill[colInsCompleteDateBill.Name, rowIndex].Value = dtl.RequiredDate.ToShortDateString();                          //要求整改完成时间
                }
                if(dtl.RectSendDate.ToShortDateString()!="")
                {
                    if (dtl.RectSendDate > ClientUtil.ToDateTime("1900-1-1"))
                    {
                        dgDetailBill[colInsSendDateBill.Name, rowIndex].Value = dtl.RectSendDate.ToShortDateString();                          //整改通知下发时间  
                    }
                }
                int rec = dtl.RectConclusion;
                if (rec == 0)
                {
                    dgDetailBill[colInsConclusionBill.Name, rowIndex].Value = "整改中";  //整改结论  
                }
                if (rec == 1)
                {
                    dgDetailBill[colInsConclusionBill.Name, rowIndex].Value = "整改未通过"; //整改结论  
                }
                if (rec == 2)
                {
                    dgDetailBill[colInsConclusionBill.Name, rowIndex].Value = "整改通过"; //整改结论  
                }
                if (dtl.RectDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgDetailBill[colInsConcluDateBill.Name, rowIndex].Value = dtl.RectDate.ToShortDateString();     //整改结论时间
                }
                dgDetailBill.Rows[rowIndex].Tag = dtl;
            }
        }

    }
}
