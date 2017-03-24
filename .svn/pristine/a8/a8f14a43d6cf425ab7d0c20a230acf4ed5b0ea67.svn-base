using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.CostManagement.EngineerChangeMng.Domain
{
    /// <summary>
    /// 工程更改明细
    /// </summary>
    [Serializable]
    public class EngineerChangeDetail : BaseDetail
    {
        private string changeDescript;
        private DateTime completeDate;
        private string engineerChangeLink;
        private PersonInfo changeHandlePerson;
        private string changeHandlePersonName;
        private string levelOrgCode;
        /// <summary>
        /// 修改完成时间
        /// </summary>
        virtual public DateTime ComplateDate
        {
            get { return completeDate; }
            set { completeDate = value; }
        }
        ///<summary>
        ///更改说明
        ///</summary>
        virtual public string ChangeDescript
        {
            get { return changeDescript; }
            set { changeDescript = value; }
        }
        ///<summary>
        ///工程更改环节
        ///</summary>
        virtual public string EngineerChangeLink
        {
            get { return engineerChangeLink; }
            set { engineerChangeLink = value; }
        }
        ///<summary>
        ///更改负责人
        ///</summary>
        virtual public PersonInfo ChangeHandlePerson
        {
            get { return changeHandlePerson; }
            set { changeHandlePerson = value; }
        }
        ///<summary>
        ///负责人名称
        ///</summary>
        virtual public string ChangeHandlePersonName
        {
            get { return changeHandlePersonName; }
            set { changeHandlePersonName = value; }
        }
        ///<summary>
        ///更改负责人组织层次码
        ///</summary>
        virtual public string LevelOrgCode
        {
            get { return levelOrgCode; }
            set { levelOrgCode = value; }
        }

    }
}
