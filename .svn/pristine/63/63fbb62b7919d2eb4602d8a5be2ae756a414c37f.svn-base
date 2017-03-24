using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using VirtualMachine.Component.Util;

/*
    思路
 * 1. 加载分包项目
 * 2. 加载项目任务划分列表
 * 3. 选择项目任务时，加载对应的任务明细
 * 4. 鼠标双击任务明细时，将该明细记录加入分工表
 * 5. 保存分包项目的任务划分情况
 * 6. 选择分包项目时，加载对应的任务划分情况列表
    约定说明
 * colSign：存储id
 * colName：存储是否新增
*/

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OBS
{
    public partial class VOBSService : TBasicDataView
    {
        // 实例化一些可能会用到的对象
        public MOBS OBSModel = new MOBS();              // 
        CurrentProjectInfo projectInfo;                 // 当前用户所属项目
        public MGWBSTree model;                         // 用于对树的操作
        private TreeNode rootNode = null;               // 根节点
        private SubContractProject curProject;          // 当前选择的分包项目
        /// <summary>
        /// 根据WBSTree的id，列出其中的明细内容
        /// </summary>
        private Dictionary<string, List<GWBSDetail>> _detailList = new Dictionary<string, List<GWBSDetail>>();
        /// <summary>
        /// 已选择的节点对应的任务明细
        /// </summary>
        private List<GWBSDetail> _selectTask = new List<GWBSDetail>();
        /// <summary>
        /// 被删除的原始数据
        /// </summary>
        private List<DataGridViewRow> oldRemoveList = new List<DataGridViewRow>();
        /// <summary>
        /// 当前选择的节点
        /// </summary>
        private TreeNode curNode;
        /// <summary>
        /// 使用键值对的方式，存储所有的树节点
        /// </summary>
        private Dictionary<string, TreeNode> _listNode = new Dictionary<string, TreeNode>();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mot"></param>
        public VOBSService(MGWBSTree mot)
        {
            InitializeComponent();
            model = mot;
            Init();
        }

        #region 初始化

        /// <summary>
        /// 基础数据初始化
        /// </summary>
        private void Init()
        {
            projectInfo = StaticMethod.GetProjectInfo();    // 读取当前用户所在项目
            TableServerLoad();                              // 加载项目分包队伍
            TreeTaskLoad();                                 // 加载项目主要任务划分树
            // 定义事件
            treeProject.AfterSelect += new TreeViewEventHandler(TreeProject_AfterSelect);
            tableServer.CellClick += new DataGridViewCellEventHandler(TableServer_CellClick);
            // 按钮事件
            btnDetailAll.Click += (a, b) => RowSelect(tableDetail, true);
            btnDetailReverse.Click += (a, b) => RowSelect(tableDetail, false);
            btnAdd.Click += (a, b) => AddTaskDetail(tableDetail);
            btnClear.Click += (a, b) => Clear();
            btnDelete.Click += (a, b) => Clear();
        }

        /// <summary>
        /// 加载分包队伍信息
        /// </summary>
        private void TableServerLoad()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = OBSModel.ContractExcuteSrv.GetContractExcute(oq);
            foreach (SubContractProject subject in list)
            {
                if (subject.BearerOrgName != null)
                {
                    int i = tableServer.Rows.Add();
                    tableServer[colServerName.Name, i].Value = subject.BearerOrgName;
                    tableServer[colContractName.Name, i].Value = subject.ContractGroupCode;
                    tableServer.Rows[i].Tag = subject;
                }
            }
        }

        /// <summary>
        /// 加载项目主要任务划分树
        /// </summary>
        private void TreeTaskLoad()
        {
            treeProject.Nodes.Clear();      // 清空树
            tableDetail.Rows.Clear();       // 清空明细
            Hashtable hashtable = new Hashtable();
            IList list = model.GetGWBSTreesByInstance(projectInfo.Id);
            foreach (GWBSTree childNode in list)
            {
                TreeNode tn = new TreeNode();
                tn.Name = childNode.Id.ToString();
                tn.Text = childNode.Name + (childNode.IsSetup ? "√" : "");
                tn.Tag = childNode;
                if (childNode.ParentNode != null)
                {
                    TreeNode tnp = null;
                    tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                    if (tnp != null)
                        tnp.Nodes.Add(tn);
                }
                else
                {
                    // 没有父节点，则该节点为树的根节点
                    treeProject.Nodes.Add(tn);
                    rootNode = tn;
                    rootNode.Expand();
                }

                hashtable.Add(tn.Name, tn);
                _listNode.Add(childNode.Id, tn);
            }
        }

        #endregion

        #region 事件函数

        /// <summary>
        /// 选择tree中的项目任务后，加载明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeProject_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown) return;
            if (curNode != null)
            {
                curNode.BackColor = Color.White;
                curNode.ForeColor = Color.Black;
            }
            SelectNode(e.Node.Tag as GWBSTree);                                 // 执行选择node的
            e.Node.BackColor = Color.Black;
            e.Node.ForeColor = Color.White;
            curNode = e.Node;
            LoadDetail();                                                       // 重新加载明细列表
        }


        /// <summary>
        /// 点击单元格后选择整行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var target = sender as DataGridView;
            target.Rows[e.RowIndex].Selected = true;
        }

        /// <summary>
        /// 选择分包项目后，加载对应的分工任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableServer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                curProject = null;
            }
            else
            {
                curProject = tableServer.Rows[e.RowIndex].Tag as SubContractProject;    // 记录当前选择的项目
            }

            // 文本框跟踪显示
            txtGroup.Text = curProject.BearerOrgName;
        }

        #endregion

        #region 功能函数

        /// <summary>
        /// 当选择树中的某任务节点时，查找该任务下的所有明细
        /// </summary>
        /// <param name="curTask">当前任务</param>
        private void SelectNode(GWBSTree curTask)
        {
            if (curTask == null) return;
            _selectTask.Clear();
            ObjectQuery oq = new ObjectQuery();                                 // 搜索条件
            oq.AddCriterion(Expression.Like("SysCode", curTask.SysCode + "%")); // 查找出该任务节点下的所有子任务(包含本身)
            oq.AddCriterion(Expression.Eq("State", 1));                         // 任务状态

            IList childList = model.ObjectQuery(typeof(GWBSTree), oq);          // 得到子任务列表
            foreach (GWBSTree item in childList)                                // 遍历任务列表
            {
                List<GWBSDetail> detailList;                                    // 声明变量，获取任务明细
                if (_detailList.Keys.Contains(item.Id))                         // 如果存在指定任务的明细，则直接获取
                {
                    detailList = _detailList[item.Id];
                }
                else
                {                                                               // 如果不存在则查找数据库
                    detailList = model.GetWBSDetail(item.Id, "");               // 得到子任务明细
                    if (detailList != null)
                    {
                        detailList = detailList.Select(a =>                     // 将任务信息写入明细对象中
                        {
                            a.TheGWBS = item;
                            return a;
                        }).ToList();
                    }
                    _detailList.Add(item.Id, detailList);                       // 将任务对应的明细信息存入字典
                }
                _selectTask.AddRange(detailList);                               // 将明细加入已选择列表中
            }
        }

        /// <summary>
        /// 加载任务明细
        /// </summary>
        private void LoadDetail()
        {
            tableDetail.Rows.Clear();
            var list = model.SearchSQL(string.Format("select gwbsdetail id,materialfeesettlementflag flag from THD_OBSService where projectid='{0}'", projectInfo.Id)).Tables[0].Select().Select(row => new
            {
                Id = row["id"] + "",
                Flag = row["flag"] + "" == "0" ? "不结算" : "结算"
            });
            foreach (var item in _selectTask)
            {
                var index = tableDetail.Rows.Add();
                tableDetail.Rows[index].Tag = item;
                tableDetail.Rows[index].Cells[colDetailCode.Name].Value = item.TheCostItem.QuotaCode;
                tableDetail.Rows[index].Cells[colDetailName.Name].Value = item.Name;
                tableDetail.Rows[index].Cells[colDetailAmount.Name].Value = item.PlanWorkAmount;
                var flagItem = list.FirstOrDefault(a => a.Id == item.Id);
                tableDetail.Rows[index].Cells[colDetailSign.Name].Value = flagItem == null ? "不结算" : flagItem.Flag;
                tableDetail.Rows[index].Cells[colGroup.Name].Value = item.ContractProjectName;
                tableDetail.Rows[index].Cells[this.colContractGroupName.Name].Value = GetContractGroupName(item.Id);
            }
        }

        private string GetContractGroupName(string cpid)
        {
            string sql = string.Format(@"select  t2.contractgroupcode
                                            from thd_gwbsdetail t1 left join thd_subcontractproject t2 
                                            on t1.contractproject  = t2.id 
                                            where t2.projectid =  '{0}' 
                                            and  t1.id = '{1}'", projectInfo.Id, cpid);
            DataTable dt = model.SearchSQL(sql).Tables[0];
            if (dt.Rows.Count < 1)
                return "";
            return ClientUtil.ToString(dt.Rows[0]["contractgroupcode"]); 
        }
        /// <summary>
        /// 将任务明细列表中选中的栏目加入分包项目列表
        /// </summary>
        private void AddTaskDetail(DataGridView table)
        {
            if (curProject == null)
            {
                MessageBox.Show("请先选择分包队伍...");
                return;
            }
            // 选中项目验证
            List<DataGridViewRow> hookList = new List<DataGridViewRow>();

            foreach (DataGridViewRow item in table.Rows)
            {
                if (item.Cells[0].Value == null || !(bool)item.Cells[0].Value) continue;
                hookList.Add(item);
            }
            if (hookList.Count == 0) return;
            var setupedList = hookList.Select(a => a.Tag as GWBSDetail).Where(a => !string.IsNullOrEmpty(a.ContractProjectName)).ToList();
            if (setupedList.Count > 0)
            {
                MessageBox.Show(string.Format("任务[{0}]已分配给队伍[{1}]...", setupedList[0].Name, setupedList[0].ContractProjectName));
                return;
            }

            // 将任务明细绑定当前选择的队伍
            Save(curProject, hookList);
            // 
            curProject = null;
            txtGroup.Text = "";
            tableServer.SelectedRows[0].Selected = false;
        }

        private void Save(SubContractProject project, IEnumerable<DataGridViewRow> list)
        {
            // 记录树节点新增明细的任务节点ID
            List<string> ids = new List<string>();
            foreach (DataGridViewRow row in list)
            {
                var obj = new OBSService();
                obj.SupplierId = curProject as SubContractProject;
                obj.SupplierName = curProject.BearerOrgName;
                obj.ServiceType = "劳务分包";        // 专业分包、劳务分包
                obj.ServiceState = "编辑";           // 编辑、执行、作废
                obj.BeginDate = new DateTime(2016, 8, 1);
                obj.EndDate = new DateTime(2016, 8, 1);
                obj.CreatePerson = ConstObject.LoginPersonInfo;
                obj.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                obj.CreateDate = ConstObject.LoginDate;
                obj.CreateYear = ConstObject.LoginDate.Year;
                obj.CreateMonth = ConstObject.LoginDate.Month;
                obj.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                obj.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                obj.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                obj.ProjectId = projectInfo.Id;
                obj.ProjectName = projectInfo.Name;
                obj.WBSDetail = row.Tag as GWBSDetail;
                obj.GWBSDetailName = obj.WBSDetail.Name;
                obj.MaterialFeeSettlementFlag = row.Cells[colDetailSign.Name].Value.ToString() == "结算" ? "1" : "0";
                obj.PlanNedQuantity = obj.WBSDetail.PlanWorkAmount;
                obj.QuotaCode = obj.WBSDetail.TheCostItem.QuotaCode;
                obj.FullPath = obj.WBSDetail.TheGWBS.FullPath;
                obj.ProjectTask = obj.WBSDetail.TheGWBS;
                obj.ProjectTaskName = obj.ProjectTask.Name;
                obj.ProjectTaskCode = obj.ProjectTask.SysCode;
                OBSModel.OBSSrv.SaveOBSService(obj);

                // 将分包队伍关联指定的任务明细
                AddGroupRelativeWBS(obj.SupplierId, obj.WBSDetail);
                // 更细列表的分包队伍
                row.Cells[colGroup.Name].Value = project.BearerOrgName;

                row.Cells[this.colContractGroupName.Name].Value = project.ContractGroupCode;
                // 记录id
                ids.Add(obj.WBSDetail.TheGWBS.Id);

            }
            if (ids.Count > 0)
            {
                UpdateTree(true, ids.ToArray());
            }
        }


        /// <summary>
        /// 清除选中项的绑定队伍
        /// </summary>
        private void Clear()
        {
            if (MessageBox.Show("是否确定解除指定的绑定？", "提示", MessageBoxButtons.YesNo) == DialogResult.No) return;
            //string sql = "delete thd_OBSService where projectid='{0}' and gwbsdetail in ({1})";
            //string sql = "begin delete thd_OBSService where projectid='{0}' and gwbsdetail in ({1}); delete thd_gwbstaskconfirmdetail where  gwbsdetail in ({1}); end;";
            string sql = "begin delete thd_OBSService where projectid='{0}' and gwbsdetail in ({1});  update thd_gwbstaskconfirmdetail set taskhandler = null,taskhandlername = null where  gwbsdetail in ({1}); end;";
            StringBuilder sb = new StringBuilder();
            // 存储需要清除的记录
            List<DataGridViewRow> clearRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in tableDetail.Rows)
            {
                if (row.Cells[0].EditedFormattedValue == null || !(bool)row.Cells[0].EditedFormattedValue || row.Cells[colGroup.Name].Value + "" == "") continue;
                var item = row.Tag as GWBSDetail;
                //if (item.QuantityConfirmed > 0)
                //{
                //    // 如果工长已确认工程量，则不允许解除绑定
                //    MessageBox.Show("任务[" + item.Name + "]已经确认工程量，不允许解除绑定！");
                //    return;
                //}
                sb.Append("'" + item.Id + "',");
                clearRows.Add(row);
            }
            if (sb.ToString() == string.Empty) return;
            var ids = sb.Remove(sb.Length - 1, 1).ToString();
            // 删除绑定
            sql = string.Format(sql, projectInfo.Id, ids);
            CommonMethod.CommonMethodSrv.InsertData(sql);
            // 清空绑定队伍
            foreach (var row in clearRows)
            {
                var item = row.Tag as GWBSDetail;
                row.Cells[colGroup.Name].Value = "";
                row.Cells[this.colContractGroupName.Name].Value = "";
                item.ContractProjectName = "";
            }
            // 移除关系
            RemoveGroupRelativeWBS(ids);
        }

        /*
         * 1. GWBSDetail类中添加字段：ContractProject分包队伍，ContractProjectName分包队伍名称
         * 2. GWBSTree类中添加字段：IsSetup任务明细是否已设置分包队伍
         * 3. 添加关联关系
         * 4. 移除关联关系
         
        */

        /// <summary>
        /// 设置任务明细与分包队伍的关联
        /// </summary>
        private void AddGroupRelativeWBS(SubContractProject supplier, GWBSDetail wbs)
        {
            // 保存任务明细
            CommonMethod.CommonMethodSrv.InsertData(string.Format("update thd_gwbsdetail set ContractProject='{0}',ContractProjectName='{1}' where id='{2}'", supplier.Id, supplier.BearerOrgName, wbs.Id));
            CommonMethod.CommonMethodSrv.InsertData(string.Format("update thd_gwbstaskconfirmdetail set taskhandler='{0}',taskhandlername='{1}' where gwbsdetail='{2}'", supplier.Id, supplier.BearerOrgName, wbs.Id));
            wbs.ContractProjectName = supplier.BearerOrgName;
            // 保存工程任务
            if (wbs.TheGWBS.IsSetup) return;
            wbs.TheGWBS.IsSetup = true;
            OBSModel.OBSSrv.SaveOrUpdateByDao(wbs.TheGWBS);
        }

        /// <summary>
        /// 设置任务明细与分包队伍的关联
        /// </summary>
        private void RemoveGroupRelativeWBS(string ids)
        {
            // 删除所有的关联关系
            CommonMethod.CommonMethodSrv.InsertData(string.Format("update thd_gwbsdetail set ContractProject=null,ContractProjectName=null where id in ({0})", ids));
            /* 检查任务节点
                1. 查询出所有删除的任务明细的父节点
             *  2. 查询父节点下所有的子节点
             *  3. 判断所有子节点是否有仍存在分包关系的，将该任务节点排除
             *  4. 将其他的任务节点的IsSetup设置为false
             *  5. 重置树节点状态
            */
            // 1、2
            var table = CommonMethod.CommonMethodSrv.GetData(string.Format("select ParentId, ContractProject from thd_gwbsdetail where parentid in (select id from thd_gwbstree where issetup=1 and id in (select parentid from thd_gwbsdetail where id in ({0}) group by parentid))", ids)).Tables[0];
            // 3
            if (table == null || table.Rows.Count == 0) return;
            var list = table.Select().Select(a => new { ParentId = a[0].ToString(), Group = a[1] + "" });    // 转化为列表
            var groupList = list.GroupBy(a => a.ParentId).Select(a => a.Key).ToList();  // 得到所有的任务id列表
            var existRelative = list.Where(a => a.Group != "").GroupBy(a => a.ParentId).Select(a => a.Key);  // 得到仍存在分包关系的任务id
            groupList.RemoveAll(a => existRelative.Contains(a));
            // 4
            if (groupList.Count == 0) return;
            var condition = CommonPlus.JoinArray(groupList.ToArray());
            CommonMethod.CommonMethodSrv.InsertData(string.Format("update thd_gwbstree set issetup=0 where id in({0})", condition));
            // 5
            UpdateTree(false, groupList.ToArray());

        }

        /// <summary>
        /// 更新树节点
        /// </summary>
        /// <param name="ids"></param>
        private void UpdateTree(bool state, params string[] ids)
        {
            var list = _listNode.Where(a => ids.Contains(a.Key));
            foreach (var item in list)
            {
                var node = item.Value as TreeNode;
                if (state)
                {
                    node.Text = (node.Tag as GWBSTree).Name + "√";
                }
                else
                {
                    node.Text = (node.Tag as GWBSTree).Name;
                }
            }
        }

        /// <summary>
        /// 设置DataGridView的选择状态
        /// </summary>
        /// <param name="table"></param>
        /// <param name="sign">如果为true则全选中，否则反选</param>
        private void RowSelect(DataGridView table, bool sign)
        {
            if (sign)
            {
                foreach (DataGridViewRow row in table.Rows)
                {
                    row.Cells[0].Value = true;
                }
            }
            else
            {
                foreach (DataGridViewRow row in table.Rows)
                {
                    row.Cells[0].Value = row.Cells[0].Value == null ? true : !(bool)row.Cells[0].Value;
                }
            }
        }

        #endregion
    }
}
