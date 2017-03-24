using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRPServiceModel.Domain.Document
{
    /// <summary>
    /// 文件
    /// </summary>
    [Serializable]
    public class DocumentDetail
    {
        private string _id;
        private long _version = -1;
        private DocumentMaster _master;
        private FileCabinet _theFileCabinet;


        /// <summary>
        /// 文档GUID
        /// </summary>
        public virtual string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        /// <summary>
        /// 版本（hibernate使用）
        /// </summary>
        public virtual long Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }
        /// <summary>
        /// 所属文档对象
        /// </summary>
        public virtual DocumentMaster Master
        {
            get { return _master; }
            set { _master = value; }
        }
        /// <summary>
        /// 文件所在的文件柜
        /// </summary>
        public virtual FileCabinet TheFileCabinet
        {
            get { return _theFileCabinet; }
            set { _theFileCabinet = value; }
        }
        /// <summary>
        /// 文件名称(用于显示和下载到磁盘里的文件名)
        /// </summary>
        public virtual string FileName { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public virtual string ExtendName { get; set; }
        /// <summary>
        /// 文件二进制流
        /// </summary>
        public virtual Byte[] FileDataByte { get; set; }
        /// <summary>
        /// 文件部分路径（除文件柜路径外）
        /// </summary>
        public virtual string FilePartPath { get; set; }
    }
}
