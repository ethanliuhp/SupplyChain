using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Service;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;
using IRPServiceModel.Services.Document;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng
{
    public class MWeatherMng
    {
        private IWeatherSrv weatherSrv;
        public IWeatherSrv WeatherSrv
        {
            get { return weatherSrv; }
            set { weatherSrv = value; }
        }
        private IDocumentSrv docSrv;
        public MWeatherMng()
        {
            if (weatherSrv == null)
            {
                weatherSrv = StaticMethod.GetService("WeatherSrv") as IWeatherSrv;
            }
            if (docSrv == null)
                docSrv = StaticMethod.GetService("DocumentSrv") as IDocumentSrv;
        }
        /// <summary>
        /// 保存晴雨表信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public WeatherInfo SaveWeather(WeatherInfo obj)
        {
            return weatherSrv.SaveWeather(obj);
        }
        /// <summary>
        /// 上传单一文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileDataByte"></param>
        /// <param name="personCode"></param>
        /// <returns></returns>
        public Hashtable UploadSingleFile(string fileName, byte[] fileDataByte, string fileDir)
        {
            return docSrv.UploadSingleFile(fileName, fileDataByte, fileDir);
        }
        /// <summary>
        /// 上传单一文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileDataByte"></param>
        /// <param name="personCode"></param>
        /// <returns></returns>
        public Hashtable UploadPicture(string fileName, byte[] fileDataByte, string percCode)
        {
            return docSrv.UpdatePicture(fileName, fileDataByte, percCode);
        }
    }
}
