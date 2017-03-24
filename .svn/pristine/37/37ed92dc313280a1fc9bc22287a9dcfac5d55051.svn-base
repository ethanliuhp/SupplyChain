using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.HelpOnlineManage.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearch.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.HelpOnlineManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.HelpOnline
{
    public   class MHelpOnline
    {
        private IHelpOnlineSrv iHelpOnlineSrv;

        public IHelpOnlineSrv IHelpOnlineSrv
        {
            get { return iHelpOnlineSrv; }
            set { iHelpOnlineSrv = value; }
        }
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;
        public MHelpOnline()
        {
            if (iHelpOnlineSrv == null)
            {
                iHelpOnlineSrv = StaticMethod.GetService("HelpOnlineSrv") as IHelpOnlineSrv;
            }
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
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
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="obj"></p aram>
        /// <returns></returns>
        public HelpOnlineMng saveImp(HelpOnlineMng obj)
        {
            return iHelpOnlineSrv.saveImp(obj);
        }
    }
}
