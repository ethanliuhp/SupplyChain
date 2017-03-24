
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using System.Data;
using VirtualMachine.Notice.Domain;
using Application.Business.Erp.SupplyChain.NoiceMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.NoticeMng
{
    public class MNotice
    {
        private static INoiceSrv EdiDeptSrv = null;


        public MNotice()
        {
            if (EdiDeptSrv == null)
            {
                EdiDeptSrv = StaticMethod.GetService("NoiceSrv") as INoiceSrv;
            }
        }

        public Notice Save(Notice obj)
        {
            if (obj.Id == "")
            {
                return EdiDeptSrv.Save(obj) as Notice;
            }
            else
            {
                return EdiDeptSrv.Update(obj) as Notice;
            }
        }
        public bool Delete(Notice obj)
        {
            return EdiDeptSrv.Delete(obj);
        }

        public Notice Update(Notice obj)
        {
            return EdiDeptSrv.Update(obj) as Notice;

        }
        public IList getobj()
        {
            return EdiDeptSrv.GetObjects(typeof(Notice));
        }

        public IList getEdiDept(ObjectQuery oq)
        {
            return EdiDeptSrv.GetObjects(typeof(Notice), oq);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet select(string condition)
        {
            return EdiDeptSrv.Select(condition);
        }
        /// <summary>
        /// 保存集合
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public IList save(IList lst)
        {
            return EdiDeptSrv.Save(lst);
        }
    }
}