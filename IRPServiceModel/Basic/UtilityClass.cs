using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;

namespace IRPServiceModel.Basic
{
    public class UtilityClass
    {
        private static WebClient _webClientObj = null;

        public static WebClient WebClientObj
        {
            get
            {
                if (_webClientObj == null)
                    _webClientObj = new WebClient();
                return _webClientObj;
            }
            set { _webClientObj = value; }
        }

        private static FileCabinetSrv.FileServiceClient _fileSrvProxy = null;
        /// <summary>
        /// 文件上传服务代理
        /// </summary>
        public static FileCabinetSrv.FileServiceClient FileSrvProxy
        {
            get
            {
                if (_fileSrvProxy == null)
                    _fileSrvProxy = new IRPServiceModel.FileCabinetSrv.FileServiceClient();
                return _fileSrvProxy;
            }
            set { _fileSrvProxy = value; }
        }

        static UtilityClass()
        {

        }
    }
}
