using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;

namespace PortalIntegrationConsole.Service
{
    public interface IOrgUsersXmlSrv
    {
        #region 岗位操作

        /// <summary>
        /// 岗位是否存在
        /// </summary>
        /// <param name="jobCode">岗位代码</param>
        /// <returns></returns>
        bool JobIsExists(string jobCode);

        /// <summary>
        /// 插入岗位(插入到配置文件中的Group中)
        /// </summary>
        /// <param name="operationJob">岗位</param>
        void InsertOperationJobNode(OperationJob operationJob);

        /// <summary>
        /// 修改岗位节点
        /// </summary>
        /// <param name="operationJob"></param>
        void ModifyOperationJobNode(OperationJob operationJob);


        /// <summary>
        /// //删除岗位节点，首先判断当前组织是否被引用，如果引用需删除此用户的组织信息
        /// </summary>
        /// <param name="operationJob"></param>
        void DeleteOperationJobNode(OperationJob operationJob);
        #endregion

        #region 角色操作

        /// <summary>
        /// 角色是否存在
        /// </summary>
        /// <param name="jobCode">角色名称</param>
        /// <returns></returns>
        bool RoleIsExists(string roleName);

        /// <summary>
        /// 插入角色节点
        /// </summary>
        /// <param name="operationRole">角色</param>
        void InsertRoleNode(OperationRole operationRole);

        /// <summary>
        /// 删除角色节点，首先判断当前角色是否被引用，如果引用需删除此用户的角色信息
        /// </summary>
        /// <param name="operationRole"></param>
        void DeleteRoleNode(OperationRole operationRole);

        #endregion

        #region 用户操作

        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="userCode">用户代码</param>
        /// <returns></returns>
        bool UserIsExists(string userCode);

        /// <summary>
        /// 插入用户节点
        /// </summary>
        /// <param name="theUser"></param>
        void InsertUserNode(StandardPerson theUser);

        /// <summary>
        /// 修改用户节点
        /// </summary>
        /// <param name="theUser"></param>
        void ModifyUserNode(StandardPerson theUser);

        /// <summary>
        /// 删除用户节点
        /// </summary>
        /// <param name="theUser"></param>
        void DeleteUserNode(StandardPerson theUser);
        #endregion

        #region 人员上岗维护对应的IRP用户的组织和角色的信息的增删
        /// <summary>
        /// 人员上岗
        /// </summary>
        /// <param name="perOnJob"></param>
        void AddJobAndRole(PersonOnJob perOnJob);

        /// <summary>
        /// 修改用户的岗位
        /// </summary>
        /// <param name="oldUser"></param>
        /// <param name="personJob"></param>
        void ModifyUserJob(StandardPerson oldUser, PersonOnJob personJob);

        /// <summary>
        /// 删除用户的岗位
        /// </summary>
        /// <param name="user"></param>
        /// <param name="operationJob"></param>
        void DeleteUserJob(StandardPerson user, OperationJob operationJob);

        /// <summary>
        /// 人员上岗
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="listJob"></param>
        void PersonOnJob(string userCode, List<OperationJob> listJob);

        #endregion

        #region 岗位关联角色操作
        /// <summary>
        /// 岗位关联角色
        /// </summary>
        /// <param name="operationJob"></param>
        void JobLinkRole(OperationJob operationJob);
        #endregion
    }
}
