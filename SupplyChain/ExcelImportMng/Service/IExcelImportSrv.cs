using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.ExcelImportMng.Service
{
   

    /// <summary>
    /// 基础数据导入服务
    /// </summary>
    public interface IExcelImportSrv : IBaseService
    {
        #region 物料分类信息
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <returns></returns>
        IDbConnection OpenConn();
        /// <summary>
        /// 保存基础数据信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int SaveSql(string sql);
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        /// <summary>
        /// 保存成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        [TransManager]
        IList Save(IList list);
        /// <summary>
        /// 查询基础数据信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable SearchSql(string sql);
        CostAccountSubject SaveCostAccountSubject(CostAccountSubject obj);
        #endregion

        void SaveCostItem(DataSet OleDsExcle, CurrentProjectInfo projectInfo);
        void SaveCostAccountSubject(DataSet OleDsExcle, CurrentProjectInfo projectInfo, string strOperOrgInfoId, string strPersonName, string strOpgSysCode);
        void SaveCostCatagry(DataSet OleDsExcle, CurrentProjectInfo projectInfo);
        void SaveFiacctitle(DataSet OleDsExcle, CurrentProjectInfo projectInfo, string strOperOrgInfoId);
        void SaveGWBS(DataSet OleDsExcle, CurrentProjectInfo projectInfo, string strOperOrgInfoId);
        void SaveMaterial(DataSet OleDsExcle, CurrentProjectInfo projectInfo, string strOperOrgInfoId, string strOperOrgInfoName, string OperOrgInfo, string strOpgSysCode);
        void SaveBasic(DataSet OleDsExcle, CurrentProjectInfo projectInfo, string strOperOrgInfoId);
        void SaveCostInstall(DataSet OleDsExcle, CurrentProjectInfo projectInfo);
    }




}
