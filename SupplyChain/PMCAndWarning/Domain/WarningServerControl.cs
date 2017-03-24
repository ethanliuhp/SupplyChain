using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.PMCAndWarning.Domain
{
    /// <summary>
    /// 预警服务控制类
    /// </summary>
    [Serializable]
    public class WarningServerControl
    {
        private static WarningServerStartStateEnum _State = WarningServerStartStateEnum.已关闭;

        /// <summary>
        /// 启动状态
        /// </summary>
        public static WarningServerStartStateEnum State
        {
            get { return WarningServerControl._State; }
            set { WarningServerControl._State = value; }
        }

        /// <summary>
        /// 启动时间
        /// </summary>
        public static DateTime? StartTime { get; set; }

        private static List<System.Windows.Forms.Timer> _Timers = new List<System.Windows.Forms.Timer>();
        /// <summary>
        /// 启动的定时器
        /// </summary>
        public static List<System.Windows.Forms.Timer> Timers
        {
            get { return WarningServerControl._Timers; }
            set { WarningServerControl._Timers = value; }
        }

    }
    /// <summary>
    /// 启动状态
    /// </summary>
    public enum WarningServerStartStateEnum
    {
        已关闭 = 0,
        已启动 = 1
    }
}
