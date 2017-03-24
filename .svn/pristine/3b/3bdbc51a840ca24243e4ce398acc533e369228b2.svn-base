using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 视图主表Domain
    /// </summary>
    [Serializable]
    [Entity]
    public class ViewMain
    {
        private string id;
        private long version = -1;
        private int state = 1;
        private string viewName;
        private string viewTypeCode;
        private string viewTypeName;
        private string collectTypeCode;
        private string collectTypeName;
        private string ifDisplaySonMother;
        private string ifYwzz;
        private string ifTime;
        private string remark;
        private DateTime createdDate;
        private CubeRegister cubeRegId;
        private OperationOrgInfo theOperOrg;
        private IList viewDetails = new ArrayList();
        private PersonInfo author;
        private SysRole theJob;
        private IList viewStyles = new ArrayList();
        private IList viewDistributes = new ArrayList();
        private int displayDecimal = 0;
        private int collectRule = 1;
        private string systemId;
        private string collectYwzz;

        /// <summary>
        /// 分发视图
        /// </summary>
        virtual public IList ViewDistributes
        {
            get { return viewDistributes; }
            set { viewDistributes = value; }
        }

        virtual public IList ViewStyles
        {
            get { return viewStyles; }
            set { viewStyles = value; }
        }

        /// <summary>
        /// 视图名称
        /// </summary>
        virtual public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        virtual public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 岗位
        /// </summary>
        virtual public SysRole TheJob
        {
            get { return theJob; }
            set { theJob = value; }
        }

        /// <summary>
        /// 制表人
        /// </summary>
        virtual public PersonInfo Author
        {
            get { return author; }
            set { author = value; }
        }

        /// <summary>
        /// 视图明细
        /// </summary>
        virtual public IList ViewDetails
        {
            get { return viewDetails; }
            set { viewDetails = value; }
        }

        /// <summary>
        /// 视图类型编码
        /// </summary>
        virtual public string ViewTypeCode
        {
            get { return viewTypeCode; }
            set { viewTypeCode = value; }
        }

        /// <summary>
        /// 视图类型名称
        /// </summary>
        virtual public string ViewTypeName
        {
            get { return viewTypeName; }
            set { viewTypeName = value; }
        }

        /// <summary>
        /// 采集频率编码(一次采集、每月、每季度)
        /// </summary>
        virtual public string CollectTypeCode
        {
            get { return collectTypeCode; }
            set { collectTypeCode = value; }
        }
        /// <summary>
        /// 采集频率名称(一次采集、每月、每季度)
        /// </summary>
        virtual public string CollectTypeName
        {
            get { return collectTypeName; }
            set { collectTypeName = value; }
        }
        /// <summary>
        /// 是否显示子项母项
        /// </summary>
        virtual public string IfDisplaySonMother
        {
            get { return ifDisplaySonMother; }
            set { ifDisplaySonMother = value; }
        }

        /// <summary>
        /// 是否显示业务组织维度
        /// </summary>
        virtual public string IfYwzz
        {
            get { return ifYwzz; }
            set { ifYwzz = value; }
        }

        /// <summary>
        /// 是否显示时间维度
        /// </summary>
        virtual public string IfTime
        {
            get { return ifTime; }
            set { ifTime = value; }
        }

        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        virtual public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 归属立方
        /// </summary>
        virtual public CubeRegister CubeRegId
        {
            get { return cubeRegId; }
            set { cubeRegId = value; }
        }

        /// <summary>
        /// 创建部门
        /// </summary>
        virtual public OperationOrgInfo TheOperOrg
        {
            get { return theOperOrg; }
            set { theOperOrg = value; }
        }

        /// <summary>
        /// 显示精度
        /// </summary>
        virtual public int DisplayDecimal
        {
            get { return displayDecimal; }
            set { displayDecimal = value; }
        }

        /// <summary>
        /// 汇总规则
        /// </summary>
        virtual public int CollectRule
        {
            get { return collectRule; }
            set { collectRule = value; }
        }

        /// <summary>
        /// 汇总部门的字符串联接
        /// </summary>
        virtual public string CollectYwzz
        {
            get { return collectYwzz; }
            set { collectYwzz = value; }
        }

        /// <summary>
        /// 分系统ID
        /// </summary>
        virtual public string SystemId
        {
            get { return systemId; }
            set { systemId = value; }
        }

    }
}
