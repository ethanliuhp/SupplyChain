using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IRPServiceModel.Domain.Document
{
    /// <summary>
    /// 文件柜
    /// </summary>
    [Serializable]
    public class FileCabinet
    {
        private string _id;
        private long _version = -1;
        private string _name;
        private string _serverName;
        private string _path;
        private string _queryStr;
        private string _userName;
        private string _userPwd;
        private string _domainName;
        private TransportProtocolsEnum _transportProtocal= TransportProtocolsEnum.Http;
        private UseState usedState= UseState.启用;

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
        /// 文件柜名称
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 服务器IP或域名（例：10.70.18.161:1234）
        /// </summary>
        public virtual string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }
        /// <summary>
        /// 文件柜路径（虚拟目录或网站目录，例：/IRPFile）
        /// </summary>
        public virtual string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        /// <summary>
        /// 查询字符串
        /// </summary>
        public virtual string QueryStr
        {
            get { return _queryStr; }
            set { _queryStr = value; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        public virtual string UserPwd
        {
            get { return _userPwd; }
            set { _userPwd = value; }
        }
        /// <summary>
        /// 域名称
        /// </summary>
        public virtual string DomainName
        {
            get { return _domainName; }
            set { _domainName = value; }
        }
        /// <summary>
        /// 传输协议
        /// </summary>
        public virtual TransportProtocolsEnum TransportProtocal
        {
            get { return _transportProtocal; }
            set { _transportProtocal = value; }
        }
        /// <summary>
        /// 使用状态
        /// </summary>
        public virtual UseState UsedState
        {
            get { return usedState; }
            set { usedState = value; }
        }
    }
    /// <summary>
    /// 传输协议
    /// </summary>
    public enum TransportProtocolsEnum
    {
        [Description("File")]
        File = 1,
        [Description("Ftp")]
        Ftp = 2,
        [Description("Http")]
        Http = 3,
        [Description("Https")]
        Https = 4
        //File = 1,
        //Ftp = 2,
        //Http = 3,
        //Https = 4
    }

    /// <summary>
    /// 使用状态
    /// </summary>
    public enum UseState
    {
        [Description("启用")]
        启用 = 1,
        [Description("不启用")]
        不启用 = 2
    }
}
