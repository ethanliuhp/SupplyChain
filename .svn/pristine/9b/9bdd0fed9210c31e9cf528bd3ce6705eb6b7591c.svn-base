using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain;
using Application.Resource.CommonClass.Attributes;
using VirtualMachine.Component.Util;
using CommonSearchLib.BillCodeMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn
{
    [ClassDescription("盘盈单Module")]
    public class MProfitIn
    {
        private static IProfitInSrv theProfitInSrv;
        private static IBillCodeRuleSrv theBillCodeRuleSrv;

        public MProfitIn()
        {
            if (theProfitInSrv == null)
            {
                theProfitInSrv = StaticMethod.GetService("ProfitInSrv") as IProfitInSrv;
            }
            if (theBillCodeRuleSrv == null)
            {
                theBillCodeRuleSrv = StaticMethod.GetService("BillCodeRuleSrv") as IBillCodeRuleSrv;
            }
        }
        //xl
        public IProfitInSrv ProfitInSrv
        {
            get { return theProfitInSrv; }
            set { theProfitInSrv = value; }
        }
        ///// <summary>
        ///// 盘盈单保存
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public ProfitIn SaveProfitInMaster(ProfitIn obj)
        //{
        //    return theProfitInSrv.SaveProfitIn(obj);
        //}


        [MethodDescription("保存")]
        public ProfitIn Save(ProfitIn obj)
        {
            if (obj.Id == null)
            {
                //obj.Code = "12345";
                obj.Code = theBillCodeRuleSrv.GetBillNoNextValue(typeof(ProfitIn), "Code", CommonMethod.GetServerDateTime(),obj.Special);
               // return theProfitInSrv.SaveByDao(obj) as ProfitIn;
                return theProfitInSrv.SaveProfitIn(obj);
            }
            else
            {
                return theProfitInSrv.UpdateByDao(obj) as ProfitIn;
            }
        }
 
        [MethodDescription("修改")]
        public ProfitIn Update(ProfitIn obj)
        {
            return theProfitInSrv.UpdateByDao(obj) as ProfitIn;
        }
        [MethodDescription("删除")]
        public bool Delete(ProfitIn obj)
        {
            return theProfitInSrv.DeleteByDao(obj);
        }
        [MethodDescription("根据Id获得域")]
        public ProfitIn GetObjectById(string id)
        {
            //return theProfitInSrv.GetDomain(typeof(ProfitIn), id, new ObjectQuery()) as ProfitIn; 
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = theProfitInSrv.GetDomainByCondition(typeof(ProfitIn), oq);
            if (list.Count > 0)
            {
                return list[0] as ProfitIn;
            }
            return null;
        }
        [MethodDescription("根据Code获得域")]
        public ProfitIn GetObject(string code,string special,string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = theProfitInSrv.GetDomainByCondition(typeof(ProfitIn), oq);
            if (list.Count > 0)
            {
                return GetObjectById((list[0] as ProfitIn).Id);
            }
            return null;
        }
        [MethodDescription("根据ObjectQuery获得域")]
        public IList GetObject(ObjectQuery oq)
        {
            return theProfitInSrv.GetDomainByCondition(typeof(ProfitIn), oq);
        }

        public IList GetDetailList(ObjectQuery oq)
        {
            return theProfitInSrv.GetDetailList(typeof(ProfitInDtl), oq);
        }
        
        [MethodDescription("记账")]
        public Hashtable Tally(Hashtable hashBillId, Hashtable hashBillCode)
        {
            return theProfitInSrv.Tally(hashBillId, hashBillCode, ConstObject.TheLogin.LoginDate, ConstObject.TheLogin.TheComponentPeriod, ConstObject.TheLogin.ThePerson, StaticMethod.GetProjectInfo().Id);
        }


        [MethodDescription("保存(账面盘盈盘亏单)")]
        public AcctLoseAndProfit SaveByAcct(AcctLoseAndProfit obj)
        {
            if (obj.Id == null)
            {
                return theProfitInSrv.Save(obj) as AcctLoseAndProfit;
            }
            else
            {
                return theProfitInSrv.Update(obj) as AcctLoseAndProfit;
            }
        }

        [MethodDescription("修改(账面盘盈盘亏单)")]
        public AcctLoseAndProfit UpdateByAcct(AcctLoseAndProfit obj)
        {
            return theProfitInSrv.Update(obj) as AcctLoseAndProfit;
        }
        [MethodDescription("删除(账面盘盈盘亏单)")]
        public bool DeleteByAcct(AcctLoseAndProfit obj)
        {
            return theProfitInSrv.DeleteObject(obj);
        }
    }
}
