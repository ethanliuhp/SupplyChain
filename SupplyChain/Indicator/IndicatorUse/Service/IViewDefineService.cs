using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service
{
    public interface IViewDefineService
    {
        /// <summary>
        /// 查询注册立方的视图集合
        /// </summary>
        /// <returns></returns>
        IList GetViewMainByCubeId(CubeRegister obj);

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
        ViewMain GetViewMainById(long id);

        /// <summary>
        /// 通过视图名称查询视图信息
        /// </summary>
        /// <returns></returns>
        ViewMain GetViewMainByName(string name);

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
        bool BatchDeleteViewDetail(IList objs);

        /// <summary>
        /// 通过视图明细ID查询视图明细信息
        /// </summary>
        /// <returns></returns>
        ViewDetail GetViewDetailById(long id);
    }
}
