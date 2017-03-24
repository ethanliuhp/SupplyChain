using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ExcelImportMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.ExcelImportMng
{
    public class MExcelImportMng
    {
        private IExcelImportSrv excelImportSrv;

        public IExcelImportSrv ExcelImportSrv
        {
            get { return excelImportSrv; }
            set { excelImportSrv = value; }
        }

        public MExcelImportMng()
        {
            if (excelImportSrv == null)
            {
                excelImportSrv = StaticMethod.GetService("ExcelImportSrv") as IExcelImportSrv;
            }
        }

        #region 基础数据导入
        /// <summary>
        /// 保存成本核算科目
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public CostAccountSubject SaveCostAccountSubject(CostAccountSubject obj)
        {
            return excelImportSrv.SaveCostAccountSubject(obj);
        }

        #endregion

        /// <summary>
        /// 查询基础数据信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable SearchSql(string sql)
        {
            return excelImportSrv.SearchSql(sql);
        }
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return excelImportSrv.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 保存成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        [TransManager]
        public IList Save(IList list)
        {
            return excelImportSrv.Save(list);
        }
    }
}
