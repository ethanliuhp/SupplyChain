using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.HelpOnlineManage.Domain
{
    [Serializable]
    public class HelpOnlineMng : BaseMaster
    {
        private string menuId;                       // 菜单Id
        private string menuName;                  // 菜单名称
        private string menuDescript;              // 菜单描述
        private DateTime lastUpdateDate;       // 最后更新时间

        /// <summary>
        /// 菜单Id
        /// </summary>
        virtual public string MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }


        /// <summary>
        /// 菜单名称
        /// </summary>
        virtual public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }


        /// <summary>
        /// 菜单描述
        /// </summary>
        virtual public string MenuDescript
        {
            get { return menuDescript; }
            set { menuDescript = value; }
        }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        virtual public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }
    }
}
