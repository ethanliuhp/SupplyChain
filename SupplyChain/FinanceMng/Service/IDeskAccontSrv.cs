using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.Financial.BasicAccount.AssisAccount.Domain;
using System.Collections;
using VirtualMachine.Core;

namespace Application.Business.Erp.Financial.BasicAccount.AssisAccount.Service
{
    /// <summary>
    /// 台账Service
    /// </summary>
    public interface IDeskAccontSrv
    {
        /// <summary>
        /// 获取台账类别基本信息
        /// </summary>
        /// <returns></returns>
        IList GetAllAccDeskSimple();

        /// <summary>
        /// 获取明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetDetail(ObjectQuery oq);
        
        /// <summary>
        /// 获取所有台账类别
        /// </summary>
        /// <returns></returns>
        IList GetAccDeskAll();

        /// <summary>
        /// 根据ID获取台账类别
        /// </summary>
        /// <param name="deskId"></param>
        /// <returns></returns>
        DeskAccount GetDesk(long deskId);

        /// <summary>
        /// 台账类别存盘
        /// </summary>
        /// <param name="desk"></param>
        /// <returns></returns>
        bool SaveDesk(DeskAccount desk);

        /// <summary>
        /// 台账类别修改
        /// </summary>
        /// <param name="Upddesk"></param>
        /// <returns></returns>
        bool UpdDesk(DeskAccount Upddesk);

        /// <summary>
        /// 台账类别删除
        /// </summary>
        /// <param name="deskId"></param>
        /// <returns></returns>
        bool delDesk(long deskId);        
    }
}
