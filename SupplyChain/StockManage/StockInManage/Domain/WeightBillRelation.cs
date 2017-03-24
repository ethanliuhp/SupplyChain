using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    public  enum EnumRefBillType{
        入库单=0
    }
   public  class WeightBillRelation
    {
       private string   id;
       private string projectId;
       private string projectCode;
       private long weightId;
       private EnumRefBillType refBillType;
       private long version;
       virtual public string  Id
       {
           get { return id; }
           set { id = value; }
       }
       /// <summary>
       /// 版本
       /// </summary>
       virtual public long Version
       {
           get { return version; }
           set { version = value; }
       }
       virtual public string ProjectId
       {
           get { return projectId; }
           set { projectId = value; }
       }
       virtual public string ProjectCode
       {
           get { return projectCode; }
           set { projectCode = value; }
       }
       virtual public long WeightId
       {
           get { return weightId; }
           set { weightId = value; }
       }
       virtual public EnumRefBillType RefBillType
       {
           get { return refBillType; }
           set { refBillType = value; }
       }
    }
}
