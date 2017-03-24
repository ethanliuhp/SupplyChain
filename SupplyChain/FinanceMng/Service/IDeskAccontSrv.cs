using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.Financial.BasicAccount.AssisAccount.Domain;
using System.Collections;
using VirtualMachine.Core;

namespace Application.Business.Erp.Financial.BasicAccount.AssisAccount.Service
{
    /// <summary>
    /// ̨��Service
    /// </summary>
    public interface IDeskAccontSrv
    {
        /// <summary>
        /// ��ȡ̨����������Ϣ
        /// </summary>
        /// <returns></returns>
        IList GetAllAccDeskSimple();

        /// <summary>
        /// ��ȡ��ϸ
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetDetail(ObjectQuery oq);
        
        /// <summary>
        /// ��ȡ����̨�����
        /// </summary>
        /// <returns></returns>
        IList GetAccDeskAll();

        /// <summary>
        /// ����ID��ȡ̨�����
        /// </summary>
        /// <param name="deskId"></param>
        /// <returns></returns>
        DeskAccount GetDesk(long deskId);

        /// <summary>
        /// ̨��������
        /// </summary>
        /// <param name="desk"></param>
        /// <returns></returns>
        bool SaveDesk(DeskAccount desk);

        /// <summary>
        /// ̨������޸�
        /// </summary>
        /// <param name="Upddesk"></param>
        /// <returns></returns>
        bool UpdDesk(DeskAccount Upddesk);

        /// <summary>
        /// ̨�����ɾ��
        /// </summary>
        /// <param name="deskId"></param>
        /// <returns></returns>
        bool delDesk(long deskId);        
    }
}
