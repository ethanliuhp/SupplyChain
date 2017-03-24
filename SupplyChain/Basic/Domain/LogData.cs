using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Basic.Domain
{
    /// <summary>
    /// 日志
    /// </summary>
    [Serializable]
    public class LogData
    {
        private string id;
        private DateTime operDate;//操作时间
        private string operPerson;//操作人
        private string descript;//描述
        private string code;//单据号

        private string billId;//单据主表ID
        private string billType;//单据类型,如[分包结算单]
        private string operType;//操作类型([新增][删除][修改])
        private string projectName;//归属项目名称
        private string projectID;//归属项目ID

        /// <summary>
        /// 项目ID
        /// </summary>
        virtual public string ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        virtual public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        /// <summary>
        /// 操作类型
        /// </summary>
        virtual public string OperType
        {
            get { return operType; }
            set { operType = value; }
        }

        /// <summary>
        /// 单据类型
        /// </summary>
        virtual public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }

        /// <summary>
        /// 单据ID
        /// </summary>
        virtual public string BillId
        {
            get { return billId; }
            set { billId = value; }
        }

        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 操作日期
        /// </summary>
        virtual public DateTime OperDate
        {
            get { return operDate; }
            set { operDate = value; }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        virtual public string OperPerson
        {
            get { return operPerson; }
            set { operPerson = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        virtual public string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 业务单据号
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }


    }
}
