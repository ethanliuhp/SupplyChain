using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ImplementationPlan.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;

namespace Application.Business.Erp.SupplyChain.ImplementationPlan.Service
{

        /// <summary>
        /// 项目实施策划书服务
        /// </summary>
        public interface IImplementSrv : IBaseService
        {
            #region 项目实施策划书
            ImplementationMaintain GetImpById(string id);
            ImplementationMaintain GetImpByCode(string code);
            IList GetImp(ObjectQuery objectQuery);
            ImplementationMaintain saveImp(ImplementationMaintain obj);
            DataSet ImplementationQuery(string condition);
            #endregion
            /// <summary>
            /// 删除对象集合
            /// </summary>
            /// <param name="list">对象集合</param>
            /// <returns></returns>
            [TransManager]
            bool Delete(IList list);
            /// <summary>
            /// 保存或修改项目实施策划书
            /// </summary>
            /// <param name="item">项目实施策划书</param>
            /// <returns></returns>
            [TransManager]
            ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument

    item);
            /// <summary>
            /// 对象查询
            /// </summary>
            /// <param name="entityType">实体类型</param>
            /// <param name="oq">查询条件</param>
            /// <returns></returns>
            IList ObjectQuery(Type entityType, ObjectQuery oq);

        }

}
