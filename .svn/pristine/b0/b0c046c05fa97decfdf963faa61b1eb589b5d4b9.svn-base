using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using System.Collections;
using CommonSearchLib.BillCodeMng.Service;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Service;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireLedger;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireBalance.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireLedger.Domain;
using System.Windows.Forms;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Util;
using VirtualMachine.Component.Util;
using Application.Resource.CommonClass.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireSettlement;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireTransportCost.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.Service
{
    /// <summary>
    /// 料具租赁管理服务
    /// </summary>
    public class MaterialHireMngSvr :BaseService, IMaterialHireMngSvr
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
        private string GetCode(Type type, DateTime time, string sProject)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", time, sProject);
        }
        private string GetCode(Type type )
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code",DateTime.Now);
        } 
        private string GetCode(Type type, EnumMatHireType matHireType,string sProjectId )
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, sProjectId, Enum.GetName(typeof(EnumMatHireType), matHireType));
        }
        private string GetCode(Type type, EnumMatHireType matHireType, bool IsLoss, string sProjectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, sProjectId, Enum.GetName(typeof(EnumMatHireType), matHireType) + (IsLoss ? "(损耗)" : ""));
        }
        private string GetCode(Type type, bool IsLoss, string sProjectId)
        {
            if (IsLoss)
            {
                return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, sProjectId, "损耗");
            }
            else
            {
                return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, sProjectId);
            }
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
        public MatHireOrderMaster GetMaterialHireOrderById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialHireOrder(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireOrderMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询料具租赁合同
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MatHireOrderMaster GetMaterialHireOrderByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetMaterialHireOrder(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireOrderMaster;
            }
            return null;
        }
        public MatHireOrderMaster GetMaterialHireOrder(SupplierRelationInfo theSupplier,CurrentProjectInfo ProjectInfo)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("TheSupplierRelationInfo.Id", theSupplier.Id));
            oQuery.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            IList list = GetMaterialHireOrder(oQuery) as IList;
            return list == null || list.Count == 0 ? null : list[0] as MatHireOrderMaster;
        }
        /// <summary>
        /// 查询料具租赁合同
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialHireOrder(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("BasiCostSets", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.BasicDtlCostSets", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MatHireOrderMaster), objectQuery);
        }
        
        /// <summary>
        /// 查询料具租赁合同明细信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialHireOrderDetail(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Master.TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master.TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master.BasiCostSets", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MatStandardUnit", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master.OperOrgInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("BasicDtlCostSets", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MatHireOrderDetail), objectQuery);
        }

        /// <summary>
        /// 料具租赁合同查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet MaterialHireOrderQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.code,t1.realoperationdate,t1.createdate,t1.originalContractNo,t1.BillCode,t5.ORGNAME,t1.docstate state,t7.MATCODE,t7.MATNAME,t7.MATSPECIFICATION,
                t2.Quantity,t2.price,t1.balRule,t8.STANDUNITNAME,t2.Descript,t1.PrintTimes
                FROM thd_MaterialHireOrderMaster t1 Left JOIN thd_MaterialHireOrderDetail t2 ON t1.id=t2.parentId
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
        public bool VerifyCurrSupplierOrder(SupplierRelationInfo theSupplier, MatHireOrderMaster Master)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "";
            if (Master.Id != null)
            {
                sql = "SELECT * FROM thd_MaterialHireOrderMaster WHERE SupplierRelation ='" + theSupplier.Id + "'and Id !='" + Master.Id + "' and projectid='" + Master.ProjectId + "'";

            }
            else
            {

                sql = "SELECT * FROM thd_MaterialHireOrderMaster WHERE SupplierRelation ='" + theSupplier.Id + "' and projectid='" + Master.ProjectId + "'";

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
        public MatHireOrderMaster SaveMaterialHireOrderMaster(MatHireOrderMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(MatHireOrderMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as MatHireOrderMaster;
        }
        #endregion

        #region 料具收料方法
        /// <summary>
        /// 保存收料单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public MatHireCollectionMaster SaveMaterialHireCollectionMaster(MatHireCollectionMaster obj, bool CanReturnUp)
        {
       
            MatHireReturnDetail ReturnDetail = null;
            MatHireReturnDetailSeq theMatHireReturnDetailSeq=null;
            bool isNew = true;
            if (obj.Id == null)
            {
                obj.Code = obj.MatHireType == EnumMatHireType.普通料具 ? GetCode(typeof(MatHireCollectionMaster),DateTime.Now ,obj.ProjectId) : GetCode(typeof(MatHireCollectionMaster),obj.MatHireType,obj.ProjectId);
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
            IList<MatHireLedgerMaster> lst_MatRenLedTemp = new List<MatHireLedgerMaster>();//存储发料量为负数
            foreach (MatHireCollectionDetail detail in obj.Details)
            {
                if (detail.RealQuantity < 0 || (detail.RealQuantity > 0 && CanReturnUp))//(detail.Quantity < 0)
                {
                    //= new MatHireReturnDetailSeq()  ;
                    //添加 租赁类型 材料长度
                    //theMatHireReturnDetailSeq.MaterialLength = detail.MaterialLength;
                    // theMatHireReturnDetailSeq.MatHireType = obj.MatHireType;
                    ReturnDetail = new MatHireReturnDetail();
                    ReturnDetail.MaterialResource = detail.MaterialResource;
                    ReturnDetail.ExitQuantity = -detail.Quantity;//Math.Abs(detail.Quantity);
                    ReturnDetail.MaterialLength = detail.MaterialLength;//长度
                    IList list = GetMatLeftQuantityByNew(ReturnDetail, obj.TheSupplierRelationInfo, detail.BorrowUnit, obj.ProjectId, obj.MatHireType);
                    foreach (MatHireLedgerMaster theMatHireLedgerMaster in list)
                    {
                        theMatHireReturnDetailSeq = new MatHireReturnDetailSeq();
                        //判断当前台账的类型：0：收料；1：退料(退料是负数时产生的收料数量参数到先进先出的退料算法中)
                        if (theMatHireLedgerMaster.WashType == 0)
                        {
                            theMatHireReturnDetailSeq.MatCollDtlId = theMatHireLedgerMaster.BillDetailId;
                        }
                        else
                        {
                            theMatHireReturnDetailSeq.MatReturnDtlId = theMatHireLedgerMaster.BillDetailId;
                        }
                        theMatHireReturnDetailSeq.SeqType = detail.RealQuantity <= 0 ? "收料(数量小于0)" : "收料(数量大于0)";
                        theMatHireReturnDetailSeq.MatLedgerId = theMatHireLedgerMaster.Id;
                        theMatHireReturnDetailSeq.ReturnQuantity = theMatHireLedgerMaster.TempQuantity;
                        theMatHireReturnDetailSeq.ReturnDate = obj.CreateDate;
                        theMatHireReturnDetailSeq.MatHireType = obj.MatHireType;//料具类型
                        theMatHireReturnDetailSeq.MaterialLength = detail.MaterialLength;//料具长度
                        theMatHireReturnDetailSeq.MaterialType = detail.MaterialType;//型号
                        //收退料单号和收退料明细数量
                        theMatHireReturnDetailSeq.MatCollCode = obj.Code;
                        theMatHireReturnDetailSeq.MatCollDtlQty = detail.RealQuantity; //detail.Quantity * detail.MaterialLength;
                        theMatHireReturnDetailSeq.MatReturnCode = theMatHireLedgerMaster.BillCode;
                        theMatHireReturnDetailSeq.MatReturnDtlQty = theMatHireLedgerMaster.ReturnQuantity;

                        list_MatRenLed.Add(theMatHireLedgerMaster);

                        detail.AddMatReturnDtlSeq(theMatHireReturnDetailSeq);
                    }
                }

            }
            MatHireCollectionMaster master = SaveByDao(obj) as MatHireCollectionMaster;

            #endregion

            #region 2.更新台账(收料剩余数量)
            foreach (MatHireLedgerMaster MatHireLedgerMaster in list_MatRenLed)
            {
                UpdateByDao(MatHireLedgerMaster);
            }
            #endregion

            #endregion

            #region 2. 生成台账信息
            foreach (MatHireCollectionDetail detail in master.Details)
            {
                if (detail.RealQuantity == 0) continue;
                MatHireLedgerMaster theMatHireLedgerMaster = new MatHireLedgerMaster();
                theMatHireLedgerMaster.TheSupplierRelationInfo = master.TheSupplierRelationInfo;
                theMatHireLedgerMaster.SupplierName = master.SupplierName;
                theMatHireLedgerMaster.TheRank = detail.BorrowUnit;
                theMatHireLedgerMaster.TheRankName = detail.BorrowUnitName;
                theMatHireLedgerMaster.ProjectId = master.ProjectId;
                theMatHireLedgerMaster.ProjectName = master.ProjectName;
                theMatHireLedgerMaster.WashType = 0;//收料
                theMatHireLedgerMaster.BillCode = master.Code;
                theMatHireLedgerMaster.BillId = master.Id;
                theMatHireLedgerMaster.OldContractNum = master.OldContractNum;
                theMatHireLedgerMaster.SystemDate = DateTime.Now;
                theMatHireLedgerMaster.RealOperationDate = master.CreateDate;
                theMatHireLedgerMaster.BillDetailId = detail.Id;
                theMatHireLedgerMaster.MatHireType = master.MatHireType;//料具类型
                theMatHireLedgerMaster.MaterialLength = detail.MaterialLength;//物资长度
                theMatHireLedgerMaster.MaterialType = detail.MaterialType;//型号
                if (CanReturnUp)//退超问题
                {//结存=收料量+退料量
                    theMatHireLedgerMaster.LeftQuantity = detail.RealQuantity + detail.MaterialReturnDetailSeqs.OfType<MatHireReturnDetailSeq>().Sum(a => a.ReturnQuantity);
                }
                else
                {
                    if (detail.RealQuantity < 0)//(detail.Quantity < 0)
                    {
                        theMatHireLedgerMaster.LeftQuantity = 0;
                        lst_MatRenLedTemp.Add(theMatHireLedgerMaster);//将负数发料台帐记录存储起来
                    }
                    else
                    {
                        theMatHireLedgerMaster.LeftQuantity = detail.RealQuantity;//Quantity*detail.MaterialLength;
                    }
                }
                theMatHireLedgerMaster.CollectionQuantity = detail.RealQuantity;// Quantity;
                theMatHireLedgerMaster.MaterialResource = detail.MaterialResource;
                theMatHireLedgerMaster.MaterialCode = detail.MaterialCode;
                theMatHireLedgerMaster.MaterialName = detail.MaterialName;
                theMatHireLedgerMaster.MaterialSpec = detail.MaterialSpec;
                theMatHireLedgerMaster.MatStandardUnit = detail.MatStandardUnit;
                theMatHireLedgerMaster.MatStandardUnitName = detail.MatStandardUnitName;
                theMatHireLedgerMaster.UsedPart = detail.UsedPart;
                theMatHireLedgerMaster.UsedPartName = detail.UsedPartName;
                theMatHireLedgerMaster.UsedPartSysCode = detail.UsedPartSysCode;
                theMatHireLedgerMaster.SubjectGUID = detail.SubjectGUID;
                theMatHireLedgerMaster.SubjectName = detail.SubjectName;
                theMatHireLedgerMaster.SubjectSysCode = detail.SubjectSysCode;
                theMatHireLedgerMaster.RentalPrice = detail.RentalPrice;

                SaveByDao(theMatHireLedgerMaster);
            }
            #endregion

            #region 3.校验退料明细数量是否退料时序的数量(和)相等
            if (!CanReturnUp)
            {
                //出现退超了 会将退超的量写到退超对应台帐明细上的 LeftQuantity=-退超量
                MatHireLedgerMaster oMatHireLedgerMasterUp = null;
                string sMsgTemp = string.Empty;
                foreach (MatHireCollectionDetail Detail in master.Details)
                {
                    if (Detail.RealQuantity < 0)//(Detail.Quantity < 0)
                    {
                        decimal ReturnSeqQty = 0;
                        foreach (MatHireReturnDetailSeq seq in Detail.MaterialReturnDetailSeqs)
                        {
                            ReturnSeqQty += Math.Abs(seq.ReturnQuantity);
                        }

                        if (ReturnSeqQty < Math.Abs(Detail.RealQuantity)) //Math.Abs(Detail.Quantity * Detail.MaterialLength))
                        {
                            //MessageBox.Show("收料负数产生的退料数量出错！");
                            oMatHireLedgerMasterUp = lst_MatRenLedTemp.FirstOrDefault(a => a.BillDetailId == Detail.Id);
                            if (oMatHireLedgerMasterUp != null)
                            {
                                oMatHireLedgerMasterUp.LeftQuantity = ReturnSeqQty + Detail.RealQuantity;
                                sMsgTemp = string.Format("编码为[{0}]的[{1}]物资在发料时,发料量为[{2}],已退料量[{3}],结存量[{4}]",
                                    Detail.MaterialCode, Detail.MaterialName, Detail.RealQuantity, ReturnSeqQty, Detail.RealQuantity + ReturnSeqQty);
                                oMatHireLedgerMasterUp.Descript = string.Format("{0},时间:{1}", sMsgTemp, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            }

                        }
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
        public MatHireCollectionMaster UpdateMaterialHireCollectionMaster(MatHireCollectionMaster obj, IList list_DeleteMatCollDtl)
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
            foreach (MatHireCollectionDetail detail in obj.Details)
            {
                foreach (MatHireReturnDetailSeq seq in detail.MaterialReturnDetailSeqs)
                {
                    list_seq.Add(seq);
                }
            }
            #region 1.删除一条或者多条明细：针对收料数量为负数的明细,然后根据退料的时序表更新台账记录
            if (list_DeleteMatCollDtl.Count > 0)
            {
                foreach (MatHireCollectionDetail MatHireCollectionDetail in list_DeleteMatCollDtl)
                {
                    foreach (MatHireReturnDetailSeq seq in MatHireCollectionDetail.MaterialReturnDetailSeqs)
                    {
                        MatHireLedgerMaster MatHireLedgerMaster = GetMaterialHireLedgerMasterById(seq.MatLedgerId);
                        MatHireLedgerMaster.LeftQuantity = MatHireLedgerMaster.LeftQuantity + seq.ReturnQuantity;
                        UpdateByDao(MatHireLedgerMaster);
                    }
                }
            }
            #endregion

            #region 2.更新退料时序：针对收料数量为负数的明细
            //根据先进先出算法重新组织退明细料时序表
            IList list_MatRenLed = new ArrayList();

            foreach (MatHireCollectionDetail detail in obj.Details)
            {
                if (detail.Quantity < 0)
                {
                    MatHireReturnDetailSeq theMatHireReturnDetailSeq = new MatHireReturnDetailSeq();

                    MatHireReturnDetail ReturnDetail = new MatHireReturnDetail();
                    ReturnDetail.MaterialResource = detail.MaterialResource;
                    ReturnDetail.ExitQuantity = Math.Abs(detail.Quantity);

                    IList list = GetMatLeftQuantityByModify(ReturnDetail, obj.TheSupplierRelationInfo, detail.BorrowUnit, obj.ProjectId);
                    foreach (MatHireLedgerMaster MatHireLedgerMaster in list)
                    {
                        theMatHireReturnDetailSeq = new MatHireReturnDetailSeq();
                        //判断当前台账的类型：0：收料；1：退料（退料是负数时产生的收料数量参数到先进先出的退料算法中）
                        if (MatHireLedgerMaster.WashType == 0)
                        {
                            theMatHireReturnDetailSeq.MatCollDtlId = MatHireLedgerMaster.BillDetailId;
                        }
                        else
                        {
                            theMatHireReturnDetailSeq.MatReturnDtlId = MatHireLedgerMaster.BillDetailId;
                        }
                        theMatHireReturnDetailSeq.SeqType = "收料(数量小于0)";
                        theMatHireReturnDetailSeq.MatCollDtlId = MatHireLedgerMaster.BillDetailId;
                        theMatHireReturnDetailSeq.MatLedgerId = MatHireLedgerMaster.Id;
                        theMatHireReturnDetailSeq.ReturnQuantity = MatHireLedgerMaster.TempQuantity;
                        theMatHireReturnDetailSeq.ReturnDate = obj.CreateDate;

                        //收退料单号和收退料明细数量
                        theMatHireReturnDetailSeq.MatCollCode = obj.Code;
                        theMatHireReturnDetailSeq.MatCollDtlQty = detail.Quantity;
                        theMatHireReturnDetailSeq.MatReturnCode = MatHireLedgerMaster.BillCode;
                        theMatHireReturnDetailSeq.MatReturnDtlQty = MatHireLedgerMaster.ReturnQuantity;

                        list_MatRenLed.Add(MatHireLedgerMaster);

                        detail.AddMatReturnDtlSeq(theMatHireReturnDetailSeq);
                    }
                }
            }

            //清除原来的退料明细时序
            foreach (MatHireReturnDetailSeq seq in list_seq)
            {
                foreach (MatHireCollectionDetail detail in obj.Details)
                {
                    detail.MaterialReturnDetailSeqs.Remove(seq);
                }
            }

            //更新台账(收料剩余数量)
            foreach (MatHireLedgerMaster MatHireLedgerMaster in list_MatRenLed)
            {
                UpdateByDao(MatHireLedgerMaster);
            }
            MatHireCollectionMaster master = UpdateByDao(obj) as MatHireCollectionMaster;


            #endregion

            #endregion

            #region 2.处理台账

            //处理1:获取料具台账信息
            MatHireLedgerMaster theMatHireLedgerMaster = null;
            list_MatRenLed = new ArrayList();
            list_MatRenLed = this.GetMaterialHireLedgerByMatReturnCollId(master.Id);

            //处理2:收料没有，台账有
            IList notExistList = new ArrayList();

            if (list_MatRenLed != null)
            {
                foreach (MatHireLedgerMaster MatHireLedgerMaster in list_MatRenLed)
                {
                    bool isExist = false;
                    foreach (MatHireCollectionDetail detail in master.Details)
                    {
                        if (MatHireLedgerMaster.BillDetailId == detail.Id)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (isExist == false)
                    {
                        notExistList.Add(MatHireLedgerMaster);
                    }
                }
            }

            //处理3:删除(修改时删除一条或者多条收料明细)
            foreach (MatHireLedgerMaster MatHireLedgerMaster in notExistList)
            {
                DeleteByDao(MatHireLedgerMaster);
            }
            //处理4：更新台账信息
            foreach (MatHireCollectionDetail detail in master.Details)
            {
                int flag = 1;//1:收料有,台账没有(新增一条台账信息) 3:收料有，台账有(更新该条台账信息)
                foreach (MatHireLedgerMaster MatHireLedgerMaster in list_MatRenLed)
                {
                    if (detail.Id == MatHireLedgerMaster.BillDetailId)
                    {
                        flag = 3;
                        theMatHireLedgerMaster = MatHireLedgerMaster;
                        break;
                    }
                }
                if (flag == 1)
                {
                    theMatHireLedgerMaster = new MatHireLedgerMaster();
                }

                theMatHireLedgerMaster.TheSupplierRelationInfo = master.TheSupplierRelationInfo;
                theMatHireLedgerMaster.SupplierName = master.SupplierName;
                theMatHireLedgerMaster.TheRank = detail.BorrowUnit;
                theMatHireLedgerMaster.TheRankName = detail.BorrowUnitName;
                theMatHireLedgerMaster.ProjectId = master.ProjectId;
                theMatHireLedgerMaster.ProjectName = master.ProjectName;
                theMatHireLedgerMaster.WashType = 0;//收料
                theMatHireLedgerMaster.BillCode = master.Code;
                theMatHireLedgerMaster.BillId = master.Id;
                theMatHireLedgerMaster.OldContractNum = master.OldContractNum;
                theMatHireLedgerMaster.SystemDate = DateTime.Now;
                theMatHireLedgerMaster.RealOperationDate = master.CreateDate;
                theMatHireLedgerMaster.BillDetailId = detail.Id;
                theMatHireLedgerMaster.CollectionQuantity = detail.Quantity;
                theMatHireLedgerMaster.LeftQuantity = detail.Quantity;
                theMatHireLedgerMaster.MaterialResource = detail.MaterialResource;
                theMatHireLedgerMaster.MaterialCode = detail.MaterialCode;
                theMatHireLedgerMaster.MaterialName = detail.MaterialName;
                theMatHireLedgerMaster.MaterialSpec = detail.MaterialSpec;
                theMatHireLedgerMaster.MatStandardUnit = detail.MatStandardUnit;
                theMatHireLedgerMaster.MatStandardUnitName = detail.MatStandardUnitName;
                theMatHireLedgerMaster.UsedPart = detail.UsedPart;
                theMatHireLedgerMaster.UsedPartName = detail.UsedPartName;
                theMatHireLedgerMaster.RentalPrice = detail.RentalPrice;

                SaveOrUpdateByDao(theMatHireLedgerMaster);
            }
            #endregion

            #region 3.校验退料明细数量是否退料时序的数量(和)相等

            foreach (MatHireCollectionDetail Detail in master.Details)
            {
                if (Detail.Quantity < 0)
                {
                    decimal ReturnSeqQty = 0;
                    foreach (MatHireReturnDetailSeq seq in Detail.MaterialReturnDetailSeqs)
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
        public bool DeleteMaterialHireCollectionMaster(MatHireCollectionMaster obj)
        {
            try
            {
                //删除台账信息
                foreach (MatHireCollectionDetail detail in obj.Details)
                {
                    MatHireLedgerMaster master = this.GetMaterialHireLedgerByMatReturnCollDtlId(detail.Id);
                    if (master != null)
                        DeleteByDao(master);
                }
                //更新台账剩余数量
                foreach (MatHireCollectionDetail theMatHireCollectionDetail in obj.Details)
                {
                    foreach (MatHireReturnDetailSeq theMatHireReturnDetailSeq in theMatHireCollectionDetail.MaterialReturnDetailSeqs)
                    {
                        MatHireLedgerMaster master = this.GetMaterialHireLedgerMasterById(theMatHireReturnDetailSeq.MatLedgerId);
                        if (master != null)
                        {
                            master.LeftQuantity = master.LeftQuantity + theMatHireReturnDetailSeq.ReturnQuantity;
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
        public MatHireCollectionMaster GetMaterialHireCollectionMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialHireCollectionMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireCollectionMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询料具收料单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MatHireCollectionMaster GetMaterialHireCollectionMasterByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetMaterialHireCollectionMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireCollectionMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询料具收料单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialHireCollectionMaster(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Contract", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("MatNotQtyCosts", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatCostDtls", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.BorrowUnit", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialReturnDetailSeqs", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MatHireCollectionMaster), objectQuery);
        }
       

        /// <summary>
        /// 料具收料单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet MaterialHireCollectionMasterQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.code,t1.BillCode,t1.PrintTimes,t1.OldContractNum,t1.SupplierName,t2.borrowunitname TheRankName,t1.STATE,t1.Descript,t2.MaterialCode,t2.MaterialName,t1.RealOperationDate,
                t2.MaterialSpec,t2.MatStandardUnitName,t2.Quantity,t2.RentalPrice,t1.BalRule,t1.CreateDate,t1.CreatePersonName,t1.transportcharge,t2.usedpartname,t2.subjectname,t1.projectname  
                ,t2.materiallength,t2.materialtype,t1.mathiretype FROM THD_MatHireCollectionMaster t1 Inner join THD_MatHireCollectionDetail t2 on t1.id=t2.parentid";
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
        public MatHireReturnMaster GetMaterialHireReturnById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialHireReturnMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireReturnMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询料具退料单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MatHireReturnMaster GetMaterialHireReturnByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetMaterialHireReturnMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireReturnMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询料具退料单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialHireReturnMaster(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Contract", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("MatReturnNotQtyCosts", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatReturnCostDtls", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatRepairs", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialReturnDetailSeqs", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MatHireReturnMaster), objectQuery);
        }

        /// <summary>
        /// 料具退料单保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public MatHireReturnMaster SaveMaterialHireReturnMaster(MatHireReturnMaster obj,bool CanReturnUp)
        {
            if (obj.Id == null)
            {
                obj.Code = obj.MatHireType == EnumMatHireType.普通料具 ? GetCode(typeof(MatHireReturnMaster),obj.IsLoss,obj.ProjectId) : GetCode(typeof(MatHireReturnMaster),obj.MatHireType,obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            //1.根据先进先出算法组织退料时序表
            IList list_MatRenLed = new ArrayList();
            foreach (MatHireReturnDetail detail in obj.Details)
            {
                if (detail.RealExitQuantity > 0||(detail.RealExitQuantity<0 && CanReturnUp))//允许退超
                {
                    MatHireReturnDetailSeq theMatHireReturnDetailSeq = null;

                    IList list = GetMatLeftQuantityByNew(detail, obj.TheSupplierRelationInfo, detail.BorrowUnit, obj.ProjectId,obj.MatHireType);
                    foreach (MatHireLedgerMaster theMatHireLedgerMaster in list)
                    {
                        theMatHireReturnDetailSeq = new MatHireReturnDetailSeq();
                        //判断当前台账的类型：0：收料；1：退料(退料是负数时产生的收料数量参数到先进先出的退料算法中)
                        if (theMatHireLedgerMaster.WashType == 0)
                        {
                            theMatHireReturnDetailSeq.MatCollDtlId = theMatHireLedgerMaster.BillDetailId;
                        }
                        else
                        {
                            theMatHireReturnDetailSeq.MatReturnDtlId = theMatHireLedgerMaster.BillDetailId;
                        }
                        theMatHireReturnDetailSeq.SeqType = detail.RealExitQuantity<0 ?"退料(数量小于0)":"退料(数量大于0)";
                        theMatHireReturnDetailSeq.MatLedgerId = theMatHireLedgerMaster.Id;
                        theMatHireReturnDetailSeq.ReturnQuantity = theMatHireLedgerMaster.TempQuantity;
                        theMatHireReturnDetailSeq.ReturnDate = obj.CreateDate;
                        //收退料单号和收退料明细数量
                        theMatHireReturnDetailSeq.MatReturnCode = obj.Code;
                        theMatHireReturnDetailSeq.MatReturnDtlQty = detail.RealExitQuantity;//实际数量
                        theMatHireReturnDetailSeq.MatHireType = obj.MatHireType;//料具类型
                        theMatHireReturnDetailSeq.MaterialLength = detail.MaterialLength;//料具长度
                        theMatHireReturnDetailSeq.MaterialType = detail.MaterialType;//碗扣型号
                        theMatHireReturnDetailSeq.MatCollCode = theMatHireLedgerMaster.BillCode;
                        theMatHireReturnDetailSeq.MatCollDtlQty = theMatHireLedgerMaster.CollectionQuantity;
                        list_MatRenLed.Add(theMatHireLedgerMaster);

                        detail.AddMatReturnDtlSeq(theMatHireReturnDetailSeq);
                    }
                }
            }
            MatHireReturnMaster master = SaveByDao(obj) as MatHireReturnMaster;
            //更新台账(收料剩余数量)
            foreach (MatHireLedgerMaster MatHireLedgerMaster in list_MatRenLed)
            {
                UpdateByDao(MatHireLedgerMaster);
            }

            //2.添加台账信息(退料)
            foreach (MatHireReturnDetail detail in master.Details)
            {
                if (detail.RealExitQuantity == 0) continue;
                MatHireLedgerMaster theMatHireLedgerMaster = new MatHireLedgerMaster();
                theMatHireLedgerMaster.TheSupplierRelationInfo = master.TheSupplierRelationInfo;
                theMatHireLedgerMaster.SupplierName = master.SupplierName;
                theMatHireLedgerMaster.ProjectId = master.ProjectId;
                theMatHireLedgerMaster.ProjectName = master.ProjectName;
                theMatHireLedgerMaster.WashType = 1;//退料
                theMatHireLedgerMaster.BillCode = master.Code;
                theMatHireLedgerMaster.BillId = master.Id;
                theMatHireLedgerMaster.OldContractNum = master.OldContractNum;
                theMatHireLedgerMaster.SystemDate = DateTime.Now;
                theMatHireLedgerMaster.RealOperationDate = master.CreateDate;
                theMatHireLedgerMaster.TheRank = detail.BorrowUnit;
                theMatHireLedgerMaster.TheRankName = detail.BorrowUnitName;
                theMatHireLedgerMaster.BillDetailId = detail.Id;
                theMatHireLedgerMaster.CollectionQuantity = detail.Quantity;
                theMatHireLedgerMaster.MaterialResource = detail.MaterialResource;
                theMatHireLedgerMaster.MaterialCode = detail.MaterialCode;
                theMatHireLedgerMaster.MaterialName = detail.MaterialName;
                theMatHireLedgerMaster.MaterialSpec = detail.MaterialSpec;
                theMatHireLedgerMaster.MatStandardUnit = detail.MatStandardUnit;
                theMatHireLedgerMaster.MatStandardUnitName = detail.MatStandardUnitName;
                theMatHireLedgerMaster.UsedPart = detail.UsedPart;
                theMatHireLedgerMaster.UsedPartName = detail.UsedPartName;
                theMatHireLedgerMaster.RentalPrice = detail.RentalPrice;
                theMatHireLedgerMaster.SubjectGUID = detail.SubjectGUID;
                theMatHireLedgerMaster.SubjectName = detail.SubjectName;
                theMatHireLedgerMaster.SubjectSysCode = detail.SubjectSysCode;
                theMatHireLedgerMaster.MatHireType = obj.MatHireType;//料具类型
                theMatHireLedgerMaster.MaterialLength = detail.MaterialLength;//料具长度
                theMatHireLedgerMaster.MaterialType = detail.MaterialType;//碗扣型号
                //退料为负数：说明为收料
                decimal ExitQuantity = detail.RealExitQuantity;//ExitQuantity;
                if (CanReturnUp)
                {
                    //结存数量=冲抵库存量-退料数量
                    theMatHireLedgerMaster.LeftQuantity = detail.MaterialReturnDetailSeqs.OfType<MatHireReturnDetailSeq>().Sum(a => a.ReturnQuantity) - detail.RealExitQuantity;
                }
                else
                {
                    if (ExitQuantity < 0)
                    {
                        theMatHireLedgerMaster.LeftQuantity = Math.Abs(ExitQuantity);
                    }
                }
                theMatHireLedgerMaster.ReturnQuantity = ExitQuantity;// detail.ExitQuantity;

                SaveByDao(theMatHireLedgerMaster);
            }

            #region 校验退料明细数量是否退料时序的数量(和)相等
            if (!CanReturnUp)
            {
                foreach (MatHireReturnDetail Detail in master.Details)
                {
                    if (Detail.ExitQuantity > 0)
                    {
                        decimal ReturnSeqQty = 0;
                        foreach (MatHireReturnDetailSeq seq in Detail.MaterialReturnDetailSeqs)
                        {
                            ReturnSeqQty += Math.Abs(seq.ReturnQuantity);
                        }

                        if (ReturnSeqQty < Detail.RealExitQuantity)//Detail.ExitQuantity
                        {
                            throw new Exception("退料数量出错！");
                        }
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
        public MatHireReturnMaster UpdateMaterialHireReturnMaster(MatHireReturnMaster obj)
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
            foreach (MatHireReturnDetail detail in obj.Details)
            {
                foreach (MatHireReturnDetailSeq seq in detail.MaterialReturnDetailSeqs)
                {
                    list_seq.Add(seq);
                }
            }
            //删除一条或者多条明细
            IList list_MatReturnDtl = new ArrayList();
            foreach (MatHireReturnDetail MatHireReturnDetail in obj.Details)
            {
                if (MatHireReturnDetail.TempData == "删除")
                {
                    foreach (MatHireReturnDetailSeq seq in MatHireReturnDetail.MaterialReturnDetailSeqs)
                    {
                        MatHireLedgerMaster MatHireLedgerMaster = GetMaterialHireLedgerMasterById(seq.MatLedgerId);
                        MatHireLedgerMaster.LeftQuantity = MatHireLedgerMaster.LeftQuantity + seq.ReturnQuantity;
                        UpdateByDao(MatHireLedgerMaster);
                        list_MatReturnDtl.Add(MatHireReturnDetail);
                    }
                }
            }
            //清除界面上删除的明细
            foreach (MatHireReturnDetail detail in list_MatReturnDtl)
            {
                obj.Details.Remove(detail);
            }

            //根据先进先出算法重新组织退明细料时序表
            IList list_MatRenLed = new ArrayList();
            foreach (MatHireReturnDetail detail in obj.Details)
            {
                if (detail.ExitQuantity > 0)
                {
                    MatHireReturnDetailSeq theMatHireReturnDetailSeq = null;

                    IList list = GetMatLeftQuantityByModify(detail, obj.TheSupplierRelationInfo, detail.BorrowUnit, obj.ProjectId);
                    foreach (MatHireLedgerMaster MatHireLedgerMaster in list)
                    {
                        theMatHireReturnDetailSeq = new MatHireReturnDetailSeq();
                        //判断当前台账的类型：0：收料；1：退料（退料是负数时产生的收料数量参数到先进先出的退料算法中）
                        if (MatHireLedgerMaster.WashType == 0)
                        {
                            theMatHireReturnDetailSeq.MatCollDtlId = MatHireLedgerMaster.BillDetailId;
                        }
                        else
                        {
                            theMatHireReturnDetailSeq.MatReturnDtlId = MatHireLedgerMaster.BillDetailId;
                        }
                        theMatHireReturnDetailSeq.SeqType = "退料";
                        theMatHireReturnDetailSeq.MatCollDtlId = MatHireLedgerMaster.BillDetailId;
                        theMatHireReturnDetailSeq.MatLedgerId = MatHireLedgerMaster.Id;
                        theMatHireReturnDetailSeq.ReturnQuantity = MatHireLedgerMaster.TempQuantity;
                        theMatHireReturnDetailSeq.ReturnDate = obj.CreateDate;

                        //收退料单号和收退料明细数量
                        theMatHireReturnDetailSeq.MatReturnCode = obj.Code;
                        theMatHireReturnDetailSeq.MatReturnDtlQty = detail.ExitQuantity;
                        theMatHireReturnDetailSeq.MatCollCode = MatHireLedgerMaster.BillCode;
                        theMatHireReturnDetailSeq.MatCollDtlQty = MatHireLedgerMaster.CollectionQuantity;

                        list_MatRenLed.Add(MatHireLedgerMaster);

                        detail.AddMatReturnDtlSeq(theMatHireReturnDetailSeq);
                    }
                }
            }

            //清除原来的退料明细时序
            foreach (MatHireReturnDetailSeq seq in list_seq)
            {
                foreach (MatHireReturnDetail detail in obj.Details)
                {
                    detail.MaterialReturnDetailSeqs.Remove(seq);
                }
            }

            //更新台账(收料剩余数量)
            foreach (MatHireLedgerMaster MatHireLedgerMaster in list_MatRenLed)
            {
                UpdateByDao(MatHireLedgerMaster);
            }

            MatHireReturnMaster master = UpdateByDao(obj) as MatHireReturnMaster;

            //处理料具台账
            MatHireLedgerMaster theMatHireLedgerMaster = null;
            IList list_temp = new ArrayList();

            list_temp = this.GetMaterialHireLedgerByMatReturnCollId(master.Id);

            //处理2:退料没有，台账有
            IList notExistList = new ArrayList();

            if (list_MatRenLed != null)
            {
                foreach (MatHireLedgerMaster MatHireLedgerMaster in list_temp)
                {
                    bool isExist = false;
                    foreach (MatHireReturnDetail detail in master.Details)
                    {
                        if (MatHireLedgerMaster.BillDetailId == detail.Id)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (isExist == false)
                    {
                        notExistList.Add(MatHireLedgerMaster);
                    }
                }
            }

            //删除
            foreach (MatHireLedgerMaster MatHireLedgerMaster in notExistList)
            {
                DeleteByDao(MatHireLedgerMaster);
            }
            //更新台账
            foreach (MatHireReturnDetail detail in master.Details)
            {
                int flag = 1;//1:退料有,台账没有 3:退料有，台账有
                foreach (MatHireLedgerMaster MatHireLedgerMaster in list_temp)
                {
                    if (detail.Id == MatHireLedgerMaster.BillDetailId)
                    {
                        flag = 3;
                        theMatHireLedgerMaster = MatHireLedgerMaster;
                        break;
                    }
                }
                if (flag == 1)
                {
                    theMatHireLedgerMaster = new MatHireLedgerMaster();
                }

                theMatHireLedgerMaster.TheSupplierRelationInfo = master.TheSupplierRelationInfo;
                theMatHireLedgerMaster.SupplierName = master.SupplierName;
                theMatHireLedgerMaster.ProjectId = master.ProjectId;
                theMatHireLedgerMaster.ProjectName = master.ProjectName;
                theMatHireLedgerMaster.WashType = 1;//退料
                theMatHireLedgerMaster.BillCode = master.Code;
                theMatHireLedgerMaster.BillId = master.Id;
                theMatHireLedgerMaster.OldContractNum = master.OldContractNum;
                theMatHireLedgerMaster.SystemDate = DateTime.Now;
                theMatHireLedgerMaster.RealOperationDate = master.CreateDate;
                theMatHireLedgerMaster.TheRank = detail.BorrowUnit;
                theMatHireLedgerMaster.TheRankName = detail.BorrowUnitName;
                theMatHireLedgerMaster.BillDetailId = detail.Id;
                theMatHireLedgerMaster.MaterialResource = detail.MaterialResource;
                theMatHireLedgerMaster.MaterialCode = detail.MaterialCode;
                theMatHireLedgerMaster.MaterialName = detail.MaterialName;
                theMatHireLedgerMaster.MaterialSpec = detail.MaterialSpec;
                theMatHireLedgerMaster.MatStandardUnit = detail.MatStandardUnit;
                theMatHireLedgerMaster.MatStandardUnitName = detail.MatStandardUnitName;
                theMatHireLedgerMaster.UsedPart = detail.UsedPart;
                theMatHireLedgerMaster.UsedPartName = detail.UsedPartName;
                theMatHireLedgerMaster.RentalPrice = detail.RentalPrice;
                //退料为负数：说明为收料
                decimal ExitQuantity = detail.ExitQuantity;
                if (ExitQuantity < 0)
                {
                    theMatHireLedgerMaster.LeftQuantity = Math.Abs(ExitQuantity);
                }
                theMatHireLedgerMaster.ReturnQuantity = detail.ExitQuantity;

                SaveOrUpdateByDao(theMatHireLedgerMaster);
            }

            #region 校验退料明细数量是否退料时序的数量(和)相等

            foreach (MatHireReturnDetail Detail in master.Details)
            {
                if (Detail.ExitQuantity > 0)
                {
                    decimal ReturnSeqQty = 0;
                    foreach (MatHireReturnDetailSeq seq in Detail.MaterialReturnDetailSeqs)
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
        public bool DeleteMaterialHireReturn(MatHireReturnMaster obj)
        {
            try
            {
                //删除台账信息
                foreach (MatHireReturnDetail detail in obj.Details)
                {
                    MatHireLedgerMaster master = this.GetMaterialHireLedgerByMatReturnCollDtlId(detail.Id);
                    if (master != null)
                        DeleteByDao(master);
                }
                //更新台账剩余数量
                foreach (MatHireReturnDetail theMatHireReturnDetail in obj.Details)
                {
                    foreach (MatHireReturnDetailSeq theMatHireReturnDetailSeq in theMatHireReturnDetail.MaterialReturnDetailSeqs)
                    {
                        MatHireLedgerMaster master = this.GetMaterialHireLedgerMasterById(theMatHireReturnDetailSeq.MatLedgerId);
                        if (master != null)
                        {
                            master.LeftQuantity = master.LeftQuantity + theMatHireReturnDetailSeq.ReturnQuantity;
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
        public DataSet GetMaterialHireReturnMasterQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t1.BillCode,t1.OldContractNum,t1.SupplierName,t1.PrintTimes,t1.Descript,t2.BorrowUnitName TheRankName,t1.SumExitQuantity,T1.PROJECTNAME,t1.State,t2.MaterialCode,t2.MaterialName,
                t2.MaterialSpec,t2.MatStandardUnitName,t2.RentalPrice,t2.RejectQuantity,t2.BalRule,t2.BroachQuantity,t2.subjectname,
                t1.CreateDate,t1.RealOperationDate,t1.CreatePersonName,t1.transportcharge,
                t2.ConsumeQuantity,t2.ProjectQuantity,t2.discardqty,t2.repairqty,t2.lossqty,t2.ExitQuantity,t2.UsedPartName,t2.beforestockqty,t1.mathiretype,t2.materialtype,t2.materiallength
                FROM THD_MatHireReturnMaster  t1 INNER JOIN THD_MatHireReturnDetail t2 ON t2.ParentId=t1.Id";
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
        public DataDomain VerifyReturnMatKC(MatHireReturnMaster Master, bool CD)
        {
            DataDomain ReturnDoamain = new DataDomain();

            decimal HistoryJC = 0;
            decimal CurrentJC = 0;
            //拼接查询条件
            foreach (MatHireReturnDetail MatHireReturnDetail in Master.Details)
            {
                if (MatHireReturnDetail.ExitQuantity > 0)
                {
                    string CurrJCcondition = "ProjectId='" + Master.ProjectId + "' and SupplierRelation='" + Master.TheSupplierRelationInfo.Id.ToString() + "' AND TheRank='" + MatHireReturnDetail.BorrowUnit.Id.ToString() + "' AND Material='" + MatHireReturnDetail.MaterialResource.Id.ToString() + "'";
                    CurrentJC = GetCurrentJC(CurrJCcondition);

                    string HisJCcondition = "ProjectId='" + Master.ProjectId + "' and SupplierRelation='" + Master.TheSupplierRelationInfo.Id.ToString() + "' AND TheRank='" + MatHireReturnDetail.BorrowUnit.Id.ToString() + "' AND Material='" + MatHireReturnDetail.MaterialResource.Id.ToString() + "' AND RealOperationDate<=to_date('" + Master.CreateDate.Date.ToShortDateString() + "','yyyy-mm-dd')";
                    HistoryJC = GetHistoryJC(HisJCcondition);
                    if (CD == true)
                    {
                        if (MatHireReturnDetail.RealExitQuantity - MatHireReturnDetail.RealTempData - HistoryJC > 0)
                        {
                            //插入业务日期的库存不足
                            ReturnDoamain.Name1 = 2;
                            ReturnDoamain.Name2 = "插入业务日期的[" + MatHireReturnDetail.MaterialName + "]库存不足";
                        }
                        if (MatHireReturnDetail.RealExitQuantity -  MatHireReturnDetail.RealTempData  - HistoryJC < 0)
                        {
                            //判断插入该笔退料后,后边的结存情况
                            string condition = "ProjectId='" + Master.ProjectId + "' and SupplierRelation='" + Master.TheSupplierRelationInfo.Id.ToString() + "' AND TheRank='" + MatHireReturnDetail.BorrowUnit.Id.ToString() + "' AND Material='" + MatHireReturnDetail.MaterialResource.Id.ToString() + "' AND RealOperationDate> to_date('" + Master.CreateDate.Date.ToShortDateString() + "','yyyy-mm-dd')";
                            DataSet ds = GetMaterialHireLedgerByCondition(condition);
                            DataTable table = ds.Tables[0];
                            decimal tempJC = 0;
                            tempJC = HistoryJC - MatHireReturnDetail.RealExitQuantity;
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
                                ReturnDoamain.Name2 = "插入该笔退料后，业务日期[" + BusinessDate + "]的[" + MatHireReturnDetail.MaterialName + "]库存为负";
                            }
                        }
                    }
                    else
                    {
                        //正常退料单：只判断当前退料数量是否大于当前库存
                        if (MatHireReturnDetail.RealExitQuantity -  MatHireReturnDetail.RealTempData  - CurrentJC > 0)
                        {
                            //当前库存不足
                            ReturnDoamain.Name1 = 1;
                            ReturnDoamain.Name2 = "[" + MatHireReturnDetail.MaterialName + "]当前库存不足";
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
                @"select sum(LeftQuantity) SumJC from THD_MaterialHireLedgerMaster where " + condition + "";
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
                @"select sum(collectionquantity)-sum(returnquantity) SumJC from THD_MaterialHireLedgerMaster where " + condition + "";
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
                @"select Id  from THD_MatHireReturnMaster where projectid='" + projectId + "' and CreateDate>= to_date('" + BusinessDate.AddDays(1).ToShortDateString() + "','yyyy-mm-dd')";
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
                @"select Id  from THD_MatHireCollectionMaster where projectid='" + projectId + "' and CreateDate>= to_date('" + BusinessDate.AddDays(1).ToShortDateString() + "','yyyy-mm-dd')";
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
        public MatHireLedgerMaster SaveMaterialHireLedgerMaster(MatHireLedgerMaster obj)
        {
            return SaveOrUpdateByDao(obj) as MatHireLedgerMaster;
        }

        /// <summary>
        /// 通过收退料单明细Id查询料具台账
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MatHireLedgerMaster GetMaterialHireLedgerByMatReturnCollDtlId(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("BillDetailId", id));
            IList list = GetMaterialHireLedgerMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireLedgerMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过收退料单Id查询料具台账
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetMaterialHireLedgerByMatReturnCollId(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("BillId", id));
            IList list = GetMaterialHireLedgerMaster(objectQuery);
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
        public IList GetMaterialHireLedgerMaster(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheRank", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheRank.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("SubjectGUID", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("UsedPart", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MatHireLedgerMaster), objectQuery);
        }

        /// <summary>
        /// 通过ID查询台账
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MatHireLedgerMaster GetMaterialHireLedgerMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialHireLedgerMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireLedgerMaster;
            }
            return null;
        }

        /// <summary>
        /// 根据条件查询台账信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet GetMaterialHireLedgerByCondition(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                 @"SELECT WashType,OldContractNum,BillCode,Material, MaterialCode, MaterialName,
                   MaterialSpec, CollectionQuantity, LeftQuantity, ReturnQuantity, SupplierName,
                   MatStandardUnitName,SystemDate, RealOperationDate, RentalPrice, TheRankName
                   FROM THD_MaterialHireLedgerMaster";
            sql += " where 1=1 and " + condition + " ORDER BY SystemDate";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        public IList GetMaterialHireLedgerMaster(string condition, DateTime BeginDate, string projectId)
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
                           FROM   THD_MaterialHireLedgerMaster t1 LEFT JOIN
                           THD_MatHireReturnDetail t2 ON t2.Id=t1.BillDetailId
                           LEFT JOIN THD_MatHireCollectionMaster t3 ON t1.BillId=t3.Id
                           LEFT JOIN  thd_MatHireReturnMaster t4 ON t1.BillId=t4.Id";
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
        public Hashtable GetPreviousJC(DateTime BeginDate, string sMaterialIds, string projectId, string supplyId)
        {
            string sWhere = string.Empty;
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "SELECT material, sum(leftquantity) leftquantity FROM THD_MaterialHireLedgerMaster where leftquantity<>0 {0}  GROUP BY material";
            if (!string.IsNullOrEmpty(supplyId))
            {
                sWhere += string.Format(" and supplierrelation='{0}' ",  supplyId);
            }
            if (!string.IsNullOrEmpty(projectId))
            {
                sWhere += string.Format(" and projectid='{0}' ",  projectId);
            }
            if (!string.IsNullOrEmpty(sMaterialIds))
            {
                sWhere += string.Format(" and Material In ({0}) ", sMaterialIds);
            }
            if (!(BeginDate==DateTime.MaxValue || BeginDate==DateTime.MinValue))
            {
                sWhere = string.Format(" and RealOperationDate<= to_date('{0}','yyyy-mm-dd ') ",BeginDate.ToShortDateString());
            }
            
//                 @"SELECT material, sum(leftquantity) leftquantity FROM THD_MaterialHireLedgerMaster 
//                   WHERE supplierrelation='" + supplyId + "'and projectid = '" + projectId + "' and RealOperationDate<= to_date('" + BeginDate.ToShortDateString() + "','yyyy-mm-dd ') AND Material In (" + sMaterialIds + ") GROUP BY material";
            command.CommandText = string.Format(sql,sWhere);
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable table = dataSet.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                ht.Add(ClientUtil.ToString(row["material"]), ClientUtil.ToDecimal(row["leftquantity"]));
            }
            return ht;
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
                 @"SELECT SUM(CollectionQuantity) SumCollQTy,SUM(ReturnQuantity) SumReturnQty FROM THD_MaterialHireLedgerMaster 
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
        public IList GetMatLeftQuantityByNew(MatHireReturnDetail detail, SupplierRelationInfo Supplier, SupplierRelationInfo rank, string projectId, EnumMatHireType enumMatHireType)
        {
            IList list_MatRenLedMaster = new ArrayList();
            //退料数量
            decimal exitQuantity =detail.RealExitQuantity;//ExitQuantity;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", Supplier));

            if (rank != null) { oq.AddCriterion(Expression.Eq("TheRank", rank)); }
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            oq.AddCriterion(Expression.Eq("MaterialResource", detail.MaterialResource));
            oq.AddCriterion(Expression.Eq("MatHireType", enumMatHireType));

            oq.AddCriterion(Expression.Sql(detail.ExitQuantity > 0 ? "LeftQuantity>0" : "LeftQuantity<0"));
            oq.AddOrder(Order.Asc("SystemDate"));
       
            IList list = this.GetMaterialHireLedgerMaster(oq);
            if (detail.ExitQuantity > 0)
            {
                foreach (MatHireLedgerMaster master in list)
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
                        else //if (exitQuantity - master.LeftQuantity <= 0)
                        {
                            master.TempQuantity = exitQuantity;
                            master.LeftQuantity = tempQuantity - exitQuantity;
                            exitQuantity = exitQuantity - tempQuantity;
                        }
                        list_MatRenLedMaster.Add(master);
                    }
                }
            }
            else  //用负退料单 冲库存
            {
               // exitQuantity = -exitQuantity;
                foreach (MatHireLedgerMaster master in list)
                {
                    decimal tempQuantity = master.LeftQuantity;
                    if (exitQuantity >= 0)
                    {
                        break;
                    }
                    else
                    {
                        if (exitQuantity-master.LeftQuantity   < 0)
                        {
                            master.TempQuantity = master.LeftQuantity;
                            exitQuantity = exitQuantity - tempQuantity;
                            master.LeftQuantity = 0;
                        }
                        else //if (exitQuantity + master.LeftQuantity >= 0)
                        {
                            master.TempQuantity = exitQuantity;
                            master.LeftQuantity = tempQuantity - exitQuantity;
                            exitQuantity = exitQuantity - tempQuantity;
                        }
                        list_MatRenLedMaster.Add(master);
                    }
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
        public IList GetMatLeftQuantityByModify(MatHireReturnDetail detail, SupplierRelationInfo Supplier, SupplierRelationInfo rank, string projectId)
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
            IList list = this.GetMaterialHireLedgerMaster(oq);
            //查询台账信息和当前退料时序的台账ID作对比：
            //如果有补上剩余数量再做先进先出计算
            foreach (MatHireLedgerMaster theMatHireLedgerMaster in list)
            {
                foreach (MatHireReturnDetailSeq seq in detail.MaterialReturnDetailSeqs)
                {
                    if (theMatHireLedgerMaster.Id == seq.MatLedgerId)
                    {
                        theMatHireLedgerMaster.LeftQuantity = theMatHireLedgerMaster.LeftQuantity + seq.ReturnQuantity;
                    }
                }
            }
            //在补上剩余数量的基础上先进先出
            foreach (MatHireLedgerMaster master in list)
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
                foreach (MatHireLedgerMaster master in list)
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
        public decimal GetMatStockQty(SupplierRelationInfo theSupplier, SupplierRelationInfo theRank, Material material, string projectId, EnumMatHireType enumMatHireType)
        {
            decimal StockQty = 0;
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("WashType", 0));//收料单
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));//料具出租方
            oq.AddCriterion(Expression.Eq("TheRank", theRank));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            oq.AddCriterion(Expression.Eq("MaterialResource", material));
            oq.AddCriterion(Expression.Eq("MatHireType", enumMatHireType));
            list = GetMaterialHireLedgerMaster(oq);

            foreach (MatHireLedgerMaster master in list)
            {
                StockQty = StockQty + master.LeftQuantity;
            }
            return StockQty;
        }

        #endregion
        #region 运输费
        public MatHireTranCostMaster SaveMatHireTranCostMaster(MatHireTranCostMaster master)
        {
            if (string.IsNullOrEmpty(master.Id))
            {
                master.Code = GetCode(typeof(MatHireTranCostMaster));
                master.RealOperationDate=DateTime.Now;
            }
            if (master.DocState == DocumentState.InExecute)
            {
                master.SubmitDate = DateTime.Now; 
            }
            dao.SaveOrUpdate(master);
            return master;
        }
        public bool DeleteMatHireTranCostMaster(MatHireTranCostMaster master)
        {
            return dao.Delete(master);
        }
        public MatHireTranCostMaster GetMatHireTranCostMasterById(string sID)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", sID));
            IList lst = GetMatHireTranCostMaster(oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as MatHireTranCostMaster;
        }
        public IList  GetMatHireTranCostMaster(ObjectQuery oQuery)
        {
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            oQuery.AddFetchMode("Details.Contract", FetchMode.Eager);
            oQuery.AddFetchMode("Details.TheSupplierRelationInfo", FetchMode.Eager);
            return dao.ObjectQuery(typeof(MatHireTranCostMaster), oQuery);
        }
        public DataSet QueryMaterialHireTranCost(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"select t1.id masterID,t1.code,t1.BillCode,t1.realoperationdate,t1.createdate,t1.createpersonname,t1.state,t1.contractcode,t1.suppliername,
t1.projectname,t2.transportmoney,t2.dispatchmoney ,t2.descript from thd_mathiretrancostmaster t1
join thd_mathiretrancostdetail t2 on t1.id=t2.parentid ";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        #endregion
        #region 料具结算方法
        /// <summary>
        /// 根据会计年，会计月取上期料具结算
        /// </summary>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <returns></returns>
        public MatHireBalanceMaster GetMatBalanceMaster(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("FiscalYear", fiscalYear));
            oq.AddCriterion(Expression.Eq("FiscalMonth", fiscalMonth));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));

            IList list = GetMatBalanceMaster(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireBalanceMaster;
            }
            return null;
        }

        public MatHireBalanceMaster GetPrrviousMatBalanceMaster(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo)
        {
            fiscalYear = TransUtil.GetLastYear(fiscalYear, fiscalMonth);
            fiscalMonth = TransUtil.GetLastMonth(fiscalYear, fiscalMonth);
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("FiscalYear", fiscalYear));
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("FiscalMonth", fiscalMonth));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));
            IList list = dao.ObjectQuery(typeof(MatHireBalanceMaster),oq); //GetMatBalanceMaster(oq);
            if (list != null && list.Count > 0)
            {

                MatHireBalanceMaster oMastr = list[0] as MatHireBalanceMaster;
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

            list = Dao.ObjectQuery(typeof(MatHireBalanceMaster), objectQuery);
            //foreach (MatHireBalanceMaster master in list)
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
            MatHireBalanceMaster matBalanceMaster = new MatHireBalanceMaster();
            MatHireBalanceDetail matBalanceDetail = null;
            MatHireBalanceOtherCostDtl matBalOtherCostDetail = null;

            //上期结存
            MatHireBalanceMaster ProphaseMatUnusedBal = new MatHireBalanceMaster();
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
            //oq.AddCriterion(Expression.Le("CreateDate", ClientUtil.ToDateTime(OperEndDate.Date.ToShortDateString())));
            oq.AddCriterion(Expression.Sql("CreateDate<=to_date('" + OperEndDate.Date.ToShortDateString() + "','yyyy-mm-dd')"));

            #region 1.本期退料(构造退料时序并计算其他费用)
            //本期退料
            IList list_matReturnMaster = GetMaterialHireReturnMaster(oq);
            //退料时序
            IList MatReturnSeq_list = new ArrayList();
            foreach (MatHireReturnMaster MatReturnMaster in list_matReturnMaster)
            {
                //计算运输费
                sumMoney += MatReturnMaster.TransportCharge;

                //循环明细
                foreach (MatHireReturnDetail MatReturnDetail in MatReturnMaster.Details)
                {
                    //退料负数产生收料添加到本期收料哈希表中(根据退料明细构建一个收料明细)
                    if (MatReturnDetail.ExitQuantity < 0)
                    {
                        MatHireCollectionDetail MatCollDetail = new MatHireCollectionDetail();
                        MatCollDetail.Id = MatReturnDetail.Id;
                        MatCollDetail.MatCollDate = MatReturnMaster.CreateDate;
                        MatCollDetail.Quantity = Math.Abs(MatReturnDetail.RealExitQuantity); //Math.Abs(MatReturnDetail.ExitQuantity);
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
                        MatCollDetail.MaterialLength = MatReturnDetail.MaterialLength;//长度
                        MatCollDetail.MaterialType = MatReturnDetail.MaterialType;//型号
                        ht_MatCollDetail.Add(MatCollDetail.Id, MatCollDetail);
                    }

                    //数量费用明细
                    foreach (MatHireReturnCostDtl MatReturnCostDtl in MatReturnDetail.MatReturnCostDtls)
                    {
                        matBalOtherCostDetail = new MatHireBalanceOtherCostDtl();
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
                    foreach (MatHireReturnDetailSeq MatReturnDeatilSeq in MatReturnDetail.MaterialReturnDetailSeqs)
                    {
                        MatReturnSeq_list.Add(MatReturnDeatilSeq);
                    }
                }
            }

            #endregion

            #region 2.本期收料 (构造收料哈希表并计算其他费用)
            //本期收料
            IList list_matCollMaster = GetMaterialHireCollectionMaster(oq);
            foreach (MatHireCollectionMaster MatCollMaster in list_matCollMaster)
            {
                //计算运输费
                sumMoney += MatCollMaster.TransportCharge;

                foreach (MatHireCollectionDetail MatCollDetail in MatCollMaster.Details)
                {
                    //数量费用明细
                    foreach (MatHireCollectionCostDtl MatCollCostDtl in MatCollDetail.MatCostDtls)
                    {
                        matBalOtherCostDetail = new MatHireBalanceOtherCostDtl();
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
                    foreach (MatHireReturnDetailSeq MatReturnDeatilSeq in MatCollDetail.MaterialReturnDetailSeqs)
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
                foreach (MatHireBalanceDetail MatHireBalanceDetail in ProphaseMatUnusedBal.Details)
                {
                    if (MatHireBalanceDetail.UnusedBalQuantity > 0)
                    {
                        ht_MatUnuesdBalDetail.Add(MatHireBalanceDetail.MatCollDtlId, MatHireBalanceDetail);
                    }
                }
            }
            #endregion

            #region 4.结算明细，租赁费用

            #region 循环退料时序在本期收料和上期结存里找到退料时序中对应的收料明细,并冲减本期收料和上期结存的剩余数量(结存数量)
            //循环退料时序，产生结算明细
            foreach (MatHireReturnDetailSeq MatReturnDeatilSeq in MatReturnSeq_list)
            {
                matBalanceDetail = new MatHireBalanceDetail();
                matBalanceDetail.ExitQuantity = MatReturnDeatilSeq.ReturnQuantity;


                if (ht_MatCollDetail.Contains(MatReturnDeatilSeq.MatCollDtlId))
                {
                    MatHireCollectionDetail MatCollDetail = (MatHireCollectionDetail)ht_MatCollDetail[MatReturnDeatilSeq.MatCollDtlId];
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
                    MatCollDetail.LeftQuantuity = MatCollDetail.LeftQuantuity - MatReturnDeatilSeq.ReturnQuantity/(MatReturnDeatilSeq.MaterialLength==0?1:MatReturnDeatilSeq.MaterialLength);//剩余数量

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
                    MatHireBalanceDetail MatBalDetail = (MatHireBalanceDetail)ht_MatUnuesdBalDetail[MatReturnDeatilSeq.MatCollDtlId];
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
                MatHireCollectionDetail MatCollDetail = ht_MatCollDetail[CollDtlId] as MatHireCollectionDetail;
                if (MatCollDetail.LeftQuantuity > 0)
                {
                    matBalanceDetail = new MatHireBalanceDetail();
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
                MatHireBalanceDetail MatBalDetail = ht_MatUnuesdBalDetail[CollDtlId] as MatHireBalanceDetail;
                if (MatBalDetail.UnusedBalQuantity > 0)
                {
                    matBalanceDetail = new MatHireBalanceDetail();
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
            MatHireOrderMaster MatRenMaster = new MatHireOrderMaster();
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));
            IList lst = GetMaterialHireOrder(oq) as IList;
            if (lst.Count > 0)
            {
                MatRenMaster = lst[0] as MatHireOrderMaster;
            }
            matBalanceMaster.OldContractNum = MatRenMaster.OriginalContractNo;

            matBalanceMaster = SaveByDao(matBalanceMaster) as MatHireBalanceMaster;
            #endregion

            #region 7.收退料单主表加上结算标记
            foreach (MatHireCollectionMaster master in list_matCollMaster)
            {
                if (master != null)
                {
                    master.BalYear = fiscalYear;
                    master.BalMonth = fiscalMonth;
                    master.BalState = 1;
                    UpdateByDao(master);
                }
            }
            foreach (MatHireReturnMaster master in list_matReturnMaster)
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
            MatHireBalanceMaster matBalanceMaster = GetMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);

            #region 生成本月的料具结算信息(商务)
            MaterialSettleMaster masterMat = new MaterialSettleMaster();
            masterMat.AuditMonth = matBalanceMaster.AuditMonth;
            masterMat.AuditYear = matBalanceMaster.AuditYear;
            decimal sumMoney = 0;
            Hashtable ht = new Hashtable();
            foreach (MatHireBalanceDetail detail in matBalanceMaster.Details)
            {
                if (detail.UsedPart != null && detail.SubjectGUID != null && detail.MaterialResource != null)
                {
                    string linkStr = detail.UsedPart.Id + "-" + detail.MaterialResource.Id + "-" + detail.SubjectGUID.Id;
                    if (ht.Contains(linkStr))
                    {
                        MatHireBalanceDetail temp = (MatHireBalanceDetail)ht[linkStr];
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
            foreach (MatHireBalanceOtherCostDtl otherDetail in matBalanceMaster.MatBalOtherCostDetails)
            {

                if (otherDetail.UsedPart != null && otherDetail.SubjectGUID != null && otherDetail.MaterialResource != null)
                {
                    string linkStr = otherDetail.UsedPart.Id + "-" + otherDetail.MaterialResource.Id + "-" + otherDetail.SubjectGUID.Id;
                    if (ht.Contains(linkStr))
                    {
                        MatHireBalanceDetail temp = (MatHireBalanceDetail)ht[linkStr];
                        temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) + otherDetail.CostMoney) + "";
                        ht.Remove(linkStr);
                        ht.Add(linkStr, temp);
                    }
                    else
                    {
                        MatHireBalanceDetail temp = new MatHireBalanceDetail();
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
                foreach (MatHireBalanceDetail dtl in ht.Values)
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
        public IList GetStockBlock(SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo, DateTime OperEndDate)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo.Id", theSupplier.Id));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.Le("CreateDate",OperEndDate));
            oq.AddFetchMode("Details",FetchMode.Eager);
            return dao.ObjectQuery(typeof(MatHireStockBlockMaster), oq);
        }
        /// <summary>
        /// 根据当前开始结束日期取得
        /// 该时间内的收退料信息以及上期结存信息
        /// </summary>
        [TransManager]
        public void MaterialReckoning(DateTime OperEndDate, int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier,
            CurrentProjectInfo ProjectInfo,decimal dChangeMoney)
        {
            MatHireBalanceMaster matBalanceMaster = new MatHireBalanceMaster();
            MatHireBalanceDetail matBalanceDetail = null;
            MatHireBalanceOtherCostDtl matBalOtherCostDetail = null;
            IList lstMatHireStockBlockMaster = null;
            IList lstMatHireTranCostMaster = null;
            Hashtable htMaterialPrice = new Hashtable();
            MatHireOrderMaster oOrderMaster = GetMaterialHireOrder(theSupplier, ProjectInfo);
            if (oOrderMaster == null)
            {
                throw new Exception("料具月结失败:未找到对应合同");
            }
            foreach (MatHireOrderDetail oOrderDetail in oOrderMaster.Details)
            {
                htMaterialPrice.Add(oOrderDetail.MaterialResource.Id, oOrderDetail.Price);
            }
            //GetMaterialHireOrder
            //上期结存
            MatHireBalanceMaster ProphaseMatUnusedBal = new MatHireBalanceMaster();
            int previousYear = TransUtil.GetLastYear(fiscalYear, fiscalMonth);
            int previousMonth = TransUtil.GetLastMonth(fiscalYear, fiscalMonth);
            ProphaseMatUnusedBal = GetMatBalanceMaster(previousYear, previousMonth, theSupplier, ProjectInfo);

            decimal sumMoney = 0;//总费用金额
            decimal sumQuantity = 0;//收料总数量
            decimal dPrice = 0;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo.Id", theSupplier.Id));
            oq.AddCriterion(Expression.Eq("BalState", 0));
            oq.AddCriterion(Expression.Le("CreateDate", ClientUtil.ToDateTime(OperEndDate.Date.ToShortDateString())));
            //oq.AddCriterion(Expression.Sql("CreateDate<=to_date('" + OperEndDate.Date.ToShortDateString() + "','yyyy-mm-dd')"));
            //料具对应料具租赁物资
            Material oTrafficMaterial = new Material();
            oTrafficMaterial.Version = 0;
            oTrafficMaterial.Id = TransUtil.ConStationTrafficMaterialId;
            oTrafficMaterial.Code = TransUtil.ConStationTrafficMaterialCode;
            oTrafficMaterial.Name = TransUtil.ConStationTrafficMaterialName;
            #region
            ////运输费用对应科目
            //CostAccountSubject transSubject = new CostAccountSubject();
            //transSubject.Id = TransUtil.ConTrafficSubjectId;
            //transSubject.Name = TransUtil.ConTrafficSubjectName;
            //transSubject.SysCode = TransUtil.ConTrafficSubjectSyscode;
            ////调整费用对应科目
            //CostAccountSubject otherSubject = new CostAccountSubject();
            //otherSubject.Id = TransUtil.ConStockOutSubjectId;
            //otherSubject.Name = TransUtil.ConStockOutSubjectName;
            //otherSubject.SysCode = TransUtil.ConStockOutSubjectSyscode;
            #endregion
            #region 1.本期退料 (租赁费用/其他费用)
            //本期退料
            IList list_matReturnMaster = GetMaterialHireReturnMaster(oq);
            MatHireReturnDetail oMatHireReturnDetail = null;
            MatHireBalanceOtherCostDtl oMatBalOtherCostDetail = null;

            decimal dMax = 0;
            foreach (MatHireReturnMaster MatReturnMaster in list_matReturnMaster)
            {
                //计算运输费  目前运输费根据运费单来的
                #region 退料单上运费
                //oMatBalOtherCostDetail = null;
                //if (MatReturnMaster.TransportCharge != 0)
                //{
                //    matBalOtherCostDetail = new MatHireBalanceOtherCostDtl();
                //    oMatBalOtherCostDetail = matBalOtherCostDetail;
                //    matBalOtherCostDetail.BusinessCode = MatReturnMaster.Code;
                //    matBalOtherCostDetail.BusinessMasterId = MatReturnMaster.Id;
                //    matBalOtherCostDetail.BusinessType = "退料";
                //    matBalOtherCostDetail.CostMoney = MatReturnMaster.TransportCharge;

                //    foreach (MatHireReturnDetail MatReturnDetail in MatReturnMaster.Details)
                //    {
                //        matBalOtherCostDetail.UsedPart = MatReturnDetail.UsedPart;
                //        matBalOtherCostDetail.UsedPartSysCode = MatReturnDetail.UsedPartSysCode;
                //        matBalOtherCostDetail.UsedPartName = MatReturnDetail.UsedPartName;
                //        break;
                //    }
                //    matBalOtherCostDetail.MaterialResource = material;
                //    matBalOtherCostDetail.MaterialCode = material.Code;
                //    matBalOtherCostDetail.MaterialName = material.Name;
                //    matBalOtherCostDetail.SubjectGUID = transSubject;
                //    matBalOtherCostDetail.SubjectName = transSubject.Name;
                //    matBalOtherCostDetail.SubjectSysCode = transSubject.SysCode;
                //    //计算总费用
                //    matBalOtherCostDetail.CostType = "运输费用";
                //    matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                //    sumMoney += MatReturnMaster.TransportCharge;
                //}
                #endregion
                dMax = decimal.MinValue;
                oMatHireReturnDetail = null;

                //循环明细
                foreach (MatHireReturnDetail MatReturnDetail in MatReturnMaster.Details)
                {
                    if (MatReturnDetail.RealExitQuantity == 0) continue;
                    dPrice = GetPrice(MatReturnDetail.MaterialResource,htMaterialPrice,"本期退料单");//获取合同中价格
                    //if (MatReturnDetail.Quantity * MatReturnDetail.RentalPrice >= dMax)
                    //if (MatReturnDetail.RealExitQuantity * MatReturnDetail.RentalPrice >= dMax)
                    //if (MatReturnDetail.RealExitQuantity * dPrice >= dMax)
                    //{
                    //    oMatHireReturnDetail = MatReturnDetail;
                    //}
                    //租赁费用
                    matBalanceDetail = new MatHireBalanceDetail();
                    matBalanceDetail.BillType = EnumBillType.退料;
                    matBalanceDetail.MatReturnCode = MatReturnMaster.Code;
                    matBalanceDetail.BalRule = MatReturnMaster.BalRule;
                    matBalanceDetail.MaterialResource = MatReturnDetail.MaterialResource;
                    matBalanceDetail.MaterialCode = MatReturnDetail.MaterialCode;
                    matBalanceDetail.MaterialName = MatReturnDetail.MaterialName;
                    matBalanceDetail.MaterialSpec = MatReturnDetail.MaterialSpec;
                    matBalanceDetail.MatStandardUnit = MatReturnDetail.MatStandardUnit;
                    matBalanceDetail.MatStandardUnitName = MatReturnDetail.MatStandardUnitName;
                    matBalanceDetail.RentalPrice = dPrice;// MatReturnDetail.RentalPrice;
                    matBalanceDetail.StartDate = MatReturnMaster.CreateDate;
                    matBalanceDetail.EndDate = OperEndDate;
                    matBalanceDetail.ExitQuantity = -MatReturnDetail.RealExitQuantity; //-MatReturnDetail.ExitQuantity;//退场数量
                    //退料单号和退料明细数量
                    matBalanceDetail.MatReturnCode = MatReturnMaster.Code;
                    matBalanceDetail.MatReturnDtlQty = MatReturnDetail.RealExitQuantity;//MatReturnDetail.ExitQuantity;
                    matBalanceDetail.MatCollDtlId = MatReturnDetail.Id;
                    //部位和科目
                    if( MatReturnDetail.SubjectGUID!=null){
                    matBalanceDetail.SubjectGUID = MatReturnDetail.SubjectGUID;
                    matBalanceDetail.SubjectName = MatReturnDetail.SubjectName;
                    matBalanceDetail.SubjectSysCode = MatReturnDetail.SubjectSysCode;
                    }
                    if(MatReturnDetail.UsedPart!=null){
                    matBalanceDetail.UsedPart = MatReturnDetail.UsedPart;
                    matBalanceDetail.UsedPartName = MatReturnDetail.UsedPartName;
                    matBalanceDetail.UsedPartSysCode = MatReturnDetail.UsedPartSysCode;
                    }

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
                    matBalanceDetail.Money = matBalanceDetail.ExitQuantity * matBalanceDetail.Days * matBalanceDetail.RentalPrice;//MatReturnDetail.RentalPrice;
                    sumMoney += matBalanceDetail.Money;
                    sumQuantity += matBalanceDetail.ExitQuantity;
                    //结存数量
                    matBalanceDetail.UnusedBalQuantity = matBalanceDetail.ExitQuantity;
                    matBalanceMaster.AddDetail(matBalanceDetail);

                    //数量费用明细
                    foreach (MatHireReturnCostDtl MatReturnCostDtl in MatReturnDetail.MatReturnCostDtls)
                    {
                        if (MatReturnCostDtl.Money == 0) continue;
                        matBalOtherCostDetail = new MatHireBalanceOtherCostDtl();
                        matBalOtherCostDetail.BusinessCode = MatReturnMaster.Code;
                        matBalOtherCostDetail.BusinessDetailId = MatReturnDetail.Id;
                        matBalOtherCostDetail.BusinessMasterId = MatReturnMaster.Id;
                        matBalOtherCostDetail.BusinessType = "退料";
                        matBalOtherCostDetail.CostMoney = MatReturnCostDtl.Money;
                        matBalOtherCostDetail.MaterialResource = MatReturnDetail.MaterialResource;
                        matBalOtherCostDetail.MaterialCode = MatReturnDetail.MaterialCode;
                        matBalOtherCostDetail.MaterialName = MatReturnDetail.MaterialName;
                        matBalOtherCostDetail.MaterialSpec = MatReturnDetail.MaterialSpec;
                        if (MatReturnDetail.SubjectGUID != null)
                        {
                            //部位和科目
                            matBalOtherCostDetail.SubjectGUID = MatReturnDetail.SubjectGUID;
                            matBalOtherCostDetail.SubjectName = MatReturnDetail.SubjectName;
                            matBalOtherCostDetail.SubjectSysCode = MatReturnDetail.SubjectSysCode;
                        }
                        if (MatReturnDetail.UsedPart != null)
                        {
                            matBalOtherCostDetail.UsedPart = MatReturnDetail.UsedPart;
                            matBalOtherCostDetail.UsedPartName = MatReturnDetail.UsedPartName;
                            matBalOtherCostDetail.UsedPartSysCode = MatReturnDetail.UsedPartSysCode;
                        }

                        //计算总费用
                        sumMoney = sumMoney + MatReturnCostDtl.Money;
                        matBalOtherCostDetail.CostType = MatReturnCostDtl.CostType;
                        #region 费用价格 理论值 数量 金额 公式
                        matBalOtherCostDetail.Price = MatReturnCostDtl.Price;
                        matBalOtherCostDetail.ConstValue = MatReturnCostDtl.ConstValue;
                        matBalOtherCostDetail.Quantity = MatReturnCostDtl.Quantity;
                        matBalOtherCostDetail.CostMoney = MatReturnCostDtl.Money;
                        matBalOtherCostDetail.Expression = MatReturnCostDtl.Expression;
                        #endregion
                        matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                    }
                }
                //if (MatReturnMaster.TransportCharge != 0 && oMatHireReturnDetail != null && oMatBalOtherCostDetail != null)
                //{
                //    //  matBalOtherCostDetail = oMatHireReturnDetail;
                //    oMatBalOtherCostDetail.UsedPart = oMatHireReturnDetail.UsedPart;
                //    oMatBalOtherCostDetail.UsedPartName = oMatHireReturnDetail.UsedPartName;
                //    oMatBalOtherCostDetail.UsedPartSysCode = oMatHireReturnDetail.UsedPartSysCode;
                //}
            }

            #endregion

            #region 2.本期收料 (租赁费用/其他费用)
            //本期收料
            MatHireCollectionDetail oMatHireCollectionDetail = null;

            IList list_matCollMaster = GetMaterialHireCollectionMaster(oq);
            foreach (MatHireCollectionMaster MatCollMaster in list_matCollMaster)
            {
                oMatBalOtherCostDetail = null;
                //2.1 计算运输费
                #region 运费计算
                //if (MatCollMaster.TransportCharge != 0)
                //{
                //    matBalOtherCostDetail = new MatHireBalanceOtherCostDtl();
                //    oMatBalOtherCostDetail = matBalOtherCostDetail;
                //    matBalOtherCostDetail.BusinessCode = MatCollMaster.Code;
                //    matBalOtherCostDetail.BusinessMasterId = MatCollMaster.Id;
                //    matBalOtherCostDetail.BusinessType = "收料";
                //    matBalOtherCostDetail.CostMoney = MatCollMaster.TransportCharge;

                //    foreach (MatHireCollectionDetail MatCollDetail in MatCollMaster.Details)
                //    {
                //        matBalOtherCostDetail.UsedPart = MatCollDetail.UsedPart;
                //        matBalOtherCostDetail.UsedPartSysCode = MatCollDetail.UsedPartSysCode;
                //        matBalOtherCostDetail.UsedPartName = MatCollDetail.UsedPartName;
                //        break;
                //    }
                //    matBalOtherCostDetail.MaterialResource = material;
                //    matBalOtherCostDetail.MaterialCode = material.Code;
                //    matBalOtherCostDetail.MaterialName = material.Name;
                //    matBalOtherCostDetail.SubjectGUID = transSubject;
                //    matBalOtherCostDetail.SubjectName = transSubject.Name;
                //    matBalOtherCostDetail.SubjectSysCode = transSubject.SysCode;
                //    //计算总费用
                //    matBalOtherCostDetail.CostType = "运输费用";
                //    matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                //    sumMoney += MatCollMaster.TransportCharge;
                //}
                #endregion
                oMatHireCollectionDetail = null;
                dMax = decimal.MinValue;
                foreach (MatHireCollectionDetail MatCollDetail in MatCollMaster.Details)
                {
                    if (MatCollDetail.RealQuantity == 0) continue;
                    dPrice = GetPrice(MatCollDetail.MaterialResource, htMaterialPrice,"本期收料单");//获取合同中价格
                    //2.2 租赁费用   //获取运费所属的明细  金额数最大的那笔
                   // if (MatCollDetail.Quantity * MatCollDetail.RentalPrice >= dMax)
                    //if (MatCollDetail.RealQuantity * MatCollDetail.RentalPrice >= dMax)
                    //if (MatCollDetail.RealQuantity * dPrice >= dMax)
                    //{
                    //    oMatHireCollectionDetail = MatCollDetail;
                    //}
                    matBalanceDetail = new MatHireBalanceDetail();
                    matBalanceDetail.BillType = EnumBillType.收料;
                    matBalanceDetail.BalRule = MatCollMaster.BalRule;
                    matBalanceDetail.MaterialResource = MatCollDetail.MaterialResource;
                    matBalanceDetail.MaterialCode = MatCollDetail.MaterialCode;
                    matBalanceDetail.MaterialName = MatCollDetail.MaterialName;
                    matBalanceDetail.MaterialSpec = MatCollDetail.MaterialSpec;
                    matBalanceDetail.MatStandardUnit = MatCollDetail.MatStandardUnit;
                    matBalanceDetail.MatStandardUnitName = MatCollDetail.MatStandardUnitName;
                    matBalanceDetail.RentalPrice = dPrice;// MatCollDetail.RentalPrice;
                    matBalanceDetail.StartDate = MatCollMaster.CreateDate;
                    matBalanceDetail.EndDate = OperEndDate;
                    matBalanceDetail.MatCollDtlId = MatCollDetail.Id;
                    matBalanceDetail.ApproachQuantity = MatCollDetail.RealQuantity;//MatCollDetail.Quantity;//进场数量
                    //部位和科目
                    matBalanceDetail.SubjectGUID = MatCollDetail.SubjectGUID;
                    matBalanceDetail.SubjectName = MatCollDetail.SubjectName;
                    matBalanceDetail.SubjectSysCode = MatCollDetail.SubjectSysCode;
                    matBalanceDetail.UsedPart = MatCollDetail.UsedPart;
                    matBalanceDetail.UsedPartName = MatCollDetail.UsedPartName;
                    matBalanceDetail.UsedPartSysCode = MatCollDetail.UsedPartSysCode;
                    //收料单号和收料明细数量
                    matBalanceDetail.MatCollCode = MatCollMaster.Code;
                    matBalanceDetail.MatCollDtlQty = MatCollDetail.RealQuantity;//MatCollDetail.Quantity;

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
                    matBalanceDetail.Money = matBalanceDetail.ApproachQuantity * matBalanceDetail.Days * matBalanceDetail.RentalPrice;//MatCollDetail.RentalPrice;
                    sumMoney += matBalanceDetail.Money;
                    sumQuantity += matBalanceDetail.ApproachQuantity;
                    //结存数量
                    matBalanceDetail.UnusedBalQuantity = matBalanceDetail.ApproachQuantity;
                    matBalanceMaster.AddDetail(matBalanceDetail);


                    //2.3 数量费用明细
                    foreach (MatHireCollectionCostDtl MatCollCostDtl in MatCollDetail.MatCostDtls)
                    {
                        if (MatCollCostDtl.Money == 0) continue;
                        matBalOtherCostDetail = new MatHireBalanceOtherCostDtl();
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
                        #region 费用价格 理论值 数量 金额 公式
                        matBalOtherCostDetail.Price = MatCollCostDtl.Price;
                        matBalOtherCostDetail.ConstValue = MatCollCostDtl.ConstValue;
                        matBalOtherCostDetail.Quantity = MatCollCostDtl.Quantity;
                        matBalOtherCostDetail.CostMoney = MatCollCostDtl.Money;
                        matBalOtherCostDetail.Expression = MatCollCostDtl.Expression;
                        #endregion
                        //计算总费用
                        sumMoney = sumMoney + MatCollCostDtl.Money;
                        matBalOtherCostDetail.CostType = MatCollCostDtl.CostType;
                        matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                       
                    }
                    MatCollDetail.MatCollDate = MatCollMaster.CreateDate;
                    MatCollDetail.LeftQuantuity = MatCollDetail.RealQuantity; //MatCollDetail.Quantity;
                    MatCollDetail.BalRule = MatCollMaster.BalRule;

                }
                //设置运费的部位
                //if (MatCollMaster.TransportCharge != 0 && oMatHireCollectionDetail != null && oMatBalOtherCostDetail != null)
                //{
                //    oMatBalOtherCostDetail.UsedPart = oMatHireCollectionDetail.UsedPart;
                //    oMatBalOtherCostDetail.UsedPartName = oMatHireCollectionDetail.UsedPartName;
                //    oMatBalOtherCostDetail.UsedPartSysCode = oMatHireCollectionDetail.UsedPartSysCode;
                //}
            }
            #endregion

            #region 3.上期结存(租赁费用)
            IList list_preMatBalDetail = new ArrayList();
            if (ProphaseMatUnusedBal != null)
            {
                //根据结算规则计算天数
                TimeSpan dt = TransUtil.ToShortDateTime(OperEndDate) - TransUtil.ToShortDateTime(ProphaseMatUnusedBal.EndDate.AddDays(1));
                int balDays = dt.Days + 1;
                foreach (MatHireBalanceDetail materialBalanceDetail in ProphaseMatUnusedBal.Details)
                {
                    if (materialBalanceDetail.UnusedBalQuantity == 0) continue;
                    dPrice = GetPrice(materialBalanceDetail.MaterialResource, htMaterialPrice, "上期结存");//获取合同中价格
                    //matBalanceDetail = new MatHireBalanceDetail();
                    //matBalanceDetail.BillType = EnumBillType.结存;
                    bool ifExist = false;
                    foreach (MatHireBalanceDetail existBalDetail in list_preMatBalDetail)
                    {
                        if (materialBalanceDetail.MaterialResource == existBalDetail.MaterialResource && materialBalanceDetail.UsedPart == existBalDetail.UsedPart)
                        {
                            existBalDetail.UnusedBalQuantity += materialBalanceDetail.UnusedBalQuantity;
                            //计算本期收料租赁费用
                            decimal currMoney = materialBalanceDetail.UnusedBalQuantity * balDays *dPrice;// materialBalanceDetail.RentalPrice;
                            existBalDetail.Money += currMoney;
                            sumMoney += currMoney;
                            ifExist = true;
                            break;
                        }
                    }
                    if (ifExist == false)
                    {
                        matBalanceDetail = new MatHireBalanceDetail();
                        matBalanceDetail.BillType = EnumBillType.结存;
                        matBalanceDetail.BalRule = materialBalanceDetail.BalRule;
                        matBalanceDetail.MaterialResource = materialBalanceDetail.MaterialResource;
                        matBalanceDetail.MaterialCode = materialBalanceDetail.MaterialCode;
                        matBalanceDetail.MaterialName = materialBalanceDetail.MaterialName;
                        matBalanceDetail.MaterialSpec = materialBalanceDetail.MaterialSpec;
                        matBalanceDetail.MatStandardUnit = materialBalanceDetail.MatStandardUnit;
                        matBalanceDetail.MatStandardUnitName = materialBalanceDetail.MatStandardUnitName;
                        matBalanceDetail.RentalPrice = dPrice;//materialBalanceDetail.RentalPrice;
                        //部位和科目
                        if (materialBalanceDetail.SubjectGUID!=null)
                        {
                            matBalanceDetail.SubjectGUID = materialBalanceDetail.SubjectGUID;
                            matBalanceDetail.SubjectName = materialBalanceDetail.SubjectName;
                            matBalanceDetail.SubjectSysCode = materialBalanceDetail.SubjectSysCode;
                        }
                        if (materialBalanceDetail.UsedPart != null)
                        {
                            matBalanceDetail.UsedPart = materialBalanceDetail.UsedPart;
                            matBalanceDetail.UsedPartName = materialBalanceDetail.UsedPartName;
                            matBalanceDetail.UsedPartSysCode = materialBalanceDetail.UsedPartSysCode;
                        }
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

            foreach (MatHireBalanceDetail preBalDetail in list_preMatBalDetail)
            {
                if (preBalDetail.UnusedBalQuantity != 0)
                {
                    matBalanceMaster.AddDetail(preBalDetail);
                }
            }
            #endregion
            #region 4.本期封存
             lstMatHireStockBlockMaster = GetStockBlock(theSupplier, ProjectInfo, OperEndDate);
            if (lstMatHireStockBlockMaster != null)
            {
                foreach (MatHireStockBlockMaster oMatHireStockBlockMaster in lstMatHireStockBlockMaster)
                {
                    foreach (MatHireStockBlockDetail oMatHireStockBlockDetail in oMatHireStockBlockMaster.Details)
                    {
                        if (oMatHireStockBlockDetail.Money == 0) continue;
                        matBalOtherCostDetail = new MatHireBalanceOtherCostDtl();
                        matBalOtherCostDetail.BusinessCode = oMatHireStockBlockMaster.Code;
                        matBalOtherCostDetail.BusinessMasterId = oMatHireStockBlockMaster.Id;
                        matBalOtherCostDetail.BusinessType = "料具封存";
                        matBalOtherCostDetail.Quantity = oMatHireStockBlockDetail.Quantity;
                        matBalOtherCostDetail.Price = oMatHireStockBlockDetail.Price;
                        matBalOtherCostDetail.CostMoney = -oMatHireStockBlockDetail.Money;
                        if (oMatHireStockBlockDetail.UsedPart != null)
                        {
                            matBalOtherCostDetail.UsedPart = oMatHireStockBlockDetail.UsedPart;
                            matBalOtherCostDetail.UsedPartSysCode = oMatHireStockBlockDetail.UsedPartSysCode;
                            matBalOtherCostDetail.UsedPartName = oMatHireStockBlockDetail.UsedPartName;
                        }
                            matBalOtherCostDetail.MaterialResource = oMatHireStockBlockDetail.MaterialResource;
                            matBalOtherCostDetail.MaterialCode = oMatHireStockBlockDetail.MaterialCode;
                            matBalOtherCostDetail.MaterialName = oMatHireStockBlockDetail.MaterialName;
                        if (oMatHireStockBlockDetail.SubjectGUID != null)
                        {
                            matBalOtherCostDetail.SubjectGUID = oMatHireStockBlockDetail.SubjectGUID;
                            matBalOtherCostDetail.SubjectName = oMatHireStockBlockDetail.SubjectName;
                            matBalOtherCostDetail.SubjectSysCode = oMatHireStockBlockDetail.SubjectSysCode;
                        }
                        //计算总费用
                        matBalOtherCostDetail.CostType = "料具封存";
                        matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                        sumMoney += matBalOtherCostDetail.CostMoney;
                    }
                }
            }
            #endregion
            #region 运输单
            lstMatHireTranCostMaster = GetMatHireTranCostMaster(oq);
            foreach (MatHireTranCostMaster oTranCostMaster in lstMatHireTranCostMaster)
            {
                if (oTranCostMaster.SumMoney == 0) continue;
                    matBalOtherCostDetail = new MatHireBalanceOtherCostDtl();
                    matBalOtherCostDetail.MaterialResource = oTrafficMaterial;
                    matBalOtherCostDetail.MaterialCode = oTrafficMaterial.Code;
                    matBalOtherCostDetail.MaterialName = oTrafficMaterial.Name;
                    matBalOtherCostDetail.BusinessCode = oTranCostMaster.Code;
                    matBalOtherCostDetail.BusinessMasterId = oTranCostMaster.Id;
                    matBalOtherCostDetail.BusinessType = "运费单";
                    matBalOtherCostDetail.CostMoney = oTranCostMaster.SumMoney;
                    matBalOtherCostDetail.CostType = "运输费用";
                    matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                    sumMoney += matBalOtherCostDetail.CostMoney;
               
            }
            #endregion
            #region 调整费
            if (dChangeMoney != 0)
            {
                matBalOtherCostDetail = new MatHireBalanceOtherCostDtl();
                matBalOtherCostDetail.MaterialResource = oTrafficMaterial;
                matBalOtherCostDetail.MaterialCode = oTrafficMaterial.Code;
                matBalOtherCostDetail.MaterialName = oTrafficMaterial.Name;
            
                matBalOtherCostDetail.BusinessCode = string.Empty;// oTranCostMaster.Code;
                matBalOtherCostDetail.BusinessMasterId = string.Empty;//oTranCostMaster.Id;
                matBalOtherCostDetail.BusinessType = "调整费";
                matBalOtherCostDetail.CostMoney = dChangeMoney;
                matBalOtherCostDetail.CostType = "调整费";
                matBalanceMaster.AddMatBalOtherCostDetails(matBalOtherCostDetail);
                sumMoney += matBalOtherCostDetail.CostMoney;
                matBalanceMaster.OtherMoney = dChangeMoney;
            }

            #endregion
            #region 4.组织主表信息保存
            Login login = VirtualMachine.Component.Util.CallContextUtil.LogicalGetData<Login>("LoginInformation");
            matBalanceMaster.CreatePerson = login.ThePerson;
            matBalanceMaster.OperOrgInfo = login.TheOperationOrgInfo;
            matBalanceMaster.OpgSysCode = login.TheOperationOrgInfo.SysCode;
            matBalanceMaster.OperOrgInfoName = login.TheOperationOrgInfo.Name;
            matBalanceMaster.CreateDate = OperEndDate;
            matBalanceMaster.OtherMoney = 0;
            matBalanceMaster.CreatePersonName = login.ThePerson.Name;
            matBalanceMaster.FiscalYear = Convert.ToInt32(fiscalYear);
            matBalanceMaster.FiscalMonth = Convert.ToInt32(fiscalMonth);
            matBalanceMaster.CreateYear = matBalanceMaster.FiscalYear;
            matBalanceMaster.CreateMonth = matBalanceMaster.FiscalMonth;
            matBalanceMaster.TheSupplierRelationInfo = theSupplier;
            matBalanceMaster.SupplierName = theSupplier.SupplierInfo.Name;
            matBalanceMaster.ProjectId = ProjectInfo.Id;
            matBalanceMaster.ProjectName = ProjectInfo.Name;
            //if (task != null)
            //{
            //    matBalanceMaster.UsedPart = task;
            //    matBalanceMaster.UsedPartName = task.Name;
            //    matBalanceMaster.UsedPartSysCode = task.SysCode;
            //}
            if (ProphaseMatUnusedBal != null)
                matBalanceMaster.StartDate = ProphaseMatUnusedBal.EndDate.AddDays(1);
            else
                matBalanceMaster.StartDate = ClientUtil.ToDateTime("1900-1-1");
            matBalanceMaster.EndDate = Convert.ToDateTime(OperEndDate);
            matBalanceMaster.SumMatMoney = sumMoney;// +otherMoney;
            //matBalanceMaster.SumMatQuantity = sumQuantity;
            MatHireOrderMaster MatRenMaster = new MatHireOrderMaster();
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));
            IList lst = GetMaterialHireOrder(oq) as IList;
            if (lst.Count > 0)
            {
                MatRenMaster = lst[0] as MatHireOrderMaster;
            }
            matBalanceMaster.OldContractNum = MatRenMaster.OriginalContractNo;

            matBalanceMaster = SaveByDao(matBalanceMaster) as MatHireBalanceMaster;
            #endregion

            #region 5.收退料单主表加上结算标记
            foreach (MatHireCollectionMaster master in list_matCollMaster)
            {
                if (master != null)
                {
                    master.BalYear = fiscalYear;
                    master.BalMonth = fiscalMonth;
                    master.BalState = 1;
                    UpdateByDao(master);
                }
            }
            foreach (MatHireReturnMaster master in list_matReturnMaster)
            {
                if (master != null)
                {
                    master.BalYear = fiscalYear;
                    master.BalMonth = fiscalMonth;
                    master.BalState = 1;
                    UpdateByDao(master);
                }
            }
            if (lstMatHireStockBlockMaster != null)
            {
                foreach (MatHireStockBlockMaster master in lstMatHireStockBlockMaster)
                {
                    master.BalYear = fiscalYear;
                    master.BalMonth = fiscalMonth;
                    master.BalState = 1;
                    UpdateByDao(master);
                }
            }
            if (lstMatHireTranCostMaster != null)
            {
                foreach (MatHireTranCostMaster master in lstMatHireTranCostMaster)
                {
                    master.BalYear = fiscalYear;
                    master.BalMonth = fiscalMonth;
                    master.BalState = 1;
                    UpdateByDao(master);
                }
            }
            #endregion
 
            #region 生成本月的料具结算信息(商务)
            //MaterialSettleMaster masterMat = new MaterialSettleMaster();
            //masterMat.AuditMonth = matBalanceMaster.AuditMonth;
            //masterMat.AuditYear = matBalanceMaster.AuditYear;
            //Hashtable ht = new Hashtable();
            //foreach (MatHireBalanceDetail detail in matBalanceMaster.Details)
            //{
            //    if (detail.UsedPart != null && detail.SubjectGUID != null && detail.MaterialResource != null)
            //    {
            //        string linkStr = detail.UsedPart.Id + "-" + detail.MaterialResource.Id + "-" + detail.SubjectGUID.Id;
            //        if (ht.Contains(linkStr))
            //        {
            //            MatHireBalanceDetail temp = (MatHireBalanceDetail)ht[linkStr];
            //            temp.TempData = (TransUtil.ToDecimal(temp.TempData) + detail.ExitQuantity + detail.ApproachQuantity) + "";
            //            temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) + detail.Money) + "";
            //            ht.Remove(linkStr);
            //            ht.Add(linkStr, temp);
            //        }
            //        else
            //        {
            //            detail.TempData = (detail.ExitQuantity + detail.ApproachQuantity) + "";
            //            detail.TempData1 = detail.Money + "";
            //            ht.Add(linkStr, detail);
            //        }
            //    }
            //}
            //foreach (MatHireBalanceOtherCostDtl otherDetail in matBalanceMaster.MatBalOtherCostDetails)
            //{
            //    if (otherDetail.UsedPart != null && otherDetail.SubjectGUID != null && otherDetail.MaterialResource != null)
            //    {
            //        string linkStr = otherDetail.UsedPart.Id + "-" + otherDetail.MaterialResource.Id + "-" + otherDetail.SubjectGUID.Id;
            //        if (ht.Contains(linkStr))
            //        {
            //            MatHireBalanceDetail temp = (MatHireBalanceDetail)ht[linkStr];
            //            temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) + otherDetail.CostMoney) + "";
            //            ht.Remove(linkStr);
            //            ht.Add(linkStr, temp);
            //        }
            //        else
            //        {
            //            MatHireBalanceDetail temp = new MatHireBalanceDetail();
            //            temp.MaterialCode = otherDetail.MaterialCode;
            //            temp.MaterialResource = otherDetail.MaterialResource;
            //            temp.MaterialName = otherDetail.MaterialName;
            //            temp.MatStandardUnit = otherDetail.MatStandardUnit;
            //            temp.MatStandardUnitName = otherDetail.MatStandardUnitName;
            //            temp.UsedPart = otherDetail.UsedPart;
            //            temp.UsedPartSysCode = otherDetail.UsedPartSysCode;
            //            temp.UsedPartName = otherDetail.UsedPartName;
            //            temp.TempData1 = otherDetail.CostMoney + "";
            //            temp.SubjectGUID = otherDetail.SubjectGUID;
            //            temp.SubjectName = otherDetail.SubjectName;
            //            temp.SubjectSysCode = otherDetail.SubjectSysCode;
            //            ht.Add(linkStr, temp);
            //        }
            //    }
            //}
            //if (ht.Count > 0)
            //{
            //    foreach (MatHireBalanceDetail dtl in ht.Values)
            //    {
            //        MaterialSettleDetail del = new MaterialSettleDetail();
            //        del.MaterialCode = dtl.MaterialCode;
            //        del.MaterialResource = dtl.MaterialResource;
            //        del.MaterialName = dtl.MaterialName;
            //        del.MatStandardUnit = dtl.MatStandardUnit;
            //        del.MatStandardUnitName = dtl.MatStandardUnitName;
            //        del.ProjectTask = dtl.UsedPart;
            //        del.ProjectTaskCode = dtl.UsedPartSysCode;
            //        del.ProjectTaskName = dtl.UsedPartName;
            //        del.Quantity = TransUtil.ToDecimal(dtl.TempData);
            //        del.Money = TransUtil.ToDecimal(dtl.TempData1);
            //        if (del.Quantity != 0)
            //        {
            //            del.Price = decimal.Round(del.Money / del.Quantity, 4);
            //        }
            //        del.AccountCostSubject = dtl.SubjectGUID;
            //        del.AccountCostName = dtl.SubjectName;
            //        del.AccountCostCode = dtl.SubjectSysCode;
            //        del.Master = masterMat;
            //        masterMat.AddDetail(del);
            //    }
            //    //增加费用调整
            //    if (otherMoney != 0)
            //    {
            //        MaterialSettleDetail del = new MaterialSettleDetail();
            //        del.MaterialCode = material.Code;
            //        del.MaterialResource = material;
            //        del.MaterialName = material.Name;
            //        del.ProjectTask = task;
            //        del.ProjectTaskCode = task.SysCode;
            //        del.ProjectTaskName = task.Name;
            //        del.Quantity = 0;
            //        del.Money = otherMoney;
            //        del.Price = 0;
            //        del.AccountCostSubject = otherSubject;
            //        del.AccountCostName = otherSubject.Name;
            //        del.AccountCostCode = otherSubject.SysCode;
            //        del.Master = masterMat;
            //        masterMat.AddDetail(del);
            //    }
            //}
            ////matBalanceMaster
            //masterMat.CreatePerson = matBalanceMaster.CreatePerson;
            //masterMat.CreatePersonName = matBalanceMaster.CreatePersonName;
            //masterMat.OperOrgInfoName = matBalanceMaster.OperOrgInfoName;
            //masterMat.OperOrgInfo = matBalanceMaster.OperOrgInfo;
            //masterMat.OpgSysCode = matBalanceMaster.OpgSysCode;
            //masterMat.HandleOrg = matBalanceMaster.HandleOrg;
            //masterMat.HandlePerson = matBalanceMaster.HandlePerson;
            //masterMat.HandlePersonName = matBalanceMaster.HandlePersonName;
            //masterMat.DocState = DocumentState.InExecute;
            //masterMat.ProjectId = ProjectInfo.Id;
            //masterMat.ProjectName = ProjectInfo.Name;
            //masterMat.CreateDate = OperEndDate;//制单时间
            //masterMat.RealOperationDate = DateTime.Now;
            //masterMat.CreateYear = fiscalYear;//制单年
            //masterMat.CreateMonth = fiscalMonth;//制单月
            //masterMat.SettleState = "materialQuery";
            //masterMat.MonthlySettlment = matBalanceMaster.Id;
            //materialSettleSrv.SaveMaterialSettle(masterMat);
            #endregion
        }
        public decimal GetPrice(Material oMaterial, Hashtable htMaterialPrice,string sMsg)
        {
            decimal dPrice=0;
            if (htMaterialPrice.ContainsKey(oMaterial.Id))//获取合同中价格
            {
                dPrice = ClientUtil.ToDecimal(htMaterialPrice[oMaterial.Id]);
            }
            else
            {
                dPrice = 0;
                throw new Exception(string.Format("[{2}]中编号[{0}]的[{1}]物资没有签订合同，无法获取当前的价格", oMaterial.Code, oMaterial.Name,sMsg));
            }
            return dPrice;
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

            //IList list_collection = GetMatHireCollectionMaster(oq);
            //IList list_return = GetMatHireReturnMaster(oq);

            //foreach (MatHireCollectionMaster master in list_collection)
            //{
            //    if (master != null)
            //    {
            //        master.BalYear = 0;
            //        master.BalMonth = 0;
            //        master.BalState = 0;
            //        UpdateByDao(master);
            //    }
            //}
            //foreach (MatHireReturnMaster master in list_return)
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

            string sql = " update thd_MatHireCollectionmaster t1 set t1.balstate=0,t1.balyear=0,t1.balmonth=0 where t1.projectid='" + ProjectInfo.Id + "' " +
                            " and t1.supplierrelation='" + theSupplier.Id + "' and t1.balstate=1 and t1.balyear=" + fiscalYear + " and t1.balmonth=" + fiscalMonth;
            command.CommandText = sql;
            command.ExecuteNonQuery();


            sql = " update thd_MatHireReturnmaster t1 set t1.balstate=0,t1.balyear=0,t1.balmonth=0 where t1.projectid='" + ProjectInfo.Id + "' " +
                            " and t1.supplierrelation='" + theSupplier.Id + "' and t1.balstate=1 and t1.balyear=" + fiscalYear + " and t1.balmonth=" + fiscalMonth;
            command.CommandText = sql;
            command.ExecuteNonQuery();

            sql = " update thd_mathirestockblockmaster t1 set t1.balstate=0,t1.balyear=0,t1.balmonth=0 where t1.projectid='" + ProjectInfo.Id + "' " +
                           " and t1.supplierrelation='" + theSupplier.Id + "' and t1.balstate=1 and t1.balyear=" + fiscalYear + " and t1.balmonth=" + fiscalMonth;
            command.CommandText = sql;
            command.ExecuteNonQuery();
            sql = " update thd_mathiretrancostmaster t1 set t1.balstate=0,t1.balyear=0,t1.balmonth=0 where t1.projectid='" + ProjectInfo.Id + "' " +
                          " and t1.supplierrelation='" + theSupplier.Id + "' and t1.balstate=1 and t1.balyear=" + fiscalYear + " and t1.balmonth=" + fiscalMonth;
            command.CommandText = sql;
            command.ExecuteNonQuery();
            #endregion

            #region 2.删除结算信息
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oQuery.AddCriterion(Expression.Eq("FiscalYear", fiscalYear));
            oQuery.AddCriterion(Expression.Eq("FiscalMonth", fiscalMonth));
            oQuery.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));
            //MatHireBalanceMaster MatBalMaster = GetMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);
            IList lst = dao.ObjectQuery(typeof(MatHireBalanceMaster),oQuery);
            MatHireBalanceMaster MatBalMaster = lst == null || lst.Count == 0 ? null : lst[0] as MatHireBalanceMaster;
            if (MatBalMaster != null)
                this.DeleteByDao(MatBalMaster);
            #endregion

            //删除料具结算信息(商务)
            //this.DeleteMaterialSettleMaster(fiscalYear, fiscalMonth, ProjectInfo.Id);
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
            MatHireBalanceMaster oMatHireBalanceMaster = GetMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);
            if (oMatHireBalanceMaster == null)
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
                string sSQL = string.Format("select count(*) from thd_MaterialSettleMaster t where t.monthaccountbill is not null and  t.monthlysettlment='{0}'", oMatHireBalanceMaster.Id);
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
            string sql = "SELECT id FROM	THD_MaterialHireBalanceMaster WHERE SupplierRelation ='" + theSupplier.Id + "' and ProjectId='" + ProjectInfo.Id + "'";
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
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("FiscalYear", previousYear));
            oq.AddCriterion(Expression.Eq("FiscalMonth", previousMonth));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));
            IList lst = dao.ObjectQuery(typeof(MatHireBalanceMaster), oq);
            //MatHireBalanceMaster master = GetMatBalanceMaster(previousYear, previousMonth, theSupplier, ProjectInfo);
            if (lst == null || lst.Count==0)
            {
                return false;
            }
            else
            {
                return true;
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
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("FiscalYear", fiscalYear));
            oq.AddCriterion(Expression.Eq("FiscalMonth", fiscalMonth));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", theSupplier));
            //MatHireBalanceMaster master =GetMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);
            IList lst= dao.ObjectQuery(typeof(MatHireBalanceMaster), oq);
            if (lst != null && lst.Count>0)
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
            MatHireBalanceMaster master = GetMatBalanceMaster(nextYear, nextMonth, theSupplier, ProjectInfo);
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
        public MaterialSettleMaster GetMaterialHireSettlementById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddFetchMode("Details.MaterialSubjectDetails.SettleSubject", NHibernate.FetchMode.Eager);
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialHireSettlement(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialSettleMaster;
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
        public MaterialSettleMaster GetMaterialHireSettlementByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetMaterialHireSettlement(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialSettleMaster;
            }
            return null;
        }

        /// <summary>
        /// 设备租赁结算查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialHireSettlement(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.UsedPart", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialSubjectDetails", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialSettleMaster), objectQuery);
        }

        public IList GetMaterialSubjectByParentId(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("MasterCost.Id", id));
            return Dao.ObjectQuery(typeof(MatHireSubjectDetail), oq);
        }

        /// <summary>
        /// 设备租赁结算查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet MaterialHireSettlementQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t2.Id,t1.Code,t1.SupplierName,t1.SupplierRelation,t1.AuditDate,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.Quantity,t2.SettleMoney,t1.State,
                t1.CreateDate,t1.CreatePersonName,t1.monthaccountbillid,t1.HandlePersonName,t1.RealOperationDate,t2.Descript, t3.balancesubjectname
                FROM THD_MaterialRentelSetMaster t1 INNER JOIN THD_MaterialHireSetDetail t2 ON t1.Id = t2.ParentId left join 
                        thd_subcontractbalsubjectdtl t3 on t2.id = t3.parentid";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        [TransManager]
        public MaterialSettleMaster AddMaterialHireSettlement(MaterialSettleMaster obj)
        {
            obj.Code = GetCode(typeof(MaterialSettleMaster), obj.ProjectId);
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as MaterialSettleMaster;
        }

        [TransManager]
        public MaterialSettleMaster SaveMaterialHireSettlement(MaterialSettleMaster obj)
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
        public DataSet QueryForMaterialHireSettlementPrint(string conditon)
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
        public DataSet GetMaterialHireReport(DateTime dStart, DateTime dEnd, string sSupplierRelationId, string sProjectId, string sMaterialId)
        {
            string sSQL = @"select * from (
select 0 billType, t.code,t.createdate,t.realoperationdate,  t1.materialtype,t1.materiallength,0 Quantity, t1.ExitQuantity , t1.RejectQuantity,t1.LoseQuantity,
t1.BroachQuantity,t1.ConsumeQuantity,t1.DisCardQty,t1.RepairQty,t1.LossQty from thd_mathirereturnmaster t
join thd_mathirereturndetail t1 on t.id=t1.parentid and t1.exitquantity>0 and t1.material='{0}'
where t.state=5 and t.createdate between to_date('{1}','yyyy-mm-dd') and  to_date('{2}','yyyy-mm-dd') and t.projectid='{3}' {4}
union all
select 1, t.code,t.createdate,t.realoperationdate, t1.materialtype,t1.materiallength,t1.quantity,0,0,0,0,0,0,0,0 from thd_mathirecollectionmaster t
join thd_mathirecollectiondetail t1 on t.id=t1.parentid and t1.quantity>0 and t1.material='{0}'
where t.state=5 and t.createdate between to_date('{1}','yyyy-mm-dd') and  to_date('{2}','yyyy-mm-dd')  and t.projectid='{3}' {4}
) order by createdate asc,realoperationdate asc";
            sSQL = string.Format(sSQL, sMaterialId, dStart.ToString("yyyy-MM-dd"), dEnd.ToString("yyyy-MM-dd"), sProjectId,
                (string.IsNullOrEmpty(sSupplierRelationId) ? "" : string.Format(" and  t.supplierrelation ='{0}' ", sSupplierRelationId)));
            return GetData(sSQL);
        }
        public DataSet GetMaterialHireBuilding(int iYear, int iMonth)
        {
            DateTime date = new DateTime(iYear, iMonth , 1).AddMonths(1).AddDays(-1);
            string sSQL = @"SELECT tt1.projectname,TT.* FROM (
SELECT projectid,materialname,max(createdate) maxDate,min(createdate) minDate,SUM(enterQty)enterQty,sum(exitQty)exitQty,sum(LOSSQTY)LOSSQTY,sum(REJECTQUANTITY)REJECTQUANTITY,sum(CONSUMEQUANTITY)CONSUMEQUANTITY from (
SELECT  projectid,(case when materialcode='I1450300001' then '3.25钢管(米)' when  materialcode='I1450300000' then '3.0钢管(米)' when materialcode='I1450100027' then '扣件' 
when materialcode='I1450100028' then '快拆头'when instr(materialcode,'I14502')>0 then '碗扣(米)' else '' end)materialname ,createdate,enterQty,exitQty,LOSSQTY,REJECTQUANTITY,CONSUMEQUANTITY 
 FROM(select t.projectid, t1.materialcode ,t.createdate, 0 enterQty,t1.Exitquantity*t1.materiallength exitQty,0 LOSSQTY,0 REJECTQUANTITY,0 CONSUMEQUANTITY 
from thd_mathirereturnmaster t
join thd_mathirereturndetail t1 on t.id=t1.parentid AND T1.EXITQUANTITY!=0 where  t.createdate<=to_date('{0}','yyyy-mm-dd') and t.Isloss=0
UNION ALL
select t.projectid, t1.materialcode ,t.createdate, 0 enterQty,t1.Exitquantity*t1.materiallength exitQty,t1.LOSSQTY*t1.materiallength LOSSQTY,t1.REJECTQUANTITY*t1.materiallength REJECTQUANTITY,t1.CONSUMEQUANTITY*t1.materiallength CONSUMEQUANTITY 
from thd_mathirereturnmaster t
join thd_mathirereturndetail t1 on t.id=t1.parentid AND T1.EXITQUANTITY!=0  where  t.createdate<=to_date('{0}','yyyy-mm-dd') and t.Isloss=1
UNION ALL
select t.projectid, t1.materialcode,t.createdate, t1.quantity*t1.materiallength quantity,0,0,0,0 from thd_mathirecollectionmaster t
join thd_mathirecollectiondetail t1 on t.id=t1.parentid AND T1.QUANTITY!=0 where  t.createdate<=to_date('{0}','yyyy-mm-dd')
 )) WHERE  materialname IS NOT NULL GROUP BY projectid,materialname) TT
 JOIN RESCONFIG TT1 ON TT.projectid=TT1.ID ORDER BY tt1.projectname,TT.materialname";
            sSQL = string.Format(sSQL, date.ToString("yyyy-MM-dd"));
            return GetData(sSQL);
        }
        public DataSet GetData(string sSQL)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
      
        public IList QueryMatHireBalanceReport(DateTime startTime, DateTime endTime, string sProjectId, string sSupplyId)
        {
            IList lstResult = new ArrayList();
            DataSet dsResult = new DataSet();
            string sSQLDetail = @"select tt.billCode,tt.materialcode,tt.materialname,tt.matstandardunitname,tt.billtype,tt.rentalprice,tt.startdate,tt.days,
sum(tt.matcolldtlqty) enterQty,sum(tt.matreturndtlqty)exitQty,sum(tt.money)money
from (select t1.materialcode,t1.materialname,nvl(t1.matstandardunitname,'')matstandardunitname,t1.billtype,
t1.rentalprice,decode(t1.billtype,2,t1.unusedbalquantity,t1.matcolldtlqty)matcolldtlqty,-t1.matreturndtlqty matreturndtlqty,t1.money,t1.startdate,t1.days, 
decode(t1.billtype,0,t1.matcollcode,1,t1.matreturncode,'') billCode
from thd_materialhirebalancemaster t  
join thd_materialhirebalancedetail t1 on t.id=t1.parentid and t1.money!=0
where t.projectid='{0}' and t.supplierrelation='{1}' and t.createdate between to_date('{2}','yyyy-mm-dd') and to_date('{3}','yyyy-mm-dd')) tt 
group by tt.billCode,tt.materialcode,tt.materialname,tt.matstandardunitname,tt.billtype,tt.rentalprice,tt.startdate,tt.days";
            string sSQLCost = @"select tt.materialcode,tt.materialname,tt.matstandardunitname,tt.businesstype,tt.costtype,tt.price,tt.constvalue,
sum(tt.enterQty) enterQty,sum(tt.exitQty) exitQty,sum(tt.costmoney)costmoney
from (select t1.materialcode,t1.materialname,nvl(t1.matstandardunitname,'')matstandardunitname,t1.businesstype,t1.costtype,
NVL(t1.price,0)price,NVL(t1.costmoney,0)costmoney,NVL(t1.constvalue,0)constvalue,
decode(t1.businesstype,'料具封存', t1.quantity,'退料',t1.quantity,0) exitQty,decode(t1.businesstype,'收料', t1.quantity,0) enterQty
 from thd_materialhirebalancemaster t   
join thd_mathirebalanceotherdtl t1 on t.id=t1.parentid and t1.costmoney!=0
where t.projectid='{0}' and t.supplierrelation='{1}' and t.createdate between to_date('{2}','yyyy-mm-dd') and to_date('{3}','yyyy-mm-dd')) tt
group by tt.materialcode,tt.materialname,tt.matstandardunitname,tt.businesstype,tt.costtype,tt.price,tt.constvalue";
            DataSet ds = GetData(string.Format(sSQLDetail, sProjectId, sSupplyId, startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd")));
            lstResult.Insert(0, ds);
             ds = GetData(string.Format(sSQLCost, sProjectId, sSupplyId, startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd")));
             lstResult.Insert(1, ds);
             return lstResult;
        }
        public DataSet QueryMaterialSizeReport(DateTime startTime, DateTime endTime, string sContractID,string sMaterialCode)
        {
            string sSQL = @"select * from (
select materialname,materialcode,materiallength,materialtype,sum(enterQuanity) enterQuanity,sum(exitQuanity)exitQuanity from (
select  t1.materialname,t1.materialcode,t1.materiallength,nvl(t1.materialtype,'')materialtype,t1.quantity enterQuanity,0 exitQuanity 
from thd_mathirecollectionmaster t
join thd_mathirecollectiondetail t1 on t.id=t1.parentid and t1.quantity!=0
where t.contractid='{0}' and  t.createdate between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd') and t1.materialcode='{3}' 
union all
select t1.materialname,t1.materialcode,t1.materiallength,nvl(t1.materialtype,'')materialtype,0 enterQuanity,t1.exitquantity exitQuanity  
from thd_mathirereturnmaster t
join thd_mathirereturndetail t1 on t.id=t1.parentid  and t1.exitquantity!=0
where t.contractid='{0}' and  t.createdate between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd') and t1.materialcode='{3}')
group by materialname,materialcode,materiallength,materialtype ) order by materialtype asc ,materiallength asc";
            return GetData(string.Format(sSQL, sContractID, startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd"),sMaterialCode));
        }
        public DataSet QueryMaterialDistributeReport(DateTime dEnd)
        {
            string sSQL = @"select t.projectid,t.projectname,t.material,t.materialcode,MATERIALNAME,MATERIALSPEC,MATSTANDARDUNITNAME,sum(leftquantity) qty from (
select t.projectid,t.projectname,t.material,t.materialcode,
T.MATERIALNAME,nvl(T.MATERIALSPEC,'')MATERIALSPEC,nvl(T.MATSTANDARDUNITNAME,'') MATSTANDARDUNITNAME,t.leftquantity leftquantity
from thd_materialhireledgermaster t where t.leftquantity!=0 and t.systemdate<=to_date('{0}','yyyy-mm-dd'))t
group by  t.projectid,t.projectname,t.material,t.materialcode,T.MATERIALNAME,T.MATERIALSPEC,T.MATSTANDARDUNITNAME";
//@"select t.projectid,t.projectname,t.materialcode,sum(t.leftquantity )qty
//from thd_materialhireledgermaster t  
//where t.leftquantity!=0 and t.materialcode in ({0}) and t.systemdate<=to_date('{1}','yyyy-mm-dd')
//group by  t.projectid,t.projectname,t.materialcode";
            return GetData(string.Format(sSQL,dEnd.ToString("yyyy-MM-dd")));
        }
    }
    
}
