using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Application.Business.Erp.PortalIntegration.Domain;
using System.Collections;

namespace Application.Business.Erp.PortalIntegration.Service
{
    /// <summary>
    /// 门户集成接口服务
    /// </summary>
    [ServiceContract]
    public interface IPortalService
    {
        #region 组织

        /// <summary>
        /// 增加组织机构
        /// </summary>
        /// <param name="parentOrgCode">父机构编码</param>
        /// <param name="opeOrg">机构实体</param>
        /// <returns></returns>
        [OperationContract]
        RetOb AddOrg(string parentOrgCode, OpeOrg opeOrg);

        /// <summary>
        /// 更新组织机构
        /// </summary>
        /// <param name="opeOrg">组织机构实体</param>
        /// <returns></returns>
        [OperationContract]
        RetOb UpdateOrg(OpeOrg opeOrg);

        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="opeOrg">组织机构实体</param>
        /// <returns></returns>
        [OperationContract]
        RetOb DeleteOrg(OpeOrg opeOrg);

        /// <summary>
        /// 查询orgCode对应的机构
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <returns></returns>
        [OperationContract]
        OpeOrg GetOrgByCode(string orgCode);

        /// <summary>
        /// 设置机构隐藏显示
        /// </summary>
        /// <param name="orgCode">组织机构代码</param>
        /// <param name="flag">隐藏显示标记（0=隐藏,1=显示）</param>
        /// <returns></returns>
        [OperationContract]
        RetOb SetOrgInfo(string orgCode, int flag);
        #endregion

        #region 岗位
        /// <summary>
        /// 增加岗位
        /// </summary>
        /// <param name="orgCode">岗位所属机构编码</param>
        /// <param name="post">岗位实体</param>
        /// <returns></returns>
        [OperationContract]
        RetOb AddPost(string orgCode, Post post);

        /// <summary>
        /// 修改岗位
        /// </summary>
        /// <param name="post">岗位实体</param>
        /// <returns></returns>
        [OperationContract]
        RetOb UpdatePost(Post post);

        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="post">岗位实体</param>
        /// <returns></returns>
        [OperationContract]
        RetOb DeletePost(Post post);

        /// <summary>
        /// 根据岗位编码查询岗位
        /// </summary>
        /// <param name="postCode">岗位编码</param>
        /// <returns></returns>
        [OperationContract]
        Post GetPostByCode(string postCode);

        /// <summary>
        /// 岗位关联角色
        /// </summary>
        /// <param name="emps">角色编码列表 形如code1,code2</param>
        /// <param name="posiCode">岗位编码</param>
        /// <returns>角色编码列表 形如code1,code2</returns>
        [OperationContract]
        string UpdateEmpRole(string emps, string posiCode);
        #endregion

        #region 用户
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        [OperationContract]
        RetOb AddUser(User user);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        [OperationContract]
        RetOb UpdateUser(User user);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        [OperationContract]
        RetOb DeleteUser(string userCode);

        /// <summary>
        /// 根据用户编码查询用户信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        [OperationContract]
        User GetUserByCode(string userCode);

        /// <summary>
        /// 修改用户拥有的岗位
        /// </summary>
        /// <param name="emps">岗位编码列表 code1,code2</param>
        /// <param name="userCode">用户编码</param>
        /// <returns>用户拥有的岗位编码列表 code1,code2</returns>
        [OperationContract]
        string UpdateEmpPost(string emps, string userCode);
        #endregion

        #region 角色
        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <returns></returns>
        [OperationContract]
        RetOb AddRole(Role role);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <returns></returns>
        [OperationContract]
        RetOb DeleteRole(Role role);

        /// <summary>
        /// 根据角色编码查询角色
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns></returns>
        [OperationContract]
        Role GetRoleByCode(string roleCode);
        #endregion

        #region 工程项目
        [OperationContract]
        RetOb AddProjectInfo(ProjectInfo model);
        [OperationContract]
        RetOb UpdateProjectInfo(ProjectInfo model);
        [OperationContract]
        RetOb DeleteProjectInfo(ProjectInfo model);
        /// <summary>
        /// 根据工程项目名称和代码查询工程项目
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="code">项目代码</param>
        /// <returns></returns>
        [OperationContract]
        ProjectInfo GetProjectInfo(string name, string code);
        /// <summary>
        /// 根据工程项目名称模糊查询工程项目
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <returns></returns>
        [OperationContract]
        List<ProjectInfo> GetProjectInfoByName(string name);
        /// <summary>
        /// 根据工程项目代码查询工程项目
        /// </summary>
        /// <param name="code">项目代码</param>
        /// <returns></returns>
        [OperationContract]
        ProjectInfo GetProjectInfoByCode(string code);
        #endregion

        /// <summary>
        /// 重新加载数据（xml到内存）
        /// </summary>
        /// <param name="validName"></param>
        /// <param name="validPwd"></param>
        /// <returns></returns>
        [OperationContract]
        bool SyncXMLData();


        /// <summary>
        /// 获取是否自动同步XML数据到内存
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool GetIsAutoSyncXMLData();

        /// <summary>
        /// 设置是否自动同步XML数据到内存开关
        /// </summary>
        /// <param name="IsAutoSyncFlag"></param>
        /// <returns></returns>
        [OperationContract]
        bool SetAutoSyncXMLDataFlag(bool IsAutoSyncFlag);

        //测试
        [OperationContract]
        void CallAppPlat(string stepGUID, string auditPersonName, string auditOpinion, string auditDate);
    }
}
