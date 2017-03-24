using System;
using System.Collections;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using VirtualMachine.Core;
namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service
{
    public interface ICubeService
    {
        bool CallDynamicCreateCubeData(Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeRegister obj);
        bool CallDynamicDelCubeData(Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeRegister obj);
        bool DeleteCubeAttribute(Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeAttribute obj);
        bool DeleteCubeRegister(Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeRegister obj);
        System.Collections.IList GetAllCubeRegister();
        System.Collections.IList GetCubeAttrByCubeResgisterId(Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeRegister obj);
        Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeAttribute GetCubeAttributeById(string id);
        Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeRegister GetCubeRegisterById(string id);
        Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeRegister GetCubeRegisterByName(string name);
        System.Collections.IList GetPartSystemCubeRegister(Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.SystemRegister obj);
        Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeAttribute SaveCubeAttribute(Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeAttribute obj);
        Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeRegister SaveCubeRegister(Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain.CubeRegister obj);
        SystemRegister GetSystemRegisterById(string id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        /// <returns></returns>
        string SetCubeData(CubeRegister cr, CubeData cd);

        /// <summary>
        /// 新增，不判断是否存在
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        /// <returns></returns>
        long InsertCubeData(CubeRegister cr, CubeData cd);

        // <summary>
        /// 删除立方数据
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        bool DeleteCubeDataById(CubeRegister cr, CubeData cd);

        // <summary>
        /// 删除立方数据,提供通过维度集进行删除
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        bool DeleteCubeDataByDimensions(CubeRegister cr, CubeData cd);


        // <summary>
        /// 修改立方数据
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        CubeData UpdateCubeDataById(CubeRegister cr, CubeData cd);

        // <summary>
        /// 批量修改立方数据
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd_list">立方数据集合</param>
        void BatchUpdateCubeDatasById(CubeRegister cr, IList cd_list);

        // <summary>
        /// 批量新增立方数据
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd_list">立方数据集合</param>
        void BatchInsertCubeDatas(CubeRegister cr, IList cd_list);

        // <summary>
        /// 修改立方数据,提供通过维度集进行修改
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        bool UpdateCubeDataByDimensions(CubeRegister cr, CubeData cd);

        // <summary>
        /// 查询立方数据中的一条记录
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        CubeData GetCubeDataById(CubeRegister cr, CubeData cd);

        // <summary>
        /// 通过维度集合查询立方数据中的结果
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        CubeData GetCubeDataByDimensions(CubeRegister cr, CubeData cd);

        // <summary>
        /// 查询立方数据中的多条记录(以立方属性方式返回数据)
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="elements">确定元素的多维组合的集合,为一个二维的集合,长度为立方的维度数</param>
        /// <returns>对应结果值的集合</returns>
        IList GetCubeDataList(CubeRegister cr, IList elements);

        // <summary>
        /// 查询立方数据中的多条记录(以显示顺序返回记录)
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="elements">确定元素的多维组合的集合,为一个二维的集合,长度为立方的维度数</param>
        /// 例如:  new String[][] {new String[] { "a1", "a2" }, new String[] { "b1", "b2" }, new String[] { "c1" }, });
        /// return 2 * 2 * 1 = 4个结果. 对应coordinates("a1", "b1", "c1" ),("a1", "b2", "c1" ),("a2", "b1", "c1" ),("a2", "b2", "c1" )
        /// <returns>CubeData对象的集合</returns>
        IList GetCubeDataListByViewStyle(CubeRegister cr, IList elements, IList styleList);

        // <summary>
        /// 同立方属性模式的维度值集合转换到显示模式的维度值集合
        /// </summary>
        /// <param name="attrOrder">立方属性维度集合</param>
        /// <param name="styleOrder">显示模式维度集合</param>
        /// <param name="sourceList">立方属性排列的数据</param>
        IList transOrderToDisplay(IList attrOrder, IList styleOrder, IList sourceList);

        // <summary>
        /// 同显示模式模式的维度值集合转换到立方属性的维度值集合
        /// </summary>
        /// <param name="attrOrder">显示模式维度集合</param>
        /// <param name="styleOrder">立方属性维度集合</param>
        /// <param name="sourceList">显示模式排列的数据</param>
        IList transDisplayToOrder(IList styleOrder, IList attrOrder, IList sourceList);

         //得到查询的维度值的集合 类似于{[ "a1", "a2" ],[ "b1", "b2" ],  [ "c1" ]}
        IList GetQueryElements(IList caList, IList styleList);

        // <summary>
        /// 取得输入值的得分
        /// </summary>
        /// <param name="dimensionId">维度ID</param>
        /// <param name="input">输入值</param>
        /// <returns>分值</returns>
        double getScoreByDimension(long dimensionId, double input);

        // <summary>
        /// 取得当前CubeData的分数
        /// </summary>
        /// <param name="cd">CubeData对象</param>
        /// <returns>分值</returns>
        double getSocreByInput(CubeData cd);

        /// <summary>
        /// 通过查询SQL获取单个值
        /// <returns></returns>
        string GetResultBySql(string sql);

        /// <summary>
        /// 查询分发序号
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        string GetMaxDistributeSerialByViewId(string viewId);

        /// <summary>
        /// 保存文件到服务器上
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        void SaveFileToServer(string fileName, byte[] source);

        /// <summary>
        /// 把结果保存文件到服务器上
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        void SaveFileToServerByResult(string fileName, byte[] source);

        /// <summary>
        /// 从服务器上删除文件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        void DeleteFileFromServer(string fileName);

        /// <summary>
        /// 判断指定文件是否存在服务器的目录下
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IfExistFileInServer(string fileName);

        /// <summary>
        /// 判断指定文件是否存在服务器
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IfExistFiles(string filePath);

        /// <summary>
        /// 修改服务器的目录下的文件名
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        void ChangeFileName(string oldFileName, string newFileName);

        /// <summary>
        /// 判断指定文件是否存在技术管理系统服务器的目录下
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool IfExistFileInKnowlodgeServer(string fileName);

        #region 事实定义方法
        /// <summary>
        /// 查询事实定义
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetFactDefine(ObjectQuery oq);

        /// <summary>
        /// 保存事实定义
        /// </summary>
        /// <param name="factDefineList"></param>
        /// <returns></returns>
        IList SaveFactDefine(IList factDefineList);

        /// <summary>
        /// 通过主题查询事实
        /// </summary>
        /// <param name="cubeRegisterId"></param>
        /// <returns></returns>
        IList GetFactDefineByCubeRegisterId(string cubeRegisterId);
        #endregion
    }
}
