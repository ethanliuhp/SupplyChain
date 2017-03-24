using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.UtilityControlService
{
    public interface IUtilityControlSrv
    {
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);
    }
}
