using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.Financial.GlobalInfo;
using VirtualMachine.Core.DataAccess;
using System.Data;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service
{
    /// <summary>
    /// ��Ŀ����Service
    /// </summary>
    public class AccTitleCodeImpl : CommonAccBookSrv, IAccTitleCodeSrv
    {
        /// <summary>
        /// ��ȡ��Ŀ����Ĭ��ֵ
        /// </summary>
        /// <param name="assisCode">��Ŀ�ּ�</param>
        /// <param name="accLevel">��Ŀ����</param>
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
