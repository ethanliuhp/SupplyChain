using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRPServiceModel.Services.Document;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using VirtualMachine.Core;
using System.Collections;
using IRPServiceModel.Domain.Document;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng
{
    public class MDocumentCategory
    {
        public IDocumentSrv docSrv;
        public MDocumentCategory()
        {
            if (docSrv == null)
                docSrv = ConstMethod.GetService("DocumentSrv") as IDocumentSrv;
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return docSrv.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 保存或修改对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        public IList SaveOrUpdate(IList list)
        {
            return docSrv.SaveOrUpdate(list);
        }

        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public object SaveOrUpdate(object obj)
        {
            return docSrv.SaveOrUpdate(obj);
        }

        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public FileCabinet SaveOrUpdatefile(FileCabinet obj)
        {
            return docSrv.SaveOrUpdate(obj);
        }

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        public bool Delete(IList list)
        {
            return docSrv.Delete(list);
        }
        public bool Delete(FileCabinet list)
        {
            return docSrv.Delete(list);
        }

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type)
        {
            return docSrv.GetCode(type);
        }

        /// <summary>
        /// 保存文档分类树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        public DocumentCategory SaveCateTree(DocumentCategory childNode)
        {
            return docSrv.SaveCateTree(childNode);
        }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        public IList SaveCateTree(IList lst)
        {
            return docSrv.SaveCateTree(lst);
        }

        /// <summary>
        /// 删除文档分类树节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCateTree(DocumentCategory cate)
        {
            return docSrv.DeleteCateTree(cate);
        }
        /// <summary>
        /// 根据ID得到父节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DocumentCategory GetCateTreeById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = docSrv.ObjectQuery(typeof(DocumentCategory), oq);
            if (list.Count == 1)
                return list[0] as DocumentCategory;
            else
                return null;
        }


        /// <summary>
        /// 上传(保存)文档
        /// </summary>
        /// <param name="listDocument">项目文档对象集</param>
        /// <returns>保存后的项目文档对象集</returns>
        public List<DocumentMaster> AddDocumentByCustomExtend(List<DocumentMaster> listDocument)
        {
            return docSrv.AddDocumentByCustomExtend(listDocument);
        }

        /// <summary>
        /// 文档文件一起保存(新增用)
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        public DocumentMaster SaveDocumentAndFile(DocumentMaster doc)
        {
            return docSrv.SaveDocumentAndFile(doc);
        }
        /// <summary>
        ///  文档文件一起保存(修改用)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="docFileList"></param>
        /// <returns></returns>
        public DocumentMaster SaveDocumentAndFile(DocumentMaster doc, IList docFileList)
        {
            return docSrv.SaveDocumentAndFile(doc, docFileList);
        }

        /// <summary>
        /// 文档、文件、MBP对象关联文档 一起保存(用于表单)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="id">MBP对象id</param>
        /// <returns></returns>
        public DocumentMaster SaveDocumentAndFileAndDocObject(DocumentMaster doc, string id)
        {
            return docSrv.SaveDocumentAndFileAndDocObject(doc, id);
        }
         /// <summary>
        /// 文档、文件、MBP对象关联文档 一起保存
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ProObjectRelaDocument SaveDocumentAndFileAndDocObject(DocumentMaster doc, ProObjectRelaDocument obj)
        {
            return docSrv.SaveDocumentAndFileAndDocObject(doc, obj);
        }
        /// <summary>
        /// 文档、文件、MBP对象关联文档 一起修改
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="dtl"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ProObjectRelaDocument UpdateDocumentAndFileAndDocObject(DocumentMaster doc, DocumentDetail dtl, ProObjectRelaDocument obj)
        {
            return docSrv.UpdateDocumentAndFileAndDocObject(doc, dtl, obj);
        }
        /// <summary>
        /// 单独保存（上传）文件
        /// </summary>
        /// <param name="master"></param>
        /// <param name="detai"></param>
        /// <returns></returns>
        public DocumentDetail SaveFile(DocumentMaster doc, DocumentDetail docFile)
        {
            return SaveFile(doc, docFile);
        }

        /// <summary>
        /// 批量保存文件
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="docFileList"></param>
        /// <returns></returns>
        public IList SaveFileList(DocumentMaster doc, IList docFileList)
        {
            return docSrv.SaveFileList(doc, docFileList);
        }

        /// <summary>
        /// 删除工程文档验证和文档
        /// </summary>
        /// <param name="list">工程文档验证集合</param>
        /// <returns></returns>
        public bool DeleteProjectDocumentVerifyAndDocument(IList list)
        {
            return docSrv.DeleteProjectDocumentVerifyAndDocument(list);
        }
        /// <summary>
        /// 删除工程对象关联文档和文档
        /// </summary>
        /// <param name="list">工程对象关联对象集合</param>
        /// <returns></returns>
        public bool DeleteProObjectRelaDocumentAndDocument(IList list)
        {
            return docSrv.DeleteProObjectRelaDocumentAndDocument(list);
        }

        /// <summary>
        /// 根据父节点得到其下一级的子节点集合
        /// </summary>
        /// <param name="level"></param>
        /// <param name="sysCode"></param>
        /// <param name="isModel"></param>
        /// <returns></returns>
        public IList GetDocumentCategoryChildList(int level, string sysCode,bool isModel,string projectId)
        {
            return docSrv.GetDocumentCategoryChildList(level, sysCode, isModel, projectId);
        }


          /// <summary>
        ///  删除单据同时删除其上挂的文档
        /// </summary>
        /// <param name="obj">单据对象</param>
        /// <param name="id">单据id</param>
        /// <returns></returns>
        public bool DeleteReceiptAndDocument(object obj, string id)
        {
            return docSrv.DeleteReceiptAndDocument(obj, id);
        }
    }
}
