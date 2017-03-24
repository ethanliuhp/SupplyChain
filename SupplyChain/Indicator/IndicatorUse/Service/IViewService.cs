using System;
using System.Collections;

using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service
{
    public interface IViewService
    {
        VirtualMachine.Core.IDao Dao { get; set; }
        VirtualMachine.Core.DataAccess.IDataAccess DBDao { get; set; }

        /// <summary>
        /// 查询注册立方的视图集合
        /// </summary>
        /// <returns></returns>
        IList GetViewMainByCubeId(CubeRegister obj);

        /// <summary>
        /// 通过主题和视图类型查询视图集合
        /// </summary>
        /// <returns></returns>
        IList GetViewMainByCubeIdAndType(CubeRegister obj, string type);

       /// <summary>
        /// 通过条件查询视图集合
        /// </summary>
        /// <returns></returns>
        IList GetViewMainByQuery(ObjectQuery oq);

        /// <summary>
        /// 新增/修改视图主表
        /// </summary>
        /// <returns></returns>
        ViewMain SaveViewMain(ViewMain obj);

        /// <summary>
        /// 删除视图主表
        /// </summary>
        /// <returns></returns>
        bool DeleteViewMain(ViewMain obj);

                /// <summary>
        /// 通过视图ID查询视图信息
        /// </summary>
        /// <returns></returns>
        ViewMain GetViewMainById(string id);

        /// <summary>
        /// 通过视图名称查询视图信息
        /// </summary>
        /// <returns></returns>
        ViewMain GetViewMainByName(string name);

        /// <summary>
        /// 新增/修改视图录入信息
        /// </summary>
        /// <returns></returns>
        ViewWriteInfo SaveViewWriteInfo(ViewWriteInfo obj);

        /// <summary>
        /// 删除视图视图录入信息
        /// </summary>
        /// <returns></returns>
        bool DeleteViewWriteInfo(ViewWriteInfo obj);

        /// <summary>
        /// 根据条件查找视图录入信息
        /// </summary>
        /// <param name="oq"></param>
        /// <returns>IList为视图录入信息的集合</returns>
        IList GetViewWriteInfoByQuery(ObjectQuery oq);

        /// <summary>
        /// 新增/修改视图规则定义
        /// </summary>
        /// <returns></returns>
        ViewRuleDef SaveViewRuleDef(ViewRuleDef obj);

        // <summary>
        /// 修改规则定义数据
        /// </summary>
        /// <param name="cd"></param>
        ViewRuleDef UpdateViewRuleDefById(ViewRuleDef obj);

        // <summary>
        /// 批量修改规则定义数据
        /// </summary>
        /// <param name="cd"></param>
        void BatchUpdateViewRuleDefById(IList list);

        /// <summary>
        /// 批量新增/修改视图规则定义
        /// </summary>
        /// <returns></returns>
        void SaveViewRuleDefs(IList objs);

        /// <summary>
        /// 删除视图规则定义
        /// </summary>
        /// <returns></returns>
        bool DeleteViewRuleDef(ViewRuleDef obj);

        /// <summary>
        /// 批量删除视图规则定义
        /// </summary>
        /// <returns></returns>
        void DeleteViewRuleDefs(IList objs);

        // <summary>
        /// 通过存储过程批量修改视图规则定义
        /// </summary>
        /// <param name="type">类型(1:新增,2:修改)</param>
        /// <param name="cd_list">视图规则定义集合</param>
        void BatchUpdateRuleDefByProc(int type, IList rule_list);


        /// <summary>
        /// 通过视图主表查询视图规则定义明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IList GetViewRuleDefByViewMain(ViewMain obj);

        /// <summary>
        /// 新增/修改视图明细
        /// </summary>
        /// <returns></returns>
        ViewDetail SaveViewDetail(ViewDetail obj);

        /// <summary>
        /// 删除视图明细
        /// </summary>
        /// <returns></returns>
        bool DeleteViewDetail(ViewDetail obj);

        /// <summary>
        /// 批量删除视图明细
        /// </summary>
        /// <returns></returns>
        bool DeleteViewDetail(IList objs);

        /// <summary>
        /// 通过视图明细ID查询视图明细信息
        /// </summary>
        /// <returns></returns>
        ViewDetail GetViewDetailById(string id);

        /// <summary>
        /// 通过视图主表查询视图明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IList GetViewDetailByViewMain(ViewMain obj);

        /// <summary>
        /// 通过视图主表查询视图格式
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ordered">是否按指定顺序排序 true排序，false不排序</param>
        /// <returns></returns>
        IList GetViewStyleByViewMain(ViewMain obj, bool ordered);

        /// <summary>
        /// 通过视图主表查询视图分发明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IList GetViewDistributeByViewMain(ViewMain obj);

        
        /// <summary>
        /// 删除视图格式表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteViewStyle(ViewStyle obj);

        /// <summary>
        /// 删除视图格式表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteViewStyle(IList objs);

        /// <summary>
        /// 删除视图格式维度表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteViewStyleDim(ViewStyleDimension obj);

        /// <summary>
        /// 删除视图格式维度表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteViewStyleDim(IList objs);

        /// <summary>
        /// 通过视图格式表ID查询视图格式维度表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IList GetViewStyleDimByVS(string viewStyleId);

        /// <summary>
        /// 通过视图格式表ID和维度名称查询视图格式维度表
        /// </summary>
        /// <param name="viewStyleId"></param>
        /// <param name="catName"></param>
        /// <returns></returns>
        IList GetViewStyleDimByVS(string viewStyleId, string catName);

        /// <summary>
        /// 新增/修改视图分发
        /// </summary>
        /// <returns></returns>
        ViewDistribute SaveViewDistribute(ViewDistribute obj);

        void SaveViewDistribute(IList lst);

        /// <summary>
        /// 删除视图分发
        /// </summary>
        /// <returns></returns>
        bool DeleteViewDistribute(ViewDistribute obj);

        /// <summary>
        /// 根据条件查找分发的视图
        /// </summary>
        /// <param name="oq"></param>
        /// <returns>IList为视图的集合</returns>
        IList GetViewDistributeByQuery(ObjectQuery oq);

         /// <summary>
        /// 根据ID查找分发的视图
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        ViewDistribute GetViewDistributeById(string id);
    }
}
