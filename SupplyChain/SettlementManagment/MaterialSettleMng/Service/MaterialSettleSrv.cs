using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using System.Data.OleDb;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Service
{
    /// <summary>
    /// 物资耗用结算单
    /// </summary>
    public class MaterialSettleSrv : BaseService, IMaterialSettleSrv
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }

        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }


        //private string GetCode(Type type,string specail)
        //{
        //    return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, specail);
        //}

        /// <summary>
        /// 根据项目 物资分类(专业分类) 生成Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <param name="matCatAbb"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId, string matCatAbb)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId, matCatAbb);
        }
        #endregion

        #region 物资耗用结算单
        /// <summary>
        /// 通过ID查询物资耗用结算单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaterialSettleMaster GetMaterialSettleById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialSettle(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialSettleMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询物资耗用结算单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MaterialSettleMaster GetMaterialSettleByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));

            IList list = GetMaterialSettle(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialSettleMaster;
            }
            return null;
        }

        /// <summary>
        /// 物资耗用结算单信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialSettle(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialSettleMaster), objectQuery);
        }

        /// <summary>
        /// 物资耗用结算单信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet MaterialSettleQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT t1.id,t1.Code,t1.monthaccountbill,t1.RealOperationDate,t1.State,t1.CreatePersonName,t1.CreateDate,t1.CreateMonth,t1.CreateYear,t2.Quantity,t2.Money,t2.Price," +
                " t1.Descript,t2.AccountCostName,t2.ProjectTaskName,t2.ProjectTaskCode,t2.MaterialName,t2.MaterialSpec,t2.MaterialStuff, t2.ACCOUNTCOSTCODE FROM THD_MaterialSettleMaster  t1 " +
                " INNER JOIN THD_MaterialSettleDetail  t2 ON t1.Id = t2.ParentId ";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }


        [TransManager]
        public MaterialSettleMaster SaveMaterialSettle(MaterialSettleMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(MaterialSettleMaster), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as MaterialSettleMaster;
        }
        /// <summary>
        /// 根据明细Id查询物资耗用结算单明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public MaterialSettleDetail GetMaterialSettleDetailById(string MaterialSettleDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", MaterialSettleDtlId));
            IList list = dao.ObjectQuery(typeof(MaterialSettleDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialSettleDetail;
            }
            return null;
        }

        [TransManager]
        public IList GetExcel(DataSet ds)
        {
            IList list = new ArrayList();
            Hashtable hashtableMaterialType = new Hashtable();//资源
            Hashtable hashtableMaterialCode = new Hashtable();//资源
            //string strSql = "select * from ResMaterial where OldCode is not null";
            string strSql = "select * from ResMaterial";
            DataTable dtMaterial = SearchSql(strSql);
            for (int i = 0; i < dtMaterial.Rows.Count; i++)
            {
                Application.Resource.MaterialResource.Domain.Material Mat = new Material();
                
                Mat.Id = dtMaterial.Rows[i]["MaterialId"].ToString();
                Mat.Code = dtMaterial.Rows[i]["MatCode"].ToString();
                Mat.Name = dtMaterial.Rows[i]["MatName"].ToString(); 
                Mat.Specification = dtMaterial.Rows[i]["MATSPECIFICATION"].ToString();
                Mat.Stuff = dtMaterial.Rows[i]["STUFF"].ToString();
                Mat.TheSyscode = dtMaterial.Rows[i]["THESYSCODE"].ToString();
                Mat.AccountRoleKind = dtMaterial.Rows[i]["OldCode"].ToString();
                hashtableMaterialType.Add(Mat, Mat.AccountRoleKind);
                hashtableMaterialCode.Add(Mat,Mat.Code);
            }            
            ////查询GUID
            //Application.Resource.MaterialResource.Domain.Material theMaterial = null;
            //ObjectQuery oqt = new ObjectQuery();
            //oqt.AddCriterion(Expression.IsNotNull("OldCode"));
            //IList lst = GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.Material), oqt);
            //if (lst != null && lst.Count > 0)
            //{
            //    for (int i = 0; i < lst.Count; i++)
            //    {
            //        theMaterial = lst[0] as Application.Resource.MaterialResource.Domain.Material;
            //        hashtableMaterialType.Add(theMaterial, theMaterial.Name);
            //    }
            //}

            Hashtable hashtablePart = new Hashtable();//工程任务
            GWBSTree Part = null;
            ObjectQuery oqPart = new ObjectQuery();
            oqPart.AddCriterion(Expression.IsNotNull("Code"));
            IList listPart = GetDomainByCondition(typeof(GWBSTree), oqPart);
            if (listPart != null && listPart.Count > 0)
            {
                for (int i = 0; i < listPart.Count; i++)
                {
                    Part = listPart[i] as GWBSTree;
                    hashtablePart.Add(Part, Part.Code);
                }
            }

            Hashtable hashtableUnit = new Hashtable();//计量单位
            Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
            ObjectQuery oq = new ObjectQuery();
            IList lists = GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
            if (lists != null && lists.Count > 0)
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    Unit = lists[i] as Application.Resource.MaterialResource.Domain.StandardUnit;
                    hashtableUnit.Add(Unit, Unit.Name);
                }
            }

            Hashtable hashtableSubject = new Hashtable();//成本核算科目
            CostAccountSubject Subject = null;
            ObjectQuery oqSub = new ObjectQuery();
            IList list1 = GetDomainByCondition(typeof(CostAccountSubject), oqSub);
            if (list1 != null && list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    Subject = list1[i] as CostAccountSubject;
                    hashtableSubject.Add(Subject, Subject.Code);
                }
            }

            //DataSet OleDsExcle = OpenExcel(path);

            if (ds.Tables[0].Columns.Count != 0)
            {
                int Columns = ds.Tables[0].Columns.Count;
                if (Columns < 12)
                {
                    return list;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)//循环读取临时表的行
                {
                    MaterialSettleDetail mDetail = new MaterialSettleDetail();
                    string strMaterialName = ds.Tables[0].Rows[i][0].ToString();//资源名称
                    string strMaterialCode = ds.Tables[0].Rows[i][1].ToString();//资源编号
                    Application.Resource.MaterialResource.Domain.Material MaterialGUID = new Application.Resource.MaterialResource.Domain.Material();
                    string strPartName = ds.Tables[0].Rows[i][2].ToString();//部位名称
                    string strPartCode = ds.Tables[0].Rows[i][3].ToString();//部位编号
                    GWBSTree PartGUID = new GWBSTree();
                    string strSubjectName = ds.Tables[0].Rows[i][4].ToString();//科目名称
                    string strSubjectCode = ds.Tables[0].Rows[i][5].ToString();//科目编号
                    CostAccountSubject SubjectGUID = new CostAccountSubject();
                    string strQuantity = ds.Tables[0].Rows[i][6].ToString();//数量
                    string strQuantityUnit = ds.Tables[0].Rows[i][7].ToString();//数量单位
                    StandardUnit QuantityGUID = new StandardUnit();
                    string strPriceUnit = ds.Tables[0].Rows[i][10].ToString();//价格单位
                    StandardUnit PriceGUID = new StandardUnit();
                    string strPrice = ds.Tables[0].Rows[i][8].ToString();//单价
                    string strMoney = ds.Tables[0].Rows[i][9].ToString();//合价
                    string strDescript = ds.Tables[0].Rows[i][11].ToString();//备注
                    if (strMaterialName != "" && strPartName != "" && strSubjectName != "")//不将空白行插入list
                    {
                        mDetail.Quantity = Convert.ToDecimal(strQuantity);
                        mDetail.Descript = strDescript;
                        mDetail.Money = Convert.ToDecimal(strMoney);
                        mDetail.Price = Convert.ToDecimal(strPrice);
                        if (strMaterialCode != "")//资源不为空
                        {
                            if (strMaterialCode.Substring(0, 1).Equals("I"))
                            {
                                if (strMaterialCode.Length < 8)
                                {
                                    strMaterialCode += "00000";
                                }
                                foreach (System.Collections.DictionaryEntry objName in hashtableMaterialCode)
                                {
                                    if (objName.Value.ToString().Equals(strMaterialCode))
                                    {
                                        MaterialGUID = (Application.Resource.MaterialResource.Domain.Material)objName.Key;
                                        mDetail.MaterialCode = MaterialGUID.Code;
                                        mDetail.MaterialName = MaterialGUID.Name;
                                        mDetail.MaterialSpec = MaterialGUID.Specification;
                                        mDetail.MaterialResource = MaterialGUID;
                                        mDetail.MaterialStuff = MaterialGUID.Stuff;
                                        mDetail.MaterialSysCode = MaterialGUID.TheSyscode;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                foreach (System.Collections.DictionaryEntry objName in hashtableMaterialType)
                                {
                                    if (objName.Value.ToString().Equals(strMaterialCode))
                                    {
                                        MaterialGUID = (Application.Resource.MaterialResource.Domain.Material)objName.Key;//获得ID
                                        mDetail.MaterialCode = MaterialGUID.Code;
                                        mDetail.MaterialName = MaterialGUID.Name;
                                        mDetail.MaterialSpec = MaterialGUID.Specification;
                                        mDetail.MaterialResource = MaterialGUID;
                                        mDetail.MaterialStuff = MaterialGUID.Stuff;
                                        mDetail.MaterialSysCode = MaterialGUID.TheSyscode;
                                        break;
                                    }
                                }
                            }
                        }
                        if (strPartName != "")//部位不为空（工程任务）
                        {
                            foreach (System.Collections.DictionaryEntry objName in hashtablePart)
                            {
                                if (objName.Value.ToString().Equals(strPartCode))
                                {
                                    PartGUID = (GWBSTree)objName.Key;
                                    mDetail.ProjectTask = PartGUID;
                                    mDetail.ProjectTaskCode = PartGUID.SysCode;
                                    mDetail.ProjectTaskName = PartGUID.Name;
                                    break;
                                }
                            }
                        }
                        if (strSubjectName != "")//科目不为空
                        {
                            foreach (System.Collections.DictionaryEntry objName in hashtableSubject)
                            {
                                if (objName.Value.ToString().Equals(strSubjectCode))
                                {
                                    SubjectGUID = (CostAccountSubject)objName.Key;
                                    mDetail.AccountCostCode = SubjectGUID.SysCode;
                                    mDetail.AccountCostName = SubjectGUID.Name;
                                    mDetail.AccountCostSubject = SubjectGUID;
                                    break;
                                }
                            }
                        }
                        if (strPriceUnit != "")//价格计量单位不为空
                        {
                            foreach (System.Collections.DictionaryEntry objName in hashtableUnit)
                            {
                                if (objName.Value.ToString().Equals(strPriceUnit))
                                {
                                    PriceGUID = (StandardUnit)objName.Key;
                                    mDetail.PriceUnit = PriceGUID;
                                    mDetail.PriceUnitName = PriceGUID.Name;
                                    break;
                                }
                            }
                        }
                        if (strQuantityUnit != "")
                        {
                            foreach (System.Collections.DictionaryEntry objName in hashtableUnit)
                            {
                                if (objName.Value.ToString().Equals(strQuantityUnit))
                                {
                                    QuantityGUID = (StandardUnit)objName.Key;
                                    mDetail.QuantityUnit = QuantityGUID;
                                    mDetail.QuantityUnitName = QuantityGUID.Name;
                                    break;
                                }
                            }
                        }
                        list.Add(mDetail);
                    }
                }
            }
            return list;
        }
        
        /// <summary>
        /// 查询基础数据信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable SearchSql(string sql)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            //IDbTransaction transaction = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //command.Transaction = transaction;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dt = new DataTable();
            dt = dataSet.Tables[0];
            return dt;

        }

       

        //private IList SearchObjectBySql(string sql)
        //{
        //    ISession session = CallContext.GetData("nhsession") as ISession;
        //    IQuery query= session.CreateQuery(sql);
        //    IList list= query.List<Material>();

        //    return list;

        //    //IDbConnection conn = session.Connection;
        //    ////IDbTransaction transaction = conn.BeginTransaction();
        //    //IDbCommand command = conn.CreateCommand();
        //    ////command.Transaction = transaction;
        //    //command.CommandText = sql;
        //    //IDataReader dataReader = command.ExecuteReader();
        //    //DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
        //    //DataTable dt = new DataTable();
        //    //dt = dataSet.Tables[0];
        //    //return dt;

        //}
        #endregion
    }
}
