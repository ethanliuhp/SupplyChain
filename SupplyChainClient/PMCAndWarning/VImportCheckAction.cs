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
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;

using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using ImportIntegration;
using System.IO;
using System.Diagnostics;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Domain;
using System.Data.OleDb;


namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning
{
    public partial class VImportCheckAction : TBasicDataView
    {

        CurrentProjectInfo projectInfo = null;

        public MPMCAndWarning model = new MPMCAndWarning();

        public VImportCheckAction()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            foreach (string mode in Enum.GetNames(typeof(StateCheckTriggerMode)))
            {
                colActionTriggerMode.Items.Add(mode);
            }
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnBrownExcel.Click += new EventHandler(btnBrownExcel_Click);
            btnLoadData.Click += new EventHandler(btnLoadData_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnDeleteTarget.Click += new EventHandler(btnDeleteTarget_Click);
            btnSave.Click += new EventHandler(btnSave_Click);

            btnAddCheckAction.Click += new EventHandler(btnAddCheckAction_Click);
            btnAddTarget.Click += new EventHandler(btnAddTarget_Click);

            gridCheckAction.CellClick += new DataGridViewCellEventHandler(gridCheckAction_CellClick);
            gridCheckAction.CellValidating += new DataGridViewCellValidatingEventHandler(gridCheckAction_CellValidating);
            gridCheckAction.CellEndEdit += new DataGridViewCellEventHandler(gridCheckAction_CellEndEdit);


            gridTarget.CellEndEdit += new DataGridViewCellEventHandler(gridTarget_CellEndEdit);
        }

        void btnAddCheckAction_Click(object sender, EventArgs e)
        {
            int index = gridCheckAction.Rows.Add();
            DataGridViewRow row = gridCheckAction.Rows[index];

            StateCheckAction checkAction = new StateCheckAction();
            row.Tag = checkAction;

            row.Cells[colActionTriggerMode.Name].Value = StateCheckTriggerMode.定时.ToString();
            gridCheckAction.CurrentCell = row.Cells[colActionName.Name];
            gridTarget.Rows.Clear();
        }

        void btnAddTarget_Click(object sender, EventArgs e)
        {
            if (gridCheckAction.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个检查动作！");
                return;
            }
            if (gridCheckAction.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能选择一个检查动作！");
                return;
            }

            StateCheckAction checkAction = gridCheckAction.SelectedRows[0].Tag as StateCheckAction;

            int index = gridTarget.Rows.Add();
            DataGridViewRow row = gridTarget.Rows[index];

            WarningTarget target = new WarningTarget();
            target.CheckAction = checkAction;
            checkAction.ListTargets.Add(target);
            row.Tag = target;

            gridTarget.CurrentCell = row.Cells[colTargetName.Name];
        }


        void gridCheckAction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            StateCheckAction checkAction = gridCheckAction.Rows[e.RowIndex].Tag as StateCheckAction;

            LoadWarningTargetInGrid(checkAction);
        }

        void gridCheckAction_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            object value = e.FormattedValue;
            if (value != null)
            {
                try
                {
                    string colName = gridCheckAction.Columns[e.ColumnIndex].Name;
                    if (colName == colActionTriggerTerm1.Name
                        || colName == colActionTriggerTerm3.Name)//触发条件
                    {
                        if (value.ToString() != "")
                            ClientUtil.ToDecimal(value);
                    }
                    else if (colName == colActionTriggerTerm2.Name)//触发条件
                    {
                        if (value.ToString() != "")
                            DateTime.Parse(DateTime.Now.ToShortDateString() + " " + value.ToString().Trim());
                    }
                }
                catch
                {
                    MessageBox.Show("输入格式不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        void gridCheckAction_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            object tempValue = gridCheckAction.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            string value = "";
            if (tempValue != null)
                value = tempValue.ToString().Trim();

            StateCheckAction checkAction = gridCheckAction.Rows[e.RowIndex].Tag as StateCheckAction;
            string colName = gridCheckAction.Columns[e.ColumnIndex].Name;

            if (colName == colActionName.Name)
            {
                checkAction.ActionName = value;
            }
            else if (colName == colActionDesc.Name)
            {
                checkAction.ActionDesc = value;
            }
            else if (colName == colActionTriggerMode.Name)
            {
                try
                {
                    checkAction.TriggerMode = VirtualMachine.Component.Util.EnumUtil<StateCheckTriggerMode>.FromDescription(value);
                }
                catch
                {

                }
            }
            else if (colName == colActionTriggerTerm1.Name)
            {
                decimal termValue = 0;
                try
                {
                    if (!string.IsNullOrEmpty(value))
                        termValue = ClientUtil.ToDecimal(value);
                }
                catch { }

                checkAction.TriggerTerm1 = termValue;
            }
            else if (colName == colActionTriggerTerm2.Name)
            {
                checkAction.TriggerTerm2 = value;
            }
            else if (colName == colActionTriggerTerm3.Name)
            {
                int termValue = 0;
                try
                {
                    if (!string.IsNullOrEmpty(value))
                        termValue = Convert.ToInt32(value);
                }
                catch { }

                checkAction.TriggerTerm3 = termValue;
            }
            else if (colName == colActionStartMode.Name)
            {
                checkAction.StartMethod = value;
            }

            gridCheckAction.Rows[e.RowIndex].Tag = checkAction;
        }


        void gridTarget_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            object tempValue = gridTarget.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            string value = "";
            if (tempValue != null)
                value = tempValue.ToString().Trim();

            WarningTarget target = gridTarget.Rows[e.RowIndex].Tag as WarningTarget;
            string colName = gridTarget.Columns[e.ColumnIndex].Name;

            if (colName == colTargetCode.Name)
            {
                target.TargetCode = value;
            }
            else if (colName == colTargetName.Name)
            {
                target.TargetName = value;
            }
            else if (colName == colTargetDesc.Name)
            {
                target.TargetDesc = value;
            }
            else if (colName == colTargetCate.Name)
            {
                target.TargetCate = value;
            }

            gridTarget.Rows[e.RowIndex].Tag = target;

        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridCheckAction.Rows.Count == 0 || gridCheckAction.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的行！");
                return;
            }

            if (MessageBox.Show("您确认要删除吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            List<int> listRowIndex = new List<int>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (DataGridViewRow row in gridCheckAction.SelectedRows)
            {
                StateCheckAction checkAction = row.Tag as StateCheckAction;
                if (!string.IsNullOrEmpty(checkAction.Id))
                    dis.Add(Expression.Eq("Id", checkAction.Id));

                listRowIndex.Add(row.Index);
            }
            oq.AddCriterion(dis);

            IList list = model.ObjectQuery(typeof(StateCheckAction), oq);
            if (list.Count > 0)
            {
                model.DeleteObjList(list);
            }

            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridCheckAction.Rows.RemoveAt(listRowIndex[i]);
            }
            gridTarget.Rows.Clear();
        }

        void btnDeleteTarget_Click(object sender, EventArgs e)
        {
            if (gridTarget.Rows.Count == 0 || gridTarget.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的行！");
                return;
            }

            if (MessageBox.Show("您确认要删除吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            List<int> listRowIndex = new List<int>();

            //ObjectQuery oq = new ObjectQuery();
            //Disjunction dis = new Disjunction();
            foreach (DataGridViewRow row in gridTarget.SelectedRows)
            {
                WarningTarget target = row.Tag as WarningTarget;
                //if (!string.IsNullOrEmpty(target.Id))
                //    dis.Add(Expression.Eq("Id", target.Id));

                target.CheckAction.ListTargets.Remove(target);

                listRowIndex.Add(row.Index);
            }
            //oq.AddCriterion(dis);

            //IList list = model.ObjectQuery(typeof(WarningTarget), oq);
            //if (list.Count > 0)
            //{
            //    model.DeleteObjList(list);
            //}

            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridTarget.Rows.RemoveAt(listRowIndex[i]);
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            IList listAction = new ArrayList();
            foreach (DataGridViewRow row in gridCheckAction.Rows)
            {
                StateCheckAction checkAction = row.Tag as StateCheckAction;
                listAction.Add(checkAction);
            }

            listAction = model.SaveOrUpdate(listAction);

            LoadCheckActionInGrid(listAction);

            MessageBox.Show("保存成功！");
        }

        void btnLoadData_Click(object sender, EventArgs e)
        {
            string filePath = txtExcelFilePath.Text.Trim();

            if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
            {
                MessageBox.Show("请选择一个Excel文件！");
                return;
            }

            string ConnectionString = string.Empty;

            if (Path.GetExtension(filePath).ToLower() == ".xls")
                ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
            else
                ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

            OleDbConnection conpart = new OleDbConnection(ConnectionString);
            conpart.Open();

            DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            List<StateCheckAction> listCheckAction = new List<StateCheckAction>();
            //导入数据
            for (int i = 0; i < tables.Rows.Count; i++)
            {
                DataRow row = tables.Rows[i];

                string tableName = row["TABLE_NAME"].ToString().Trim();

                string sqlStr = "select * from [" + tableName + "]";
                OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, conpart);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (tableName == "状态检查动作$")
                {
                    if (listCheckAction.Count > 0)
                        continue;

                    foreach (DataRow dataRow in dt.Rows)
                    {
                        StateCheckAction checkAction = new StateCheckAction();

                        string id = dataRow["ID"].ToString().Trim();//代码
                        string name = dataRow["状态检查动作名称"].ToString().Trim();
                        string term1 = dataRow["触发条件一"].ToString().Trim();
                        string term2 = dataRow["触发条件二"].ToString().Trim();
                        string term3 = dataRow["触发条件三"].ToString().Trim();
                        string triggerMode = dataRow["触发方式"].ToString().Trim();
                        string startMethod = dataRow["启动方式"].ToString().Trim();

                        checkAction.TempID = id;
                        checkAction.ActionName = name;
                        try
                        {
                            if (!string.IsNullOrEmpty(term1))
                                checkAction.TriggerTerm1 = ClientUtil.ToDecimal(term1);
                            if (!string.IsNullOrEmpty(term2))
                                checkAction.TriggerTerm2 = term2;
                            if (!string.IsNullOrEmpty(term3))
                                checkAction.TriggerTerm3 = Convert.ToInt32(term3);
                        }
                        catch { }

                        try
                        {
                            if (!string.IsNullOrEmpty(triggerMode))
                                checkAction.TriggerMode = (StateCheckTriggerMode)Convert.ToInt32(triggerMode);
                        }
                        catch { }

                        checkAction.StartMethod = startMethod;

                        listCheckAction.Add(checkAction);
                    }

                    i = 0;
                }
                else if (tableName == "预警指标$")
                {
                    if (listCheckAction.Count == 0)
                        continue;

                    foreach (DataRow dataRow in dt.Rows)
                    {
                        string ParentID = dataRow["ParentID"].ToString().Trim().ToUpper();//代码
                        var query = from ac in listCheckAction
                                    where ac.TempID == ParentID
                                    select ac;

                        if (query.Count() > 0)
                        {
                            StateCheckAction checkAction = query.ElementAt(0);
                            WarningTarget target = new WarningTarget();

                            string cate = dataRow["预警指标分类"].ToString().Trim();
                            string code = dataRow["预警指标代码"].ToString().Trim();
                            string name = dataRow["预警指标名称"].ToString().Trim();
                            string desc = dataRow["预警指标描述"].ToString().Trim();


                            target.TargetCate = cate;
                            target.TargetCode = code;
                            target.TargetName = name;
                            target.TargetDesc = desc;

                            target.CheckAction = checkAction;
                            checkAction.ListTargets.Add(target);
                        }
                    }
                }
            }

            LoadCheckActionInGrid(listCheckAction);
        }

        private void LoadCheckActionInGrid(IList list)
        {
            gridTarget.Rows.Clear();
            gridCheckAction.Rows.Clear();

            if (list == null || list.Count == 0)
                return;

            foreach (StateCheckAction action in list)
            {
                int index = gridCheckAction.Rows.Add();
                DataGridViewRow rowAction = gridCheckAction.Rows[index];

                rowAction.Cells[colActionName.Name].Value = action.ActionName;
                rowAction.Cells[colActionDesc.Name].Value = action.ActionDesc;
                rowAction.Cells[colActionTriggerMode.Name].Value = action.TriggerMode.ToString();

                rowAction.Cells[colActionTriggerTerm1.Name].Value = StaticMethod.DecimelTrimEnd0(action.TriggerTerm1);
                rowAction.Cells[colActionTriggerTerm2.Name].Value = action.TriggerTerm2;
                rowAction.Cells[colActionTriggerTerm3.Name].Value = action.TriggerTerm3;
                rowAction.Cells[colActionStartMode.Name].Value = action.StartMethod;

                rowAction.Tag = action;
            }

            if (gridCheckAction.Rows.Count > 0)
            {
                gridCheckAction.CurrentCell = gridCheckAction.Rows[0].Cells[0];
                LoadWarningTargetInGrid(gridCheckAction.Rows[0].Tag as StateCheckAction);
            }
        }

        private void LoadWarningTargetInGrid(StateCheckAction action)
        {
            gridTarget.Rows.Clear();

            if (action == null || action.ListTargets.Count == 0)
                return;

            foreach (WarningTarget target in action.ListTargets)
            {
                int index1 = gridTarget.Rows.Add();
                DataGridViewRow rowTarget = gridTarget.Rows[index1];

                rowTarget.Cells[colTargetCode.Name].Value = target.TargetCode;
                rowTarget.Cells[colTargetName.Name].Value = target.TargetName;
                rowTarget.Cells[colTargetDesc.Name].Value = target.TargetDesc;
                rowTarget.Cells[colTargetCate.Name].Value = target.TargetCate;

                rowTarget.Tag = target;
            }
        }

        void btnBrownExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                txtExcelFilePath.Text = filePath;
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            if (txtActionName.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("ActionName", txtActionName.Text.Trim(), MatchMode.Anywhere));
            }
            if (txtActionDesc.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("ActionDesc", txtActionDesc.Text.Trim(), MatchMode.Anywhere));
            }
            oq.AddFetchMode("ListTargets", NHibernate.FetchMode.Eager);

            IList list = model.ObjectQuery(typeof(StateCheckAction), oq);

            LoadCheckActionInGrid(list);

        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.AddNew:


                    break;

                case MainViewState.Modify:


                    break;

                case MainViewState.Browser:


                    break;

                case MainViewState.Initialize://添加根节点


                    break;
            }

            ViewState = state;
        }

        public override bool ModifyView()
        {

            return false;
        }

        public override bool CancelView()
        {
            try
            {

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        public override void RefreshView()
        {
            try
            {

            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            bool isNew = false;
            try
            {
                //if (!ValideSave())
                //    return false;



                RefreshControls(MainViewState.Browser);

                return true;
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null && exp.InnerException.Message.Contains("违反唯一约束条件"))
                    MessageBox.Show("编码必须唯一！");
                else
                    MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(exp));
            }
            return false;
        }

    }
}
