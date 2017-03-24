using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using System.Collections;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using NHibernate.Exceptions;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.StockManage.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;
using Application.Business.Erp.SupplyChain.Util;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Service;
using NHibernate.Criterion;
using System.Data.SqlClient;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Service
{
    /// <summary>
    /// ���ⵥ����
    /// </summary>
    public class StockOutSrv : BaseService, IStockOutSrv
    {
        #region Code���ɷ���
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

        private string GetCode(Type type,string special)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, special);
        }

        private string GetCode(Type type, string projectId, string matCatAbb)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now,projectId,matCatAbb);
        }
        #endregion

        #region ����ͳ�Ʊ���
        /// �õ���Ŀ���ǲ��ϵ����Ľ��
        /// </summary>
        /// <returns></returns>
        public IList GetSporadicMoney(string projectID, DateTime startDate, DateTime endDate)
        {
            IList list = new ArrayList();
            decimal sporadicMoney = 0;
            decimal totalMoney = 0;
            decimal netMoney = 0;
            decimal elecMoney = 0;//����I139/I1220202 
            IList catList = new ArrayList();//�������ǲ��Ϸ����׼�ķ��༯��
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = " select t1.code from resmaterialcategory t1 where t1.ifsporadic=1 ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    catList.Add(TransUtil.ToString(dataRow["code"]));
                }
            }
            //��ѯ����Ŀ�ĳ�����ϸ��Ϣ
            command.CommandText = " select to_char(t2.materialcode) materialcode, t2.money from thd_stkstockout t1,thd_stkstockoutdtl t2 where t1.id=t2.parentid and t1.projectid =  '"
                    + projectID + "' and t1.stockoutmanner in (20) and t1.istally=1 and  t1.CreateDate>=to_date('" 
                    + startDate.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')"
                +" union all "
                    + " select '' materialcode,t1.summatmoney money from thd_materialbalancemaster t1 " +
                    " where t1.CreateDate>=to_date('"+ startDate.ToShortDateString() + "','yyyy-mm-dd') "+
                    " and t1.CreateDate<= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')  and t1.projectid='" + projectID + "'"
                + " union all "
                    + " select to_char(t2.materialcode) materialcode,-t2.totalvalue money from thd_wastematprocessmaster t1,thd_wastematprocessdetail t2 " +
                    " where t1.id=t2.parentid and t1.state=5 and  t1.CreateDate>=to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') " +
                    " and t1.CreateDate<= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')  and t1.projectid='" + projectID + "'";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string materialcode = TransUtil.ToString(dataRow["materialcode"]);
                    decimal money = TransUtil.ToDecimal(dataRow["money"]);
                    totalMoney += money;
                    if (materialcode != null && materialcode.Contains("I1370201"))
                    {
                        netMoney += money;
                    }
                    if (materialcode != null && (materialcode.Contains("I139") || materialcode.Contains("I1220202")))
                    {
                        elecMoney += money;
                    }
                    bool ifExist = false;
                    foreach (string catCode in catList)
                    {
                        if (materialcode != null && materialcode.Contains(catCode))
                        {
                            ifExist = true;
                        }
                    }
                    if (ifExist == true)
                    {
                        sporadicMoney += money;
                    }
                }
            }
            list.Add(totalMoney);
            list.Add(sporadicMoney);
            list.Add(netMoney);
            list.Add(elecMoney);
            return list;
        }
        #endregion

        #region ���ϳ��ⵥ����
        public IList GetStockOut(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.SubjectGUID", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockOut), oq);
        }
        public IList GetMaterialLst(IList list)
        {
            ObjectQuery oObjectQuery = new ObjectQuery();
            oObjectQuery .AddCriterion (Expression .In ("Id",list ));
            return Dao.ObjectQuery(typeof(Material), oObjectQuery);
        }
        public IList GetMaterial(string sID)
        {
            ObjectQuery oObjectQuery = new ObjectQuery();
            oObjectQuery.AddCriterion(Expression.Eq("Id", sID));
            return Dao.ObjectQuery(typeof(Material), oObjectQuery);
        }
        public StockOut GetStockOutByCode(string code,string special,string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code",code));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockOut(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockOut;
            }
            return null;
        }
       
        public StockOut GetStockOutByCode(string code, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockOut(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockOut;
            }
            return null;
        }

        public StockOut GetStockOutById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetStockOut(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockOut;
            }
            return null;
        }

        [TransManager]
        public StockOut SaveStockOut(StockOut obj)
        {
            if (obj.Id == null)
            {
                if (obj.Special == "����")
                {
                    obj.Code = GetCode(typeof(StockOut), obj.ProjectId, obj.MaterialCategory==null?"":obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "��װ")
                {
                    obj.Code = GetCode(typeof(StockOut), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;

            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return (StockOut)SaveOrUpdateByDao(obj);
        }
        [TransManager]
        public StockOut SaveStockOut1(StockOut obj)
        {
            if (obj.Id == null)
            {
                if (obj.Special == "����")
                {
                    obj.Code = GetCode(typeof(StockOut), obj.ProjectId, obj.MaterialCategory == null ? "��Ʒ��" : obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "��װ")
                {
                    obj.Code = GetCode(typeof(StockOut), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;

            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return (StockOut)SaveOrUpdateByDao(obj);
        }
        /// <summary>
        /// �����ѯ
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet StockOutQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql = @"SELECT t2.id mxid,t1.id,t1.Code,t1.SupplierRelationName,t1.moveoutprojectname,t1.STATE,t1.IsLimited,t1.CreateDate,t1.CreatePersonName,t1.Descript,t1.PrintTimes,t2.subjectname,
                            t1.StockOutManner,t2.UsedPartName,t2.MaterialCode,t2.MaterialName,t1.TheStockInOutKind,t2.movePrice,t2.MoveMoney,
                            t2.MaterialSpec,t2.Quantity,t2.Price,t2.MONEY,t2.MatStandardUnitName,t2.ProfessionalCategory,t1.SumQuantity,t1.SumMoney,t2.DiagramNumber,t2.material materialID
                            FROM thd_stkStockOut t1 inner join thd_stkstockOutdtl t2
                            ON t1.id=t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.Code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }
        public DataSet StockOutMasterQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql = @"SELECT  t1.id,t1.Code,t1.SupplierRelationName,t1.moveoutprojectname,t1.STATE,t1.IsLimited,t1.CreateDate,t1.CreatePersonName,t1.Descript,t1.PrintTimes,
                            t1.SumQuantity,t1.SumMoney
                            FROM thd_stkStockOut t1 ";
            sql += " where 1=1 " + condition + " order by t1.Code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }
        /// <summary>
        /// ������ϸId��ѯ���ϳ��ⵥ��ϸ
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public StockOutDtl GetStockOutDtlById(string stockOutDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", stockOutDtlId));
            IList list = dao.ObjectQuery(typeof(StockOutDtl), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockOutDtl;
            }
            return null;
        }

        /// <summary>
        /// ��ѯ����ʱ��
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetStockOutDtlSeq(ObjectQuery oq)
        {
            return dao.ObjectQuery(typeof(StockOutDtlSeq), oq);
        }

        /// <summary>
        /// ���ݳ���Id��ѯ����ʱ��(�����ʱ������)
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public IList GetStockOutDtlSeqByStockOutDtlId(string stockOutDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("StockOutDtl.Id", stockOutDtlId));
            oq.AddOrder(Order.Desc("SeqCreateDate"));
            return GetStockOutDtlSeq(oq);
        }

        /// <summary>
        /// ���������ϸ��ѯ����ʱ��
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <returns></returns>
        public IList GetStockOutDtlSeqByStockInDtlId(string stockInDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("StockInDtlId", stockInDtlId));
            return GetStockOutDtlSeq(oq);
        }

        /// <summary>
        /// ��ѯ������ϸ(��������ʱ��)
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public StockOutDtl GetStockOutDtlByIdWithDetails(string stockOutDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", stockOutDtlId));
            oq.AddFetchMode("StockOutDtlSeqList", NHibernate.FetchMode.Eager);
            IList list = dao.ObjectQuery(typeof(StockOutDtl), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockOutDtl;
            }
            return null;
        }

        /// <summary>
        /// ������ϸʱ��������ϸID��ѯ������ϸ
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <returns></returns>
        public IList GetStockOutDtlByStockInDtlId(string stockInDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("StockOutDtlSeqList", FetchMode.Eager);
            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            //oq.AddCriterion(Expression.Eq("MaterialResource.Id", "1"));
            //oq.AddCriterion(Expression.Eq("StockOutDtlSeqList.StockInDtlId", stockInDtlId));
            //oq.AddCriterion(Expression.Eq("StockOutDtlSeq.StockInDtlId", stockInDtlId));
            
            //oq.AddOrder(new NHibernate.Criterion.Order("Master.CreateDate", true));
            return Dao.ObjectQuery(typeof(StockOutDtl), oq);
        }

        /// <summary>
        /// ������ϸʱ��������ϸID��ѯ������ϸ
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <returns></returns>
        public DataSet QueryStockOutDtl(string stockInDtlId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT t1.Code,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.Quantity,t2.RefQuantity,t2.Price
                FROM thd_stkstockout t1 inner join thd_stkstockoutdtl t2 ON t2.ParentId=t1.id
                inner JOIN thd_stkstockoutdtlSeq t3 ON t3.StockOutDtlId=t2.id
                WHERE 1=1 and t3.StockInDtlId='{0}'";
            command.CommandText = string.Format(sql, stockInDtlId);
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        /// <summary>
        /// ��ѯ���ⵥ ������������ȷ�ϼ۸�
        /// </summary>
        /// <param name="ssStockOutDtlID"></param>
        /// <returns></returns>
        public DataSet QueryStockInQuantityAndOutPrice(string sStockOutDtlID)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select nvl(t.confirmprice,0) as confirmprice,nvl(t1.quantity,0) as quantity,t1.stockoutdtlid from thd_stkstockindtl t join thd_stkstockoutdtlseq t1 on t.id=t1.stockindtlid  where t1.stockoutdtlid='{0}' order by t1.seqcreatedate ";
            command.CommandText = string.Format(sql, sStockOutDtlID);
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        #endregion
        /// <summary>
        /// ��ȡ��װ�ĺ���ڵ�
        /// </summary>
        /// <returns></returns>
        public IList GetSetUpCostAccountSubject()
        {
            ObjectQuery oq = new ObjectQuery();
            IList listName = new ArrayList();
            //'��װ���ķ�','���������','�����Ͼ����ʴ���','���ϼ���','��װ�豸��','���Ϸ�����'
            listName.Add("��װ���ķ�");
            listName.Add("���������");
            listName.Add("�����Ͼ����ʴ���");
            listName.Add("���ϼ���");
            listName.Add("��װ�豸��");
            listName.Add("���Ϸ�����");
            oq.AddCriterion(Expression.In("Name", listName));

            IList list = dao.ObjectQuery(typeof(CostAccountSubject), oq);
            return list;
        }
        /// <summary>
        /// �����ѯ
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <param name="oq">��ѯ����</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// ���� ��Ŀ���� �������� ����ID��ȡû�м��˵ĵ���
        /// </summary>
        /// <param name="sProjectName"></param>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public DataSet QueryListStockOutNotHS(string sProjectName, string sCode)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sSQL = "select t.id ,t.code,t.summoney,t.sumquantity,t.createpersonname,t.realoperationdate,to_char(t.createdate,'YYYY-MM-DD')createdate,t.descript ,t.projectid,t.projectname  from thd_stkstockout t where t.projectname='{0}' and t.code='{1}'";
            sSQL = string.Format(sSQL, sProjectName, sCode);
            command.CommandText = sSQL;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        public bool UpdateStockOutNotHS(string sID, DateTime time, string sDescript, int iYear, int iMonth)
        {
             
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            
           
                string sSQL = "update thd_stkstockout t  set t.createdate =to_date('{1}','YYYY-MM-DD'), t.descript='{2}',t.createyear={3},t.createmonth={4} where t.id='{0}'";
                
                string sSQLTemp = string.Format(sSQL, sID, time.ToShortDateString(), sDescript, iYear, iMonth);
                command.CommandText = sSQLTemp;
                command.ExecuteNonQuery();
             
           return  true ;
            
        }

        #region ���ϳ��ⵥ�쵥����
        public IList GetStockOutRed(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockOutRed), oq);
        }

        public StockOutRed GetStockOutRedByCode(string code, string special, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockOutRed(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockOutRed;
            }
            return null;
        }

        public StockOutRed GetStockOutRedByCode(string code,string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockOutRed(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockOutRed;
            }
            return null;
        }

        public StockOutRed GetStockOutRedById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetStockOutRed(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockOutRed;
            }
            return null;
        }
        [TransManager]
        public StockOutRed SaveStockOutRed(StockOutRed obj )
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                if (obj.Special == "����")
                {
                    obj.Code = GetCode(typeof(StockOutRed), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "��װ")
                {
                    obj.Code = GetCode(typeof(StockOutRed), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveByDao(obj) as StockOutRed;
            }
            return obj;
        }
        [TransManager]
        public StockOutRed SaveStockOutRed(StockOutRed obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                if (obj.Special == "����")
                {
                    obj.Code = GetCode(typeof(StockOutRed), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "��װ")
                {
                    obj.Code = GetCode(typeof(StockOutRed), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveByDao(obj) as StockOutRed;
                //����ʱ�޸�ǰ�����ݵ���������
                foreach (StockOutRedDtl dtl in obj.Details)
                {
                    StockOutDtl forwardDtl = GetStockOutDtlById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            } else
            {
                if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveOrUpdateByDao(obj) as StockOutRed;
                foreach (StockOutRedDtl dtl in obj.Details)
                {
                    StockOutDtl forwardDtl = GetStockOutDtlById(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                    } else
                    {
                        //�޸�ʱ�޸�ǰ�����ݵ���������Ϊ ��������+��ǰ����������ϴγ�������Ĳ�ֵ
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                    }
                    dao.Save(forwardDtl);
                }

                //�޸�ʱ����ɾ������ϸ ɾ����������
                foreach (StockOutRedDtl dtl in movedDtlList)
                {
                    StockOutDtl forwardDtl = GetStockOutDtlById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            }
            
            return obj;
        }

        [TransManager]
        public bool DeleteStockOutRed(StockOutRed obj)
        {
            if (obj.Id == null) return true;
            //ɾ����ϸʱ ɾ����������
            foreach (StockOutRedDtl dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    StockOutDtl forwardDtl = GetStockOutDtlById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            }
            return dao.Delete(obj);
        }
        #endregion

        #region �������ⵥ��ѯ����
        public IList GetStockMoveOut(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockMoveOut), oq);
        } 
        /// <summary>
        /// ��ѯ��������쵥
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetStockMoveOutRed(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            return dao.ObjectQuery(typeof(StockMoveOutRed), oq);
        }
        #endregion

        #region ������˷���
        private IStockInOutSrv stockInOutSrv;
        public IStockInOutSrv StockInOutSrv
        {
            get { return stockInOutSrv; }
            set { stockInOutSrv = value; }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="hashLst">key StockIn;value id List;key StockInRed;value id List;</param>
        /// <param name="hashCode">key StockIn;value Code List;key StockInRed;value Code List;</param>
        /// <param name="year">������</param>
        /// <param name="month">������</param>
        /// <param name="tallyPersonId">������ID</param>
        /// <param name="tallyPersonName">����������</param>
        /// <returns></returns>
        virtual public Hashtable TallyStockOut(Hashtable hashLst, Hashtable hashCode, int year, int month, string tallyPersonId, string tallyPersonName, string projectId)
        {
            //����ϸ����Ƿ���ʣ��ϸ��±�����ʣ����²��ܿ�չ����ҵ��
            string lastDate = year.ToString() + "-" + month.ToString() + "-01";
            int lastYear = Convert.ToDateTime(lastDate).AddMonths(-1).Year;
            int lastMonth = Convert.ToDateTime(lastDate).AddMonths(-1).Month;
            //2013-1-22ע��
            //if (!stockInOutSrv.CheckIfNewProject(projectId))
            //{
            //    if (!StockInOutSrv.CheckReckoning(lastYear, lastMonth, projectId))
            //        throw new Exception("�����[" + lastYear.ToString() + "-" + lastMonth.ToString() + "]δ����,���Ƚ��н��ˣ�");

            //    if (StockInOutSrv.CheckReckoning(year, month, projectId))
            //        throw new Exception("�����[" + year.ToString() + "-" + month.ToString() + "]�Ѿ�����,���ܽ��м��ˣ�");
            //}

            Hashtable returnValue = new Hashtable();

            if (hashLst != null)
            {
                foreach (string billName in hashLst.Keys)
                {
                    if (billName == "StockOut")
                    {
                        returnValue = Tally(hashLst[billName] as IList, hashCode[billName] as IList, 0, year, month, tallyPersonId, tallyPersonName);
                    } else if (billName == "StockOutRed")
                    {
                        returnValue = Tally(hashLst[billName] as IList, hashCode[billName] as IList, 1, year, month, tallyPersonId, tallyPersonName);
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// ������˷���
        /// </summary>
        /// <param name="billIdList">ID List</param>
        /// <param name="codeList">Code List</param>
        /// <param name="billType">0��ʾ������1��ʾ�쵥</param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="tallyPersonId"></param>
        /// <param name="tallyPersonName"></param>
        /// <returns></returns>
        private Hashtable Tally(IList billIdList, IList codeList, int billType, int year, int month, string tallyPersonId, string tallyPersonName)
        {
            Hashtable retValue = new Hashtable();
            if (billIdList == null) return retValue;

            IList listSuccess = new ArrayList();
            string errMsg = "";

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;

            foreach (string billId in billIdList)
            {
                IDbCommand cmd = cnn.CreateCommand();
                session.Transaction.Enlist(cmd);
                cmd.CommandText = "thd_StockOutTally";
                
                cmd.CommandType = CommandType.StoredProcedure;

                #region �������
                //����ID
                IDbDataParameter id = cmd.CreateParameter();
                id.Value = billId;
                id.Direction = ParameterDirection.Input;
                id.ParameterName = "billid";
                cmd.Parameters.Add(id);

                //������ID
                IDbDataParameter v_tallyPersonId = cmd.CreateParameter();
                v_tallyPersonId.Value = tallyPersonId;
                v_tallyPersonId.Direction = ParameterDirection.Input;
                v_tallyPersonId.ParameterName = "tallyPersonId";
                cmd.Parameters.Add(v_tallyPersonId);

                //����������
                IDbDataParameter v_tallyPersonName = cmd.CreateParameter();
                v_tallyPersonName.Value = tallyPersonName;
                v_tallyPersonName.Direction = ParameterDirection.Input;
                v_tallyPersonName.ParameterName = "tallyPersonName";
                cmd.Parameters.Add(v_tallyPersonName);

                //��������
                IDbDataParameter v_TallyDate = cmd.CreateParameter();
                v_TallyDate.DbType = DbType.DateTime;
                v_TallyDate.Direction = ParameterDirection.Input;
                v_TallyDate.Value = DateTime.Now;
                v_TallyDate.ParameterName = "TallyDate";
                cmd.Parameters.Add(v_TallyDate);

                //�����
                IDbDataParameter v_TallyYear = cmd.CreateParameter();
                v_TallyYear.Direction = ParameterDirection.Input;
                v_TallyYear.Value = year;
                v_TallyYear.ParameterName = "TallyYear";
                cmd.Parameters.Add(v_TallyYear);

                //�����
                IDbDataParameter v_TallyMonth = cmd.CreateParameter();
                v_TallyMonth.Direction = ParameterDirection.Input;
                v_TallyMonth.Value = month;
                v_TallyMonth.ParameterName = "TallyMonth";
                cmd.Parameters.Add(v_TallyMonth);

                //�졢������־
                IDbDataParameter v_BillType = cmd.CreateParameter();
                v_BillType.Direction = ParameterDirection.Input;
                v_BillType.Value = billType;
                v_BillType.ParameterName = "BillType";
                cmd.Parameters.Add(v_BillType);

                IDbDataParameter err = cmd.CreateParameter();
                err.DbType = DbType.AnsiString;
                err.Direction = ParameterDirection.Output;
                err.ParameterName = "errMsg";
                err.Size = 500;
                cmd.Parameters.Add(err);
                #endregion

                cmd.ExecuteNonQuery();
                if (err.Value == null || Convert.ToString(err.Value) == "")
                    listSuccess.Add(billId);
                else
                    errMsg += "����:" + codeList[billIdList.IndexOf(billId)].ToString() + ":" + err.Value + "\n";
            }
            retValue.Add("err", errMsg);
            retValue.Add("Succ", listSuccess);
            return retValue;
        }
        #endregion

        #region ����
        /// <summary>
        /// ������ⵥ��ϸ��ID��ȡ����ʱ��� ���ⵥ��ϸ 
        /// </summary>
        /// <param name="sStockInDtlID"></param>
        /// <returns></returns>
      public  IList  GetStockOutDtlSeqByStockInDtlID1(string sStockInDtlID)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(  Expression .Eq  ("StockInDtlId", sStockInDtlID));
         // oQuery .AddCriterion (Expression .Or (Expression .Eq ("StockOutDtl.Master.TheStockInOutKind",0),Expression .Eq ("StockOutDtl.Master.TheStockInOutKind",3)));
           // oQuery.AddCriterion(Expression.Eq("StockOutDtl.Master.TheStockInOutKind", long.Parse ("0")));  
            
           // oQuery.AddCriterion(Expression.Sql("TheStockInOutKind=0"));
            oQuery.AddFetchMode("StockOutDtl", FetchMode.Eager);
           //oQuery.AddFetchMode("StockOutDtl.Master", FetchMode.Eager);
            //oQuery.AddFetchMode("StockOutDtl.Master.MaterialCategory", FetchMode.Eager);
          
            IList list = dao.ObjectQuery(typeof(StockOutDtlSeq), oQuery);

          //foreach(StockOutDtlSeq item in list)
          //{
          //    object obj= item.StockOutDtl.Master;
          //}
            return list;
        }
      
      public StockOut  GetStockOutByStockOutDtlID(string sID)
      {
          ISession session = CallContext.GetData("nhsession") as ISession;
          IDbConnection cnn = session.Connection;
          IDbCommand command = cnn.CreateCommand();
          StockOut oStockOut=null;
          string sql = @"select distinct t.id from thd_stkstockout t join thd_stkstockoutdtl t1 on t.id=t1.parentid and t1.id='{0}'";
          sql = string.Format(sql, sID);
           
          command.CommandText = sql;
          string sStockOutID= command.ExecuteScalar() as string ;
          if (!string.IsNullOrEmpty(sStockOutID))
          {
              ObjectQuery oQuery = new ObjectQuery();
              oQuery.AddCriterion(Expression.Eq("Id", sID));
              StockOutDtl d = new StockOutDtl();
              oQuery.AddFetchMode("MaterialCategory", FetchMode.Eager);
              IList list = dao.ObjectQuery(typeof(StockOut), oQuery);
              if (list != null && list.Count > 0)
              {
                  oStockOut = list[0] as StockOut;
              }
          }
         return oStockOut;
          
      }
        #endregion

      public string GetUnitDiffMaterial(IList lstMaterial, string sProjectID, string sSpecial, string sProfessioncategory, IList lstDiagramNum)
      {

          ISession session = CallContext.GetData("nhsession") as ISession;
          IDbConnection cnn = session.Connection;
          IDbCommand command = cnn.CreateCommand();
          string sSQL = "select distinct t.material ,t1.matstandardunit,t1.matstandardunitname from thd_stkstockrelation t join thd_stkstockindtl t1 on t.stockindtlid=t1.id  ";
          string sWhere=string.Empty ;
          string sTemp = string.Empty;
          string sMsg = string.Empty;
          string sMsgTemp = string.Empty;
          string sMaterialID=string .Empty ;
          string sMatStandardUnitName=string.Empty ;

          sTemp = GetMaterialWhere(lstMaterial, lstDiagramNum);
          sWhere = "  where " + sTemp;
          sWhere += string.Format(" and nvl(t.remainquantity,0)>0  and t.projectid='{0}' and t.special='{1}'", sProjectID, sSpecial);
          if (string.Equals(sSpecial.Trim(), "��װ"))
          {//sWhere = " where {0}  and t.projectid='{1}' and t.special='{2}' and t.professioncategory='{3}' and t.diagramnumber='{4}' and  t.special='{5}'";

              sWhere += "  and " + WhereStatement("t.professioncategory", sProfessioncategory);
             // sWhere += "  and " + WhereStatement("t.diagramnumber", sDiagramnumber);

          }
          else
          {
          }
          sSQL += sWhere;
          command.CommandText = sSQL;
          IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
          DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
          if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
          {
              foreach (Material oMaterial in lstMaterial)
              {
                  sMsgTemp=string.Empty ;
                  foreach (DataRow oRow in dataSet.Tables[0].Rows)
                  {
                      sMaterialID = GetValue(oRow, "material");
                      if (string.Equals(oMaterial.Id, sMaterialID))
                      {
                          sMatStandardUnitName = GetValue(oRow, "matstandardunitname");
                          if (string.IsNullOrEmpty(sMsgTemp))
                          {
                              sMsgTemp = string.Format("��������:��{0}�� ���ʡ�{1}�� ����ͺš�{2}��  ��λ:{3}��{4}", oMaterial.Name, oMaterial.Stuff, oMaterial.Specification, oMaterial.BasicUnit.Name, sMatStandardUnitName);
                          }
                          else
                          {
                              sMsgTemp += "��" + sMatStandardUnitName;
                          }
                      }
                  }
                  if (!string.IsNullOrEmpty(sMsgTemp))
                  {
                      if (string.IsNullOrEmpty(sMsg))
                      {
                          sMsg += sMsgTemp;
                      }
                      else
                      {
                          sMsg += "\n"+sMsgTemp;
                      }
                  }
              }
          }
          return sMsg;
      }
      public string GetValue(DataRow oRow, string sFiledName)
      {
          string sValue = string.Empty;
          object obj = null;
          obj = oRow[sFiledName];
          if (obj != null)
          {
              sValue = obj.ToString();
          }
          return sValue;  
      }
      public string WhereStatement(string sFieldName, string sValue)
      {
          string sWhere = string.Empty;
          if (string.IsNullOrEmpty(sValue.Trim()))
          {
              sWhere = string .Format ("   ({0} is null) ",sFieldName );
          }
          else
          {
              sWhere = string.Format(" {0}='{1}' ", sFieldName, sValue);
          }
          return sWhere;
      }
      public string GetMaterialWhere(IList lstMaterial,IList    lstDiagramNum)
      {
          string sWhere = string.Empty;
          Material oMaterial=null;
          string sDiagramNum=string.Empty ;
          string sTemp = "( t.material='{0}' and  t1.matstandardunit<>'{1}' and {2} )";
          if(lstDiagramNum !=null &&lstMaterial!=null && lstMaterial .Count ==lstDiagramNum .Count )
          {
              sTemp = "( t.material='{0}' and  t1.matstandardunit<>'{1}' and {2} )";
              for (int i = 0; i < lstDiagramNum.Count;i++ )
              {
                  oMaterial = lstMaterial[i] as Material ;
                  sDiagramNum = lstDiagramNum[i] as string;
                  if (string.IsNullOrEmpty(sWhere))
                  {
                      sWhere = string.Format(sTemp, oMaterial.Id, oMaterial.BasicUnit.Id, WhereStatement("t.diagramnumber", sDiagramNum));
                  }
                  else
                  {
                      sWhere += " or " + string.Format(sTemp, oMaterial.Id, oMaterial.BasicUnit.Id, WhereStatement("t.diagramnumber", sDiagramNum));
                  }
              }
          }
          else if (lstMaterial != null && lstDiagramNum==null)
          {
              sTemp = "( t.material='{0}' and  t1.matstandardunit<>'{1}'  )";
              for (int i = 0; i < lstMaterial.Count; i++)
              {
                  oMaterial = lstMaterial[i] as Material;
                
                  if (string.IsNullOrEmpty(sWhere))
                  {
                      sWhere = string.Format(sTemp, oMaterial.Id, oMaterial.BasicUnit.Id);
                  }
                  else
                  {
                      sWhere += " or " + string.Format(sTemp, oMaterial.Id, oMaterial.BasicUnit.Id);
                  }
              }
          }
          if (!string.IsNullOrEmpty(sWhere))
          {
              sWhere = "(" + sWhere + ")";
          }
          else
          {
              sWhere = "(1=1)";
          }
          return sWhere;
      }
      public DataSet GetStockMatByUnit(string sProjectID,string sSpec ,string sCode ,string sName , string sSpecial,string sProfession, MaterialCategory oMaterialCategory,bool IsMoveOut)
      {
          ISession session = CallContext.GetData("nhsession") as ISession;
          IDbConnection cnn = session.Connection;
          IDbCommand command = cnn.CreateCommand();
          string sWhere = string.Empty;
          string sMoveOut = "idlequantity";
          string sStockOut = "remainquantity";
          string sOutType = IsMoveOut ? sMoveOut : sStockOut;
          string sDiagramNumber=string.Empty ;
          if (!string.IsNullOrEmpty(sSpec))
          {
              sWhere = string.Format(" and t.materialspec like '%{0}%'", sSpec);
          }
          if (!string.IsNullOrEmpty(sCode))
          {
              sWhere += string.Format(" and t.materialcode like '%{0}%'", sCode);
          }
          if (!string.IsNullOrEmpty(sName))
          {
              sWhere += string.Format(" and t.materialname like '%{0}%'", sName);
          }
          if (string.Equals(sSpecial, "��װ"))
          {
              sWhere += string.Format("  and t.professioncategory='{0}'", sProfession);
              sDiagramNumber = ",t.diagramnumber";
            
          }
          if (oMaterialCategory != null)
          {
              sWhere += string.Format(" and t1.thesyscode like '{0}%'", oMaterialCategory.SysCode);
          }
          
          string sSQL = @"select t.material,t.materialname,t.materialcode,t.materialspec,t.materialstuff,
                         '' matstandardunit,t.matstandardunitname {4}, sum(t.{3})quantity
                        from thd_stkstockrelation t
                        join resmaterial t1 on  t.material=t1.materialid  
                        where  t.projectid='{0}' and t.special='{1}' {2}
                        group by t.material,t.materialname,t.materialcode,t.materialspec,t.materialstuff,
                        t.matstandardunitname {4}
                        having sum(t.{3})>0";
          sSQL = string.Format(sSQL, sProjectID, sSpecial, sWhere, sOutType, sDiagramNumber);
          command.CommandText = sSQL;
          IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
          DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
          return dataSet;
      }
     
    }
}