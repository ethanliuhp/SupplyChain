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
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Service;

namespace Application.Business.Erp.SupplyChain.ExcelImportMng.Service
{


    /// <summary>
    /// 基础数据导入服务
    /// </summary>
    public class ExcelImportSrv : BaseService, IExcelImportSrv
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
        #endregion

        #region 注入服务
        private ICostItemCategorySrv costTypeSrv;
        public ICostItemCategorySrv CostTypeSrv
        {
            get { return costTypeSrv; }
            set { costTypeSrv = value; }
        }
        private ICostAccountSubjectSrv costSubjectSrv;
        public ICostAccountSubjectSrv CostSubjectSrv
        {
            get { return costSubjectSrv; }
            set { costSubjectSrv = value; }
        }
        private ICostItemSrv costItemSrv;
        public ICostItemSrv CostItemSrv
        {
            get { return costItemSrv; }
            set { costItemSrv = value; }
        }

        #endregion


        #region 物料分类信息
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public IDbConnection OpenConn()
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            return conn;
        }
        /// <summary>
        /// 保存基础数据信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int SaveSql(string sql)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = sql;
            int res = command.ExecuteNonQuery();
            return res;

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
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dt = new DataTable();
            dt = dataSet.Tables[0];
            return dt;

        }
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }
        /// <summary>
        /// 保存成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        [TransManager]
        public IList Save(IList list)
        {
            dao.Save(list);
            return list;
        }

        [TransManager]
        public CostAccountSubject SaveCostAccountSubject(CostAccountSubject obj)
        {
            //obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as CostAccountSubject;
        }


        #endregion

        #region Id生成方法
        private IFCGuidGenerator billIdRuleSrv;
        public IFCGuidGenerator BillIdRuleSrv
        {
            get { return billIdRuleSrv; }
            set { billIdRuleSrv = value; }
        }

        public string GetGuid()
        {
            string guid = "";
            IFCGuidGenerator gen = new IFCGuidGenerator();
            guid = gen.GeneratorIFCGuid();
            return guid;
        }
        DateTime strDate = DateTime.Now;
        #endregion

        #region 成本项
        [TransManager]
        public void SaveCostItem(DataSet OleDsExcle, CurrentProjectInfo projectInfo)
        {
            CostAccountSubject curBillMaster = new CostAccountSubject();

            //打开数据库连接。返回command
            //IDbConnection conn = model.ExcelImportSrv.OpenConn();
            Hashtable hashtableCostType = new Hashtable();//成本项分类
            string strCostType = "select ID,CODE from THD_CostItemCategory";
            DataTable dtCostType = SearchSql(strCostType);
            for (int i = 0; i < dtCostType.Rows.Count; i++)
            {
                string strID = dtCostType.Rows[i][0].ToString();
                string strCode = dtCostType.Rows[i][1].ToString();
                hashtableCostType.Add(strID, strCode);
            }
            Hashtable hashtableCost = new Hashtable();//成本项
            string strCost = "select ID,Code from THD_CostItem";
            DataTable dtCost = SearchSql(strCost);
            for (int i = 0; i < dtCost.Rows.Count; i++)
            {
                string strID = dtCost.Rows[i][0].ToString();
                string strCode = dtCost.Rows[i][1].ToString();
                hashtableCost.Add(strID, strCode);
            }
            Hashtable hashtableSubjectCost = new Hashtable();//资源耗用定额
            string strSubjectCost = "select ID,Name from THD_SubjectCostQuota";
            DataTable dtSubjectCost = SearchSql(strSubjectCost);
            for (int i = 0; i < dtSubjectCost.Rows.Count; i++)
            {
                string strID = dtSubjectCost.Rows[i][0].ToString();
                string strCode = dtSubjectCost.Rows[i][1].ToString();
                hashtableSubjectCost.Add(strID, strCode);
            }

            Hashtable hashtableUnit = new Hashtable();
            string strSearchUint = "select STANDUNITID,STANDUNITNAME from RESSTANDUNIT";//计量单位
            DataTable strUnitdt = SearchSql(strSearchUint);
            for (int k = 0; k < strUnitdt.Rows.Count; k++)
            {
                string strUnitId = strUnitdt.Rows[k][0].ToString();
                string strUnitName = strUnitdt.Rows[k][1].ToString();
                hashtableUnit.Add(strUnitId, strUnitName);
            }

            Hashtable hashtableResource = new Hashtable();//资源
            string strResourceCode = "select MaterialID,MatCode from ResMaterial";
            DataTable dtResource = SearchSql(strResourceCode);
            for (int k = 0; k < dtResource.Rows.Count; k++)
            {
                string strResourceId = dtResource.Rows[k][0].ToString();
                string strResourceName = dtResource.Rows[k][1].ToString();
                hashtableResource.Add(strResourceId, strResourceName);
            }
            Hashtable hashtableCostAccount = new Hashtable();//成本科目
            Hashtable hashtableCostAccountName = new Hashtable();//成本科目名称
            string strCostAct = "select ID,CODE,Name from thd_CostAccountSubject";
            DataTable dtCostAccount = SearchSql(strCostAct);
            for (int k = 0; k < dtCostAccount.Rows.Count; k++)
            {
                string strCostAccountId = dtCostAccount.Rows[k][0].ToString();
                string strCostAccountCode = dtCostAccount.Rows[k][1].ToString();
                string strCostAccountName = dtCostAccount.Rows[k][2].ToString();
                hashtableCostAccount.Add(strCostAccountId, strCostAccountCode);
                hashtableCostAccountName.Add(strCostAccountId, strCostAccountName);
            }



            int Rows = 0;
            if (OleDsExcle.Tables[0].Columns.Count != 0)
            {
                int Columns = OleDsExcle.Tables[0].Columns.Count;
                if (Columns < 33)
                {
                    MessageBox.Show("Excel格式不正确！");
                    return;
                }
                int IntForword = 1;//自动生成顺序码
                string strForword = null;
                string strCostName = null;
                string strCCode = null;
                string strPart = null;
                string strResource = null;
                string strDoWay = null;
                string strUnit = null;
                string strUnitGUID = null;
                string strDCost = null;
                string PriceUnitGUID = null;
                string strPriceUnit = null;
                int IntApplicableLevel = 0;
                string strName = null;//成本项名称
                string strGuid = null;
                int IntContentType = 0;//内容类型
                string strCostTypeId = null;

                for (int i = 2; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行
                {
                    string strDDCost = OleDsExcle.Tables[0].Rows[i][2].ToString();//定额编码
                    if (strDDCost.Equals(""))
                    { }
                    else
                    {
                        string strCostName1 = OleDsExcle.Tables[0].Rows[i][3].ToString();//成本项名称
                        if (strCostName1.Equals(""))
                        { }
                        else
                        {
                            strCostName = strCostName1;
                            strDCost = strDDCost;
                            strCCode = OleDsExcle.Tables[0].Rows[i][1].ToString();//成本项分类代码
                            strPart = OleDsExcle.Tables[0].Rows[i][5].ToString();//部位
                            strResource = OleDsExcle.Tables[0].Rows[i][6].ToString();//资源
                            strDoWay = OleDsExcle.Tables[0].Rows[i][7].ToString();//做法
                            strUnit = OleDsExcle.Tables[0].Rows[i][12].ToString();//计量单位
                            strUnitGUID = null;
                            foreach (System.Collections.DictionaryEntry objUnit in hashtableUnit)
                            {
                                if (objUnit.Value.ToString().Equals(strUnit))
                                {
                                    strUnitGUID = objUnit.Key.ToString();
                                    break;
                                }
                            }

                            strPriceUnit = "元";//价格计量单位
                            foreach (System.Collections.DictionaryEntry objPriceUnit in hashtableUnit)
                            {
                                if (objPriceUnit.Value.ToString().Equals(strPriceUnit))
                                {
                                    PriceUnitGUID = objPriceUnit.Key.ToString();
                                    break;
                                }
                            }
                            IntApplicableLevel = 2;//适用级别
                            if (strPart.Equals(""))
                            {
                                if (strResource.Equals(""))
                                {
                                    if (strDoWay.Equals(""))
                                    {
                                        strName = strCostName;
                                    }
                                    else
                                    {
                                        strName = strCostName + "(" + strDoWay + ")";
                                    }
                                }
                                else
                                {
                                    if (strDoWay.Equals(""))
                                    {
                                        strName = strCostName + "(" + strResource + ")";
                                    }
                                    else
                                    {
                                        strName = strCostName + "(" + strResource + "," + strDoWay + ")";
                                    }

                                }
                            }
                            else
                            {
                                if (strResource.Equals(""))
                                {
                                    if (strDoWay.Equals(""))
                                    {
                                        strName = strCostName + "(" + strPart + ")";
                                    }
                                    else
                                    {
                                        strName = strCostName + "(" + strPart + "," + strDoWay + ")";
                                    }
                                }
                                else
                                {
                                    if (strDoWay.Equals(""))
                                    {
                                        strName = strCostName + "(" + strPart + "," + strResource + ")";
                                    }
                                    else
                                    {
                                        strName = strCostName + "(" + strPart + "," + strResource + "," + strDoWay + ")";
                                    }
                                }
                            }
                            strGuid = GetGuid();//生成Id
                            string strFour = strCCode.Substring(0, 4);
                            if (strFour.Equals("0104"))
                            {
                                IntContentType = 1;
                            }
                            else
                            {
                                IntContentType = 0;
                            }
                            //通过strCCode成本项分类代码查找成本项分类的ID

                            foreach (System.Collections.DictionaryEntry objDE in hashtableCostType)
                            {
                                if (objDE.Value.ToString().Equals(strCCode))
                                {
                                    strCostTypeId = objDE.Key.ToString();//获得ID
                                    break;
                                }
                                //没有找到相应的信息跳出循环
                            }
                            if (IntForword.ToString().Length == 1)
                            {
                                strForword = "0000" + IntForword.ToString();
                            }
                            if (IntForword.ToString().Length == 2)
                            {
                                strForword = "000" + IntForword.ToString();
                            }
                            if (IntForword.ToString().Length == 3)
                            {
                                strForword = "00" + IntForword.ToString();
                            }
                            if (IntForword.ToString().Length == 4)
                            {
                                strForword = "0" + IntForword.ToString();
                            }
                            if (IntForword.ToString().Length == 5)
                            {
                                strForword = IntForword.ToString();
                            }
                            string strCodeCode = strCCode + strForword;

                            string strsql = "insert into THD_CostItem (ID,VERSION,CODE,NAME,QUOTACODE,THECOSTITEMCATEGORY,PROJECTUNITGUID,PROJECTUNITNAME,PRICEUNITGUID,PRICEUNITNAME,APPLYLEVEL,THEPROJECTGUID,THEPROJECTNAME,CREATETIME,ITEMSTATE,CONTENTTYPE)";
                            strsql += "values('" + strGuid + "','0','" + strCodeCode + "','" + strName + "','" + strDCost + "','" + strCostTypeId + "','" + strUnitGUID + "','" + strUnit + "','" + PriceUnitGUID + "','" + strPriceUnit + "','" + IntApplicableLevel + "','" + projectInfo.Id + "','" + projectInfo.Name + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'2','" + IntContentType + "')";

                            int res = SaveSql(strsql);
                            if (res != 0)
                            {
                                Rows += 1;
                                IntForword += 1;
                                hashtableCost.Add(strGuid, strCCode);
                            }
                        }
                        //资源耗用定额
                        string strCBName = OleDsExcle.Tables[0].Rows[i][22].ToString();//成本名称
                        string strResourceType = OleDsExcle.Tables[0].Rows[i][23].ToString();//资源类型
                        string strCostAccount = OleDsExcle.Tables[0].Rows[i][24].ToString();//成本核算科目
                        decimal strLoss = Convert.ToDecimal(OleDsExcle.Tables[0].Rows[i][25].ToString());//定额数量
                        string strDProjectUnit = OleDsExcle.Tables[0].Rows[i][29].ToString();//定额工程量计量单位
                        string strCostPriceUnit = OleDsExcle.Tables[0].Rows[i][30].ToString();//价格计量单位
                        string strResourceSign = OleDsExcle.Tables[0].Rows[i][33].ToString();//主资源标志
                        string strResourceId = null;//资源编码
                        string strCostAccountId = null;//成本核算科目编码
                        string strCostAccountName = null;//成本核算科目名称
                        string strUnitQuantity = null;//定额工程量计量单位编码
                        string strUnitPrice = null;//价格计量单位名称

                        //成本核算科目
                        foreach (System.Collections.DictionaryEntry objAccount in hashtableCostAccount)
                        {
                            if (objAccount.Value.ToString().Equals(strCostAccount))
                            {
                                strCostAccountId = objAccount.Key.ToString();//获得资源ID
                                foreach (System.Collections.DictionaryEntry objAccountName in hashtableCostAccountName)
                                {
                                    if (objAccount.Key.ToString().Equals(strCostAccountId))
                                    {
                                        strCostAccountName = objAccount.Value.ToString();//获得资源ID
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        //工程量计量单位
                        foreach (System.Collections.DictionaryEntry objUnitPrice in hashtableUnit)
                        {
                            if (objUnitPrice.Value.ToString().Equals(strDProjectUnit))
                            {
                                strUnitQuantity = objUnitPrice.Key.ToString();//获得资源ID
                                break;
                            }
                        }
                        //价格计量单位
                        foreach (System.Collections.DictionaryEntry objUnitQuantity in hashtableUnit)
                        {
                            if (objUnitQuantity.Value.ToString().Equals(strCostPriceUnit))
                            {
                                strUnitPrice = objUnitQuantity.Key.ToString();//获得资源ID
                                break;
                            }
                        }

                        if (strResourceType.Contains("/"))
                        {
                            //资源类型里面含有“/”
                            string Teamp = strResourceType.Replace("/", "");
                            int length = strResourceType.Length - Teamp.Length;//字符串中的“/”个数
                            string strSubID = null;//资源耗用类型ID
                            for (int j = length; j >= 0; j--)
                            {
                                string[] sArray = strResourceType.Split('/');
                                string stra = sArray[j];
                                foreach (System.Collections.DictionaryEntry objSub in hashtableResource)
                                {
                                    if (objSub.Value.ToString().Equals(stra))
                                    {
                                        strResourceId = objSub.Key.ToString();//获得资源ID
                                        break;
                                    }
                                }

                                if (j == length)
                                {
                                    strSubID = GetGuid();
                                    string strSubsql = "insert into Thd_Subjectcostquota (ID,VERSION,NAME,PROJECTAMOUNTUNITGUID,PROJECTAMOUNTUNITNAME,PRICEUNITGUID,PRICEUNITNAME,COSTACCOUNTSUBJECTGUID,COSTACCOUNTSUBJECTNAME,COSTITEMID,QUOTAPROJECTAMOUNT,MainResourceFlag,CREATETIME,State,ResourceTypeGUID,ResourceTypeName,TheProjectGUID,TheProjectName)";
                                    strSubsql += "values('" + strSubID + "','0','" + strCBName + "','" + strUnitQuantity + "','" + strDProjectUnit + "','" + strUnitPrice + "','" + strCostPriceUnit + "','" + strCostAccountId + "','" + strCostAccountName + "','" + strGuid + "','" + strLoss + "','" + strResourceSign + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'2','" + strResourceId + "','" + stra + "','" + projectInfo.Id + "','" + projectInfo.Name + "')";

                                    int resSub = SaveSql(strSubsql);
                                    if (resSub != 0)
                                    {
                                        hashtableSubjectCost.Add(strSubID, strCBName);
                                        //向resourcegroup添加信息

                                    }

                                }
                                string strResourceGroupID = GetGuid();
                                string strGroupsql = "insert into Thd_ResourceGroup (ID,VERSION,ResourceTypeGUID,RESOURCETYPENAME,CostQuotaId)";
                                strGroupsql += "values('" + strResourceGroupID + "','0','" + strResourceId + "','" + stra + "','" + strSubID + "')";
                                int resGroup = SaveSql(strGroupsql);
                            }
                        }
                        else
                        {
                            string strNewType = null;
                            strNewType = strResourceType + "00000";
                            //资源类型
                            foreach (System.Collections.DictionaryEntry objSub in hashtableResource)
                            {
                                if (objSub.Value.ToString().Equals(strNewType))
                                {
                                    strResourceId = objSub.Key.ToString();//获得资源ID
                                    break;
                                }
                            }
                            string strSubID = GetGuid();//资源耗用类型ID
                            string strSubsql = "insert into Thd_Subjectcostquota (ID,VERSION,NAME,PROJECTAMOUNTUNITGUID,PROJECTAMOUNTUNITNAME,PRICEUNITGUID,PRICEUNITNAME,COSTACCOUNTSUBJECTGUID,COSTACCOUNTSUBJECTNAME,COSTITEMID,QUOTAPROJECTAMOUNT,MainResourceFlag,CREATETIME,State,ResourceTypeGUID,ResourceTypeName,TheProjectGUID,TheProjectName)";
                            strSubsql += "values('" + strSubID + "','0','" + strCBName + "','" + strUnitQuantity + "','" + strDProjectUnit + "','" + strUnitPrice + "','" + strCostPriceUnit + "','" + strCostAccountId + "','" + strCostAccountName + "','" + strGuid + "','" + strLoss + "','" + strResourceSign + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'2','" + strResourceId + "','" + strResourceType + "','" + projectInfo.Id + "','" + projectInfo.Name + "')";

                            int resSub = SaveSql(strSubsql);
                            if (resSub != 0)
                            {
                                hashtableSubjectCost.Add(strSubID, strCBName);
                                //向resourcegroup添加信息
                                string strResourceGroupID = GetGuid();
                                string strGroupsql = "insert into Thd_ResourceGroup (ID,VERSION,ResourceTypeGUID,RESOURCETYPENAME,CostQuotaId)";
                                strGroupsql += "values('" + strResourceGroupID + "','0','" + strResourceId + "','" + strResourceType + "','" + strSubID + "')";
                                int resGroup = SaveSql(strGroupsql);
                            }
                        }

                    }
                }
            }
            MessageBox.Show(Rows + "行信息保存成功！");

        }
        #endregion

        #region 成本核算科目
        [TransManager]
        public void SaveCostAccountSubject(DataSet OleDsExcle, CurrentProjectInfo projectInfo, string strOperOrgInfoId, string strPersonName, string strOpgSysCode)
        {
            CostAccountSubject curBillMaster = new CostAccountSubject();
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            IList Costlist = Dao.ObjectQuery(typeof(CostAccountSubject), objectQuery);
            Hashtable hashtableSubject = new Hashtable();
            Hashtable hashtableSystemCode = new Hashtable();
            Hashtable hashtableName = new Hashtable();
            string strSearchSubject = "select ID,CODE,SYSCODE,Name from thd_CostAccountSubject";
            DataTable strdtSubject = SearchSql(strSearchSubject);
            for (int t = 0; t < strdtSubject.Rows.Count; t++)
            {
                string strSubjectId = strdtSubject.Rows[t][0].ToString();
                string strSubjectCode = strdtSubject.Rows[t][1].ToString();
                string strSubjectSysCode = strdtSubject.Rows[t][2].ToString();
                string strSubjectName = strdtSubject.Rows[t][3].ToString();
                hashtableSubject.Add(strSubjectId, strSubjectCode);
                hashtableSystemCode.Add(strSubjectId, strSubjectSysCode);
                hashtableName.Add(strSubjectId, strSubjectName);
            }
            int Rows = 0;
            if (OleDsExcle.Tables[0].Columns.Count != 0)
            {
                int Columns = OleDsExcle.Tables[0].Columns.Count;
                if (Columns < 3)
                {
                    MessageBox.Show("Excel格式不正确！");
                    return;
                }

                for (int i = 1; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行
                {
                    string strCode = OleDsExcle.Tables[0].Rows[i][1].ToString();//读取临时表第i行第1列的数据
                    string strSubjectName = OleDsExcle.Tables[0].Rows[i][2].ToString();//获得成本核算科目名称
                    string strFlag = OleDsExcle.Tables[0].Rows[i][3].ToString();//获得是否成本核算科目标志
                    int IntFlag = 1;
                    if (strFlag.Equals(""))
                    {
                        IntFlag = 0;
                    }
                    if (strCode.Equals(""))
                    { }
                    else
                    {
                        if (strCode.Length == 4)
                        {
                            string strGuid = GetGuid();//生成Id
                            var queryRoot = from p in Costlist.OfType<CostAccountSubject>()
                                            where p.CategoryNodeType == NodeType.RootNode
                                            select p;
                            CostAccountSubject CostSubject = queryRoot.ElementAt(0);
                            string strNewId = CostSubject.Id;

                            string strSyscode = strNewId + strGuid + ".";
                            string strThree = strCode.Remove(0, 3);
                            string strsql = "insert into thd_CostAccountSubject (ID,CATEGORYNODETYPE,NAME,CODE,CREATEDATE,STATE,TLEVEL,PARENTNODEID,AUTHOR,THETREE,ORDERNO,CREATETIME,OWNERGUID,OWNERNAME,OWNERORGSYSCODE,SUBJECTSTATE,THEPROJECTGUID,THEPROJECTNAME,SYSCODE,IFSUBBALANCEFLAG)";
                            strsql += "values ('" + strGuid + "','" + 1 + "','" + strSubjectName + "','" + strCode + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'1','2','" + strNewId + "','" + strOperOrgInfoId + "','11','" + strThree + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strOperOrgInfoId + "','" + strPersonName + "','" + strOpgSysCode + "','1','" + projectInfo.Id + "','" + projectInfo.Name + "','" + strSyscode + "','" + IntFlag + "')";

                            int res = SaveSql(strsql);
                            if (res != 0)
                            {
                                Rows += 1;
                                hashtableSubject.Add(strGuid, strCode);
                                hashtableSystemCode.Add(strGuid, strSyscode);
                            }
                        }
                        else
                        {
                            string strGuid = GetGuid();//生成Id
                            string strSysCode = null;
                            string strId = null;
                            string str = strCode.Substring(0, strCode.Length - 2);

                            foreach (System.Collections.DictionaryEntry objDE in hashtableSubject)
                            {
                                if (objDE.Value.ToString().Equals(str))
                                {
                                    strId = objDE.Key.ToString();//获得ID
                                    break;
                                }
                                //没有找到相应的信息跳出循环
                            }
                            foreach (System.Collections.DictionaryEntry objSub in hashtableSystemCode)
                            {
                                if (objSub.Key.ToString().Equals(strId))
                                {
                                    strSysCode = objSub.Value.ToString();//获得ID

                                    break;
                                }
                                //没有找到相应的信息跳出循环
                            }

                            string strSyscode = strSysCode + strGuid + "."; //根节点的Id为“DC9580D40C4BE6D615E4BEC6134”
                            string strThree = strCode.Remove(0, 3);
                            string strLevel = null;
                            string state = null;
                            if (strCode.Length == 6)
                            {
                                strLevel = "3";
                                state = "1";
                            }
                            if (strCode.Length == 8)
                            {
                                strLevel = "4";
                                state = "1";
                            }
                            if (strCode.Length == 10)
                            {
                                strLevel = "5";
                                state = "1";
                            }
                            string strsql = "insert into thd_CostAccountSubject (ID,CATEGORYNODETYPE,NAME,CODE,CREATEDATE,STATE,TLEVEL,PARENTNODEID,AUTHOR,THETREE,ORDERNO,CREATETIME,OWNERGUID,OWNERNAME,OWNERORGSYSCODE,SUBJECTSTATE,THEPROJECTGUID,THEPROJECTNAME,SYSCODE,IFSUBBALANCEFLAG)";
                            strsql += "values ('" + strGuid + "','" + 1 + "','" + strSubjectName + "','" + strCode + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'" + state + "','" + strLevel + "','" + strId + "','" + strOperOrgInfoId + "','11','" + strThree + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strOperOrgInfoId + "','" + strPersonName + "','" + strOpgSysCode + "','1','" + projectInfo.Id + "','" + projectInfo.Name + "','" + strSyscode + "','" + IntFlag + "')";

                            int res = SaveSql(strsql);
                            if (res != 0)
                            {
                                Rows += 1;
                                hashtableSubject.Add(strGuid, strCode);
                                hashtableSystemCode.Add(strGuid, strSysCode);
                            }
                        }
                    }
                }
            }
            MessageBox.Show(Rows + "条信息保存成功！");
        }
        #endregion

        #region 成本项分类数据
        [TransManager]
        public void SaveCostCatagry(DataSet OleDsExcle, CurrentProjectInfo projectInfo)
        {

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            IList Costlist = Dao.ObjectQuery(typeof(CostItemCategory), objectQuery);

            int Rows = 0;
            Hashtable hashtableCostType = new Hashtable();//成本项分类
            Hashtable hashtableSysCost = new Hashtable();
            string strCostType = "select ID,CODE,SYSCODE from THD_CostItemCategory";
            DataTable dtCostType = SearchSql(strCostType);
            for (int i = 0; i < dtCostType.Rows.Count; i++)
            {
                string strID = dtCostType.Rows[i][0].ToString();
                string strCode = dtCostType.Rows[i][1].ToString();
                string strSysCode = dtCostType.Rows[i][2].ToString();
                hashtableCostType.Add(strID, strCode);
                hashtableSysCost.Add(strID, strSysCode);
            }
            if (OleDsExcle.Tables[0].Columns.Count != 0)
            {
                int Columns = OleDsExcle.Tables[0].Columns.Count;
                if (Columns < 4)
                {
                    MessageBox.Show("Excel格式不正确！");
                    return;
                }
                for (int i = 1; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行
                {
                    string strCode = OleDsExcle.Tables[0].Rows[i][1].ToString();//分类代码
                    string strName = OleDsExcle.Tables[0].Rows[i][2].ToString();//分类名称
                    string strDescript = OleDsExcle.Tables[0].Rows[i][3].ToString();//分类说明
                    string strCostTypeId = null;
                    string strSysCodeNo = null;
                    string strGuid = null;
                    int strTLevel = 0;
                    string strOrderNo = strCode.Substring(strCode.Length - 2, 2);
                    if (strCode.Equals(""))
                    {

                    }
                    else
                    {
                        if (strCode.Length == 2)
                        {
                            var queryRoot = from p in Costlist.OfType<CostItemCategory>()
                                            where p.CategoryNodeType == NodeType.RootNode
                                            select p;
                            CostItemCategory CostCategory = queryRoot.ElementAt(0);
                            strCostTypeId = CostCategory.Id;
                            foreach (System.Collections.DictionaryEntry objSys in hashtableSysCost)
                            {
                                if (objSys.Key.ToString().Equals(strCostTypeId))
                                {
                                    strSysCodeNo = objSys.Value.ToString();//获得ID
                                    break;
                                }
                                //没有找到相应的信息跳出循环
                            }

                            strGuid = GetGuid();
                            string strNewSysCode = strSysCodeNo + strGuid + ".";
                            string strSql = "insert into THD_CostItemCategory(ID,CategoryNodeType,Name,Code,CREATEDATE,STATE,TLEVEL,DESCRIBE,PARENTNODEID,THETREE,ORDERNO,CREATETIME,THEPROJECTGUID,THEPROJECTNAME,CATEGORYSTATE,SYSCODE)";
                            strSql += "values('" + strGuid + "','1','" + strName + "','" + strCode + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'1','2','" + strDescript + "','" + strCostTypeId + "','" + 11 + "','" + strOrderNo + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'" + projectInfo.Id + "','" + projectInfo.Name + "','1','" + strNewSysCode + "')";
                            int res = SaveSql(strSql);
                            if (res != 0)
                            {
                                Rows += 1;
                                hashtableCostType.Add(strGuid, strCode);
                                hashtableSysCost.Add(strGuid, strNewSysCode);
                            }

                        }
                        else
                        {
                            if (strCode.Length == 4)
                            {
                                strTLevel = 3;
                            }
                            if (strCode.Length == 6)
                            {
                                strTLevel = 4;
                            }
                            if (strCode.Length == 8)
                            {
                                strTLevel = 5;
                            }
                            if (strCode.Length == 10)
                            {
                                strTLevel = 6;
                            }
                            if (strCode.Length == 12)
                            {
                                strTLevel = 7;
                            }
                            if (strCode.Length == 14)
                            {
                                strTLevel = 8;
                            }
                            if (strCode.Length == 16)
                            {
                                strTLevel = 9;
                            }
                            if (strCode.Length == 18)
                            {
                                strTLevel = 10;
                            }
                            string strNewCode = strCode.Substring(0, strCode.Length - 2);
                            foreach (System.Collections.DictionaryEntry objDE in hashtableCostType)
                            {
                                if (objDE.Value.ToString().Equals(strNewCode))
                                {
                                    strCostTypeId = objDE.Key.ToString();//获得ID
                                    foreach (System.Collections.DictionaryEntry objSys in hashtableSysCost)
                                    {
                                        if (objSys.Key.ToString().Equals(strCostTypeId))
                                        {
                                            strSysCodeNo = objSys.Value.ToString();//获得ID
                                            break;
                                        }
                                        //没有找到相应的信息跳出循环
                                    }
                                    break;
                                }
                                //没有找到相应的信息跳出循环
                            }
                            strGuid = GetGuid();
                            string strNewSysCode = strSysCodeNo + strGuid + ".";
                            string strSql = "insert into THD_CostItemCategory(ID,CategoryNodeType,Name,Code,CREATEDATE,STATE,TLEVEL,DESCRIBE,PARENTNODEID,THETREE,ORDERNO,CREATETIME,THEPROJECTGUID,THEPROJECTNAME,CATEGORYSTATE,SYSCODE)";
                            strSql += "values('" + strGuid + "','1','" + strName + "','" + strCode + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'1','" + strTLevel + "','" + strDescript + "','" + strCostTypeId + "','" + 11 + "','" + strOrderNo + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'" + projectInfo.Id + "','" + projectInfo.Name + "','1','" + strNewSysCode + "')";
                            int res = SaveSql(strSql);
                            if (res != 0)
                            {
                                Rows += 1;
                                hashtableCostType.Add(strGuid, strCode);
                                hashtableSysCost.Add(strGuid, strNewSysCode);
                            }
                        }
                    }
                }
            }
            MessageBox.Show(Rows + "行信息保存成功！");
        }
        #endregion


        #region 成本项安装分类
        [TransManager]
        public void SaveCostInstall(DataSet OleDsExcle, CurrentProjectInfo projectInfo)
        {
            Hashtable hashtableCostType = new Hashtable();//成本项分类
            IList listCostType = CostTypeSrv.GetCostItemCategorys(typeof(CostItemCategory));
            if (listCostType.Count > 0)
            {
                foreach (CostItemCategory category in listCostType)
                {
                    hashtableCostType.Add(category, category.Code);
                }
            }
            Hashtable hashtableCost = new Hashtable();//成本项
            ObjectQuery oq = new ObjectQuery();
            IList listCost = CostItemSrv.ObjectQuery(typeof(CostItem), oq);
            if (listCost.Count > 0)
            {
                foreach (CostItem item in listCost)
                {
                    hashtableCost.Add(item, item.Code);
                }
            }
            Hashtable hashtableSubjectCost = new Hashtable();//资源耗用定额
            IList listSubjectCost = CostItemSrv.ObjectQuery(typeof(SubjectCostQuota), oq);
            if (listSubjectCost.Count > 0)
            {
                foreach (SubjectCostQuota subject in listSubjectCost)
                {
                    hashtableSubjectCost.Add(subject, subject.Code);
                }
            }
            //计量单位
            Hashtable hashtableUnit = new Hashtable();
            IList listUnit = CostItemSrv.ObjectQuery(typeof(StandardUnit), oq);
            if (listUnit.Count > 0)
            {
                foreach (StandardUnit unit in listUnit)
                {
                    hashtableUnit.Add(unit, unit.Name);
                }
            }
            Hashtable hashtableResource = new Hashtable();//资源

            Hashtable hashtableCostAccount = new Hashtable();//成本科目
            IList listCostAccountSubject = CostSubjectSrv.GetCostAccountSubjects(typeof(CostAccountSubject));
            if (listCostAccountSubject.Count > 0)
            {
                foreach (CostAccountSubject CostSubject in listCostAccountSubject)
                {
                    hashtableCostAccount.Add(CostSubject, CostSubject.Code);
                }
            }

            Hashtable hashtablecategry = new Hashtable();
            Dictionary<string, int> dicCateMaxCostItemCode = new Dictionary<string, int>();
            int Rows = 0;
            string CostName = "";
            decimal priQuan = 0;//价格所含数量
            decimal sumPrice = 0;
            CostItem cost = new CostItem();
            if (OleDsExcle.Tables[0].Columns.Count != 0)
            {
                int Columns = OleDsExcle.Tables[0].Columns.Count;
                if (Columns < 23)
                {
                    MessageBox.Show("Excel格式不正确！");
                    return;
                }
                for (int i = 0; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行
                {
                    string strCostType = OleDsExcle.Tables[0].Rows[i][0].ToString();//成本项分类代码
                    string strDDCost = OleDsExcle.Tables[0].Rows[i][1].ToString();//定额编码
                    string strCostNames = OleDsExcle.Tables[0].Rows[i][2].ToString();//成本项分类名称
                    string strCostExplain = OleDsExcle.Tables[0].Rows[i][3].ToString();//成本项说明
                    string strCostName = null;
                    if (strCostNames.Equals(""))
                    {
                        strCostName = CostName;
                    }
                    else
                    {
                        CostName = strCostNames;
                        strCostName = strCostNames;
                    }
                    decimal strPrc = 0;//单价
                    decimal strQuantity = 0;//单价所含数量
                    string strUnits = null;//计量单位
                    string strContentType = null;//内容类型
                    string strlevel = null;//适用级别
                    string strModel = null;//适用模式
                    string strFilter = null;//基数成本项分类过滤（第一位不是0补0）
                    string strFilterOne = null;
                    string strFilterTwo = null;
                    string strFilterThree = null;
                    decimal strPriceRate = 0;//计价费率（转换成小数）
                    string strHYName = null;//定额耗用名称
                    string strMaterialType = null;//资源类型
                    string strSubject = null;//成本核算科目
                    string strSubjectName = null;//成本核算科目名称
                    decimal strLoss = 0;//损耗
                    decimal strPrice = 0;//定额单价
                    decimal strProjectQuantity = 0;//定额工程量
                    decimal strProjectMoney = 0;//定额金额
                    string strProjectUnit = null;//工程量计量单位
                    string strPriceUnit = null;//价格计量单位
                    decimal strProporty = 0;//分摊比例

                    if (strDDCost.Equals(""))
                    {
                        strlevel = OleDsExcle.Tables[0].Rows[i][4].ToString();//适用级别
                        strModel = OleDsExcle.Tables[0].Rows[i][5].ToString();//适用模式
                        strContentType = OleDsExcle.Tables[0].Rows[i][6].ToString();//内容类型
                        strFilter = OleDsExcle.Tables[0].Rows[i][7].ToString();//基数成本项分类过滤（第一位不是0补0）
                        if (strFilter != "")
                        {
                            if (strFilter.Substring(0, 1) != "0")
                            {
                                strFilter = "0" + strFilter;
                            }
                        }
                        strFilterOne = OleDsExcle.Tables[0].Rows[i][8].ToString();
                        if (strFilterOne != "")
                        {
                            if (strFilterOne.Substring(0, 1) != "0")
                            {
                                strFilterOne = "0" + strFilterOne;
                            }
                        }
                        strFilterTwo = OleDsExcle.Tables[0].Rows[i][9].ToString();
                        if (strFilterTwo != "")
                        {
                            if (strFilterTwo.Substring(0, 1) != "0")
                            {
                                strFilterTwo = "0" + strFilterTwo;
                            }
                        }
                        strFilterThree = OleDsExcle.Tables[0].Rows[i][10].ToString();
                        if (strFilterThree != "")
                        {
                            if (strFilterThree.Substring(0, 1) != "0")
                            {
                                strFilterThree = "0" + strFilterThree;
                            }
                        }

                        string strRace = OleDsExcle.Tables[0].Rows[i][11].ToString().Trim();
                        strPriceRate = ClientUtil.ToDecimal(strRace.Substring(0, strRace.Length - 1)) / 100;//计价费率（转换成小数）
                        strHYName = OleDsExcle.Tables[0].Rows[i][12].ToString();//定额耗用名称
                        strMaterialType = OleDsExcle.Tables[0].Rows[i][13].ToString();//资源类型
                        strSubject = OleDsExcle.Tables[0].Rows[i][14].ToString();//成本核算科目
                        strSubjectName = OleDsExcle.Tables[0].Rows[i][15].ToString();//成本核算科目名称
                        strLoss = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][16].ToString());//损耗
                        strPrice = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][17].ToString());//定额单价
                        strProjectQuantity = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][18].ToString());//定额工程量
                        strProjectMoney = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][19].ToString());//定额金额
                        strProjectUnit = OleDsExcle.Tables[0].Rows[i][20].ToString();//工程量计量单位
                        strPriceUnit = OleDsExcle.Tables[0].Rows[i][21].ToString();//价格计量单位
                        strProporty = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][22].ToString());//分摊比例
                    }
                    else
                    {
                        strPrc = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][4].ToString());//单价
                        decimal strQuantity1 = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][5].ToString());//单价所含数量

                        if (strQuantity1 == 0)
                        {
                            strQuantity = priQuan;
                        }
                        else
                        {
                            priQuan = strQuantity1;
                            strQuantity = strQuantity1;
                        }


                        strUnits = OleDsExcle.Tables[0].Rows[i][6].ToString();//计量单位
                        strContentType = OleDsExcle.Tables[0].Rows[i][7].ToString();//内容类型
                        strlevel = OleDsExcle.Tables[0].Rows[i][8].ToString();//适用级别
                        strModel = OleDsExcle.Tables[0].Rows[i][9].ToString();//适用模式
                        strFilter = OleDsExcle.Tables[0].Rows[i][10].ToString();//基数成本项分类过滤（第一位不是0补0）
                        if (strFilter != "")
                        {
                            if (strFilter.Substring(0, 1) != "0")
                            {
                                strFilter = "0" + strFilter;
                            }
                        }
                        strFilterOne = OleDsExcle.Tables[0].Rows[i][11].ToString();
                        if (strFilterOne != "")
                        {
                            if (strFilterOne.Substring(0, 1) != "0")
                            {
                                strFilterOne = "0" + strFilterOne;
                            }
                        }

                        strFilterTwo = OleDsExcle.Tables[0].Rows[i][12].ToString();
                        if (strFilterTwo != "")
                        {
                            if (strFilterTwo.Substring(0, 1) != "0")
                            {
                                strFilterTwo = "0" + strFilterTwo;
                            }
                        }

                        strFilterThree = OleDsExcle.Tables[0].Rows[i][13].ToString();
                        if (strFilterThree != "")
                        {
                            if (strFilterThree.Substring(0, 1) != "0")
                            {
                                strFilterThree = "0" + strFilterThree;
                            }
                        }

                        string strRace = OleDsExcle.Tables[0].Rows[i][14].ToString().Trim();
                        strPriceRate = ClientUtil.ToDecimal(strRace.Substring(0, strRace.Length - 1)) / 100;//计价费率（转换成小数）
                        strHYName = OleDsExcle.Tables[0].Rows[i][15].ToString();//定额耗用名称
                        strMaterialType = OleDsExcle.Tables[0].Rows[i][16].ToString();//资源类型
                        strSubject = OleDsExcle.Tables[0].Rows[i][17].ToString();//成本核算科目
                        strSubjectName = OleDsExcle.Tables[0].Rows[i][18].ToString();//成本核算科目名称
                        strLoss = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][19].ToString());//损耗
                        strPrice = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][20].ToString());//定额单价
                        strProjectQuantity = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][21].ToString());//定额工程量
                        strProjectMoney = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][22].ToString());//定额金额
                        strProjectUnit = OleDsExcle.Tables[0].Rows[i][23].ToString();//工程量计量单位
                        strPriceUnit = OleDsExcle.Tables[0].Rows[i][24].ToString();//价格计量单位
                        strProporty = ClientUtil.ToDecimal(OleDsExcle.Tables[0].Rows[i][25].ToString());//分摊比例
                    }
                    StandardUnit UnitPrice = null;//价格计量单位
                    StandardUnit UnitQuantity = null;//工程量计量单位
                    StandardUnit UnitDW = null;//计量单位
                    Material mat = null;
                    CostItemCategory categry = null;
                    CostAccountSubject subject = null;
                    //价格计量单位
                    foreach (System.Collections.DictionaryEntry objUnitQuantity in hashtableUnit)
                    {
                        if (objUnitQuantity.Value.ToString().Equals(strPriceUnit))
                        {
                            UnitPrice = objUnitQuantity.Key as StandardUnit;
                            break;
                        }
                    }
                    //工程量计量单位
                    foreach (System.Collections.DictionaryEntry objUnitPrice in hashtableUnit)
                    {
                        if (objUnitPrice.Value.ToString().Equals(strProjectUnit))
                        {
                            UnitQuantity = objUnitPrice.Key as StandardUnit;
                            break;
                        }
                    }
                    if (strUnits != "")
                    {
                        foreach (System.Collections.DictionaryEntry objUnitDW in hashtableUnit)
                        {
                            if (objUnitDW.Value.ToString().Equals(strUnits))
                            {
                                UnitDW = objUnitDW.Key as StandardUnit;
                                break;
                            }
                        }

                    }
                    //资源
                    bool materialFlag = false;
                    foreach (System.Collections.DictionaryEntry objSub in hashtableResource)
                    {
                        if (objSub.Value.ToString().Equals(strMaterialType))
                        {
                            mat = objSub.Key as Material;
                            materialFlag = true;
                            break;
                        }
                    }
                    if (!materialFlag)
                    {
                        //查询数据库信息
                        ObjectQuery objectquery = new ObjectQuery();
                        objectquery.AddCriterion(Expression.Eq("Code", strMaterialType));
                        IList listmaterial = CostItemSrv.ObjectQuery(typeof(Material), objectquery);
                        if (listmaterial.Count > 0)
                        {
                            mat = listmaterial[0] as Material;
                            hashtableResource.Add(mat, mat.Code);
                        }
                    }
                    //成本项分类
                    foreach (System.Collections.DictionaryEntry objSub in hashtableCostType)
                    {
                        if (objSub.Value.ToString().Equals(strCostType))
                        {
                            categry = objSub.Key as CostItemCategory;
                            break;
                        }
                    }
                    if (categry != null)
                    {
                        if (strCostNames != "")
                        {
                            cost = new CostItem();
                            decimal sumMoney = 0;
                            int costItemCode = 0;
                            if (dicCateMaxCostItemCode.Count == 0)
                            {
                                costItemCode = CostItemSrv.GetMaxCostItemCodeByCate(categry.Id);
                                dicCateMaxCostItemCode.Add(categry.Id, costItemCode);
                            }
                            else if (dicCateMaxCostItemCode.ContainsKey(categry.Id))
                            {
                                costItemCode = dicCateMaxCostItemCode[categry.Id];
                                costItemCode += 1;
                                dicCateMaxCostItemCode[categry.Id] = costItemCode;
                            }
                            else
                            {
                                costItemCode = CostItemSrv.GetMaxCostItemCodeByCate(categry.Id);
                                dicCateMaxCostItemCode.Add(categry.Id, costItemCode);
                            }
                            cost.Code = categry.Code + "-" + costItemCode.ToString().PadLeft(5, '0');
                            cost.ManagementModeName = strModel;
                            cost.ApplyLevel = EnumUtil<CostItemApplyLeve>.FromDescription(strlevel);//适用级别

                            //成本项分类过滤1
                            if (!string.IsNullOrEmpty(strFilter))
                            {
                                CostItemCategory cateFilter = null;
                                foreach (System.Collections.DictionaryEntry objSub in hashtableCostType)
                                {
                                    if (objSub.Value.ToString().Equals(strFilter))
                                    {
                                        cateFilter = objSub.Key as CostItemCategory;
                                        break;
                                    }
                                }

                                cost.CateFilter1 = cateFilter;
                                cost.CateFilterName1 = cateFilter.Name;
                                cost.CateFilterSysCode1 = cateFilter.SysCode;
                            }

                            //成本核算科目过滤条件1
                            if (!string.IsNullOrEmpty(strFilterOne))
                            {
                                CostAccountSubject subFilter1 = null;
                                foreach (System.Collections.DictionaryEntry objSub in hashtableCostAccount)
                                {
                                    if (objSub.Value.ToString().Equals(strFilterOne))
                                    {
                                        subFilter1 = objSub.Key as CostAccountSubject;
                                        break;
                                    }
                                }

                                cost.SubjectCateFilter1 = subFilter1;
                                cost.SubjectCateFilterName1 = subFilter1.Name;
                                cost.SubjectCateFilterSyscode1 = subFilter1.SysCode;
                            }
                            //成本核算科目过滤条件2
                            if (!string.IsNullOrEmpty(strFilterTwo))
                            {
                                CostAccountSubject subFilter2 = null;
                                foreach (System.Collections.DictionaryEntry objSub in hashtableCostAccount)
                                {
                                    if (objSub.Value.ToString().Equals(strFilterTwo))
                                    {
                                        subFilter2 = objSub.Key as CostAccountSubject;
                                        break;
                                    }
                                }

                                cost.SubjectCateFilter2 = subFilter2;
                                cost.SubjectCateFilterName2 = subFilter2.Name;
                                cost.SubjectCateFilterSyscode2 = subFilter2.SysCode;
                            }
                            //成本核算科目过滤条件3
                            if (!string.IsNullOrEmpty(strFilterThree))
                            {
                                CostAccountSubject subFilter3 = null;
                                foreach (System.Collections.DictionaryEntry objSub in hashtableCostAccount)
                                {
                                    if (objSub.Value.ToString().Equals(strFilterThree))
                                    {
                                        subFilter3 = objSub.Key as CostAccountSubject;
                                        break;
                                    }
                                }

                                cost.SubjectCateFilter1 = subFilter3;
                                cost.SubjectCateFilterName1 = subFilter3.Name;
                                cost.SubjectCateFilterSyscode1 = subFilter3.SysCode;
                            }

                            //cost.ContentType = strContentType;
                            cost.CostDesc = strCostExplain;
                            cost.CreateTime = DateTime.Now;
                            cost.QuotaCode = strDDCost;
                            //cost.ListQuotas//成本核算科目定额

                            cost.Name = strCostName;
                            cost.Price = strPrice;//定额单价

                            cost.PriceUnitGUID = UnitPrice;
                            cost.PriceUnitName = strPriceUnit;
                            cost.PricingRate = strPriceRate;//计价费率

                            if (UnitDW != null)
                            {
                                cost.ProjectUnitName = UnitDW.Name;//计量单位
                                cost.ProjectUnitGUID = UnitDW;
                            }
                            else
                            {
                                if (UnitQuantity != null)
                                {
                                    cost.ProjectUnitName = UnitQuantity.Name;//计量单位
                                    cost.ProjectUnitGUID = UnitQuantity;
                                }
                            }
                            //cost.ProjectUnitGUID = UnitQuantity;
                            //cost.ProjectUnitName = strProjectUnit;
                            if (mat != null)
                            {
                                cost.ResourceTypeGUID = mat.Id;
                                cost.ResourceTypeName = mat.Name;
                            }
                            cost.SubContractPrice = strPrice;
                            sumPrice = 0;
                            cost.TheCostItemCategory = categry;//所属成本项分类
                            cost.TheCostItemCateSyscode = categry.SysCode;

                            cost.TheProjectGUID = projectInfo.Id;
                            cost.TheProjectName = projectInfo.Name;
                            //SaveOrUpdateCostItem
                            cost = CostItemSrv.SaveOrUpdateCostItem(cost);
                            Rows++;
                        }
                        //成本核算科目
                        foreach (System.Collections.DictionaryEntry objAccount in hashtableCostAccount)
                        {
                            if (objAccount.Value.ToString().Equals(strSubject))
                            {
                                subject = objAccount.Key as CostAccountSubject;
                                break;
                            }
                        }
                        SubjectCostQuota quota = new SubjectCostQuota();
                        quota.AssessmentRate = strProporty;
                        if (subject != null)
                        {
                            quota.CostAccountSubjectGUID = subject;
                            quota.CostAccountSubjectName = subject.Name;
                        }
                        quota.Name = subject.Name;//资源耗用名称
                        quota.CreateTime = DateTime.Now;
                        quota.PriceUnitGUID = UnitPrice;
                        quota.PriceUnitName = strPriceUnit;
                        if (UnitQuantity != null)
                        {
                            quota.ProjectAmountUnitName = UnitQuantity.Name;//工程量计量单位
                            quota.ProjectAmountUnitGUID = UnitQuantity;
                        }

                        if (strProjectUnit.Equals("项") && strProjectQuantity == 1)
                        {
                            if (strQuantity != 0)
                            {
                                strPrice = strPrice / strQuantity;
                            }
                        }
                        else
                        {
                            if (strProjectQuantity != 1)
                            {
                                if (strQuantity != 0)
                                {
                                    strProjectQuantity = strProjectQuantity / strQuantity;
                                }
                            }
                        }

                        quota.QuotaMoney = strPrice * strProjectQuantity;//工程量单价 = 定额数量*数量单价


                        quota.QuotaPrice = strPrice;
                        sumPrice += quota.QuotaMoney;
                        quota.QuotaProjectAmount = strProjectQuantity;//定额数量
                        if (mat != null)
                        {
                            quota.ResourceTypeGUID = mat.Id;
                            quota.ResourceTypeName = mat.Name;
                            //quota.ListResources =;//资源组
                        }
                        quota.TheCostItem = cost;
                        quota.TheProjectGUID = projectInfo.Id;
                        quota.TheProjectName = projectInfo.Name;
                        quota.Wastage = strLoss;
                        quota = CostItemSrv.SaveOrUpdateSubjectCostQuota(quota);
                        cost.Price = sumPrice;
                        cost = CostItemSrv.SaveOrUpdateCostItem(cost);
                        //保存资源组
                        ResourceGroup group = new ResourceGroup();
                        group.ResourceTypeGUID = mat.Id;
                        group.ResourceTypeCode = mat.Code;
                        group.ResourceTypeName = mat.Name;
                        group.ResourceTypeQuality = mat.Stuff;
                        group.ResourceTypeSpec = mat.Specification;
                        group.TheCostQuota = quota;
                        group.ResourceCateId = mat.Category.Id;
                        group.ResourceCateSyscode = mat.TheSyscode;
                        group = CostItemSrv.SaveOrUpdateGroup(group);


                    }
                    else
                    {
                        MessageBox.Show("未找到成本项分类" + strCostType);
                    }
                }
                MessageBox.Show(Rows + "条信息保存成功！");
            }
        }


        #endregion

        #region 会计科目
        [TransManager]
        public void SaveFiacctitle(DataSet OleDsExcle, CurrentProjectInfo projectInfo, string strOperOrgInfoId)
        {
            int Rows = 0;
            Hashtable hashtableAccountCode = new Hashtable();
            Hashtable hashtableAccountSysCode = new Hashtable();
            string strAccountType = "select ACCTITLEID,ACCCODE,ACCSYSCODE from THD_FIACCTITLE";
            DataTable dtAccountType = SearchSql(strAccountType);
            for (int i = 0; i < dtAccountType.Rows.Count; i++)
            {
                string strID = dtAccountType.Rows[i][0].ToString();
                string strCode = dtAccountType.Rows[i][1].ToString();
                string strSysCode = dtAccountType.Rows[i][2].ToString();
                hashtableAccountCode.Add(strID, strCode);
                hashtableAccountSysCode.Add(strCode, strSysCode);
            }
            Hashtable hashtableCostType = new Hashtable();//成本项分类
            string strCostType = "select ID,CODE from THD_CostItemCategory";
            DataTable dtCostType = SearchSql(strCostType);
            for (int i = 0; i < dtCostType.Rows.Count; i++)
            {
                string strID = dtCostType.Rows[i][0].ToString();
                string strCode = dtCostType.Rows[i][1].ToString();
                hashtableCostType.Add(strID, strCode);
            }
            if (OleDsExcle.Tables[0].Columns.Count != 0)
            {
                int Columns = OleDsExcle.Tables[0].Columns.Count;
                if (Columns < 6)
                {
                    MessageBox.Show("Excel格式不正确！");
                    return;
                }
                string Code = null;
                string Name = null;
                string strSysCodeNo = null;//获得的父类的层次码
                string strGuid = null;
                string strNewSysCode = null;//组合后最新的层次码
                string strOrderNo = null;//获取后两位作为顺序码
                string strParentId = null;
                string Sql = null;
                string strSql = null;
                int IntOrderNo = 0;
                int IntLevel = 0;//树深度
                int strCategoryNodeType = 0;//根节点，枝节点，叶节点
                for (int i = 4; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行
                {
                    string strCode = OleDsExcle.Tables[0].Rows[i][0].ToString().Trim();//科目编码
                    if (strCode != "")
                    {
                        Code = strCode;
                    }
                    string strName = OleDsExcle.Tables[0].Rows[i][1].ToString().Trim();//科目名称
                    if (strName != "")
                    {
                        Name = strName;
                    }
                    string strAccountCode = OleDsExcle.Tables[0].Rows[i][4].ToString().Trim();
                    string strAccountName = OleDsExcle.Tables[0].Rows[i][5].ToString().Trim();
                    if (strCode != "")
                    {
                        if (strCode != "")
                        {
                            strOrderNo = strCode.Substring(strCode.Length - 2);//获取后两位作为顺序码
                            IntOrderNo = Convert.ToInt32(strOrderNo);
                            strGuid = GetGuid();
                            string strJQ = strCode;
                            strSysCodeNo = null;
                            if (strCode.Length != 4)
                            {
                                strJQ = strCode.Substring(0, strCode.Length - 2);
                            }
                            foreach (System.Collections.DictionaryEntry objSys in hashtableAccountSysCode)
                            {
                                if (objSys.Key.ToString().Equals(strJQ))
                                {
                                    strSysCodeNo = objSys.Value.ToString();
                                    break;
                                }
                            }
                            strNewSysCode = strSysCodeNo + strGuid + ".";//新的层次码

                            if (strCode.Length == 4)
                            {
                                IntLevel = 0;
                                strCategoryNodeType = 0;
                            }
                            if (strCode.Length == 6)
                            {
                                IntLevel = 1;
                                strCategoryNodeType = 1;
                            }
                            if (strCode.Length == 8)
                            {
                                IntLevel = 2;
                                strCategoryNodeType = 1;
                            }
                            if (strCode.Length == 10)
                            {
                                IntLevel = 3;
                                strCategoryNodeType = 1;
                            }
                            if (strCode.Length == 12)
                            {
                                IntLevel = 4;
                                strCategoryNodeType = 2;
                            }
                            if (strCode.Length == 14)
                            {
                                IntLevel = 5;
                                strCategoryNodeType = 2;
                            }
                            if (strCode.Length == 16)
                            {
                                IntLevel = 6;
                                strCategoryNodeType = 2;
                            }
                            if (strCode.Length == 4)
                            {
                                Sql = "insert into THD_FIACCTITLE (ACCTITLEID,VERSION,PERID,ACCNODETYPE,ACCCREATEDATE,ACCNODELEVEL,ACCNAME,CODE,ACCSTATE,ACCSYSCODE,CATTREEID,ORDERNO)";
                                Sql += "values('" + strGuid + "','0','" + strOperOrgInfoId + "','" + strCategoryNodeType + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'" + IntLevel + "','" + Name + "','" + Code + "','1','" + strNewSysCode + "','11','" + IntOrderNo + "')";
                            }
                            else
                            {
                                //找到父节点
                                string strSubstring = strCode.Substring(0, strCode.Length - 2);
                                foreach (System.Collections.DictionaryEntry objSys in hashtableAccountCode)
                                {
                                    if (objSys.Key.ToString().Equals(strSubstring))
                                    {
                                        strParentId = objSys.Value.ToString();
                                        break;
                                    }
                                }
                                Sql = "insert into THD_FIACCTITLE (ACCTITLEID,VERSION,PERID,ACCNODETYPE,ACCCREATEDATE,ACCNODELEVEL,ACCNAME,CODE,ACCSTATE,ACCSYSCODE,CATTREEID,ORDERNO,PARENTNODEID)";
                                Sql += "values('" + strGuid + "','0','" + strOperOrgInfoId + "','" + strCategoryNodeType + "',to_date('" + strDate.ToShortDateString() + "','yyyy-mm-dd hh24:mi:ss'),'" + IntLevel + "','" + Name + "','" + Code + "','1','" + strNewSysCode + "','11','" + IntOrderNo + "','" + strParentId + "')";
                            }
                            int res = SaveSql(Sql);
                            if (res != 0)
                            {
                                Rows += 1;
                                hashtableAccountCode.Add(strGuid, strCode);
                                hashtableAccountSysCode.Add(strCode, strNewSysCode);
                            }
                            else
                            {
                                return;
                            }
                        }

                    }
                    if (strAccountCode != "" && strAccountName != "")
                    {
                        string strUpdateId = null;
                        foreach (System.Collections.DictionaryEntry objSys in hashtableCostType)
                        {
                            if (objSys.Value.ToString().Equals(strAccountCode))
                            {
                                strUpdateId = objSys.Key.ToString();
                                break;
                            }
                        }
                        //更新成本核算科目信息
                        strSql = "update thd_CostAccountSubject set accountingsubjectguid = '" + strGuid + "' ,accountingsubjectname = '" + Name + "' where Code = '" + strAccountCode + "'";
                        int res = SaveSql(strSql);
                    }
                }
            }
            MessageBox.Show(Rows + "条信息保存成功！");
        }
        #endregion

        #region 工程WBS
        [TransManager]
        public void SaveGWBS(DataSet OleDsExcle, CurrentProjectInfo projectInfo, string strOperOrgInfoId)
        {
            //读取数据表中的信息，将信息保存到hashtable中
            Hashtable hashtableProject = new Hashtable();
            string strSearchProject = "select ID,Code from THD_ProjectTaskTypeTree";
            DataTable strProjectdt = SearchSql(strSearchProject);
            for (int k = 0; k < strProjectdt.Rows.Count; k++)
            {
                string strProjectId = strProjectdt.Rows[k][0].ToString();
                string strProjectName = strProjectdt.Rows[k][1].ToString();
                hashtableProject.Add(strProjectId, strProjectName);
            }

            Hashtable hashtableSysCode = new Hashtable();
            string strSearchCode = "select ID,SysCode from THD_ProjectTaskTypeTree";
            DataTable strCodedt = SearchSql(strSearchCode);
            for (int k = 0; k < strCodedt.Rows.Count; k++)
            {
                string strSysId = strProjectdt.Rows[k][0].ToString();
                string strSysCode = strProjectdt.Rows[k][1].ToString();
                hashtableSysCode.Add(strSysId, strSysCode);
            }
            int Rows = 0;
            if (OleDsExcle.Tables[0].Rows.Count != 0)
            {
                int Columns = OleDsExcle.Tables[0].Columns.Count;
                if (Columns < 8)
                {
                    MessageBox.Show("Excel格式不正确！");
                    return;
                }
                for (int i = 0; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行，第一行的信息不读取
                {
                    string strMasageOne = OleDsExcle.Tables[0].Rows[i][2].ToString();//获得工程任务类型编码
                    string strMasageTew = OleDsExcle.Tables[0].Rows[i][3].ToString();//获得工程任务类型名称
                    string strmasageThree = OleDsExcle.Tables[0].Rows[i][4].ToString();//获得遵循标准
                    string strMasageSix = OleDsExcle.Tables[0].Rows[i][7].ToString();//获得检查要求
                    if (strMasageOne.Equals(""))
                    { }
                    else
                    {
                        if (strMasageOne.Length != 13)
                        {
                            MessageBox.Show((i + 2) + "行有错误，工程任务类型编码长度应为13");
                            return;
                        }
                        if (strMasageTew.Equals(""))
                        {
                            MessageBox.Show((i + 2) + "行有错误，工程任务类型名称不能为空");
                            return;
                        }
                        if (strmasageThree.Equals(""))
                        {
                            MessageBox.Show((i + 2) + "行有错误，遵循标准不能为空");
                            return;
                        }
                        if (strMasageSix.Equals("") || strMasageSix.Length != 5)
                        {
                            MessageBox.Show((i + 2) + "行有错误，检查要求不能为空并且长度等于5");
                            return;
                        }
                    }
                }
            }
            if (OleDsExcle.Tables[0].Rows.Count != 0)
            {
                string strSysSql = "select SysCode,ID from THD_ProjectTaskTypeTree where Length(SysCode) = 28";
                DataTable Sysdt = SearchSql(strSysSql);

                for (int i = 0; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行，第一行的信息不读取
                {
                    string strCode = OleDsExcle.Tables[0].Rows[i][2].ToString();//读取临时表第i行第j列的数据
                    if (strCode != "")
                    {
                        //第二列有信息，无信息不处理
                        string strGuid = GetGuid();//生成Id
                        string strName = OleDsExcle.Tables[0].Rows[i][3].ToString().Trim();
                        string strTypeStandard = OleDsExcle.Tables[0].Rows[i][4].ToString().Trim();
                        string strCheckRequire = OleDsExcle.Tables[0].Rows[i][7].ToString().Trim();
                        string typeSummary = OleDsExcle.Tables[0].Rows[i][5].ToString().Trim();
                        string summary = OleDsExcle.Tables[0].Rows[i][6].ToString().Trim();
                        string strId = null;
                        string strSysCode = null;

                        string strTow = strCode.Remove(0, 2);//去掉前两个字符
                        if (strTow.Equals("00000000000"))
                        {
                            string strParentID = Sysdt.Rows[0][1].ToString();
                            strSysCode = Sysdt.Rows[0][0].ToString();
                            strSysCode += strGuid + '.';
                            int strOrderNo = ClientUtil.ToInt(strCode.Substring(0, 2));
                            //截取字符串，截去前两位后面几位都是零，第一级
                            string strsql = "insert into THD_ProjectTaskTypeTree (ID,CategoryNodeType,Name,Code,CreateDate,SysCode,State,TLevel,Author,ParentNodeID,TheTree,OrderNo,TypeLevel,TypeStandard,CheckRequire,TheProjectGUID,TheProjectName,TypeSummary,Summary)";
                            strsql += "values('" + strGuid + "','1','" + strName + "','" + strCode + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSysCode + "','1','1','" + strOperOrgInfoId + "','" + strParentID + "','11','" + strOrderNo + "','1','" + strTypeStandard + "','" + strCheckRequire + "','','','"+typeSummary+"','"+summary+"')";
                            int res = SaveSql(strsql);
                            if (res != 0)
                            {
                                Rows += 1;
                                hashtableProject.Add(strGuid, strCode);
                                hashtableSysCode.Add(strGuid, strSysCode);
                            }
                        }
                        else
                        {
                            string strFour = strCode.Remove(0, 4);//去点前4个字符
                            if (strFour.Equals("000000000"))
                            {
                                //截去前四位，后几位都是零，第二级
                                string strNew = strCode.Substring(0, 2) + "00000000000";
                                //从hashtable中查找编号为strnew的信息
                                foreach (System.Collections.DictionaryEntry objDE in hashtableProject)
                                {
                                    if (objDE.Value.ToString().Equals(strNew))
                                    {
                                        strId = objDE.Key.ToString();//获得ID
                                        foreach (System.Collections.DictionaryEntry objCode in hashtableSysCode)
                                        {
                                            if (objCode.Key.ToString().Equals(strId))
                                            {
                                                strSysCode = objCode.Value.ToString();
                                            }
                                        }
                                        break;
                                    }
                                    //没有找到相应的信息跳出循环
                                }

                                if (string.IsNullOrEmpty(strSysCode))
                                {
                                    strSysCode = Sysdt.Rows[0][0].ToString() + strGuid + ".";
                                    strId = Sysdt.Rows[0][1].ToString();
                                }
                                else
                                {
                                    strSysCode += strGuid + '.';
                                }
                                int strOrderNo = ClientUtil.ToInt(strCode.Substring(2, 2));
                                string strsql = "insert into THD_ProjectTaskTypeTree (ID,CategoryNodeType,Name,Code,CreateDate,SysCode,State,ParentNodeID,TLevel,Author,TheTree,OrderNo,TypeLevel,TypeStandard,CheckRequire,TheProjectGUID,TheProjectName,TypeSummary,Summary)";
                                strsql += "values('" + strGuid + "','1','" + strName + "','" + strCode + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSysCode + "','1','" + strId + "','2','" + strOperOrgInfoId + "','11','" + strOrderNo + "','2','" + strTypeStandard + "','" + strCheckRequire + "','','','" + typeSummary + "','" + summary + "')";
                                int res = SaveSql(strsql);
                                if (res != 0)
                                {
                                    Rows += 1;
                                    hashtableProject.Add(strGuid, strCode);
                                    hashtableSysCode.Add(strGuid, strSysCode);
                                }

                            }
                            else
                            {
                                string strSix = strCode.Remove(0, 6);//去掉前六位
                                if (strSix.Equals("0000000"))
                                {
                                    //截去前六位，后几位都是零，第三级
                                    string strNew = strCode.Substring(0, 4) + "000000000";
                                    //从hashtable中查找编号为strnew的信息
                                    foreach (System.Collections.DictionaryEntry objDE in hashtableProject)
                                    {
                                        if (objDE.Value.ToString().Equals(strNew))
                                        {
                                            strId = objDE.Key.ToString();//获得ID
                                            foreach (System.Collections.DictionaryEntry objCode in hashtableSysCode)
                                            {
                                                if (objCode.Key.ToString().Equals(strId))
                                                {
                                                    strSysCode = objCode.Value.ToString();
                                                }
                                            }
                                            break;
                                        }
                                        //没有找到相应的信息跳出循环
                                    }
                                    if (string.IsNullOrEmpty(strSysCode))
                                    {
                                        strSysCode = Sysdt.Rows[0][0].ToString() + strGuid + ".";
                                        strId = Sysdt.Rows[0][1].ToString();
                                    }
                                    else
                                    {
                                        strSysCode += strGuid + '.';
                                    }
                                    int strOrderNo = ClientUtil.ToInt(strCode.Substring(4, 2));
                                    string strsql = "insert into THD_ProjectTaskTypeTree (ID,CategoryNodeType,Name,Code,CreateDate,SysCode,State,ParentNodeID,TLevel,Author,TheTree,OrderNo,TypeLevel,TypeStandard,CheckRequire,TheProjectGUID,TheProjectName,TypeSummary,Summary)";
                                    strsql += "values('" + strGuid + "','1','" + strName + "','" + strCode + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSysCode + "','1','" + strId + "','3','" + strOperOrgInfoId + "','11','" + strOrderNo + "','3','" + strTypeStandard + "','" + strCheckRequire + "','','','" + typeSummary + "','" + summary + "')";
                                    int res = SaveSql(strsql);
                                    if (res != 0)
                                    {
                                        Rows += 1;
                                        hashtableProject.Add(strGuid, strCode);
                                        hashtableSysCode.Add(strGuid, strSysCode);
                                    }
                                }
                                else
                                {
                                    string strEight = strCode.Remove(0, 8);
                                    if (strEight.Equals("00000"))
                                    {
                                        string strNew = strCode.Substring(0, 6) + "0000000";
                                        //从hashtable中查找编号为strnew的信息
                                        foreach (System.Collections.DictionaryEntry objDE in hashtableProject)
                                        {
                                            if (objDE.Value.ToString().Equals(strNew))
                                            {
                                                strId = objDE.Key.ToString();//获得ID
                                                foreach (System.Collections.DictionaryEntry objCode in hashtableSysCode)
                                                {
                                                    if (objCode.Key.ToString().Equals(strId))
                                                    {
                                                        strSysCode = objCode.Value.ToString();
                                                    }
                                                }
                                                break;
                                            }
                                            //没有找到相应的信息跳出循环
                                        }
                                        if (string.IsNullOrEmpty(strSysCode))
                                        {
                                            strSysCode = Sysdt.Rows[0][0].ToString() + strGuid + ".";
                                            strId = Sysdt.Rows[0][1].ToString();
                                        }
                                        else
                                        {
                                            strSysCode += strGuid + '.';
                                        }
                                        int strOrderNo = ClientUtil.ToInt(strCode.Substring(6, 2));
                                        string strsql = "insert into THD_ProjectTaskTypeTree (ID,CategoryNodeType,Name,Code,CreateDate,SysCode,State,ParentNodeID,TLevel,Author,TheTree,OrderNo,TypeLevel,TypeStandard,CheckRequire,TheProjectGUID,TheProjectName,TypeSummary,Summary)";
                                        strsql += "values('" + strGuid + "','1','" + strName + "','" + strCode + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSysCode + "','1','" + strId + "','4','" + strOperOrgInfoId + "','11','" + strOrderNo + "','4','" + strTypeStandard + "','" + strCheckRequire + "','','','" + typeSummary + "','" + summary + "')";
                                        int res = SaveSql(strsql);
                                        if (res != 0)
                                        {
                                            Rows += 1;
                                            hashtableProject.Add(strGuid, strCode);
                                            hashtableSysCode.Add(strGuid, strSysCode);
                                        }
                                    }
                                    else
                                    {
                                        string strTen = strCode.Remove(0,10);
                                        if (strTen.Equals("000"))
                                        {
                                            string strNew = strCode.Substring(0, 8) + "00000";
                                            //从hashtable中查找编号为strnew的信息
                                            foreach (System.Collections.DictionaryEntry objDE in hashtableProject)
                                            {
                                                if (objDE.Value.ToString().Equals(strNew))
                                                {
                                                    strId = objDE.Key.ToString();//获得ID
                                                    foreach (System.Collections.DictionaryEntry objCode in hashtableSysCode)
                                                    {
                                                        if (objCode.Key.ToString().Equals(strId))
                                                        {
                                                            strSysCode = objCode.Value.ToString();
                                                        }
                                                    }
                                                    break;
                                                }
                                                //没有找到相应的信息跳出循环
                                            }
                                            if (string.IsNullOrEmpty(strSysCode))
                                            {
                                                strSysCode = Sysdt.Rows[0][0].ToString() + strGuid + ".";
                                                strId = Sysdt.Rows[0][1].ToString();
                                            }
                                            else
                                            {
                                                strSysCode += strGuid + '.';
                                            }
                                            int strOrderNo = ClientUtil.ToInt(strCode.Substring(8, 2));
                                            string strsql = "insert into THD_ProjectTaskTypeTree (ID,CategoryNodeType,Name,Code,CreateDate,SysCode,State,ParentNodeID,TLevel,Author,TheTree,OrderNo,TypeLevel,TypeStandard,CheckRequire,TheProjectGUID,TheProjectName,TypeSummary,Summary)";
                                            strsql += "values('" + strGuid + "','1','" + strName + "','" + strCode + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSysCode + "','1','" + strId + "','5','" + strOperOrgInfoId + "','11','" + strOrderNo + "','5','" + strTypeStandard + "','" + strCheckRequire + "','','','" + typeSummary + "','" + summary + "')";
                                            int res = SaveSql(strsql);
                                            if (res != 0)
                                            {
                                                Rows += 1;
                                                hashtableProject.Add(strGuid, strCode);
                                                hashtableSysCode.Add(strGuid, strSysCode);
                                            }
                                        }
                                        else
                                        {
                                            //剩下的位第四级
                                            string strNew = strCode.Substring(0, 10) + "000";
                                            //从hashtable中查找编号为strnew的信息
                                            foreach (System.Collections.DictionaryEntry objDE in hashtableProject)
                                            {
                                                if (objDE.Value.ToString().Equals(strNew))
                                                {
                                                    strId = objDE.Key.ToString();//获得ID
                                                    foreach (System.Collections.DictionaryEntry objCode in hashtableSysCode)
                                                    {
                                                        if (objCode.Key.ToString().Equals(strId))
                                                        {
                                                            strSysCode = objCode.Value.ToString();
                                                        }
                                                    }
                                                    break;
                                                }
                                                //没有找到相应的信息跳出循环
                                            }
                                            if (string.IsNullOrEmpty(strSysCode))
                                            {
                                                strSysCode = Sysdt.Rows[0][0].ToString() + strGuid + ".";
                                                strId = Sysdt.Rows[0][1].ToString();
                                            }
                                            else
                                            {
                                                strSysCode += strGuid + '.';
                                            }
                                            int strOrderNo = ClientUtil.ToInt(strCode.Substring(8, 3));
                                            string strsql = "insert into THD_ProjectTaskTypeTree (ID,CategoryNodeType,Name,Code,CreateDate,SysCode,State,ParentNodeID,TLevel,Author,TheTree,OrderNo,TypeLevel,TypeStandard,CheckRequire,TheProjectGUID,TheProjectName,TypeSummary,Summary)";
                                            strsql += "values('" + strGuid + "','2','" + strName + "','" + strCode + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSysCode + "','1','" + strId + "','6','" + strOperOrgInfoId + "','11','" + strOrderNo + "','6','" + strTypeStandard + "','" + strCheckRequire + "','','','" + typeSummary + "','" + summary + "')";
                                            int res = SaveSql(strsql);
                                            if (res != 0)
                                            {
                                                Rows += 1;
                                                hashtableProject.Add(strGuid, strCode);
                                                hashtableSysCode.Add(strGuid, strSysCode);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            MessageBox.Show(Rows + "条信息保存成功！");
        }
        #endregion

        #region 基础数据
        [TransManager]
        public void SaveBasic(DataSet OleDsExcle, CurrentProjectInfo projectInfo, string strOperOrgInfoId)
        {
            Hashtable hashtableName = new Hashtable();
            string strSearchData = "select * from THD_BasicDataOptr";
            DataTable strDatadt = SearchSql(strSearchData);
            for (int k = 0; k < strDatadt.Rows.Count; k++)
            {
                string strDataId = strDatadt.Rows[k][0].ToString();
                string strDataName = strDatadt.Rows[k][3].ToString();
                hashtableName.Add(strDataId, strDataName);
            }
            int R = 0;
            if (OleDsExcle.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行
                {
                    int flag = 1;//1父信息，2子信息，0重复信息
                    string strId = null;
                    string strOwn = OleDsExcle.Tables[0].Rows[i][0].ToString();//读取临时表归属信息
                    string strCode = OleDsExcle.Tables[0].Rows[i][1].ToString();//读取临时表编号
                    string strName = OleDsExcle.Tables[0].Rows[i][2].ToString();//读取临时表名称
                    if (strCode != "" && strName != "" && strOwn != "")
                    {
                        foreach (System.Collections.DictionaryEntry objDE in hashtableName)
                        {
                            if (objDE.Value.ToString().Equals(strOwn))
                            {
                                //哈希表中存在读取的归属信息,
                                strId = objDE.Key.ToString();
                                flag = 2;
                                break;
                            }
                        }
                        foreach (System.Collections.DictionaryEntry objdt in hashtableName)
                        {
                            if (objdt.Value.ToString().Equals(strName))
                            {
                                flag = 0;
                                break;
                            }
                        }
                        if (flag == 1)
                        {
                            //保存父信息
                            string strGuid = GetGuid();//生成Id
                            string strsql = "insert into THD_BasicDataOptr(Id,BasicName,Descript,State)";
                            strsql += "values ('" + strGuid + "','" + strOwn + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','-1 ')";

                            int res = SaveSql(strsql);
                            if (res != 0)
                            {
                                R += 1;
                                hashtableName.Add(strGuid, strOwn);
                            }

                            if (strId == "" || strId == null)
                            {
                                foreach (System.Collections.DictionaryEntry objDE1 in hashtableName)
                                {
                                    if (objDE1.Value.ToString().Equals(strOwn))
                                    {
                                        //哈希表中存在读取的归属信息,
                                        strId = objDE1.Key.ToString();//获得父Id
                                        flag = 2;
                                        break;
                                    }
                                }
                                foreach (System.Collections.DictionaryEntry objdt1 in hashtableName)
                                {
                                    if (objdt1.Value.ToString().Equals(strName))
                                    {
                                        flag = 0;
                                        break;
                                    }
                                }
                            }
                        }
                        if (flag == 2)
                        {
                            string strGuid = GetGuid();//生成Id
                            string strsql = "insert into THD_BasicDataOptr(Id,ParentId,BasicCode,BasicName,Descript,STATE)";
                            strsql += "values ('" + strGuid + "','" + strId + "','" + strCode + "','" + strName + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','0 ')";

                            int res = SaveSql(strsql);
                            if (res != 0)
                            {
                                R += 1;
                                hashtableName.Add(strGuid, strName);
                            }
                        }
                    }
                }
            }
            MessageBox.Show(R + "行信息保存成功！");
        }
        #endregion

        #region 物资分类
        [TransManager]
        public void SaveMaterial(DataSet OleDsExcle, CurrentProjectInfo projectInfo, string strOperOrgInfoId, string strOperOrgInfoName, string OperOrgInfo, string strOpgSysCode)
        {

            //打开数据库连接。返回command
            //IDbConnection conn = model.ExcelImportSrv.OpenConn();

            Hashtable hashtableUnit = new Hashtable();
            string strSearchUint = "select STANDUNITID,STANDUNITNAME from RESSTANDUNIT";
            DataTable strUnitdt = SearchSql(strSearchUint);
            for (int k = 0; k < strUnitdt.Rows.Count; k++)
            {
                string strUnitId = strUnitdt.Rows[k][0].ToString();
                string strUnitName = strUnitdt.Rows[k][1].ToString();
                hashtableUnit.Add(strUnitId, strUnitName);
            }

            Hashtable hashtableMaterial = new Hashtable();
            Hashtable hashtableMaterialType = new Hashtable();
            string strSearchMaterial = "select ID,CODE,SYSCODE from RESMATERIALCATEGORY";
            DataTable strdtMaterial = SearchSql(strSearchMaterial);
            for (int t = 0; t < strdtMaterial.Rows.Count; t++)
            {
                string strUnitId = strdtMaterial.Rows[t][0].ToString();
                string strUnitCode = strdtMaterial.Rows[t][1].ToString();
                string strUnitName = strdtMaterial.Rows[t][2].ToString();
                hashtableMaterialType.Add(strUnitId, strUnitName);
                hashtableMaterial.Add(strUnitId, strUnitCode);
            }

            int Rows = 0;
            if (OleDsExcle.Tables[0].Rows.Count != 0)
            {

                for (int i = 1; i < OleDsExcle.Tables[0].Rows.Count; i++)//循环读取临时表的行
                {
                    string strCode = OleDsExcle.Tables[0].Rows[i][0].ToString();//读取临时表第i行第j列的数据
                    string strTreeCode = OleDsExcle.Tables[0].Rows[i][2].ToString();
                    string strName2 = OleDsExcle.Tables[0].Rows[i][1].ToString();
                    string strGuid = GetGuid();//生成Id
                    if (strCode.Length == 4)
                    {
                        //长度为4，RESMATERIALCATEGORY
                        //string strGuid = GetGuid();//生成Id
                        string strId1 = "1.";
                        string strSYSTEMCODE = strId1 + strGuid + ".";
                        string strsql = "insert into RESMATERIALCATEGORY (ID,NAME,NODETYPE,CODE,CREATEDATE,SYSCODE,STATE,numLEVEL,PARENTNODEID,THETREEID,MATCATATTRUBUTE,PraStateControlRuleID,ManStateControlRuleID,PERID,OrderNo,MatCatKind,OperationOrg,OpgSysCode,ABBREVIATION)";
                        strsql += "values ('" + strGuid + "','" + strName2 + "','1','" + strCode + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSYSTEMCODE + "','1','3','29BioV9QP5T9tJmw1VKARN','7','0','2','14','" + strOperOrgInfoName + "','1','0','" + OperOrgInfo + "','" + strOpgSysCode + "','" + strName2 + "')";
                        int res = SaveSql(strsql);
                        if (res != 0)
                        {
                            Rows += 1;
                            hashtableMaterial.Add(strGuid, OleDsExcle.Tables[0].Rows[i][0].ToString());
                            hashtableMaterialType.Add(strGuid, strSYSTEMCODE);
                        }
                    }
                    string strId = null;
                    string strParentId = null;
                    if (strCode.Length == 6)
                    {
                        //长度为6，RESMATERIALCATEGORY
                        string strCodeThree = OleDsExcle.Tables[0].Rows[i][2].ToString();
                        if (strCodeThree == "")
                        {
                            string strfour = strCode.Substring(0, 4);//截取前四位
                            foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                            {
                                if (objDE.Value.ToString().Equals(strfour))
                                {
                                    strId = objDE.Key.ToString();
                                    foreach (System.Collections.DictionaryEntry objType in hashtableMaterialType)
                                    {
                                        if (objType.Key.ToString().Equals(strId))
                                        {
                                            strParentId = objType.Value.ToString();
                                        }
                                    }
                                    break;
                                }
                            }
                            //string strGuid = GetGuid();//生成Id
                            string strSYSTEMCODE = strParentId + strGuid + ".";
                            string strsql = "insert into RESMATERIALCATEGORY (ID,NAME,NODETYPE,CODE,CREATEDATE,SYSCODE,STATE,numLEVEL,PARENTNODEID,THETREEID,MATCATATTRUBUTE,PraStateControlRuleID,ManStateControlRuleID,PERID,OrderNo,MatCatKind,OperationOrg,OpgSysCode,ABBREVIATION)";
                            strsql += "values ('" + strGuid + "','" + strName2 + "','1','" + strCode + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSYSTEMCODE + "','1','4','" + strId + "','7','0','2','14','" + strOperOrgInfoName + "','1','0','" + OperOrgInfo + "','" + strOpgSysCode + "','" + strName2 + "')";
                            int res = SaveSql(strsql);
                            if (res != 0)
                            {
                                Rows += 1;
                                hashtableMaterial.Add(strGuid, OleDsExcle.Tables[0].Rows[i][0].ToString());
                                hashtableMaterialType.Add(strGuid, strSYSTEMCODE);
                            }
                        }
                        else
                        {
                            //编号不为空
                            string strUnit = OleDsExcle.Tables[0].Rows[i][8].ToString();
                            if (strUnit != "")
                            {//excel表中有计量单位信息，无计量单位的信息不保存
                                string strMaterialCategoryId = null;
                                foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                                {
                                    if (objDE.Value.ToString().Equals(strCode))
                                    {
                                        strMaterialCategoryId = objDE.Key.ToString();
                                        break;
                                    }
                                }
                                //string strGuid = GetGuid();//生成Id
                                //有计量单位信息
                                string strUnitId = null;
                                foreach (System.Collections.DictionaryEntry value in hashtableUnit)
                                {
                                    if (value.Value.ToString().Equals(strUnit))
                                    {
                                        strUnitId = value.Key.ToString();
                                    }
                                }
                                string strNewCode = strCode + strTreeCode;
                                string strsql = "insert into ResMaterial (Version,MATERIALID,MATCODE,MATNAME,ALIAS,MaterialCategoryId,MATSPECIFICATION,Quality,PracticalityStateControlRuleId,ManageStateControlRuleId,MATATTRIBUTE,Requirement,StandardUnitID,States,CreatedDate,PERID,ModifyPerson,OperationOrg,OpgSysCode,IsAuto,IFCATRESOURCE)";
                                strsql += "values('0','" + strGuid + "','" + strNewCode + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','" + strMaterialCategoryId + "','" + OleDsExcle.Tables[0].Rows[i][4].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][6].ToString() + "','2','14','0','0','" + strUnitId + "','1',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strOperOrgInfoId + "','" + strOperOrgInfoName + "','" + OperOrgInfo + "','" + strOpgSysCode + "','0','0')";
                                int res = SaveSql(strsql);
                                if (res != 0)
                                {
                                    Rows += 1;
                                }
                            }
                        }
                    }
                    if (strCode.Length == 8)
                    {
                        string strRowCon = OleDsExcle.Tables[0].Rows[i][1].ToString();
                        if (strRowCon.Equals("") || strRowCon.Equals("null"))
                        {
                            //第二列信息为空
                            string strUnit = OleDsExcle.Tables[0].Rows[i][8].ToString();
                            if (strUnit != "")
                            {//excel表中有计量单位信息，无计量单位的信息不保存
                                //string streigth = strCode.Substring(0, 8);//截取前八位
                                string strMaterialCategoryId = null;
                                foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                                {
                                    if (objDE.Value.ToString().Equals(strCode))
                                    {
                                        strMaterialCategoryId = objDE.Key.ToString();
                                        break;
                                    }
                                }
                                //string strGuid = GetGuid();//生成Id
                                //有计量单位信息
                                string strUnitId = null;
                                foreach (System.Collections.DictionaryEntry value in hashtableUnit)
                                {
                                    if (value.Value.ToString().Equals(strUnit))
                                    {
                                        strUnitId = value.Key.ToString();
                                    }
                                }
                                string strNewCode = strCode + strTreeCode;
                                string strsql = "insert into ResMaterial (Version,MATERIALID,MATCODE,MATNAME,ALIAS,MaterialCategoryId,MATSPECIFICATION,Quality,PracticalityStateControlRuleId,ManageStateControlRuleId,MATATTRIBUTE,Requirement,StandardUnitID,States,CreatedDate,PERID,ModifyPerson,OperationOrg,OpgSysCode,IsAuto,IFCATRESOURCE)";
                                strsql += "values('0','" + strGuid + "','" + strNewCode + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][3].ToString() + "','" + strMaterialCategoryId + "','" + OleDsExcle.Tables[0].Rows[i][4].ToString() + "','" + OleDsExcle.Tables[0].Rows[i][6].ToString() + "','2','14','0','0','" + strUnitId + "','1',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strOperOrgInfoId + "','" + strOperOrgInfoName + "','" + OperOrgInfo + "','" + strOpgSysCode + "','0','0')";
                                int res = SaveSql(strsql);
                                if (res != 0)
                                {
                                    Rows += 1;
                                }
                            }
                        }
                        else
                        {
                            //长度为8，RESMATERIALCATEGORY
                            string strsix = strCode.Substring(0, 6);//截取前六位
                            foreach (System.Collections.DictionaryEntry objDE in hashtableMaterial)
                            {
                                if (objDE.Value.ToString().Equals(strsix))
                                {
                                    strId = objDE.Key.ToString();
                                    foreach (System.Collections.DictionaryEntry objType in hashtableMaterialType)
                                    {
                                        if (objType.Key.ToString().Equals(strId))
                                        {
                                            strParentId = objType.Value.ToString();
                                        }
                                    }
                                    break;
                                }
                            }
                            //string strGuid = GetGuid();//生成Id
                            string strSYSTEMCODE = strParentId + strGuid + ".";
                            string strsql = "insert into RESMATERIALCATEGORY (ID,NAME,NODETYPE,CODE,CREATEDATE,SYSCODE,STATE,numLEVEL,PARENTNODEID,THETREEID,MATCATATTRUBUTE,PraStateControlRuleID,ManStateControlRuleID,PERID,OrderNo,MatCatKind,OperationOrg,OpgSysCode,ABBREVIATION)";
                            strsql += "values ('" + strGuid + "','" + strName2 + "','2','" + OleDsExcle.Tables[0].Rows[i][0].ToString() + "',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strSYSTEMCODE + "','1','5','" + strId + "','7','0','2','14','" + strOperOrgInfoName + "','1','0','" + OperOrgInfo + "','" + strOpgSysCode + "','" + strName2 + "')";
                            //int res = savesql(command, sql);
                            int res = SaveSql(strsql);
                            if (res != 0)
                            {
                                Rows += 1;
                                hashtableMaterial.Add(strGuid, OleDsExcle.Tables[0].Rows[i][0].ToString());
                                hashtableMaterialType.Add(strGuid, strSYSTEMCODE);
                            }
                        }
                    }
                    if (strTreeCode.Equals("") && strCode != "" && strName2 != "")
                    {
                        //将分类信息保存到ResMaterial数据表中，只保存ID和名称
                        string strNewCode = strCode + "00000";
                        string strNewGuid = GetGuid();
                        string strNewUnitId = null;
                        string strNewUnitName = "个";
                        foreach (System.Collections.DictionaryEntry value in hashtableUnit)
                        {
                            if (value.Value.ToString().Equals(strNewUnitName))
                            {
                                strNewUnitId = value.Key.ToString();
                            }
                        }


                        string strsql = "insert into ResMaterial (Version,MATERIALID,MATCODE,MATNAME,MaterialCategoryId,PracticalityStateControlRuleId,ManageStateControlRuleId,MATATTRIBUTE,Requirement,States,CreatedDate,PERID,ModifyPerson,OperationOrg,OpgSysCode,IsAuto,IFCATRESOURCE,ALIAS,STANDARDUNITID)";
                        strsql += "values('0','" + strNewGuid + "','" + strNewCode + "','" + strName2 + "','" + strGuid + "','2','14','0','0','1',to_date('" + strDate + "','yyyy-mm-dd hh24:mi:ss'),'" + strOperOrgInfoId + "','" + strOperOrgInfoName + "','" + OperOrgInfo + "','" + strOpgSysCode + "','0','1','" + strName2 + "','" + strNewUnitId + "')";
                        int res = SaveSql(strsql);

                    }
                }
            }

            MessageBox.Show(Rows + "条信息保存成功！");
        }
        #endregion

    }

}
