using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程文档验证
    /// </summary>
    [Serializable]
    public class ProjectDocumentVerify
    {
        private string id;
        private long version;
        private ProjectDocumentAssociateLevel associateLevel;
        private ProjectDocumentSubmitState submitState;
        private string docuemntID;
        private string documentCode;
        private string documentCategoryCode;
        private string documentCategoryName;
        private string documentWorkflowName;
        private string documentName;
        private string documentDesc;
        private string fileSourceURl;
        private ProjectTaskTypeAlterMode alterMode;
        private ProjectDocumentVerifySwitch verifySwitch;
        private GWBSTree projectTask;
        private string projectTaskName;
        private string taskSyscode;
        private string projectID;
        private string projectName;
        private string projectCode;

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
        /// 关联级别
        /// </summary>
        virtual public ProjectDocumentAssociateLevel AssociateLevel
        {
            get { return associateLevel; }
            set { associateLevel = value; }
        }
        
        /// <summary>
        /// 提交状态
        /// </summary>
        virtual public ProjectDocumentSubmitState SubmitState
        {
            get { return submitState; }
            set { submitState = value; }
        }
        
        /// <summary>
        /// 文档GUID
        /// </summary>
        virtual public string DocuemntID
        {
            get { return docuemntID; }
            set { docuemntID = value; }
        }
        
        /// <summary>
        /// 文档代码
        /// </summary>
        virtual public string DocumentCode
        {
            get { return documentCode; }
            set { documentCode = value; }
        }
        
        /// <summary>
        /// 文档分类编码
        /// </summary>
        virtual public string DocumentCategoryCode
        {
            get { return documentCategoryCode; }
            set { documentCategoryCode = value; }
        }
        /// <summary>
        /// 文档分类名称
        /// </summary>
        virtual public string DocumentCategoryName
        {
            get { return documentCategoryName; }
            set { documentCategoryName = value; }
        }
        /// <summary>
        /// 文档工作流名称
        /// </summary>
        virtual public string DocumentWorkflowName
        {
            get { return documentWorkflowName; }
            set { documentWorkflowName = value; }
        }
        
        /// <summary>
        /// 文档名称
        /// </summary>
        virtual public string DocumentName
        {
            get { return documentName; }
            set { documentName = value; }
        }
        
        /// <summary>
        /// 文档说明
        /// </summary>
        virtual public string DocumentDesc
        {
            get { return documentDesc; }
            set { documentDesc = value; }
        }
        
        /// <summary>
        /// 文件原URl
        /// </summary>
        virtual public string FileSourceURl
        {
            get { return fileSourceURl; }
            set { fileSourceURl = value; }
        }
        
        /// <summary>
        /// 验证报警方式
        /// </summary>
        virtual public ProjectTaskTypeAlterMode AlterMode
        {
            get { return alterMode; }
            set { alterMode = value; }
        }
        
        /// <summary>
        /// 验证开关
        /// </summary>
        virtual public ProjectDocumentVerifySwitch VerifySwitch
        {
            get { return verifySwitch; }
            set { verifySwitch = value; }
        }
        
        /// <summary>
        /// 工程项目任务
        /// </summary>
        virtual public GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        
        /// <summary>
        ///工程项目任务名称 
        /// </summary>
        virtual public string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        
        /// <summary>
        /// 层次编码
        /// </summary>
        virtual public string TaskSyscode
        {
            get { return taskSyscode; }
            set { taskSyscode = value; }
        }

        /// <summary>
        /// 所属项目ID
        /// </summary>
        virtual public string ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
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
        /// 所属项目代码
        /// </summary>
        virtual public string ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }

    }
    /// <summary>
    /// 关联级别
    /// </summary>
    public enum ProjectDocumentAssociateLevel
    {
        /// <summary>
        /// GWBS：关联在{工程项目任务}对象上
        /// </summary>
        [Description("GWBS")]
        GWBS = 1,
        /// <summary>
        /// 检验批：关联在{检验批}对象上
        /// </summary>
        [Description("检验批")]
        检验批 = 2
    }
    /// <summary>
    /// 提交状态
    /// </summary>
    public enum ProjectDocumentSubmitState
    {
        [Description("未提交")]
        未提交 = 1,
        [Description("已提交")]
        已提交 = 2
    }
    ///// <summary>
    ///// 验证报警方式
    ///// </summary>
    //public enum ProjectDocumentAlterMode
    //{
    //    [Description("无需验证报警")]
    //    无需验证报警 = 0,
    //    /// <summary>
    //    /// GWBS工程形象进度100%时触发验证，并在资料员角色起始页中提示
    //    /// </summary>
    //    [Description("任务完成时触发验证")]
    //    任务完成时触发验证 = 1
    //}
    /// <summary>
    /// 验证开关
    /// </summary>
    public enum ProjectDocumentVerifySwitch
    {
        [Description("不验证")]
        不验证 = 0,
        [Description("验证")]
        验证 = 1
    }

}
