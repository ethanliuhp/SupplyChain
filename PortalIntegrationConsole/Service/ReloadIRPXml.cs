using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PortalIntegrationConsole.Service
{
    /// <summary>
    /// 重新加载IRP配置文件
    /// </summary>
    public class ReloadIRPXml
    {
        private static PortalIntegrationConsole.IRPOrgUserSyncDataSrv.OrgUserSyncDataSrvSoapClient _IRPService;
        private static PortalIntegrationConsole.KBOrgUserSyncDataSrv.OrgUserSyncDataSrvSoapClient _KBService;

        public static PortalIntegrationConsole.IRPOrgUserSyncDataSrv.OrgUserSyncDataSrvSoapClient IRPService
        {
            get
            {
                if (_IRPService == null)
                    _IRPService = new PortalIntegrationConsole.IRPOrgUserSyncDataSrv.OrgUserSyncDataSrvSoapClient();

                return ReloadIRPXml._IRPService;
            }
            set { ReloadIRPXml._IRPService = value; }
        }

        public static PortalIntegrationConsole.KBOrgUserSyncDataSrv.OrgUserSyncDataSrvSoapClient KBService
        {
            get
            {
                if (_KBService == null)
                {
                    _KBService = new PortalIntegrationConsole.KBOrgUserSyncDataSrv.OrgUserSyncDataSrvSoapClient();
                }
                return ReloadIRPXml._KBService;
            }
            set { ReloadIRPXml._KBService = value; }
        }

        /// <summary>
        /// 加载IRP用户配置文件
        /// </summary>
        public static void ReloadUserXml()
        {
            IRPService.SyncXMLData();
        }

        /// <summary>
        /// 加载知识库用户配置文件
        /// </summary>
        public static void ReloadKBUserXml()
        {
            KBService.SyncXMLData();
        }
    }
}
