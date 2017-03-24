using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core;
using System.Collections;
using CommonSearchLib.BillCodeMng.Service;
using IRPServiceModel.Domain.Document;
using VirtualMachine.SystemAspect.Security;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.CommonClass.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using NHibernate.Criterion;
using System.IO;
using System.Configuration;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;

namespace IRPServiceModel.Services.Document
{
    /// <summary>
    /// 文档服务
    /// </summary>
    public class DocumentSrv : IDocumentSrv
    {
        #region 注入服务
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        private ICategoryNodeService nodeSrv;
        public ICategoryNodeService NodeSrv
        {
            get { return nodeSrv; }
            set { nodeSrv = value; }
        }
        #endregion
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        [TransManager]
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 保存或修改对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdate(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        [TransManager]
        public object SaveOrUpdate(object obj)
        {
            dao.SaveOrUpdate(obj);

            return obj;
        }
        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        [TransManager]
        public DocumentMaster SaveOrUpdateMaster(DocumentMaster master, bool isModel)
        {
            master = SaveOrUpdate(master) as DocumentMaster;
            if (isModel)
            {
                ProTaskTypeDocumentStencil docStencil = new ProTaskTypeDocumentStencil();

                docStencil.ProDocumentMasterID = master.Id;
                docStencil.DocumentCateCode = master.CategoryCode;
                docStencil.DocumentCateName = master.CategoryName;

                if (master.IsInspectionLot)
                    docStencil.InspectionMark = ProjectTaskTypeCheckFlag.针对检验批;
                else
                    docStencil.InspectionMark = ProjectTaskTypeCheckFlag.针对项目任务节点;

                docStencil.ProjectCode = "KB";
                docStencil.ProjectName = "知识库";

                docStencil.AlarmMode = ProjectTaskTypeAlterMode.任务完成时触发验证;
                docStencil.StencilName = master.Name;
                docStencil.StencilCode = master.Code;
                docStencil.StencilDescription = master.Description;

            }
            return master;
        }
        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        [TransManager]
        public FileCabinet SaveOrUpdate(FileCabinet obj)
        {
            dao.SaveOrUpdate(obj);

            return obj;
        }
        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(IList list)
        {
            return dao.Delete(list);
        }
        /// <summary>
        /// 删除文件柜
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(FileCabinet obj)
        {
            return dao.Delete(obj);
        }

        /// <summary>
        /// 删除工程文档验证和文档
        /// </summary>
        /// <param name="list">工程文档验证集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteProjectDocumentVerifyAndDocument(IList list)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (ProjectDocumentVerify pdv in list)
                {
                    dis.Add(Expression.Eq("Id", pdv.DocuemntID));
                }
                oq.AddCriterion(dis);
                oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
                IList resultDocumentList = dao.ObjectQuery(typeof(DocumentMaster), oq);
                dao.Delete(resultDocumentList);
                dao.Delete(list);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 删除工程对象关联文档和文档
        /// </summary>
        /// <param name="list">工程对象关联对象集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteProObjectRelaDocumentAndDocument(IList list)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (ProObjectRelaDocument pord in list)
                {
                    dis.Add(Expression.Eq("Id", pord.DocumentGUID));
                }
                oq.AddCriterion(dis);
                oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
                IList resultDocumentList = dao.ObjectQuery(typeof(DocumentMaster), oq);
                dao.Delete(resultDocumentList);
                dao.Delete(list);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }

        /// <summary>
        /// 保存文档分类树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public DocumentCategory SaveCateTree(DocumentCategory childNode)
        {
            childNode.UpdatedDate = DateTime.Now;
            if (childNode.Id == null)
            {
                childNode.CreateDate = DateTime.Now;
                string operId = "";
                if (childNode.Author == null)
                {
                    try
                    {
                        operId = SecurityUtil.GetLogOperId();
                    }
                    catch { }
                }
                else
                {
                    operId = childNode.Author.Id;
                }
                childNode.Author = Dao.Get(typeof(BusinessOperators), operId) as IBusinessOperators;

                childNode.OwnerGUID = operId;
                childNode.OwnerName = childNode.Author.PerName;

                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
                childNode.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

                if (childNode.ParentNode == null)
                {
                    CategoryTree tree = null;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", "文档分类树"));
                    IList list = dao.ObjectQuery(typeof(CategoryTree), oq);
                    if (list.Count == 0)
                    {
                        tree = new CategoryTree();
                        tree.CreateDate = DateTime.Now;
                        tree.Author = childNode.Author;
                        tree.Code = ClassUtil.GetFullNameAndAssembly(childNode.GetType());
                        tree.Name = "文档分类树";
                        tree.RootId = "1";
                        tree.MaxLevel = 0;
                        tree.CurrMaxLevel = 1;

                        dao.SaveOrUpdate(tree);
                    }
                    else
                        tree = list[0] as CategoryTree;

                    childNode.CreateDate = DateTime.Now;
                    childNode.State = 1;
                    childNode.CategoryNodeType = NodeType.RootNode;
                    childNode.Level = 1;
                    childNode.OrderNo = 0;

                    childNode.TheTree = tree;

                    dao.SaveOrUpdate(childNode);

                    if (!string.IsNullOrEmpty(childNode.Id))
                        childNode.SysCode = childNode.Id + ".";

                    dao.SaveOrUpdate(childNode);
                }
                else
                {
                    //childNode.TheTree = childNode.ParentNode.TheTree;
                    //childNode.ParentNode = dao.Get(typeof(DocumentCategory), childNode.ParentNode.Id) as DocumentCategory;
                    nodeSrv.AddChildNode(childNode);
                }
            }
            else
                nodeSrv.UpdateCategoryNode(childNode);
            return childNode;
        }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveCateTree(IList lst)
        {
            DocumentCategory node = lst[0] as DocumentCategory;

            string operId = "";
            if (node.Author == null)
            {
                try
                {
                    operId = SecurityUtil.GetLogOperId();
                }
                catch { }
            }
            else
            {
                operId = node.Author.Id;
            }
            IBusinessOperators author = Dao.Get(typeof(BusinessOperators), operId) as IBusinessOperators;


            CategoryTree tree = null;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Name", "文档分类树"));
            IList list = dao.ObjectQuery(typeof(CategoryTree), oq);
            if (list.Count == 0)
            {
                tree = new CategoryTree();
                tree.CreateDate = DateTime.Now;
                tree.Author = author;
                tree.Code = ClassUtil.GetFullNameAndAssembly(node.GetType());
                tree.Name = "文档分类树";
                tree.RootId = "1";
                tree.MaxLevel = 0;
                tree.CurrMaxLevel = 1;

                dao.SaveOrUpdate(tree);
            }
            else
                tree = list[0] as CategoryTree;

            node.CreateDate = DateTime.Now;
            node.UpdatedDate = DateTime.Now;
            node.State = 1;
            node.CategoryNodeType = NodeType.RootNode;
            node.Level = 1;
            node.OrderNo = 0;

            node.Author = author;

            node.OwnerGUID = operId;
            node.OwnerName = node.Author.PerName;

            Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
            node.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

            node.TheTree = tree;

            dao.SaveOrUpdate(node);

            if (!string.IsNullOrEmpty(node.Id))
                node.SysCode = node.Id + ".";

            dao.SaveOrUpdate(node);

            lst[0] = node;

            for (int i = 1; i < lst.Count; i++)
            {
                lst[i] = SaveCateTree(lst[i] as DocumentCategory);
            }

            return lst;
        }

        /// <summary>
        /// 删除文档分类树节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCateTree(DocumentCategory cate)
        {
            cate = dao.Get(typeof(DocumentCategory), cate.Id) as DocumentCategory;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("CategorySysCode", cate.SysCode, MatchMode.Start));
            oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
            IList list = dao.ObjectQuery(typeof(DocumentMaster), oq);
            dao.Delete(list);
            return nodeSrv.DeleteCategoryNode(cate);
        }

        /// <summary>
        /// 上传(保存)文档
        /// </summary>
        /// <param name="listDocument">项目文档对象集</param>
        /// <returns>保存后的项目文档对象集</returns>
        [TransManager]
        public List<DocumentMaster> AddDocumentByCustomExtend(List<DocumentMaster> listDocument)
        {
            if (listDocument == null || listDocument.Count == 0)
                return null;

            #region 远程客户端文件上传
            try
            {
                string errorMsg = string.Empty;
                string fileTempDir = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["FileUploadTempDir"];
                VirtualMachine.Component.Util.IFCGuidGenerator genObj = new IFCGuidGenerator();//用于生成GUID

                //FileCabinet appFileCabinet = null;

                //IList listFileCabinet = dao.ObjectQuery(typeof(FileCabinet), new ObjectQuery());
                //if (listFileCabinet.Count > 0)
                //{
                //    appFileCabinet = listFileCabinet[0] as FileCabinet;
                //}

                //if (appFileCabinet == null)
                //{
                //    errorMsg = "没有使用的文件柜.";
                //    Exception ex = new Exception(errorMsg);
                //    throw ex;
                //}

                //创建临时目录
                if (!string.IsNullOrEmpty(fileTempDir) && Directory.Exists(fileTempDir) == false)
                {
                    Directory.CreateDirectory(fileTempDir);
                }

                for (int i = 0; i < listDocument.Count; i++)
                {
                    DocumentMaster doc = listDocument[i];

                    if (string.IsNullOrEmpty(doc.Name))
                    {
                        errorMsg = "文档名称不能为空.";
                        break;
                    }
                    if (string.IsNullOrEmpty(doc.ProjectCode))
                    {
                        errorMsg = "文档“" + doc.Name + "”保存失败,所属项目代码不能为空.";
                        break;
                    }

                    doc.NGUID = genObj.GeneratorIFCGuid();
                    //doc.Id = genObj.GeneratorIFCGuid();
                    if (string.IsNullOrEmpty(doc.VersionMajor))
                        doc.SetNewVersion();
                    if (string.IsNullOrEmpty(doc.Revision))
                        doc.SetNewRevision();

                    if (doc.ListFiles != null && doc.ListFiles.Count > 0)
                    {
                        foreach (DocumentDetail docFile in doc.ListFiles)
                        {
                            #region 上传文件
                            if (!string.IsNullOrEmpty(docFile.FileName) && docFile.FileName.IndexOf(".") > -1)//文件扩展名统一小写
                                docFile.FileName = docFile.FileName.Substring(0, docFile.FileName.LastIndexOf(".")) + Path.GetExtension(docFile.FileName).ToLower();

                            if (!string.IsNullOrEmpty(docFile.ExtendName))//文件扩展名统一小写
                                docFile.ExtendName = docFile.ExtendName.ToLower();

                            //docFile.Id = genObj.GeneratorIFCGuid();

                            if (docFile.FileDataByte != null && docFile.FileDataByte.Length > 0)
                            {
                                string fileName = docFile.FileName;
                                if (string.IsNullOrEmpty(fileName))
                                {
                                    errorMsg = "文档“" + doc.Name + "”下的文件保存失败,文件名不能为空.";
                                    break;
                                }
                                if (CheckFileExtension(fileName) == false)
                                {
                                    errorMsg = "文件“" + fileName + "”保存失败,不支持文件格式:" + Path.GetExtension(fileName);
                                    break;
                                }
                                if (docFile.TheFileCabinet == null)
                                {
                                    errorMsg = "文件“" + fileName + "”保存失败,未指定文件柜.";
                                    break;
                                }
                            }
                            #endregion
                        }
                    }

                    Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;

                    doc.OwnerID = login.ThePerson;
                    doc.OwnerName = login.ThePerson.Name;
                    doc.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Exception ex = new Exception(errorMsg);
                    throw ex;
                }

                dao.Save(listDocument);


                #region 上传文件

                var queryFile = from d in listDocument
                                where d.ListFiles != null && d.ListFiles.Count > 0
                                select d;

                IList listFiles = new ArrayList();
                foreach (DocumentMaster doc in listDocument)
                {
                    foreach (DocumentDetail docFile in doc.ListFiles)
                    {
                        if (docFile.FileDataByte != null && docFile.FileDataByte.Length > 0)
                        {
                            string fileName = docFile.FileName;

                            string DocumentNumber = GetMaxCodeByProjectAndClass(doc.ProjectCode, doc.ProjectName, docFile.GetType());//所属项目流水号
                            fileName = doc.ProjectCode + "." + doc.CategoryCode + "." + DocumentNumber + "V" + doc.VersionMajor + doc.Revision + "_" + fileName;
                            string tempFullName = fileTempDir + "\\" + fileName;

                            WriteBinaryToFile(tempFullName, docFile.FileDataByte);//写到服务端临时文件

                            docFile.FilePartPath = @doc.ProjectCode + "/" + doc.CategoryCode + "/" + fileName;

                            if (FilePropertyUtility.IsEnabledWriteCustomProperty)
                            {
                                //写入【文档代码】、【GUID】、【文档分类码】值到windows文件属性里
                                object docCategoryCode = doc.CategoryCode == null ? "" : doc.CategoryCode;
                                object docCode = doc.Code == null ? "" : doc.Code;
                                object docGUID = doc.Id;
                                object docFileGUID = docFile.Id;

                                Dictionary<string, object> dicKeyValue = new Dictionary<string, object>();
                                dicKeyValue.Add("文档分类码", docCategoryCode);
                                dicKeyValue.Add("文档代码", docCode);
                                dicKeyValue.Add("文档GUID", docGUID);
                                dicKeyValue.Add("文件GUID", docFileGUID);

                                FilePropertyUtility.SetCustomProperty(tempFullName, dicKeyValue);
                            }

                            ToVault(tempFullName, docFile);//上传到文件柜

                            FileInfo fInfo = new FileInfo(tempFullName);//删除临时文件
                            if (fInfo.Exists)
                                fInfo.Delete();

                            docFile.FileDataByte = null;

                            listFiles.Add(docFile);
                        }
                    }
                }
                #endregion

                if (listFiles.Count > 0)
                {
                    dao.Update(listFiles);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            return listDocument;
        }

        /// <summary>
        /// 文档文件一起保存(新增用)
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        [TransManager]
        public DocumentMaster SaveDocumentAndFile(DocumentMaster doc)
        {
            if (doc == null) return null;

            string errorMsg = string.Empty;
            VirtualMachine.Component.Util.IFCGuidGenerator genObj = new IFCGuidGenerator();//用于生成GUID
            //DocumentMaster doc = master;
            try
            {
                if (string.IsNullOrEmpty(doc.Name))
                {
                    errorMsg = "文档名称不能为空.";
                }
                if (string.IsNullOrEmpty(doc.ProjectCode))
                {
                    errorMsg = "文档“" + doc.Name + "”保存失败,所属项目代码不能为空.";
                }

                foreach (DocumentDetail docFile in doc.ListFiles)
                {
                    if (docFile.FileDataByte != null && docFile.FileDataByte.Length > 0)
                    {
                        string fileName = docFile.FileName;
                        if (string.IsNullOrEmpty(fileName))
                        {
                            errorMsg = "文档“" + doc.Name + "”下的文件保存失败,文件名不能为空.";
                            break;
                        }
                        if (CheckFileExtension(fileName) == false)
                        {
                            errorMsg = "文件“" + fileName + "”保存失败,不支持文件格式:" + Path.GetExtension(fileName);
                            break;
                        }
                        if (docFile.TheFileCabinet == null)
                        {
                            errorMsg = "文件“" + fileName + "”保存失败,未指定文件柜.";
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Exception ex = new Exception(errorMsg);
                    throw ex;
                }

                if (string.IsNullOrEmpty(doc.NGUID))
                    doc.NGUID = genObj.GeneratorIFCGuid();
                if (string.IsNullOrEmpty(doc.VersionMajor))
                    doc.SetNewVersion();
                if (string.IsNullOrEmpty(doc.Revision))
                    doc.SetNewRevision();

                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
                if (login != null)
                {
                    doc.OwnerID = login.ThePerson;
                    doc.OwnerName = login.ThePerson.Name;
                    doc.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

                }
                dao.SaveOrUpdate(doc);


                if (doc.ListFiles != null && doc.ListFiles.Count > 0)
                {
                    foreach (DocumentDetail docFile in doc.ListFiles)
                    {
                        SaveFile(doc, docFile);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return doc;
        }

        /// <summary>
        ///  文档文件一起保存(修改用)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="docFileList"></param>
        /// <returns></returns>
        [TransManager]
        public DocumentMaster SaveDocumentAndFile(DocumentMaster doc, IList docFileList)
        {
            if (doc == null) return null;

            string errorMsg = string.Empty;
            VirtualMachine.Component.Util.IFCGuidGenerator genObj = new IFCGuidGenerator();//用于生成GUID
            //DocumentMaster doc = master;
            try
            {
                if (string.IsNullOrEmpty(doc.Name))
                {
                    errorMsg = "文档名称不能为空.";
                }
                if (string.IsNullOrEmpty(doc.ProjectCode))
                {
                    errorMsg = "文档“" + doc.Name + "”保存失败,所属项目代码不能为空.";
                }

                foreach (DocumentDetail docFile in doc.ListFiles)
                {
                    if (docFile.FileDataByte != null && docFile.FileDataByte.Length > 0)
                    {
                        string fileName = docFile.FileName;
                        if (string.IsNullOrEmpty(fileName))
                        {
                            errorMsg = "文档“" + doc.Name + "”下的文件保存失败,文件名不能为空.";
                            break;
                        }
                        if (CheckFileExtension(fileName) == false)
                        {
                            errorMsg = "文件“" + fileName + "”保存失败,不支持文件格式:" + Path.GetExtension(fileName);
                            break;
                        }
                        if (docFile.TheFileCabinet == null)
                        {
                            errorMsg = "文件“" + fileName + "”保存失败,未指定文件柜.";
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Exception ex = new Exception(errorMsg);
                    throw ex;
                }

                if (string.IsNullOrEmpty(doc.NGUID))
                    doc.NGUID = genObj.GeneratorIFCGuid();

                if (string.IsNullOrEmpty(doc.VersionMajor))
                    doc.SetNewVersion();
                if (string.IsNullOrEmpty(doc.Revision))
                    doc.SetNewRevision();

                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;

                doc.OwnerID = login.ThePerson;
                doc.OwnerName = login.ThePerson.Name;
                doc.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

                dao.SaveOrUpdate(doc);

                if (docFileList != null && docFileList.Count > 0)
                {
                    foreach (DocumentDetail docFile in docFileList)
                    {
                        SaveFile(doc, docFile);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return doc;
        }

        /// <summary>
        /// 文档、文件、MBP对象关联文档 一起保存（用于表单）
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="id">MBP对象id</param>
        /// <returns></returns>
        [TransManager]
        public DocumentMaster SaveDocumentAndFileAndDocObject(DocumentMaster doc, string id)
        {
            doc.Temp1 = id;

            doc = SaveDocumentAndFile(doc);

            ProObjectRelaDocument obj = new ProObjectRelaDocument();
            obj.ProObjectGUID = id;
            obj.DocumentGUID = doc.Id;
            obj.DocumentCateCode = doc.CategoryCode;
            obj.DocumentCateName = doc.CategoryName;
            obj.DocumentCode = doc.Code;
            obj.DocumentDesc = doc.Description;
            obj.DocumentName = obj.DocumentName;
            dao.Save(obj);
            return doc;
        }
        /// <summary>
        /// 文档、文件、MBP对象关联文档 一起保存
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public ProObjectRelaDocument SaveDocumentAndFileAndDocObject(DocumentMaster doc, ProObjectRelaDocument obj)
        {
            doc = SaveDocumentAndFile(doc);
            obj.DocumentGUID = doc.Id;
            dao.Save(obj);
            return obj;
        }
        /// <summary>
        /// 文档、文件、MBP对象关联文档 一起修改
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="dtl"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public ProObjectRelaDocument UpdateDocumentAndFileAndDocObject(DocumentMaster doc, DocumentDetail docFile, ProObjectRelaDocument obj)
        {
            if (docFile != null)
            {
                string errorMsg = "";
                if (docFile.FileDataByte != null && docFile.FileDataByte.Length > 0)
                {
                    string fileName = docFile.FileName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        errorMsg = "文档“" + doc.Name + "”下的文件保存失败,文件名不能为空.";
                    }
                    if (CheckFileExtension(fileName) == false)
                    {
                        errorMsg = "文件“" + fileName + "”保存失败,不支持文件格式:" + Path.GetExtension(fileName);
                    }
                    if (docFile.TheFileCabinet == null)
                    {
                        errorMsg = "文件“" + fileName + "”保存失败,未指定文件柜.";
                    }
                }
                if (!string.IsNullOrEmpty(errorMsg))
                    throw new Exception(errorMsg);
            }

            dao.SaveOrUpdate(doc);

            if (docFile != null)
            {
                SaveFile(doc, docFile);
            }

            dao.SaveOrUpdate(obj);

            return obj;
        }
        /// <summary>
        /// 单独保存（上传）文件
        /// </summary>
        /// <param name="master"></param>
        /// <param name="detai"></param>
        /// <returns></returns>
        [TransManager]
        public DocumentDetail SaveFile(DocumentMaster doc, DocumentDetail docFile)
        {
            try
            {
                string errorMsg = string.Empty;
                string fileTempDir = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["FileUploadTempDir"];

                VirtualMachine.Component.Util.IFCGuidGenerator genObj = new IFCGuidGenerator();//用于生成GUID

                #region 上传文件

                if (!string.IsNullOrEmpty(docFile.FileName) && docFile.FileName.IndexOf(".") > -1)//文件扩展名统一小写
                    docFile.FileName = docFile.FileName.Substring(0, docFile.FileName.LastIndexOf(".")) + Path.GetExtension(docFile.FileName).ToLower();

                if (!string.IsNullOrEmpty(docFile.ExtendName))//文件扩展名统一小写
                    docFile.ExtendName = docFile.ExtendName.ToLower();

                //if (string.IsNullOrEmpty(docFile.Id))
                //    docFile.Id = genObj.GeneratorIFCGuid();

                if (docFile.FileDataByte != null && docFile.FileDataByte.Length > 0)
                {
                    string fileName = docFile.FileName;

                    //if (doc.DocType == DocumentInfoTypeEnum.IFC文件)
                    //{
                    //    UploadFileInBIMServer(doc.Temp1, docFile);
                    //}
                    //else
                    //{
                    //设置基本属性值
                    if (!string.IsNullOrEmpty(fileTempDir) && Directory.Exists(fileTempDir) == false)
                    {
                        Directory.CreateDirectory(fileTempDir);
                    }

                    string DocumentNumber = GetMaxCodeByProjectAndClass(doc.ProjectCode, doc.ProjectName, docFile.GetType());//所属项目流水号
                    fileName = doc.ProjectCode + "." + doc.CategoryCode + "." + DocumentNumber + "V" + doc.VersionMajor + doc.Revision + "_" + fileName;
                    string tempFullName = fileTempDir + "\\" + fileName;
                    WriteBinaryToFile(tempFullName, docFile.FileDataByte);//写到服务端临时文件

                    docFile.FilePartPath = @doc.ProjectCode + "/" + doc.CategoryCode + "/" + fileName;

                    if (FilePropertyUtility.IsEnabledWriteCustomProperty)
                    {
                        //写入【文档代码】、【GUID】、【文档分类码】值到windows文件属性里
                        object docCategoryCode = doc.CategoryCode == null ? "" : doc.CategoryCode;
                        object docCode = doc.Code == null ? "" : doc.Code;
                        object docGUID = doc.Id;
                        object docFileGUID = docFile.Id;

                        Dictionary<string, object> dicKeyValue = new Dictionary<string, object>();
                        dicKeyValue.Add("文档分类码", docCategoryCode);
                        dicKeyValue.Add("文档代码", docCode);
                        dicKeyValue.Add("文档GUID", docGUID);
                        dicKeyValue.Add("文件GUID", docFileGUID);

                        FilePropertyUtility.SetCustomProperty(tempFullName, dicKeyValue);
                    }

                    ToVault(tempFullName, docFile);//上传到文件柜

                    FileInfo fInfo = new FileInfo(tempFullName);//删除临时文件
                    if (fInfo.Exists)
                        fInfo.Delete();
                    //}

                    docFile.FileDataByte = null;
                }
                #endregion

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    throw new Exception(errorMsg);
                }

                dao.SaveOrUpdate(docFile);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return docFile;
        }

        /// <summary>
        /// 上传照片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileDataByte"></param>
        /// <param name="personCode"></param>
        /// <returns></returns>
        [TransManager]
        public Hashtable UpdatePicture(string fileName, byte[] fileDataByte, string personCode)
        {
            string filecabinetId = string.Empty;
            Hashtable hs = new Hashtable();
            VirtualMachine.Component.Util.IFCGuidGenerator genObj = new IFCGuidGenerator();
            try
            {
                string errorMsg = string.Empty;
                FileCabinet appFileCabinet = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("UsedState", UseState.启用));
                IList listFileCabinet = dao.ObjectQuery(typeof(FileCabinet), oq);
                if (listFileCabinet != null && listFileCabinet.Count > 0)
                {
                    appFileCabinet = listFileCabinet[0] as FileCabinet;
                    filecabinetId = appFileCabinet.Id;
                }
                else
                {
                    errorMsg = "请先配置文件柜！";
                    throw new Exception(errorMsg);
                }
                string fileTempDir = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["PictureUploadTempDir"];

                #region 上传照片

                if (!string.IsNullOrEmpty(fileName) && fileName.IndexOf(".") > -1)//文件扩展名统一小写
                    fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + Path.GetExtension(fileName).ToLower();
                fileName = personCode + genObj.GeneratorIFCGuid() + fileName;
                if (fileDataByte != null && fileDataByte.Length > 0)
                {
                    //设置基本属性值
                    if (!string.IsNullOrEmpty(fileTempDir) && Directory.Exists(fileTempDir) == false)
                    {
                        Directory.CreateDirectory(fileTempDir);
                    }
                    string tempFullName = fileTempDir + "\\" + fileName;
                    WriteBinaryToFile(tempFullName, fileDataByte);//写到服务端临时文件

                    //path = personCode + fileName;

                    ToVault(tempFullName, appFileCabinet, fileName);//上传到文件柜

                    FileInfo fInfo = new FileInfo(tempFullName);//删除临时文件
                    if (fInfo.Exists)
                        fInfo.Delete();
                }
                hs.Add(filecabinetId, fileName);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hs;
        }

        /// <summary>
        /// 得到照片在服务端的路径（显示）
        /// </summary>
        /// <param name="theFileCabinetId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetPicturePath(string theFileCabinetId, string fileName)
        {
            string path = "";
            FileCabinet appFileCabinet = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", theFileCabinetId));
            IList listFileCabinet = dao.ObjectQuery(typeof(FileCabinet), oq);
            if (listFileCabinet != null && listFileCabinet.Count > 0)
            {
                appFileCabinet = listFileCabinet[0] as FileCabinet;
                string fileFullPath1 = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                if (!Directory.Exists(fileFullPath1))
                    Directory.CreateDirectory(fileFullPath1);
                string tempFileFullPath = fileFullPath1 + @"\\" + fileName;

                //string address = "http://10.70.18.203//TestFile//Files//0001//0001//0001.0001.000027V_特效.jpg";
                string baseAddress = @appFileCabinet.TransportProtocal.ToString().ToLower() + "://" + appFileCabinet.ServerName + "/" + appFileCabinet.Path + "/";

                string address = baseAddress + "PersonPicture//" + fileName;
                try
                {
                    UtilityClass.WebClientObj.DownloadFile(address, tempFileFullPath);
                    path = tempFileFullPath;
                }
                catch (Exception)
                {
                }
            }
            return path;
        }

        /// <summary>
        /// 得到电子签名路径（显示）
        /// </summary>
        /// <param name="theFileCabinetId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] GetElecSign(string id)
        {
            StandardPerson sp = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList listPerson = dao.ObjectQuery(typeof(StandardPerson), oq);
            if (listPerson.Count == 0) return null;
            sp = listPerson[0] as StandardPerson;
            oq.Criterions.Clear();
            if (sp != null)
            {
                oq.AddCriterion(Expression.Eq("Id", sp.FilecabinetId));
            }
            IList listFileCabinet = dao.ObjectQuery(typeof(FileCabinet), oq);
            if (listFileCabinet != null && listFileCabinet.Count > 0)
            {
                FileCabinet appFileCabinet = listFileCabinet[0] as FileCabinet;
                string baseAddress = @appFileCabinet.TransportProtocal.ToString().ToLower() + "://" + appFileCabinet.ServerName + "/" + appFileCabinet.Path + "/";
#if DEBUG
                string address = baseAddress + "PersonPicture//test-signature-zzb.jpg";
#else
                string address = baseAddress + "PersonPicture//" + sp.Photo;
#endif
                string filepath = address;
                WebClient wc = new WebClient();
                return wc.DownloadData(filepath);
            }
            return null;
        }


        /// <summary>
        /// 得到照片存储路径（删除）
        /// </summary>
        /// <param name="theFileCabinetId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetPictureStoragePath(string theFileCabinetId, string fileName)
        {
            string path = "";
            FileCabinet appFileCabinet = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", theFileCabinetId));
            IList listFileCabinet = dao.ObjectQuery(typeof(FileCabinet), oq);
            if (listFileCabinet != null && listFileCabinet.Count > 0)
            {
                appFileCabinet = listFileCabinet[0] as FileCabinet;
                string baseAddress = @appFileCabinet.TransportProtocal.ToString().ToLower() + "://" + appFileCabinet.ServerName + "/" + appFileCabinet.Path + "/";

                path = baseAddress + "PersonPicture/" + fileName;
            }
            return path;
        }


        //private void UploadFileInBIMServer(string projectId, DocumentDetail docFile)
        //{
        //    try
        //    {
        //        //添加项目节点到BIMServer
        //        JObject opt = new JObject();
        //        JObject req = new JObject();

        //        JObject param = new JObject();
        //        param.Add("interface", "AuthInterface");
        //        param.Add("method", "login");
        //        param.Add("parameters", new JObject(new JProperty("username", BIMServerUtil.UserName), new JProperty("password", BIMServerUtil.UserPwd)));

        //        opt.Add("token", "");
        //        opt.Add("request", param);

        //        string optStr = opt.ToString();
        //        string resultStr = BIMServerUtil.SendRequest(optStr);
        //        JObject resultJson = JObject.Parse(resultStr);
        //        string token = resultJson["response"]["result"].ToString();

        //        if (string.IsNullOrEmpty(token))
        //        {
        //            throw new Exception("登录BIMServer服务器失败,未获取到有效的令牌信息.");
        //        }

        //        //上传文件
        //        JObject getAllDeserializers = new JObject();
        //        getAllDeserializers.Add("token", token);
        //        req.Add("interface", "PluginInterface");
        //        req.Add("method", "getAllDeserializers");
        //        req.Add("parameters", new JObject(new JProperty("onlyEnabled", "true")));
        //        getAllDeserializers.Add("request", req);
        //        resultStr = BIMServerUtil.SendRequest(getAllDeserializers.ToString());
        //        resultJson = JObject.Parse(resultStr);
        //        string deserializerOid = resultJson["response"]["result"][0]["oid"].ToString();

        //        if (deserializerOid == "")
        //        {
        //            throw new Exception("IFC文件上次失败,调用接口PluginInterface的getAllDeserializers方法未返回有效数据.");
        //        }

        //        JObject checkinByProject = new JObject();
        //        checkinByProject.Add("token", token);
        //        req = new JObject();
        //        req.Add("interface", "ServiceInterface");
        //        req.Add("method", "checkin");

        //        JObject checkParam = new JObject();
        //        checkParam.Add("poid", projectId);
        //        checkParam.Add("comment", docFile.FileName);
        //        checkParam.Add("deserializerOid", deserializerOid);
        //        checkParam.Add("fileSize", docFile.FileDataByte.Length);
        //        checkParam.Add("fileName", docFile.FileName);
        //        checkParam.Add("ifcFile", Convert.ToBase64String(docFile.FileDataByte));
        //        checkParam.Add("merge", "false");
        //        checkParam.Add("sync", "true");

        //        req.Add("parameters", checkParam);

        //        checkinByProject.Add("request", req);
        //        resultStr = BIMServerUtil.SendRequest(checkinByProject.ToString());
        //        resultJson = JObject.Parse(resultStr);

        //        if (resultStr == "" || resultJson["response"]["result"].ToString() == "")
        //        {
        //            throw new Exception("IFC文件上次失败,服务器未返回任何响应信息.");
        //        }

        //        //获取最新版本
        //        JObject getProjectLastRevisionId = new JObject();
        //        getProjectLastRevisionId.Add("token", token);
        //        req.Add("interface", "ServiceInterface");
        //        req.Add("method", "getProjectByPoid");
        //        req.Add("parameters", new JObject(new JProperty("poid", projectId)));
        //        getProjectLastRevisionId.Add("request", req);
        //        resultStr = BIMServerUtil.SendRequest(getProjectLastRevisionId.ToString());
        //        resultJson = JObject.Parse(resultStr);
        //        string lastRevisionId = resultJson["response"]["result"][0]["lastRevisionId"].ToString();

        //        if (lastRevisionId == "" || lastRevisionId == "-1")
        //        {
        //            throw new Exception("IFC文件上次失败,未获取到此次上传文件的版本信息.");
        //        }

        //        docFile.BIMProjectRevisionId = lastRevisionId;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 批量保存文件
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="docFileList"></param>
        /// <returns></returns>
        [TransManager]
        public IList SaveFileList(DocumentMaster doc, IList docFileList)
        {
            if (docFileList == null || docFileList.Count == 0)
                return null;

            string errorMsg = "";
            foreach (DocumentDetail docFile in docFileList)
            {
                if (docFile.FileDataByte != null && docFile.FileDataByte.Length > 0)
                {
                    string fileName = docFile.FileName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        errorMsg = "文档“" + doc.Name + "”下的文件保存失败,文件名不能为空.";
                        break;
                    }
                    if (CheckFileExtension(fileName) == false)
                    {
                        errorMsg = "文件“" + fileName + "”保存失败,不支持文件格式:" + Path.GetExtension(fileName);
                        break;
                    }
                    if (docFile.TheFileCabinet == null)
                    {
                        errorMsg = "文件“" + fileName + "”保存失败,未指定文件柜.";
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(errorMsg))
                throw new Exception(errorMsg);

            foreach (DocumentDetail docFile in docFileList)
            {
                SaveFile(doc, docFile);
            }

            return docFileList;
        }

        /// <summary>
        /// 写文件到本地目录
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileByte"></param>
        private void WriteBinaryToFile(string filePath, byte[] fileByte)
        {
            string extName = Path.GetExtension(filePath).ToLower();

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                if (FilePropertyUtility.ListCommonFile.Contains(extName))
                {
                    //写普通文件方式
                    fs.Write(fileByte, 0, fileByte.GetUpperBound(0));
                }
                else if (FilePropertyUtility.ListOfficeFile.Contains(extName))
                {
                    //写office文件方式
                    BinaryWriter w = new BinaryWriter(fs);
                    w.Write(fileByte);
                    w.Close();
                }
                else
                {
                    fs.Write(fileByte, 0, fileByte.GetUpperBound(0));
                }

                fs.Close();
                fs.Dispose();
            }
        }

        private bool CheckFileExtension(string fileName)
        {
            FileInfo fInfo = new FileInfo(fileName);
            if (fInfo.Extension.ToUpper() == ".EXE" || fInfo.Extension.ToUpper() == ".DLL")
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 上传文件时是否使用文件柜配置，true：使用文件柜的配置，false使用app.config中的文件柜地址
        /// </summary>
        private bool IsUseFileCabinetConfig()
        {
            bool _IsUseFileCabinetConfig = true;

            try
            {
                if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["IsUseFileCabinetConfig"]))
                    _IsUseFileCabinetConfig = Convert.ToBoolean(ConfigurationSettings.AppSettings["IsUseFileCabinetConfig"]);
            }
            catch { }

            return _IsUseFileCabinetConfig;
        }

        /// <summary>
        /// 上传文件时是否使用文件柜配置，true：使用文件柜的配置，false使用app.config中的文件柜地址
        /// </summary>
        private string FileCabinetAddress()
        {
            return ConfigurationSettings.AppSettings["FileCabinetAddress"];
        }
        /// <summary>
        /// 上传到文件柜
        /// </summary>
        /// <param name="tempFullName"></param>
        /// <param name="docFile"></param>
        private void ToVault(string tempFullName, DocumentDetail docFile)
        {
            string baseAddress = "";
            if (IsUseFileCabinetConfig())
                baseAddress = @docFile.TheFileCabinet.TransportProtocal.ToString().ToLower() + "://" + docFile.TheFileCabinet.ServerName + "/" + docFile.TheFileCabinet.Path + "/";
            else
                baseAddress = FileCabinetAddress();

            string address = baseAddress + docFile.FilePartPath;//创建子目录存储

            WebClient client = new WebClient();
            client.Credentials = CredentialCache.DefaultCredentials;
            if (docFile.TheFileCabinet != null && (!string.IsNullOrEmpty(docFile.TheFileCabinet.UserName) && !string.IsNullOrEmpty(docFile.TheFileCabinet.UserPwd)))
            {
                NetworkCredential credentials = new NetworkCredential(docFile.TheFileCabinet.UserName, docFile.TheFileCabinet.UserPwd);
                client.Credentials = credentials;//添加网络信道安全，指定用户名密码
            }

            string partDir = docFile.FilePartPath.Substring(0, docFile.FilePartPath.LastIndexOf("/"));
            FileCabinetSrv.FileServiceClient fileProxySrv = new IRPServiceModel.FileCabinetSrv.FileServiceClient();
            fileProxySrv.ClientCredentials.UserName.UserName = "FileCabinetAdministrator";
            fileProxySrv.ClientCredentials.UserName.Password = "1qaz@WSX";
            fileProxySrv.CreateFileDirectionary(partDir);//在文件柜虚拟目录下创建子目录
            fileProxySrv.Close();

            client.UploadFile(address, "PUT", tempFullName);
            client.Dispose();
        }

        /// <summary>
        /// 上传到文件柜（签照）
        /// </summary>
        /// <param name="tempFullName"></param>
        /// <param name="docFile"></param>
        private void ToVault(string tempFullName, FileCabinet theFileCabinet, string fileName)
        {
            string baseAddress = "";
            if (IsUseFileCabinetConfig())
                baseAddress = @theFileCabinet.TransportProtocal.ToString().ToLower() + "://" + theFileCabinet.ServerName + "/" + theFileCabinet.Path + "/";
            else
                baseAddress = FileCabinetAddress();

            //string baseAddress = @theFileCabinet.TransportProtocal.ToString().ToLower() + "://" + theFileCabinet.ServerName + "/" + theFileCabinet.Path + "/";
            string address = baseAddress + "PersonPicture//" + fileName;//创建子目录存储

            WebClient client = new WebClient();
            client.Credentials = CredentialCache.DefaultCredentials;
            if (theFileCabinet != null && (!string.IsNullOrEmpty(theFileCabinet.UserName) && !string.IsNullOrEmpty(theFileCabinet.UserPwd)))
            {
                NetworkCredential credentials = new NetworkCredential(theFileCabinet.UserName, theFileCabinet.UserPwd);
                client.Credentials = credentials;//添加网络信道安全，指定用户名密码
            }

            string partDir = "PersonPicture";
            FileCabinetSrv.FileServiceClient fileProxySrv = new IRPServiceModel.FileCabinetSrv.FileServiceClient();
            fileProxySrv.ClientCredentials.UserName.UserName = "FileCabinetAdministrator";
            fileProxySrv.ClientCredentials.UserName.Password = "1qaz@WSX";
            fileProxySrv.CreateFileDirectionary(partDir);//在文件柜虚拟目录下创建子目录
            fileProxySrv.Close();

            client.UploadFile(address, "PUT", tempFullName);
            client.Dispose();
        }
        private static object objGenCode = new object();
        /// <summary>
        /// 获取项目中对象的最大流水号
        /// </summary>
        /// <param name="projectCode">项目代码</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="classType">实体类型</param>
        /// <returns></returns>
        private string GetMaxCodeByProjectAndClass(string projectCode, string projectName, Type classType)
        {
            string Code = string.Empty;

            lock (objGenCode)
            {
                try
                {
                    string className = classType.FullName + "," + classType.Module.Assembly.FullName.Substring(0, classType.Module.Assembly.FullName.IndexOf(","));

                    ObjectQuery oq = new ObjectQuery();
                    if (!string.IsNullOrEmpty(projectCode))
                    {
                        oq.AddCriterion(Expression.Eq("ProjectCode", projectCode));
                    }
                    else
                    {
                        oq.AddCriterion(Expression.IsNull("ProjectCode"));
                    }

                    if (!string.IsNullOrEmpty(className))
                    {
                        oq.AddCriterion(Expression.Eq("AppClassName", className));
                    }
                    else
                    {
                        oq.AddCriterion(Expression.IsNull("AppClassName"));
                    }

                    IList list = dao.ObjectQuery(typeof(GenerateSerialNumber), oq);

                    GenerateSerialNumber genObj = null;
                    if (list.Count > 0)//如果存在，取最大值
                    {
                        genObj = list[0] as GenerateSerialNumber;
                    }
                    else
                    {
                        genObj = new GenerateSerialNumber();
                        genObj.ProjectCode = projectCode;
                        genObj.ProjectName = projectName;
                        genObj.AppClassName = className;
                        genObj.RuleName = classType.Name + "生成编码规则";
                        genObj.CodeLength = 6;
                        genObj.CurrMaxValue = 1;
                    }

                    int codeLength = genObj.CodeLength;
                    long currMaxValue = genObj.CurrMaxValue;

                    Code = currMaxValue.ToString().PadLeft(codeLength, '0');

                    genObj.CurrMaxValue = genObj.CurrMaxValue + 1;

                    dao.SaveOrUpdate(genObj);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return Code;
        }

        /// <summary>
        /// 初始化文档模板数据
        /// </summary>
        /// <param name="listDocCate"></param>
        /// <param name="listDoc"></param>
        /// <returns></returns>
        [TransManager]
        public bool InitDocumentTemple(IList listDocCate, IList listDoc)
        {
            for (int i = 0; i < listDocCate.Count; i++)
            {
                DocumentCategory childNode = listDocCate[i] as DocumentCategory;

                childNode.CreateDate = DateTime.Now;
                childNode.UpdatedDate = DateTime.Now;

                string operId = "";
                if (childNode.Author == null)
                {
                    try
                    {
                        operId = SecurityUtil.GetLogOperId();
                    }
                    catch { }
                }
                else
                {
                    operId = childNode.Author.Id;
                }
                childNode.Author = Dao.Get(typeof(BusinessOperators), operId) as IBusinessOperators;

                //Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
                //childNode.OwnerGUID = login.ThePerson.Id;
                //childNode.OwnerName = login.ThePerson.Name;
                //childNode.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

                nodeSrv.AddChildNode(childNode);

            }

            AddDocumentByCustomExtend(listDoc.OfType<DocumentMaster>().ToList());

            return true;
        }
        /// <summary>
        /// 根据父节点得到其下一级的子节点集合
        /// </summary>
        /// <param name="level"></param>
        /// <param name="sysCode"></param>
        /// <param name="isModel"></param>
        /// <returns></returns>
        public IList GetDocumentCategoryChildList(int level, string sysCode, bool isModel, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Level", level));
            oq.AddCriterion(Expression.Like("SysCode", sysCode, MatchMode.Start));
            if (isModel)
            {
                oq.AddCriterion(Expression.Eq("ProjectCode", "KB"));
            }
            else
            {
                //Disjunction dis = new Disjunction();
                //dis.Add(Expression.Not(Expression.Eq("ProjectCode", "KB")));
                //dis.Add(Expression.IsNull("ProjectCode"));
                oq.AddCriterion(Expression.Eq("ProjectId", projectId));
                //oq.AddCriterion(dis);
            }
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            return dao.ObjectQuery(typeof(DocumentCategory), oq);
        }

        /// <summary>
        ///  删除单据同时删除其上挂的文档
        /// </summary>
        /// <param name="obj">单据对象</param>
        /// <param name="id">单据id</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteReceiptAndDocument(object obj, string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProObjectGUID", id));
            IList list = dao.ObjectQuery(typeof(ProObjectRelaDocument), oq);
            DeleteProObjectRelaDocumentAndDocument(list);

            oq = new ObjectQuery();
            Disjunction dis = Expression.Disjunction();
            //重置逐日派工单的引用数量
            var master = obj as LaborSporadicMaster;
            if (master != null && master.Details != null && master.Details.Count != 0)
            {
                foreach (LaborSporadicDetail item in master.Details)
                {
                    if (!string.IsNullOrEmpty(item.ForwardDetailId))
                    {
                        dis.Add(Expression.Eq("Id", item.ForwardDetailId));
                    }
                }
                oq.AddCriterion(dis);
                var lstDetail = dao.ObjectQuery(typeof(LaborSporadicDetail), oq).OfType<LaborSporadicDetail>().ToList();

                if (lstDetail != null && lstDetail.Count != 0)
                {
                    foreach (var detail in lstDetail)
                    {
                        detail.RefQuantity = 0;
                    }
                    dao.Update(lstDetail);
                }
            }

            return dao.Delete(obj);
        }

        /// <summary>
        ///  删除单据其上挂的文档
        /// </summary>
        /// <param name="obj">单据对象</param>
        /// <param name="id">单据id</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteDocument(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProObjectGUID", id));
            IList list = dao.ObjectQuery(typeof(ProObjectRelaDocument), oq);

            return DeleteProObjectRelaDocumentAndDocument(list);
        }

        #region 上传单一文件
        /// <summary>
        /// 上传单一文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileDataByte"></param>
        /// <param name="personCode"></param>
        /// <returns></returns>
        [TransManager]
        public Hashtable UploadSingleFile(string fileName, byte[] fileDataByte, string fileDir)
        {
            string filecabinetId = string.Empty;
            Hashtable hs = new Hashtable();
            VirtualMachine.Component.Util.IFCGuidGenerator genObj = new IFCGuidGenerator();
            try
            {
                string errorMsg = string.Empty;
                FileCabinet appFileCabinet = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("UsedState", UseState.启用));
                IList listFileCabinet = dao.ObjectQuery(typeof(FileCabinet), oq);
                if (listFileCabinet != null && listFileCabinet.Count > 0)
                {
                    appFileCabinet = listFileCabinet[0] as FileCabinet;
                    filecabinetId = appFileCabinet.Id;
                }
                else
                {
                    errorMsg = "请先配置文件柜！";
                    throw new Exception(errorMsg);
                }
                string fileTempDir = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["PictureUploadTempDir"];

                #region 上传照片

                if (!string.IsNullOrEmpty(fileName) && fileName.IndexOf(".") > -1)//文件扩展名统一小写
                    fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + Path.GetExtension(fileName).ToLower();
                //fileName = genObj.GeneratorIFCGuid() + fileName;
                string address = "";//返回地址
                if (fileDataByte != null && fileDataByte.Length > 0)
                {
                    //设置基本属性值
                    if (!string.IsNullOrEmpty(fileTempDir) && Directory.Exists(fileTempDir) == false)
                    {
                        Directory.CreateDirectory(fileTempDir);
                    }
                    string tempFullName = fileTempDir + "\\" + fileName;
                    WriteBinaryToFile(tempFullName, fileDataByte);//写到服务端临时文件

                    //path = personCode + fileName;

                    address = ToSingleVault(tempFullName, appFileCabinet, fileName, "");//上传到文件柜

                    FileInfo fInfo = new FileInfo(tempFullName);//删除临时文件
                    if (fInfo.Exists)
                        fInfo.Delete();
                }
                hs.Add(filecabinetId, address);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hs;
        }
        /// <summary>
        /// 上传到文件柜（单一文件）
        /// </summary>
        /// <param name="tempFullName"></param>
        /// <param name="docFile"></param>
        private string ToSingleVault(string tempFullName, FileCabinet theFileCabinet, string fileName, string fileDir)
        {
            string baseAddress = "";
            if (IsUseFileCabinetConfig())
                baseAddress = @theFileCabinet.TransportProtocal.ToString().ToLower() + "://" + theFileCabinet.ServerName + "/" + theFileCabinet.Path + "/";
            else
                baseAddress = FileCabinetAddress();

            //string baseAddress = @theFileCabinet.TransportProtocal.ToString().ToLower() + "://" + theFileCabinet.ServerName + "/" + theFileCabinet.Path + "/";
            string address = baseAddress + "QRCode//" + fileName;//创建子目录存储

            WebClient client = new WebClient();
            client.Credentials = CredentialCache.DefaultCredentials;
            if (theFileCabinet != null && (!string.IsNullOrEmpty(theFileCabinet.UserName) && !string.IsNullOrEmpty(theFileCabinet.UserPwd)))
            {
                NetworkCredential credentials = new NetworkCredential(theFileCabinet.UserName, theFileCabinet.UserPwd);
                client.Credentials = credentials;//添加网络信道安全，指定用户名密码
            }

            string partDir = "QRCode";
            FileCabinetSrv.FileServiceClient fileProxySrv = new IRPServiceModel.FileCabinetSrv.FileServiceClient();
            fileProxySrv.ClientCredentials.UserName.UserName = "FileCabinetAdministrator";
            fileProxySrv.ClientCredentials.UserName.Password = "1qaz@WSX";
            fileProxySrv.CreateFileDirectionary(partDir);//在文件柜虚拟目录下创建子目录
            fileProxySrv.Close();

            client.UploadFile(address, "PUT", tempFullName);
            client.Dispose();
            string urlAddress = @theFileCabinet.TransportProtocal.ToString().ToLower() + "://" + theFileCabinet.ServerName + "/" + theFileCabinet.Path + "/" + "QRCode//" + fileName;
            return urlAddress;
        }
        #endregion

    }

}
