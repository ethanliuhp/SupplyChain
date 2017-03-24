using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using System.ComponentModel;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Iesi.Collections;
using Iesi.Collections.Generic;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace IRPServiceModel.Domain.Document
{
    /// <summary>
    /// 文档
    /// </summary>
    [Serializable]
    public class DocumentMaster
    {
        private string _NGUID;
        private string _id;
        private long _version = -1;
        private bool _isInspectionLot = false;
        private DocumentState _State = DocumentState.Edit;
        private DateTime _CreateTime = DateTime.Now;
        private DateTime _UpdateTime = DateTime.Now;
        private DocumentSecurityLevelEnum _SecurityLevel = DocumentSecurityLevelEnum.公开;
        private DocumentCheckOutStateEnum _CheckoutState = DocumentCheckOutStateEnum.检入;
        private DocumentInfoTypeEnum _DocType = DocumentInfoTypeEnum.普通文档;
        private ISet<DocumentDetail> _listFiles = new HashedSet<DocumentDetail>();


        /// <summary>
        /// 名称ID(用于管理不同版本的文档实例)
        /// </summary>
        public virtual string NGUID
        {
            get { return _NGUID; }
            set { _NGUID = value; }
        }
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
        /// 项目Id
        /// </summary>
        public virtual string ProjectId { get; set; }
        /// <summary>
        /// 所属项目代码
        /// </summary>
        public virtual string ProjectCode { get; set; }
        /// <summary>
        /// 所属项目名称
        /// </summary>
        public virtual string ProjectName { get; set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        public virtual DocumentCategory Category { get; set; }
        /// <summary>
        /// 文档分类代码
        /// </summary>
        public virtual string CategoryCode { get; set; }
        /// <summary>
        /// 文档分类名称
        /// </summary>
        public virtual string CategoryName { get; set; }
        /// <summary>
        /// 文档分类层次码
        /// </summary>
        public virtual string CategorySysCode { get; set; }

        /// <summary>
        /// 文档信息类型(1.文本;2.图片;3.音频;4.视频;5.信息模型;6.程序;7.普通文档;8.合同;9.技术文档;)
        /// </summary>
        public virtual DocumentInfoTypeEnum DocType
        {
            get { return _DocType; }
            set { _DocType = value; }
        }

        /// <summary>
        /// 文档代码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 文档名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 文档标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 文档作者
        /// </summary>
        public virtual string Author { get; set; }
        /// <summary>
        /// 文档关键字
        /// </summary>
        public virtual string KeyWords { get; set; }
        /// <summary>
        /// 文档说明
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 文档版本
        /// </summary>
        public virtual string VersionMajor { get; set; }
        /// <summary>
        /// 文档版次
        /// </summary>
        public virtual string Revision { get; set; }
        /// <summary>
        /// 文档状态
        /// </summary>
        public virtual DocumentState State
        {
            get { return _State; }
            set { _State = value; }
        }
        /// <summary>
        /// 责任人
        /// </summary>
        public virtual PersonInfo OwnerID { get; set; }
        /// <summary>
        /// 责任人名称
        /// </summary>
        public virtual string OwnerName { get; set; }
        /// <summary>
        /// 责任人组织层次码
        /// </summary>
        public virtual string OwnerOrgSysCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public virtual DateTime UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; }
        }
        /// <summary>
        /// 是否是检验批文档(模板)
        /// </summary>
        public virtual bool IsInspectionLot
        {
            get { return _isInspectionLot; }
            set { _isInspectionLot = value; }
        }
        /// <summary>
        /// 文档密级
        /// </summary>
        public virtual DocumentSecurityLevelEnum SecurityLevel
        {
            get { return _SecurityLevel; }
            set { _SecurityLevel = value; }
        }
        /// <summary>
        /// 文档检出状态
        /// </summary>
        public virtual DocumentCheckOutStateEnum CheckoutState
        {
            get { return _CheckoutState; }
            set { _CheckoutState = value; }
        }
        /// <summary>
        /// 责任者（编辑者，业务显示用）
        /// </summary>
        public virtual string EditOwner { get; set; }
        /// <summary>
        /// 文档模板参照标准名称
        /// </summary>
        public virtual string ConsultStandardName { get; set; }
        /// <summary>
        /// 文件模板参照标准中的编号
        /// </summary>
        public virtual string ConsultStandardCode { get; set; }
        /// <summary>
        /// 文件列表
        /// </summary>
        public virtual ISet<DocumentDetail> ListFiles
        {
            get { return _listFiles; }
            set { _listFiles = value; }
        }
        /// <summary>
        /// 临时属性（不做MAP）
        /// </summary>
        public virtual string Temp1
        {
            get;
            set;
        }

        /// <summary>
        /// 文档版本+版次（显示用，不做map）
        /// </summary>
        public virtual string VersionStr
        {
            get
            {
                return "V" + this.VersionMajor + "." + this.Revision;
            }
        }
        /// <summary>
        /// 更新版本
        /// </summary>
        public virtual void SetNewVersion()
        {
            List<string> list = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            if (string.IsNullOrEmpty(this.VersionMajor))
            {
                this.VersionMajor = list[0];
            }
            else
            {
                string endStr = this.VersionMajor.Substring(this.VersionMajor.Length - 1);
                int index = list.IndexOf(endStr);

                string newStr = "";
                if (index == list.Count - 1)
                {
                    index = 0;
                    newStr = list[index] + list[index];
                }
                else
                {
                    index += 1;
                    newStr = list[index];
                }

                this.VersionMajor = this.VersionMajor.Substring(0, this.VersionMajor.Length - 1) + newStr;
            }
        }
        /// <summary>
        /// 更新版次
        /// </summary>
        public virtual void SetNewRevision()
        {
            if (string.IsNullOrEmpty(this.Revision))
            {
                this.Revision = "0";
            }
            else
            {
                this.Revision = (Convert.ToInt64(this.Revision) + 1).ToString();
            }
        }
    }

    /// <summary>
    /// 文档信息类型
    /// </summary>
    public enum DocumentInfoTypeEnum
    {
        [Description("文本")]
        文本 = 1,
        [Description("图片")]
        图片 = 2,
        [Description("音频")]
        音频 = 3,
        [Description("视频")]
        视频 = 4,
        [Description("信息模型")]
        信息模型 = 5,
        [Description("程序")]
        程序 = 6,
        [Description("普通文档")]
        普通文档 = 7,
        [Description("合同")]
        合同 = 8,
        [Description("技术文档")]
        技术文档 = 9
    }

    /// <summary>
    /// 文档状态(未使用，使用虚拟机里的文档状态)
    /// </summary>
    public enum DocumentStateEnum
    {
        [Description("编制")]
        编制 = 1,
        [Description("提交")]
        提交 = 2,
        [Description("发布")]
        发布 = 3,
        [Description("作废")]
        作废 = 4
    }

    /// <summary>
    /// 文档保存方式
    /// </summary>
    public enum DocumentSaveModeEnum
    {
        [Description("一个文件生成一个文档对象")]
        一个文件生成一个文档对象 = 1,
        [Description("所有文件生成一个文档对象")]
        所有文件生成一个文档对象 = 2
    }

    /// <summary>
    /// 文档更新模式
    /// </summary>
    public enum DocumentUpdateModeEnum
    {
        [Description("添加一个新版次文件")]
        添加一个新版次文件 = 1,
        [Description("覆盖原有最新版次文件")]
        覆盖原有最新版次文件 = 2
    }

    /// <summary>
    /// 查询文件版本
    /// </summary>
    public enum DocumentQueryVersionEnum
    {
        [Description("最新版本")]
        最新版本 = 1,
        [Description("所有版本")]
        所有版本 = 2
    }

    /// <summary>
    /// 文档密级
    /// </summary>
    public enum DocumentSecurityLevelEnum
    {
        [Description("公开")]
        公开 = 1,
        [Description("秘密")]
        秘密 = 2,
        [Description("机密")]
        机密 = 3,
        [Description("绝密")]
        绝密 = 4
    }
    /// <summary>
    /// 文档检出状态
    /// </summary>
    public enum DocumentCheckOutStateEnum
    {
        /// <summary>
        /// 提交时默认为检入状态;当检出人提交文档时,设置为检入状态.
        /// </summary>
        [Description("检入")]
        检入 = 1,
        /// <summary>
        /// 只有责任人才可进行检出操作,当检出操作时,将文档设置为检出状态,检出状态下不应许其他人浏览和下载.
        /// </summary>
        [Description("检出")]
        检出 = 2
    }
}
