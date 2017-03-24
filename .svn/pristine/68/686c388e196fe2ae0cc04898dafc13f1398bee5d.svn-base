using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using VirtualMachine.Core;
using System.Data;
using Application.Business.Erp.SupplyChain.EngineerManage.MeetingManage.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;

namespace Application.Business.Erp.SupplyChain.EngineerManage.MeetingManage.Service
{
        // <summary>
        /// 会议管理服务
        /// </summary>
        public interface IMeetingMngSrv : IBaseService
        {
            #region 会议纪要管理
            MeetingMng GetImpById(string id);
            MeetingMng GetImpByCode(string code);
            IList GetImp(ObjectQuery objectQuery);
            MeetingMng saveImp(MeetingMng obj);
            //DataSet ImplementationQuery(string condition);
            #endregion
            IList ObjectQuery(Type entityType, ObjectQuery oq);
            bool Delete(IList list);
           DataSet MeetingQuery(string condition);
           /// <summary>
           /// 保存或修改会议管理
           /// </summary>
           /// <param name="item">工程对象关联文档</param>
           /// <returns></returns>
           [TransManager]
           ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument

   item);
        }
    }

