using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.ImplementationPlan.Domain
{
    [Serializable]
    public class ImplementationMaintain : BaseMaster
    {

            private long version;
            private string cITarget;                               //CI目标
            private string id;                                      //GUID
            private string materialPaystype;                   //材料支付方式
            private string materialMoney;                     //材料款
            private string floorStructure;                       //层数结构
            private string costObjective;                           //成本目标
            private string teamPayStyle;                         //分包队伍支付方式
            private string engChoice;                               //工程分包选择
            private string periodTarget;                           //工期目标
            private string contractChange;                     //合同变更
            private string enGoal;                                 //环保目标
            private string techTarget;                          //技术管理目标
            private decimal coveredArea;                    //建筑面积
            private string structType;                           //结构类型
            private string doteamPayStyle;                   //劳务队伍支付方式
            private string fileName;                            //文档名称
            private string thingsBuy;                           //物资采购
            private string projectId;                            //项目GUID
            private string proName;                            //项目名称
            private string cashProject;                       //项目预兑现及兑现管理目标
            private string ownerpayment;                      //业主支付方式
            private string dofficerId;                       //项目责任人GUID
            private string dutyOfficer;                       //责任人
            private string dofficerName;                        //责任人名称
            private string dofficerLevel;                        //责任人组织层次码
            private string professionSafe;                   //职业安全健康管理目标
            private string qualityObj;                          //质量目标
            private string objectiveName;                     //资金管理目标

            private Iesi.Collections.Generic.ISet<ImplementProjectUnit> details = new Iesi.Collections.Generic.HashedSet<ImplementProjectUnit>();
            virtual public Iesi.Collections.Generic.ISet<ImplementProjectUnit> Details
            {
                get { return details; }
                set { details = value; }
            }
            /// <summary>
            /// 版本
            /// </summary>
            virtual public long Version
            {
                get { return version; }
                set { version = value; }
            }

            /// <summary>
            /// 增加明细
            /// </summary>
            /// <param name="detail"></param>
            virtual public void AddDetail(ImplementProjectUnit detail)
            {
                detail.Master = this;
                Details.Add(detail);
            }

             /// <summary>
            /// CI目标
           /// </summary>
            virtual public string CITarget
            {
                get { return cITarget; }
                set { cITarget = value; }
            }
               /// <summary>
             /// GUID
            /// </summary>
                virtual public string Id
                {
                  get { return id; }
                  set { id = value; }
                }
              /// <summary>
             /// 材料支付方式
            /// </summary>
                virtual public string MaterialPaystype
            {
                get { return materialPaystype; }
                set { materialPaystype = value; }
            }      
             /// <summary>
             /// 材料款
            /// </summary>
                virtual public string MaterialMoney
                {
                    get { return materialMoney; }
                    set { materialMoney = value; }
                }     
           /// <summary>
          /// 层数结构
        /// </summary>
                virtual public string FloorStructure
                {
                    get { return floorStructure; }
                    set { floorStructure = value; }
                }
           /// <summary>
          /// 成本目标
        /// </summary>
                virtual public string CostObjective
                {
                    get { return costObjective; }
                    set { costObjective = value; }
                }  
        
         /// <summary>
        /// 分包队伍支付方式
        /// </summary>
                virtual public string TeamPayStyle
                {
                    get { return teamPayStyle; }
                    set { teamPayStyle = value; }
                }   
          /// <summary>
         /// 工程分包选择
        /// </summary>
                virtual public string EngChoice
                {
                    get { return engChoice; }
                    set { engChoice = value; }
                }    
          /// <summary>
        /// 工期目标
        /// </summary>
                virtual public string PeriodTarget 
                {
                    get { return periodTarget; }
                    set { periodTarget = value; }
                }
             /// <summary>
           /// 合同变更
          /// </summary>
                virtual public string ContractChange
                { 
                    get { return contractChange; }
                    set { contractChange = value; }
                }  
           /// <summary>
         /// 环保目标
        /// </summary>
                virtual public string EnGoal
                {
                    get { return enGoal; }
                    set { enGoal = value; }
                }     
           /// <summary>
          /// 技术管理目标
        /// </summary>
                virtual public string TechTarget
                {
                    get { return techTarget; }
                    set { techTarget = value; }
                }
          /// <summary>
          /// 建筑面积
        /// </summary>
                virtual public decimal CoveredArea
                {
                    get { return coveredArea; }
                    set { coveredArea = value; }
                }
           /// <summary>
         /// 结构类型
        /// </summary>
                virtual public string StructType
                {
                    get { return structType; }
                    set { structType = value; }
                }  
           /// <summary>
         /// 劳务队伍支付方式
        /// </summary>
                virtual public string DoteamPayStyle
                {
                    get { return doteamPayStyle; }
                    set { doteamPayStyle = value; }
                }
           /// <summary>
         /// 文档名称
        /// </summary>
                virtual public string FileName
                {
                    get { return fileName; }
                    set { fileName = value; }
                }
          /// <summary>
         /// 物资采购
        /// </summary>
                virtual public string ThingsBuy
                {
                    get { return thingsBuy; }
                    set { thingsBuy = value; }
                } 
           /// <summary>
          /// 项目GUID
        /// </summary>
                virtual public string ProjectId
                {
                    get { return projectId; }
                    set { projectId = value; }
                }
          /// <summary>
         /// 项目名称
        /// </summary>
                virtual public string ProName
                {
                    get { return proName; }
                    set { proName = value; }
                }    
                /// <summary>
                /// 项目预兑现及兑现管理目标
                /// </summary>
                virtual public string CashProject
                {
                    get { return cashProject; }
                    set { cashProject = value; }
                }
                /// <summary>
                /// 业主支付方式
        /// </summary>
                virtual public string Ownerpayment
                {
                    get { return ownerpayment; }
                    set { ownerpayment = value; }
                }    
          /// <summary>
         /// 项目责任人GUID
        /// </summary>
                virtual public string DofficerId
                {
                    get { return dofficerId; }
                    set { dofficerId = value; }
                }
          /// <summary>
         /// 责任人
        /// </summary>
                virtual public string DutyOfficer
                {
                    get { return dutyOfficer; }
                    set { dutyOfficer = value; }
                }
          /// <summary>
        /// 责任人名称
        /// </summary>
                virtual public string DofficerName
                {
                    get { return dofficerName; }
                    set { dofficerName = value; }
                } 
           /// <summary>
         /// 责任人组织层次码
        /// </summary>
                virtual public string DofficerLevel
                {
                    get { return dofficerLevel; }
                    set { dofficerLevel = value; }
                }
           /// <summary>
         /// 职业安全健康管理目标
        /// </summary>
                virtual public string ProfessionSafe
                {
                    get { return professionSafe; }
                    set { professionSafe = value; }
                }
          /// <summary>
         /// 质量目标
        /// </summary>
                virtual public string QualityObj
                {
                    get { return qualityObj; }
                    set { qualityObj = value; }
                }
  
          /// <summary>
         /// 资金管理目标
        /// </summary>
                virtual public string ObjectiveName
                {
                    get { return objectiveName; }
                    set { objectiveName = value; }
                }
    }
}
