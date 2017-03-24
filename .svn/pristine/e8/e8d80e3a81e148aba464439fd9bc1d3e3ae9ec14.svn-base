using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRPServiceModel.Domain.Basic;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace IRPServiceModel.Domain.MoneyManage
{
    public class InformationNotice : BasicMasterBill
    {
        private string id;
        private string createPerson;          //创建人
        private string createPersonName;         //创建人名称
        private DateTime createDate = DateTime.Now;//StringUtil.StrToDateTime("1900-01-01");            //创建时间
        private DocumentState docState;        //状态
        private string noticeTitle;       //公告标题
        private string noticeCotent;          //公告内容
        private string descript;                  //备注
        private string code;
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }

        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        virtual public string CreatePerson
        {
            get { return createPerson; }
            set { createPerson = value; }
        }

        /// <summary>
        /// 创建人名称
        /// </summary>
        virtual public string CreatePersonName
        {
            get { return createPersonName; }
            set { createPersonName = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        virtual public DocumentState DocState
        {
            get { return docState; }
            set { docState = value; }
        }

        /// <summary>
        /// 公告标题
        /// </summary>
        virtual public string NoticeTitle
        {
            get { return noticeTitle; }
            set { noticeTitle = value; }
        }

        /// <summary>
        /// 公告内容
        /// </summary>
        virtual public string NoticeCotent
        {
            get { return noticeCotent; }
            set { noticeCotent = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Descript
        {
            get { return descript; }
            set { descript = value; }
        }
    }
}
