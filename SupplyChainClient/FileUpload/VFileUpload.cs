using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ImportIntegration;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using System.Reflection;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using IRPServiceModel.Services.Document;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using IRPServiceModel.Domain.Document;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core.Expression;
using System.Data.OleDb;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using VirtualMachine.SystemAspect.Security;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.BIMServerProxy;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Service;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using System.Net;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;

namespace Application.Business.Erp.SupplyChain.Client.FileUpload
{
    public partial class VFileUpload : TBasicDataView
    {
        IGWBSTreeSrv model2 = null;
        IDocumentSrv model = null;
        ISpecialCostSrv srvSpecialCost = null;
        IStockInSrv stockInSrv = null;
        public VFileUpload()
        {
            InitializeComponent();


            //language = "zhs";
            //if (cFuntions == null)
            //{
            //    cFuntions = new IntergrationFrameWork();

            //    DataPackage cadImpl = new DataPackage(this, language);
            //    cFuntions.Init(cadImpl);

            //}
            //BatchImportLocalize.Load(language);

            InitForm();



            //List<GWBSTree> list = new List<GWBSTree>();
            //GWBSTree g = new GWBSTree();
            //g.Name = "abcd";
            //GWBSTree g1 = new GWBSTree();
            //g1.Name = "abc";
            //GWBSTree g2 = new GWBSTree();
            //g2.Name = "acccc";
            //list.Add(g);
            //list.Add(g1);
            //list.Add(g2);

            //var query = list.OrderBy(p => p.Name.Length);

        }

        private void InitForm()
        {
            InitEvents();

            if (model == null)
                model = ConstMethod.GetService("DocumentSrv") as IDocumentSrv;
            if (model2 == null)
                model2 = ConstMethod.GetService("GWBSTreeSrv") as IGWBSTreeSrv;
            if (srvSpecialCost == null)
                srvSpecialCost = ConstMethod.GetService("SpecialCostSrv") as ISpecialCostSrv;
            if (stockInSrv == null)
                stockInSrv = ConstMethod.GetService("StockInSrv") as IStockInSrv;

            List<string> list = new List<string>();
            list.Add("1.2.3.4.5.");
            //list.Add("1.2.");
            //list.Add("1.");
            list.Add("1.2.4.");
            list.Add("1.2.4.5");
            list.Add("1.2.3.");

            list = removeRepeat(list);


            List<List<OperationOrg>> listAll = new List<List<OperationOrg>>();
            List<OperationOrg> list1 = new List<OperationOrg>();
            OperationOrg org1 = new OperationOrg();
            org1.Name = "张三";
            org1.Level = 1;
            list1.Add(org1);

            OperationOrg org2 = new OperationOrg();
            org2.Name = "李四";
            org2.Level = 2;
            list1.Add(org2);

            List<OperationOrg> list2 = new List<OperationOrg>();
            OperationOrg org3 = new OperationOrg();
            org3.Name = "王五";
            org3.Level = 1;
            list2.Add(org3);

            OperationOrg org4 = new OperationOrg();
            org4.Name = "赵六";
            org4.Level = 2;
            list2.Add(org4);

            listAll.Add(list1);
            listAll.Add(list2);

            listAll = listAll.OrderBy(o => o.ElementAt(0).Name).ToList();


        }
        private List<string> removeRepeat(List<string> list)
        {

            //去除具有包含关系的组织
            for (int i = list.Count - 1; i > -1; i--)
            {
                string syscode = list[i];

                for (int j = 0; j < i; j++)
                {
                    string s = list[j];
                    if (syscode.IndexOf(s) > -1)
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
            }

            return list;
        }
        private void InitEvents()
        {
            btnSelectFile.Click += new EventHandler(btnSelectFile_Click);
            btnClearSelected.Click += new EventHandler(btnClearSelected_Click);
            btnClearAll.Click += new EventHandler(btnClearAll_Click);
            btnBatchUpload.Click += new EventHandler(btnBatchUpload_Click);

            btnSelectDocTemple.Click += new EventHandler(btnSelectDocTemple_Click);
            btnInitDocTemple.Click += new EventHandler(btnInitDocTemple_Click);

            btnPrintTestRpt.Click += new EventHandler(btnPrintTestRpt_Click);
        }
        //打印测试报表
        void btnPrintTestRpt_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\Users\Administrator\Desktop\测试文件\KB.01010201001.005197VA0_0201001工程概况表.doc";
            FileInfo file = new FileInfo(filePath);
            FilePropertyUtility.DocumentType type = FilePropertyUtility.getDocumentType(file.Extension);
            object value = FilePropertyUtility.GetPropertyByDocumentType(type, filePath, "文档分类码");
            object value1 = FilePropertyUtility.GetPropertyByDocumentType(type, filePath, "文档代码");
            object value2 = FilePropertyUtility.GetPropertyByDocumentType(type, filePath, "文档GUID");
            object value3 = FilePropertyUtility.GetPropertyByDocumentType(type, filePath, "文件GUID");

        }

        void btnInitDocTemple_Click(object sender, EventArgs e)
        {
            string filePath = txtDocTempleExcelFile.Text.Trim();
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("请选择初始化的Excel文件！");
                btnSelectDocTemple_Click(btnSelectDocTemple, new EventArgs());
                return;
            }
            OleDbConnection conpart = null;
            try
            {
                string ConnectionString = string.Empty;

                if (Path.GetExtension(filePath).ToLower() == ".xls")
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                else
                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

                conpart = new OleDbConnection(ConnectionString);
                conpart.Open();

                DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                List<DocumentCategory> listSavePlaceCate = new List<DocumentCategory>();//需要保存的归档分类
                List<DocumentCategory> listSaveUnPlaceCate = new List<DocumentCategory>();//需要保存的非归档文档分类

                List<DocumentMaster> listSavePlaceDoc = new List<DocumentMaster>();//需要保存的归档文档
                List<DocumentMaster> listSaveUnPlaceDoc = new List<DocumentMaster>();//需要保存的非归档文档

                //数据准备
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();

                FileCabinet appFileCabinet = StaticMethod.GetDefaultFileCabinet();

                //ObjectQuery oqFileCabinet = new ObjectQuery();
                //oqFileCabinet.AddCriterion(Expression.Eq("Id", "1QVBACeaf4HQvuPfVjAnFD"));
                //IList listFileCabinet = model.ObjectQuery(typeof(FileCabinet), oqFileCabinet);
                //appFileCabinet = listFileCabinet[0] as FileCabinet;

                VirtualMachine.Component.Util.IFCGuidGenerator genObj = new IFCGuidGenerator();//用于生成GUID

                DocumentCategory catePlace = null;//归档分类
                DocumentCategory cateUnPlace = null;//非归档分类
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectCode", "KB"));
                oq.AddCriterion(Expression.Or(Expression.Eq("Code", "0101"), Expression.Eq("Code", "0102")));
                IEnumerable<DocumentCategory> listTempDocCate = model.ObjectQuery(typeof(DocumentCategory), oq).OfType<DocumentCategory>();
                var query = from c in listTempDocCate
                            where c.Code == "0101"
                            select c;
                if (query.Count() > 0)
                    catePlace = query.ElementAt(0);

                query = from c in listTempDocCate
                        where c.Code == "0102"
                        select c;
                if (query.Count() > 0)
                    cateUnPlace = query.ElementAt(0);

                string operId = SecurityUtil.GetLogOperId();
                IBusinessOperators author = model2.GetObjectById(typeof(BusinessOperators), operId) as IBusinessOperators;

                string codePrefix = "";
                string placeFilePath = @"E:\MBP文档\竣工资料9.6\文件";//归档文档
                string unPlaceFilePath = @"E:\MBP文档\竣工资料9.6\文件";//非归档文档
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow row = tables.Rows[i];

                    string tableName = row["TABLE_NAME"].ToString().Trim();
                    if (tableName == "文档模板归档资料$")
                    {
                        codePrefix = "0101";

                        #region 导数

                        DirectoryInfo dir = new DirectoryInfo(placeFilePath);
                        FileInfo[] filesInfo = dir.GetFiles();

                        string sqlStr = "select * from [" + tableName + "]";

                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, ConnectionString);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        string code = string.Empty;
                        string name = string.Empty;
                        string desc = string.Empty;
                        string owner = string.Empty;
                        string extendName = string.Empty;
                        string standardName = string.Empty;
                        string standardCode = string.Empty;
                        string docFilePath = string.Empty;

                        int orderNo = 0;
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dataRow = dt.Rows[j];
                            code = dataRow["代码"].ToString().Trim().ToUpper();//代码
                            name = dataRow["名称"].ToString().Trim();
                            desc = dataRow["描述"].ToString().Trim();
                            owner = dataRow["责任人"].ToString().Trim();
                            extendName = dataRow["扩展名"].ToString().Trim();
                            standardName = dataRow["文档模板参照标准名称"].ToString().Trim();
                            standardCode = dataRow["文件模板参照标准中的编号"].ToString().Trim();

                            //一级分类
                            string tempCode = GetFirstCode(code);
                            tempCode = tempCode.Length == 1 ? tempCode + "0" : tempCode;

                            if (tempCode.Length == 2)
                            {
                                code = codePrefix + tempCode;
                                orderNo += 1;

                                DocumentCategory docCate = new DocumentCategory();
                                docCate.ParentNode = catePlace;

                                docCate.Id = genObj.GeneratorIFCGuid();
                                docCate.SysCode = catePlace.SysCode + docCate.Id + ".";
                                docCate.Name = name;
                                docCate.Code = code;
                                docCate.Describe = desc;
                                docCate.OrderNo = orderNo;
                                docCate.OwnerGUID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Id;
                                docCate.OwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                                docCate.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;
                                docCate.ProjectCode = "KB";
                                docCate.ProjectId = "";
                                docCate.ProjectName = "知识库";
                                docCate.State = 1;

                                listSavePlaceCate.Add(docCate);

                                //二级分类
                                int orderNo2 = 0;
                                foreach (DataRow dataRow2 in dt.Rows)
                                {
                                    string code2 = dataRow2["代码"].ToString().Trim().ToUpper();//代码
                                    string name2 = dataRow2["名称"].ToString().Trim();
                                    string desc2 = dataRow2["描述"].ToString().Trim();
                                    string owner2 = dataRow2["责任人"].ToString().Trim();

                                    string tempCode2 = GetFirstCode(code2);
                                    tempCode2 = tempCode2.Length == 3 ? tempCode2 + "0" : tempCode2;

                                    if (tempCode2.Length == 4 && tempCode2.IndexOf(tempCode) == 0)
                                    {
                                        code2 = codePrefix + tempCode2;
                                        orderNo2 += 1;

                                        DocumentCategory docCate2 = new DocumentCategory();
                                        docCate2.ParentNode = docCate;

                                        docCate2.Id = genObj.GeneratorIFCGuid();
                                        docCate2.SysCode = docCate.SysCode + docCate2.Id + ".";
                                        docCate2.Name = name2;
                                        docCate2.Code = code2;
                                        docCate2.Describe = desc2;
                                        docCate2.OrderNo = orderNo2;
                                        docCate2.OwnerGUID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Id;
                                        docCate2.OwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                                        docCate2.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;
                                        docCate2.ProjectCode = "KB";
                                        docCate2.ProjectId = "";
                                        docCate2.ProjectName = "知识库";
                                        docCate2.State = 1;

                                        listSavePlaceCate.Add(docCate2);

                                        #region 文档对象
                                        int orderNo3 = 0;
                                        foreach (DataRow dataRow3 in dt.Rows)
                                        {
                                            string code3 = dataRow3["代码"].ToString().Trim();//代码
                                            string name3 = dataRow3["名称"].ToString().Trim();
                                            string desc3 = dataRow3["描述"].ToString().Trim();
                                            string owner3 = dataRow3["责任人"].ToString().Trim();
                                            string extendName3 = dataRow3["扩展名"].ToString().Trim();
                                            string standardName3 = dataRow3["文档模板参照标准名称"].ToString().Trim();
                                            string standardCod3 = dataRow3["文件模板参照标准中的编号"].ToString().Trim();
                                            bool isINSPECTIONLOT = string.IsNullOrEmpty(dataRow3["是否属于检验批文档"].ToString().Trim()) ? false : true;

                                            string docPath3 = string.Empty;

                                            if (GetFirstCode(code3).Length > 4 && code3.IndexOf(tempCode2) == 0)
                                            {
                                                //匹配文件
                                                foreach (FileInfo fileItem in filesInfo)
                                                {
                                                    if (fileItem.Name.IndexOf(code3) == 0)
                                                    {
                                                        docPath3 = fileItem.FullName;
                                                        break;
                                                    }
                                                }
                                                //根据文档名称匹配
                                                //if (string.IsNullOrEmpty(docPath3))
                                                //{
                                                //    foreach (FileInfo fileItem in filesInfo)
                                                //    {
                                                //        if (fileItem.Name == name3)
                                                //        {
                                                //            docPath3 = fileItem.FullName;
                                                //            break;
                                                //        }
                                                //    }
                                                //}

                                                code3 = codePrefix + code3;
                                                orderNo3 += 1;

                                                //添加三级分类
                                                DocumentCategory docCate3 = new DocumentCategory();
                                                docCate3.ParentNode = docCate2;

                                                docCate3.Id = genObj.GeneratorIFCGuid();
                                                docCate3.SysCode = docCate2.SysCode + docCate3.Id + ".";
                                                docCate3.Name = name3;
                                                docCate3.Code = code3;
                                                docCate3.Describe = desc3;
                                                docCate3.OrderNo = orderNo3;
                                                docCate3.OwnerGUID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Id;
                                                docCate3.OwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                                                docCate3.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;
                                                docCate3.ProjectCode = "KB";
                                                docCate3.ProjectId = "";
                                                docCate3.ProjectName = "知识库";
                                                docCate3.State = 1;

                                                listSavePlaceCate.Add(docCate3);

                                                //添加文件对象
                                                code3 = code3 + "001";

                                                DocumentMaster doc = new DocumentMaster();
                                                doc.ProjectCode = "KB";
                                                doc.ProjectId = "";
                                                doc.ProjectName = "知识库";
                                                doc.Category = docCate3;
                                                doc.CategoryCode = docCate3.Code;
                                                doc.CategoryName = docCate3.Name;
                                                doc.CategorySysCode = docCate3.SysCode;//此处无值
                                                doc.OwnerID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                                                doc.OwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                                                doc.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;
                                                doc.Name = name3;
                                                doc.Code = code3;
                                                doc.Description = desc3;
                                                doc.DocType = DocumentInfoTypeEnum.普通文档;
                                                doc.IsInspectionLot = isINSPECTIONLOT;
                                                doc.Title = doc.Name;
                                                doc.KeyWords = doc.Name;
                                                doc.EditOwner = owner3;
                                                doc.ConsultStandardName = standardName3;
                                                doc.ConsultStandardCode = standardCod3;

                                                if (!string.IsNullOrEmpty(docPath3) && File.Exists(docPath3))
                                                {
                                                    DocumentDetail docFile = new DocumentDetail();
                                                    docFile.Master = doc;
                                                    doc.ListFiles.Add(docFile);

                                                    FileInfo file = new FileInfo(docPath3);
                                                    FileStream fileStream = file.OpenRead();
                                                    int FileLen = (int)file.Length;
                                                    Byte[] FileData = new Byte[FileLen];
                                                    ////将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                                                    fileStream.Read(FileData, 0, FileLen);
                                                    if (FileData.Length > 0)
                                                    {
                                                        docFile.ExtendName = Path.GetExtension(docPath3); //文档扩展名
                                                        docFile.FileDataByte = FileData; //文件二进制流
                                                        docFile.FileName = Path.GetFileName(docPath3);
                                                        docFile.TheFileCabinet = appFileCabinet;
                                                    }
                                                }

                                                listSavePlaceDoc.Add(doc);

                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else if (tableName == "文档模板非归档资料$")
                    {
                        codePrefix = "0102";

                        #region 导数
                        DirectoryInfo dir = new DirectoryInfo(placeFilePath);
                        FileInfo[] filesInfo = dir.GetFiles();

                        string sqlStr = "select * from [" + tableName + "]";

                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, ConnectionString);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        string code = string.Empty;
                        string name = string.Empty;
                        string desc = string.Empty;
                        string owner = string.Empty;

                        int orderNo = 0;
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dataRow = dt.Rows[j];

                            code = dataRow["代码"].ToString().Trim().ToUpper();//代码
                            name = dataRow["名称"].ToString().Trim();
                            desc = dataRow["描述"].ToString().Trim();
                            owner = dataRow["责任人"].ToString().Trim();

                            //一级分类
                            string tempCode = GetFirstCode(code);
                            tempCode = tempCode.Length == 1 ? tempCode + "0" : tempCode;

                            if (tempCode.Length == 2)
                            {
                                code = codePrefix + tempCode;
                                orderNo += 1;

                                DocumentCategory docCate = new DocumentCategory();
                                docCate.ParentNode = cateUnPlace;
                                //cateUnPlace.ChildNodes.Add(docCate);

                                docCate.Id = genObj.GeneratorIFCGuid();
                                docCate.SysCode = cateUnPlace.SysCode + docCate.Id + ".";
                                docCate.Name = name;
                                docCate.Code = code;
                                docCate.Describe = desc;
                                //docCate.Author=author;
                                docCate.OrderNo = orderNo;
                                docCate.OwnerGUID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Id;
                                docCate.OwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                                docCate.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;
                                docCate.ProjectCode = "KB";
                                docCate.ProjectId = "";
                                docCate.ProjectName = "知识库";
                                docCate.State = 1;

                                listSavePlaceCate.Add(docCate);

                                //二级分类
                                int orderNo2 = 0;
                                foreach (DataRow dataRow2 in dt.Rows)
                                {

                                    string code2 = dataRow2["代码"].ToString().Trim().ToUpper();//代码
                                    string name2 = dataRow2["名称"].ToString().Trim();
                                    string desc2 = dataRow2["描述"].ToString().Trim();
                                    string owner2 = dataRow2["责任人"].ToString().Trim();

                                    string tempCode2 = GetFirstCode(code2);
                                    tempCode2 = tempCode2.Length == 3 ? tempCode2 + "0" : tempCode2;

                                    if (tempCode2.Length == 4 && tempCode2.IndexOf(tempCode) == 0)
                                    {
                                        code2 = codePrefix + tempCode2;
                                        orderNo2 += 1;

                                        DocumentCategory docCate2 = new DocumentCategory();
                                        docCate2.ParentNode = docCate;

                                        docCate2.Id = genObj.GeneratorIFCGuid();
                                        docCate2.SysCode = docCate.SysCode + docCate2.Id + ".";
                                        docCate2.Name = name2;
                                        docCate2.Code = code2;
                                        docCate2.Describe = desc2;
                                        docCate2.OrderNo = orderNo2;
                                        docCate2.OwnerGUID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Id;
                                        docCate2.OwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                                        docCate2.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;
                                        docCate2.ProjectCode = "KB";
                                        docCate2.ProjectId = "";
                                        docCate2.ProjectName = "知识库";
                                        docCate2.State = 1;

                                        listSavePlaceCate.Add(docCate2);

                                        #region 文档对象
                                        int orderNo3 = 0;
                                        foreach (DataRow dataRow3 in dt.Rows)
                                        {
                                            string code3 = dataRow3["代码"].ToString().Trim();//代码
                                            string name3 = dataRow3["名称"].ToString().Trim();
                                            string desc3 = dataRow3["描述"].ToString().Trim();
                                            string owner3 = dataRow3["责任人"].ToString().Trim();
                                            bool isINSPECTIONLOT = string.IsNullOrEmpty(dataRow3["是否属于检验批文档"].ToString().Trim()) ? false : true;

                                            string docPath3 = string.Empty;

                                            if (GetFirstCode(code3).Length > 4 && code3.IndexOf(tempCode2) == 0)
                                            {
                                                //匹配文件
                                                foreach (FileInfo fileItem in filesInfo)
                                                {
                                                    if (fileItem.Name.IndexOf(code3) == 0)
                                                    {
                                                        docPath3 = fileItem.FullName;
                                                        break;
                                                    }
                                                }
                                                if (string.IsNullOrEmpty(docPath3))
                                                {
                                                    foreach (FileInfo fileItem in filesInfo)
                                                    {
                                                        if (fileItem.Name == name3)
                                                        {
                                                            docPath3 = fileItem.FullName;
                                                            break;
                                                        }
                                                    }
                                                }

                                                code3 = codePrefix + code3;
                                                orderNo3 += 1;

                                                //添加三级分类
                                                DocumentCategory docCate3 = new DocumentCategory();
                                                docCate3.ParentNode = docCate2;

                                                docCate3.Id = genObj.GeneratorIFCGuid();
                                                docCate3.SysCode = docCate2.SysCode + docCate3.Id + ".";
                                                docCate3.Name = name3;
                                                docCate3.Code = code3;
                                                docCate3.Describe = desc3;
                                                docCate3.OrderNo = orderNo3;
                                                docCate3.OwnerGUID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Id;
                                                docCate3.OwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                                                docCate3.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;
                                                docCate3.ProjectCode = "KB";
                                                docCate3.ProjectId = "";
                                                docCate3.ProjectName = "知识库";
                                                docCate3.State = 1;

                                                listSavePlaceCate.Add(docCate3);

                                                //添加文档对象
                                                code3 = code3 + "001";

                                                DocumentMaster doc = new DocumentMaster();
                                                doc.ProjectCode = "KB";
                                                doc.ProjectId = "";
                                                doc.ProjectName = "知识库";
                                                doc.Category = docCate3;
                                                doc.CategoryCode = docCate3.Code;
                                                doc.CategoryName = docCate3.Name;
                                                doc.CategorySysCode = docCate3.SysCode;
                                                doc.OwnerID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                                                doc.OwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                                                doc.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;
                                                doc.Name = name3;
                                                doc.Code = code3;
                                                doc.Description = desc3;
                                                doc.DocType = DocumentInfoTypeEnum.普通文档;
                                                doc.IsInspectionLot = isINSPECTIONLOT;
                                                doc.Title = doc.Name;
                                                doc.KeyWords = doc.Name;
                                                doc.EditOwner = owner3;

                                                if (!string.IsNullOrEmpty(docPath3) && File.Exists(docPath3))
                                                {
                                                    DocumentDetail docFile = new DocumentDetail();
                                                    docFile.Master = doc;
                                                    doc.ListFiles.Add(docFile);

                                                    FileInfo file = new FileInfo(docPath3);
                                                    FileStream fileStream = file.OpenRead();
                                                    int FileLen = (int)file.Length;
                                                    Byte[] FileData = new Byte[FileLen];
                                                    ////将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                                                    fileStream.Read(FileData, 0, FileLen);
                                                    if (FileData.Length > 0)
                                                    {
                                                        docFile.ExtendName = Path.GetExtension(docPath3); //文档扩展名
                                                        docFile.FileDataByte = FileData; //文件二进制流
                                                        docFile.FileName = Path.GetFileName(docPath3);
                                                        docFile.TheFileCabinet = appFileCabinet;
                                                    }
                                                }
                                                listSavePlaceDoc.Add(doc);
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }

                model.InitDocumentTemple(listSavePlaceCate, listSavePlaceDoc);

                MessageBox.Show("初始化完成！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常信息：" + ExceptionUtil.ExceptionMessage(ex));
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

        private string GetFirstCode(string code)
        {
            string str = string.Empty;
            try
            {


                int index = 0;
                char[] chs = code.ToCharArray();
                for (int i = chs.Length - 1; i > -1; i--)
                {
                    if (chs[i] != '0')
                    {
                        index = i;
                        break;
                    }
                }
                for (int i = 0; i <= index; i++)
                {
                    str += chs[i];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }

        void btnSelectDocTemple_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "所有文件(*.*)|*.*|Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                string extName = Path.GetExtension(filePath).ToLower();
                if (extName != ".xls" && extName != ".xlsx")
                {
                    MessageBox.Show("请选择Excel文件(支持2003和2007)！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSelectDocTemple_Click(btnSelectDocTemple, new EventArgs());
                    return;
                }
                txtDocTempleExcelFile.Text = filePath;
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "所有文件(*.*)|*.*";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] strFiles = openFileDialog1.FileNames;
                int iCount = strFiles.Length;
                for (int i = 0; i < iCount; i++)
                {
                    InsertToGrid(strFiles[i]);
                }

                gridFiles.AutoResizeColumns();
            }
        }
        private const double KBCount = 1024;
        private const double MBCount = KBCount * 1024;
        private const double GBCount = MBCount * 1024;
        private const double TBCount = GBCount * 1024;

        /// <summary>
        /// 得到适应的大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns>string</returns>
        public static string GetAutoSizeString(double size, int roundCount)
        {
            if (KBCount > size)
            {
                return Math.Round(size, roundCount) + "B";
            }
            else if (MBCount > size)
            {
                return Math.Round(size / KBCount, roundCount) + "KB";
            }
            else if (GBCount > size)
            {
                return Math.Round(size / MBCount, roundCount) + "MB";
            }
            else if (TBCount > size)
            {
                return Math.Round(size / GBCount, roundCount) + "GB";
            }
            else
            {
                return Math.Round(size / TBCount, roundCount) + "TB";
            }
        }

        private void InsertToGrid(string filePath)
        {
            int index = gridFiles.Rows.Add();
            DataGridViewRow row = gridFiles.Rows[index];
            row.Cells[FileName.Name].Value = Path.GetFileName(filePath);
            row.Cells[FilePath.Name].Value = filePath;


            FileInfo fileInfo = new FileInfo(filePath);
            row.Cells[FileSize.Name].Value = GetAutoSizeString(fileInfo.Length, 3);
        }

        #region 变量

        private DataTable sourceDataTable = new DataTable();
        private IntergrationFrameWork cFuntions = null;
        private string language = "zhs";

        #endregion

        private void btnBatchUpload_Click(object sender, EventArgs e)
        {
            CurrentProjectInfo project = StaticMethod.GetProjectInfo();

            FileCabinet appFileCabinet = StaticMethod.GetDefaultFileCabinet();
            //ObjectQuery oqFileCabinet = new ObjectQuery();
            //oqFileCabinet.AddCriterion(Expression.Eq("Id", "1QVBACeaf4HQvuPfVjAnFD"));
            //IList listFileCabinet = model.ObjectQuery(typeof(FileCabinet), oqFileCabinet);
            //appFileCabinet = listFileCabinet[0] as FileCabinet;

            List<DocumentMaster> listDoc = new List<DocumentMaster>();

            DocumentMaster master = new DocumentMaster();
            master.ProjectId = project.Id;
            master.ProjectCode = project.Code;
            master.ProjectName = project.Name;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode));
            IList list = model.ObjectQuery(typeof(DocumentCategory), oq);
            DocumentCategory cate = list[0] as DocumentCategory;

            master.Category = cate;
            master.CategoryCode = cate.Code;
            master.CategoryName = cate.Name;
            master.CategorySysCode = cate.SysCode;

            master.DocType = DocumentInfoTypeEnum.普通文档;
            master.Name = "测试文档";

            listDoc.Add(master);

            foreach (DataGridViewRow row in gridFiles.Rows)
            {
                DocumentDetail dtl = new DocumentDetail();
                dtl.Master = master;
                master.ListFiles.Add(dtl);

                string filePath = row.Cells[FilePath.Name].Value.ToString();
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)
                {
                    FileStream fileStream = file.OpenRead();
                    int FileLen = (int)file.Length;
                    Byte[] FileData = new Byte[FileLen];
                    //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                    fileStream.Read(FileData, 0, FileLen);

                    dtl.FileDataByte = FileData;
                }

                dtl.ExtendName = Path.GetExtension(filePath);
                dtl.FileName = Path.GetFileName(filePath);
                dtl.TheFileCabinet = appFileCabinet;
            }

            listDoc = model.AddDocumentByCustomExtend(listDoc);

            //PLMWebServicesByKB.CategoryNode[] listCateResult = null;
            //StaticMethod.GetDocumentCategoryByKB(StaticMethod.ProjectDocumentCategoryTypeEnum.CLASSDOCUMENT,
            //    Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.CategoryQueryModeEnum.树方式, null, null, null, null, null, out listCateResult);


            //#region 文件上传

            //if (gridFiles.Rows.Count == 0)
            //{
            //    MessageBox.Show("请先选择要上传的文件！");
            //    btnSelectFile.Focus();
            //    return;
            //}

            //DataTable sourceDataTable = new DataTable();
            //sourceDataTable = PartConfigureLogic.CreatSourceDataTable(false, false, false);

            //foreach (DataGridViewRow row in gridFiles.Rows)
            //{
            //    DataRow newRow = sourceDataTable.NewRow();
            //    //设置文件类型和文件路径
            //    newRow["TempFile"] = row.Cells[FilePath.Name].Value.ToString();
            //    newRow["TempKey"] = row.Cells[FilePath.Name].Value.ToString();
            //    newRow["FileType"] = "DOCUMENT";
            //    newRow["FileStructureType"] = "FILESTRUCTURE";
            //    //newRow["PartToFileType"] = "PARTTOFILELINK";

            //    //如果有必填字段加上必填字段
            //    newRow["FILENAME"] = row.Cells[FileName.Name].Value.ToString();

            //    //设置文件类型和文件路径
            //    //PartConfigureLogic.AnalyseDataRow(newRow, "C:\\temp\\PWS\\file\\PartFile2\\1016.txt", "FILE", "FILESTRUCTURE", "PARTTOFILELINK");//this.FileStructureType.SelectedValue.ToString()

            //    sourceDataTable.Rows.Add(newRow);
            //}

            ////initional intergration frameWork
            ////initional dataPackage
            //DataPackage cadImpl = new DataPackage(this, language);

            ////cadImpl.SetType("PART", "DOCUMENT", "PARTEBOM", "FILESTRUCTURE", "PARTTOFILELINK");
            //cadImpl.SetType("", "DOCUMENT", "", "FILESTRUCTURE", "");

            ////fill data into package
            //cadImpl.SetSourceDataTable(sourceDataTable);

            ////set option item 
            //cadImpl.bOnlySaveFile = true;

            //cFuntions = new IntergrationFrameWork();
            //cFuntions.Init(cadImpl);

            //cFuntions.Save();
            //#endregion
        }

        private void btnClearSelected_Click(object sender, EventArgs e)
        {
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridFiles.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }
            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridFiles.Rows.RemoveAt(listRowIndex[i]);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            for (int i = gridFiles.Rows.Count - 1; i > -1; i--)
            {
                gridFiles.Rows.RemoveAt(i);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (objectQuerySelect1.SelectObject != null)
            {
                PBSTree pbs = objectQuerySelect1.SelectObject as PBSTree;

                MessageBox.Show("名称：" + pbs.Name + ",代码：" + pbs.Code + ",创建时间：" + pbs.CreateDate.ToShortDateString());
            }
        }

        private void btnAddBIMProjectAndIFCFile_Click(object sender, EventArgs e)
        {
            BIMServerProxy.soapClient proxy = new BIMServerProxy.soapClient();
            bool loginFlag = proxy.login("liuyoumin163@163.com", "abc123abc");
            sUser loginUser = proxy.getLoggedInUser();

            sProject testProject = proxy.addProject("测试项目");

            BIMServerProxy.sExtendedData extData = new BIMServerProxy.sExtendedData();
            FileInfo file = new FileInfo(@"E:\BIMServer\File\1.png");
            if (file.Exists)
            {
                FileStream fileStream = file.OpenRead();
                int FileLen = (int)file.Length;
                Byte[] FileData = new Byte[FileLen];
                //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                fileStream.Read(FileData, 0, FileLen);

                extData.data = FileData;
            }
            extData.title = "IFC测试文件";
            extData.userId = loginUser.oid;

            proxy.addExtendedDataToProject(testProject.id, extData);
        }

        private void btnGenGUID_Click(object sender, EventArgs e)
        {
            IFCGuidGenerator g = new IFCGuidGenerator();
            txtGUID.Text = g.GeneratorIFCGuid();
        }

        private void btnRecountPlanOutputVal_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认操作？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                FlashScreen.Show("正在执行，请稍候........");
                srvSpecialCost.RecountPlanOutputValue(StaticMethod.GetProjectInfo());
                FlashScreen.Close();
                MessageBox.Show("操作完成！");
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
            }
            finally
            {
                FlashScreen.Close();
            }

        }

        private void btnRecountRealOutputVal_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认操作？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                FlashScreen.Show("正在执行，请稍候........");
                srvSpecialCost.RecountRealOutputValue();
                FlashScreen.Close();
                MessageBox.Show("操作完成！");
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void btnRecountPlanoutputValByAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认操作？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                FlashScreen.Show("正在执行，请稍候........");
                srvSpecialCost.RecountPlanOutputValue(null);
                FlashScreen.Close();
                MessageBox.Show("操作完成！");
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filePath = @"http://10.70.18.203/TestFile/Files/0001/0001..000001VA0_新建文本文档.txt";
            WebClient wc = new WebClient();
            byte[] fileDa = wc.DownloadData(filePath);
        }

        private void btnInitProjectInfo_Click(object sender, EventArgs e)
        {
            if (txtMaxProjectCode.Text.Trim() == "")
            {
                MessageBox.Show("请设置当前最大项目号！");
                txtMaxProjectCode.Focus();
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Or(Expression.Eq("OperationType", "zgxmb"), Expression.Eq("OperationType", "fgsxmb")));
            oq.AddCriterion(Expression.Eq("State", 1));
            IList listOrg = model2.ObjectQuery(typeof(OperationOrg), oq);

            List<CurrentProjectInfo> listSaveProject = new List<CurrentProjectInfo>();

            int maxProjectCode = ClientUtil.ToInt(txtMaxProjectCode.Text);
            foreach (OperationOrg org in listOrg)
            {
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("OwnerOrg.Id", org.Id));
                IList listProject = model2.ObjectQuery(typeof(CurrentProjectInfo), oq);
                if (listProject.Count == 0)
                {
                    CurrentProjectInfo projectInfo = new CurrentProjectInfo();
                    projectInfo.Name = org.Name;
                    projectInfo.Code = (maxProjectCode + 1).ToString();
                    projectInfo.OwnerOrg = org;
                    projectInfo.OwnerOrgName = org.Name;
                    projectInfo.OwnerOrgSysCode = org.SysCode;
                    listSaveProject.Add(projectInfo);

                    stockInSrv.SaveCurrentProjectInfo(projectInfo);

                    maxProjectCode += 1;
                }
            }

            txtMaxProjectCode.Text = maxProjectCode.ToString();

            //FrmInitProjectInfo frm = new FrmInitProjectInfo();
            //frm.ShowDialog();
        }
    }
}
