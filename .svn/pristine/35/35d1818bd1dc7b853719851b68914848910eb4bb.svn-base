using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace IRPServiceModel.Domain.Basic
{
   public  class BasicMasterBill
    {
       private string id;
       private long version;
       private DateTime createDate = DateTime.Now;
       private DocumentState docState = DocumentState.Edit;
       /// <summary>
       /// 版本号
       /// </summary>
       public virtual long Version
       {
           get { return version; }
           set { version = value; }
       }
       /// <summary>
       /// 唯一ID
       /// </summary>
       public virtual string Id
       {
           get { return id; }
           set { id = value; }
       }
       /// <summary>
       /// 创建时间
       /// </summary>
       public virtual DateTime CreateDate
       {
           get { return createDate; }
           set { createDate = value; }
       }
       public virtual DocumentState DocState
       {
           get { return docState; }
           set { docState = value; }
       }

    }
}
