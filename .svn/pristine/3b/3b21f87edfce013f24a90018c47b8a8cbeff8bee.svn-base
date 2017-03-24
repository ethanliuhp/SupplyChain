using System;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorDefine.Domain;
using VirtualMachine.Core;
using System.Collections;
namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorDefine.Service
{
    public interface IIndicatorDefineService
    {
        /// <summary>
        /// 删除指标分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteCategory(IndicatorCategory obj);
        
        /// <summary>
        /// 删除指标
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteIndicatorDefinition(IndicatorDefinition obj);

        /// <summary>
        /// 根据ID查找指标分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IndicatorCategory GetCategoryById(string id);

        /// <summary>
        /// 获取指标分类节点集合
        /// </summary>
        /// <returns></returns>
        IList GetCategorys();

        /// <summary>
        /// 根据指标分类节点查找指标
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        IList GetIndicatorDefinitionByCategory(IndicatorCategory category);

        /// <summary>
        /// 根据ID查找指标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IndicatorDefinition GetIndicatorDefinitionById(long id);

        /// <summary>
        /// 根据条件查找指标
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetIndicatorDefinitionByQuery(ObjectQuery oq);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IList InvalidateCategory(IndicatorCategory obj);

        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="toObj"></param>
        /// <returns></returns>
        IndicatorCategory MoveCategory(IndicatorCategory obj, IndicatorCategory toObj);

        /// <summary>
        /// 保存指标分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IndicatorCategory SaveCategory(IndicatorCategory obj);

        /// <summary>
        /// 保存指标分类集合
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        IList SaveCategorys(IList lst);

        /// <summary>
        /// 保存指标
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IndicatorDefinition SaveIndicatorDefinition(IndicatorDefinition obj);
    }
}
