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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng
{
    public partial class VInspectionRecordQuery : TBasicDataView
    {
        MProductionMng model = new MProductionMng();
        public VInspectionRecordQuery()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        public void InitData()
        {
            comMngType.Items.Clear();
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
                }
            }
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate.AddDays(1);
            VBasicDataOptr.InitWBSCheckRequir(cbWBSCheckRequir, true);
            cbCheckConclusion.Items.AddRange(new object[] { "通过", "不通过" });
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3;
        }
        public void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnSelectWBS.Click += new EventHandler(btnSelectWBS_Click);
            //dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }

        //选择工程任务
        void btnSelectWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                txtWBSName.Tag = (list[0] as TreeNode).Name;
                txtWBSName.Text = (list[0] as TreeNode).Text;
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                #region 定义查询条件
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                if (this.txtWBSName.Text != "")
                {
                    condition = condition + " and t1.GWBSTreeName='" + this.txtWBSName.Text + "'";
                }
                //承担队伍
                if (txtSupply.Text != "" && txtSupply.Result.Count > 0)
                {
                    condition += "and t1.BearTeamName = '" + txtSupply.Text + "'";
                }
                if (dtpDateBegin.Value > ClientUtil.ToDateTime("1900-1-1"))
                {
                    if (StaticMethod.IsUseSQLServer())
                    {
                        condition += " and t1.InspectionDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.InspectionDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                    }
                    else
                    {
                        condition += " and t1.InspectionDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.InspectionDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                    }
                }

                if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
                {
                    condition = condition + " and t1.InspectionPerson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
                }
                if (cbWBSCheckRequir.Text != "")
                {
                    condition = condition + " and t1.InspectionSpecial='" + cbWBSCheckRequir.Text + "'";
                }
                if (cbCheckConclusion.Text != "")
                {
                    condition = condition + " and t1.InspectionConclusion='" + cbCheckConclusion.SelectedItem + "'";
                }
                //if (comMngType.Text != "")
                //{
                //    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                //    int values = ClientUtil.ToInt(li.Value);
                //    condition += "and t1.State= '" + values + "'";
                //}
                #endregion
                DataSet ds = model.ProductionManagementSrv.GetInspectionRecordQuery(condition);
                dgDetail.Rows.Clear();
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail.Rows[rowIndex].Tag = dataRow;
                    dgDetail[colGWBSTree.Name, rowIndex].Value = dataRow["GWBSTreeName"].ToString();
                    dgDetail[colDgMasterTeam.Name, rowIndex].Value = dataRow["BearTeamName"].ToString();
                    dgDetail[colCheckSpecial.Name, rowIndex].Value = dataRow["InspectionSpecial"].ToString();
                    dgDetail[colCheckConclusion.Name, rowIndex].Value = dataRow["InspectionConclusion"].ToString();
                    dgDetail[colContent.Name, rowIndex].Value = dataRow["InspectionContent"].ToString();
                    dgDetail[colCheckStatus.Name, rowIndex].Value = dataRow["InspectionStatus"].ToString();
                    dgDetail[colCheckPerson.Name, rowIndex].Value = dataRow["InspectionPersonName"].ToString();
                    dgDetail[colRecStatus.Name, rowIndex].Value = dataRow["RectificationConclusion"].ToString();
                    dgDetail[colInspectionDate.Name, rowIndex].Value = ClientUtil.ToString(ClientUtil.ToDateTime(dataRow["InspectionDate"]).ToShortDateString());
                    dgDetail[colCreateDate.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();
                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    string strCorrectiveSign = ClientUtil.ToString(dataRow["CorrectiveSign"]);
                    if (strCorrectiveSign.Equals("0"))
                    {
                        dgDetail[colRecStatus.Name, rowIndex].Value = "不需整改";
                    }
                    if (strCorrectiveSign.Equals("1"))
                    {
                        dgDetail[colRecStatus.Name, rowIndex].Value = "需要整改，未进行处理";
                    }
                    if (strCorrectiveSign.Equals("2"))
                    {
                        dgDetail[colRecStatus.Name, rowIndex].Value = "需要整改，已进行处理";
                    }
                }
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
    }
}
