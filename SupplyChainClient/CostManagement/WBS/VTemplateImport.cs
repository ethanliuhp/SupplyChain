using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.IO;
using System.Data.OleDb;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using IRPServiceModel.Domain.Document;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public partial class VTemplateImport : TBasicDataView
    {
        MWBSManagement model = new MWBSManagement();
        public VTemplateImport()
        {
            InitializeComponent();
            InitEvent();
            InitDate();
        }
        void InitDate()
        {
            object[] o = new object[] { txtProTaskType };
            ObjectLock.Lock(o);
        }
        void InitEvent()
        {
            this.btnBrownExcel.Click += new EventHandler(btnBrownExcel_Click);
            this.btnLoadData.Click += new EventHandler(btnLoadData_Click);
            this.btnCheckProTaskType.Click += new EventHandler(btnCheckProTaskType_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            if (txtProTaskType.Tag != null)
            {
                ProjectTaskTypeTree type = txtProTaskType.Tag as ProjectTaskTypeTree;
                objectQuery.AddCriterion(Expression.Eq("ProTaskType.Id", type.Id));
                //objectQuery.AddCriterion(Expression.Eq("ProTaskType.Name", type.Name));
            }
            if (txtTempName.Text.Trim() != "")
            {
                objectQuery.AddCriterion(Expression.Like("StencilName", txtTempName.Text, MatchMode.Anywhere));
            }
            if (txtTempCode.Text.Trim() != "")
            {
                objectQuery.AddCriterion(Expression.Eq("StencilCode", txtTempCode.Text));
            }
            IList tempList = model.ObjectQuery(typeof(ProTaskTypeDocumentStencil), objectQuery);
            if (tempList != null && tempList.Count > 0)
            {
                dgvDocumentList.Rows.Clear();
                foreach (ProTaskTypeDocumentStencil temp in tempList)
                {
                    int rowIndex = dgvDocumentList.Rows.Add();
                    dgvDocumentList[colTempName.Name, rowIndex].Value = temp.StencilName;
                    dgvDocumentList[colTempCode.Name, rowIndex].Value = temp.StencilCode;
                    dgvDocumentList[colTempDesc.Name, rowIndex].Value = temp.StencilDescription;
                    dgvDocumentList[colCateCode.Name, rowIndex].Value = temp.DocumentCateCode;
                    dgvDocumentList[colCateName.Name, rowIndex].Value = temp.DocumentCateName;
                    dgvDocumentList[colCheckFlag.Name, rowIndex].Value = temp.InspectionMark.ToString();
                    dgvDocumentList[colAlterMode.Name, rowIndex].Value = temp.AlarmMode.ToString();
                    dgvDocumentList.Rows[rowIndex].Tag = temp;
                }
            }
            else
            {
                dgvDocumentList.Rows.Clear();
                MessageBox.Show("未找到数据！");
            }
        }
        /// <summary>
        /// 选择工程任务类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCheckProTaskType_Click(object sender, EventArgs e)
        {
            //MWBSManagement mo = new MWBSManagement();
            VSelectWBSProjectTaskType ptt = new VSelectWBSProjectTaskType(model);
            ptt.ShowDialog();
            if (ptt.SelectResult != null && ptt.SelectResult.Count > 0)
            {
                ProjectTaskTypeTree type = ptt.SelectResult[0].Tag as ProjectTaskTypeTree;
                txtProTaskType.Text = type.Name;
                txtProTaskType.Tag = type;
            }
        }

        /// <summary>
        /// 加载Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnLoadData_Click(object sender, EventArgs e)
        {
            OleDbConnection conpart = null;
            //int erow = 0;
            //int cell = 0;

            try
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

                conpart = new OleDbConnection(ConnectionString);
                conpart.Open();

                DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                //导入数据
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow row = tables.Rows[i];

                    string tableName = row["TABLE_NAME"].ToString().Trim();
                    if (tableName == "工程WBS任务类型与文档模板$")
                    {
                        string sqlStr = "select * from [" + tableName + "]";
                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, conpart);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        IList addDocStencil = new ArrayList();

                        #region 加载数据-IRP文档方式

                        //foreach (DataRow dataRow in dt.Rows)
                        //{
                        //    object[] obj = dataRow.ItemArray;
                        //    if (obj[0] == null || obj[0].ToString().Length < 2)
                        //        continue;

                        //    string code = obj[0].ToString();//任务类型代码
                        //    int length = code.Length;
                        //    string updateCode = "0" + code.Substring(0, 1) + "0" + code.Substring(1, length - 2) + "0" + code.Substring(length - 1, 1);

                        //    ProjectTaskTypeTree proTaskType = null;

                        //    ObjectQuery oq = new ObjectQuery();
                        //    oq.AddCriterion(Expression.Eq("Code", updateCode));
                        //    IList taskTypeList = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                        //    if (taskTypeList != null && taskTypeList.Count > 0)
                        //    {
                        //        proTaskType = taskTypeList[0] as ProjectTaskTypeTree;
                        //    }
                        //    else
                        //    {
                        //        continue;
                        //    }

                        //    for (int j = 0; j < obj.Length; j++)
                        //    {
                        //        //cell++;
                        //        if (j > 1 && obj[j].ToString().Trim() != "")
                        //        {

                        //            PLMWebServicesByKB.ProjectDocument[] resultDoc = null;

                        //            string tempCode = obj[j].ToString().Trim();

                        //            if (tempCode.Length == 6)//6位补倒数第三位
                        //            {
                        //                tempCode = tempCode.Substring(0, 4) + "0" + tempCode.Substring(4);
                        //            }

                        //            tempCode = tempCode.PadRight(7, '0') + "001";//不足七位补0 然后加001表示分类下的第一个文档对象（模板）

                        //            if (Convert.ToInt32(tempCode.Substring(0, 2)) <= 12)//以01-12开头的文档编码为归档文档模板，编码更改为 0101+文档编码
                        //            {
                        //                tempCode = "0101" + tempCode;
                        //            }
                        //            else//以大于12开头的文档编码为非归档文档模板，编码更改为 0102+文档编码
                        //            {
                        //                tempCode = "0102" + tempCode;
                        //            }

                        //            System.Threading.Thread.Sleep(50);

                        //            PLMWebServicesByKB.ErrorStack es = StaticMethod.GetProjectDocumentByKB("KB", "知识库", tempCode, null, null, null, DocumentQueryVersion.最新版本, null, null, null, null, null, null, null, null,
                        //                StaticMethod.KB_System_UserName, StaticMethod.KB_System_JobId, null, out resultDoc);

                        //            if (es != null)
                        //            {
                        //                MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //                //return;
                        //            }
                        //            if (resultDoc != null)
                        //            {
                        //                ProjectDocument doc = resultDoc[0];

                        //                ProTaskTypeDocumentStencil docStencil = new ProTaskTypeDocumentStencil();
                        //                docStencil.ProTaskType = proTaskType;
                        //                docStencil.ProTaskTypeName = proTaskType.Name;
                        //                docStencil.ProDocumentMasterID = doc.EntityID;

                        //                if (doc.IsInspectionLot)
                        //                    docStencil.InspectionMark = ProjectTaskTypeCheckFlag.针对检验批;
                        //                else
                        //                    docStencil.InspectionMark = ProjectTaskTypeCheckFlag.针对项目任务节点;

                        //                //docStencil.ControlWorkflowName = "测试";

                        //                docStencil.ProjectCode = "KB";
                        //                docStencil.ProjectName = "知识库";

                        //                docStencil.AlarmMode = ProjectTaskTypeAlterMode.任务完成时触发验证;
                        //                docStencil.StencilName = doc.Name;
                        //                docStencil.StencilCode = doc.Code;
                        //                docStencil.StencilDescription = doc.Description;
                        //                if (doc.Category != null)
                        //                {
                        //                    docStencil.DocumentCateCode = doc.Category.CategoryCode;
                        //                    docStencil.DocumentCateName = doc.Category.CategoryName;
                        //                }
                        //                docStencil.SysCode = proTaskType.SysCode;

                        //                addDocStencil.Add(docStencil);
                        //            }
                        //            else//调试
                        //            {
                        //                string s = tempCode;
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion

                        #region 加在数据

                        foreach (DataRow dataRow in dt.Rows)
                        {
                            object[] obj = dataRow.ItemArray;
                            if (obj[0] == null || obj[0].ToString().Length < 2)
                                continue;

                            string code = obj[0].ToString();//任务类型代码
                            int length = code.Length;
                            string updateCode = "0" + code.Substring(0, 1) + "0" + code.Substring(1, length - 2) + "0" + code.Substring(length - 1, 1);

                            ProjectTaskTypeTree proTaskType = null;

                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("Code", updateCode));
                            IList taskTypeList = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);
                            oq.Criterions.Clear();

                            if (taskTypeList != null && taskTypeList.Count > 0)
                            {
                                proTaskType = taskTypeList[0] as ProjectTaskTypeTree;
                            }
                            else
                            {
                                continue;
                            }

                            for (int j = 0; j < obj.Length; j++)
                            {
                                if (j > 1 && obj[j].ToString().Trim() != "")
                                {
                                    string tempCode = obj[j].ToString().Trim();

                                    if (tempCode.Length == 6)//6位补倒数第三位
                                    {
                                        tempCode = tempCode.Substring(0, 4) + "0" + tempCode.Substring(4);
                                    }

                                    tempCode = tempCode.PadRight(7, '0') + "001";//不足七位补0 然后加001表示分类下的第一个文档对象（模板）

                                    if (Convert.ToInt32(tempCode.Substring(0, 2)) <= 12)//以01-12开头的文档编码为归档文档模板，编码更改为 0101+文档编码
                                    {
                                        tempCode = "0101" + tempCode;
                                    }
                                    else//以大于12开头的文档编码为非归档文档模板，编码更改为 0102+文档编码
                                    {
                                        tempCode = "0102" + tempCode;
                                    }

                                    DocumentMaster m = new DocumentMaster();

                                    oq.Criterions.Clear();
                                    oq.AddCriterion(Expression.Eq("Code", tempCode));
                                    IList resultDoc = model.ObjectQuery(typeof(DocumentMaster), oq);
                                    if (resultDoc != null && resultDoc.Count > 0)
                                    {
                                        DocumentMaster doc = resultDoc[0] as DocumentMaster;

                                        ProTaskTypeDocumentStencil docStencil = new ProTaskTypeDocumentStencil();
                                        docStencil.ProTaskType = proTaskType;
                                        docStencil.ProTaskTypeName = proTaskType.Name;
                                        docStencil.SysCode = proTaskType.SysCode;

                                        docStencil.ProDocumentMasterID = doc.Id;
                                        docStencil.DocumentCateCode = doc.CategoryCode;
                                        docStencil.DocumentCateName = doc.CategoryName;

                                        if (doc.IsInspectionLot)
                                            docStencil.InspectionMark = ProjectTaskTypeCheckFlag.针对检验批;
                                        else
                                            docStencil.InspectionMark = ProjectTaskTypeCheckFlag.针对项目任务节点;

                                        docStencil.ProjectCode = "KB";
                                        docStencil.ProjectName = "知识库";

                                        docStencil.AlarmMode = ProjectTaskTypeAlterMode.任务完成时触发验证;
                                        docStencil.StencilName = doc.Name;
                                        docStencil.StencilCode = doc.Code;
                                        docStencil.StencilDescription = doc.Description;

                                        addDocStencil.Add(docStencil);
                                    }
                                    else//调试
                                    {
                                        string s = tempCode;
                                    }
                                }
                            }
                        }

                        #endregion

                        if (addDocStencil.Count > 0)
                            model.SaveOrUpdate(addDocStencil);

                        MessageBox.Show("导入成功！");
                        txtExcelFilePath.Text = "";
                        filePath = "";
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("行:" + erow + "      列:" + cell);
            }
            finally
            {

                if (conpart != null)
                {
                    conpart.Close();
                    conpart.Dispose();
                }
            }
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private string GetExceptionMessage(PLMWebServicesByKB.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServicesByKB.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "；\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if ((msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1) || msg.IndexOf("违反唯一约束") > -1)
            {
                msg = "已存在同名文档，请重命名该文档名称.";
            }

            return msg;
        }
    }
}
