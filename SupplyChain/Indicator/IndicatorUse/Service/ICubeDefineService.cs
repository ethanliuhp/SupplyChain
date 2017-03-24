using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;

/**
 * 立方的注册接口
 * 描述：定义立方的注册信息，以及每个立方所拥有的维度属性
 */
namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service
{
    public interface ICubeDefineService
    {
        /// <summary>
        /// 得到所有立方的集合
        /// </summary>
        /// <returns></returns>
        IList GetAllCubeRegister();

        /// <summary>
        /// 得到注册分系统的立方集合
        /// </summary>
        /// <returns></returns>
        IList GetPartSystemCubeRegister(SystemRegister obj);

        /// <summary>
        /// 通过立方注册ID获取立方信息
        /// </summary>
        /// <returns></returns>
        CubeRegister GetCubeRegisterById(long id);

        /// <summary>
        /// 通过立方注册名称获取立方信息
        /// </summary>
        /// <returns></returns>
        CubeRegister GetCubeRegisterByName(String name);

        /// <summary>
        /// 新增/修改立方注册信息
        /// </summary>
        /// <returns></returns>
        CubeRegister SaveCubeRegister(CubeRegister obj);

        /// <summary>
        /// 删除立方注册信息
        /// </summary>
        /// <returns></returns>
        bool DeleteCubeRegister(CubeRegister obj);

        /// <summary>
        /// 通过立方ID获取该立方的属性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IList GetCubeAttrByCubeResgisterId(CubeRegister obj);


        /// <summary>
        /// 通过立方属性ID获取属性信息
        /// </summary>
        /// <param name="id">立方属性ID</param>
        /// <returns></returns>
        CubeAttribute GetCubeAttributeById(long id);

        /// <summary>
        /// 新增/修改立方属性信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        CubeAttribute SaveCubeAttribute(CubeAttribute obj);

        /// <summary>
        /// 删除立方属性信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteCubeAttribute(CubeAttribute obj);

        /// <summary>
        /// 调用脚本：系统动态生成立方数据表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool CallDynamicScript(CubeRegister obj);
    }

}
