using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.EngineerChangeMng.Domain
{
    /// <summary>
    /// 工程更改主表
    /// </summary>
    [Serializable]
    public class EngineerChangeMaster : BaseMaster
    {
        private string currentState;
        private string workFlowIntance;
        private string workflowIntanceName;
        private ContractGroup contractGroup;
        private string contractGroupName;
        private string levelOrgCode;
        /// <summary>
        /// 当前状态
        /// </summary>
        virtual public string CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }
        ///<summary>
        ///工作流实例
        /// </summary>
        virtual public string WorkFlowIntance
        {
            get { return workFlowIntance; }
            set { workFlowIntance = value; }
        }
        ///<summary>
        ///工作流实例名称
        ///</summary
        virtual public string WorkFlowIntanceName
        {
            get { return workflowIntanceName; }
            set { workflowIntanceName = value; }
        }
        ///<summary>
        ///契约组
        ///</summary>
        virtual public ContractGroup ContractGroup
        {
            get { return contractGroup; }
            set { contractGroup = value; }
        }
        ///<summary>
        ///契约组名称
        ///</summary>
        virtual public string ContractGroupName
        {
            get { return contractGroupName; }
            set { contractGroupName = value; }
        }
        /// <summary>
        /// 变更发起人组织层次码
        /// </summary>
        virtual public string LevelOrgCode
        {
            get { return levelOrgCode; }
            set { levelOrgCode = value; }
        }
       
    }
}
