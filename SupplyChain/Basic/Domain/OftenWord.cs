using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Basic.Domain
{
    [Serializable]
    public class OftenWord
    {
        private string id;
        private long version;
        private string oftenWords;
        private string userID;
        //private string userName;
        private string interphaseID;
        //private string interphaseName;
        private string controlID;
        //private string controlName;

        /// <summary>
        /// 唯一标识
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 常用短语
        /// </summary>
        virtual public string OftenWords
        {
            get { return oftenWords; }
            set { oftenWords = value; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        virtual public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        ///// <summary>
        ///// 用户名称
        ///// </summary>
        //virtual public string UserName
        //{
        //    get { return userName; }
        //    set { userName = value; }
        //}
        /// <summary>
        /// 界面ID
        /// </summary>
        virtual public string InterphaseID
        {
            get { return interphaseID; }
            set { interphaseID = value; }
        }
        ///// <summary>
        ///// 界面名称
        ///// </summary>
        //virtual public string InterphaseName
        //{
        //    get { return interphaseName; }
        //    set { interphaseName = value; }
        //}
        /// <summary>
        /// 控件ID
        /// </summary>
        virtual public string ControlID
        {
            get { return controlID; }
            set { controlID = value; }
        }
        ///// <summary>
        ///// 控件名称
        ///// </summary>
        //virtual public string ControlName
        //{
        //    get { return controlName; }
        //    set { controlName = value; }
        //}

    }
}