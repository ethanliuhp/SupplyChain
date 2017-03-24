using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace IRPServiceModel.Domain.CompanyMng
{
 
   
    [Serializable]
    public class CompanyInfo : CategoryNode
    {

        private int personNum;//人数
        private string address;//场所地址
        private PersonInfo personMng;//负责人

        /// <summary>
        /// 人数
        /// </summary>
        public virtual int PersonNum
        {
            get { return personNum; }
            set { personNum = value; }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Address
        {
            get { return address; }
            set { address = value; }
        }
        /// <summary>
        /// 管理者
        /// </summary>
        public virtual PersonInfo PersonMng
        {
            get { return personMng; }
            set { personMng = value; }
        }

    }
}
