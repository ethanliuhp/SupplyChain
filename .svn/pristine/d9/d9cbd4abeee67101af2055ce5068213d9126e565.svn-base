using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程任务类型关联文档模板
    /// </summary>
    [Serializable]
    public class ProTaskTypeDocumentStencil
    {
        private string id;
        private long version;
        private ProjectTaskTypeTree proTaskType;
        private string proDocumentMasterID;
        private ProjectTaskTypeCheckFlag inspectionMark;
        private string controlWorkflowName;
        private string projectCode;
        private ProjectTaskTypeAlterMode alarmMode;
        private string stencilName;
        private string stencilCode;
        private string stencilDescription;
        private string projectName;

        private string documentCateCode;
        private string documentCateName;

        private string proTaskTypeName;
        private string sysCode;
        

        /// <summary>
        /// 标识
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
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
        /// 工程任务类型
        /// </summary>
        virtual public ProjectTaskTypeTree ProTaskType
        {
            get { return proTaskType; }
            set { proTaskType = value; }
        }
        /// <summary>
        /// 工程任务类型名称
        /// </summary>
        virtual public string ProTaskTypeName
        {
            get { return proTaskTypeName; }
            set { proTaskTypeName = value; }
        }
        /// <summary>
        /// 相关的工程文档模板的业务标识
        /// </summary>
        virtual public string ProDocumentMasterID
        {
            get { return proDocumentMasterID; }
            set { proDocumentMasterID = value; }
        }
        
        /// <summary>
        /// 检验批标志
        /// </summary>
        virtual public ProjectTaskTypeCheckFlag InspectionMark
        {
            get { return inspectionMark; }
            set { inspectionMark = value; }
        }
        
        /// <summary>
        /// 控制工作流名称
        /// </summary>
        virtual public string ControlWorkflowName
        {
            get { return controlWorkflowName; }
            set { controlWorkflowName = value; }
        }
        
        /// <summary>
        /// 所属项目代码
        /// </summary>
        virtual public string ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }

        /// <summary>
        /// 验证报警方式
        /// </summary>
        virtual public ProjectTaskTypeAlterMode AlarmMode
        {
            get { return alarmMode; }
            set { alarmMode = value; }
        }

        /// <summary>
        /// 模版名称
        /// </summary>
        virtual public string StencilName
        {
            get { return stencilName; }
            set { stencilName = value; }
        }

        /// <summary>
        /// 模版代码
        /// </summary>
        virtual public string StencilCode
        {
            get { return stencilCode; }
            set { stencilCode = value; }
        }

        /// <summary>
        /// 模版描述
        /// </summary>
        virtual public string StencilDescription
        {
            get { return stencilDescription; }
            set { stencilDescription = value; }
        }

        /// <summary>
        /// 所属项目名称
        /// </summary>
        virtual public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        /// <summary>
        /// 文档编码分类
        /// </summary>
        virtual public string DocumentCateCode
        {
            get { return documentCateCode; }
            set { documentCateCode = value; }
        }

        /// <summary>
        /// 文档分类名称
        /// </summary>
        virtual public string DocumentCateName
        {
            get { return documentCateName; }
            set { documentCateName = value; }
        }
        /// <summary>
        /// 工程任务类型层次码
        /// </summary>
        virtual public string SysCode
        {
            get { return sysCode; }
            set { sysCode = value; }
        }

    }
    /// <summary>
    /// 验证报警方式
    /// </summary>
    public enum ProjectTaskTypeAlterMode
    {
        [Description("无需验证报警")]
        无需验证报警 = 0,
        /// <summary>
        /// GWBS工程形象进度100%时触发验证，并在资料员角色起始页中提示
        /// </summary>
        [Description("任务完成时触发验证")]
        任务完成时触发验证 = 1
    }
    /// <summary>
    /// 检验批标志
    /// </summary>
    public enum ProjectTaskTypeCheckFlag
    {
        /// <summary>
        /// 不针对检验批，针对GWBS节点
        /// </summary>
        [Description("针对项目任务节点")]
        针对项目任务节点 = 0,
        /// <summary>
        /// 针对检验批，GWBS下属每个检验批生成一个文档实例
        /// </summary>
        [Description("针对检验批")]
        针对检验批 = 1
    }
}

