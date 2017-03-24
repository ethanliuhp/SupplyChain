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
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng
{
    public partial class VProInsRecordQuery : TBasicDataView
    {
        private MProInsRecordMng model = new MProInsRecordMng();
        public VProInsRecordQuery()
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
            this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateBegindg.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnddg.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            this.btnOBS.Click += new EventHandler(btnOBS_Click);
            this.btnOBSdg.Click += new EventHandler(btnOBSdg_Click);
            VBasicDataOptr.InitWBSCheckRequir(txtInspectionSpecail, true);
            VBasicDataOptr.InitWBSCheckRequir(txtInspectionSpecaildg, true);
        }

        //单据显示选择工程任务
        void btnOBS_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtBearDepart.Text = engineerMaster.BearerOrgName;
            txtBearDepart.Tag = engineerMaster;
            txtCodeBegin.Focus();
        }

        //明细显示选择工程任务
        void btnOBSdg_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtBearDepartdg.Text = engineerMaster.BearerOrgName;
            txtBearDepartdg.Tag = engineerMaster;
            txtCodeBegindg.Focus();
        }
        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSearchdg.Click += new EventHandler(btnSearchdg_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            //this.btnExceldg.Click+=new EventHandler(btnExceldg_Click);
            dgMaster.CellClick += new DataGridViewCellEventHandler(dgMaster_CellClick);
            dgMaster.CellDoubleClick += new DataGridViewCellEventHandler(dgMaster_CellDoubleClick);
            dgDetaildg.CellDoubleClick += new DataGridViewCellEventHandler(dgDetaildg_CellDoubleClick);
            dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            dgDetaildg.CellContentClick += new DataGridViewCellEventHandler(dgDetaildg_CellContentClick);
        }
        //打印预览
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                ProfessionInspectionRecordMaster master = dgMaster.Rows[e.RowIndex].Tag as ProfessionInspectionRecordMaster;
                VProInsRecord vmro = new VProInsRecord();
                try
                {
                    int c = master.Details.Count;
                }
                catch
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id",master.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    master = model.ObjectQuery(typeof(ProfessionInspectionRecordMaster), oq)[0] as ProfessionInspectionRecordMaster;
                    dgMaster.Rows[e.RowIndex].Tag = master;
                }
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
            else if (dgvCell.OwningColumn.Name == colDocument.Name)
            {
                ProfessionInspectionRecordMaster master = dgMaster.Rows[e.RowIndex].Tag as ProfessionInspectionRecordMaster;
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
        void dgDetaildg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetaildg[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCodedg.Name)
            {
                ProfessionInspectionRecordMaster master = model.ProfessionInspectionSrv.GetProfessionInspectionRecordByCode(dgvCell.Value.ToString());
                VProInsRecord vmro = new VProInsRecord();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }


        void dgMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                ProfessionInspectionRecordMaster billId = dgMaster.Rows[e.RowIndex].Tag as ProfessionInspectionRecordMaster;
                if (ClientUtil.ToString(billId) != "")
                {
                    VProInsRecord vProInsRe = new VProInsRecord();
                    vProInsRe.Start(billId.Id);
                    vProInsRe.ShowDialog();
                }
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgMaster, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery oq = new ObjectQuery();
            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            //创建人
            if (txtCreatePerson.Text != "")
            {
                oq.AddCriterion(Expression.Like("CreatePersonName", txtCreatePerson.Text, MatchMode.Anywhere));
            }
            if (comMngType.Text != "")
            {
                System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                int values = ClientUtil.ToInt(li.Value);
                //oq.AddCriterion(Expression.Eq("DocState", (DocumentState)values));
            }
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();

            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            if (txtInspectionSpecail.Text != "")
            {
                oq.AddCriterion(Expression.Eq("InspectionSpecail", txtInspectionSpecail.Text));
            }
            //查询字表信息
            ObjectQuery oqDetail = new ObjectQuery();
            if (txtBearDepart.Text != "" && txtBearDepart.Tag != null)
            {
                oqDetail.AddCriterion(Expression.Eq("InspectionSupplier", txtBearDepart.Tag));
                oqDetail.AddCriterion(Expression.Eq("InspectionSupplierName", txtBearDepart.Text));
            }
            IList listDtl = model.ProfessionInspectionSrv.ObjectQuery(typeof(ProfessionInspectionRecordDetail), oqDetail);
            Disjunction dis = new Disjunction();
            if (listDtl.Count > 0)
            {
                IEnumerable<ProfessionInspectionRecordDetail> queryDtl = listDtl.OfType<ProfessionInspectionRecordDetail>();
                var queryMaster = from d in queryDtl
                                  group d by new { d.Master.Id }
                                      into g
                                      select new
                                      {
                                          g.Key.Id
                                      };

                foreach (var parent in queryMaster)
                {
                    dis.Add(Expression.Eq("Id", parent.Id));
                }
            }
            oq.AddCriterion(dis);
            //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            try
            {
                IList list = model.ProfessionInspectionSrv.ObjectQuery(typeof(ProfessionInspectionRecordMaster), oq);
                FillMaster(list);
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询专业检查出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        //主表显示
        private void FillMaster(IList masterList)
        {
            dgMaster.Rows.Clear();
            dgDetial.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (ProfessionInspectionRecordMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                DataGridViewRow dr = dgMaster.Rows[rowIndex];
                dr.Tag = master;
                dr.Cells[colCode.Name].Value = master.Code;
                dr.Cells[colCreateDate.Name].Value = master.RealOperationDate.ToString();
                dr.Cells[colInspectionDate.Name].Value = master.CreateDate.ToShortDateString();
                dr.Cells[colInspectionSpecail.Name].Value = master.InspectionSpecail;
                dr.Cells[colCreatePerson.Name].Value = master.CreatePersonName;
                dr.Cells[colDescript.Name].Value = master.Descript;
                object objState = master.DocState;
                if (objState != null)
                {
                    dr.Cells[colState.Name].Value = ClientUtil.GetDocStateName((master.DocState));
                }
                dr.Cells[colDocument.Name].Value = "查看文档";
            }
            dgMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.CurrentCell = dgMaster.Rows[0].Cells[0];
                dgMaster_CellClick(dgMaster, new DataGridViewCellEventArgs(0, 0));
            }
        }

        void dgMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dr = dgMaster.CurrentRow;
            if (dr == null) return;
            ProfessionInspectionRecordMaster master = dr.Tag as ProfessionInspectionRecordMaster;
            if (master == null) return;
            FillDgDetail(master);
        }
        //明细显示
        private void FillDgDetail(ProfessionInspectionRecordMaster master)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
            oq.AddFetchMode("InspectionSupplier", NHibernate.FetchMode.Eager);

            IList listDtl = model.ProfessionInspectionSrv.ObjectQuery(typeof(ProfessionInspectionRecordDetail), oq);

            dgDetial.Rows.Clear();
            foreach (ProfessionInspectionRecordDetail dtl in listDtl)
            {
                int i = this.dgDetial.Rows.Add();
                DataGridViewRow row = dgDetial.Rows[i];

                row.Cells[colBearPart.Name].Value = ClientUtil.ToString(dtl.InspectionSupplierName);
                row.Cells[colBearPart.Name].Tag = dtl.InspectionSupplier;
                row.Cells[colBearPerson.Name].Value = ClientUtil.ToString(dtl.InspectionPersonName);
                string strName = ClientUtil.ToString(dtl.InspectionConclusion);
                if (strName.Equals("0"))
                {
                    row.Cells[colInspectionConclusion.Name].Value = "检查通过";//检查结论;
                }
                if (strName.Equals("1"))
                {
                    row.Cells[colInspectionConclusion.Name].Value = "检查不通过";//检查结论;
                }
                row.Cells[colInspectionContent.Name].Value = dtl.InspectionContent;
                string strSign = ClientUtil.ToString(dtl.CorrectiveSign);
                if (strSign == "0")
                {
                    row.Cells[colInspectionflag.Name].Value = "不需整改";
                }
                if (strSign == "1")
                {
                    row.Cells[colInspectionflag.Name].Value = "需要整改";
                }
                row.Cells[colLevel.Name].Value = dtl.DangerLevel;
                row.Cells[colType.Name].Value = dtl.DangerType;
                row.Cells[colPart.Name].Value = dtl.DangerPart;

                row.Tag = dtl;
            }
        }
        void dgDetaildg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetaildg.Columns[e.ColumnIndex].Name.Equals(this.colCodedg.Name))
            {
                return;
            }
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetaildg.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VProInsRecord vRN = new VProInsRecord();
                    vRN.Start(billId);
                    vRN.ShowDialog();
                }
            }
        }
        /// <summary>
        /// 明细显示查找按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchdg_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                //condition += "and t1.ProjectName = '" + projectInfo.Name + "'";
                //单据号
                if (this.txtCodeBegindg.Text != "")
                {
                    condition += "and t1.code = '" + txtCodeBegindg.Text + "' and t1.code = '" + (txtCodeBegindg.Tag as SubContractProject).Id + "'";
                }
                //业务日期
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.InspectionDate>='" + dtpDateBegindg.Value.Date.ToShortDateString() + "' and t1.InspectionDate<'" + dtpDateEnddg.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.InspectionDate>=to_date('" + dtpDateBegindg.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.InspectionDate<to_date('" + dtpDateEnddg.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }
                ////制单人
                //if (!txtCreatePersondg.Text.Trim().Equals("") && txtCreatePersondg.Result != null && txtCreatePersondg.Result.Count > 0)
                //{
                //    condition += " and t1.CreatePerson='" + (txtCreatePersondg.Result[0] as PersonInfo).Id + "'";
                //}
                //制单人
                if (txtCreatePersondg.Text != "")
                {
                    condition += "and t1.CreatePersonName = '" + txtCreatePersondg.Text + "'";
                }

                //受检承担单位
                if (txtBearDepartdg.Text != "")
                {
                    condition += "and t2.InspectionSupplierName ='" + txtBearDepartdg.Text + "'";
                }
                //检查专业
                if (txtInspectionSpecaildg.Text != "")
                {
                    condition += "and t1.InspectionSpecail = '" + txtInspectionSpecaildg.Text + "'";
                }
                //状态
                if (comMngType1.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType1.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                #endregion
                DataSet dataSet = model.ProfessionInspectionSrv.ProfessionInspectionRecordQuery(condition);
                this.dgDetaildg.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetaildg.Rows.Add();
                    dgDetaildg[colCodedg.Name, rowIndex].Value = dataRow["code"];
                    DataGridViewRow currRow = dgDetaildg.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["Id"]);
                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetaildg[colStatedg.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    dgDetaildg[colInspectionSpecaildg.Name, rowIndex].Value = dataRow["InspectionSpecail"].ToString();//检查专业
                    //dgDetail[colCostName.Name, rowIndex].Value = dataRow["CostName"];
                    dgDetaildg[colInspectionDatedg.Name, rowIndex].Value = Convert.ToDateTime(dataRow["InspectionDate"]).ToShortDateString();//检查时间
                    dgDetaildg[colCreatePersondg.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();//制单人
                    dgDetaildg[colRealOperationDate.Name, rowIndex].Value = Convert.ToDateTime(dataRow["RealOperationDate"]).ToString();//制单时间
                    dgDetaildg[colBearPartdg.Name, rowIndex].Value = dataRow["InspectionSupplierName"].ToString();//受检承担单位
                    dgDetaildg[colDescriptdg.Name, rowIndex].Value = dataRow["Descript"].ToString();//备注
                    string strName = dataRow["InspectionConclusion"].ToString();//检查结论
                    if (strName.Equals("0"))
                    {
                        dgDetaildg[colInspectionConclusiondg.Name, rowIndex].Value = "检查通过";//检查结论;
                    }
                    if (strName.Equals("1"))
                    {
                        dgDetaildg[colInspectionConclusiondg.Name, rowIndex].Value = "检查不通过";//检查结论;
                    }
                    dgDetaildg[colBearPersondg.Name, rowIndex].Value = dataRow["InspectionPersonName"].ToString();//受检管理责任者
                    dgDetaildg[colInspectionContentdg.Name, rowIndex].Value = dataRow["InspectionContent"].ToString();//检查内容说明
                    string strSign = dataRow["CorrectiveSign"].ToString();//整改标志
                    if (strSign == "0")
                    {
                        dgDetaildg[colInspectionflagdg.Name, rowIndex].Value = "不需整改";
                    }
                    if (strSign == "1")
                    {
                        dgDetaildg[colInspectionflagdg.Name, rowIndex].Value = "需要整改";
                    }

                    dgDetaildg[colTypedg.Name, rowIndex].Value = dataRow["DangerType"].ToString();//隐患类型
                    dgDetaildg[colLeveldg.Name, rowIndex].Value = dataRow["DangerLevel"].ToString();//隐患级别
                    dgDetaildg[colPartdg.Name, rowIndex].Value = dataRow["DangerPart"].ToString();//隐患部位

                }
                FlashScreen.Close();
                this.dgDetaildg.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
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
    }
}
