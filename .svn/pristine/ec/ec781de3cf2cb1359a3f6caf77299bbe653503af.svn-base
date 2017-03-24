using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

/**
 * 维度定义接口
 * 描述：提供统一的方式对各维度树进行维护
 *       树的根节点为"root",无父节点
 *       各维度的根节点位于整棵维度树的第一级节点。
 */
namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service
{
    public interface IDimensionDefineService
    {

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Delete(object obj);

        /// <summary>
        /// 通过系统名称查询系统代码
        /// </summary>
        /// <param name="catName"></param>
        /// <returns></returns>
        SystemRegister GetSysteCodeByName(String systemName);

        /// <summary>
        /// 删除维度分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteDimensionCategory(DimensionCategory obj);

         // <summary>
        /// 修改维度值的节点类型
        /// </summary>
        /// <param name="dimId">维度值ID</param>
        /// <param name="nodetype">节点类型</param>
        bool UpdateDimNodeType(string dimId, int nodetype);

        /// <summary>
        /// 根据ID查找维度分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DimensionCategory GetDimensionCategoryById(string id);

        /// <summary>
        /// 获取所有维度的ID和名称的对应
        /// </summary>
        /// <returns></returns>
        Hashtable GetDimensionCategorysBySql();

        /// <summary>
        /// 根据ID查找维度
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DimensionDefine GetDimensionDefineById(string id);


        /// <summary>
        /// 获取维度分类节点集合
        /// </summary>
        /// <returns></returns>
        IList GetDimensionCategorys();

        /// <summary>
        /// 通过维度CODE和系统登录ID查询维度ID
        /// </summary>
        /// <returns></returns>
        long GetDimensionIdByCode(string code, string registerId);

        /// <summary>
        /// 根据条件查找维度分类
        /// </summary>
        /// <param name="oq"></param>
        /// <returns>IList为维度分类的集合</returns>
        IList GetDimensionCategoryByQuery(ObjectQuery oq);

        /// <summary>
        /// 获取登录子系统的维度分类节点集合
        /// </summary>
        /// <param name="systemId">分系统ID</param>
        /// <returns></returns>
        IList GetPartDimensionCategorys(string systemId);

        /// <summary>
        /// 获取某个节点的所有子节点
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        IList GetAllChildNodes(CategoryNode cat);

        /// <summary>
        /// 根据分类名称查找所有子节点
        /// </summary>
        /// <param name="catName"></param>
        /// <returns></returns>
        IList GetAllChildNodesByName(String catName);

        /// <summary>
        /// 通过维度分类的SysCode查找所有子节点,指标采集专用
        /// </summary>
        /// <param name="catName"></param>
        /// <returns></returns>
        IList GetAllChildNodesBySysCodeByDBDao(String sysCode);

         /// <summary>
        /// 通过维度分类的SysCode查找所有子节点
        /// </summary>
        /// <param name="catName"></param>
        /// <returns></returns>
        IList GetAllChildNodesBySysCode(String sysCode);

        /// <summary>
        /// 根据维度分类节点查找维度值
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        IList GetDimensionDefineByCategory(DimensionCategory category);

        
        /// <summary>
        /// 根据条件查找维度
        /// </summary>
        /// <param name="oq"></param>
        /// <returns>IList为维度值的集合</returns>
        IList GetDimensionDefineByQuery(ObjectQuery oq);



        /// <summary>
        /// 保存维度分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        DimensionCategory SaveDimensionCategory(DimensionCategory obj);

        /// <summary>
        /// 保存维度分类集合
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        IList SaveDimensionCategorys(IList lst);

        /// <summary>
        /// 保存维度
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        DimensionDefine SaveDimensionDefine(DimensionDefine obj);

        /// <summary>
        /// 根据ID查找维度注册表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DimensionRegister GetDimensionRegisterById(string id);

         /// <summary>
        /// 根据维度注册编码查询维度注册表对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DimensionRegister GetDimensionRegisterByCode(string code);

        /// <summary>
        /// 通过分系统权限查找维度注册表
        /// </summary>
        /// <param name="rights">分系统的ID</param>
        /// <returns></returns>
        IList GetDimensionRegisterByRights(string rights);

        /// <summary>
        /// 求所有己注册的维度
        /// </summary>
        /// <returns></returns>
        IList GetAllDimensionRegister();

        /// <summary>
        /// 保存纬度区间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        DimensionScope SaveDimensionScope(DimensionScope obj);

        /// <summary>
        /// 删除立方注册信息
        /// </summary>
        /// <returns></returns>
        bool DeleteDimensionScope(DimensionScope obj);

        /// <summary>
        /// 根据ID查找评分区间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DimensionScope GetDimensionScopeById(string id);

        /// <summary>
        /// 根据条件查找维度评分区间
        /// </summary>
        /// <param name="oq"></param>
        /// <returns>IList为维度评分区间的集合</returns>
        IList GetDimensionScopeByQuery(ObjectQuery oq);

    }
}
