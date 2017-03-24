using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using System.Collections;
using System.IO;
using VirtualMachine.SystemAspect.Security;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng
{
    public partial class VCostItemCategoryAdd : Form
    {
        //public int iMaxCount = 10000;
        private CostItemCategory cate;//选择的分类节点
        private MCostItemCategory model = new MCostItemCategory();
        /// <summary>
        /// 添加成本项分类
        /// </summary>
        /// <param name="c">选择的分类节点</param>
        public VCostItemCategoryAdd(CostItemCategory c)
        {
            InitializeComponent();
            cate = c;
            InitData();
            InitEvent();
            lblText.Text = string.Format(">>[{1}]{0}", cate.Code, cate.Name);
        }

        void InitEvent()
        {
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnAdd.Click += new EventHandler(btnAdd_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnCancel1.Click += new EventHandler(btnCancel1_Click);
            this.btnInsertRows.Click += new EventHandler(btnInsertRows_Click);
        }
        void InitData()
        {
            InitalFlexCell();
        }
        //初始化FlexCell
        void InitalFlexCell()
        {
            flexGrid.DisplayRowNumber = true;
            flexGrid.Rows = 1;
            flexGrid.Cols = 5;
            //flexGrid.Column(0).Visible = false;
            //flexGrid.Column(1).Visible = false;

            //flexGrid.Column(1).Locked = true;
            FlexCell.Cell oCell = flexGrid.Cell(0, 1);
            oCell.Text = "分类代码";

            oCell = flexGrid.Cell(0, 2);
            oCell.Text = "分类名称";

            oCell = flexGrid.Cell(0, 3);
            oCell.Text = "分类说明";
            oCell = flexGrid.Cell(0, 4);
            oCell.Text = "分类类型";

            for (int i = 1; i < 4; i++)
            {
                flexGrid.Column(i).Width = 190;
            }
        }

        //取消
        void btnCancel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //插入行
        void btnInsertRows_Click(object sender, EventArgs e)
        {
            int iRowCount = 0;
            try
            {
                if (txtRowCount.Text.Trim() == "")
                {
                    MessageBox.Show("无法添加;[添加的行数应大于零]");
                    this.txtRowCount.Focus();
                    return;
                }
                else
                {
                    iRowCount = int.Parse(txtRowCount.Text);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.txtRowCount.Focus();
                return;
            }
            if (iRowCount <= 0)
            {
                MessageBox.Show("无法添加;[添加的行数应大于零]");
                this.txtRowCount.Focus();
                return;
            }
            //if (flexGrid.Rows == iMaxCount + 1)
            //{
            //    MessageBox.Show(string.Format("无法添加:[此列表只能批量添加{0}条记录,已经添加了{0}条。]", iMaxCount));
            //    this.txtRowCount.Focus();
            //    return;
            //}
            //if (flexGrid.Rows + iRowCount > iMaxCount + 1)
            //{
            //    MessageBox.Show(string.Format("无法添加:[此列表只能批量添加{0}条记录,此次只能添加{1}条记录。]", iMaxCount, iMaxCount + 1 - flexGrid.Rows));
            //    this.txtRowCount.Focus();
            //    return;
            //}
            flexGrid.AutoRedraw = false;
            for (int i = 0; i < iRowCount; i++)
            {
                flexGrid.InsertRow(flexGrid.Rows, 1);
                flexGrid.Rows += 1;
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
        //添加行
        void btnAdd_Click(object sender, EventArgs e)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.InsertRow(flexGrid.Rows, 1);
            flexGrid.Rows += 1;
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
        private void AddRow()
        {
            flexGrid.InsertRow(flexGrid.Rows, 1);
            flexGrid.Rows += 1;
        }
        //删除
        void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRow(flexGrid.Selection.FirstRow);
        }
        void DeleteRow(int iRow)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.RemoveItem(iRow);
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            if (flexGrid.Rows <= 0)
            {
                MessageBox.Show("无保存数据");
                return;
            }
            int errorIndex = 0;
            try
            {
                if (!ValideView()) return;

                #region 导入数据
                //VirtualMachine.Component.Util.IFCGuidGenerator genObj = new IFCGuidGenerator();//用于生成GUID
                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "errorLog.txt";
                StringBuilder logMsg = new StringBuilder();

                CostItemCategory rootNode = null;

                List<CostItemCategory> listSaveNode = new List<CostItemCategory>();

                listSaveNode.Add(cate);

                ObjectQuery oq = new ObjectQuery();
                if (rootNode == null)
                {
                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();

                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    IList list = model.modelCostItem.ObjectQuery(typeof(CostItemCategory), oq);
                    if (list.Count == 0)
                    {
                        MessageBox.Show("未找到分类根节点.");
                        return;
                    }
                    rootNode = list[0] as CostItemCategory;
                }

                FlexCell.Cell cell = null;
                for (int i = 1; i < flexGrid.Rows; i++)
                {
                    errorIndex = i;
                    cell = flexGrid.Cell(i, 1);
                    string code = cell.Text.Trim();
                    if (code.StartsWith("0") == false)//分类第一位0丢失的情况
                        code = "0" + code;

                    cell = flexGrid.Cell(i, 2);
                    string name = cell.Text;
                    cell = flexGrid.Cell(i, 3);
                    string desc = cell.Text;
                    cell = flexGrid.Cell(i, 4);
                    string cateCode = code.Substring(0, 2);
                    CostItemCategoryTypeEnum cateType = (CostItemCategoryTypeEnum)Convert.ToInt32(cateCode);

                    //if (code.Length == 2)//一级分类,加载到根节点
                    //{

                    //    CostItemCategory childNode = new CostItemCategory();
                    //    childNode.ParentNode = rootNode;

                    //    childNode.Code = code;
                    //    childNode.Name = name;
                    //    childNode.Describe = desc;

                    //    childNode.CategoryState = CostItemCategoryState.制定;
                    //    childNode.TheProjectGUID = rootNode.TheProjectGUID;
                    //    childNode.TheProjectName = rootNode.TheProjectName;

                    //    childNode.OrderNo = model.GetMaxOrderNo(childNode) + 1;

                    //    childNode.CategoryType = cateType;

                    //    listSaveNode.Add(childNode);
                    //}
                    //else
                    //{
                    string parentCode = code.Substring(0, code.Length - 2);

                    var queryParent = from c in listSaveNode
                                      where c.Code == parentCode
                                      select c;

                    if (queryParent.Count() == 0)
                    {
                        logMsg.Append("第" + i + "行,分类代码为" + code + "的分类未找到所属父分类.");
                        logMsg.Append(Environment.NewLine);
                        continue;
                    }

                    CostItemCategory parentNode = queryParent.ElementAt(0);

                    CostItemCategory childNode = new CostItemCategory();

                    //childNode.Id = genObj.GeneratorIFCGuid();
                    childNode.Level = parentNode.Level + 1;
                    //childNode.SysCode = parentNode.SysCode + childNode.Id + ".";
                    childNode.CreateDate = DateTime.Now;
                    childNode.UpdatedDate = DateTime.Now;

                    childNode.ParentNode = parentNode;
                    parentNode.ChildNodes.Add(childNode);

                    childNode.Code = code;
                    childNode.Name = name;
                    childNode.Describe = desc;
                    childNode.CategoryState = CostItemCategoryState.制定;
                    childNode.TheProjectGUID = rootNode.TheProjectGUID;
                    childNode.TheProjectName = rootNode.TheProjectName;
                    childNode.OrderNo = parentNode.ChildNodes.Count;
                    childNode.CategoryType = cateType;

                    listSaveNode.Add(childNode);
                }
                listSaveNode.Remove(cate);
                foreach (CostItemCategory c in listSaveNode)
                {
                    if (c.ChildNodes.Count > 0)
                    {
                        c.CategoryNodeType = NodeType.MiddleNode;
                    }
                    else
                    {
                        c.CategoryNodeType = NodeType.LeafNode;
                    }
                    c.ChildNodes.Clear();
                }
               
                if (logMsg.Length > 0)
                {
                    //写日志
                    StreamWriter write = new StreamWriter(logFilePath, false, Encoding.Default);
                    write.Write(logMsg.ToString());
                    write.Close();
                    write.Dispose();
                }

                //打开错误日志
                if (logMsg.Length > 0)
                {
                    FileInfo file = new FileInfo(logFilePath);

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
                    }
                }
                else
                {
                    if (listSaveNode.Count > 0)
                    {
                        //保存节点
                        //model.SaveCostItemCategorys(listSaveNode);
                        model.SaveCostItemCate(listSaveNode,cate);
                    }
                    MessageBox.Show("数据导入完毕！");
                    this.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错：" + ExceptionUtil.ExceptionMessage(ex) + "，错误可能在第" + errorIndex.ToString() + "行");
            }
        }
        bool ValideView()
        {
            string errMsgCode = "";
            string errMsgName = "";
            try
            {
                FlexCell.Cell cell = null;
                for (int i = 1; i < flexGrid.Rows; i++)
                {
                    cell = flexGrid.Cell(i, 1);
                    if (string.IsNullOrEmpty(cell.Text.Trim()))
                    {
                        errMsgCode += "第" + i + "行的分类编码为空！" + "\n";
                    }
                    cell = flexGrid.Cell(i, 2);
                    if (string.IsNullOrEmpty(cell.Text.Trim()))
                    {
                        errMsgName += "第" + i + "行的分类名称为空！" + "\n";
                    }
                }
                if (errMsgCode != "" || errMsgName != "")
                {
                    MessageBox.Show(errMsgCode + "\n" + errMsgName);
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
                return false;
            }
            return true;
        }
    }
}
