using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain;
using Application.Resource.CommonClass.Attributes;
using VirtualMachine.Component.Util;
using CommonSearchLib.BillCodeMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut
{
    [ClassDescription("盘亏单Module")]
    public class MLossOut
    {
        private static ILossOutSrv theLossOutSrv = null;

        public MLossOut()
        {
            if (theLossOutSrv == null)
            {
                theLossOutSrv = StaticMethod.GetService("LossOutSrv") as ILossOutSrv;
            }
        }
        public ILossOutSrv LossOutSrv
        {
            get { return theLossOutSrv; }
            set { theLossOutSrv = value; }
        }
        [MethodDescription("保存")]
        public LossOut Save(LossOut obj)
        {
            return theLossOutSrv.SaveLossOut(obj);
        }
        [MethodDescription("修改")]
        public LossOut Update(LossOut obj)
        {
            return theLossOutSrv.UpdateByDao(obj) as LossOut;
        }
        [MethodDescription("删除")]
        public bool Delete(LossOut obj)
        {
            return theLossOutSrv.DeleteLossOut(obj);
        }
        [MethodDescription("根据Id获得域")]
        public LossOut GetObjectById(string id)
        { 
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetObject(oq);
            if (list.Count > 0)
            {
                return list[0] as LossOut;
            }
            return null;
        }
        [MethodDescription("根据Code获得域")]
        public LossOut GetObject(string code,string special,string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetObject(oq);
            if (list.Count > 0)
            {
                return GetObjectById((list[0] as LossOut).Id);
            }
            return null;
        }
        [MethodDescription("根据ObjectQuery获得域")]
        public IList GetObject(ObjectQuery oq)
        {
            return theLossOutSrv.GetLossOut(oq);
        }

        [MethodDescription("记账")]
        public Hashtable Tally(Hashtable hashlst, Hashtable hashCode)
        {
            return theLossOutSrv.Tally(hashlst, hashCode, ConstObject.TheLogin.LoginDate, ConstObject.TheLogin.TheComponentPeriod, ConstObject.TheLogin.ThePerson, StaticMethod.GetProjectInfo().Id);
        }
    }
}
