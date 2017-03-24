using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using VirtualMachine.Core;
using IRPServiceModel.Domain.PaymentOrder;
using AuthManagerLib.AuthMng.MenusMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace IRPServiceModel.Services.Common
{
    public interface ICommonMethodSrv
    {
        DataSet GetDataAndRowCount(string sDataSQL, int iCurrentPage, int iPageSize, ref int iRowCount);
        DataSet GetData(string sSQL);
        void InsertData(string sql);
        IList Query(Type type, ObjectQuery oQuery);
        object QueryById(Type type, string sID);
        /// <summary>
        /// 根据用户Code获取对应的人信息和岗位信息
        /// </summary>
        /// <param name="sUserCode"></param>
        /// <returns></returns>
        IList GetPersonOnJob(string sUserCode, string sPassWord);
        IList GetPersonOnJob(string sUserCode);
        /// <summary>
        /// 根据用户Code和密码、岗位ID 获取对应的人信息和岗位信息
        /// </summary>
        /// <param name="sUserCode"></param>
        /// <param name="sPassWord"></param>
        /// <param name="sOperationJobId"></param>
        /// <returns></returns>
        IList GetPersonOnJob(string sUserCode, string sPassWord, string sOperationJobId);
        /// <summary>
        /// 根据岗位获取权限配置集合
        /// </summary>
        /// <param name="sOperationJobID"></param>
        /// <returns></returns>
        IList GetAuthConfigByOperationJobID(string sOperationJobID, string sRootMenuCode);
        IList GetAuthConfigByMenuIDCode(string sRootMenuCode);
        /// <summary>
        /// 根据菜单Code进行查找
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>
        Menus GetMenus(string sCode);
        /// <summary>
        /// 获取管理员菜单
        /// </summary>
        /// <param name="sRootId"></param>
        /// <returns></returns>
        IList GetAdminMenus(string sRootMenuCode);
        OperationOrgInfo GetOperationOrgInfo(string sID);
        /// <summary>
        /// 查找项目信息
        /// </summary>
        /// <param name="ownerorgsyscode">组织层次码</param>
        /// <returns></returns>
        CurrentProjectInfo GetProjectInfo(string ownerorgsyscode);

        /// <summary>
        /// 根据ID查找项目
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns></returns>
        CurrentProjectInfo GetProjectInfoById(string projectId);
        /// <summary>
        /// 通过组织ID查询项目ID
        /// </summary>
        string GetProjectIDByOperationOrg(string opgID);

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="sqlTable">完整的SQL语句，做tableName用，如：select * from ( sqlTable ) t where t.</param>
        /// <param name="iCurrentPage">当前页</param>
        /// <param name="iPageSize">分页大小</param>
        /// <param name="iRowCount">总记录数</param>
        /// <returns></returns>
        DataSet GetDataByPage(string sqlTable, int iCurrentPage, int iPageSize, ref int iRowCount);
    }
}
