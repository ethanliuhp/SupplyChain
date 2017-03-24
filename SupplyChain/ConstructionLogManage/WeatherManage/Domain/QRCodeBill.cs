using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain
{
    /// <summary>
    /// 二维码
    /// </summary>
    [Serializable]
    public class QRCodeBill : BaseMaster
    {
        private string billType;
        private string fileName;
        private string fileUrl;
        private string filecabinetId;
        private DateTime fileLastTime;
        private string sysFileName;
        private string codeTitle;
        private int printTimes;

        /// <summary>
        /// 业务类型
        /// </summary>
        virtual public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }
        /// <summary>
        /// 文件名
        /// </summary>
        virtual public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        /// <summary>
        /// 文件URL
        /// </summary>
        virtual public string FileUrl
        {
            get { return fileUrl; }
            set { fileUrl = value; }
        }
        /// <summary>
        /// 文件柜ID
        /// </summary>
        virtual public string FilecabinetId
        {
            get { return filecabinetId; }
            set { filecabinetId = value; }
        }
        /// <summary>
        /// 文件最后更新时间
        /// </summary>
        virtual public DateTime FileLastTime
        {
            get { return fileLastTime; }
            set { fileLastTime = value; }
        }
        /// <summary>
        /// 系统生成的文件名guid
        /// </summary>
        virtual public string SysFileName
        {
            get { return sysFileName; }
            set { sysFileName = value; }
        }
        /// <summary>
        /// 编号标题
        /// </summary>
        virtual public string CodeTitle
        {
            get { return codeTitle; }
            set { codeTitle = value; }
        }
        /// <summary>
        /// 打印次数
        /// </summary>
        virtual public int PrintTimes
        {
            get { return printTimes; }
            set { printTimes = value; }
        }
    }
}
