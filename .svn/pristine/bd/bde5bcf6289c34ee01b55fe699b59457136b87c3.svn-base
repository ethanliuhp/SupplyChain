using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using VirtualMachine.Core;
using IRPServiceModel.Domain.Document;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;

namespace IRPServiceModel.Services.Document
{
    /// <summary>
    /// 文档服务
    /// </summary>
    public interface IDocumentSrv
    {
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);

        /// <summary>
        /// 保存或修改对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        IList SaveOrUpdate(IList list);
        FileCabinet SaveOrUpdate(FileCabinet obj);
        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        object SaveOrUpdate(object obj);

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        bool Delete(IList list);
        bool Delete(FileCabinet obj);

        /// <summary>
        /// 删除工程文档验证和文档
        /// </summary>
        /// <param name="list">工程文档验证集合</param>
        /// <returns></returns>
        bool DeleteProjectDocumentVerifyAndDocument(IList list);
        /// <summary>
        /// 删除工程对象关联文档和文档
        /// </summary>
        /// <param name="list">工程对象关联对象集合</param>
        /// <returns></returns>
        bool DeleteProObjectRelaDocumentAndDocument(IList list);

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetCode(Type type);

        /// <summary>
        /// 保存文档分类树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        DocumentCategory SaveCateTree(DocumentCategory childNode);

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        IList SaveCateTree(IList lst);

        /// <summary>
        /// 删除文档分类树节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        bool DeleteCateTree(DocumentCategory cate);

        /// <summary>
        /// 上传(保存)文档
        /// </summary>
        /// <param name="listDocument">项目文档对象集</param>
        /// <returns>保存后的项目文档对象集</returns>
        List<DocumentMaster> AddDocumentByCustomExtend(List<DocumentMaster> listDocument);

        /// <summary>
        /// 文档文件一起保存(新增用)
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        DocumentMaster SaveDocumentAndFile(DocumentMaster doc);
        /// <summary>
        ///  文档文件一起保存(修改用)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="docFileList"></param>
        /// <returns></returns>
        DocumentMaster SaveDocumentAndFile(DocumentMaster doc, IList docFileList);
        /// <summary>
        /// 文档、文件、MBP对象关联文档 一起保存(用于表单)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="id">MBP对象id</param>
        /// <returns></returns>
        DocumentMaster SaveDocumentAndFileAndDocObject(DocumentMaster doc, string id);
        /// <summary>
        /// 文档、文件、MBP对象关联文档 一起保存
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        ProObjectRelaDocument SaveDocumentAndFileAndDocObject(DocumentMaster doc, ProObjectRelaDocument obj);
        /// <summary>
        /// 文档、文件、MBP对象关联文档 一起修改
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="dtl"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        ProObjectRelaDocument UpdateDocumentAndFileAndDocObject(DocumentMaster doc, DocumentDetail dtl, ProObjectRelaDocument obj);
        /// <summary>
        /// 单独保存（上传）文件
        /// </summary>
        /// <param name="master"></param>
        /// <param name="detai"></param>
        /// <returns></returns>
        DocumentDetail SaveFile(DocumentMaster doc, DocumentDetail docFile);

        /// <summary>
        /// 上传照片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileDataByte"></param>
        /// <param name="personCode"></param>
        /// <returns></returns>
        Hashtable UpdatePicture(string fileName, byte[] fileDataByte, string personCode);
        /// <summary>
        /// 得到照片路径（显示 临时路径）
        /// </summary>
        /// <param name="theFileCabinetId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GetPicturePath(string theFileCabinetId, string fileName);

        /// <summary>
        /// 得到照片存储路径（删除）
        /// </summary>
        /// <param name="theFileCabinetId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GetPictureStoragePath(string theFileCabinetId, string fileName);

        /// <summary>
        /// 批量保存文件
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="docFileList"></param>
        /// <returns></returns>
        IList SaveFileList(DocumentMaster doc, IList docFileList);

        /// <summary>
        /// 初始化文档模板数据
        /// </summary>
        /// <param name="listDocCate"></param>
        /// <param name="listDoc"></param>
        /// <returns></returns>
        bool InitDocumentTemple(IList listDocCate, IList listDoc);

        /// <summary>
        /// 根据父节点得到其下一级的子节点集合
        /// </summary>
        /// <param name="level"></param>
        /// <param name="sysCode"></param>
        /// <param name="isModel"></param>
        /// <returns></returns>
        IList GetDocumentCategoryChildList(int level, string sysCode, bool isModel, string projectId);

         /// <summary>
        ///  删除单据同时删除其上挂的文档
        /// </summary>
        /// <param name="obj">单据对象</param>
        /// <param name="id">单据id</param>
        /// <returns></returns>
        bool DeleteReceiptAndDocument(object obj, string id);
        byte[] GetElecSign(string  id);

        bool DeleteDocument(string id);
        Hashtable UploadSingleFile(string fileName, byte[] fileDataByte, string fileDir);
    }
}
