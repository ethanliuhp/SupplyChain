using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.EngineerManage.MeetingManage.Domain;
using Application.Business.Erp.SupplyChain.EngineerManage.MeetingManage.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using System.Collections;
using VirtualMachine.Core;
using CommonSearch.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary
{
 public   class MMeetingManage
    {

        private IMeetingMngSrv iMeetingMngSrv;

        public IMeetingMngSrv IMeetingMngSrv
        {
            get { return iMeetingMngSrv; }
            set { iMeetingMngSrv = value; }
        }

        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;

        public MMeetingManage()
        {
            if (iMeetingMngSrv == null)
            {
                iMeetingMngSrv = StaticMethod.GetService("MeetingMngSrv") as IMeetingMngSrv;
            }
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
        }

        /// <summary>
        /// 保存会议纪要信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MeetingMng saveImp(MeetingMng obj)
        {
            return iMeetingMngSrv.saveImp(obj);
        }
        /// <summary>
        /// 删除工程对象关联文档对象集合
        /// </summary>
        /// <param name="list">工程对象关联文档对象集合</param>
        /// <returns></returns>
        public bool DeleteProObjRelaDoc(IList list)
        {
            return modelDoc.DeleteProObjRelaDoc(list);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return model.ObjectQuery(entityType, oq);
        }
    }
}
