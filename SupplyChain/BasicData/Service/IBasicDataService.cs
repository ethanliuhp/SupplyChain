using System;
using Application.Business.Erp.SupplyChain.BasicData.Domain;
using System.Collections;
namespace Application.Business.Erp.SupplyChain.BasicData.Service
{
    public interface IBasicDataService
    {
        bool DeleteBasicDatas(BasicDatas obj);
        IList ListAllBasicDatas();
        BasicDatas SaveBasicDatas(BasicDatas obj);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="deletedDetail"></param>
        /// <returns></returns>
        BasicDatas SaveBasicDatas(BasicDatas obj, IList deletedDetail);

        /// <summary>
        /// 保存明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        BasicDataDetail SaveBasicDataDetail(BasicDataDetail obj);

        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteBasicDataDetail(BasicDataDetail obj);

        /// <summary>
        /// 根据主表查询明细
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        IList GetDetailByMaster(BasicDatas bd);

        /// <summary>
        /// 根据表名查询明细
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList GetDetailByBasicTableName(string name);
    }
}
