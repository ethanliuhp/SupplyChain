using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialCollectionMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalLedgerMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialBalanceMng.Domain;
using VirtualMachine.Component.Util;
using Application.Resource.CommonClass.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Service;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalContractMng.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.Service
{
    /// <summary>
    /// 料具租赁管理服务
    /// </summary>
    public class MatMngSrv : BaseService, IMatMngSrv
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        private string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }

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
        private IMaterialSettleSrv materialSettleSrv;
        public IMaterialSettleSrv MaterialSettleSrv
        {
            get { return materialSettleSrv; }
            set { materialSettleSrv = value; }
        }
        #region 料具租赁合同方法
        /// <summary>
        /// 通过ID查询料具租赁合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaterialRentalOrderMaster GetMaterialRentalOrderById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialRentalOrder(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialRentalOrderMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询料具租赁合同
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MaterialRentalOrderMaster GetMaterialRentalOrderByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetMaterialRentalOrder(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialRentalOrderMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询料具租赁合同
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialRentalOrder(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("BasiCostSets", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.BasicDtlCostSets", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialRentalOrderMaster), objectQuery);
        }

        /// <summary>
        /// 查询料具租赁合同明细信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialRentalOrderDetail(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Master.TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master.TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master.BasiCostSets", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MatStandardUnit", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master.OperOrgInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("BasicDtlCostSets", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialRentalOrderDetail), objectQuery);
        }

        /// <summary>
        /// 料具租赁合同查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet MaterialRentalOrderQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.code,t1.realoperationdate,t1.createdate,t1.originalContractNo,t5.ORGNAME,t1.state,t7.MATCODE,t7.MATNAME,t7.MATSPECIFICATION,
                t2.Quantity,t2.price,t1.balRule,t8.STANDUNITNAME,t2.Descript,t1.PrintTimes
                FROM thd_materialRentalOrderMaster t1 Left JOIN thd_materialRentalOrderDetail t2 ON t1.id=t2.parentId
                LEFT JOIN ResMaterial t7 ON t7.MATERIALID=t2.Material
                left join ressupplierrelation t4 on t1.supplierrelation=t4.suprelid
                left join resorganization t5 on t4.orgid=t5.orgid
                left join resstandunit t8 on t2.MATSTANDARDUNIT=t8.STANDUNITID";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        /// <summary>
        /// 校验当前料具商是否已经签订租赁合同
        /// </summary>
        /// <param name="theSupplier"></param>
        /// <returns></returns>
        public bool VerifyCurrSupplierOrder(SupplierRelationInfo theSupplier, MaterialRentalOrderMaster Master)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "";
            if (Master.Id != null)
            {
                sql = "SELECT * FROM thd_materialRentalOrderMaster WHERE SupplierRelation ='" + theSupplier.Id + "'and Id !='" + Master.Id + "' and projectid='" + Master.ProjectId + "'";

            }
            else
            {

                sql = "SELECT * FROM thd_materialRentalOrderMaster WHERE SupplierRelation ='" + theSupplier.Id + "' and projectid='" + Master.ProjectId + "'";

            }
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable table = dataSet.Tables[0];
            int count = table.Rows.Count;
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [TransManager]
        public MaterialRentalOrderMaster SaveMaterialRentalOrderMaster(MaterialRentalOrderMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(MaterialRentalOrderMaster), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as MaterialRentalOrderMaster;
        }
        #endregion

        #region 料具收料方法
        /// <summary>
        /// 保存收料单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public MaterialCollectionMaster SaveMaterialCollectionMaster(MaterialCollectionMaster obj)
        {
            bool isNew = true;



            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(MaterialCollectionMaster), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            #region 1. 处理收料数量为负数：按照退料处理(生成退料时序),并保存收料单

            #region 1.根据先进先出算法组织退料时序表

            //循环校验收料明细的数量是否为负数：
            IList list_MatRenLed = new ArrayList();

            foreach (MaterialCollectionDetail detail in obj.Details)
            {
                if (detail.Quantity < 0)
                {
                    MaterialReturnDetailSeq theMaterialReturnDetailSeq = new MaterialReturnDetailSeq();

                    MaterialReturnDetail ReturnDetail = new MaterialReturnDetail();
                    ReturnDetail.MaterialResource = detail.MaterialResource;
                    ReturnDetail.ExitQuantity = Math.Abs(detail.Quantity);

                    IList list = GetMatLeftQuantityByNew(ReturnDetail, obj.TheSupplierRelationInfo, detail.BorrowUnit, obj.ProjectId);
                    foreach (MaterialRentalLedger theMaterialRentalLedger in list)
                    {
                        theMaterialReturnDetailSeq = new MaterialReturnDetailSeq();
                        //判断当前台账的类型：0：收料；1：退料(退料是负数时产生的收料数量参数到先进先出的退料算法中)
                        if (theMaterialRentalLedger.WashType == 0)
                        {
                            theMaterialReturnDetailSeq.MatCollDtlId = theMaterialRentalLedger.BillDetailId;
                        }
                        else
                        {
                            theMaterialReturnDetailSeq.MatReturnDtlId = theMaterialRentalLedger.BillDetailId;
                        }
                        theMaterialReturnDetailSeq.SeqType = "收料(数量小于0)";
                        theMaterialReturnDetailSeq.MatLedgerId = theMaterialRentalLedger.Id;
                        theMaterialReturnDetailSeq.ReturnQuantity = theMaterialRentalLedger.TempQuantity;
                        theMaterialReturnDetailSeq.ReturnDate = obj.CreateDate;

                        //收退料单号和收退料明细数量
                        theMaterialReturnDetailSeq.MatCollCode = obj.Code;
                        theMaterialReturnDetailSeq.MatCollDtlQty = detail.Quantity;
                        theMaterialReturnDetailSeq.MatReturnCode = theMaterialRentalLedger.BillCode;
                        theMaterialReturnDetailSeq.MatReturnDtlQty = theMaterialRentalLedger.ReturnQuantity;

                        list_MatRenLed.Add(theMaterialRentalLedger);

                        detail.AddMatReturnDtlSeq(theMaterialReturnDetailSeq);
                    }
                }
            }
            MaterialCollectionMaster master = SaveByDao(obj) as MaterialCollectionMaster;

            #endregion

            #region 2.更新台账(收料剩余数量)
            foreach (MaterialRentalLedger MaterialRentalLedger in list_MatRenLed)
            {
                UpdateByDao(MaterialRentalLedger);
            }
            #endregion

            #endregion

            #region 2. 生成台账信息
            foreach (MaterialCollectionDetail detail in master.Details)
            {
                MaterialRentalLedger theMaterialRentalLedger = new MaterialRentalLedger();
                theMaterialRentalLedger.TheSupplierRelationInfo = master.TheSupplierRelationInfo;
                theMaterialRentalLedger.SupplierName = master.SupplierName;
                theMaterialRentalLedger.TheRank = detail.BorrowUnit;
                theMaterialRentalLedger.TheRankName = detail.BorrowUnitName;
                theMaterialRentalLedger.ProjectId = master.ProjectId;
                theMaterialRentalLedger.ProjectName = master.ProjectName;
                theMaterialRentalLedger.WashType = 0;//收料
                theMaterialRentalLedger.BillCode = master.Code;
                theMaterialRentalLedger.BillId = master.Id;
                theMaterialRentalLedger.OldContractNum = master.OldContractNum;
                theMaterialRentalLedger.SystemDate = DateTime.Now;
                theMaterialRentalLedger.RealOperationDate = master.CreateDate;
                theMaterialRentalLedger.BillDetailId = detail.Id;
                if (detail.Quantity < 0)
                {
                    theMaterialRentalLedger.LeftQuantity = 0;
                }
                else
                {
                    theMaterialRentalLedger.LeftQuantity = detail.Quantity;
                }
                theMaterialRentalLedger.CollectionQuantity = detail.Quantity;
                theMaterialRentalLedger.MaterialResource = detail.MaterialResource;
                theMaterialRentalLedger.MaterialCode = detail.MaterialCode;
                theMaterialRentalLedger.MaterialName = detail.MaterialName;
                theMaterialRentalLedger.MaterialSpec = detail.MaterialSpec;
                theMaterialRentalLedger.MatStandardUnit = detail.MatStandardUnit;
                theMaterialRentalLedger.MatStandardUnitName = detail.MatStandardUnitName;
                theMaterialRentalLedger.UsedPart = detail.UsedPart;
                theMaterialRentalLedger.UsedPartName = detail.UsedPartName;
                theMaterialRentalLedger.UsedPartSysCode = detail.UsedPartSysCode;
                theMaterialRentalLedger.SubjectGUID = detail.SubjectGUID;
                theMaterialRentalLedger.SubjectName = detail.SubjectName;
                theMaterialRentalLedger.SubjectSysCode = detail.SubjectSysCode;
                theMaterialRentalLedger.RentalPrice = detail.RentalPrice;

                SaveByDao(theMaterialRentalLedger);
            }
            #endregion

            #region 3.校验退料明细数量是否退料时序的数量(和)相等

            foreach (MaterialCollectionDetail Detail in master.Details)
            {
                if (Detail.Quantity < 0)
                {
                    decimal ReturnSeqQty = 0;
                    foreach (MaterialReturnDetailSeq seq in Detail.MaterialReturnDetailSeqs)
                    {
                        ReturnSeqQty += Math.Abs(seq.ReturnQuantity);
                    }

                    if (ReturnSeqQty < Math.Abs(Detail.Quantity))
                    {
                        MessageBox.Show("收料负数产生的退料数量出错！");
                    }
                }
            }

            #endregion

            return master;
        }

        /// <summary>
        /// 更新收料单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public MaterialCollectionMaster UpdateMaterialCollectionMaster(MaterialCollectionMaster obj, IList list_DeleteMatCollDtl)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            #region 1.处理收料数量为负数：按照退料处理(生成退料时序)
            //修改前的退料时序表
            IList list_seq = new ArrayList();
            foreach (MaterialCollectionDetail detail in obj.Details)
            {
                foreach (MaterialReturnDetailSeq seq in detail.MaterialReturnDetailSeqs)
                {
                    list_seq.Add(seq);
                }
            }
            #region 1.删除一条或者多条明细：针对收料数量为负数的明细,然后根据退料的时序表更新台账记录
            if (list_DeleteMatCollDtl.Count > 0)
            {
                foreach (MaterialCollectionDetail MaterialCollectionDetail in list_DeleteMatCollDtl)
                {
                    foreach (MaterialReturnDetailSeq seq in MaterialCollectionDetail.MaterialReturnDetailSeqs)
                    {
                        MaterialRentalLedger MaterialRentalLedger = GetMaterialLedgerMasterById(seq.MatLedgerId);
                        MaterialRentalLedger.LeftQuantity = MaterialRentalLedger.LeftQuantity + seq.ReturnQuantity;
                        UpdateByDao(MaterialRentalLedger);
                    }
                }
            }
            #endregion

            #region 2.更新退料时序：针对收料数量为负数的明细
            //根据先进先出算法重新组织退明细料时序表
            IList list_MatRenLed = new ArrayList();

            foreach (MaterialCollectionDetail detail in obj.Details)
            {
                if (detail.Quantity < 0)
                {
                    MaterialReturnDetailSeq theMaterialReturnDetailSeq = new MaterialReturnDetailSeq();

                    MaterialReturnDetail ReturnDetail = new MaterialReturnDetail();
                    ReturnDetail.MaterialResource = detail.MaterialResource;
                    ReturnDetail.ExitQuantity = Math.Abs(detail.Quantity);

                    IList list = GetMatLeftQuantityByModify(ReturnDetail, obj.TheSupplierRelationInfo, detail.BorrowUnit, obj.ProjectId);
                    foreach (MaterialRentalLedger MaterialRentalLedger in list)
                    {
                        theMaterialReturnDetailSeq = new MaterialReturnDetailSeq();
                        //判断当前台账的类型：0：收料；1：退料（退料是负数时产生的收料数量参数到先进先出的退料算法中）
                        if (MaterialRentalLedger.WashType == 0)
                        {
                            theMaterialReturnDetailSeq.MatCollDtlId = MaterialRentalLedger.BillDetailId;
                        }
                        else
                        {
                            theMaterialReturnDetailSeq.MatReturnDtlId = MaterialRentalLedger.BillDetailId;
                        }
                        theMaterialReturnDetailSeq.SeqType = "收料(数量小于0)";
                        theMaterialReturnDetailSeq.MatCollDtlId = MaterialRentalLedger.BillDetailId;
                        theMaterialReturnDetailSeq.MatLedgerId = MaterialRentalLedger.Id;
                        theMaterialReturnDetailSeq.ReturnQuantity = MaterialRentalLedger.TempQuantity;
                        theMaterialReturnDetailSeq.ReturnDate = obj.CreateDate;

                        //收退料单号和收退料明细数量
                        theMaterialReturnDetailSeq.MatCollCode = obj.Code;
                        theMaterialReturnDetailSeq.MatCollDtlQty = detail.Quantity;
                        theMaterialReturnDetailSeq.MatReturnCode = MaterialRentalLedger.BillCode;
                        theMaterialReturnDetailSeq.MatReturnDtlQty = MaterialRentalLedger.ReturnQuantity;

                        list_MatRenLed.Add(MaterialRentalLedger);

                        detail.AddMatReturnDtlSeq(theMaterialReturnDetailSeq);
                    }
                }
            }

            //清除原来的退料明细时序
            foreach (MaterialReturnDetailSeq seq in list_seq)
            {
                foreach (MaterialCollectionDetail detail in obj.Details)
                {
                    detail.MaterialReturnDetailSeqs.Remove(seq);
                }
            }

            //更新台账(收料剩余数量)
            foreach (MaterialRentalLedger MaterialRentalLedger in list_MatRenLed)
            {
                UpdateByDao(MaterialRentalLedger);
            }
            MaterialCollectionMaster master = UpdateByDao(obj) as MaterialCollectionMaster;


            #endregion

            #endregion

            #region 2.处理台账

            //处理1:获取料具台账信息
            MaterialRentalLedger theMaterialRentalLedger = null;
            list_MatRenLed = new ArrayList();
            list_MatRenLed = this.GetMatRentalLedgerByMatReturnCollId(master.Id);

            //处理2:收料没有，台账有
            IList notExistList = new ArrayList();

            if (list_MatRenLed != null)
            {
                foreach (MaterialRentalLedger materialRentalLedger in list_MatRenLed)
                {
                    bool isExist = false;
                    foreach (MaterialCollectionDetail detail in master.Details)
                    {
                        if (materialRentalLedger.BillDetailId == detail.Id)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (isExist == false)
                    {
                        notExistList.Add(materialRentalLedger);
                    }
                }
            }

            //处理3:删除(修改时删除一条或者多条收料明细)
            foreach (MaterialRentalLedger MaterialRentalLedger in notExistList)
            {
                DeleteByDao(MaterialRentalLedger);
            }
            //处理4：更新台账信息
            foreach (MaterialCollectionDetail detail in master.Details)
            {
                int flag = 1;//1:收料有,台账没有(新增一条台账信息) 3:收料有，台账有(更新该条台账信息)
                foreach (MaterialRentalLedger materialRentalLedger in list_MatRenLed)
                {
                    if (detail.Id == materialRentalLedger.BillDetailId)
                    {
                        flag = 3;
                        theMaterialRentalLedger = materialRentalLedger;
                        break;
                    }
                }
                if (flag == 1)
                {
                    theMaterialRentalLedger = new MaterialRentalLedger();
                }

                theMaterialRentalLedger.TheSupplierRelationInfo = master.TheSupplierRelationInfo;
                theMaterialRentalLedger.SupplierName = master.SupplierName;
                theMaterialRentalLedger.TheRank = detail.BorrowUnit;
                theMaterialRentalLedger.TheRankName = detail.BorrowUnitName;
                theMaterialRentalLedger.ProjectId = master.ProjectId;
                theMaterialRentalLedger.ProjectName = master.ProjectName;
                theMaterialRentalLedger.WashType = 0;//收料
                theMaterialRentalLedger.BillCode = master.Code;
                theMaterialRentalLedger.BillId = master.Id;
                theMaterialRentalLedger.OldContractNum = master.OldContractNum;
                theMaterialRentalLedger.SystemDate = DateTime.Now;
                theMaterialRentalLedger.RealOperationDate = master.CreateDate;
                theMaterialRentalLedger.BillDetailId = detail.Id;
                theMaterialRentalLedger.CollectionQuantity = detail.Quantity;
                theMaterialRentalLedger.LeftQuantity = detail.Quantity;
                theMaterialRentalLedger.MaterialResource = detail.MaterialResource;
                theMaterialRentalLedger.MaterialCode = detail.MaterialCode;
                theMaterialRentalLedger.MaterialName = detail.MaterialName;
                theMaterialRentalLedger.MaterialSpec = detail.MaterialSpec;
                theMaterialRentalLedger.MatStandardUnit = detail.MatStandardUnit;
                theMaterialRentalLedger.MatStandardUnitName = detail.MatStandardUnitName;
                theMaterialRentalLedger.UsedPart = detail.UsedPart;
                theMaterialRentalLedger.UsedPartName = detail.UsedPartName;
                theMaterialRentalLedger.RentalPrice = detail.RentalPrice;

                SaveOrUpdateByDao(theMaterialRentalLedger);
            }
            #endregion

            #region 3.校验退料明细数量是否退料时序的数量(和)相等

            foreach (MaterialCollectionDetail Detail in master.Details)
            {
                if (Detail.Quantity < 0)
                {
                    decimal ReturnSeqQty = 0;
                    foreach (MaterialReturnDetailSeq seq in Detail.MaterialReturnDetailSeqs)
                    {
                        ReturnSeqQty += Math.Abs(seq.ReturnQuantity);
                    }

                    if (ReturnSeqQty < Math.Abs(Detail.Quantity))
                    {
                        MessageBox.Show("收料负数产生的退料数量出错！");
                    }
                }
            }

            #endregion
            return master;
        }

        [TransManager]
        public bool DeleteMaterialCollectionMaster(MaterialCollectionMaster obj)
        {
            try
            {
                //删除台账信息
                foreach (MaterialCollectionDetail detail in obj.Details)
                {
                    MaterialRentalLedger master = this.GetMatRentalLedgerByMatReturnCollDtlId(detail.Id);
                    if (master != null)
                        DeleteByDao(master);
                }
                //更新台账剩余数量
                foreach (MaterialCollectionDetail theMaterialCollectionDetail in obj.Details)
                {
                    foreach (MaterialReturnDetailSeq theMaterialReturnDetailSeq in theMaterialCollectionDetail.MaterialReturnDetailSeqs)
                    {
                        MaterialRentalLedger master = this.GetMaterialLedgerMasterById(theMaterialReturnDetailSeq.MatLedgerId);
                        if (master != null)
                        {
                            master.LeftQuantity = master.LeftQuantity + theMaterialReturnDetailSeq.ReturnQuantity;
                            UpdateByDao(master);
                        }
                    }
                }
                DeleteByDao(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// 通过ID查询料具收料单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaterialCollectionMaster GetMaterialCollectionMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialCollectionMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialCollectionMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询料具收料单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MaterialCollectionMaster GetMaterialCollectionMasterByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetMaterialCollectionMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialCollectionMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询料具收料单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialCollectionMaster(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("MatNotQtyCosts", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatCostDtls", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.BorrowUnit", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialReturnDetailSeqs", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialCollectionMaster), objectQuery);
        }

        /// <summary>
        /// 料具收料单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet MaterialCollectionMasterQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.code,t1.PrintTimes,t1.OldContractNum,t1.SupplierName,t2.borrowunitname TheRankName,t1.STATE,t1.Descript,t2.MaterialCode,t2.MaterialName,t1.RealOperationDate,
                t2.MaterialSpec,t2.MatStandardUnitName,t2.Quantity,t2.RentalPrice,t1.BalRule,t1.CreateDate,t1.CreatePersonName,t1.transportcharge,t2.usedpartname,t2.subjectname 
                FROM THD_MaterialCollectionMaster t1 Inner join THD_MaterialCollectionDetail t2 on t1.id=t2.parentid";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        #endregion

        #region 料具退料单方法
        /// <summary>
        /// 通过ID查询料具退料单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaterialReturnMaster GetMaterialReturnById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialReturnMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialReturnMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询料具退料单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MaterialReturnMaster GetMaterialReturnByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetMaterialReturnMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialReturnMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询料具退料单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialReturnMaster(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("MatReturnNotQtyCosts", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatReturnCostDtls", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatRepairs", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialReturnDetailSeqs", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialReturnMaster), objectQuery);
        }

        /// <summary>
        /// 料具退料单保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public MaterialReturnMaster SaveMaterialReturnMaster(MaterialReturnMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(MaterialReturnMaster), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            //1.根据先进先出算法组织退料时序表
            IList list_MatRenLed = new ArrayList();
            foreach (MaterialReturnDetail detail in obj.Details)
            {
                if (detail.ExitQuantity > 0)
                {
                    MaterialReturnDetailSeq theMaterialReturnDetailSeq = null;

                    IList list = GetMatLeftQuantityByNew(detail, obj.TheSupplierRelationInfo, detail.BorrowUnit, obj.ProjectId);
                    foreach (MaterialRentalLedger theMaterialRentalLedger in list)
                    {
                        theMaterialReturnDetailSeq = new MaterialReturnDetailSeq();
                        //判断当前台账的类型：0：收料；1：退料(退料是负数时产生的收料数量参数到先进先出的退料算法中)
                        if (theMaterialRentalLedger.WashType == 0)
                        {
                            theMaterialReturnDetailSeq.MatCollDtlId = theMaterialRentalLedger.BillDetailId;
                        }
                        else
                        {
                            theMaterialReturnDetailSeq.MatReturnDtlId = theMaterialRentalLedger.BillDetailId;
                        }
                        theMaterialReturnDetailSeq.SeqType = "退料";
                        theMaterialReturnDetailSeq.MatLedgerId = theMaterialRentalLedger.Id;
                        theMaterialReturnDetailSeq.ReturnQuantity = theMaterialRentalLedger.TempQuantity;
                        theMaterialReturnDetailSeq.ReturnDate = obj.CreateDate;
                        //收退料单号和收退料明细数量
                        theMaterialReturnDetailSeq.MatReturnCode = obj.Code;
                        theMaterialReturnDetailSeq.MatReturnDtlQty = detail.ExitQuantity;
                        theMaterialReturnDetailSeq.MatCollCode = theMaterialRentalLedger.BillCode;
                        theMaterialReturnDetailSeq.MatCollDtlQty = theMaterialRentalLedger.CollectionQuantity;
                        list_MatRenLed.Add(theMaterialRentalLedger);

                        detail.AddMatReturnDtlSeq(theMaterialReturnDetailSeq);
                    }
                }
            }
            MaterialReturnMaster master = SaveByDao(obj) as MaterialReturnMaster;
            //更新台账(收料剩余数量)
            foreach (MaterialRentalLedger MaterialRentalLedger in list_MatRenLed)
            {
                UpdateByDao(MaterialRentalLedger);
            }

            //2.添加台账信息(退料)
            foreach (MaterialReturnDetail detail in master.Details)
            {
                MaterialRentalLedger theMaterialRentalLedger = new MaterialRentalLedger();
                theMaterialRentalLedger.TheSupplierRelationInfo = master.TheSupplierRelationInfo;
                theMaterialRentalLedger.SupplierName = master.SupplierName;
                theMaterialRentalLedger.ProjectId = master.ProjectId;
                theMaterialRentalLedger.ProjectName = master.ProjectName;
                theMaterialRentalLedger.WashType = 1;//退料
                theMaterialRentalLedger.BillCode = master.Code;
                theMaterialRentalLedger.BillId = master.Id;
                theMaterialRentalLedger.OldContractNum = master.OldContractNum;
                theMaterialRentalLedger.SystemDate = DateTime.Now;
                theMaterialRentalLedger.RealOperationDate = master.CreateDate;
                theMaterialRentalLedger.TheRank = detail.BorrowUnit;
                theMaterialRentalLedger.TheRankName = detail.BorrowUnitName;
                theMaterialRentalLedger.BillDetailId = detail.Id;
                theMaterialRentalLedger.CollectionQuantity = detail.Quantity;
                theMaterialRentalLedger.MaterialResource = detail.MaterialResource;
                theMaterialRentalLedger.MaterialCode = detail.MaterialCode;
                theMaterialRentalLedger.MaterialName = detail.MaterialName;
                theMaterialRentalLedger.MaterialSpec = detail.MaterialSpec;
                theMaterialRentalLedger.MatStandardUnit = detail.MatStandardUnit;
                theMaterialRentalLedger.MatStandardUnitName = detail.MatStandardUnitName;
                theMaterialRentalLedger.UsedPart = detail.UsedPart;
                theMaterialRentalLedger.UsedPartName = detail.UsedPartName;
                theMaterialRentalLedger.RentalPrice = detail.RentalPrice;
                theMaterialRentalLedger.SubjectGUID = detail.SubjectGUID;
                theMaterialRentalLedger.SubjectName = detail.SubjectName;
                theMaterialRentalLedger.SubjectSysCode = detail.SubjectSysCode;
                //退料为负数：说明为收料
                decimal ExitQuantity = detail.ExitQuantity;
                if (ExitQuantity < 0)
                {
                    theMaterialRentalLedger.LeftQuantity = Math.Abs(ExitQuantity);
                }
                theMaterialRentalLedger.ReturnQuantity = detail.ExitQuantity;

                SaveByDao(theMaterialRentalLedger);
            }

            #region 校验退料明细数量是否退料时序的数量(和)相等

            foreach (MaterialReturnDetail Detail in master.Details)
            {
                if (Detail.ExitQuantity > 0)
                {
                    decimal ReturnSeqQty = 0;
                    foreach (MaterialReturnDetailSeq seq in Detail.MaterialReturnDetailSeqs)
                    {
                        ReturnSeqQty += Math.Abs(seq.ReturnQuantity);
                    }

                    if (ReturnSeqQty < Detail.ExitQuantity)
                    {
                        throw new Exception("退料数量出错！");
                    }
                }
            }

            #endregion
            return master;
        }

        /// <summary>
        /// 更新退料单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="oldObj">修改前</param>
        /// <returns></returns>
        [TransManager]
        public MaterialReturnMaster UpdateMaterialReturnMaster(MaterialReturnMaster obj)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {

                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            IList list_seq = new ArrayList();
            //修改前的退料时序表
            foreach (MaterialReturnDetail detail in obj.Details)
            {
                foreach (MaterialReturnDetailSeq seq in detail.MaterialReturnDetailSeqs)
                {
                    list_seq.Add(seq);
                }
            }
            //删除一条或者多条明细
            IList list_MatReturnDtl = new ArrayList();
            foreach (MaterialReturnDetail MaterialReturnDetail in obj.Details)
            {
                if (MaterialReturnDetail.TempData == "删除")
                {
                    foreach (MaterialReturnDetailSeq seq in MaterialReturnDetail.MaterialReturnDetailSeqs)
                    {
                        MaterialRentalLedger MaterialRentalLedger = GetMaterialLedgerMasterById(seq.MatLedgerId);
                        MaterialRentalLedger.LeftQuantity = MaterialRentalLedger.LeftQuantity + seq.ReturnQuantity;
                        UpdateByDao(MaterialRentalLedger);
                        list_MatReturnDtl.Add(MaterialReturnDetail);
                    }
                }
            }
            //清除界面上删除的明细
            foreach (MaterialReturnDetail detail in list_MatReturnDtl)
            {
                obj.Details.Remove(detail);
            }

            //根据先进先出算法重新组织退明细料时序表
            IList list_MatRenLed = new ArrayList();
            foreach (MaterialReturnDetail detail in obj.Details)
            {
                if (detail.ExitQuantity > 0)
                {
                    MaterialReturnDetailSeq theMaterialReturnDetailSeq = null;

                    IList list = GetMatLeftQuantityByModify(detail, obj.TheSupplierRelationInfo, detail.BorrowUnit, obj.ProjectId);
                    foreach (MaterialRentalLedger MaterialRentalLedger in list)
                    {
                        theMaterialReturnDetailSeq = new MaterialReturnDetailSeq();
                        //判断当前台账的类型：0：收料；1：退料（退料是负数时产生的收料数量参数到先进先出的退料算法中）
                        if (MaterialRentalLedger.WashType == 0)
                        {
                            theMaterialReturnDetailSeq.MatCollDtlId = MaterialRentalLedger.BillDetailId;
                        }
                        else
                        {
                            theMaterialReturnDetailSeq.MatReturnDtlId = MaterialRentalLedger.BillDetailId;
                        }
                        theMaterialReturnDetailSeq.SeqType = "退料";
                        theMaterialReturnDetailSeq.MatCollDtlId = MaterialRentalLedger.BillDetailId;
                        theMaterialReturnDetailSeq.MatLedgerId = MaterialRentalLedger.Id;
                        theMaterialReturnDetailSeq.ReturnQuantity = MaterialRentalLedger.TempQuantity;
                        theMaterialReturnDetailSeq.ReturnDate = obj.CreateDate;

                        //收退料单号和收退料明细数量
                        theMaterialReturnDetailSeq.MatReturnCode = obj.Code;
                        theMaterialReturnDetailSeq.MatReturnDtlQty = detail.ExitQuantity;
                        theMaterialReturnDetailSeq.MatCollCode = MaterialRentalLedger.BillCode;
                        theMaterialReturnDetailSeq.MatCollDtlQty = MaterialRentalLedger.CollectionQuantity;

                        list_MatRenLed.Add(MaterialRentalLedger);

                        detail.AddMatReturnDtlSeq(theMaterialReturnDetailSeq);
                    }
                }
            }

            //清除原来的退料明细时序
            foreach (MaterialReturnDetailSeq seq in list_seq)
            {
                foreach (MaterialReturnDetail detail in obj.Details)
                {
                    detail.MaterialReturnDetailSeqs.Remove(seq);
                }
            }

            //更新台账(收料剩余数量)
            foreach (MaterialRentalLedger MaterialRentalLedger in list_MatRenLed)
            {
                UpdateByDao(MaterialRentalLedger);
            }

            MaterialReturnMaster master = UpdateByDao(obj) as MaterialReturnMaster;

            //处理料具台账
            MaterialRentalLedger theMaterialRentalLedger = null;
            IList list_temp = new ArrayList();

            list_temp = this.GetMatRentalLedgerByMatReturnCollId(master.Id);

            //处理2:退料没有，台账有
            IList notExistList = new ArrayList();

            if (list_MatRenLed != null)
            {
                foreach (MaterialRentalLedger materialRentalLedger in list_temp)
                {
                    bool isExist = false;
                    foreach (MaterialReturnDetail detail in master.Details)
                    {
                        if (materialRentalLedger.BillDetailId == detail.Id)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (isExist == false)
                    {
                        notExistList.Add(materialRentalLedger);
                    }
                }
            }

            //删除
            foreach (MaterialRentalLedger MaterialRentalLedger in notExistList)
            {
                DeleteByDao(MaterialRentalLedger);
            }
            //更新台账
            foreach (MaterialReturnDetail detail in master.Details)
            {
                int flag = 1;//1:退料有,台账没有 3:退料有，台账有
                foreach (MaterialRentalLedger materialRentalLedger in list_temp)
                {
                    if (detail.Id == materialRentalLedger.BillDetailId)
                    {
                        flag = 3;
                        theMaterialRentalLedger = materialRentalLedger;
                        break;
                    }
                }
                if (flag == 1)
                {
                    theMaterialRentalLedger = new MaterialRentalLedger();
                }

                theMaterialRentalLedger.TheSupplierRelationInfo = master.TheSupplierRelationInfo;
                theMaterialRentalLedger.SupplierName = master.SupplierName;
                theMaterialRentalLedger.ProjectId = master.ProjectId;
                theMaterialRentalLedger.ProjectName = master.ProjectName;
                theMaterialRentalLedger.WashType = 1;//退料
                theMaterialRentalLedger.BillCode = master.Code;
                theMaterialRentalLedger.BillId = master.Id;
                theMaterialRentalLedger.OldContractNum = master.OldContractNum;
                theMaterialRentalLedger.SystemDate = DateTime.Now;
                theMaterialRentalLedger.RealOperationDate = master.CreateDate;
                theMaterialRentalLedger.TheRank = detail.BorrowUnit;
                theMaterialRentalLedger.TheRankName = detail.BorrowUnitName;
                theMaterialRentalLedger.BillDetailId = detail.Id;
                theMaterialRentalLedger.MaterialResource = detail.MaterialResource;
                theMaterialRentalLedger.MaterialCode = detail.MaterialCode;
                theMaterialRentalLedger.MaterialName = detail.MaterialName;
                theMaterialRentalLedger.MaterialSpec = detail.MaterialSpec;
                theMaterialRentalLedger.MatStandardUnit = detail.MatStandardUnit;
                theMaterialRentalLedger.MatStandardUnitName = detail.MatStandardUnitName;
                theMaterialRentalLedger.UsedPart = detail.UsedPart;
                theMaterialRentalLedger.UsedPartName = detail.UsedPartName;
                theMaterialRentalLedger.RentalPrice = detail.RentalPrice;
                //退料为负数：说明为收料
                decimal ExitQuantity = detail.ExitQuantity;
                if (ExitQuantity < 0)
                {
                    theMaterialRentalLedger.LeftQuantity = Math.Abs(ExitQuantity);
                }
                theMaterialRentalLedger.ReturnQuantity = detail.ExitQuantity;

                SaveOrUpdateByDao(theMaterialRentalLedger);
            }

            #region 校验退料明细数量是否退料时序的数量(和)相等

            foreach (MaterialReturnDetail Detail in master.Details)
            {
                if (Detail.ExitQuantity > 0)
                {
                    decimal ReturnSeqQty = 0;
                    foreach (MaterialReturnDetailSeq seq in Detail.MaterialReturnDetailSeqs)
                    {
                        ReturnSeqQty += Math.Abs(seq.ReturnQuantity);
                    }

                    if (ReturnSeqQty < Detail.ExitQuantity)
                    {
                        MessageBox.Show("退料数量出错！");
                    }
                }
            }

            #endregion
            return master;
        }

        /// <summary>
        /// 删除退料单同时更新台账剩余数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteMaterialReturn(MaterialReturnMaster obj)
        {
            try
            {
                //删除台账信息
                foreach (MaterialReturnDetail detail in obj.Details)
                {
                    MaterialRentalLedger master = this.GetMatRentalLedgerByMatReturnCollDtlId(detail.Id);
                    if (master != null)
                        DeleteByDao(master);
                }
                //更新台账剩余数量
                foreach (MaterialReturnDetail theMaterialReturnDetail in obj.Details)
                {
                    foreach (MaterialReturnDetailSeq theMaterialReturnDetailSeq in theMaterialReturnDetail.MaterialReturnDetailSeqs)
                    {
                        MaterialRentalLedger master = this.GetMaterialLedgerMasterById(theMaterialReturnDetailSeq.MatLedgerId);
                        if (master != null)
                        {
                            master.LeftQuantity = master.LeftQuantity + theMaterialReturnDetailSeq.ReturnQuantity;
                            UpdateByDao(master);
                        }
                    }
                }
                DeleteByDao(obj);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// 料具退料单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet GetMaterialReturnMasterQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t1.OldContractNum,t1.SupplierName,t1.PrintTimes,t1.Descript,t2.BorrowUnitName TheRankName,t1.SumExitQuantity,t1.State,t2.MaterialCode,t2.MaterialName,
                t2.MaterialSpec,t2.MatStandardUnitName,t2.RentalPrice,t2.RejectQuantity,t2.BalRule,t2.BroachQuantity,t2.subjectname,
                t1.CreateDate,t1.RealOperationDate,t1.CreatePersonName,t1.transportcharge,
                t2.ConsumeQuantity,t2.ProjectQuantity,t2.discardqty,t2.repairqty,t2.lossqty,t2.ExitQuantity,t2.UsedPartName
                FROM THD_MaterialReturnMaster  t1 INNER JOIN THD_MaterialReturnDetail t2 ON t2.ParentId=t1.Id";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        /// <summary>
        /// 校验库存(退料)
        /// </summary>
        /// <param name="master">退料主表</param>
        /// <param name="CD"></param>
        /// <param name="material">是否是插入的退料单</param>
        /// <returns>0：通过 1：当前库存不足 2: 插入业务日期的库存不足 3:插入该笔退料后，业务日期[yyyy-MM-dd]的库存为负[yyyy-MM-dd]是计算中的第一笔负数的日期</returns>
        public DataDomain VerifyReturnMatKC(MaterialReturnMaster Master, bool CD)
        {
            DataDomain ReturnDoamain = new DataDomain();

            decimal HistoryJC = 0;
            decimal CurrentJC = 0;
            //拼接查询条件
            foreach (MaterialReturnDetail MaterialReturnDetail in Master.Details)
            {
                if (MaterialReturnDetail.ExitQuantity > 0)
                {
                    string CurrJCcondition = "ProjectId='" + Master.ProjectId + "' and SupplierRelation='" + Master.TheSupplierRelationInfo.Id.ToString() + "' AND TheRank='" + MaterialReturnDetail.BorrowUnit.Id.ToString() + "' AND Material='" + MaterialReturnDetail.MaterialResource.Id.ToString() + "'";
                    CurrentJC = GetCurrentJC(CurrJCcondition);

                    string HisJCcondition = "ProjectId='" + Master.ProjectId + "' and SupplierRelation='" + Master.TheSupplierRelationInfo.Id.ToString() + "' AND TheRank='" + MaterialReturnDetail.BorrowUnit.Id.ToString() + "' AND Material='" + MaterialReturnDetail.MaterialResource.Id.ToString() + "' AND RealOperationDate<=to_date('" + Master.CreateDate.Date.ToShortDateString() + "','yyyy-mm-dd')";
                    HistoryJC = GetHistoryJC(HisJCcondition);
                    if (CD == true)
                    {
                        if (MaterialReturnDetail.ExitQuantity - TransUtil.ToDecimal(MaterialReturnDetail.TempData) - HistoryJC > 0)
                        {
                            //插入业务日期的库存不足
                            ReturnDoamain.Name1 = 2;
                            ReturnDoamain.Name2 = "插入业务日期的[" + MaterialReturnDetail.MaterialName + "]库存不足";
                        }
                        if (MaterialReturnDetail.ExitQuantity - TransUtil.ToDecimal(MaterialReturnDetail.TempData) - HistoryJC < 0)
                        {
                            //判断插入该笔退料后,后边的结存情况
                            string condition = "ProjectId='" + Master.ProjectId + "' and SupplierRelation='" + Master.TheSupplierRelationInfo.Id.ToString() + "' AND TheRank='" + MaterialReturnDetail.BorrowUnit.Id.ToString() + "' AND Material='" + MaterialReturnDetail.MaterialResource.Id.ToString() + "' AND RealOperationDate> to_date('" + Master.CreateDate.Date.ToShortDateString() + "','yyyy-mm-dd')";
                            DataSet ds = GetMaterialRentalLedgerByCondition(condition);
                            DataTable table = ds.Tables[0];
                            decimal tempJC = 0;
                            tempJC = HistoryJC - MaterialReturnDetail.ExitQuantity;
                            string BusinessDate = "";
                            int tempCount = 0;
                            int WashType = 0;//0:收料 1：退料
                            foreach (DataRow row in table.Rows)
                            {
                                WashType = TransUtil.ToInt(row["WashType"]);
                                if (WashType == 0)
                                {
                                    //收料
                                    tempJC = tempJC + TransUtil.ToDecimal(row["CollectionQuantity"]);
                                }
                                else if (WashType == 1)
                                {
                                    //收料
                                    tempJC = tempJC - TransUtil.ToDecimal(row["ReturnQuantity"]);
                                }

                                //判断结存数量是否小于0：如果小于0循环结束
                                if (tempJC < 0)
                                {
                                    tempCount++;
                                    BusinessDate = row["CreateDate"].ToString();
                                    break;
                                }
                            }
                            if (tempCount == 0)
                            {
                                ReturnDoamain.Name1 = 0;
                                ReturnDoamain.Name2 = "通过";
                            }
                            else if (tempCount == 1)
                            {
                                ReturnDoamain.Name1 = 3;
                                ReturnDoamain.Name2 = "插入该笔退料后，业务日期[" + BusinessDate + "]的[" + MaterialReturnDetail.MaterialName + "]库存为负";
                            }
                        }
                    }
                    else
                    {
                        //正常退料单：只判断当前退料数量是否大于当前库存
                        if (MaterialReturnDetail.ExitQuantity - TransUtil.ToDecimal(MaterialReturnDetail.TempData) - CurrentJC > 0)
                        {
                            //当前库存不足
                            ReturnDoamain.Name1 = 1;
                            ReturnDoamain.Name2 = "[" + MaterialReturnDetail.MaterialName + "]当前库存不足";
                        }
                        else
                        {
                            //通过
                            ReturnDoamain.Name1 = 0;
                            ReturnDoamain.Name2 = "通过";
                        }
                    }
                }
            }
            return ReturnDoamain;
        }


        /// <summary>
        /// 获取当前结存
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private decimal GetCurrentJC(string condition)
        {
            decimal CurrentJC = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sqlCurrentJC =
                @"select sum(LeftQuantity) SumJC from THD_MaterialRentalLedger where " + condition + "";
            command.CommandText = sqlCurrentJC;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dataTable = dataSet.Tables[0] as DataTable;
            foreach (DataRow row in dataTable.Rows)
            {
                CurrentJC = TransUtil.ToDecimal(row["SumJC"]);
            }
            return CurrentJC;
        }
        /// <summary>
        /// 获取历史结存
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private decimal GetHistoryJC(string condition)
        {
            decimal HistoryJC = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sqlCurrentJC =
                @"select sum(collectionquantity)-sum(returnquantity) SumJC from THD_MaterialRentalLedger where " + condition + "";
            command.CommandText = sqlCurrentJC;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dataTable = dataSet.Tables[0] as DataTable;
            foreach (DataRow row in dataTable.Rows)
            {
                HistoryJC = TransUtil.ToDecimal(row["SumJC"]);
            }
            return HistoryJC;
        }
        /// <summary>
        /// 判断当前退料单是否是插入单据
        /// </summary>
        /// <param name="BusinessDate"></param>
        /// <returns></returns>
        public bool VerifyReturnMatBusinessDate(DateTime BusinessDate, string projectId)
        {
            bool value = true;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"select Id  from THD_MaterialReturnMaster where projectid='" + projectId + "' and CreateDate>= to_date('" + BusinessDate.AddDays(1).ToShortDateString() + "','yyyy-mm-dd')";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dataTable = dataSet.Tables[0] as DataTable;
            if (dataTable.Rows.Count > 0)
            {
                value = true;
            }
            else
            {
                value = false;
            }
            return value;
        }

        /// <summary>
        /// 判断当前收料单是否是插入单据
        /// </summary>
        /// <param name="BusinessDate"></param>
        /// <returns></returns>
        public bool VerifyCollMatBusinessDate(DateTime BusinessDate, string projectId)
        {
            bool value = true;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"select Id  from THD_MaterialCollectionMaster where projectid='" + projectId + "' and CreateDate>= to_date('" + BusinessDate.AddDays(1).ToShortDateString() + "','yyyy-mm-dd')";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dataTable = dataSet.Tables[0] as DataTable;
            if (dataTable.Rows.Count > 0)
            {
                value = true;
            }
            else
            {
                value = false;
            }
            return value;
        }
        #endregion

        #region 料具租赁台账方法
        /// <summary>
        /// 料具台账保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MaterialRentalLedger SaveMaterialRentalLedger(MaterialRentalLedger obj)
        {
            return SaveOrUpdateByDao(obj) as MaterialRentalLedger;
        }

        /// <summary>
        /// 通过收退料单明细Id查询料具台账
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaterialRentalLedger GetMatRentalLedgerByMatReturnCollDtlId(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("BillDetailId", id));
            IList list = GetMaterialRentalLedger(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialRentalLedger;
            }
            return null;
        }

        /// <summary>
        /// 通过收退料单Id查询料具台账
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetMatRentalLedgerByMatReturnCollId(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("BillId", id));
            IList list = GetMaterialRentalLedger(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list;
            }
            return null;
        }

        /// <summary>
        /// 查询料具台账信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialRentalLedger(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheRank", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheRank.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("SubjectGUID", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("UsedPart", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialRentalLedger), objectQuery);
        }

        /// <summary>
        /// 通过ID查询台账
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaterialRentalLedger GetMaterialLedgerMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialRentalLedger(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialRentalLedger;
            }
            return null;
        }

        /// <summary>
        /// 根据条件查询台账信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet GetMaterialRentalLedgerByCondition(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                 @"SELECT WashType,OldContractNum,BillCode,Material, MaterialCode, MaterialName,
                   MaterialSpec, CollectionQuantity, LeftQuantity, ReturnQuantity, SupplierName,
                   MatStandardUnitName,SystemDate, RealOperationDate, RentalPrice, TheRankName
                   FROM THD_MaterialRentalLedger";
            sql += " where 1=1 and " + condition + " ORDER BY SystemDate";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        public IList GetMaterialRentalLedger(string condition, DateTime BeginDate, string projectId)
        {
            IList resultList = new ArrayList();
            Hashtable hs_table = new Hashtable();
            Hashtable jc_table = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT
                           t3.BalState AS  CollBalState,
                           t4.BalState AS ReturnBalState,
                           t1.WashType,
                           t1.OldContractNum,
                           t1.BillCode,
                           t1.Material,
                           t1.MaterialCode,
                           t1.MaterialName,
                           t1.MaterialSpec,
                           t1.CollectionQuantity,
                           t1.LeftQuantity,
                           t1.ReturnQuantity,
                           t1.SupplierName,
                           t1.MatStandardUnitName,
                           t1.SystemDate,
                           t1.RealOperationDate,
                           t1.RentalPrice,
                           t1.TheRankName,
                           t2.BroachQuantity,
                           t2.RejectQuantity,
                           t2.ConsumeQuantity,
                           t2.LossQty,
                           t2.DisCardQty,
                           t2.RepairQty
                           FROM   THD_MaterialRentalLedger t1 LEFT JOIN
                           THD_MaterialReturnDetail t2 ON t2.Id=t1.BillDetailId
                           LEFT JOIN THD_MaterialCollectionMaster t3 ON t1.BillId=t3.Id
                           LEFT JOIN  thd_MaterialReturnMaster t4 ON t1.BillId=t4.Id";
            sql += " where 1=1 " + condition + " ORDER BY t1.RealOperationDate ,t1.SupplierRelation ,t1.TheRank ,t1.Material,t1.SystemDate ASC";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable table = dataSet.Tables[0];
            if (table.Rows.Count > 0)
            {
                foreach (DataRow var in table.Rows)
                {
                    DataDomain domain = new DataDomain();
                    if (var["WashType"].ToString() == "0")
                    {
                        domain.Name1 = "收料";
                        domain.Name23 = var["CollBalState"];

                    }
                    else if (var["WashType"].ToString() == "1")
                    {
                        domain.Name1 = "退料";
                        domain.Name24 = var["ReturnBalState"];
                    }
                    domain.Name2 = var["OldContractNum"];
                    domain.Name3 = var["BillCode"];
                    domain.Name4 = var["Material"];
                    domain.Name5 = var["MaterialCode"];
                    domain.Name6 = var["MaterialName"];
                    domain.Name7 = var["MaterialSpec"];
                    domain.Name8 = var["CollectionQuantity"];
                    domain.Name9 = var["LeftQuantity"];
                    domain.Name10 = var["ReturnQuantity"];
                    domain.Name11 = var["SupplierName"];
                    domain.Name12 = var["MatStandardUnitName"];
                    domain.Name13 = var["SystemDate"];
                    domain.Name14 = var["RealOperationDate"];
                    domain.Name15 = var["RentalPrice"];
                    domain.Name16 = var["TheRankName"];
                    domain.Name17 = var["BroachQuantity"];
                    domain.Name18 = var["RejectQuantity"];
                    domain.Name19 = var["ConsumeQuantity"];
                    domain.Name20 = var["LossQty"];
                    domain.Name21 = var["DisCardQty"];
                    domain.Name22 = var["RepairQty"];

                    if (!hs_table.Contains(domain.Name4))
                    {
                        IList list = new ArrayList();
                        //计算当前材料的在当前开始日期以前的结存，并插入一条数量
                        decimal previousJC = GetPreviousJC(BeginDate, Convert.ToString(domain.Name4), projectId);
                        DataDomain previousDomain = new DataDomain();
                        previousDomain.Name1 = "上期结存";
                        previousDomain.Name4 = domain.Name4;
                        previousDomain.Name5 = domain.Name5;
                        previousDomain.Name6 = domain.Name6;
                        previousDomain.Name7 = domain.Name7;
                        previousDomain.Name9 = previousJC;

                        list.Add(previousDomain);
                        list.Add(domain);

                        if (domain.Name1 == "收料")
                        {
                            domain.Name9 = TransUtil.ToDecimal(domain.Name8) + TransUtil.ToDecimal(previousJC);
                            jc_table.Add(domain.Name4, domain.Name9);
                        }
                        else if (domain.Name1 == "退料")
                        {
                            domain.Name9 = -TransUtil.ToDecimal(domain.Name10) + TransUtil.ToDecimal(previousJC);
                            jc_table.Add(domain.Name4, domain.Name9);
                        }
                        hs_table.Add(domain.Name4, list);
                    }
                    else
                    {
                        IList list = (ArrayList)hs_table[domain.Name4];
                        decimal tempJC = TransUtil.ToDecimal(jc_table[domain.Name4]);
                        if (domain.Name1 == "收料")
                        {
                            tempJC = tempJC + TransUtil.ToDecimal(domain.Name8);
                            domain.Name9 = tempJC;
                            jc_table.Remove(domain.Name4);
                            jc_table.Add(domain.Name4, tempJC + "");
                        }
                        else if (domain.Name1 == "退料")
                        {
                            tempJC = tempJC - TransUtil.ToDecimal(domain.Name10);
                            domain.Name9 = tempJC;
                            jc_table.Remove(domain.Name4);
                            jc_table.Add(domain.Name4, tempJC + "");
                        }
                        list.Add(domain);
                    }
                }
            }

            foreach (string keys in hs_table.Keys)
            {
                IList list = hs_table[keys] as IList;
                foreach (DataDomain domain in list)
                {
                    resultList.Add(domain);
                }
            }
            return resultList;

        }
        /// <summary>
        /// 获取该时间以前的结存数量
        /// </summary>
        /// <param name="BeginDate"></param>
        /// <returns></returns>
        private decimal GetPreviousJC(DateTime BeginDate, string MaterialId, string projectId)
        {
            decimal SumCollQty = 0;
            decimal SumReturnQty = 0;
            decimal SumJC = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string sql =
                 @"SELECT SUM(CollectionQuantity) SumCollQTy,SUM(ReturnQuantity) SumReturnQty FROM THD_MaterialRentalLedger 
                   WHERE projectid = '" + projectId + "' and RealOperationDate< to_date('" + BeginDate.ToShortDateString() + "','yyyy-mm-dd ') AND Material='" + MaterialId + "'";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable table = dataSet.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                object sumCollQty = row["SumCollQty"];
                object sumReturnQty = row["SumReturnQty"];
                if (sumCollQty != null)
                {
                    SumCollQty = ClientUtil.ToDecimal(sumCollQty);
                }
                if (sumReturnQty != null)
                {
                    SumReturnQty = ClientUtil.ToDecimal(sumReturnQty);
                }
            }
            SumJC = SumCollQty - SumReturnQty;
            return SumJC;
        }

        /// <summary>
        /// 先进先出算法(新建退料单使用)
        /// </summary>
        public IList GetMatLeftQuantityByNew(MaterialReturnDetail detail, SupplierRelationInfo Supplier, SupplierRelationInfo rank, string projectId)
        {
            IList list_MatRenLedMaster = new ArrayList();
            //退料数量
            decimal exitQuantity = detail.ExitQuantity;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", Supplier));
            oq.AddCriterion(Expression.Eq("TheRank", rank));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            oq.AddCriterion(Expression.Eq("MaterialResource", detail.MaterialResource));
            oq.AddCriterion(Expression.Sql("LeftQuantity>0"));
            oq.AddOrder(Order.Asc("SystemDate"));
            IList list = this.GetMaterialRentalLedger(oq);

            foreach (MaterialRentalLedger master in list)
            {
                decimal tempQuantity = master.LeftQuantity;
                if (exitQuantity <= 0)
                {
                    break;
                }
                else
                {
                    if (exitQuantity - master.LeftQuantity > 0)
                    {
                        master.TempQuantity = master.LeftQuantity;
                        exitQuantity = exitQuantity - tempQuantity;
                        master.LeftQuantity = 0;
                    }
                    else if (exitQuantity - master.LeftQuantity <= 0)
                    {
                        master.TempQuantity = exitQuantity;
                        master.LeftQuantity = tempQuantity - exitQuantity;
                        exitQuantity = exitQuantity - tempQuantity;
                    }
                    list_MatRenLedMaster.Add(master);
                }
            }
            return list_MatRenLedMaster;
        }

        /// <summary>
        /// 先进先出算法(退料单修改时使用)
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        public IList GetMatLeftQuantityByModify(MaterialReturnDetail detail, SupplierRelationInfo Supplier, SupplierRelationInfo rank, string projectId)
        {
            IList list_MatRenLedMaster = new ArrayList();
            //退料数量
            decimal exitQuantity = detail.ExitQuantity;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", Supplier));
            oq.AddCriterion(Expression.Eq("TheRank", rank));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            oq.AddCriterion(Expression.Eq("MaterialResource", detail.MaterialResource));
            oq.AddOrder(Order.Asc("SystemDate"));
            IList list = this.GetMaterialRentalLedger(oq);
            //查询台账信息和当前退料时序的台账ID作对比：
            //如果有补上剩余数量再做先进先出计算
            foreach (MaterialRentalLedger theMaterialRentalLedger in list)
            {
                foreach (MaterialReturnDetailSeq seq in detail.MaterialReturnDetailSeqs)
                {
                    if (theMaterialRentalLedger.Id == seq.MatLedgerId)
                    {
                        theMaterialRentalLedger.LeftQuantity = theMaterialRentalLedger.LeftQuantity + seq.ReturnQuantity;
                    }
                }
            }
            //在补上剩余数量的基础上先进先出
            foreach (MaterialRentalLedger master in list)
            {
                decimal tempQuantity = master.LeftQuantity;
                if (exitQuantity <= 0)
                {
                    break;
                }
                else
                {
                    if (exitQuantity - master.LeftQuantity > 0)
                    {
                        master.TempQuantity = master.LeftQuantity;
                        exitQuantity = exitQuantity - tempQuantity;
                        master.LeftQuantity = 0;
                    }
                    else if (exitQuantity - master.LeftQuantity < 0)
                    {
                        master.TempQuantity = exitQuantity;
                        master.LeftQuantity = tempQuantity - exitQuantity;
                        exitQuantity = exitQuantity - tempQuantity;
                    }
                    list_MatRenLedMaster.Add(master);
                }
            }
            return list_MatRenLedMaster;
        }

        /// <summary>
        /// 删除退料单时写回台账剩余数量
        /// </summary>
        /// <param name="list"></param>
        public void UpdateMatRenLedLeftQuantityByDel(IList list)
        {
            if (list.Count > 0)
            {
                foreach (MaterialRentalLedger master in list)
                {
                    master.LeftQuantity = master.LeftQuantity + master.TempQuantity;
                    UpdateByDao(master);
                }
            }
        }

        /// <summary>
        /// 获取确定出租方，队伍，料具的库存量
        /// </summary>
        /// <param name="theSupplier"></param>
        /// <param name="theRank"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        public decimal GetMatStockQty(SupplierRelationInfo theSupplier, SupplierRelationInfo theRank, Material material, string projectId)
        {
            decimal StockQty = 0;
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("WashType", 0));//收料单
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));//料具出租方
            oq.AddCriterion(Expression.Eq("TheRank", theRank));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            oq.AddCriterion(Expression.Eq("MaterialResource", material));
            list = GetMaterialRentalLedger(oq);

            foreach (MaterialRentalLedger master in list)
            {
                StockQty = StockQty + master.LeftQuantity;
            }
            return StockQty;
        }

        #endregion

        #region 料具结算方法
        /// <summary>
        /// 根据会计年，会计月取上期料具结算
        /// </summary>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <returns></returns>
        public MaterialBalanceMaster GetMatBalanceMaster(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("FiscalYear", fiscalYear));
            oq.AddCriterion(Expression.Eq("FiscalMonth", fiscalMonth));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));

            IList list = GetMatBalanceMaster(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialBalanceMaster;
            }
            return null;
        }

        public MaterialBalanceMaster GetPrrviousMatBalanceMaster(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            fiscalYear = TransUtil.GetLastYear(fiscalYear, fiscalMonth);
            fiscalMonth = TransUtil.GetLastMonth(fiscalYear, fiscalMonth);
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("FiscalYear", fiscalYear));
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("FiscalMonth", fiscalMonth));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));
            IList list = GetMatBalanceMaster(oq);
            if (list != null && list.Count > 0)
            {

                MaterialBalanceMaster oMastr = list[0] as MaterialBalanceMaster;
                if (oMastr.Id == null)
                {
                    oMastr.RealOperationDate = DateTime.Now;
                }

                return oMastr;
            }
            return null;
        }
        /// <summary>
        /// 查询料具结算
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMatBalanceMaster(ObjectQuery objectQuery)
        {
            IList list = new ArrayList();
            objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("CreatePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MatBalOtherCostDetails", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("MatBalOtherCostDetails.TheSupplierRelationInfo", NHibernate.FetchMode.Eager);

            list = Dao.ObjectQuery(typeof(MaterialBalanceMaster), objectQuery);
            //foreach (MaterialBalanceMaster master in list)
            //{
            //    objectQuery = new ObjectQuery();
            //    objectQuery.AddCriterion(Expression.Eq("Master.Id", master.Id));
            //    IList list_other = Dao.ObjectQuery(typeof(MatBalOtherCostDetail), objectQuery);
            //    foreach (MatBalOtherCostDetail oDetail in list_other)
            //    {
            //        oDetail.Master = master;
            //        master.MatBalOtherCostDetails.Add(oDetail);
            //    }
            //}
            return list;
        }


        /// <summary>
        /// 根据当前开始结束日期取得(老算法)
        /// 该时间内的收退料信息以及上期结存信息
        /// </summary>
        [TransManager]
        private void MaterialReckoning1(DateTime OperEndDate, int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            MaterialBalanceMaster matBalanceMaster = new MaterialBalanceMaster();
            MaterialBalanceDetail matBalanceDetail = null;
            MatBalOtherCostDetail matBalOtherCostDetail = null;

            //上期结存
            MaterialBalanceMaster ProphaseMatUnusedBal = new MaterialBalanceMaster();
            int previousYear = TransUtil.GetLastYear(fiscalYear, fiscalMonth);
            int previousMonth = TransUtil.GetLastMonth(fiscalYear, fiscalMonth);
            ProphaseMatUnusedBal = GetMatBalanceMaster(previousYear, previousMonth, theSupplier, ProjectInfo);

            decimal sumMoney = 0;//总费用金额
            decimal sumQuantity = 0;//收料总数量

            //存放本期收料和上期结存
            Hashtable ht_MatCollDetail = new Hashtable();
            Hashtable ht_MatUnuesdBalDetail = new Hashtable();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo.Id", theSupplier.Id));
            oq.AddCriterion(Expression.Eq("BalState", 0));
            oq.AddCriterion(Expression.Sql("CreateDate<=to_date('" + OperEndDate.Date.ToShortDateString() + "','yyyy-mm-dd')"));

            #region 1.本期退料(构造退料时序并计算其他费用)
            //本期退料
            IList list_matReturnMaster = GetMaterialReturnMaster(oq);
            //退料时序
            IList MatReturnSeq_list = new ArrayList();
            foreach (MaterialReturnMaster MatReturnMaster in list_matReturnMaster)
            {
                //计算运输费
                sumMoney += MatReturnMaster.TransportCharge;

                //循环明细
                foreach (MaterialReturnDetail MatReturnDetail in MatReturnMaster.Details)
                {
                    //退料负数产生收料添加到本期收料哈希表中(根据退料明细构建一个收料明细)
                    if (MatReturnDetail.ExitQuantity < 0)
                    {
                        MaterialCollectionDetail MatCollDetail = new MaterialCollectionDetail();
                        MatCollDetail.Id = MatReturnDetail.Id;
                        MatCollDetail.MatCollDate = MatReturnMaster.CreateDate;
                        MatCollDetail.Quantity = Math.Abs(MatReturnDetail.ExitQuantity);
                        MatCollDetail.MaterialResource = MatReturnDetail.MaterialResource;
                        MatCollDetail.MaterialCode = MatReturnDetail.MaterialCode;
                        MatCollDetail.MaterialName = MatReturnDetail.MaterialName;
                        MatCollDetail.MaterialSpec = MatReturnDetail.MaterialSpec;
                        MatCollDetail.MatStandardUnit = MatReturnDetail.MatStandardUnit;
                        MatCollDetail.MatStandardUnitName = MatReturnDetail.MatStandardUnitName;
                        MatCollDetail.RentalPrice = MatReturnDetail.RentalPrice;
                        MatCollDetail.BalRule = MatReturnMaster.BalRule;
                        MatCollDetail.LeftQuantuity = Math.Abs(MatReturnDetail.ExitQuantity);
                        MatCollDetail.Master = new BaseMaster();
                        MatCollDetail.Master.Code = MatReturnDetail.Master.Code;

                        ht_MatCollDetail.Add(MatCollDetail.Id, MatCollDetail);
                    }

                    //数量费用明细
                    foreach (MaterialReturnCostDtl MatReturnCostDtl in MatReturnDetail.MatReturnCostDtls)
                    {
                        matBalOtherCostDetail = new MatBalOtherCostDetail();
                        matBalOtherCostDetail.BusinessCode = MatReturnMaster.Code;
                        matBalOtherCostDetail.BusinessDetailId = MatReturnDetail.Id;
                        matBalOtherCostDetail.BusinessMasterId = MatReturnMaster.Id;
                        matBalOtherCostDetail.BusinessType = "退料";
                        matBalOtherCostDetail.CostMoney = MatReturnCostDtl.Money;
                        matBalOtherCostDetail.MaterialResource = MatReturnDetail.MaterialResource;
                        matBalOtherCostDetail.MaterialCode = MatReturnDetail.MaterialCode;
                        matBalOtherCostDetail.MaterialName = MatReturnDetail.MaterialName;
                        matBalOtherCostDetail.MaterialSpec = MatReturnDetail.MaterialSpec;
                        //计算总费用
                        sumMoney = sumMoney + MatReturnCostDtl.Money;
                        matBalOtherCostDetail.CostType = MatReturnCostDtl.CostType;
                        matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                    }
                    //循环退料时序
                    foreach (MaterialReturnDetailSeq MatReturnDeatilSeq in MatReturnDetail.MaterialReturnDetailSeqs)
                    {
                        MatReturnSeq_list.Add(MatReturnDeatilSeq);
                    }
                }
            }

            #endregion

            #region 2.本期收料 (构造收料哈希表并计算其他费用)
            //本期收料
            IList list_matCollMaster = GetMaterialCollectionMaster(oq);
            foreach (MaterialCollectionMaster MatCollMaster in list_matCollMaster)
            {
                //计算运输费
                sumMoney += MatCollMaster.TransportCharge;

                foreach (MaterialCollectionDetail MatCollDetail in MatCollMaster.Details)
                {
                    //数量费用明细
                    foreach (MaterialCostDtl MatCollCostDtl in MatCollDetail.MatCostDtls)
                    {
                        matBalOtherCostDetail = new MatBalOtherCostDetail();
                        matBalOtherCostDetail.BusinessCode = MatCollMaster.Code;
                        matBalOtherCostDetail.BusinessDetailId = MatCollDetail.Id;
                        matBalOtherCostDetail.BusinessMasterId = MatCollMaster.Id;
                        matBalOtherCostDetail.BusinessType = "收料";
                        matBalOtherCostDetail.CostMoney = MatCollCostDtl.Money;
                        matBalOtherCostDetail.MaterialResource = MatCollDetail.MaterialResource;
                        matBalOtherCostDetail.MaterialCode = MatCollDetail.MaterialCode;
                        matBalOtherCostDetail.MaterialName = MatCollDetail.MaterialName;
                        matBalOtherCostDetail.MaterialSpec = MatCollDetail.MaterialSpec;
                        //计算总费用
                        sumMoney = sumMoney + MatCollCostDtl.Money;
                        matBalOtherCostDetail.CostType = MatCollCostDtl.CostType;
                        matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                    }
                    MatCollDetail.MatCollDate = MatCollMaster.CreateDate;
                    MatCollDetail.LeftQuantuity = MatCollDetail.Quantity;
                    MatCollDetail.BalRule = MatCollMaster.BalRule;
                    ht_MatCollDetail.Add(MatCollDetail.Id, MatCollDetail);

                    //计算收料负数产生的退料时序
                    foreach (MaterialReturnDetailSeq MatReturnDeatilSeq in MatCollDetail.MaterialReturnDetailSeqs)
                    {
                        if (MatReturnDeatilSeq.MatCollDtlId == null)
                        {
                            MatReturnDeatilSeq.MatCollDtlId = MatReturnDeatilSeq.MatReturnDtlId;
                        }
                        MatReturnSeq_list.Add(MatReturnDeatilSeq);
                    }
                }
            }
            #endregion

            #region 3.上期结存(构造上期结存哈希表)
            if (ProphaseMatUnusedBal != null)
            {
                foreach (MaterialBalanceDetail MaterialBalanceDetail in ProphaseMatUnusedBal.Details)
                {
                    if (MaterialBalanceDetail.UnusedBalQuantity > 0)
                    {
                        ht_MatUnuesdBalDetail.Add(MaterialBalanceDetail.MatCollDtlId, MaterialBalanceDetail);
                    }
                }
            }
            #endregion

            #region 4.结算明细，租赁费用

            #region 循环退料时序在本期收料和上期结存里找到退料时序中对应的收料明细,并冲减本期收料和上期结存的剩余数量(结存数量)
            //循环退料时序，产生结算明细
            foreach (MaterialReturnDetailSeq MatReturnDeatilSeq in MatReturnSeq_list)
            {
                matBalanceDetail = new MaterialBalanceDetail();
                matBalanceDetail.ExitQuantity = MatReturnDeatilSeq.ReturnQuantity;


                if (ht_MatCollDetail.Contains(MatReturnDeatilSeq.MatCollDtlId))
                {
                    MaterialCollectionDetail MatCollDetail = (MaterialCollectionDetail)ht_MatCollDetail[MatReturnDeatilSeq.MatCollDtlId];
                    matBalanceDetail.BalRule = MatCollDetail.BalRule;
                    matBalanceDetail.MaterialResource = MatCollDetail.MaterialResource;
                    matBalanceDetail.MaterialCode = MatCollDetail.MaterialCode;
                    matBalanceDetail.MaterialName = MatCollDetail.MaterialName;
                    matBalanceDetail.MaterialSpec = MatCollDetail.MaterialSpec;
                    matBalanceDetail.MatStandardUnit = MatCollDetail.MatStandardUnit;
                    matBalanceDetail.MatStandardUnitName = MatCollDetail.MatStandardUnitName;
                    matBalanceDetail.RentalPrice = MatCollDetail.RentalPrice;
                    matBalanceDetail.StartDate = MatCollDetail.MatCollDate;
                    matBalanceDetail.EndDate = MatReturnDeatilSeq.ReturnDate;
                    matBalanceDetail.MatCollDtlId = MatCollDetail.Id;
                    matBalanceDetail.ApproachQuantity = MatReturnDeatilSeq.ReturnQuantity;//进场数量
                    matBalanceDetail.ExitQuantity = MatReturnDeatilSeq.ReturnQuantity;//退场数量
                    MatCollDetail.LeftQuantuity = MatCollDetail.LeftQuantuity - MatReturnDeatilSeq.ReturnQuantity;//剩余数量

                    //收退料单号和收退料明细数量
                    matBalanceDetail.MatCollCode = MatReturnDeatilSeq.MatCollCode;
                    matBalanceDetail.MatCollDtlQty = MatReturnDeatilSeq.MatCollDtlQty;
                    matBalanceDetail.MatReturnCode = MatReturnDeatilSeq.MatReturnCode;
                    matBalanceDetail.MatReturnDtlQty = MatReturnDeatilSeq.MatReturnDtlQty;

                    //根据结算规则计算天数
                    TimeSpan dt = TransUtil.ToShortDateTime(matBalanceDetail.EndDate) - TransUtil.ToShortDateTime(matBalanceDetail.StartDate);
                    if (matBalanceDetail.BalRule == "算头不算尾" || matBalanceDetail.BalRule == "算尾不算头")
                    {
                        matBalanceDetail.Days = dt.Days;
                    }
                    else if (matBalanceDetail.BalRule == "两头都不算")
                    {
                        matBalanceDetail.Days = dt.Days - 1;
                    }
                    else if (matBalanceDetail.BalRule == "两头都算")
                    {
                        matBalanceDetail.Days = dt.Days + 1;
                    }
                    //业务日期<结算开始日期为上期未结
                    if (ProphaseMatUnusedBal != null)
                    {
                        if (MatCollDetail.MatCollDate < ProphaseMatUnusedBal.EndDate)
                        {
                            matBalanceDetail.BalState = "上期未结";
                        }
                        else
                        {
                            matBalanceDetail.BalState = "本期发生";
                        }
                    }
                    else
                    {
                        matBalanceDetail.BalState = "本期发生";
                    }
                    //计算本期收料租赁费用
                    matBalanceDetail.Money = MatReturnDeatilSeq.ReturnQuantity * matBalanceDetail.Days * MatCollDetail.RentalPrice;
                    sumMoney += matBalanceDetail.Money;
                    sumQuantity += matBalanceDetail.ApproachQuantity;
                    //结存数量
                    matBalanceDetail.UnusedBalQuantity = 0;
                    matBalanceMaster.AddDetail(matBalanceDetail);
                }
                else
                {
                    MaterialBalanceDetail MatBalDetail = (MaterialBalanceDetail)ht_MatUnuesdBalDetail[MatReturnDeatilSeq.MatCollDtlId];
                    if (MatBalDetail != null)
                    {
                        matBalanceDetail.BalRule = MatBalDetail.BalRule;
                        matBalanceDetail.MaterialResource = MatBalDetail.MaterialResource;
                        matBalanceDetail.MaterialCode = MatBalDetail.MaterialCode;
                        matBalanceDetail.MaterialName = MatBalDetail.MaterialName;
                        matBalanceDetail.MaterialSpec = MatBalDetail.MaterialSpec;
                        matBalanceDetail.MatStandardUnit = MatBalDetail.MatStandardUnit;
                        matBalanceDetail.MatStandardUnitName = MatBalDetail.MatStandardUnitName;
                        matBalanceDetail.RentalPrice = MatBalDetail.RentalPrice;
                        matBalanceDetail.StartDate = ProphaseMatUnusedBal.EndDate.AddDays(1);
                        matBalanceDetail.EndDate = MatReturnDeatilSeq.ReturnDate;
                        //根据结算规则计算天数
                        TimeSpan dt = TransUtil.ToShortDateTime(matBalanceDetail.EndDate) - TransUtil.ToShortDateTime(matBalanceDetail.StartDate);
                        if (matBalanceDetail.BalRule == "算头不算尾" || matBalanceDetail.BalRule == "算尾不算头")
                        {
                            matBalanceDetail.Days = dt.Days;
                        }
                        else if (matBalanceDetail.BalRule == "两头都不算")
                        {
                            matBalanceDetail.Days = dt.Days - 1;
                        }
                        else if (matBalanceDetail.BalRule == "两头都算")
                        {
                            matBalanceDetail.Days = dt.Days + 1;
                        }
                        matBalanceDetail.BalState = "上期结存";
                        matBalanceDetail.ApproachQuantity = MatReturnDeatilSeq.ReturnQuantity;
                        matBalanceDetail.ExitQuantity = MatReturnDeatilSeq.ReturnQuantity;
                        matBalanceDetail.MatCollDtlId = MatBalDetail.MatCollDtlId;

                        //收退料单号和收退料明细数量
                        matBalanceDetail.MatCollCode = MatReturnDeatilSeq.MatCollCode;
                        matBalanceDetail.MatCollDtlQty = MatReturnDeatilSeq.MatCollDtlQty;
                        matBalanceDetail.MatReturnCode = MatReturnDeatilSeq.MatReturnCode;
                        matBalanceDetail.MatReturnDtlQty = MatReturnDeatilSeq.MatReturnDtlQty;

                        //计算上期结存租赁费用
                        matBalanceDetail.Money = MatReturnDeatilSeq.ReturnQuantity * matBalanceDetail.Days * matBalanceDetail.RentalPrice;
                        sumMoney += matBalanceDetail.Money;
                        sumQuantity += matBalanceDetail.ApproachQuantity;

                        //结存数量
                        matBalanceDetail.UnusedBalQuantity = 0;
                        MatBalDetail.UnusedBalQuantity = MatBalDetail.UnusedBalQuantity - MatReturnDeatilSeq.ReturnQuantity;
                        matBalanceMaster.AddDetail(matBalanceDetail);
                    }
                }
            }

            #endregion

            #region 本期结存数量：1. 本期收料有剩余产生的结存  2.上期结存仍有结存的产生结存放到本期
            //本期收料结存
            foreach (string CollDtlId in ht_MatCollDetail.Keys)
            {
                MaterialCollectionDetail MatCollDetail = ht_MatCollDetail[CollDtlId] as MaterialCollectionDetail;
                if (MatCollDetail.LeftQuantuity > 0)
                {
                    matBalanceDetail = new MaterialBalanceDetail();
                    matBalanceDetail.BalRule = MatCollDetail.BalRule;
                    matBalanceDetail.MaterialResource = MatCollDetail.MaterialResource;
                    matBalanceDetail.MaterialCode = MatCollDetail.MaterialCode;
                    matBalanceDetail.MaterialName = MatCollDetail.MaterialName;
                    matBalanceDetail.MaterialSpec = MatCollDetail.MaterialSpec;
                    matBalanceDetail.MatStandardUnit = MatCollDetail.MatStandardUnit;
                    matBalanceDetail.MatStandardUnitName = MatCollDetail.MatStandardUnitName;
                    matBalanceDetail.RentalPrice = MatCollDetail.RentalPrice;
                    matBalanceDetail.StartDate = MatCollDetail.MatCollDate;
                    matBalanceDetail.EndDate = OperEndDate;
                    matBalanceDetail.MatCollDtlId = MatCollDetail.Id;
                    matBalanceDetail.ApproachQuantity = MatCollDetail.LeftQuantuity;//进场数量
                    matBalanceDetail.ExitQuantity = 0;//退场数量
                    //收料单号和收料明细数量
                    matBalanceDetail.MatCollCode = MatCollDetail.Master.Code;
                    matBalanceDetail.MatCollDtlQty = MatCollDetail.Quantity;

                    //根据结算规则计算天数
                    TimeSpan dt = TransUtil.ToShortDateTime(matBalanceDetail.EndDate) - TransUtil.ToShortDateTime(matBalanceDetail.StartDate);
                    if (matBalanceDetail.BalRule == "算头不算尾" || matBalanceDetail.BalRule == "两头都算")
                    {
                        matBalanceDetail.Days = dt.Days + 1;
                    }
                    else if (matBalanceDetail.BalRule == "两头都不算" || matBalanceDetail.BalRule == "算尾不算头")
                    {
                        matBalanceDetail.Days = dt.Days;
                    }
                    matBalanceDetail.BalState = "本期发生";
                    //计算本期收料租赁费用 
                    matBalanceDetail.Money = MatCollDetail.LeftQuantuity * matBalanceDetail.Days * MatCollDetail.RentalPrice;
                    sumMoney += matBalanceDetail.Money;
                    sumQuantity += matBalanceDetail.ApproachQuantity;

                    //结存数量
                    matBalanceDetail.UnusedBalQuantity = MatCollDetail.LeftQuantuity;
                    matBalanceMaster.AddDetail(matBalanceDetail);
                }
            }

            //上期结存产生本期结存
            foreach (string CollDtlId in ht_MatUnuesdBalDetail.Keys)
            {
                MaterialBalanceDetail MatBalDetail = ht_MatUnuesdBalDetail[CollDtlId] as MaterialBalanceDetail;
                if (MatBalDetail.UnusedBalQuantity > 0)
                {
                    matBalanceDetail = new MaterialBalanceDetail();
                    matBalanceDetail.BalRule = MatBalDetail.BalRule;
                    matBalanceDetail.MaterialResource = MatBalDetail.MaterialResource;
                    matBalanceDetail.MaterialCode = MatBalDetail.MaterialCode;
                    matBalanceDetail.MaterialName = MatBalDetail.MaterialName;
                    matBalanceDetail.MaterialSpec = MatBalDetail.MaterialSpec;
                    matBalanceDetail.MatStandardUnit = MatBalDetail.MatStandardUnit;
                    matBalanceDetail.MatStandardUnitName = MatBalDetail.MatStandardUnitName;
                    matBalanceDetail.RentalPrice = MatBalDetail.RentalPrice;
                    if (ProphaseMatUnusedBal != null)
                    {
                        matBalanceDetail.StartDate = ProphaseMatUnusedBal.EndDate.AddDays(1);
                    }
                    else
                    {

                    }
                    matBalanceDetail.EndDate = OperEndDate;
                    matBalanceDetail.MatCollDtlId = MatBalDetail.MatCollDtlId;
                    matBalanceDetail.ApproachQuantity = MatBalDetail.UnusedBalQuantity;//进场数量
                    matBalanceDetail.ExitQuantity = 0;//退场数量
                    //收退料单号和收退料明细数量
                    matBalanceDetail.MatCollCode = MatBalDetail.MatCollCode;
                    matBalanceDetail.MatCollDtlQty = MatBalDetail.MatCollDtlQty;
                    matBalanceDetail.MatReturnCode = MatBalDetail.MatReturnCode;
                    matBalanceDetail.MatReturnDtlQty = MatBalDetail.MatReturnDtlQty;


                    //根据结算规则计算天数
                    TimeSpan dt = TransUtil.ToShortDateTime(matBalanceDetail.EndDate) - TransUtil.ToShortDateTime(matBalanceDetail.StartDate);
                    matBalanceDetail.Days = dt.Days + 1;
                    matBalanceDetail.BalState = "上期结存";
                    //结存数量
                    matBalanceDetail.UnusedBalQuantity = MatBalDetail.UnusedBalQuantity;
                    //计算本期收料租赁费用

                    matBalanceDetail.Money = matBalanceDetail.UnusedBalQuantity * matBalanceDetail.Days * MatBalDetail.RentalPrice;
                    sumMoney += matBalanceDetail.Money;
                    sumQuantity += matBalanceDetail.ApproachQuantity;

                    matBalanceMaster.AddDetail(matBalanceDetail);
                }
            }
            #endregion
            #endregion

            #region 6.组织主表信息保存
            Login login = VirtualMachine.Component.Util.CallContextUtil.LogicalGetData<Login>("LoginInformation");
            matBalanceMaster.CreatePerson = login.ThePerson;
            matBalanceMaster.CreateDate = DateTime.Now;
            matBalanceMaster.OperOrgInfo = login.TheOperationOrgInfo;
            matBalanceMaster.OperOrgInfoName = login.TheOperationOrgInfo.Name;
            matBalanceMaster.OpgSysCode = login.TheOperationOrgInfo.SysCode;
            matBalanceMaster.CreatePersonName = login.ThePerson.Name;
            matBalanceMaster.FiscalYear = Convert.ToInt32(fiscalYear);
            matBalanceMaster.FiscalMonth = Convert.ToInt32(fiscalMonth);
            matBalanceMaster.TheSupplierRelationInfo = theSupplier;
            matBalanceMaster.SupplierName = theSupplier.SupplierInfo.Name;
            matBalanceMaster.ProjectId = ProjectInfo.Id;
            matBalanceMaster.ProjectName = ProjectInfo.Name;
            if (ProphaseMatUnusedBal != null)
                matBalanceMaster.StartDate = ProphaseMatUnusedBal.EndDate.AddDays(1);
            else
                matBalanceMaster.StartDate = ClientUtil.ToDateTime("1900-1-1");
            matBalanceMaster.EndDate = Convert.ToDateTime(OperEndDate);
            matBalanceMaster.SumMatMoney = sumMoney;
            matBalanceMaster.SumMatQuantity = sumQuantity;
            MaterialRentalOrderMaster MatRenMaster = new MaterialRentalOrderMaster();
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));
            IList lst = GetMaterialRentalOrder(oq) as IList;
            if (lst.Count > 0)
            {
                MatRenMaster = lst[0] as MaterialRentalOrderMaster;
            }
            matBalanceMaster.OldContractNum = MatRenMaster.OriginalContractNo;

            matBalanceMaster = SaveByDao(matBalanceMaster) as MaterialBalanceMaster;
            #endregion

            #region 7.收退料单主表加上结算标记
            foreach (MaterialCollectionMaster master in list_matCollMaster)
            {
                if (master != null)
                {
                    master.BalYear = fiscalYear;
                    master.BalMonth = fiscalMonth;
                    master.BalState = 1;
                    UpdateByDao(master);
                }
            }
            foreach (MaterialReturnMaster master in list_matReturnMaster)
            {
                if (master != null)
                {
                    master.BalYear = fiscalYear;
                    master.BalMonth = fiscalMonth;
                    master.BalState = 1;
                    UpdateByDao(master);
                }
            }
            #endregion
        }

        public void CreateMatSetBalInfoByYearAndMonth(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            MaterialBalanceMaster matBalanceMaster = GetMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);

            #region 生成本月的料具结算信息(商务)
            MaterialSettleMaster masterMat = new MaterialSettleMaster();
            masterMat.AuditMonth = matBalanceMaster.AuditMonth;
            masterMat.AuditYear = matBalanceMaster.AuditYear;
            decimal sumMoney = 0;
            Hashtable ht = new Hashtable();
            foreach (MaterialBalanceDetail detail in matBalanceMaster.Details)
            {
                if (detail.UsedPart != null && detail.SubjectGUID != null && detail.MaterialResource != null)
                {
                    string linkStr = detail.UsedPart.Id + "-" + detail.MaterialResource.Id + "-" + detail.SubjectGUID.Id;
                    if (ht.Contains(linkStr))
                    {
                        MaterialBalanceDetail temp = (MaterialBalanceDetail)ht[linkStr];
                        temp.TempData = (TransUtil.ToDecimal(temp.TempData) + detail.ExitQuantity + detail.ApproachQuantity) + "";
                        temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) + detail.Money) + "";
                        ht.Remove(linkStr);
                        ht.Add(linkStr, temp);
                    }
                    else
                    {
                        detail.TempData = (detail.ExitQuantity + detail.ApproachQuantity) + "";
                        detail.TempData1 = detail.Money + "";
                        ht.Add(linkStr, detail);
                    }
                }
            }
            foreach (MatBalOtherCostDetail otherDetail in matBalanceMaster.MatBalOtherCostDetails)
            {

                if (otherDetail.UsedPart != null && otherDetail.SubjectGUID != null && otherDetail.MaterialResource != null)
                {
                    string linkStr = otherDetail.UsedPart.Id + "-" + otherDetail.MaterialResource.Id + "-" + otherDetail.SubjectGUID.Id;
                    if (ht.Contains(linkStr))
                    {
                        MaterialBalanceDetail temp = (MaterialBalanceDetail)ht[linkStr];
                        temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) + otherDetail.CostMoney) + "";
                        ht.Remove(linkStr);
                        ht.Add(linkStr, temp);
                    }
                    else
                    {
                        MaterialBalanceDetail temp = new MaterialBalanceDetail();
                        temp.MaterialCode = otherDetail.MaterialCode;
                        temp.MaterialResource = otherDetail.MaterialResource;
                        temp.MaterialName = otherDetail.MaterialName;
                        temp.MatStandardUnit = otherDetail.MatStandardUnit;
                        temp.MatStandardUnitName = otherDetail.MatStandardUnitName;
                        temp.UsedPart = otherDetail.UsedPart;
                        temp.UsedPartSysCode = otherDetail.UsedPartSysCode;
                        temp.UsedPartName = otherDetail.UsedPartName;
                        temp.Money = otherDetail.CostMoney;
                        temp.SubjectGUID = otherDetail.SubjectGUID;
                        temp.SubjectName = otherDetail.SubjectName;
                        temp.SubjectSysCode = otherDetail.SubjectSysCode;
                        ht.Add(linkStr, temp);
                    }
                }
            }
            if (ht.Count > 0)
            {
                foreach (MaterialBalanceDetail dtl in ht.Values)
                {
                    MaterialSettleDetail del = new MaterialSettleDetail();
                    del.MaterialCode = dtl.MaterialCode;
                    del.MaterialResource = dtl.MaterialResource;
                    del.MaterialName = dtl.MaterialName;
                    del.MaterialSpec = dtl.MaterialSpec;
                    del.MaterialStuff = dtl.MaterialStuff;
                    del.QuantityUnit = dtl.MatStandardUnit;
                    del.QuantityUnitName = dtl.MatStandardUnitName;
                    del.ProjectTask = dtl.UsedPart;
                    del.ProjectTaskCode = dtl.UsedPartSysCode;
                    del.ProjectTaskName = dtl.UsedPartName;
                    del.Quantity = TransUtil.ToDecimal(dtl.TempData);
                    del.Money = TransUtil.ToDecimal(dtl.TempData1);
                    sumMoney += del.Money;
                    if (del.Quantity != 0)
                    {
                        del.Price = decimal.Round(del.Money / del.Quantity, 4);
                    }
                    del.AccountCostSubject = dtl.SubjectGUID;
                    del.AccountCostName = dtl.SubjectName;
                    del.AccountCostCode = dtl.SubjectSysCode;
                    del.Master = masterMat;
                    masterMat.AddDetail(del);
                }
            }
            //matBalanceMaster
            masterMat.CreatePerson = matBalanceMaster.CreatePerson;
            masterMat.CreatePersonName = matBalanceMaster.CreatePersonName;
            masterMat.OperOrgInfoName = matBalanceMaster.OperOrgInfoName;
            masterMat.OperOrgInfo = matBalanceMaster.OperOrgInfo;
            masterMat.OpgSysCode = matBalanceMaster.OpgSysCode;
            masterMat.HandOrgLevel = matBalanceMaster.OperOrgInfo.Level;
            masterMat.HandleOrg = matBalanceMaster.HandleOrg;
            masterMat.HandlePerson = matBalanceMaster.HandlePerson;
            masterMat.HandlePersonName = matBalanceMaster.HandlePersonName;
            masterMat.DocState = DocumentState.InExecute;
            masterMat.ProjectId = ProjectInfo.Id;
            masterMat.ProjectName = ProjectInfo.Name;
            masterMat.CreateDate = DateTime.Now;//制单时间
            masterMat.RealOperationDate = DateTime.Now;
            masterMat.CreateYear = fiscalYear;//制单年
            masterMat.CreateMonth = fiscalMonth;//制单月
            masterMat.SumMoney = sumMoney;
            masterMat.SettleState = "materialQuery";
            materialSettleSrv.SaveMaterialSettle(masterMat);
            #endregion
        }

        /// <summary>
        /// 根据当前开始结束日期取得
        /// 该时间内的收退料信息以及上期结存信息
        /// </summary>
        [TransManager]
        public void MaterialReckoning(DateTime OperEndDate, int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier,
            CurrentProjectInfo ProjectInfo, decimal otherMoney, GWBSTree task)
        {
            MaterialBalanceMaster matBalanceMaster = new MaterialBalanceMaster();
            MaterialBalanceDetail matBalanceDetail = null;
            MatBalOtherCostDetail matBalOtherCostDetail = null;

            //上期结存
            MaterialBalanceMaster ProphaseMatUnusedBal = new MaterialBalanceMaster();
            int previousYear = TransUtil.GetLastYear(fiscalYear, fiscalMonth);
            int previousMonth = TransUtil.GetLastMonth(fiscalYear, fiscalMonth);
            ProphaseMatUnusedBal = GetMatBalanceMaster(previousYear, previousMonth, theSupplier, ProjectInfo);

            decimal sumMoney = 0;//总费用金额
            decimal sumQuantity = 0;//收料总数量

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo.Id", theSupplier.Id));
            oq.AddCriterion(Expression.Eq("BalState", 0));
            oq.AddCriterion(Expression.Sql("CreateDate<=to_date('" + OperEndDate.Date.ToShortDateString() + "','yyyy-mm-dd')"));
            //料具对应料具租赁物资
            Material material = new Material();
            material.Version = 0;
            material.Id = TransUtil.ConTrafficMaterialId;
            material.Code = TransUtil.ConTrafficMaterialCode;
            material.Name = TransUtil.ConTrafficMaterialName;
            //运输费用对应科目
            CostAccountSubject transSubject = new CostAccountSubject();
            transSubject.Id = TransUtil.ConTrafficSubjectId;
            transSubject.Name = TransUtil.ConTrafficSubjectName;
            transSubject.SysCode = TransUtil.ConTrafficSubjectSyscode;
            //调整费用对应科目
            CostAccountSubject otherSubject = new CostAccountSubject();
            otherSubject.Id = TransUtil.ConStockOutSubjectId;
            otherSubject.Name = TransUtil.ConStockOutSubjectName;
            otherSubject.SysCode = TransUtil.ConStockOutSubjectSyscode;

            #region 1.本期退料 (租赁费用/运输费用/其他费用)
            //本期退料
            IList list_matReturnMaster = GetMaterialReturnMaster(oq);
            MaterialReturnDetail oMaterialReturnDetail = null;
            MatBalOtherCostDetail oMatBalOtherCostDetail = null;

            decimal dMax = 0;
            foreach (MaterialReturnMaster MatReturnMaster in list_matReturnMaster)
            {
                //计算运输费
                oMatBalOtherCostDetail = null;
                if (MatReturnMaster.TransportCharge != 0)
                {
                    matBalOtherCostDetail = new MatBalOtherCostDetail();
                    oMatBalOtherCostDetail = matBalOtherCostDetail;
                    matBalOtherCostDetail.BusinessCode = MatReturnMaster.Code;
                    matBalOtherCostDetail.BusinessMasterId = MatReturnMaster.Id;
                    matBalOtherCostDetail.BusinessType = "退料";
                    matBalOtherCostDetail.CostMoney = MatReturnMaster.TransportCharge;

                    foreach (MaterialReturnDetail MatReturnDetail in MatReturnMaster.Details)
                    {
                        matBalOtherCostDetail.UsedPart = MatReturnDetail.UsedPart;
                        matBalOtherCostDetail.UsedPartSysCode = MatReturnDetail.UsedPartSysCode;
                        matBalOtherCostDetail.UsedPartName = MatReturnDetail.UsedPartName;
                        break;
                    }
                    matBalOtherCostDetail.MaterialResource = material;
                    matBalOtherCostDetail.MaterialCode = material.Code;
                    matBalOtherCostDetail.MaterialName = material.Name;
                    matBalOtherCostDetail.SubjectGUID = transSubject;
                    matBalOtherCostDetail.SubjectName = transSubject.Name;
                    matBalOtherCostDetail.SubjectSysCode = transSubject.SysCode;
                    //计算总费用
                    matBalOtherCostDetail.CostType = "运输费用";
                    matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                    sumMoney += MatReturnMaster.TransportCharge;
                }
                dMax = decimal.MinValue;
                oMaterialReturnDetail = null;

                //循环明细
                foreach (MaterialReturnDetail MatReturnDetail in MatReturnMaster.Details)
                {
                    //
                    if (MatReturnDetail.Quantity * MatReturnDetail.RentalPrice >= dMax)
                    {
                        oMaterialReturnDetail = MatReturnDetail;
                    }
                    //租赁费用
                    matBalanceDetail = new MaterialBalanceDetail();
                    matBalanceDetail.BalRule = MatReturnMaster.BalRule;
                    matBalanceDetail.MaterialResource = MatReturnDetail.MaterialResource;
                    matBalanceDetail.MaterialCode = MatReturnDetail.MaterialCode;
                    matBalanceDetail.MaterialName = MatReturnDetail.MaterialName;
                    matBalanceDetail.MaterialSpec = MatReturnDetail.MaterialSpec;
                    matBalanceDetail.MatStandardUnit = MatReturnDetail.MatStandardUnit;
                    matBalanceDetail.MatStandardUnitName = MatReturnDetail.MatStandardUnitName;
                    matBalanceDetail.RentalPrice = MatReturnDetail.RentalPrice;
                    matBalanceDetail.StartDate = MatReturnMaster.CreateDate;
                    matBalanceDetail.EndDate = OperEndDate;
                    matBalanceDetail.ExitQuantity = -MatReturnDetail.ExitQuantity;//退场数量
                    //退料单号和退料明细数量
                    matBalanceDetail.MatReturnCode = MatReturnMaster.Code;
                    matBalanceDetail.MatReturnDtlQty = MatReturnDetail.ExitQuantity;
                    matBalanceDetail.MatCollDtlId = MatReturnDetail.Id;
                    //部位和科目
                    matBalanceDetail.SubjectGUID = MatReturnDetail.SubjectGUID;
                    matBalanceDetail.SubjectName = MatReturnDetail.SubjectName;
                    matBalanceDetail.SubjectSysCode = MatReturnDetail.SubjectSysCode;
                    matBalanceDetail.UsedPart = MatReturnDetail.UsedPart;
                    matBalanceDetail.UsedPartName = MatReturnDetail.UsedPartName;
                    matBalanceDetail.UsedPartSysCode = MatReturnDetail.UsedPartSysCode;

                    //根据结算规则计算天数
                    TimeSpan dt = TransUtil.ToShortDateTime(matBalanceDetail.EndDate) - TransUtil.ToShortDateTime(matBalanceDetail.StartDate);
                    if (matBalanceDetail.BalRule == "两头都算" || matBalanceDetail.BalRule == "算尾不算头")
                    {
                        matBalanceDetail.Days = dt.Days;
                    }
                    else if (matBalanceDetail.BalRule == "两头都不算" || matBalanceDetail.BalRule == "算头不算尾")
                    {
                        matBalanceDetail.Days = dt.Days + 1;
                    }
                    //业务日期<结算开始日期为上期未结
                    if (ProphaseMatUnusedBal != null)
                    {
                        if (MatReturnMaster.CreateDate < ProphaseMatUnusedBal.EndDate)
                        {
                            matBalanceDetail.BalState = "上期未结";
                        }
                        else
                        {
                            matBalanceDetail.BalState = "本期发生";
                        }
                    }
                    else
                    {
                        matBalanceDetail.BalState = "本期发生";
                    }
                    //计算本期收料租赁费用
                    matBalanceDetail.Money = matBalanceDetail.ExitQuantity * matBalanceDetail.Days * MatReturnDetail.RentalPrice;
                    sumMoney += matBalanceDetail.Money;
                    sumQuantity += matBalanceDetail.ExitQuantity;
                    //结存数量
                    matBalanceDetail.UnusedBalQuantity = matBalanceDetail.ExitQuantity;
                    matBalanceMaster.AddDetail(matBalanceDetail);

                    //数量费用明细
                    foreach (MaterialReturnCostDtl MatReturnCostDtl in MatReturnDetail.MatReturnCostDtls)
                    {
                        matBalOtherCostDetail = new MatBalOtherCostDetail();
                        matBalOtherCostDetail.BusinessCode = MatReturnMaster.Code;
                        matBalOtherCostDetail.BusinessDetailId = MatReturnDetail.Id;
                        matBalOtherCostDetail.BusinessMasterId = MatReturnMaster.Id;
                        matBalOtherCostDetail.BusinessType = "退料";
                        matBalOtherCostDetail.CostMoney = MatReturnCostDtl.Money;
                        matBalOtherCostDetail.MaterialResource = MatReturnDetail.MaterialResource;
                        matBalOtherCostDetail.MaterialCode = MatReturnDetail.MaterialCode;
                        matBalOtherCostDetail.MaterialName = MatReturnDetail.MaterialName;
                        matBalOtherCostDetail.MaterialSpec = MatReturnDetail.MaterialSpec;
                        //部位和科目
                        matBalOtherCostDetail.SubjectGUID = MatReturnDetail.SubjectGUID;
                        matBalOtherCostDetail.SubjectName = MatReturnDetail.SubjectName;
                        matBalOtherCostDetail.SubjectSysCode = MatReturnDetail.SubjectSysCode;
                        matBalOtherCostDetail.UsedPart = MatReturnDetail.UsedPart;
                        matBalOtherCostDetail.UsedPartName = MatReturnDetail.UsedPartName;
                        matBalOtherCostDetail.UsedPartSysCode = MatReturnDetail.UsedPartSysCode;

                        //计算总费用
                        sumMoney = sumMoney + MatReturnCostDtl.Money;
                        matBalOtherCostDetail.CostType = MatReturnCostDtl.CostType;
                        matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                    }
                }
                if (MatReturnMaster.TransportCharge != 0 && oMaterialReturnDetail != null && oMatBalOtherCostDetail != null)
                {
                    //  matBalOtherCostDetail = oMaterialReturnDetail;
                    oMatBalOtherCostDetail.UsedPart = oMaterialReturnDetail.UsedPart;
                    oMatBalOtherCostDetail.UsedPartName = oMaterialReturnDetail.UsedPartName;
                    oMatBalOtherCostDetail.UsedPartSysCode = oMaterialReturnDetail.UsedPartSysCode;
                }
            }

            #endregion

            #region 2.本期收料 (租赁费用/运输费用/其他费用)
            //本期收料
            MaterialCollectionDetail oMaterialCollectionDetail = null;

            IList list_matCollMaster = GetMaterialCollectionMaster(oq);
            foreach (MaterialCollectionMaster MatCollMaster in list_matCollMaster)
            {
                oMatBalOtherCostDetail = null;
                //2.1 计算运输费
                if (MatCollMaster.TransportCharge != 0)
                {
                    matBalOtherCostDetail = new MatBalOtherCostDetail();
                    oMatBalOtherCostDetail = matBalOtherCostDetail;
                    matBalOtherCostDetail.BusinessCode = MatCollMaster.Code;
                    matBalOtherCostDetail.BusinessMasterId = MatCollMaster.Id;
                    matBalOtherCostDetail.BusinessType = "收料";
                    matBalOtherCostDetail.CostMoney = MatCollMaster.TransportCharge;

                    foreach (MaterialCollectionDetail MatCollDetail in MatCollMaster.Details)
                    {
                        matBalOtherCostDetail.UsedPart = MatCollDetail.UsedPart;
                        matBalOtherCostDetail.UsedPartSysCode = MatCollDetail.UsedPartSysCode;
                        matBalOtherCostDetail.UsedPartName = MatCollDetail.UsedPartName;
                        break;
                    }
                    matBalOtherCostDetail.MaterialResource = material;
                    matBalOtherCostDetail.MaterialCode = material.Code;
                    matBalOtherCostDetail.MaterialName = material.Name;
                    matBalOtherCostDetail.SubjectGUID = transSubject;
                    matBalOtherCostDetail.SubjectName = transSubject.Name;
                    matBalOtherCostDetail.SubjectSysCode = transSubject.SysCode;
                    //计算总费用
                    matBalOtherCostDetail.CostType = "运输费用";
                    matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                    sumMoney += MatCollMaster.TransportCharge;
                }
                oMaterialCollectionDetail = null;
                dMax = decimal.MinValue;
                foreach (MaterialCollectionDetail MatCollDetail in MatCollMaster.Details)
                {
                    //2.2 租赁费用
                    if (MatCollDetail.Quantity * MatCollDetail.RentalPrice >= dMax)
                    {
                        oMaterialCollectionDetail = MatCollDetail;
                    }
                    matBalanceDetail = new MaterialBalanceDetail();
                    matBalanceDetail.BalRule = MatCollMaster.BalRule;
                    matBalanceDetail.MaterialResource = MatCollDetail.MaterialResource;
                    matBalanceDetail.MaterialCode = MatCollDetail.MaterialCode;
                    matBalanceDetail.MaterialName = MatCollDetail.MaterialName;
                    matBalanceDetail.MaterialSpec = MatCollDetail.MaterialSpec;
                    matBalanceDetail.MatStandardUnit = MatCollDetail.MatStandardUnit;
                    matBalanceDetail.MatStandardUnitName = MatCollDetail.MatStandardUnitName;
                    matBalanceDetail.RentalPrice = MatCollDetail.RentalPrice;
                    matBalanceDetail.StartDate = MatCollMaster.CreateDate;
                    matBalanceDetail.EndDate = OperEndDate;
                    matBalanceDetail.MatCollDtlId = MatCollDetail.Id;
                    matBalanceDetail.ApproachQuantity = MatCollDetail.Quantity;//进场数量
                    //部位和科目
                    matBalanceDetail.SubjectGUID = MatCollDetail.SubjectGUID;
                    matBalanceDetail.SubjectName = MatCollDetail.SubjectName;
                    matBalanceDetail.SubjectSysCode = MatCollDetail.SubjectSysCode;
                    matBalanceDetail.UsedPart = MatCollDetail.UsedPart;
                    matBalanceDetail.UsedPartName = MatCollDetail.UsedPartName;
                    matBalanceDetail.UsedPartSysCode = MatCollDetail.UsedPartSysCode;
                    //收料单号和收料明细数量
                    matBalanceDetail.MatCollCode = MatCollMaster.Code;
                    matBalanceDetail.MatCollDtlQty = MatCollDetail.Quantity;

                    //根据结算规则计算天数
                    TimeSpan dt = TransUtil.ToShortDateTime(matBalanceDetail.EndDate) - TransUtil.ToShortDateTime(matBalanceDetail.StartDate);
                    if (matBalanceDetail.BalRule == "算头不算尾" || matBalanceDetail.BalRule == "两头都算")
                    {
                        matBalanceDetail.Days = dt.Days + 1;
                    }
                    else if (matBalanceDetail.BalRule == "两头都不算" || matBalanceDetail.BalRule == "算尾不算头")
                    {
                        matBalanceDetail.Days = dt.Days;
                    }
                    if (ProphaseMatUnusedBal != null)
                    {
                        if (MatCollMaster.CreateDate < ProphaseMatUnusedBal.EndDate)
                        {
                            matBalanceDetail.BalState = "上期未结";
                        }
                        else
                        {
                            matBalanceDetail.BalState = "本期发生";
                        }
                    }
                    else
                    {
                        matBalanceDetail.BalState = "本期发生";
                    }
                    //计算本期收料租赁费用 
                    matBalanceDetail.Money = matBalanceDetail.ApproachQuantity * matBalanceDetail.Days * MatCollDetail.RentalPrice;
                    sumMoney += matBalanceDetail.Money;
                    sumQuantity += matBalanceDetail.ApproachQuantity;
                    //结存数量
                    matBalanceDetail.UnusedBalQuantity = matBalanceDetail.ApproachQuantity;
                    matBalanceMaster.AddDetail(matBalanceDetail);


                    //2.3 数量费用明细
                    foreach (MaterialCostDtl MatCollCostDtl in MatCollDetail.MatCostDtls)
                    {
                        matBalOtherCostDetail = new MatBalOtherCostDetail();
                        matBalOtherCostDetail.BusinessCode = MatCollMaster.Code;
                        matBalOtherCostDetail.BusinessDetailId = MatCollDetail.Id;
                        matBalOtherCostDetail.BusinessMasterId = MatCollMaster.Id;
                        matBalOtherCostDetail.BusinessType = "收料";
                        matBalOtherCostDetail.CostMoney = MatCollCostDtl.Money;
                        matBalOtherCostDetail.MaterialResource = MatCollDetail.MaterialResource;
                        matBalOtherCostDetail.MaterialCode = MatCollDetail.MaterialCode;
                        matBalOtherCostDetail.MaterialName = MatCollDetail.MaterialName;
                        matBalOtherCostDetail.MaterialSpec = MatCollDetail.MaterialSpec;
                        //部位和科目
                        matBalOtherCostDetail.SubjectGUID = MatCollDetail.SubjectGUID;
                        matBalOtherCostDetail.SubjectName = MatCollDetail.SubjectName;
                        matBalOtherCostDetail.SubjectSysCode = MatCollDetail.SubjectSysCode;
                        matBalOtherCostDetail.UsedPart = MatCollDetail.UsedPart;
                        matBalOtherCostDetail.UsedPartName = MatCollDetail.UsedPartName;
                        matBalOtherCostDetail.UsedPartSysCode = MatCollDetail.UsedPartSysCode;
                        //计算总费用
                        sumMoney = sumMoney + MatCollCostDtl.Money;
                        matBalOtherCostDetail.CostType = MatCollCostDtl.CostType;
                        matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                    }
                    MatCollDetail.MatCollDate = MatCollMaster.CreateDate;
                    MatCollDetail.LeftQuantuity = MatCollDetail.Quantity;
                    MatCollDetail.BalRule = MatCollMaster.BalRule;

                }
                if (MatCollMaster.TransportCharge != 0 && oMaterialCollectionDetail != null && oMatBalOtherCostDetail != null)
                {
                    oMatBalOtherCostDetail.UsedPart = oMaterialCollectionDetail.UsedPart;
                    oMatBalOtherCostDetail.UsedPartName = oMaterialCollectionDetail.UsedPartName;
                    oMatBalOtherCostDetail.UsedPartSysCode = oMaterialCollectionDetail.UsedPartSysCode;
                }
            }
            #endregion

            #region 3.上期结存(租赁费用)
            IList list_preMatBalDetail = new ArrayList();
            if (ProphaseMatUnusedBal != null)
            {
                //根据结算规则计算天数
                TimeSpan dt = TransUtil.ToShortDateTime(OperEndDate) - TransUtil.ToShortDateTime(ProphaseMatUnusedBal.EndDate.AddDays(1));
                int balDays = dt.Days + 1;
                foreach (MaterialBalanceDetail materialBalanceDetail in ProphaseMatUnusedBal.Details)
                {
                    matBalanceDetail = new MaterialBalanceDetail();
                    bool ifExist = false;
                    foreach (MaterialBalanceDetail existBalDetail in list_preMatBalDetail)
                    {
                        if (materialBalanceDetail.MaterialResource == existBalDetail.MaterialResource && materialBalanceDetail.UsedPart == existBalDetail.UsedPart)
                        {
                            existBalDetail.UnusedBalQuantity += materialBalanceDetail.UnusedBalQuantity;
                            //计算本期收料租赁费用
                            decimal currMoney = materialBalanceDetail.UnusedBalQuantity * balDays * materialBalanceDetail.RentalPrice;
                            existBalDetail.Money += currMoney;
                            sumMoney += currMoney;
                            ifExist = true;
                            break;
                        }
                    }
                    if (ifExist == false)
                    {
                        matBalanceDetail.BalRule = materialBalanceDetail.BalRule;
                        matBalanceDetail.MaterialResource = materialBalanceDetail.MaterialResource;
                        matBalanceDetail.MaterialCode = materialBalanceDetail.MaterialCode;
                        matBalanceDetail.MaterialName = materialBalanceDetail.MaterialName;
                        matBalanceDetail.MaterialSpec = materialBalanceDetail.MaterialSpec;
                        matBalanceDetail.MatStandardUnit = materialBalanceDetail.MatStandardUnit;
                        matBalanceDetail.MatStandardUnitName = materialBalanceDetail.MatStandardUnitName;
                        matBalanceDetail.RentalPrice = materialBalanceDetail.RentalPrice;
                        //部位和科目
                        matBalanceDetail.SubjectGUID = materialBalanceDetail.SubjectGUID;
                        matBalanceDetail.SubjectName = materialBalanceDetail.SubjectName;
                        matBalanceDetail.SubjectSysCode = materialBalanceDetail.SubjectSysCode;
                        matBalanceDetail.UsedPart = materialBalanceDetail.UsedPart;
                        matBalanceDetail.UsedPartName = materialBalanceDetail.UsedPartName;
                        matBalanceDetail.UsedPartSysCode = materialBalanceDetail.UsedPartSysCode;
                        matBalanceDetail.Days = balDays;
                        if (ProphaseMatUnusedBal != null)
                        {
                            matBalanceDetail.StartDate = ProphaseMatUnusedBal.EndDate.AddDays(1);
                        }
                        matBalanceDetail.EndDate = OperEndDate;
                        //matBalanceDetail.MatCollDtlId = materialBalanceDetail.MatCollDtlId;
                        //matBalanceDetail.ApproachQuantity = materialBalanceDetail.ApproachQuantity;//进场数量
                        //收退料单号和收退料明细数量
                        //matBalanceDetail.MatCollCode = materialBalanceDetail.MatCollCode;
                        //matBalanceDetail.MatCollDtlQty = materialBalanceDetail.MatCollDtlQty;

                        matBalanceDetail.BalState = "上期结存";
                        //结存数量
                        matBalanceDetail.UnusedBalQuantity = materialBalanceDetail.UnusedBalQuantity;
                        //计算本期收料租赁费用
                        matBalanceDetail.Money = matBalanceDetail.UnusedBalQuantity * balDays * materialBalanceDetail.RentalPrice;
                        sumMoney += matBalanceDetail.Money;

                        list_preMatBalDetail.Add(matBalanceDetail);
                    }
                }
            }

            foreach (MaterialBalanceDetail preBalDetail in list_preMatBalDetail)
            {
                if (preBalDetail.UnusedBalQuantity != 0)
                {
                    matBalanceMaster.AddDetail(preBalDetail);
                }
            }
            #endregion

            #region 4.组织主表信息保存
            Login login = VirtualMachine.Component.Util.CallContextUtil.LogicalGetData<Login>("LoginInformation");
            matBalanceMaster.CreatePerson = login.ThePerson;
            matBalanceMaster.OperOrgInfo = login.TheOperationOrgInfo;
            matBalanceMaster.OpgSysCode = login.TheOperationOrgInfo.SysCode;
            matBalanceMaster.OperOrgInfoName = login.TheOperationOrgInfo.Name;
            matBalanceMaster.CreateDate = OperEndDate;
            matBalanceMaster.OtherMoney = otherMoney;
            matBalanceMaster.CreatePersonName = login.ThePerson.Name;
            matBalanceMaster.FiscalYear = Convert.ToInt32(fiscalYear);
            matBalanceMaster.FiscalMonth = Convert.ToInt32(fiscalMonth);
            matBalanceMaster.TheSupplierRelationInfo = theSupplier;
            matBalanceMaster.SupplierName = theSupplier.SupplierInfo.Name;
            matBalanceMaster.ProjectId = ProjectInfo.Id;
            matBalanceMaster.ProjectName = ProjectInfo.Name;
            if (task != null)
            {
                matBalanceMaster.UsedPart = task;
                matBalanceMaster.UsedPartName = task.Name;
                matBalanceMaster.UsedPartSysCode = task.SysCode;
            }
            if (ProphaseMatUnusedBal != null)
                matBalanceMaster.StartDate = ProphaseMatUnusedBal.EndDate.AddDays(1);
            else
                matBalanceMaster.StartDate = ClientUtil.ToDateTime("1900-1-1");
            matBalanceMaster.EndDate = Convert.ToDateTime(OperEndDate);
            matBalanceMaster.SumMatMoney = sumMoney + otherMoney;
            //matBalanceMaster.SumMatQuantity = sumQuantity;
            MaterialRentalOrderMaster MatRenMaster = new MaterialRentalOrderMaster();
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));
            IList lst = GetMaterialRentalOrder(oq) as IList;
            if (lst.Count > 0)
            {
                MatRenMaster = lst[0] as MaterialRentalOrderMaster;
            }
            matBalanceMaster.OldContractNum = MatRenMaster.OriginalContractNo;

            matBalanceMaster = SaveByDao(matBalanceMaster) as MaterialBalanceMaster;
            #endregion

            #region 5.收退料单主表加上结算标记
            foreach (MaterialCollectionMaster master in list_matCollMaster)
            {
                if (master != null)
                {
                    master.BalYear = fiscalYear;
                    master.BalMonth = fiscalMonth;
                    master.BalState = 1;
                    UpdateByDao(master);
                }
            }
            foreach (MaterialReturnMaster master in list_matReturnMaster)
            {
                if (master != null)
                {
                    master.BalYear = fiscalYear;
                    master.BalMonth = fiscalMonth;
                    master.BalState = 1;
                    UpdateByDao(master);
                }
            }
            #endregion

            #region 生成本月的料具结算信息(商务)
            MaterialSettleMaster masterMat = new MaterialSettleMaster();
            masterMat.AuditMonth = matBalanceMaster.AuditMonth;
            masterMat.AuditYear = matBalanceMaster.AuditYear;
            Hashtable ht = new Hashtable();
            foreach (MaterialBalanceDetail detail in matBalanceMaster.Details)
            {
                if (detail.UsedPart != null && detail.SubjectGUID != null && detail.MaterialResource != null)
                {
                    string linkStr = detail.UsedPart.Id + "-" + detail.MaterialResource.Id + "-" + detail.SubjectGUID.Id;
                    if (ht.Contains(linkStr))
                    {
                        MaterialBalanceDetail temp = (MaterialBalanceDetail)ht[linkStr];
                        temp.TempData = (TransUtil.ToDecimal(temp.TempData) + detail.ExitQuantity + detail.ApproachQuantity) + "";
                        temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) + detail.Money) + "";
                        ht.Remove(linkStr);
                        ht.Add(linkStr, temp);
                    }
                    else
                    {
                        detail.TempData = (detail.ExitQuantity + detail.ApproachQuantity) + "";
                        detail.TempData1 = detail.Money + "";
                        ht.Add(linkStr, detail);
                    }
                }
            }
            foreach (MatBalOtherCostDetail otherDetail in matBalanceMaster.MatBalOtherCostDetails)
            {
                if (otherDetail.UsedPart != null && otherDetail.SubjectGUID != null && otherDetail.MaterialResource != null)
                {
                    string linkStr = otherDetail.UsedPart.Id + "-" + otherDetail.MaterialResource.Id + "-" + otherDetail.SubjectGUID.Id;
                    if (ht.Contains(linkStr))
                    {
                        MaterialBalanceDetail temp = (MaterialBalanceDetail)ht[linkStr];
                        temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) + otherDetail.CostMoney) + "";
                        ht.Remove(linkStr);
                        ht.Add(linkStr, temp);
                    }
                    else
                    {
                        MaterialBalanceDetail temp = new MaterialBalanceDetail();
                        temp.MaterialCode = otherDetail.MaterialCode;
                        temp.MaterialResource = otherDetail.MaterialResource;
                        temp.MaterialName = otherDetail.MaterialName;
                        temp.MatStandardUnit = otherDetail.MatStandardUnit;
                        temp.MatStandardUnitName = otherDetail.MatStandardUnitName;
                        temp.UsedPart = otherDetail.UsedPart;
                        temp.UsedPartSysCode = otherDetail.UsedPartSysCode;
                        temp.UsedPartName = otherDetail.UsedPartName;
                        temp.TempData1 = otherDetail.CostMoney + "";
                        temp.SubjectGUID = otherDetail.SubjectGUID;
                        temp.SubjectName = otherDetail.SubjectName;
                        temp.SubjectSysCode = otherDetail.SubjectSysCode;
                        ht.Add(linkStr, temp);
                    }
                }
            }
            if (ht.Count > 0)
            {
                foreach (MaterialBalanceDetail dtl in ht.Values)
                {
                    MaterialSettleDetail del = new MaterialSettleDetail();
                    del.MaterialCode = dtl.MaterialCode;
                    del.MaterialResource = dtl.MaterialResource;
                    del.MaterialName = dtl.MaterialName;
                    del.MatStandardUnit = dtl.MatStandardUnit;
                    del.MatStandardUnitName = dtl.MatStandardUnitName;
                    del.ProjectTask = dtl.UsedPart;
                    del.ProjectTaskCode = dtl.UsedPartSysCode;
                    del.ProjectTaskName = dtl.UsedPartName;
                    del.Quantity = TransUtil.ToDecimal(dtl.TempData);
                    del.Money = TransUtil.ToDecimal(dtl.TempData1);
                    if (del.Quantity != 0)
                    {
                        del.Price = decimal.Round(del.Money / del.Quantity, 4);
                    }
                    del.AccountCostSubject = dtl.SubjectGUID;
                    del.AccountCostName = dtl.SubjectName;
                    del.AccountCostCode = dtl.SubjectSysCode;
                    del.Master = masterMat;
                    masterMat.AddDetail(del);
                }
                //增加费用调整
                if (otherMoney != 0)
                {
                    MaterialSettleDetail del = new MaterialSettleDetail();
                    del.MaterialCode = material.Code;
                    del.MaterialResource = material;
                    del.MaterialName = material.Name;
                    del.ProjectTask = task;
                    del.ProjectTaskCode = task.SysCode;
                    del.ProjectTaskName = task.Name;
                    del.Quantity = 0;
                    del.Money = otherMoney;
                    del.Price = 0;
                    del.AccountCostSubject = otherSubject;
                    del.AccountCostName = otherSubject.Name;
                    del.AccountCostCode = otherSubject.SysCode;
                    del.Master = masterMat;
                    masterMat.AddDetail(del);
                }
            }
            //matBalanceMaster
            masterMat.CreatePerson = matBalanceMaster.CreatePerson;
            masterMat.CreatePersonName = matBalanceMaster.CreatePersonName;
            masterMat.OperOrgInfoName = matBalanceMaster.OperOrgInfoName;
            masterMat.OperOrgInfo = matBalanceMaster.OperOrgInfo;
            masterMat.OpgSysCode = matBalanceMaster.OpgSysCode;
            masterMat.HandleOrg = matBalanceMaster.HandleOrg;
            masterMat.HandlePerson = matBalanceMaster.HandlePerson;
            masterMat.HandlePersonName = matBalanceMaster.HandlePersonName;
            masterMat.DocState = DocumentState.InExecute;
            masterMat.ProjectId = ProjectInfo.Id;
            masterMat.ProjectName = ProjectInfo.Name;
            masterMat.CreateDate = OperEndDate;//制单时间
            masterMat.RealOperationDate = DateTime.Now;
            masterMat.CreateYear = fiscalYear;//制单年
            masterMat.CreateMonth = fiscalMonth;//制单月
            masterMat.SettleState = "materialQuery";
            masterMat.MonthlySettlment = matBalanceMaster.Id;
            materialSettleSrv.SaveMaterialSettle(masterMat);
            #endregion
        }

        /// <summary>
        /// 反结账
        /// </summary>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <param name="theSupplier"></param>
        [TransManager]
        public void MaterialUnReckoning(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            #region 1.将收退料的结算标志写回
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            //oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));
            //oq.AddCriterion(Expression.Eq("BalState", 1));
            //oq.AddCriterion(Expression.Eq("BalYear", fiscalYear));
            //oq.AddCriterion(Expression.Eq("BalMonth", fiscalMonth));

            //IList list_collection = GetMaterialCollectionMaster(oq);
            //IList list_return = GetMaterialReturnMaster(oq);

            //foreach (MaterialCollectionMaster master in list_collection)
            //{
            //    if (master != null)
            //    {
            //        master.BalYear = 0;
            //        master.BalMonth = 0;
            //        master.BalState = 0;
            //        UpdateByDao(master);
            //    }
            //}
            //foreach (MaterialReturnMaster master in list_return)
            //{
            //    if (master != null)
            //    {
            //        master.BalYear = 0;
            //        master.BalMonth = 0;
            //        master.BalState = 0;
            //        UpdateByDao(master);
            //    }
            //}
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            string sql = " update thd_materialcollectionmaster t1 set t1.balstate=0,t1.balyear=0,t1.balmonth=0 where t1.projectid='" + ProjectInfo.Id + "' " +
                            " and t1.supplierrelation='" + theSupplier.Id + "' and t1.balstate=1 and t1.balyear=" + fiscalYear + " and t1.balmonth=" + fiscalMonth;
            command.CommandText = sql;
            command.ExecuteNonQuery();


            sql = " update thd_materialreturnmaster t1 set t1.balstate=0,t1.balyear=0,t1.balmonth=0 where t1.projectid='" + ProjectInfo.Id + "' " +
                            " and t1.supplierrelation='" + theSupplier.Id + "' and t1.balstate=1 and t1.balyear=" + fiscalYear + " and t1.balmonth=" + fiscalMonth;
            command.CommandText = sql;
            command.ExecuteNonQuery();
            #endregion

            #region 2.删除结算信息
            MaterialBalanceMaster MatBalMaster = GetMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);
            if (MatBalMaster != null)
                this.DeleteByDao(MatBalMaster);
            #endregion

            //删除料具结算信息(商务)
            this.DeleteMaterialSettleMaster(fiscalYear, fiscalMonth, ProjectInfo.Id);
        }
        /// <summary>
        /// 判断是否核算 true：商务已经对该结算记录核算了 或者  没有找到该结算记录
        /// false：有结算记录 商务没有核算
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">月</param>
        /// <param name="theSupplier">提供商</param>
        /// <param name="ProjectInfo">项目</param>
        /// <param name="sMsg">错误提示</param>
        /// <returns></returns>
        public bool IsAccount(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo, ref string sMsg)
        {
            MaterialBalanceMaster oMaterialBalanceMaster = GetMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);
            if (oMaterialBalanceMaster == null)
            {
                sMsg = "料具商[" + theSupplier.SupplierInfo.Name + "]的[" + fiscalYear + "-" + fiscalMonth + "]月结记录不存在！";
                return true;
            }
            else
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                string sSQL = string.Format("select count(*) from thd_materialsettlemaster t where t.monthaccountbill is not null and  t.monthlysettlment='{0}'", oMaterialBalanceMaster.Id);
                command.CommandText = sSQL;
                int iCount = int.Parse(command.ExecuteScalar().ToString());
                if (iCount > 0)
                {
                    sMsg = "商务已经成本核算，料具商[" + theSupplier.SupplierInfo.Name + "]的[" + fiscalYear + "-" + fiscalMonth + "]月结不能反结！";
                    return true;
                }
                else
                {
                    sMsg = string.Empty;
                    return false;
                }
            }
        }



        /// <summary>
        /// 根据会计年，会计月删除材料消耗信息
        /// </summary>
        /// <returns></returns>
        private void DeleteMaterialSettleMaster(int kjn, int kjy, string projectId)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                string sql = " delete from THD_MaterialSettleMaster t1 where createyear = " + kjn + " and createmonth=" + kjy + " and settlestate='materialQuery' and projectid='" + projectId + "'";

                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 判断当前料具商时候是第一次结账
        /// </summary>
        /// <param name="theSupplier"></param>
        /// <returns></returns>
        public bool CheckIsNotFirstReckoning(SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "SELECT * FROM	THD_MaterialBalanceMaster WHERE SupplierRelation ='" + theSupplier.Id + "' and ProjectId='" + ProjectInfo.Id + "'";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable table = dataSet.Tables[0];
            int count = table.Rows.Count;
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 校验该会计年月是否能结账
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">会计月</param>
        /// <param name="theSupplier">租赁商</param>
        public bool CheckReckoning(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            int previousYear = TransUtil.GetLastYear(fiscalYear, fiscalMonth);
            int previousMonth = TransUtil.GetLastMonth(fiscalYear, fiscalMonth);
            MaterialBalanceMaster master = GetMatBalanceMaster(previousYear, previousMonth, theSupplier, ProjectInfo);
            if (master != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 校验该供应商当前会计年月是否已经结账
        /// </summary>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <param name="theSupplier"></param>
        /// <returns></returns>
        public bool CheckReckoningCurrentMonth(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            MaterialBalanceMaster master = GetMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);
            if (master != null)
            {
                //已经结账
                return true;
            }
            else
            {
                //未结账
                return false;
            }

        }

        /// <summary>
        /// 校验该会计年月是否能反结账
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">会计月</param>
        /// <param name="theSupplier">租赁商</param>
        public bool CheckUnReckoning(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            int nextYear = TransUtil.GetNextYear(fiscalYear, fiscalMonth);
            int nextMonth = TransUtil.GetNextMonth(fiscalYear, fiscalMonth);
            MaterialBalanceMaster master = GetMatBalanceMaster(nextYear, nextMonth, theSupplier, ProjectInfo);
            if (master != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 设备租赁结算
        /// <summary>
        /// 通过ID查询设备租赁结算信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaterialRentalSettlementMaster GetMaterialRentalSettlementById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddFetchMode("Details.MaterialSubjectDetails.SettleSubject", NHibernate.FetchMode.Eager);
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialRentalSettlement(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialRentalSettlementMaster;
            }
            return null;
        }

        public decimal GetAccumulativeTotalMoney(string projectid, string supplier, DateTime lastdate)
        {
            decimal result = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = string.Format(@"select sum(t.summoney) as AccumulativeTotalMoney from THD_MaterialRentelSetMaster t
                                      where t.projectid = '{0}' and t.suppliername = '{1}'
                                      and t.state=5 and t.EndDate<=to_date('{2}','yyyy-mm-dd')", projectid, supplier, lastdate.ToShortDateString());
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet != null)
            {
                if (dataSet.Tables[0] != null)
                {
                    if (dataSet.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow dr in dataSet.Tables[0].Rows)
                        {
                            result = ClientUtil.ToDecimal(dr["AccumulativeTotalMoney"]);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 通过Code查询设备租赁结算信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MaterialRentalSettlementMaster GetMaterialRentalSettlementByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetMaterialRentalSettlement(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialRentalSettlementMaster;
            }
            return null;
        }

        /// <summary>
        /// 设备租赁结算查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialRentalSettlement(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.UsedPart", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialSubjectDetails", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialRentalSettlementMaster), objectQuery);
        }

        public IList GetMaterialSubjectByParentId(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("MasterCost.Id", id));
            return Dao.ObjectQuery(typeof(MaterialSubjectDetail), oq);
        }

        /// <summary>
        /// 设备租赁结算查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet MaterialRentalSettlementQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t2.Id,t1.Code,t1.SupplierName,t1.SupplierRelation,t1.AuditDate,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.Quantity,t2.SettleMoney,t1.State,
                t1.CreateDate,t1.CreatePersonName,t1.monthaccountbillid,t1.HandlePersonName,t1.RealOperationDate,t2.Descript, t3.balancesubjectname
                FROM THD_MaterialRentelSetMaster t1 INNER JOIN THD_MaterialRentalSetDetail t2 ON t1.Id = t2.ParentId left join 
                        thd_subcontractbalsubjectdtl t3 on t2.id = t3.parentid";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        [TransManager]
        public MaterialRentalSettlementMaster AddMaterialRentalSettlement(MaterialRentalSettlementMaster obj)
        {
            obj.Code = GetCode(typeof(MaterialRentalSettlementMaster), obj.ProjectId);
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as MaterialRentalSettlementMaster;
        }

        [TransManager]
        public MaterialRentalSettlementMaster SaveMaterialRentalSettlement(MaterialRentalSettlementMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(MaterialRentalSettlementMaster), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as MaterialRentalSettlementMaster;
        }
        #endregion
        /// <summary>
        /// 保存或修改料具退还
        /// </summary>
        /// <param name="item">收发邀请函</param>
        /// <returns></returns>
        [TransManager]
        public ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item)
        {
            dao.SaveOrUpdate(item);

            return item;
        }

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            bool flag = false;
            foreach (ProObjectRelaDocument cg in list)
            {
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    dis.Add(Expression.Eq("Id", cg.Id));
                    flag = true;
                }
            }
            if (flag)
            {
                oq.AddCriterion(dis);

                IList listTemp = dao.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                if (listTemp != null && listTemp.Count > 0)
                    dao.Delete(listTemp);
            }

            return true;
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

        public void UpdatePrintTimes(int table, string id)
        {
            string useTable = "";
            switch (table)
            {
                case 1:
                    useTable = "thd_materialrentelsetmaster";
                    break;
            }
            if (useTable == "")
            {
                return;
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql = " update " + useTable + " set printtimes = nvl(printtimes,0) + 1 where id = '" + id + "'";
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }
        public DataSet QueryForMaterialRentalSettlementPrint(string conditon)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = conditon;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds;
        }

        public int QueryPrintTimes(int table, string id)
        {
            int times = 0;
            string useTable = "";
            switch (table)
            {
                case 1:
                    useTable = "thd_materialrentelsetmaster";
                    break;
            }
            if (useTable == "")
            {
                return -1;
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql = " select nvl(printtimes,0) printtimes from " + useTable + " where id = '" + id + "' ";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                {
                    times = TransUtil.ToInt(oDataRow["printtimes"]);
                }
            }
            return times;
        }

        #region 设备租赁合同
        /// <summary>
        /// 通过ID查询设备租赁合同信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaterialRentalContractMaster GetMaterialRentalContractById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialRentalContract(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialRentalContractMaster;
            }
            return null;
        }

        [TransManager]
        public MaterialRentalContractMaster SaveMaterialRentalContract(MaterialRentalContractMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(MaterialRentalContractMaster), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as MaterialRentalContractMaster;
        }

        /// <summary>
        /// 设备租赁合同查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialRentalContract(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.UsedPart", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialRentalContractMaster), objectQuery);
        }
        #endregion
    }
}
