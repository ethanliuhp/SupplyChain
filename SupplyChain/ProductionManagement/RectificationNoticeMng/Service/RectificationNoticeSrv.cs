using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Service
{


    /// <summary>
    /// 整改通知单
    /// </summary>
    public class RectificationNoticeSrv : BaseService, IRectificationNoticeSrv
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }

        //private string GetCode(Type type,string specail)
        //{
        //    return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, specail);
        //}

        /// <summary>
        /// 根据项目 物资分类(专业分类) 生成Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <param name="matCatAbb"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId, string matCatAbb)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId, matCatAbb);
        }
        #endregion

        #region 整改通知单
        /// <summary>
        /// 通过ID查询整改通知单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RectificationNoticeMaster GetRectificationNoticeById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetRectificationNotice(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as RectificationNoticeMaster;
            }
            return null;
        }

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            bool flag = false;
            foreach (ProObjectRelaDocument cg in list)
            {
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    dis.Add(Expression.Eq("Id", cg.Id));
                    flag = true;
                }
            }
            if (flag)
            {
                oq.AddCriterion(dis);

                IList listTemp = dao.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                if (listTemp != null && listTemp.Count > 0)
                    dao.Delete(listTemp);
            }

            return true;
        }

        /// <summary>
        /// 通过Code查询整改通知单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public RectificationNoticeMaster GetRectificationNoticeByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetRectificationNotice(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as RectificationNoticeMaster;
            }
            return null;
        }

        /// <summary>
        /// 整改通知单信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetRectificationNotice(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ForwordInsLot", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProfessionDetail", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.AccepIns", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProfessionDetail.Master", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("SupplierUnit", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(RectificationNoticeMaster), objectQuery);
        }
        /// <summary>
        /// 整改通知单明细信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetRectificationDetail(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master.SupplierUnit", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("ForwordInsLot", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("ForwordInsLot.GWBSTree", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("ProfessionDetail", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("AccepIns", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(RectificationNoticeDetail), objectQuery);
        }

        /// <summary>
        /// 整改通知单信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet RectificationNoticeQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT t1.id,t1.Code,t1.State,t1.InspectionType,t1.SupplierUnitName,t1.RealOperationDate,t1.CreateDate,t1.CreatePersonName,t2.RequiredDate,t2.RectDate,t2.RectConclusion,t1.HandlePersonName,t2.QuestionState,t2.ProblemCode,t2.Rectrequired,t2.RectContent,t2.RectSendDate FROM Thd_Rectificatnoticemaster  t1 INNER JOIN Thd_Rectificatnoticedetail  t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }

        /// <summary>
        /// 保存主表信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public RectificationNoticeMaster SaveRectificationNotice(RectificationNoticeMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(RectificationNoticeMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as RectificationNoticeMaster;
        }

        /// <summary>
        /// 保存主表信息(用于把日常检查 相应文档带过来)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public RectificationNoticeMaster SaveRectificationNoticeOne(RectificationNoticeMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(RectificationNoticeMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return saveCopyDocument(obj);
        }

        /// <summary>
        /// 提交操作
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        [TransManager]
        public RectificationNoticeMaster SaveRectificationNotice(RectificationNoticeMaster obj, IList list, int i)
        {
            if (i == 0)
            {
                foreach (InspectionRecord record in list)
                {
                    SaveRecord(record);
                }
            }
            else
            {
                foreach (ProfessionInspectionRecordDetail record in list)
                {
                    SaveProfession(record);
                }
            }
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(RectificationNoticeMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as RectificationNoticeMaster;
        }
        /// <summary>
        /// 提交操作(用于把日常检查 相应文档带过来)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        [TransManager]
        public RectificationNoticeMaster SaveRectificationNoticeOne(RectificationNoticeMaster obj, IList list, int i)
        {
            if (list != null && list.Count > 0)
            {
                if (list[0] is InspectionRecord)
                {
                    foreach (InspectionRecord record in list)
                    {
                        SaveRecord(record);
                    }
                }
                else if (list[0] is ProfessionInspectionRecordDetail)
                {
                    foreach (ProfessionInspectionRecordDetail record in list)
                    {
                        SaveProfession(record);
                    }
                }
            }
            //if (i == 0)
            //{
            //    foreach (InspectionRecord record in list)
            //    {
            //        SaveRecord(record);
            //    }
            //}
            //else
            //{
            //    foreach (ProfessionInspectionRecordDetail record in list)
            //    {
            //        SaveProfession(record);
            //    }
            //}
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(RectificationNoticeMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return saveCopyDocument(obj);
        }
        public RectificationNoticeMaster saveCopyDocument(RectificationNoticeMaster master)
        {
            #region 过滤出修改的
            IList listUpdate = new ArrayList();
            IList listNew = new ArrayList();
            foreach (RectificationNoticeDetail detail in master.Details)
            {
                if (detail.Id != null && detail.Id != "")
                {
                    listUpdate.Add(detail);
                }
            }
            master = SaveOrUpdateByDao(master) as RectificationNoticeMaster;

            foreach (RectificationNoticeDetail d in master.Details)
            {
                bool isNew = true;
                foreach (RectificationNoticeDetail update in listUpdate)
                {
                    if (d.Id == update.Id && d.ForwordInsLot != null && d.ForwordInsLot.Id != null)
                        isNew = false;
                }
                if (isNew)
                    listNew.Add(d);
            }
            #endregion
           
            if (listNew != null && listNew.Count > 0)
            {
                IList saveList = new ArrayList();

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();

                foreach (RectificationNoticeDetail dtl in listNew)
                {
                    if (dtl.ForwordInsLot != null)
                    {
                        dis.Add(Expression.Eq("ProObjectGUID", dtl.ForwordInsLot.Id));
                    }
                }
               
                oq.AddCriterion(dis);
                IList listObj = dao.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                if (listObj != null && listObj.Count > 0)
                {
                    foreach (RectificationNoticeDetail rDtl in listNew)
                    {
                        foreach (ProObjectRelaDocument objDoc in listObj)
                        {
                            if (objDoc.ProObjectGUID == rDtl.ForwordInsLot.Id)
                            {
                                ProObjectRelaDocument doc = new ProObjectRelaDocument();
                                doc.ProObjectGUID = rDtl.Id;
                                doc.DocumentGUID = objDoc.DocumentGUID;
                                doc.DocumentCateCode = objDoc.DocumentCateCode;
                                doc.DocumentCateName = objDoc.DocumentCateName;
                                doc.DocumentCode = objDoc.DocumentCode;
                                doc.DocumentDesc = objDoc.DocumentDesc;
                                doc.DocumentName = objDoc.DocumentName;
                                saveList.Add(doc);
                            }
                        }
                    }
                    if (saveList != null && saveList.Count > 0)
                        dao.Save(saveList);
                }
            }
            return master;
        }
        /// <summary>
        /// 保存明细信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public RectificationNoticeDetail SaveRectificationNotice(RectificationNoticeDetail obj)
        {
            return SaveOrUpdateByDao(obj) as RectificationNoticeDetail;
        }



        /// <summary>
        /// 根据明细Id查询整改通知单明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public RectificationNoticeDetail GetRectificationNoticeDetailById(string RecDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", RecDtlId));
            IList list = dao.ObjectQuery(typeof(RectificationNoticeDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as RectificationNoticeDetail;
            }
            return null;
        }

        [TransManager]
        public InspectionRecord SaveRecord(InspectionRecord obj)
        {
            return SaveOrUpdateByDao(obj) as InspectionRecord;
        }


        [TransManager]
        public ProfessionInspectionRecordDetail SaveProfession(ProfessionInspectionRecordDetail obj)
        {
            return SaveOrUpdateByDao(obj) as ProfessionInspectionRecordDetail;
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        [TransManager]
        public ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item)
        {
            dao.SaveOrUpdate(item);

            return item;
        }
        /// <summary>
        /// 通过ID查询专业检查记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProfessionInspectionRecordMaster GetProfessionInspectionRecordById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list = Dao.ObjectQuery(typeof(ProfessionInspectionRecordMaster), objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ProfessionInspectionRecordMaster;
            }
            return null;
        }
        /// <summary>
        /// 查询检查记录单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetInspectionRecord(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("PBSTree", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("RectificationNotices", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("RectificationNotices.SubjectOrganization", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("RectificationNotices.SupjectOrgPerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("SitePictureVideos", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("WeekScheduleDetail", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("WeekScheduleDetail.GWBSTree", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(InspectionRecord), objectQuery);
        }
        #endregion
    }





}
