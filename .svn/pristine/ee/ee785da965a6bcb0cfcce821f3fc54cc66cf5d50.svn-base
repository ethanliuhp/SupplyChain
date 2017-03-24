using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain
{
    /// <summary>
    /// 现场照片和视频
    /// </summary>
    [Serializable]
    public class SitePictureVideo
    {
        private string id;
        private string contentNotes;
        private string type;
        private DateTime shootingDate;
        private PersonInfo shootingPerson;
        private string shootingPersonName;
        private string documentName;
        private string documentURL;
        private string documentId;
        private InspectionRecord master;

        /// <summary>
        /// 主表GUID
        /// </summary>
        public virtual InspectionRecord Master
        {
            get { return master; }
            set { master = value; }
        }

        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 内容说明
        /// </summary>
        virtual public string ContentNotes
        {
            get { return contentNotes; }
            set { contentNotes = value; }
        }
        /// <summary>
        /// 类型：视频，照片
        /// </summary>
        virtual public string Type
        {
            get { return type; }
            set { type = value; }
        }
        /// <summary>
        /// 拍摄时间
        /// </summary>
        virtual public DateTime ShootingDate
        {
            get { return shootingDate; }
            set { shootingDate = value; }
        }
        /// <summary>
        /// 拍摄人
        /// </summary>
        virtual public PersonInfo ShootingPerson
        {
            get { return shootingPerson; }
            set { shootingPerson = value; }
        }
        /// <summary>
        /// 拍摄人名称
        /// </summary>
        virtual public string ShootingPersonName
        {
            get { return shootingPersonName; }
            set { shootingPersonName = value; }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        virtual public string DocumentName
        {
            get { return documentName; }
            set { documentName = value; }
        }
        /// <summary>
        /// 文件URL
        /// </summary>
        virtual public string DocumentURL
        {
            get { return documentURL; }
            set { documentURL = value; }
        }
        /// <summary>
        /// 文档GUID
        /// </summary>
        virtual public string DocumentId
        {
            get { return documentId; }
            set { documentId = value; }
        }
    }
}
