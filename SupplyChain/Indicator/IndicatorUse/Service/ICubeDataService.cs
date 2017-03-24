using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;

/**
 * 立方数据接口
 * 描述：对立方中的数据进行汇总、维护等操作
 */
namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service
{
    public interface ICubeDataService
    {
        /// <summary>
        /// 新增立方数据
        /// </summary>
        /// <returns></returns>
        CubeData AddCubeData(CubeData obj);

        /// <summary>
        /// 修改立方数据
        /// </summary>
        /// <returns></returns>
        CubeData UpdateCubeData(CubeData obj);
        
        // <summary>
        /// 删除立方数据
        /// </summary>
        /// <returns></returns>
        bool DeleteCubeData(CubeData obj);
        
        // <summary>
        /// 通过条件查询立方数据
        /// </summary>
        /// <returns></returns>
        IList GetCubeDataByQuery(string tableName,string query);

        // <summary>
        /// 判断传入的立方数据对象的合法性
        /// </summary>
        /// <returns></returns>
        bool CheckCubeData(CubeData obj);


    }
}
