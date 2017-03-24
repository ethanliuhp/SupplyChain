using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.Financial.GlobalInfo;
using VirtualMachine.Core.DataAccess;
using System.Data;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service
{
    /// <summary>
    /// 科目编码Service
    /// </summary>
    public class AccTitleCodeImpl : CommonAccBookSrv, IAccTitleCodeSrv
    {
        /// <summary>
        /// 获取科目编码默认值
        /// </summary>
        /// <param name="assisCode">科目分级</param>
        /// <param name="accLevel">科目级别</param>
        /// <returns></returns>
        virtual public string GetDefaultCode(long accId)
        {
            //string quySql = "select max(acccode) from thd_fiacctitle where parentnodeid=:inid";
            //IList<DataAccessParameter> lsParams = new List<DataAccessParameter>();
            //DBDao.SetInParameterForQuery(":inid", OracleDbType.Decimal, accId, ref lsParams);

            //object maxCode = DBDao.OpenQueryScalar(quySql, lsParams);
            //int rtnCode = int.Parse(maxCode.ToString()) + 1;
            //return rtnCode.ToString();
            return "";
        }

    }
}
