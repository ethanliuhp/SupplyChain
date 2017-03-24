using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using IRPServiceModel.Domain.CompanyMng;
using VirtualMachine.Core;

namespace IRPServiceModel.Services.CompanyMng
{
   public  interface ICompanyInfoSvr
    {
       IList GetTree();
       /// <summary>
       /// 创建树的根节点
       /// </summary>
       /// <returns></returns>
       CompanyInfo SavePlaceTreeRootNode();
       /// <summary>
       /// 查询
       /// </summary>
       /// <param name="type"></param>
       /// <param name="oQuery"></param>
       /// <returns></returns>
       IList Query(Type type, ObjectQuery oQuery);
       /// <summary>
       /// 根据ID获取公司信息
       /// </summary>
       /// <param name="strID"></param>
       /// <returns></returns>
       CompanyInfo GetCompanyInfoById(string strID);
       object QueryById(Type type, string strID);
       CompanyInfo SaveOrUpdate(CompanyInfo oNode);
       bool DeleteCompanyInfoByID(string sID);
    }
}
