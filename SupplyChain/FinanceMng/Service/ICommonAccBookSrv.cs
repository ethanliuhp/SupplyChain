using System;
namespace Application.Business.Erp.Financial.GlobalInfo
{
    public interface ICommonAccBookSrv
    {
        //校验会计期是否正确
        bool VerifyDate(int inYear, int inMonth);
        bool VerifyDate(DateTime inDate);
    }
}
