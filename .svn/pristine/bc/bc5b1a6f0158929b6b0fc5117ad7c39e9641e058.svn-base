using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using NHibernate.Exceptions;
using NHibernate;
using System.Data;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.CommonClass.Domain;
using NHibernate.Criterion;
using Application.Resource.FinancialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;
using Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Service;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.StockInventory.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Service
{
    public class StockInOutSrv : Application.Business.Erp.SupplyChain.StockManage.Stock.Service.IStockInOutSrv
    {

        #region 服务注入
        private IDao dao;

        virtual public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        private IDataAccess dBDao;
        /// <summary>
        /// 直接数据库操作DAO
        /// </summary>
        public IDataAccess DBDao
        {
            get { return dBDao; }
            set { dBDao = value; }
        }

        private IMaterialSettleSrv materialSettleSrv;
        public IMaterialSettleSrv MaterialSettleSrv
        {
            get { return materialSettleSrv; }
            set { materialSettleSrv = value; }
        }
        #endregion

        #region 结帐
        [TransManager]
        public bool Reckoning(StockInOut oStockInOut, CurrentProjectInfo projectInfo, PersonInfo oPersonInfo)
        {
            oStockInOut.RealOperationDate = DateTime.Now;
            InsertStkStockInOut(oStockInOut);
            CreateRealConsumeBill(oStockInOut, projectInfo, oPersonInfo);
            return true;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;

            IDbCommand cmd = cnn.CreateCommand();
            cmd.CommandText = "thd_ReckoningByFiscal";
            cmd.CommandType = CommandType.StoredProcedure;


            IDbDataParameter V_fiscalYear = cmd.CreateParameter();
            V_fiscalYear.Value = oStockInOut.FiscalYear;
            V_fiscalYear.Direction = ParameterDirection.Input;
            V_fiscalYear.ParameterName = "p_fiscalYear";

            cmd.Parameters.Add(V_fiscalYear);

            IDbDataParameter V_fiscalMonth = cmd.CreateParameter();
            V_fiscalMonth.Value = oStockInOut.FiscalYear;
            V_fiscalMonth.Direction = ParameterDirection.Input;
            V_fiscalMonth.ParameterName = "p_fiscalMonth";

            cmd.Parameters.Add(V_fiscalMonth);

            IDbDataParameter v_projectId = cmd.CreateParameter();
            v_projectId.Value = projectInfo.Id;
            v_projectId.Direction = ParameterDirection.Input;
            v_projectId.ParameterName = "p_projectId";
            cmd.Parameters.Add(v_projectId);

            IDbDataParameter err = cmd.CreateParameter();
            err.DbType = DbType.AnsiString;
            err.Direction = ParameterDirection.Output;
            err.ParameterName = "errMsg";
            err.Size = 500;
            cmd.Parameters.Add(err);
            cmd.ExecuteNonQuery();
            if (!(err.Value == null || Convert.ToString(err.Value) == ""))
                throw new Exception(Convert.ToString(err.Value));
            return true;
        }
        [TransManager]
        public void InsertStkStockInOut(StockInOut oStockInOut)
        {
            if (oStockInOut != null)
            {
                Dao.SaveOrUpdate(oStockInOut);
            }
        }
        [TransManager]
        private void CreateRealConsumeBill(StockInOut oStockInOut, CurrentProjectInfo projectInfo, PersonInfo oPersonInfo)
        {
            int kjn = oStockInOut.FiscalYear;
            int kjy = oStockInOut.FiscalMonth;
            string endDate = this.GetEndDateByFiscalPeriod(kjn, kjy);
            //读取本月的领料出库的信息
            MaterialSettleMaster exMaster = new MaterialSettleMaster();
            ObjectQuery oq = new ObjectQuery();
            IList lstProfessionStkOut = new ArrayList();

            IList lstBuildStkOut = new ArrayList();
            IList lstTotal = null;
            Hashtable oHashTb = new Hashtable();
            decimal dSumQuality = 0;
            decimal dSumMoney = 0;
            //查询自己的单据
            oq.AddCriterion(Expression.Le("CreateDate", TransUtil.ToDateTime(endDate)));
            oq.AddCriterion(Expression.Eq("StockOutManner", EnumStockInOutManner.领料出库));
            //区分项目
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("IsTally", 1));
            oq.AddCriterion(Expression.IsNull("MonthConsumeId"));
            oq.AddCriterion(Expression.Like("OpgSysCode", oStockInOut.AccountOrgSyscode, MatchMode.Start));
            lstTotal = GetStockOut(oq);
            if (lstTotal.Count > 0)
            {
                GetSplit(lstTotal, lstProfessionStkOut, lstBuildStkOut);
                if (lstBuildStkOut != null && lstBuildStkOut.Count > 0)//土建存在
                {
                    OperateBuildBill(lstBuildStkOut, ref dSumQuality, ref dSumMoney, oHashTb);
                }
                if (lstProfessionStkOut != null && lstProfessionStkOut.Count > 0)
                {
                    OperateProfessionBill(oStockInOut, projectInfo, lstProfessionStkOut, ref dSumQuality, ref dSumMoney, oHashTb);
                }

                exMaster.AuditYear = kjn;
                exMaster.AuditMonth = kjy;
                decimal sumMoney = 0;
                foreach (BasicStockOutDtl dtlStock in oHashTb.Values)
                {
                    MaterialSettleDetail exDetail = new MaterialSettleDetail();
                    exDetail.MaterialCode = dtlStock.MaterialCode;
                    exDetail.MaterialResource = dtlStock.MaterialResource;
                    exDetail.MaterialSpec = dtlStock.MaterialSpec;
                    exDetail.MaterialStuff = dtlStock.MaterialStuff;
                    exDetail.MaterialName = dtlStock.MaterialName;
                    exDetail.QuantityUnit = dtlStock.MatStandardUnit;
                    exDetail.QuantityUnitName = dtlStock.MatStandardUnitName;
                    exDetail.ProjectTask = dtlStock.UsedPart;
                    
                    exDetail.ProjectTaskCode = dtlStock.UsedPartSysCode;
                    exDetail.ProjectTaskName = dtlStock.UsedPartName;
                    exDetail.Quantity = TransUtil.ToDecimal(dtlStock.TempData1);
                    exDetail.Money = TransUtil.ToDecimal(dtlStock.TempData2);
                    sumMoney += exDetail.Money;
                    if (exDetail.Quantity != 0)
                    {
                        exDetail.Price = decimal.Round(exDetail.Money / exDetail.Quantity, 8);//结算时价格保留8位
                    }
                    exDetail.AccountCostSubject = dtlStock.SubjectGUID;
                    exDetail.AccountCostName = dtlStock.SubjectName;
                    exDetail.AccountCostCode = dtlStock.SubjectSysCode;
                    exDetail.Master = exMaster;
                    exMaster.AddDetail(exDetail);
                }
                //废旧物资对应科目
                CostAccountSubject wasterSubject = new CostAccountSubject();
                wasterSubject.Id = TransUtil.ConWasterSubjectId;
                wasterSubject.Name = TransUtil.ConWasterSubjectName;
                wasterSubject.SysCode = TransUtil.ConWasterSubjectSyscode;
                //废旧物资冲减成本
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Le("CreateDate", TransUtil.ToDateTime(endDate)));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute));
                oq.AddCriterion(Expression.IsNull("MonthConsumeId"));
                oq.AddCriterion(Expression.Like("OpgSysCode", oStockInOut.AccountOrgSyscode, MatchMode.Start));
                IList wasterList = dao.ObjectQuery(typeof(WasteMatProcessMaster), oq);
                foreach (WasteMatProcessMaster master in wasterList)
                {
                    foreach (WasteMatProcessDetail detail in master.Details)
                    {
                        MaterialSettleDetail exDetail = new MaterialSettleDetail();
                        exDetail.MaterialCode = detail.MaterialCode;
                        exDetail.MaterialResource = detail.MaterialResource;
                        exDetail.MaterialSpec = detail.MaterialSpec;
                        exDetail.MaterialStuff = detail.MaterialStuff;
                        exDetail.MaterialName = detail.MaterialName;
                        exDetail.QuantityUnit = detail.MatStandardUnit;
                        exDetail.QuantityUnitName = detail.MatStandardUnitName;
                        exDetail.ProjectTask = detail.UsedPart;
                        exDetail.ProjectTaskCode = detail.UsedPartSysCode;
                        exDetail.ProjectTaskName = detail.UsedPartName;
                        exDetail.Quantity = detail.NetWeight;
                        exDetail.Money = -detail.TotalValue;
                        sumMoney += -detail.TotalValue;
                        exDetail.Price = -detail.ProcessPrice;
                        exDetail.AccountCostSubject = wasterSubject;
                        exDetail.AccountCostName = wasterSubject.Name;
                        exDetail.AccountCostCode = wasterSubject.SysCode;
                        exDetail.Master = exMaster;
                        exMaster.AddDetail(exDetail);
                    }          
                }

                BasicStockOut stockMaster = lstTotal[0] as BasicStockOut;
                // PersonInfo pInfo = new PersonInfo();
                // pInfo.Id = oStockInOut.CreatePersonID;
                // pInfo.Name = oStockInOut.CreatePersonName;
                exMaster.CreatePerson = oPersonInfo;
                exMaster.CreatePersonName = oStockInOut.CreatePersonName;
                exMaster.OperOrgInfo = stockMaster.OperOrgInfo;
                exMaster.OperOrgInfoName = stockMaster.OperOrgInfoName;
                exMaster.OpgSysCode = stockMaster.OpgSysCode;
                exMaster.HandOrgLevel = stockMaster.OperOrgInfo.Level;
                exMaster.HandleOrg = stockMaster.HandleOrg;
                exMaster.HandlePerson = stockMaster.HandlePerson;
                exMaster.HandlePersonName = stockMaster.HandlePersonName;
                exMaster.ProjectId = projectInfo.Id;
                exMaster.ProjectName = projectInfo.Name;
                exMaster.CreateDate = DateTime.Parse(endDate);//制单时间
                exMaster.RealOperationDate = DateTime.Now;
                exMaster.CreateYear = kjn;//制单年
                exMaster.CreateMonth = kjy;//制单月
                exMaster.SumMoney = sumMoney;
                exMaster.DocState = DocumentState.InExecute;
                exMaster.SettleState = "materialConsume";
                exMaster.MonthlySettlment = oStockInOut.Id;
                exMaster.MonthlyAccount = 1;
                exMaster = MaterialSettleSrv.SaveMaterialSettle(exMaster);
                UpdateMaterailSetSyscode();
                this.WriteForwardStockOutFlag(endDate, projectInfo.Id, oStockInOut);
            }
        }
        /// <summary>
        /// 将出库单 安装专业分类
        /// </summary>
        /// <param name="lstTotal"></param>
        /// <param name="lstProfessionStkOut"></param>
        /// <param name="lstBuildStkOut"></param>
        public void GetSplit(IList lstTotal, IList lstProfessionStkOut, IList lstBuildStkOut)
        {
            lstProfessionStkOut.Clear();
            lstBuildStkOut.Clear();
            foreach (BasicStockOut oBasicStockOut in lstTotal)
            {
                switch (oBasicStockOut.Special)
                {
                    case "安装":
                        {
                            lstProfessionStkOut.Add(oBasicStockOut);
                            break;
                        }
                    case "土建":
                        {
                            lstBuildStkOut.Add(oBasicStockOut);
                            break;
                        }
                }
            }
        }

        public void OperateBuildBill(IList list, ref decimal sumQ, ref decimal sumM, Hashtable ht)
        {
            foreach (BasicStockOut outMaster in list)
            {
                foreach (BasicStockOutDtl outMat in outMaster.Details)
                {
                    if (outMat.UsedPart != null && outMat.SubjectGUID != null && outMat.MaterialResource.Id != null)
                    {
                        string linkStr = outMat.UsedPart.Id + "-" + outMat.MaterialResource.Id + "-" + outMat.SubjectGUID.Id;
                        if (!ht.Contains(linkStr))
                        {
                            outMat.TempData1 = outMat.Quantity + "";
                            outMat.TempData2 = outMat.Money + "";
                            ht.Add(linkStr, outMat);
                        }
                        else
                        {
                            BasicStockOutDtl temp = (BasicStockOutDtl)ht[linkStr];
                            ht.Remove(linkStr);
                            temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) + outMat.Quantity) + "";
                            temp.TempData2 = (TransUtil.ToDecimal(temp.TempData2) + outMat.Money) + "";
                            ht.Add(linkStr, temp);
                        }
                    }
                    sumQ += outMat.Quantity;
                    sumM += outMat.Money;
                }
            }
        }
        [TransManager]
        public void OperateProfessionBill(StockInOut oStockInOut, CurrentProjectInfo projectInfo, IList lstProfessionStkOut, ref decimal dSumQuality, ref decimal dSumMoney, Hashtable oHashTb)
        {
            if (lstProfessionStkOut != null && lstProfessionStkOut.Count > 0)
            {
                int kjn = oStockInOut.FiscalYear;
                int kjy = oStockInOut.FiscalMonth;
                string endDate = this.GetEndDateByFiscalPeriod(kjn, kjy);
                //读取本月的领料出库的信息

                #region 本月出库
                //ObjectQuery oq = new ObjectQuery();
                ////查询自己的单据
                ////oq.AddCriterion(Expression.Eq("CreateYear", kjn));
                ////oq.AddCriterion(Expression.Eq("CreateMonth", kjy));
                //oq.AddCriterion(Expression.Le("CreateDate", TransUtil.ToDateTime(endDate)));
                //oq.AddCriterion(Expression.Eq("StockOutManner", EnumStockInOutManner.领料出库));
                ////区分项目
                //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                //oq.AddCriterion(Expression.Eq("IsTally", 1));
                //oq.AddCriterion(Expression.IsNull("MonthConsumeId"));
                //oq.AddCriterion(Expression.Eq("OpgSysCode", oStockInOut.AccountPersonOrgSysCode));
                //IList list = GetStockOut(oq);
                #endregion
                #region 本月盘点单
                ObjectQuery oq = new ObjectQuery();
                if (kjy == 1)
                {
                    kjy = 12;
                    kjn -= 1;
                }
                else
                {
                    kjy -= 1;
                }
                StockInventoryMaster o;
               
                string sCurrStartDate = this.GetEndDateByFiscalPeriod(kjn, kjy);
                oq.AddCriterion(Expression.Gt("CreateDate", TransUtil.ToDateTime(sCurrStartDate)));//刚好在那个时间段内
                oq.AddCriterion(Expression.Le("CreateDate", TransUtil.ToDateTime(endDate)));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute ));
                oq.AddCriterion(Expression.Like("OpgSysCode", oStockInOut.AccountOrgSyscode, MatchMode.Start));
                IList listCurrMonthInve = Dao.ObjectQuery(typeof(StockInventoryMaster), oq);
                #endregion
                #region 上个月盘点单
                if (kjy == 1)
                {
                    kjy = 12;
                    kjn -= 1;
                }
                else
                {
                    kjy -= 1;
                }
                string sLastStartDate = this.GetEndDateByFiscalPeriod(kjn, kjy);
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Gt("CreateDate", TransUtil.ToDateTime(sLastStartDate)));
                oq.AddCriterion(Expression.Le("CreateDate", TransUtil.ToDateTime(sCurrStartDate)));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
                oq.AddCriterion(Expression.Like("OpgSysCode", oStockInOut.AccountOrgSyscode, MatchMode.Start));
                IList listLastMonthInve = Dao.ObjectQuery(typeof(StockInventoryMaster), oq);
                #endregion

                //计算公式：上期盘点+本月盘点-本月盘点=当月消耗
                //当前月出库
                if (lstProfessionStkOut.Count > 0)
                {
                    foreach (BasicStockOut outMaster in lstProfessionStkOut)
                    {
                        foreach (BasicStockOutDtl outMat in outMaster.Details)
                        {
                            if (outMat.UsedPart != null && outMat.SubjectGUID != null && outMat.MaterialResource.Id != null)
                            {
                                try
                                {
                                    string linkStr = outMat.UsedPart.Id + "-" + outMat.MaterialResource.Id + "-" + outMat.SubjectGUID.Id;
                                    if (!oHashTb.Contains(linkStr))
                                    {
                                        outMat.TempData1 = outMat.Quantity + "";
                                        outMat.TempData2 = outMat.Money + "";
                                        try
                                        {
                                            outMat.UsedPartName = outMat.UsedPart.Name;
                                        }
                                        catch { }
                                        oHashTb.Add(linkStr, outMat);

                                    }
                                    else
                                    {
                                        BasicStockOutDtl temp = (BasicStockOutDtl)oHashTb[linkStr];
                                        oHashTb.Remove(linkStr);
                                        temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) + outMat.Quantity) + "";
                                        temp.TempData2 = (TransUtil.ToDecimal(temp.TempData2) + outMat.Money) + "";
                                        oHashTb.Add(linkStr, temp);
                                    }
                                }
                                catch { }
                            }
                            dSumQuality += outMat.Quantity;
                            dSumMoney += outMat.Money;
                        }
                    }
                }
                //本月盘点
                if (listCurrMonthInve.Count > 0)
                {
                    foreach (StockInventoryMaster outMaster in listCurrMonthInve)
                    {
                        foreach (StockInventoryDetail outMat in outMaster.Details)
                        {
                            if (outMaster.UserPart != null && outMat.SubjectGuid != null && outMat.MaterialResource.Id != null)
                            {
                                string linkStr = outMaster.UserPart.Id + "-" + outMat.MaterialResource.Id + "-" + outMat.SubjectGuid;
                                if (!oHashTb.Contains(linkStr))
                                {
                                    BasicStockOutDtl temp = CopyStockInventry(outMaster, outMat);
                                    temp.TempData1 = -outMat.InventoryQuantity + "";
                                    temp.TempData2 = -outMat.Money + "";
                                    //outMat.TempData1 = outMat.Quantity + "";
                                    //outMat.TempData2 = outMat.Money + "";
                                    oHashTb.Add(linkStr, temp);
                                    outMat.UsedPart = outMaster.UserPart;
                                    outMat.UsedPartName = outMat.UsedPart.Name;
                                    outMat.UsedPartSysCode = outMaster.UserPart.SysCode;
                                }
                                else
                                {
                                    BasicStockOutDtl temp = (BasicStockOutDtl)oHashTb[linkStr];
                                    oHashTb.Remove(linkStr);
                                    temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) - outMat.InventoryQuantity) + "";
                                    temp.TempData2 = (TransUtil.ToDecimal(temp.TempData2) - outMat.Money) + "";
                                    oHashTb.Add(linkStr, temp);
                                }
                            }
                            dSumQuality -= outMat.InventoryQuantity;
                            dSumMoney -= outMat.Money;
                        }
                    }
                }
                //上个月盘点
                if (listLastMonthInve.Count > 0)
                {
                    foreach (StockInventoryMaster outMaster in listLastMonthInve)
                    {
                        foreach (StockInventoryDetail outMat in outMaster.Details)
                        {
                            if (outMaster.UserPart != null && outMat.SubjectGuid != null && outMat.MaterialResource.Id != null)
                            {
                                string linkStr = outMaster.UserPart + "-" + outMat.MaterialResource.Id + "-" + outMat.SubjectGuid;
                                if (!oHashTb.Contains(linkStr))
                                {
                                    BasicStockOutDtl temp = CopyStockInventry(outMaster, outMat);
                                    temp.TempData1 = outMat.InventoryQuantity + "";
                                    temp.TempData2 = outMat.Money + "";
                                    //outMat.TempData1 = outMat.Quantity + "";
                                    //outMat.TempData2 = outMat.Money + "";
                                    outMat.UsedPart = outMaster.UserPart;
                                    outMat.UsedPartName = outMat.UsedPart.Name;
                                    outMat.UsedPartSysCode = outMaster.UserPart.SysCode;
                                    oHashTb.Add(linkStr, temp);
                                }
                                else
                                {
                                    BasicStockOutDtl temp = (BasicStockOutDtl)oHashTb[linkStr];
                                    oHashTb.Remove(linkStr);
                                    temp.TempData1 = (TransUtil.ToDecimal(temp.TempData1) + outMat.InventoryQuantity) + "";
                                    temp.TempData2 = (TransUtil.ToDecimal(temp.TempData2) + outMat.Money) + "";
                                    oHashTb.Add(linkStr, temp);
                                }
                            }
                            dSumQuality += outMat.InventoryQuantity;
                            dSumMoney += outMat.Money;
                        }
                    }
                }
            }

        }
        public BasicStockOutDtl CopyStockInventry(StockInventoryMaster oStockInventoryMaster, StockInventoryDetail oStockInventoryDetail)
        {
            BasicStockOutDtl oBasicStockOutDtl = new StockOutDtl();
            oBasicStockOutDtl.MaterialCode = oStockInventoryDetail.MaterialCode;
            oBasicStockOutDtl.MaterialResource = oStockInventoryDetail.MaterialResource;
            oBasicStockOutDtl.MaterialSpec = oStockInventoryDetail.MaterialSpec;
            oBasicStockOutDtl.MaterialStuff = oStockInventoryDetail.MaterialStuff;
            oBasicStockOutDtl.MaterialName = oStockInventoryDetail.MaterialName;
            oBasicStockOutDtl.MatStandardUnit = oStockInventoryDetail.MatStandardUnit;
            oBasicStockOutDtl.MatStandardUnitName = oStockInventoryDetail.MatStandardUnitName;
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", oStockInventoryMaster.UserPart.Id));
            IList list = Dao.ObjectQuery(typeof(GWBSTree), oQuery);
            oBasicStockOutDtl.UsedPart = ((list != null && list.Count > 0) ? list[0] as GWBSTree : null); ;
            oBasicStockOutDtl.UsedPartSysCode = oStockInventoryDetail.UsedPartSysCode;
            oBasicStockOutDtl.UsedPartName = oStockInventoryDetail.UsedPartName;
            oBasicStockOutDtl.TempData1 = 0 + "";
            oBasicStockOutDtl.TempData2 = 0 + "";
            oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", oStockInventoryDetail.SubjectGuid));
            list = Dao.ObjectQuery(typeof(CostAccountSubject), oQuery);
            oBasicStockOutDtl.SubjectGUID = ((list != null && list.Count > 0) ? list[0] as CostAccountSubject : null);
            oBasicStockOutDtl.SubjectName = oStockInventoryDetail.SubjectName;
            oBasicStockOutDtl.SubjectSysCode = oStockInventoryDetail.SubjectSysCode;
            return oBasicStockOutDtl;
        }
        public bool IsSetUp(BasicStockOut oBasicStockOut)
        {
            bool bFlag = false;
            if (oBasicStockOut != null)
            {
                string sSpecial = oBasicStockOut.Special;
                if (!string.IsNullOrEmpty(sSpecial) && string.Equals(sSpecial.Trim(), "安装"))
                {
                    bFlag = true;
                }

            }
            return bFlag;
        }
        /// <summary>
        /// 获取当前会计期的结束日期
        /// </summary>
        /// <param name="kjn"></param>
        /// <param name="kjy"></param>
        /// <returns></returns>
        public string GetEndDateByFiscalPeriod(int kjn, int kjy)
        {
            string dt = "";
            string sql = "  select t1.enddate from resfiscalperioddet t1 where t1.fiscalyear=" + kjn + " and t1.fiscalmonth= " + kjy;

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sql;
            session.Transaction.Enlist(command);
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            while (dataReader.Read())
            {
                dt = dataReader.GetDateTime(0).ToShortDateString();
            }
            return dt;
        }


        /// <summary>
        /// 获取当前会计期的开始日期
        /// </summary>
        /// <param name="kjn"></param>
        /// <param name="kjy"></param>
        /// <returns></returns>
        public string GetStartDateByFiscalPeriod(int kjn, int kjy)
        {
            string dt = "";
            string sql = "  select t1.begindate from resfiscalperioddet t1 where t1.fiscalyear=" + kjn + " and t1.fiscalmonth= " + kjy;

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sql;
            session.Transaction.Enlist(command);
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            while (dataReader.Read())
            {
                dt = dataReader.GetDateTime(0).ToShortDateString();
            }
            return dt;
        }
        /// <summary>
        /// 根据会计年，会计月回写出库单主表信息和废旧物资主表
        /// </summary>
        /// <returns></returns>
        private void WriteForwardStockOutFlag(string endDate, string projectId, StockInOut oStockInOut)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                //string sql = " update thd_stkstockout set monthConsumeId='" + monthConsumeId + "' where createyear = " + kjn + " and createmonth=" + kjy +
                //            " and stockoutmanner=20 and isTally=1 and projectid='" + projectId + "'";
                string sql = " update thd_stkstockout set monthConsumeId='" + oStockInOut.Id + "' where createdate<=to_date('" + endDate + "','yyyy-mm-dd') " +
                                " and stockoutmanner=20 and isTally=1 and monthConsumeId is null and projectid='" + projectId 
                                + "' and opgsyscode like '" + oStockInOut.AccountOrgSyscode + "%'";
                command.CommandText = sql;
                command.ExecuteNonQuery();
                //废旧物资
                sql = " update thd_wastematprocessmaster set monthConsumeId='" + oStockInOut.Id + "' where createdate<=to_date('" + endDate + "','yyyy-mm-dd') " +
                                " and monthConsumeId is null and projectid='" + projectId + "' and state=5 and opgsyscode like '" + oStockInOut.AccountOrgSyscode + "%'";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 回写材料消耗单的资源层次玛
        /// </summary>
        /// <returns></returns>
        private void UpdateMaterailSetSyscode()
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                string sql = " update thd_materialsettledetail t1 set t1.materialsyscode= "+
                            " (select k1.thesyscode from resmaterial k1 where k1.materialid=t1.material) where t1.materialsyscode is null";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void InsertStkStockInOut(int kjn, int kjy, string projectId, PersonInfo oPersonInfo, GWBSTree oGWBSTree)
        {
            try
            {
                IFCGuidGenerator gen = new IFCGuidGenerator();
                string guid = gen.GeneratorIFCGuid();

                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                string sql = " insert into thd_stkstockinout t1 (t1.id,t1.version,t1.projectid,t1.fiscalyear,t1.fiscalmonth,t1.accttype) values " +
                                "('" + guid + "',0,'" + projectId + "'," + kjn + "," + kjy + ",1 ) ";

                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 根据会计年，会计月删除材料消耗信息
        /// </summary>
        /// <returns></returns>
        [TransManager]
        private void DeleteMaterialSettleMaster(StockInOut oStockInOut)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                string sql = " update thd_stkstockout set monthConsumeId='' where monthConsumeId = '" + oStockInOut.Id + "'";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                sql = " update thd_wastematprocessmaster set monthConsumeId='' where monthConsumeId = '" + oStockInOut.Id + "'";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                sql = " delete from THD_MaterialSettleMaster t1 where t1.monthlysettlment ='" + oStockInOut.Id + "'";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                sql = "delete from thd_stkstockinout t1 where t1.id ='" + oStockInOut.Id + "'";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IList GetStockOut(ObjectQuery oq)
        {
            //oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(BasicStockOut), oq);
        }

        [TransManager]
        public bool UnReckoning(StockInOut oStockInOut)
        {
            this.DeleteMaterialSettleMaster(oStockInOut);
            return true;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;

            IDbCommand cmd = cnn.CreateCommand();
            cmd.CommandText = "thd_UnReckoningByFiscal";
            cmd.CommandType = CommandType.StoredProcedure;


            IDbDataParameter V_fiscalYear = cmd.CreateParameter();
            V_fiscalYear.Value = oStockInOut.FiscalYear;
            V_fiscalYear.Direction = ParameterDirection.Input;
            V_fiscalYear.ParameterName = "p_fiscalYear";

            cmd.Parameters.Add(V_fiscalYear);

            IDbDataParameter V_fiscalMonth = cmd.CreateParameter();
            V_fiscalMonth.Value = oStockInOut.FiscalMonth;
            V_fiscalMonth.Direction = ParameterDirection.Input;
            V_fiscalMonth.ParameterName = "p_fiscalMonth";

            cmd.Parameters.Add(V_fiscalMonth);

            IDbDataParameter v_projectId = cmd.CreateParameter();
            v_projectId.Value = oStockInOut.ProjectId;
            v_projectId.Direction = ParameterDirection.Input;
            v_projectId.ParameterName = "p_projectId";
            cmd.Parameters.Add(v_projectId);

            IDbDataParameter err = cmd.CreateParameter();
            err.DbType = DbType.AnsiString;
            err.Direction = ParameterDirection.Output;
            err.ParameterName = "errMsg";
            err.Size = 500;
            cmd.Parameters.Add(err);
            cmd.ExecuteNonQuery();
            if (!(err.Value == null || Convert.ToString(err.Value) == ""))
                throw new Exception(Convert.ToString(err.Value));
            return true;
        }
        /// <summary>
        /// 检查指定的会计期是否结帐
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">会计月</param>
        /// <returns></returns>
        public bool CheckReckoning(int fiscalYear, int fiscalMonth, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();

            oq.AddCriterion(Expression.Eq("FiscalYear", fiscalYear));
            oq.AddCriterion(Expression.Eq("FiscalMonth", fiscalMonth));
            oq.AddCriterion(Expression.Eq("AcctType", 1));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            int count = Dao.Count(typeof(StockInOut), oq);
            if (count == 0) return false;
            return true;
        }

        /// <summary>
        /// 检查是否是新项目
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">会计月</param>
        /// <returns></returns>
        public bool CheckIfNewProject(string projectId)
        {
            ObjectQuery oq = new ObjectQuery();

            oq.AddCriterion(Expression.Eq("AcctType", 1));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            int count = Dao.Count(typeof(StockInOut), oq);
            if (count > 0) return false;
            return true;
        }
        /// <summary>
        /// 除了本月外其他月份结了没
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">会计月</param>
        /// <returns></returns>
        public bool CheckIfOtherMonthAcc(string projectId, int iYear, int iMonth)
        {
            ObjectQuery oq = new ObjectQuery();
            string sEndDate = this.GetEndDateByFiscalPeriod(iYear, iMonth);
            string sStartDate = this.GetStartDateByFiscalPeriod(iYear, iMonth);
            oq.AddCriterion(Expression.Eq("AcctType", 1));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            string sSQL = string.Format(" (fiscalyear!={0} or fiscalmonth!={1} )", iYear, iMonth);
            oq.AddCriterion(Expression.Sql(sSQL));
            int count = Dao.Count(typeof(StockInOut), oq);
            if (count > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取该核节点下最近结算记录
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sSysCode"></param>
        /// <returns></returns>
        public DataTable GetMaxAccTimeBySysCode(string sProjectID, string sSysCode)
        {
            DataTable oDataTable = null;
            string sSQL = string.Empty;
            sSQL = @"select     nvl(t.fiscalyear,0) fiscalyear,nvl( t.fiscalmonth,0) fiscalmonth from thd_stkstockinout t where (instr(t.accounttasksyscode,'{0}')>0 or instr('{0}',t.accounttasksyscode)>0)and t.projectid='{1}'  
                    order by to_number(to_char(t.fiscalyear)||lpad( to_char(t.fiscalmonth),2,0)) desc";
            sSQL = string.Format(sSQL, sSysCode, sProjectID);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sSQL;

            IDataReader oDataRead = command.ExecuteReader(CommandBehavior.Default);
            DataSet oDataSet = DataAccessUtil.ConvertDataReadertoDataSet(oDataRead);
            if (oDataSet != null && oDataSet.Tables.Count > 0 && oDataSet.Tables[0].Rows.Count > 0)
            {
                oDataTable = oDataSet.Tables[0];
            }
            return oDataTable;
        }
        /// <summary>
        /// 获取该项目下最早记账的会计年月
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <returns></returns>
        public DataTable GetMinAccTimeByProjectID(string sProjectID)
        {
            DataTable oDataTable = null;
            string sSQL = string.Empty;
            sSQL = @"select     t.fiscalyear , t.fiscalmonth  from thd_stkstockinout t where  t.projectid='{0}'  
                    order by to_number(to_char(t.fiscalyear)||lpad( to_char(t.fiscalmonth),2,0)) asc";
            sSQL = string.Format(sSQL, sProjectID);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sSQL;

            IDataReader oDataRead = command.ExecuteReader(CommandBehavior.Default);
            DataSet oDataSet = DataAccessUtil.ConvertDataReadertoDataSet(oDataRead);
            if (oDataSet != null && oDataSet.Tables.Count > 0 && oDataSet.Tables[0].Rows.Count > 0)
            {
                oDataTable = oDataSet.Tables[0];
            }
            return oDataTable;
        }
        /// <summary>
        /// 检查是否有出入库单未记账
        /// </summary>
        /// <param name="accountYear"></param>
        /// <param name="accountMonth"></param>
        /// <returns></returns>
        public string CheckStockInOutIsTally(int accountYear, int accountMonth, string projectId)
        {
            string info = "";

            string sql = " select '入库单:' infotype,t1.code from thd_stkstockin t1 where t1.istally=0 and t1.createyear=" + accountYear + " and t1.createmonth=" + accountMonth + " and t1.projectId='" + projectId + "'" +
                         "  union " +
                         "  select '出库单:' infotype,t1.code from thd_stkstockout t1 where t1.istally=0 and t1.createyear=" + accountYear + " and t1.createmonth=" + accountMonth + " and t1.projectId='" + projectId + "'";

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            while (dataReader.Read())
            {
                string infoType = dataReader.GetString(0);
                string code = dataReader.GetString(1);
                info += infoType + code + " \n";
            }
            return info;
        }

        /// <summary>
        /// 检查是否有收料入库单未做验收结算单
        /// </summary>
        /// <param name="accountYear"></param>
        /// <param name="accountMonth"></param>
        /// <returns></returns>
        public string CheckStockInIsBal(int accountYear, int accountMonth, string projectId)
        {
            string result = "";
            string sql = @"SELECT DISTINCT t1.Code,t1.Special 
                FROM THD_StkStockIn t1 INNER JOIN THD_StkStockInDtl t2 ON t1.id=t2.ParentId
                WHERE t1.TheStockInOutKind=0 AND t1.StockInManner=10 AND (t2.ConcreteBalDtlID IS NULL OR t2.ConcreteBalDtlID='')
                AND t2.Quantity-t2.RefQuantity-t2.BalQuantity>0 and t1.CreateYear={0} AND t1.CreateMonth={1} and t1.projectId='{2}'";
            sql = string.Format(sql, accountYear, accountMonth, projectId);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            string tempStr = "以下收料单未做验收结算：";
            result = tempStr;
            while (dr.Read())
            {
                string code = dr.GetString(0);
                string special = dr.GetString(1);
                result += "\n" + code + "(" + special + ") ";
            }
            if (!(result.Length > tempStr.Length))
            {
                result = "";
            }
            return result;
        }
        #endregion

        public IList WZXH_Query(string sProjectID, int iYear, int iMonth)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("ProjectId", sProjectID));
            oQuery.AddCriterion(Expression.Eq("FiscalYear", iYear));
            oQuery.AddCriterion(Expression.Eq("FiscalMonth", iMonth));
            IList list = Dao.ObjectQuery(typeof(StockInOut), oQuery);
            return list;

        }
        /// <summary>
        /// 判断是否能结帐
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sSysCode"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        public bool IsAccount(string sProjectID, string sSysCode, int iYear, int iMonth)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("ProjectId", sProjectID));
            oQuery.AddCriterion(Expression.Eq("FiscalYear", iYear));
            oQuery.AddCriterion(Expression.Eq("FiscalMonth", iMonth));
            oQuery.AddCriterion(Expression.Sql(String.Format(" (instr('{0}',accounttasksyscode )>0 or instr(accounttasksyscode,'{0}' )>0 ) ", sSysCode)));
            IList list = Dao.ObjectQuery(typeof(StockInOut), oQuery);
            return (list == null || list.Count == 0) ? false : true;
        }
        /// <summary>
        /// 仓库收发存查询
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetStockInOut(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(StockInOut), oq);
        }

        /// <summary>
        /// 仓库收发存查询
        /// </summary>
        /// <param name="condition">本期条件</param>
        /// <param name="addCondition">累计条件</param>
        /// <returns></returns>
        public DataSet StockInOutQuery(string condition, string addCondition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"
                SELECT MatCode,MatName,MatSpec,UnitName,SUM(LastQuantity) LastQuantity,SUM(LastMoney) LastMoney,
                SUM(BuyInQuantity) BuyInQuantity,SUM(BuyInMoney) BuyInMoney,SUM(MoveInQuantity) MoveInQuantity,SUM(MoveInMoney) MoveInMoney,
                SUM(SaleOutQuantity) SaleOutQuantity,SUM(SaleOutMoney) SaleOutMoney,SUM(MoveOutQuantity) MoveOutQuantity,
                SUM(MoveOutMoney) MoveOutMoney,SUM(ProfitInQuantity) ProfitInQuantity,SUM(ProfitInMoney) ProfitInMoney,
                SUM(LossOutQuantity) LossOutQuantity,SUM(LossOutMoney) LossOutMoney,SUM(NowQuantity) NowQuantity,SUM(NowMoney) NowMoney,
                SUM(BuyInAddQuantity) BuyInAddQuantity,SUM(BuyInAddMoney) BuyInAddMoney,SUM(MoveInAddQuantity) MoveInAddQuantity,
                SUM(MoveInAddMoney) MoveInAddMoney,SUM(SaleOutAddQuantity) SaleOutAddQuantity,SUM(SaleOutAddMoney) SaleOutAddMoney,
                SUM(MoveOutAddQuantity) MoveOutAddQuantity,SUM(MoveOutAddMoney) MoveOutAddMoney,SUM(ProfitInAddQuantity) ProfitInAddQuantity,
                SUM(ProfitInAddMoney) ProfitInAddMoney,SUM(LossOutAddQuantity) LossOutAddQuantity,SUM(LossOutAddMoney) LossOutAddMoney
                FROM (
                SELECT t1.MatCode,t1.MatName,t1.MatSpec,t1.UnitName,sum(t1.LastQuantity) LastQuantity,sum(t1.LastMoney) LastMoney,
                sum(t1.BuyInQuantity-t1.BuyInRedQuantity) BuyInQuantity,sum(t1.BuyInMoney-t1.BuyInRedMoney) BuyInMoney,
                sum(t1.MoveInQuantity-t1.MoveInRedQuantity) MoveInQuantity,sum(t1.MoveInMoney-t1.MoveInRedMoney) MoveInMoney,
                sum(t1.SaleOutQuantity-t1.SaleOutRedQuantity) SaleOutQuantity,sum(t1.SaleOutMoney-t1.SaleOutRedMoney) SaleOutMoney,
                sum(t1.MoveOutQuantity-t1.MoveOutRedQuantity) MoveOutQuantity,sum(t1.MoveOutMoney-t1.MoveOutRedMoney) MoveOutMoney,
                sum(t1.ProfitInQuantity) ProfitInQuantity,sum(t1.ProfitInMoney) ProfitInMoney,sum(t1.LossOutQuantity) LossOutQuantity,
                sum(t1.LossOutMoney) LossOutMoney,sum(t1.NowQuantity) NowQuantity,sum(t1.NowMoney) NowMoney,
                0 BuyInAddQuantity,0 BuyInAddMoney,0 MoveInAddQuantity,0 MoveInAddMoney,0 SaleOutAddQuantity,0 SaleOutAddMoney,
                0 MoveOutAddQuantity,0 MoveOutAddMoney,0 ProfitInAddQuantity,0 ProfitInAddMoney,0 LossOutAddQuantity,0 LossOutAddMoney
                FROM thd_stkStockInOut t1 LEFT JOIN thd_stkStockInDtl t2 ON t1.StockInDtlId=t2.id
                LEFT JOIN thd_stkStockIn t3 ON t3.id=t2.ParentId
                WHERE t1.AcctType=1 ";
            sql = sql + condition;
            sql = sql + @"
                GROUP BY t1.MatCode,t1.MatName,t1.MatSpec,t1.UnitName
                UNION ALL
                SELECT t1.MatCode,t1.MatName,t1.MatSpec,t1.UnitName,0 LastQuantity,0 LastMoney,
                0 BuyInQuantity,0 BuyInMoney,0 MoveInQuantity,0 MoveInMoney,0 SaleOutQuantity,0 SaleOutMoney,
                0 MoveOutQuantity,0 MoveOutMoney,0 ProfitInQuantity,0 ProfitInMoney,0 LossOutQuantity,
                0 LossOutMoney,0 NowQuantity,0 NowMoney,
                sum(t1.BuyInQuantity-t1.BuyInRedQuantity) BuyInAddQuantity,sum(t1.BuyInMoney-t1.BuyInRedMoney) BuyInAddMoney,
                sum(t1.MoveInQuantity-t1.MoveInRedQuantity) MoveInAddQuantity,sum(t1.MoveInMoney-t1.MoveInRedMoney) MoveInAddMoney,
                sum(t1.SaleOutQuantity-t1.SaleOutRedQuantity) SaleOutAddQuantity,sum(t1.SaleOutMoney-t1.SaleOutRedMoney) SaleOutAddMoney,
                sum(t1.MoveOutQuantity-t1.MoveOutRedQuantity) MoveOutAddQuantity,sum(t1.MoveOutMoney-t1.MoveOutRedMoney) MoveOutAddMoney,
                sum(t1.ProfitInQuantity) ProfitInAddQuantity,sum(t1.ProfitInMoney) ProfitInAddMoney,sum(t1.LossOutQuantity) LossOutAddQuantity,
                sum(t1.LossOutMoney) LossOutAddMoney
                FROM thd_stkStockInOut t1 LEFT JOIN thd_stkStockInDtl t2 ON t1.StockInDtlId=t2.id
                LEFT JOIN thd_stkStockIn t3 ON t3.id=t2.ParentId
                WHERE t1.AcctType=1 AND t1.TheMaterial IS NOT NULL ";
            sql = sql + addCondition;
            sql += @"
                GROUP BY t1.MatCode,t1.MatName,t1.MatSpec,t1.UnitName
                ) t
                GROUP BY MatCode,MatName,MatSpec,UnitName";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        #region 物资报表
        private FiscalPeriodDetail GetFiscalPeriod(string date)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Le("BeginDate", DateTime.Parse(date)));
            oq.AddCriterion(Expression.Ge("EndDate", DateTime.Parse(date)));

            IList lst = Dao.ObjectQuery(typeof(FiscalPeriodDetail), oq);
            if (lst != null && lst.Count > 0)
            {
                return lst[0] as FiscalPeriodDetail;
            }
            return null;
        }
        /// <summary>
        /// 材料收发存月报表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectId">项目ID</param>
        /// <param name="wzfl">物资分类</param>
        /// <returns></returns>
        public DataSet WZBB_clsfcybb(string beginDate, string endDate, string projectId, IList lstMaterialCategory)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            if (fiscalPeorid == null)
            {
                return null;
            }
            int lastYear = 0, lastMonth = 0;
            //上个会计期
            if (fiscalPeorid.FiscalMonth == 12)
            {
                lastYear = fiscalPeorid.FiscalYear - 1;
                lastMonth = 1;
            }
            else
            {
                lastYear = fiscalPeorid.FiscalYear;
                lastMonth = fiscalPeorid.FiscalMonth - 1;
            }
            string sExpCode = GetMaterialCatCodes(lstMaterialCategory);


            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            //string qcCon = " and a.fiscalYear=" + lastYear + " and fiscalmonth=" + lastMonth + " and a.projectid='" + projectId + "' and c.code like '" + wzfl+"%'";
            string qcCon = " and a.projectid='" + projectId + "' " + sExpCode;
            //string qcCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl + "%' and d.createdate>=to_date('" + fiscalPeorid.BeginDate.ToShortDateString() + "','yyyy-mm-dd') and d.createdate<to_date('" + beginDate + "','yyyy-mm-dd')";
            string qcCon_bq = " and d.projectid='" + projectId + "' " + sExpCode + " and d.createdate<to_date('" + beginDate + "','yyyy-mm-dd')";
            string bqCon_bq = " and d.projectid='" + projectId + "'" + sExpCode + " and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            string bqCon_lj = " and d.projectid='" + projectId + "'" + sExpCode + " and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            //以前
            ////string qcCon = " and a.fiscalYear=" + lastYear + " and fiscalmonth=" + lastMonth + " and a.projectid='" + projectId + "' and c.code like '" + wzfl+"%'";
            //string qcCon = " and a.projectid='" + projectId + "' and c.code like '" + wzfl + "%' ";
            ////string qcCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl + "%' and d.createdate>=to_date('" + fiscalPeorid.BeginDate.ToShortDateString() + "','yyyy-mm-dd') and d.createdate<to_date('" + beginDate + "','yyyy-mm-dd')";
            //string qcCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl + "%' and d.createdate<to_date('" + beginDate + "','yyyy-mm-dd')";
            //string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl + "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            //string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + wzfl + "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";



            string sql = @"select materialid||unitname materialid,matcode,matname,matspec,unitname,sum(lastQty) lastQty,sum(lastMoney) lastMoney,
                sum(cgsl) cgsl,sum(cgje) cgje,sum(ljcgsl) ljcgsl,sum(ljcgje) ljcgje,sum(dbrksl) dbrksl,sum(dbrkje) dbrkje,
                sum(ljdbrksl) ljdbrksl,sum(ljdbrkje) ljdbrkje,sum(pysl) pysl,sum(pyje) pyje,sum(ljpysl) ljpysl,sum(ljpyje) ljpyje,
                sum(xhsl) xhsl,sum(xhje) xhje,sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje,sum(dbcksl) dbcksl,sum(dbckje) dbckje,
                sum(ljdbcksl) ljdbcksl,sum(ljdbckje) ljdbckje,sum(pksl) pksl,sum(pkje) pkje,sum(ljpksl) ljpksl,sum(ljpkje) ljpkje,
                sum(lastQty)+sum(cgsl)+sum(dbrksl)-sum(xhsl)-sum(dbcksl) jcsl,
                (case when -0.05<= (sum(lastMoney)+sum(cgje)+sum(dbrkje)-sum(ckje)) 
                 and (sum(lastMoney)+sum(cgje)+sum(dbrkje)-sum(ckje)) <=0.05  then 0 else (sum(lastMoney)+sum(cgje)+sum(dbrkje)-sum(ckje)) end) jcje
                from (
                select materialid,matcode,matname,matspec,unitname,sum(nowquantity)-sum(outqty) lastQty,sum(nowmoney)-sum(outmoney) lastMoney, 
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,0 ckje
                from (
                --期初
                select b.materialid,b.matcode,a.matname,a.matspec,a.unitname,a.nowquantity,a.nowmoney,0 outqty,0 outmoney
                from thd_stkstockinout a ,resmaterial b,resmaterialcategory c
                where   1<>1 
                union all
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,a.quantity,a.money,0 outqty,0 outmoney
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockinmanner in (10,11,12) " + qcCon_bq + @"                
                union all
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 inqty,0 inmoney,a.quantity,a.money
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (20,21,22)" + qcCon_bq + @"
                ) group by materialid,matcode,matname,matspec,unitname
                union all
                --本期收入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                a.quantity cgsl,a.money cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrkjsl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,0 ckje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id  and d.special='土建' 
                and d.istally=1 and d.stockinmanner in (10) " + bqCon_bq + @"
                union all
                --本期累计收入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                0 cgsl,0 cgje,a.quantity ljcgsl,a.money ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,0 ckje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockinmanner in (10) " + bqCon_lj + @"
                union all
                --本期调入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,a.quantity dbrksl,a.money dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,0 ckje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockinmanner in (11) " + bqCon_bq + @"
                union all
                --本期累计调入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,a.quantity ljdbrksl,a.money ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,0 ckje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockinmanner in (11) " + bqCon_lj + @"
                union all
                --本期消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,a.money ckje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (20) " + bqCon_bq + @"
                union all
                --本期累计消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,0 ckje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (20) " + bqCon_lj + @"
                union all
                --本期调出数量、金额、盈亏金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,a.quantity dbcksl,a.moveMoney dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,
                 (nvl(a.movemoney,0)-(round(a.quantity*(select sum(( case when a.quantity>=0 then t1.quantity else -t1.quantity end)*
                  nvl( (select avg(round((k1.money+k1.costmoney)/k1.quantity,4)) from thd_stockinbaldetail k1 where k1.refquantity != k1.quantity and 
                    t1.stockindtlid=k1.forwarddetailid),t1.price) ) /sum(case when a.quantity>=0 then t1.quantity else -t1.quantity end)
                   from thd_stkstockoutdtlseq t1 where t1.stockoutdtlid=
                  (case when a.quantity>=0 then a.id else a.forwarddetailid end) ),2)))  pkje,
                0 ljpksl,0 ljpkje,a.money ckje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (21) " + bqCon_bq + @"
                union all
                --累计调出数量、金额、盈亏金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,a.quantity ljdbcksl,a.moveMoney ljdbckje,0 pksl,0 pkje,0 ljpksl,

                (nvl(a.movemoney,0)-(round(a.quantity*(select sum(( case when a.quantity>=0 then t1.quantity else -t1.quantity end)*
                  nvl( (select avg(round((k1.money+k1.costmoney)/k1.quantity,4)) from thd_stockinbaldetail k1 where k1.refquantity != k1.quantity and 
                    t1.stockindtlid=k1.forwarddetailid),t1.price) ) /sum(case when a.quantity>=0 then t1.quantity else -t1.quantity end)
                   from thd_stkstockoutdtlseq t1 where t1.stockoutdtlid=
                  (case when a.quantity>=0 then a.id else a.forwarddetailid end) ),2))) ljpkje,0 ckje

                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (21) " + bqCon_lj + @"
                ) group by materialid,matcode,matname,matspec,unitname order by matname,matcode";

            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        public DataSet WZBB_skmwztj(string beginDate, string endDate, string sProjectId, string sCostSysCode, string sGWBSSysCode)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sSQL = @"select t3.id, nvl(t1.materialname,'') materialname,nvl(t1.materialspec,'')materialspec,nvl(t1.matstandardunitname,'')matstandardunitname,sum(t1.quantity) qty,sum(t1.money)money
from thd_stkstockout t 
join thd_stkstockoutdtl t1 on t.id=t1.parentid and t1.subjectguid is not null
join resmaterial t2 on t2.materialid=t1.material
join (select tt.id,tt.syscode from resmaterialcategory tt where tt.numlevel=3 ) t3
on instr(t2.thesyscode,t3.syscode)>0
join THD_GWBSTree t4 on t1.usedpart=t4.id
where t.projectid='{0}' and instr( t1.subjectsyscode,'{1}')>0 and  instr(t4.syscode,'{2}')>0 
 and t.createdate<=to_date('{3}','YYYY-MM-DD')+1 and  t.createdate>=to_date('{4}','YYYY-MM-DD') 
group by t3.id, t1.materialname ,t1.materialspec,t1.matstandardunitname";
            sSQL = string.Format(sSQL, sProjectId, sCostSysCode, sGWBSSysCode, endDate, beginDate);
            command.CommandText = sSQL;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }


        public string GetMaterialCatCodes(IList lstMaterialCategory)
        {
            string sExpCode = string.Empty;
            foreach (MaterialCategory oMaterialCategory in lstMaterialCategory)
            {
                if (string.IsNullOrEmpty(sExpCode))
                {
                    sExpCode = "^" + oMaterialCategory.Code;
                }
                else
                {
                    sExpCode += "|^" + oMaterialCategory.Code;
                }
            }
            if (!string.IsNullOrEmpty(sExpCode))
            {
                sExpCode = string.Format(" and  regexp_like(c.code,'{0}')  ", sExpCode);
            }
            return sExpCode;
        }

        public string GetUsePartCodes(IList lstUsePart)
        {
            string sExpCode = string.Empty;
            sExpCode = " '0' ";
            foreach (GWBSTree oGWBSTree in lstUsePart)
            {
               
                 
                    sExpCode += ",'" + oGWBSTree.Id + "'";
                 
            }
            if (!string.IsNullOrEmpty(sExpCode))
            {
                sExpCode = string.Format(" id in ({0})    ", sExpCode);
            }
            return sExpCode;
        }
        /// <summary>
        /// 材料收发存月报表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectId">项目ID</param>
        /// <param name="wzfl">物资分类</param>
        /// <returns></returns>
        public DataSet WZBB_clsfcybb(string beginDate, string endDate, string projectId, string wzfl)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            if (fiscalPeorid == null)
            {
                return null;
            }
            int lastYear = 0, lastMonth = 0;
            //上个会计期
            if (fiscalPeorid.FiscalMonth == 12)
            {
                lastYear = fiscalPeorid.FiscalYear - 1;
                lastMonth = 1;
            }
            else
            {
                lastYear = fiscalPeorid.FiscalYear;
                lastMonth = fiscalPeorid.FiscalMonth - 1;
            }

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            //string qcCon = " and a.fiscalYear=" + lastYear + " and fiscalmonth=" + lastMonth + " and a.projectid='" + projectId + "' and c.code like '" + wzfl+"%'";
            string qcCon = " and a.projectid='" + projectId + "' and c.code like '" + wzfl + "%' ";
            //string qcCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl + "%' and d.createdate>=to_date('" + fiscalPeorid.BeginDate.ToShortDateString() + "','yyyy-mm-dd') and d.createdate<to_date('" + beginDate + "','yyyy-mm-dd')";
            string qcCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl + "%' and d.createdate<to_date('" + beginDate + "','yyyy-mm-dd')";
            string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl + "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + wzfl + "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";

            string sql = @"select materialid,matcode,matname,matspec,unitname,sum(lastQty) lastQty,sum(lastMoney) lastMoney,
                sum(cgsl) cgsl,sum(cgje) cgje,sum(ljcgsl) ljcgsl,sum(ljcgje) ljcgje,sum(dbrksl) dbrksl,sum(dbrkje) dbrkje,
                sum(ljdbrksl) ljdbrksl,sum(ljdbrkje) ljdbrkje,sum(pysl) pysl,sum(pyje) pyje,sum(ljpysl) ljpysl,sum(ljpyje) ljpyje,
                sum(xhsl) xhsl,sum(xhje) xhje,sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje,sum(dbcksl) dbcksl,sum(dbckje) dbckje,
                sum(ljdbcksl) ljdbcksl,sum(ljdbckje) ljdbckje,sum(pksl) pksl,sum(pkje) pkje,sum(ljpksl) ljpksl,sum(ljpkje) ljpkje,
                sum(lastQty)+sum(cgsl)+sum(dbrksl)-sum(xhsl)-sum(dbcksl) jcsl,
                (case when -0.05=<sum(lastMoney)+sum(cgje)+sum(dbrkje)-sum(xhje)-sum(dbckje)+sum(pkje) <=0.05 then 0 else sum(lastMoney)+sum(cgje)+sum(dbrkje)-sum(xhje)-sum(dbckje)+sum(pkje)) jcje
                from (
                select materialid,matcode,matname,matspec,unitname,sum(nowquantity)-sum(outqty) lastQty,sum(nowmoney)-sum(outmoney) lastMoney, 
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from (
                --期初
                select b.materialid,b.matcode,a.matname,a.matspec,a.unitname,a.nowquantity,a.nowmoney,0 outqty,0 outmoney
                from thd_stkstockinout a ,resmaterial b,resmaterialcategory c
                where   1<>1 
                union all
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,a.quantity,a.money,0 outqty,0 outmoney
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockinmanner in (10,11,12) " + qcCon_bq + @"                
                union all
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 inqty,0 inmoney,a.quantity,a.money
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (20,21,22)" + qcCon_bq + @"
                ) group by materialid,matcode,matname,matspec,unitname
                union all
                --本期收入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                a.quantity cgsl,a.money cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrkjsl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id  and d.special='土建' 
                and d.istally=1 and d.stockinmanner in (10) " + bqCon_bq + @"
                union all
                --本期累计收入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                0 cgsl,0 cgje,a.quantity ljcgsl,a.money ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockinmanner in (10) " + bqCon_lj + @"
                union all
                --本期调入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,a.quantity dbrksl,a.money dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockinmanner in (11) " + bqCon_bq + @"
                union all
                --本期累计调入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,a.quantity ljdbrksl,a.money ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockinmanner in (11) " + bqCon_lj + @"
                union all
                --本期消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (20) " + bqCon_bq + @"
                union all
                --本期累计消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (20) " + bqCon_lj + @"
                union all
                --本期调出数量、金额、盈亏金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,a.quantity dbcksl,a.moveMoney dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,
                (nvl(a.movemoney,0)-(round(a.quantity*(select sum(( case when a.quantity>=0 then t1.quantity else -t1.quantity end)*
                  nvl( (select avg(round((k1.money+k1.costmoney)/k1.quantity,4)) from thd_stockinbaldetail k1 where k1.refquantity != k1.quantity and 
                    t1.stockindtlid=k1.forwarddetailid),t1.price) ) /sum(case when a.quantity>=0 then t1.quantity else -t1.quantity end)
                   from thd_stkstockoutdtlseq t1 where t1.stockoutdtlid=
                  (case when a.quantity>=0 then a.id else a.forwarddetailid end) ),2))) pkje,
                0 ljpksl,0 ljpkje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (21) " + bqCon_bq + @"
                union all
                --累计调出数量、金额、盈亏金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,a.quantity ljdbcksl,a.moveMoney ljdbckje,0 pksl,0 pkje,0 ljpksl,
                (nvl(a.movemoney,0)-(round(a.quantity*(select sum(( case when a.quantity>=0 then t1.quantity else -t1.quantity end)*
                  nvl( (select avg(round((k1.money+k1.costmoney)/k1.quantity,4)) from thd_stockinbaldetail k1 where k1.refquantity != k1.quantity and 
                    t1.stockindtlid=k1.forwarddetailid),t1.price) ) /sum(case when a.quantity>=0 then t1.quantity else -t1.quantity end)
                   from thd_stkstockoutdtlseq t1 where t1.stockoutdtlid=
                  (case when a.quantity>=0 then a.id else a.forwarddetailid end) ),2))) ljpkje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (21) " + bqCon_lj + @"
                ) group by materialid,matcode,matname,matspec,unitname order by matname,matcode";

            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        /// <summary>
        /// 材料收发存月报表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="_GWBSTreeSysCode">使用部位层次码</param>
        /// <param name="wzfl">物资分类</param>
        /// <returns></returns>
        public DataSet WZBB_clsfcybb_xhmx(string beginDate, string endDate, string projectId, string _GWBSTreeSysCode, IList lstMaterialCategory)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sExpCodes = string.Empty;
            sExpCodes = GetMaterialCatCodes(lstMaterialCategory);
            string bqCon_bq = " and d.projectid='" + projectId + "'  " + sExpCodes +
               " and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
            string bqCon_lj = " and d.projectid='" + projectId + "' " + sExpCodes +
                "  and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
            //string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl +
            //    "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
            //string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + wzfl +
            //    "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
            string sql = @"
                select materialid,matcode,materialname,materialspec,matstandardunitname,sum(xhsl) xhsl,sum(xhje) xhje,
                sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje from(
                --本期消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.special='土建'  and d.stockoutmanner in (20) " + bqCon_bq + @"
                union all
                --累计消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.special='土建'  and d.stockoutmanner in (20) " + bqCon_lj + @"
                ) group by materialid,matcode,materialname,materialspec,matstandardunitname order by materialname,matcode
                ";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        public DataSet WZBB_clsfcybb_xhmx(string beginDate, string endDate, string projectId,  IList lstMaterialCategory,IList lstUsePart)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sExpCodes = string.Empty;
            string sUsePartSysCode = string.Empty;
            sExpCodes = GetMaterialCatCodes(lstMaterialCategory);
            sUsePartSysCode = GetUsePartCodes(lstUsePart);
            string bqCon_bq = " and d.projectid='" + projectId + "'  " + sExpCodes +
              " and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1  "  ;
            string bqCon_lj = " and d.projectid='" + projectId + "' " + sExpCodes +
                "  and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1  ";
            //string bqCon_bq = " and d.projectid='" + projectId + "'  " + sExpCodes +
            //   " and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1  " + sUsePartSysCode;
            //string bqCon_lj = " and d.projectid='" + projectId + "' " + sExpCodes +
            //    "  and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" +sUsePartSysCode;
            //string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl +
            //    "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
            //string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + wzfl +
            //    "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
//            string sql = @"
//                select materialid,matcode,materialname,materialspec,matstandardunitname,sum(xhsl) xhsl,sum(xhje) xhje,
//                sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje from(
//                --本期消耗数量、金额
//                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
//                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
//                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
//                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
//                left join thd_gwbstree e on a.usedpart=e.id 
//                where d.istally=1 and d.special='土建'  and d.stockoutmanner in (20) " + bqCon_bq + @"
//                union all
//                --累计消耗数量、金额
//                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
//                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
//                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
//                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
//                left join thd_gwbstree e on a.usedpart=e.id 
//                where d.istally=1 and d.special='土建'  and d.stockoutmanner in (20) " + bqCon_lj + @"
//                ) group by materialid,matcode,materialname,materialspec,matstandardunitname order by materialname,matcode
//                ";
            string sql = @"select materialid||matstandardunitname materialid,matcode,materialname,materialspec,matstandardunitname,sum(xhsl) quantity,sum(xhje) money,
                nvl(t2.usepartID,'')HeadID,nvl(t2.userpartname,'')userpartname ,
                sum(ljxhsl) lstquantity,sum(ljxhje) lstmoney from(
                --本期消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname, nvl(e.syscode,'') usePartsyscode,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on  a.usedpart=e.id 
                where d.istally=1 and d.special='土建'  and d.stockoutmanner in (20) " + bqCon_bq + @"
                union all
                --累计消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname, nvl(e.syscode,'') usePartsyscode,
                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.special='土建'  and d.stockoutmanner in (20) " + bqCon_lj + @"
                )t1
                left join (
                select t.id usepartID,t.syscode usePartSysCode,t.name userpartname  from  thd_gwbstree t   
                where  "+sUsePartSysCode+@" )t2 on instr(t1.usePartsyscode,t2.usePartSysCode)>0
                 group by materialid,matcode,materialname,materialspec,matstandardunitname,
                 nvl(t2.usepartID,'') ,nvl(t2.userpartname,'') order by materialname,matcode";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        public DataSet WZBB_clsfcybb_xhmx(string beginDate, string endDate, string projectId, IList lstMaterialCategory)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sExpCodes = string.Empty;
            sExpCodes = GetMaterialCatCodes(lstMaterialCategory);
            string bqCon_bq = " and d.projectid='" + projectId + "'  " + sExpCodes +
               " and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and   a.usedpart is null   ";
            string bqCon_lj = " and d.projectid='" + projectId + "' " + sExpCodes +
                "  and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and   a.usedpart is null    ";
            //string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl +
            //    "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
            //string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + wzfl +
            //    "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
            string sql = @"
                select materialid,matcode,materialname,materialspec,matstandardunitname,sum(xhsl) xhsl,sum(xhje) xhje,
                sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje from(
                --本期消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.special='土建'  and d.stockoutmanner in (20) " + bqCon_bq + @"
                union all
                --累计消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.special='土建'  and d.stockoutmanner in (20) " + bqCon_lj + @"
                ) group by materialid,matcode,materialname,materialspec,matstandardunitname order by materialname,matcode
                ";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        public DataSet WZBB_GetCostAccountSubjectSum(string beginDate, string endDate, string projectId, IList lstMaterialCategory)
        {
            string sExpCode = GetMaterialCatCodes(lstMaterialCategory);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string bqCon_bq = " and d.projectid='" + projectId + "'" + sExpCode + " and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            string sql = @"  select b.materialid||a.matstandardunitname materialid ,nvl(subjectguid,'未定核算科目') subjectguid,nvl(sum(a.quantity),0) sumQuantity,nvl(sum(a.money),0) sumMoney 
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b ,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (20)   {0}   group by b.materialid||a.matstandardunitname ,a.subjectguid";
            sql = string.Format(sql, bqCon_bq);
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        public DataSet WZBB_GetCostAccountSubjectSum(string beginDate, string endDate, string projectId, IList lstMaterialCategory,IList lstUsepart)
        {
            string sExpCode = GetMaterialCatCodes(lstMaterialCategory);


            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string bqCon_bq = " and d.projectid='" + projectId + "'" + sExpCode + " and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            string sql = @"  select b.materialid ,nvl(subjectguid,'未定核算科目') subjectguid,nvl(sum(a.quantity),0) Quantity,nvl(sum(a.money),0) Money 
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b ,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (20)   {0}   group by b.materialid ,a.subjectguid";
            sql = string.Format(sql, bqCon_bq);
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        public DataSet WZBB_GetCostAccountSubjectCat(string beginDate, string endDate, string projectId, IList lstMaterialCategory)
        {
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sExpCodes = string.Empty;
            sExpCodes = GetMaterialCatCodes(lstMaterialCategory);
            string bqCon_bq = " and d.projectid='" + projectId + "'  " + sExpCodes +
              " and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1  ";
            //string bqCon_lj = " and d.projectid='" + projectId + "' " + sExpCodes +
            //  "  and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1  ";
            //            string sql = @"  select distinct nvl(a.subjectname,'') Name, nvl(t1.name,'') ParentName,nvl(a.subjectguid,'') ID,nvl(t1.id,'') ParentID  from thd_stkstockoutdtl a
            //                             join thd_stkstockout d on a.parentid=d.id  and d.istally=1 and d.stockoutmanner in (20) 
            //                            join (select  syscode, name,id  from thd_costaccountsubject t where t.tlevel=2) t1 on  
            //                            t1.syscode=substr(a.subjectsyscode,1,instr(a.subjectsyscode,'.',1,2))
            //                             join resmaterial b on a.material=b.materialid 
            //                             join resmaterialcategory c on b.materialcategoryid=c.id
            //                            where a.subjectguid is not null   {0}  order by nvl(t1.name,'')";

            string sql = @" select  distinct nvl(a.subjectname,'') Name, nvl(e.name,'') ParentName,nvl(a.subjectguid,'') ID,nvl(e.id,'') ParentID
               from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c ,
               (select  syscode, name,id  from thd_costaccountsubject t where t.tlevel=2) e
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建' 
                and d.istally=1 and d.stockoutmanner in (20) and a.subjectguid is not null
                and  e.syscode=substr(a.subjectsyscode,1,instr(a.subjectsyscode,'.',1,2))  {0}  order by nvl(e.name,'')";
            sql = string.Format(sql, bqCon_bq);
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        /// <summary>
        /// 材料收发存月报表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="_GWBSTreeSysCode">使用部位层次码</param>
        /// <param name="wzfl">物资分类</param>
        /// <returns></returns>
        public DataSet WZBB_clsfcybb_xhmx(string beginDate, string endDate, string projectId, string _GWBSTreeSysCode, string wzfl)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + wzfl +
                "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
            string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + wzfl +
                "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
            string sql = @"
                select materialid,matcode,materialname,materialspec,matstandardunitname,sum(xhsl) xhsl,sum(xhje) xhje,
                sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje from(
                --本期消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.stockoutmanner in (20) " + bqCon_bq + @"
                union all
                --累计消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.stockoutmanner in (20) " + bqCon_lj + @"
                ) group by materialid,matcode,materialname,materialspec,matstandardunitname order by materialname,matcode
                ";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        /// <summary>
        /// 商品砼收发存月报表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectId">项目ID</param>
        /// <param name="categoryCode"></param>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public DataSet WZBB_sptsfcybb(string beginDate, string endDate, string projectId, string categoryCode, SupplierRelationInfo supplier)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            int lastYear = 0, lastMonth = 0;
            //上个会计期
            if (fiscalPeorid.FiscalMonth == 12)
            {
                lastYear = fiscalPeorid.FiscalYear - 1;
                lastMonth = 1;
            }
            else
            {
                lastYear = fiscalPeorid.FiscalYear;
                lastMonth = fiscalPeorid.FiscalMonth - 1;
            }

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + categoryCode + "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + categoryCode + "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";

            if (supplier != null)
            {
                bqCon_bq += " and d.supplierrelation='" + supplier.Id + "'";
                bqCon_lj += " and d.supplierrelation='" + supplier.Id + "'";
            }

            string sql = @"select materialid||nvl(matstandardunitname,'') materialid,matcode,materialname,materialspec,matstandardunitname,
                sum(cgsl) cgsl,sum(cgje) cgje,sum(ljcgsl) ljcgsl,sum(ljcgje) ljcgje,sum(dbrksl) dbrksl,sum(dbrkje) dbrkje,
                sum(ljdbrksl) ljdbrksl,sum(ljdbrkje) ljdbrkje,sum(pysl) pysl,sum(pyje) pyje,sum(ljpysl) ljpysl,sum(ljpyje) ljpyje,
                sum(xhsl) xhsl,sum(xhje) xhje,sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje,sum(dbcksl) dbcksl,sum(dbckje) dbckje,
                sum(ljdbcksl) ljdbcksl,sum(ljdbckje) ljdbckje,sum(pksl) pksl,sum(pkje) pkje,sum(ljpksl) ljpksl,sum(ljpkje) ljpkje
                from (
                --本期收入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                a.quantity cgsl,a.money cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id  and d.special='土建'
                and d.istally=1 and d.stockinmanner in (10) " + bqCon_bq + @"
                union all
                --本期累计收入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                0 cgsl,0 cgje,a.quantity ljcgsl,a.money ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockinmanner in (10) " + bqCon_lj + @"
                union all
                --本期调入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,a.quantity dbrksl,a.money dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockinmanner in (11) " + bqCon_bq + @"
                union all
                --本期累计调入数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastMoney,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,a.quantity ljdbrksl,a.money ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockinmanner in (11) " + bqCon_lj + @"
                union all
                --本期消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockoutmanner in (20) " + bqCon_bq + @"
                union all
                --本期累计消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockoutmanner in (20) " + bqCon_lj + @"
                union all
                --本期调出数量、金额、盈亏金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,a.quantity dbcksl,a.moveMoney dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,a.moveMoney-a.money pkje,0 ljpksl,0 ljpkje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id  and d.special='土建'
                and d.istally=1 and d.stockoutmanner in (21) " + bqCon_bq + @"
                union all
                --累计调出数量、金额、盈亏金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,a.quantity ljdbcksl,a.moveMoney ljdbckje,0 pksl,0 pkje,0 ljpksl,a.moveMoney-a.money ljpkje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockoutmanner in (21) " + bqCon_lj + @"
                ) group by materialid||nvl(matstandardunitname,''),matcode,materialname,materialspec,matstandardunitname order by materialname,matcode";

            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        /// <summary>
        /// 商品砼收发存月报表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="_GWBSTreeSysCode">使用部位层次码</param>
        /// <param name="categoryCode"></param>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public DataSet WZBB_sptsfcybb_xhmx(string beginDate, string endDate, string projectId, string _GWBSTreeSysCode, string categoryCode, SupplierRelationInfo supplier)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + categoryCode +
                "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 " +
                " and e.syscode like '" + _GWBSTreeSysCode + "%'";

            string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + categoryCode +
                "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";

            //供应商查询条件
            if (supplier != null)
            {
                bqCon_bq += " and d.supplierrelation='" + supplier.Id + "'";
                bqCon_lj += " and d.supplierrelation='" + supplier.Id + "'";
            }

            string sql = @"
                select materialid,matcode,materialname,materialspec,matstandardunitname,sum(xhsl) xhsl,sum(xhje) xhje,
                sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje from(
                --本期消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.stockoutmanner in (20) " + bqCon_bq + @"
                union all
                --累计消耗数量、金额
                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.stockoutmanner in (20) " + bqCon_lj + @"
                ) group by materialid,matcode,materialname,materialspec,matstandardunitname order by materialname,matcode
                ";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="categoryCode"></param>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public DataSet WZBB_sptsfcybb_xhmx(string beginDate, string endDate, string projectId, string categoryCode, SupplierRelationInfo supplier)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + categoryCode +
                "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 ";

            string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + categoryCode +
                "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1  ";

            //供应商查询条件
            if (supplier != null)
            {
                bqCon_bq += " and d.supplierrelation='" + supplier.Id + "'";
                bqCon_lj += " and d.supplierrelation='" + supplier.Id + "'";
            }

            //            string sql = @"
            //                select materialid,matcode,materialname,materialspec,matstandardunitname,sum(xhsl) xhsl,sum(xhje) xhje,
            //                sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje from(
            //                --本期消耗数量、金额
            //                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
            //                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
            //                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
            //                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
            //                left join thd_gwbstree e on a.usedpart=e.id 
            //                where d.istally=1 and d.stockoutmanner in (20) " + bqCon_bq + @"
            //                union all
            //                --累计消耗数量、金额
            //                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
            //                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
            //                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
            //                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
            //                left join thd_gwbstree e on a.usedpart=e.id 
            //                where d.istally=1 and d.stockoutmanner in (20) " + bqCon_lj + @"
            //                ) group by materialid,matcode,materialname,materialspec,matstandardunitname order by materialname,matcode
            //                ";
            string sql = @"     select materialid, id ,sum(xhsl) sumQuantity,sum(xhje) sumMoney,
                sum(ljxhsl) sumLstQuantity,sum(ljxhje) sumLstMoney from(
                --本期消耗数量、金额
                select b.materialid, a.usedpart id,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.stockoutmanner in (20)   " + bqCon_bq + @"
                union all
                --累计消耗数量、金额
                select b.materialid, a.usedpart id,
                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.stockoutmanner in (20)  " + bqCon_lj + @"  
                ) group by materialid, id ";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        public DataSet WZBB_sptsfcybb_xhmx(string beginDate, string endDate, string projectId, string categoryCode, SupplierRelationInfo supplier,IList lstUsePart)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sWhere = GetUsePartCodes(lstUsePart);
            string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + categoryCode +
                "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 ";

            string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + categoryCode +
                "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1  ";

            //供应商查询条件
            if (supplier != null)
            {
                bqCon_bq += " and d.supplierrelation='" + supplier.Id + "'";
                bqCon_lj += " and d.supplierrelation='" + supplier.Id + "'";
            }
//            string sql = @"     select materialid, id ,sum(xhsl) sumQuantity,sum(xhje) sumMoney,
//                sum(ljxhsl) sumLstQuantity,sum(ljxhje) sumLstMoney from(
//                --本期消耗数量、金额
//                select b.materialid, a.usedpart id,
//                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
//                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
//                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
//                left join thd_gwbstree e on a.usedpart=e.id 
//                where d.istally=1 and d.stockoutmanner in (20)   " + bqCon_bq + @"
//                union all
//                --累计消耗数量、金额
//                select b.materialid, a.usedpart id,
//                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
//                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
//                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
//                left join thd_gwbstree e on a.usedpart=e.id 
//                where d.istally=1 and d.stockoutmanner in (20)  " + bqCon_lj + @"  
//                ) group by materialid, id ";
            string sql = @" select t.materialid , nvl(t1.id,'')headid ,sum(t.xhsl) Quantity,sum(t.xhje) Money,
                sum(t.ljxhsl) LstQuantity,sum(t.ljxhje) LstMoney from(
                --本期消耗数量、金额
                select b.materialid||nvl(a.matstandardunitname,'') materialid, e.syscode usePartSyscode,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.stockoutmanner in (20)    " + bqCon_bq + @"
                union all
                --累计消耗数量、金额
                select b.materialid||nvl(a.matstandardunitname,'') materialid, e.syscode usePartSyscode,
                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.stockoutmanner in (20)  " + bqCon_lj + @"
                )t
               left join ( select t.syscode,t.id from thd_gwbstree t where " + sWhere + @"  )t1 
               on instr(t.usePartSyscode,t1.syscode)>0
                group by t.materialid, nvl(t1.id,'')";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        /// <summary>
        /// 商品砼收发存月报表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="_GWBSTreeSysCode">使用部位层次码</param>
        /// <param name="categoryCode"></param>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public DataSet WZBB_sptsfcybb_xhmx_UserPart(string beginDate, string endDate, string projectId, string categoryCode, SupplierRelationInfo supplier)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string bqCon_bq = " and d.projectid='" + projectId + "' and c.code like '" + categoryCode +
                "%' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 ";

            string bqCon_lj = " and d.projectid='" + projectId + "' and c.code like '" + categoryCode +
                "%' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 ";

            //供应商查询条件
            if (supplier != null)
            {
                bqCon_bq += " and d.supplierrelation='" + supplier.Id + "'";
                bqCon_lj += " and d.supplierrelation='" + supplier.Id + "'";
            }
            string sql = @"
                 select distinct t.usedpart id, t.usedpartname name ,t1.id parentid,t1.name parentname from
                   ( --本期消耗数量、金额
                    select  a.usedpart,a.usedpartname,e.syscode
                    from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                    join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                    left join thd_gwbstree e on a.usedpart=e.id 
                    where d.istally=1 and d.stockoutmanner in (20)   " + bqCon_bq + @" 
                    union all
                    --累计消耗数量、金额
                    select  a.usedpart,a.usedpartname,e.syscode
                    from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                    join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                    left join thd_gwbstree e on a.usedpart=e.id 
                    where d.istally=1 and d.stockoutmanner in (20)   " + bqCon_lj + @"   
                    ) t
                    join (select t.id,t.name,t.syscode from thd_gwbstree t where t.warehouseflag=1 ) t1 
                    on instr(t.syscode,t1.syscode)>0  order by t1.id  ,t1.name  ";
            //            string sql = @"
            //                select materialid,matcode,materialname,materialspec,matstandardunitname,sum(xhsl) xhsl,sum(xhje) xhje,
            //                sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje from(
            //                --本期消耗数量、金额
            //                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
            //                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
            //                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
            //                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
            //                left join thd_gwbstree e on a.usedpart=e.id 
            //                where d.istally=1 and d.stockoutmanner in (20) " + bqCon_bq + @"
            //                union all
            //                --累计消耗数量、金额
            //                select b.materialid,b.matcode,a.materialname,a.materialspec,a.matstandardunitname,
            //                0 xhsl,0 xhje,a.quantity ljxhsl,a.money ljxhje
            //                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
            //                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
            //                left join thd_gwbstree e on a.usedpart=e.id 
            //                where d.istally=1 and d.stockoutmanner in (20) " + bqCon_lj + @"
            //                ) group by materialid,matcode,materialname,materialspec,matstandardunitname order by materialname,matcode
            //                ";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        /// <summary>
        /// 材料收发存汇总表
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="isSummary">是否汇总表 true 汇总表</param>
        /// <returns></returns>
        public DataSet WZBB_clsfchzb(string beginDate, string endDate, string projectId, bool isSummary)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            int lastYear = 0, lastMonth = 0;
            if (fiscalPeorid == null)
            {
                throw new Exception("开始时间的会计期间未定义！");
            }
            //上个会计期
            if (fiscalPeorid.FiscalMonth == 12)
            {
                lastYear = fiscalPeorid.FiscalYear - 1;
                lastMonth = 1;
            }
            else
            {
                lastYear = fiscalPeorid.FiscalYear;
                lastMonth = fiscalPeorid.FiscalMonth - 1;
            }

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string qcCon = "", qcCon_bq = "", bqCon_bq = "";
            if (isSummary)
            {
                qcCon = " and 1=2";
                qcCon_bq = "and 1=2";
                bqCon_bq = " and d.projectid='" + projectId + "' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            }
            else
            {
                qcCon = " and a.fiscalYear=" + lastYear + " and fiscalmonth=" + lastMonth + " and a.projectid='" + projectId + "'";
                qcCon_bq = " and d.projectid='" + projectId + "' and d.createdate<to_date('" + beginDate + "','yyyy-mm-dd')";
                bqCon_bq = " and d.projectid='" + projectId + "' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            }
            

            string sql = @"
                /*****这是料具租赁行******/
select * from (
                select name,0 lastQty,0 lastMoney, 
                0 cgsl,sum(zlje) cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,sum(zlje) xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,
                0 jcsl,0 jcje,sum(zlje) zlje,0 cjclcbje,0 ckje
                from(
                select '料具租赁结算' name,d.summatmoney zlje
                from thd_materialbalancemaster d where 1=1 " + bqCon_bq + @") group by name                
                /******以下是物资部分******/
                union all
                select to_char(parentCat.Name) name,sum(lastQty) lastQty,sum(lastMoney) lastMoney,
                sum(cgsl) cgsl,sum(cgje) cgje,sum(ljcgsl) ljcgsl,sum(ljcgje) ljcgje,sum(dbrksl) dbrksl,sum(dbrkje) dbrkje,
                sum(ljdbrksl) ljdbrksl,sum(ljdbrkje) ljdbrkje,sum(pysl) pysl,sum(pyje) pyje,sum(ljpysl) ljpysl,sum(ljpyje) ljpyje,
                sum(xhsl) xhsl,sum(xhje) xhje,sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje,sum(dbcksl) dbcksl,sum(dbckje) dbckje,
                sum(ljdbcksl) ljdbcksl,sum(ljdbckje) ljdbckje,sum(pksl) pksl,sum(pkje) pkje,sum(ljpksl) ljpksl,sum(ljpkje) ljpkje,
                sum(lastQty)+sum(cgsl)+sum(dbrksl)-sum(xhsl)-sum(dbcksl) jcsl,
                 (case when -0.05<= (sum(lastMoney)+sum(cgje)+sum(dbrkje)-sum(ckje)) 
                 and (sum(lastMoney)+sum(cgje)+sum(dbrkje)-sum(ckje)) <=0.05  then 0 else (sum(lastMoney)+sum(cgje)+sum(dbrkje)-sum(ckje)) end) jcje,
                0 zlje,0 cjclcbje,0 ckje
                from (
                select id,name,syscode,sum(nowquantity)-sum(outqty) lastQty,sum(nowmoney)-sum(outmoney) lastMoney, 
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,0 ckje
                from (
                --期初
                select c.id,c.name,c.syscode,a.nowquantity,a.nowmoney,0 outqty,0 outmoney
                from thd_stkstockinout a ,resmaterial b,resmaterialcategory c
                where  1<>1
                union all
                select c.id,c.name,c.syscode,a.quantity,a.money,0 outqty,0 outmoney
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockinmanner in (10,11,12) " + qcCon_bq + @"
                union all
                select c.id,c.name,c.syscode,0 inqty,0 inmoney,a.quantity,a.money
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockoutmanner in (20,21,22) " + qcCon_bq + @"
                ) group by id,name,syscode
                union all
                --本期收入数量、金额
                select c.id,c.name,c.syscode,0 lastQty,0 lastMoney,
                a.quantity cgsl,a.money cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrkjsl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,0 ckje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockinmanner in (10) " + bqCon_bq + @"
                union all
                --本期调入数量、金额
                select c.id,c.name,c.syscode,0 lastQty,0 lastMoney,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,a.quantity dbrksl,a.money dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,0 ckje
                from thd_stkstockin d,thd_stkstockindtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockinmanner in (11) " + bqCon_bq + @"
                union all
                --本期消耗数量、金额
                select c.id,c.name,c.syscode,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,a.money ckje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockoutmanner in (20) " + bqCon_bq + @"
                union all
                --本期调出数量、金额、盈亏金额
                select c.id,c.name,c.syscode,0 lastQty,0 lastQty,
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,0 xhje,0 ljxhsl,0 ljxhje,a.quantity dbcksl,a.moveMoney dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,

                (nvl(a.movemoney,0)-(round(a.quantity*(select sum(( case when a.quantity>=0 then t1.quantity else -t1.quantity end)*
                  nvl( (select avg(round((k1.money+k1.costmoney)/k1.quantity,4)) from thd_stockinbaldetail k1 where k1.refquantity != k1.quantity and 
                    t1.stockindtlid=k1.forwarddetailid),t1.price) ) /sum(case when a.quantity>=0 then t1.quantity else -t1.quantity end)
                   from thd_stkstockoutdtlseq t1 where t1.stockoutdtlid=
                  (case when a.quantity>=0 then a.id else a.forwarddetailid end) ),2))) pkje,
                0 ljpksl,0 ljpkje,a.money ckje
                from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                and d.istally=1 and d.stockoutmanner in (21) " + bqCon_bq + @"
                ) e,resmaterialcategory parentCat 
                where parentCat.Id=substr(e.syscode,instr(e.syscode,'.',1,2)+1,instr(e.syscode,'.',1,3)-instr(e.syscode,'.',1,2)-1 )
                 group by parentCat.Name /*order by parentCat.Name*/
                /*****这是冲减材料成本部份******/
                union all
                select '冲减材料成本' name,0 lastQty,0 lastMoney, 
                0 cgsl,0 cgje,0 ljcgsl,0 ljcgje,0 dbrksl,0 dbrkje,0 ljdbrksl,0 ljdbrkje,0 pysl,0 pyje,0 ljpysl,0 ljpyje,
                0 xhsl,-sum(t2.totalvalue) xhje,0 ljxhsl,0 ljxhje,0 dbcksl,0 dbckje,0 ljdbcksl,0 ljdbckje,0 pksl,0 pkje,0 ljpksl,0 ljpkje,0 jcsl,0 jcje,0 zlje,
                -sum(t2.totalvalue) cjclcbje ,0 ckje
                from thd_wastematprocessmaster d,thd_wastematprocessdetail t2 where d.id = t2.parentid and d.state=5 " + bqCon_bq + @"
  ) order by name
                ";

            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        /// <summary>
        /// 材料收发存汇总表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="_GWBSTreeSysCode"></param>
        /// <param name="isSummary">是否汇总表 true 汇总表</param>
        /// <returns></returns>
        public DataSet WZBB_clsfchzb_xhmx(string beginDate, string endDate, string projectId, string _GWBSTreeSysCode, bool isSummary)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string bqCon_bq = "";
            string otherCon = "";
            if (isSummary)
            {
                bqCon_bq = " and d.projectid='" + projectId + "' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
                otherCon = " and d.projectid='" + projectId + "' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and instr('" + _GWBSTreeSysCode + "',b.usedpart)>0";
            }
            else
            {
                bqCon_bq = " and d.projectid='" + projectId + "' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and e.syscode like '" + _GWBSTreeSysCode + "%'";
                otherCon = " and d.projectid='" + projectId + "' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and instr('" + _GWBSTreeSysCode + "',b.usedpart)>0";
            }


            string sql = @"
                /******料具租赁部分*****/
                select name,0 xhsl,sum(xhje) xhje,0 ljxhsl,0 ljxhje
                from(
                select '料具租赁' name,0 xhsl,sum(b.money) xhje,0 ljxhsl,0 ljxhje
                from thd_materialbalancemaster d,thd_materialbalancedetail b where d.id=b.parentid " + otherCon + @"
                union all
                select '料具租赁',0 xhsl,sum(b.costmoney) xhje,0 ljxhsl,0 ljxhje 
                from thd_materialbalancemaster d,thd_matbalothercostdetail b where d.id=b.parentid " + otherCon + @") group by name
                /****这是物资部分****/
                union all
                select to_char(parentCat.name) name,sum(xhsl) xhsl,sum(xhje) xhje,sum(ljxhsl) ljxhsl,sum(ljxhje) ljxhje from(
                --本期消耗数量、金额
                select c.id,c.name,c.syscode,a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje
                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
                left join thd_gwbstree e on a.usedpart=e.id 
                where d.istally=1 and d.stockoutmanner in (20) " + bqCon_bq + @"
                ) g,resmaterialcategory parentCat 
                where parentCat.Id=substr(g.syscode,instr(g.syscode,'.',1,2)+1,instr(g.syscode,'.',1,3)-instr(g.syscode,'.',1,2)-1 )
                 group by parentCat.Name /*order by parentCat.Name*/
                /*****这是冲减材料成本消耗部分*******/
                union all
                select '冲减材料成本' name,0 xhsl,-sum(b.totalvalue) xhje,0 ljxhsl,0 ljxhje 
                from thd_wastematprocessmaster d,thd_wastematprocessdetail b where d.id = b.parentid and d.state=5 " + otherCon + @"
                ";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
          
        /// <summary>
        /// 材料收发存汇总表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="_GWBSTreeSysCode"></param>
        /// <param name="isSummary">是否汇总表 true 汇总表</param>
        /// <returns></returns>
        public DataSet WZBB_clsfchzb_xhmx(string beginDate, string endDate, string projectId, bool isSummary)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string bqCon_bq = "";

            if (isSummary)
            {
                bqCon_bq = " and d.projectid='" + projectId + "' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 ";
            }
            else
            {

                bqCon_bq = " and d.projectid='" + projectId + "' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 ";
            }
            string sql = @" select   materialid,id,sum(xhsl) sumQuantity,sum(xhje) sumMoney 
                    from (
                    select   '料具租赁' materialid,b.usedpart id,b.money xhje,0 xhsl
                    from thd_materialbalancemaster d,thd_materialbalancedetail b 
                    where d.id=b.parentid  " + bqCon_bq + @"
                    union  all
                    select  '料具租赁' materialid,b.usedpart id,b.costmoney xhje,0 xhsl
                    from thd_materialbalancemaster d,thd_matbalothercostdetail b 
                    where d.id=b.parentid  " + bqCon_bq + @"
                    union  all
                    select   to_char(parentCat.Name) materialid  , e.id, xhje, xhsl 
                    from (  
                    select t.syscode, t.id,t.xhje,t.xhsl from 
                    (
                        select c.syscode,a.usedpart id ,a.money xhje, a.quantity xhsl
                        from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                        where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                        and d.istally=1 and d.stockoutmanner in (20)   " + bqCon_bq + @" 
                    ) t  
                   left join thd_gwbstree t1 on t.id=t1.id
                   )  e,resmaterialcategory parentCat 
                   where parentCat.Id=substr(e.syscode,instr(e.syscode,'.',1,2)+1,instr(e.syscode,'.',1,3)-instr(e.syscode,'.',1,2)-1 )
               
                /*****这是冲减材料成本部份******/
                union  all
                select  '冲减材料成本' materialid ,t2.usedpart id,0 xhsl,-t2.totalvalue xhje 
                from thd_wastematprocessmaster d,thd_wastematprocessdetail t2 where d.id = t2.parentid and d.state=5   " + bqCon_bq + @") t
                 group by  materialid,id";


            //                string sql = @"select  tt.materialid,tt.id,sum(xhsl) sumQuantity,sum(xhje) sumMoney 
            //                from(
            //                select '料具租赁' materialid,nvl(USEDPART,'未知部位')id, 0 xhsl,b.money xhje ,to_char(b.USEDPARTSYSCODE) syscode
            //                from thd_materialbalancemaster d,thd_materialbalancedetail b where d.id=b.parentid " + bqCon_bq+ @"
            //                union all
            //                select '料具租赁' materialid,nvl(USEDPART,'未知部位')id,0 xhsl,b.costmoney xhje ,to_char(b.USEDPARTSYSCODE) syscode
            //                from thd_materialbalancemaster d,thd_matbalothercostdetail b where d.id=b.parentid  " + bqCon_bq + @" 
            //              
            //                /****这是物资部分****/
            //                union all
            //                select to_char(nvl(parentCat.name,'')) materialid,nvl(USEDPART,'未知部位') id, xhsl, xhje ,USEDPARTSYSCODE syscode from(
            //                --本期消耗数量、金额
            //                select c.id,c.name,c.syscode,a.USEDPART,a.quantity xhsl,a.money xhje,0 ljxhsl,0 ljxhje,to_char(e.syscode) USEDPARTSYSCODE
            //                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
            //                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
            //                left join thd_gwbstree e on a.usedpart=e.id 
            //                where d.istally=1 and d.stockoutmanner in (20)  " + bqCon_bq + @"
            //                ) g,resmaterialcategory parentCat 
            //                where parentCat.Id=substr(g.syscode,instr(g.syscode,'.',1,2)+1,instr(g.syscode,'.',1,3)-instr(g.syscode,'.',1,2)-1 )
            //               
            //                /*****这是冲减材料成本消耗部分*******/
            //                union all
            //                select '冲减材料成本' materialid,nvl(b.USEDPART,'未知部位') id,0 xhsl,b.totalvalue xhje ,to_char(b.USEDPARTSYSCODE) USEDPARTSYSCODE
            //                from thd_wastematprocessmaster d,thd_wastematprocessdetail b where d.id = b.parentid and d.state=5  " + bqCon_bq + @") tt
            //                  left join (
            //               select t1.syscode,t1.name,t1.id from thd_gwbstree t1 where t1.warehouseflag=1 ) tt1   on instr(tt.syscode,tt1.syscode)>0
            //                group by   tt.materialid,tt.id";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        public DataSet WZBB_clsfchzb_xhmx(string beginDate, string endDate, string projectId,IList lstUsePart, bool isSummary)
        {
            //查询开始日期所在的会计期
            FiscalPeriodDetail fiscalPeorid = GetFiscalPeriod(beginDate);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string bqCon_bq = "";

            if (isSummary)
            {
                bqCon_bq = " and d.projectid='" + projectId + "' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 ";
            }
            else
            {

                bqCon_bq = " and d.projectid='" + projectId + "' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 ";
            }
            string sWhereUsePart = GetUsePartCodes(lstUsePart);
//            string sql = @" select   materialid,id,sum(xhsl) sumQuantity,sum(xhje) sumMoney 
//                    from (
//                    select   '料具租赁' materialid,b.usedpart id,b.money xhje,0 xhsl
//                    from thd_materialbalancemaster d,thd_materialbalancedetail b 
//                    where d.id=b.parentid  " + bqCon_bq + @"
//                    union  all
//                    select  '料具租赁' materialid,b.usedpart id,b.costmoney xhje,0 xhsl
//                    from thd_materialbalancemaster d,thd_matbalothercostdetail b 
//                    where d.id=b.parentid  " + bqCon_bq + @"
//                    union  all
//                    select   to_char(parentCat.Name) materialid  , e.id, xhje, xhsl 
//                    from (  
//                    select t.syscode, t.id,t.xhje,t.xhsl from 
//                    (
//                        select c.syscode,a.usedpart id ,a.money xhje, a.quantity xhsl
//                        from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
//                        where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
//                        and d.istally=1 and d.stockoutmanner in (20)   " + bqCon_bq + @" 
//                    ) t  
//                   left join thd_gwbstree t1 on t.id=t1.id
//                   )  e,resmaterialcategory parentCat 
//                   where parentCat.Id=substr(e.syscode,instr(e.syscode,'.',1,2)+1,instr(e.syscode,'.',1,3)-instr(e.syscode,'.',1,2)-1 )
//               
//                /*****这是冲减材料成本部份******/
//                union  all
//                select  '冲减材料成本' materialid ,t2.usedpart id,0 xhsl,-t2.totalvalue xhje 
//                from thd_wastematprocessmaster d,thd_wastematprocessdetail t2 where d.id = t2.parentid and d.state=5   " + bqCon_bq + @") t
//                 group by  materialid,id";

            string sSQL = @"select   materialid,to_char(nvl(t1.id,'')) headid,sum(xhsl) Quantity,sum(xhje) Money 
                    from (
                    select   '料具租赁' materialid,to_char(b.usedpartsyscode)usedpartsyscode ,b.money xhje,0 xhsl
                    from thd_materialbalancemaster d,thd_materialbalancedetail b 
                    where d.id=b.parentid  " + bqCon_bq + @"
                    union  all
                    select  '料具租赁' materialid,to_char(b.usedpartsyscode)usedpartsyscode  ,b.costmoney xhje,0 xhsl
                    from thd_materialbalancemaster d,thd_matbalothercostdetail b 
                    where d.id=b.parentid   " + bqCon_bq + @"
                    union all
                    select   '料具租赁' materialid,   to_char(d.usedpartsyscode)usedpartsyscode ,d.othermoney xhje,0 xhsl
                    from thd_materialbalancemaster d
                    where d.usedpart is not null " + bqCon_bq + @"
                    union  all
                    select   to_char(parentCat.Name) materialid  ,to_char( e.usedpartsyscode)usedpartsyscode, xhje, xhsl 
                    from (  
                    select t.syscode, t1.syscode usedpartsyscode,t.xhje,t.xhsl from 
                    (
                        select c.syscode,a.usedpart id ,a.money xhje, a.quantity xhsl
                        from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                        where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                        and d.istally=1 and d.stockoutmanner in (20)   " + bqCon_bq + @"
                    ) t  
                   left join thd_gwbstree t1 on t.id=t1.id
                   )  e,resmaterialcategory parentCat 
                   where parentCat.Id=substr(e.syscode,instr(e.syscode,'.',1,2)+1,instr(e.syscode,'.',1,3)-instr(e.syscode,'.',1,2)-1 )
               
                /*****这是冲减材料成本部份******/
                union  all
                select  '冲减材料成本' materialid ,to_char(t2.usedpartsyscode) usedpartsyscode ,-t2.totalvalue xhje,0 xhsl 
                from thd_wastematprocessmaster d,thd_wastematprocessdetail t2 where d.id = t2.parentid and d.state=5  " + bqCon_bq + @" ) t
                left join (
                select t.id,t.syscode   from  thd_gwbstree t   
                where  " + sWhereUsePart + @"
                ) t1 on instr(t.usedpartsyscode,t1.syscode)>0
                 group by  t.materialid,to_char(nvl(t1.id,''))
                 ";


            command.CommandText = sSQL;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        public DataSet WZBB_clsfchzb_UserPart(string beginDate, string endDate, string projectId, bool isSummary)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string bqCon_bq = "";

            if (isSummary)
            {
                bqCon_bq = " and d.projectid='" + projectId + "' and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1  ";

            }
            else
            {
                bqCon_bq = " and d.projectid='" + projectId + "' and d.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and d.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1  ";
                //otherCon = " and t.projectid='" + projectId + "' and t.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and t.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1  ";
            }
            //nvl(a.subjectname,'') Name, nvl(e.name,'') ParentName,nvl(a.subjectguid,'') ID,nvl(e.id,'') ParentID
            string sql = @"select  distinct t.usedpart id,nvl(t.usedpartname,'') name, t1.id parentid,nvl(t1.name,'') parentname
                    from (
                    select distinct b.usedpart,to_char(b.usedpartname) usedpartname,to_char(b.usedpartsyscode)usedpartsyscode
                    from thd_materialbalancemaster d,thd_materialbalancedetail b 
                    where d.id=b.parentid  " + bqCon_bq + @"
                    union  
                    select distinct b.usedpart,to_char(b.usedpartname),to_char(b.usedpartsyscode)
                    from thd_materialbalancemaster d,thd_matbalothercostdetail b 
                    where d.id=b.parentid   " + bqCon_bq + @"
                    union  
                    select distinct usedpart,to_char(usedpartname),to_char(usedpartsyscode)   
                    from ( 
                    select t.syscode, t.usedpart, t.usedpartname,t1.syscode usedpartsyscode from 
                    (
                        select c.syscode, a.usedpart,a.usedpartname
                        from thd_stkstockout d,thd_stkstockoutdtl a,resmaterial b,resmaterialcategory c
                        where d.id=a.parentid and a.material=b.materialid and b.materialcategoryid=c.id and d.special='土建'
                        and d.istally=1 and d.stockoutmanner in (20)    " + bqCon_bq + @"
                    ) t 
                   left join thd_gwbstree t1 on t.usedpart=t1.id
                   ) e,resmaterialcategory parentCat 
                   where parentCat.Id=substr(e.syscode,instr(e.syscode,'.',1,2)+1,instr(e.syscode,'.',1,3)-instr(e.syscode,'.',1,2)-1 )
               
                /*****这是冲减材料成本部份******/
                union  
                select  distinct t2.usedpart,to_char(t2.usedpartname),to_char(t2.usedpartsyscode)
                from thd_wastematprocessmaster d,thd_wastematprocessdetail t2 where d.id = t2.parentid and d.state=5    " + bqCon_bq + @") t
                 join (select  t.id,t.name,t.syscode from   thd_gwbstree t where t.warehouseflag=1) t1 on instr(t.usedpartsyscode,t1.syscode)>0 
               where t.usedpart is not null    order by t1.id  ,  parentname";
            //            string sql = @"select tt.id id, tt.name  name ,nvl(tt1.name,'') parentname,nvl(tt1.id,'') parentid
            //                from(
            //                select   b.USEDPART ID ,to_char(b.USEDPARTNAME) NAME,to_char(b.USEDPARTSYSCODE) syscode
            //                from thd_materialbalancemaster d,thd_materialbalancedetail b where d.id=b.parentid  " + bqCon_bq + @"
            //                union  
            //                select  b.USEDPART ID ,to_char(b.USEDPARTNAME) NAME,to_char(b.USEDPARTSYSCODE) syscode
            //                from thd_materialbalancemaster d,thd_matbalothercostdetail b where d.id=b.parentid   " + bqCon_bq + @"
            //              
            //                /****这是物资部分****/
            //                union  
            //                select  G.USEDPART ID ,to_char(G.USEDPARTNAME) NAME ,to_char(USEDPARTSYSCODE) syscode from(
            //                --本期消耗数量、金额
            //                select c.id,c.name,c.syscode,a.USEDPART, A.USEDPARTNAME,e.syscode USEDPARTSYSCODE  
            //                from thd_stkstockout d join thd_stkstockoutdtl a on d.id=a.parentid
            //                join resmaterial b on a.material=b.materialid join resmaterialcategory c on b.materialcategoryid=c.id
            //                left join thd_gwbstree e on a.usedpart=e.id 
            //                where d.istally=1 and d.stockoutmanner in (20)      " + bqCon_bq + @"
            //                ) g,resmaterialcategory parentCat 
            //                where parentCat.Id=substr(g.syscode,instr(g.syscode,'.',1,2)+1,instr(g.syscode,'.',1,3)-instr(g.syscode,'.',1,2)-1 )
            //               
            //                /*****这是冲减材料成本消耗部分*******/
            //                union  
            //                select  B.USEDPART ID ,to_char(B.USEDPARTNAME) NAME ,to_char(b.USEDPARTSYSCODE) syscode
            //                from thd_wastematprocessmaster d,thd_wastematprocessdetail b where d.id = b.parentid and d.state=5 " + bqCon_bq + @" )tt
            //               left join (
            //               select t1.syscode,t1.name,t1.id from thd_gwbstree t1 where t1.warehouseflag=1 ) tt1 
            //               on instr(tt.syscode,tt1.syscode)>0
            //               where tt.id  is not null
            //               order by    nvl(tt1.name,'') ,tt.name  ";
            //  sql = string.Format(sql, bqCon_bq);
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        /// <summary>
        /// 调拨材料统计表(出库)
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataSet WZBB_dbck(string beginDate, string endDate, string projectId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string conBq = " and a.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and a.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and a.projectid='" + projectId + "'";
            string conLj = " and a.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and a.projectid='" + projectId + "'";

            string sql = @"select moveoutprojectname,materialname,materialspec,matstandardunitname,sum(bqsl) bqsl,sum(bqje) bqje,sum(ljsl) ljsl,sum(ljje) ljje,sum(bqykje) bqykje,sum(ljykje) ljykje
                from (
                select a.moveoutprojectname,b.materialname,b.materialspec,b.matstandardunitname,b.quantity bqsl,b.moveMoney bqje,0 ljsl,0 ljje,

                (nvl(b.movemoney,0)-(round(b.quantity*(select sum(( case when b.quantity>=0 then t1.quantity else -t1.quantity end)*
                  nvl( (select avg(round((k1.money+k1.costmoney)/k1.quantity,4)) from thd_stockinbaldetail k1 where k1.refquantity != k1.quantity and 
                t1.stockindtlid=k1.forwarddetailid),t1.price) ) /sum(case when b.quantity>=0 then t1.quantity else -t1.quantity end)
                   from thd_stkstockoutdtlseq t1 where t1.stockoutdtlid=
                  (case when b.quantity>=0 then b.id else b.forwarddetailid end) ),2))) bqykje,0 ljykje
                from thd_stkstockout a,thd_stkstockoutdtl b where a.id=b.parentid and a.special='土建'
                and a.stockoutmanner=21 " + conBq + @"
                union all
                select a.moveoutprojectname,b.materialname,b.materialspec,b.matstandardunitname,0 bqsl,0 bqje,b.quantity ljsl,b.moveMoney ljje,0 bqykje,
                 (nvl(b.movemoney,0)-(round(b.quantity*(select sum(( case when b.quantity>=0 then t1.quantity else -t1.quantity end)*
                  nvl( (select avg(round((k1.money+k1.costmoney)/k1.quantity,4)) from thd_stockinbaldetail k1 where k1.refquantity != k1.quantity and 
                t1.stockindtlid=k1.forwarddetailid),t1.price) ) /sum(case when b.quantity>=0 then t1.quantity else -t1.quantity end)
                   from thd_stkstockoutdtlseq t1 where t1.stockoutdtlid=
                  (case when b.quantity>=0 then b.id else b.forwarddetailid end) ),2))) ljykje
                from thd_stkstockout a,thd_stkstockoutdtl b where a.id=b.parentid and a.special='土建'
                and a.stockoutmanner=21 " + conLj + @"
                ) group by moveoutprojectname,materialname,materialspec,matstandardunitname";

            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        /// <summary>
        /// 调拨材料统计表(入库)、甲供材料汇总表
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="isMaterialProvider">true 甲供</param>
        /// <returns></returns>
        public DataSet WZBB_dbrk(string beginDate, string endDate, string projectId, bool isMaterialProvider)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string conBq = " and a.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and a.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and a.projectid='" + projectId + "'";
            string conLj = " and a.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and a.projectid='" + projectId + "'";

            if (isMaterialProvider)
            {
                conBq += " and a.materialprovider=1";
                conLj += " and a.materialprovider=1";
            }
            string sql = @"select moveoutprojectname,materialname,materialspec,matstandardunitname,sum(bqsl) bqsl,sum(bqje) bqje,sum(ljsl) ljsl,sum(ljje) ljje
                from (
                select a.moveoutprojectname,b.materialname,b.materialspec,b.matstandardunitname,b.quantity bqsl,b.money bqje,0 ljsl,0 ljje
                from thd_stkstockin a,thd_stkstockIndtl b where a.id=b.parentid and a.special='土建' 
                and a.stockInManner=11 " + conBq + @"
                union all
                select a.moveoutprojectname,b.materialname,b.materialspec,b.matstandardunitname,0 bqsl,0 bqje,b.quantity ljsl,b.money ljje
                from thd_stkstockin a,thd_stkstockIndtl b where a.id=b.parentid and a.special='土建'   
                and a.stockInManner=11 " + conLj + @"
                ) group by moveoutprojectname,materialname,materialspec,matstandardunitname";

            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        /// <summary>
        /// 材料收发存汇总表 冲减材料成本
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="isSummary">是否汇总表 true 汇总表</param>
        /// <returns></returns>
        public Hashtable WZBB_cjclcbje(string beginDate, string endDate, string projectId, bool isSummary)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            Hashtable ht = new Hashtable();
            string con = "";
            if (isSummary)
            {
                con = "and projectid='" + projectId + "' and createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            }
            else
            {
                con = "and projectid='" + projectId + "' and createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 and createdate>=to_date('" + beginDate + "','yyyy-mm-dd')";
            }
            string sql = @"select sum(t2.totalvalue),t2.usedpartsyscode from thd_wastematprocessmaster t1 inner join thd_wastematprocessdetail t2 on t1.id = t2.parentid where state=5 " + con + " group by t2.usedpartsyscode ";

            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            //读取dateset的值
            DataTable dataTable = dataSet.Tables[0];
            //Tables[0].Rows.Count != 0
            if (dataTable.Rows.Count != 0)
            {
                //循环读取table的值
                for (int i = 0; i < dataTable.Rows.Count; i++)//循环读取临时表的行，第一行的信息不读取
                {
                    decimal money = Convert.ToDecimal(dataTable.Rows[i][0]);
                    string strSysCode = dataTable.Rows[i][1].ToString();
                    if (strSysCode != "")
                    {
                        string[] strArry = strSysCode.Split('.');
                        //层次码小于三层的不用截取
                        if (strArry.Length <= 4)
                        {
                            if (ht.Count == 0)
                            {
                                ht.Add(strSysCode, money);
                            }
                            else
                            {
                                if (ht.Contains(strSysCode))
                                {
                                    decimal temp = TransUtil.ToDecimal(ht[strSysCode]);
                                    ht.Remove(strSysCode);
                                    ht.Add(strSysCode, temp + money);
                                }
                                else
                                {
                                    ht.Add(strSysCode, money);
                                }
                            }
                        }
                        if (strArry.Length > 4)
                        {
                            string strNewSysCode = strArry[0] + "." + strArry[1] + "." + strArry[2];
                            if (ht.Contains(strNewSysCode))
                            {
                                decimal temp = TransUtil.ToDecimal(ht[strNewSysCode]);
                                ht.Remove(strNewSysCode);
                                ht.Add(strNewSysCode, temp + money);
                            }
                            else
                            {
                                ht.Add(strSysCode, money);
                            }
                        }
                    }
                }
            }
            //command.CommandText = sql;
            //object o = command.ExecuteScalar();
            //decimal ret = 0M;
            //if (o != null && !(o is DBNull))
            //{
            //    ret = decimal.Parse(o + "");
            //    if (ht.Contains(""))
            //    {
            //        decimal temp = TransUtil.ToDateTime(ht[""]);
            //        ht.Remove("");
            //        ht.Add("", temp + 0);
            //    }
            //    else {
            //        ht.Add("", 0);
            //    }
            //}
            return ht;
        }

        /// <summary>
        /// 材料收发存台帐
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="materailId"></param>
        /// <returns></returns>
        public DataSet WZBB_clsfctz(string projectid, string materailId, string beginDate, string endDate)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string con = " and a.projectid='" + projectid + "' and b.material='" + materailId + "' and a.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and a.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1 ";
            string sql = @"select * from (
                select djlx,type,to_char(createdate,'yyyy-mm-dd') createdate,code,supplierrelationname,lastmodifydate,price,sum(cgsl) cgsl,sum(cgje) cgje,
                sum(jssl) jssl,sum(jsje) jsje,sum(dbrksl) dbrksl,sum(dbrkje) dbrkje,sum(dbrksl)+sum(cgsl) rksl,
                sum(cgje)+sum(dbrkje) rkje,sum(llcksl) llcksl,sum(llckje) llckje,sum(dbcksl) dbcksl,sum(dbckje) dbckje,
                sum(llcksl)+sum(dbcksl) cksl,sum(llckje)+sum(dbckje) ckje,sum(ykje) ykje,gwbsTreeSyscode,sum(tempckje) tempckje
                from (
                --收料数量、金额
                select 'cgrk' djlx,0 type, a.createdate,a.code,a.supplierrelationname,a.lastmodifydate,b.price,b.quantity cgsl,b.money cgje,
                0 jssl,0 jsje,0 dbrksl,0 dbrkje,0 llcksl,0 llckje,0 dbcksl,0 dbckje,0 ykje,null gwbsTreeSyscode,0 tempckje
                from thd_stkstockin a,thd_stkstockindtl b where a.id=b.parentid and a.stockinmanner=10  and a.special='土建'
                and a.istally=1 " + con + @"
                union all
                --验收数量、金额
                ---'ysjsd' djlx,a.createdate,a.code,a.supplierrelationname,a.lastmodifydate,
                select 'ysjsd' djlx, 2 type,a.createdate,a.code,a.supplierrelationname,a.lastmodifydate,b.price,0 cgsl,0 cgje,b.quantity jssl,b.money jsje,
                0 dbrksl,0 dbrkje,0 llcksl,0 llckje,0 dbcksl,0 dbckje,0 ykje,null gwbsTreeSyscode,0 tempckje
                from thd_stockinbalmaster a,thd_stockinbaldetail b,thd_stkstockindtl c ,thd_stkstockin d  where a.id=b.parentid   
                and b.forwarddetailid=c.id and c.parentid=d.id and d.special='土建'
                and a.state=5 " + con + @"
                union all
                --验收红单数量、金额
                ---'ysjsd' djlx,a.createdate,a.code,a.supplierrelationname,a.lastmodifydate,
                select 'ysjsd' djlx, 2 type,a.createdate,a.code,a.supplierrelationname,a.lastmodifydate,b.price,0 cgsl,0 cgje,b.quantity jssl,b.money jsje,
                0 dbrksl,0 dbrkje,0 llcksl,0 llckje,0 dbcksl,0 dbckje,0 ykje,null gwbsTreeSyscode,0 tempckje
                from thd_stockinbalmaster a,thd_stockinbaldetail b,thd_stkstockindtl c ,thd_stkstockin d ,thd_stockinbaldetail e where a.id=b.parentid   
                and   b.forwarddetailid=e.id and e.forwarddetailid=c.id and c.parentid=d.id and d.special='土建'
                and a.state=5 " + con + @"
                union all
                --调拨入库数量、金额
                select 'dbrk' djlx, 1 type, a.createdate,a.code,a.moveoutprojectname,a.lastmodifydate,b.price,0 cgsl,0 cgje,
                0 jssl,0 jsje,b.quantity dbrksl,b.money dbrkje,0 llcksl,0 llckje,0 dbcksl,0 dbckje,0 ykje,null gwbsTreeSyscode,0 tempckje
                from thd_stkstockin a,thd_stkstockindtl b where a.id=b.parentid and a.stockinmanner=11 and a.special='土建'
                and a.istally=1 " + con + @"
                union all
                --领料出库数量、金额
                select 'llck' djlx, 3 type, a.createdate,a.code,a.supplierrelationname,a.lastmodifydate,b.price,0 cgsl,0 cgje,
                0 jssl,0 jsje,0 dbrksl,0 dbrkje,b.quantity llcksl,b.money llckje,0 dbcksl,0 dbckje,0 ykje,c.syscode gwbsTreeSyscode,b.money  tempckje
                from thd_stkstockout a,thd_stkstockoutdtl b,thd_gwbstree c 
                where a.id=b.parentid and a.stockOutManner=20 and b.usedpart=c.id(+) and a.stockOutManner=20 and a.special='土建'
                and a.istally=1 " + con + @"
                union all
                --调拨出库数量、金额、盈亏金额
                select 'dbck' djlx, 4 type, a.createdate,a.code,a.moveoutprojectname,a.lastmodifydate,b.moveprice,0 cgsl,0 cgje,
                0 jssl,0 jsje,0 dbrksl,0 dbrkje,0 llcksl,0 llckje,b.quantity dbcksl,b.moveMoney dbckje,
                (nvl(b.movemoney,0)-(round(b.quantity*(select sum(( case when b.quantity>=0 then t1.quantity else -t1.quantity end)*
                  nvl( (select avg(round((k1.money+k1.costmoney)/k1.quantity,4)) from thd_stockinbaldetail k1 where k1.refquantity != k1.quantity and 
                t1.stockindtlid=k1.forwarddetailid),t1.price) ) /sum(case when b.quantity>=0 then t1.quantity else -t1.quantity end)
                   from thd_stkstockoutdtlseq t1 where t1.stockoutdtlid=
                  (case when b.quantity>=0 then b.id else b.forwarddetailid end) ),2))) ykje,null gwbsTreeSyscode,b.money  tempckje
                   from thd_stkstockout a,thd_stkstockoutdtl b where a.id=b.parentid and a.stockOutManner=21 and a.special='土建'
                and a.istally=1 " + con + @"
                ) group by djlx,type,createdate,code,supplierrelationname,lastmodifydate,price,gwbsTreeSyscode order by lastmodifydate) order by  createdate  ,type,lastmodifydate
                ";
            //nvl( (select avg(abs((k1.money+k1.costmoney)/k1.quantity)) 
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        #endregion

        #region 料具报表
        /// <summary>
        /// 料具租赁月报
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public DataSet WZBB_Ljzlyb(string projectId, int fiscalYear, int fiscalMonth, string supplierId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string conBy = " and a.projectid='" + projectId + "' and a.fiscalyear=" + fiscalYear + " and a.fiscalmonth=" + fiscalMonth;
            string conLj = " and a.projectid='" + projectId + "' and a.fiscalyear*100+a.fiscalmonth<=" + (fiscalYear * 100 + fiscalMonth);
            if (!string.IsNullOrEmpty(supplierId)) 
            {
                conBy += " and a.supplierrelation='" + supplierId + "'";
                conLj += " and a.supplierrelation='" + supplierId + "'";
            }
            string sql = @"select * from (
                select to_char(max(enddate),'yyyy-mm-dd') enddate,nvl(material,'')material,nvl(materialcode,'')materialcode,nvl(materialname,'')materialname,nvl(materialspec,'')materialspec,nvl(matstandardunitname,'')matstandardunitname,
                sum(jcslby) jcslby,sum(jcsllj) jcsllj,sum(zljeby) zljeby,sum(zljelj) zljelj,sum(whslby) whslby,sum(whsllj) whsllj,
                sum(wxslby) wxslby,sum(wxsllj) wxsllj,sum(qtslby) qtslby,sum(qtsllj) qtsllj,sum(bfslby) bfslby,sum(bfsllj) bfsllj,
                sum(bsslby) bsslby,sum(bssllj) bssllj,sum(xhslby) xhslby,sum(xhsllj) xhsllj, max(sysj) sysj,
                sum(whslby)+sum(wxslby)+sum(qtslby)+sum(bfslby)+sum(bsslby)+sum(xhslby) tcslby,
                sum(whsllj)+sum(wxsllj)+sum(qtsllj)+sum(bfsllj)+sum(bssllj)+sum(xhsllj) tcsllj,
                sum(jcsllj)-(sum(whsllj)+sum(wxsllj)+sum(qtsllj)+sum(bfsllj)+sum(bssllj)+sum(xhsllj)) jcsl
                from (
                /****本月进场数量***/
                select a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname,
                sum(b.approachquantity) jcslby,0 jcsllj,0 zljeby,0 zljelj,0 whslby,0 whsllj,0 wxslby,0 wxsllj,
                0 qtslby,0 qtsllj,0 bfslby,0 bfsllj,0 bsslby,0 bssllj,0 xhslby,0 xhsllj,0 sysj
                from thd_materialbalancemaster a,thd_materialbalancedetail b where a.id=b.parentid and b.balstate='本期发生' " + conBy + @" 
                group by a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /*****累计进场数量******/
                union all
                select a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname,
                0 jcslby,sum(b.approachquantity) jcsllj,0 zljeby,0 zljelj,0 whslby,0 whsllj,0 wxslby,0 wxsllj,
                0 qtslby,0 qtsllj,0 bfslby,0 bfsllj,0 bsslby,0 bssllj,0 xhslby,0 xhsllj,0 sysj
                from thd_materialbalancemaster a,thd_materialbalancedetail b where a.id=b.parentid " + conLj + @" 
                group by a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /*****本月租赁金额*****/
                union all
                select a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname,
                0 jcslby,0 jcsllj,sum(b.money) zljeby,0 zljelj,0 whslby,0 whsllj,0 wxslby,0 wxsllj,
                0 qtslby,0 qtsllj,0 bfslby,0 bfsllj,0 bsslby,0 bssllj,0 xhslby,0 xhsllj,0 sysj
                from thd_materialbalancemaster a,thd_materialbalancedetail b where a.id=b.parentid " + conBy + @"  
                group by a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /*****累计租赁金额*****/
                union all
                select a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname,
                0 jcslby,0 jcsllj,0 zljeby,sum(b.money) zljelj,0 whslby,0 whsllj,0 wxslby,0 wxsllj,
                0 qtslby,0 qtsllj,0 bfslby,0 bfsllj,0 bsslby,0 bssllj,0 xhslby,0 xhsllj,0 sysj
                from thd_materialbalancemaster a,thd_materialbalancedetail b where a.id=b.parentid " + conLj + @"  
                group by a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /*****本月退场数量*****/
                union all
                select a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname,
                0 jcslby,0 jcsllj,0 zljeby,0 zljelj,sum(broachQuantity) whslby,0 whsllj,sum(repairQty) wxslby,0 wxsllj,
                sum(discardQty) qtslby,0 qtsllj,sum(rejectQuantity) bfslby,0 bfsllj,sum(lossQty) bsslby,0 bssllj,sum(consumeQuantity) xhslby,0 xhsllj,0 sysj
                from thd_materialbalancemaster a,thd_materialbalancedetail b,thd_materialreturndetail c
                where a.id=b.parentid and b.matcolldtlid=c.id and b.matreturncode is not null and b.balstate='本期发生' " + conBy + @"  
                group by a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /****累计退场数量******/
                union all
                select a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname,
                0 jcslby,0 jcsllj,0 zljeby,0 zljelj,0 whslby,sum(broachQuantity) whsllj,0 wxslby,sum(repairQty) wxsllj,
                0 qtslby,sum(discardQty) qtsllj,0 bfslby,sum(rejectQuantity) bfsllj,0 bsslby,sum(lossQty) bssllj,0 xhslby,sum(consumeQuantity) xhsllj,0 sysj
                from thd_materialbalancemaster a,thd_materialbalancedetail b,thd_materialreturndetail c
                where a.id=b.parentid and b.matcolldtlid=c.id and b.matreturncode is not null " + conLj + @" 
                group by a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /******使用时间****/
                union all
                select a.enddate,b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname,
                0 jcslby,0 jcsllj,0 zljeby,0 zljelj,0 whslby,0 whsllj,0 wxslby,0 wxsllj,
                0 qtslby,0 qtsllj,0 bfslby,0 bfsllj,0 bsslby,0 bssllj,0 xhslby,0 xhsllj,
                /******months_between(to_date(to_char(a.fiscalyear*10000+a.fiscalmonth*100+1),'yyyymmdd'),to_date(to_char(d.createyear*10000+d.createmonth*100+1),'yyyymmdd'))+1 sysj****/
                ((a.fiscalyear-d.createyear)*12+a.fiscalmonth-d.createmonth) sysj 
                from thd_materialbalancemaster a,thd_materialbalancedetail b,thd_materialcollectionmaster d,thd_materialcollectiondetail c
                where a.id=b.parentid and b.matcollcode is not null and c.parentid=d.id and c.id=b.matcolldtlid " + conBy + @" 
                ) group by material,materialcode,materialname,materialspec,matstandardunitname) t order by materialname,materialspec,matstandardunitname
                ";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }



        public DataSet WZBB_LjzlybByUsePart(string projectId, int fiscalYear, int fiscalMonth, string supplierId,IList lstUsePart)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string conBy = " and a.projectid='" + projectId + "' and a.fiscalyear=" + fiscalYear + " and a.fiscalmonth=" + fiscalMonth;
            string conLj = " and a.projectid='" + projectId + "' and a.fiscalyear*100+a.fiscalmonth<=" + (fiscalYear * 100 + fiscalMonth);
            string sUsePartIDs = GetUsePartCodes(lstUsePart);
            if (!string.IsNullOrEmpty(supplierId))
            {
                conBy += " and a.supplierrelation='" + supplierId + "'";
                conLj += " and a.supplierrelation='" + supplierId + "'";
            }
            string sql = @" select  tt.material materialid, nvl( tt1.usepartID,tt.usedpart) HeadID ,sum(tt.jcslby) Quantity,sum(tt.jcsllj) lstQuantity,sum(moneyby) moneyby,sum(moneylj) moneylj
                from (
                 ---结存
                select b.material,to_char(b.usedpartsyscode) usedpartsyscode,to_char(b.usedpart) usedpart,
                nvl(b.approachquantity,0) jcslby ,nvl(b.approachquantity,0) jcsllj,0 moneyby,0 moneylj
                from thd_materialbalancemaster a,thd_materialbalancedetail b where a.id=b.parentid and b.balstate='本期发生' " + conLj + @" 
                union all
                 select  b.material,to_char(b.usedpartsyscode),to_char(b.usedpart),
                -(c.broachQuantity+c.REPAIRQTY+c.DISCARDQTY+c.REJECTQUANTITY+c.LOSSQTY+c.CONSUMEQUANTITY)jcslby,
                 -(c.broachQuantity+c.REPAIRQTY+c.DISCARDQTY+c.REJECTQUANTITY+c.LOSSQTY+c.CONSUMEQUANTITY)jcsllj,
                 0 moneyby,0 moneylj
                from thd_materialbalancemaster a,thd_materialbalancedetail b,thd_materialreturndetail c
                where a.id=b.parentid and b.matcolldtlid=c.id and b.matreturncode is not null " + conLj + @" 
                union all
                ---本月租赁
                 select b.material,to_char(b.usedpartsyscode),to_char(b.usedpart),
                0 jcslby ,0 jcsllj,nvl(b.money,0) moneyby,0 moneylj
                from thd_materialbalancemaster a,thd_materialbalancedetail b where a.id=b.parentid   " + conBy + @" 
                 union all
                 ---累计租赁
                 select b.material,to_char(b.usedpartsyscode),to_char(b.usedpart),
                0 jcslby ,0 jcsllj,0 moneyby,nvl(b.money,0) moneylj
                from thd_materialbalancemaster a,thd_materialbalancedetail b where a.id=b.parentid  " + conLj + @" 
                union all
               select  to_char(b.COSTTYPE),to_char(b.usedpartsyscode),to_char(b.usedpart),
                0 tcslby,0 tcsllj,nvl(b.costMoney,0)moneyby,0 moneylj
                from thd_materialbalancemaster a,thd_matbalothercostdetail b where a.id=b.parentid  " + conBy + @" 
                 union all
               select  to_char(b.COSTTYPE),to_char(b.usedpartsyscode),to_char(b.usedpart),
                0 tcslby,0 tcsllj,0 moneyby,nvl(b.costMoney,0) moneylj
                from thd_materialbalancemaster a,thd_matbalothercostdetail b where a.id=b.parentid  " + conLj + @" 
                union all
                 select  '调整费用',to_char(a.usedpartsyscode),to_char(a.usedpart),
                0 tcslby,0 tcsllj,nvl(a.othermoney,0)moneyby,0 moneylj
                from thd_materialbalancemaster a  where 1=1    " + conBy + @" 
                union all
               select   '调整费用',to_char(a.usedpartsyscode),to_char(a.usedpart),
                0 tcslby,0 tcsllj,0 moneyby,nvl(a.othermoney,0) moneylj
                from thd_materialbalancemaster a  where 1=1   " + conLj + @" 
               ) tt
              left join (
                select t.id usepartID,t.syscode usePartSysCode  from  thd_gwbstree t 
                where " + sUsePartIDs+ @")tt1  
                on instr(  tt.usedpartsyscode,tt1.usePartSysCode)>0
               group by tt.material  ,  nvl( tt1.usepartID,tt.usedpart)";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);

             //union all
             //   select  b.material,b.usedpartsyscode,b.usedpart, 
             //   nvl(broachQuantity,0) + nvl(repairQty,0)+nvl(discardQty,0)+nvl(rejectQuantity,0)+nvl(lossQty,0)+nvl(consumeQuantity,0) tcslby,0 tcsllj
             //   ,nvl(b.money,0)moneyby,0 moneylj
             //   from thd_materialbalancemaster a,thd_materialbalancedetail b,thd_materialreturndetail c
             //   where a.id=b.parentid and b.matcolldtlid=c.id and b.matreturncode is not null  " + conBy + @" 
             //   union all
             //   select  b.material,b.usedpartsyscode,b.usedpart ,
             //   0 tcslby, nvl(broachQuantity,0) + nvl(repairQty,0)+nvl(discardQty,0)+nvl(rejectQuantity,0)+nvl(lossQty,0)+nvl(consumeQuantity,0)  tcsllj, 0 moneyby,nvl(b.money,0) moneylj
             //   from thd_materialbalancemaster a,thd_materialbalancedetail b,thd_materialreturndetail c
             //   where a.id=b.parentid and b.matcolldtlid=c.id and b.matreturncode is not null   " + conLj + @" 
        }

        /// <summary>
        /// 料具租赁月报 其他费用
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public DataSet WZBB_Ljzlyb_qtfy(string projectId, int fiscalYear, int fiscalMonth, string supplierId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string conBy = " and a.projectid='" + projectId + "' and a.fiscalyear=" + fiscalYear + " and a.fiscalmonth=" + fiscalMonth;
            string conLj = " and a.projectid='" + projectId + "' and a.fiscalyear*100+a.fiscalmonth<=" + (fiscalYear * 100 + fiscalMonth);
            if (!string.IsNullOrEmpty(supplierId))
            {
                conBy += " and a.supplierrelation='" + supplierId + "'";
                conLj += " and a.supplierrelation='" + supplierId + "'";
            }
            string sql = @"
                select costType,sum(fyjeby) fyjeby,sum(fyjelj) fyjelj
                from (
                select costType,sum(costMoney) fyjeby,0 fyjelj
                from thd_materialbalancemaster a,thd_matbalothercostdetail b where a.id=b.parentid " + conBy + @" group by costType
                union all
                select costType,0 fyjeby,sum(costMoney) fyjelj
                from thd_materialbalancemaster a,thd_matbalothercostdetail b where a.id=b.parentid " + conLj + @" group by costType
                union all
                select N'调整费用' costType,sum(nvl(otherMoney,0)) fyjeby,0 fyjelj
                from thd_materialbalancemaster a where 1=1 " + conBy + @" 
                union all
                select N'调整费用' costType,0 fyjeby,sum(nvl(otherMoney,0)) fyjelj
                from thd_materialbalancemaster a where 1=1 " + conLj + @" 
                ) group by costtype
                ";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        public DataSet WZBB_Ljzlyb_UseParts(string projectId, int fiscalYear, int fiscalMonth, string supplierId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
           
            string conLj = " and a.projectid='" + projectId + "' and a.fiscalyear*100+a.fiscalmonth<=" + (fiscalYear * 100 + fiscalMonth);
            if (!string.IsNullOrEmpty(supplierId))
            {
               
                conLj += " and a.supplierrelation='" + supplierId + "'";
            }
            string sql = @"
                select distinct b.usedpart,b.usedpartname,b.usedpartsyscode
                from thd_materialbalancemaster a,thd_matbalothercostdetail b where a.id=b.parentid " + conLj ;
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        /// <summary>
        /// 料具租赁报表 消耗明细
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <param name="supplierId"></param>
        /// <param name="usedPartSysCode"></param>
        /// <returns></returns>
        public DataSet WZBB_Ljzlyb_xhmx(string projectId, int fiscalYear, int fiscalMonth, string supplierId, string usedPartSysCode)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string conBy = " and a.projectid='" + projectId + "' and a.fiscalyear=" + fiscalYear + " and a.fiscalmonth=" + fiscalMonth + " and instr('" + usedPartSysCode + "',b.usedpart)>0";
            if (!string.IsNullOrEmpty(supplierId))
            {
                conBy += " and a.supplierrelation='" + supplierId + "'";
            }
            string sql = @"
                select to_char(b.material) material,b.materialname,sum(b.money) money                
                from thd_materialbalancemaster a,thd_materialbalancedetail b where a.id=b.parentid and b.usedpart is not null " + conBy + @" 
                group by b.material,b.materialname
                union all
                select to_char(costType),null,sum(costMoney) fyjeby
                from thd_materialbalancemaster a,thd_matbalothercostdetail b where a.id=b.parentid and b.usedpart is not null " + conBy + @" group by b.costType
                ";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        #endregion

        #region 其它报表

        /// <summary>
        /// 物资消耗对比表
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="materialCategory"></param>
        /// <returns></returns>
        public DataSet WZBB_Wzxhdbb(string projectId, string beginDate, string endDate, MaterialCategory materialCategory)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string conZjh = " and a.projectid='" + projectId + "'";
            string conBq = " and a.projectid='" + projectId + "' and a.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and a.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            string conLj = " and a.projectid='" + projectId + "' and a.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            string conGcrwhs = " and a.theprojectguid='" + projectId + "' and d.thesyscode like '%.29BioV9QP5T9tJmw1VKARN%' "
                + " and a.createdate>=to_date('" + beginDate + "','yyyy-mm-dd') and a.createdate<to_date('" + endDate + "','yyyy-mm-dd')+1";
            if (materialCategory != null)
            {
                conZjh += " and b.materialcode like '" + materialCategory.Code + "%'";
                conBq += " and b.materialcode like '" + materialCategory.Code + "%'";
                conLj += " and b.materialcode like '" + materialCategory.Code + "%'";
                conGcrwhs += " and d.matcode like '" + materialCategory.Code + "%'";
            }

            string sql = @"
                select material,materialcode,materialname,materialspec,matstandardunitname,
                sum(zjhsl) zjhsl,sum(yjhsl) yjhsl,sum(rjhsl) rjhsl,sum(cgsl) cgsl,sum(cgje) cgje,sum(cgsllj) cgsllj,sum(cgjelj) cgjelj,
                sum(dbrksl) dbrksl,sum(dbrkje) dbrkje,sum(dbrksllj) dbrksllj,sum(dbrkjelj) dbrkjelj,sum(xhsl) xhsl,
                sum(xhje) xhje,sum(xhsllj) xhsllj,sum(xhjelj) xhjelj,sum(htsl) htsl,sum(htje) htje,sum(zrcbsl) zrcbsl,sum(zrcbje) zrcbje,
                sum(zrcbsl)-sum(xhsl) jdcbsl,sum(zrcbje)-sum(xhje) jdcbje
                from(
                /****总计划***/
                select to_char(b.material) material,to_char(b.materialcode) materialcode,to_char(b.materialname) materialname,
                to_char(b.materialspec) materialspec,to_char(b.matstandardunitname) matstandardunitname,
                sum(b.quantity) zjhsl,0 yjhsl,0 rjhsl,0 cgsl,0 cgje,0 cgsllj,0 cgjelj,0 dbrksl,0 dbrkje,0 dbrksllj,0 dbrkjelj,
                0 xhsl,0 xhje,0 xhsllj,0 xhjelj,0 htsl,0 htje,0 zrcbsl,0 zrcbje
                from thd_demandmasterplanmaster a,thd_demandmasterplandetail b 
                where a.id=b.parentid " + conZjh + @" 
                group by b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /***月计划**/
                union all
                select to_char(b.material),to_char(b.materialcode),to_char(b.materialname),to_char(b.materialspec),to_char(b.matstandardunitname),
                0 zjhsl,sum(b.quantity) yjhsl,0 rjhsl,0 cgsl,0 cgje,0 cgsllj,0 cgjelj,0 dbrksl,0 dbrkje,0 dbrksllj,0 dbrkjelj,
                0 xhsl,0 xhje,0 xhsllj,0 xhjelj,0 htsl,0 htje,0 zrcbsl,0 zrcbje
                from thd_monthlyplanmaster a,thd_monthlyplandetail b where a.id=b.parentid " + conBq + @"
                group by b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /***日计划**/
                union all
                select to_char(b.material),to_char(b.materialcode),to_char(b.materialname),to_char(b.materialspec),to_char(b.matstandardunitname),
                0 zjhsl,0 yjhsl,sum(b.quantity) rjhsl,0 cgsl,0 cgje,0 cgsllj,0 cgjelj,0 dbrksl,0 dbrkje,0 dbrksllj,0 dbrkjelj,
                0 xhsl,0 xhje,0 xhsllj,0 xhjelj,0 htsl,0 htje,0 zrcbsl,0 zrcbje
                from thd_dailyPlanmaster a,thd_dailyPlandetail b where a.id=b.parentid " + conBq + @"
                group by b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /***采购数量、金额****/
                union all
                select b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname, 
                0 zjhsl,0 yjhsl,0 rjhsl,sum(b.quantity) cgsl,sum(b.money) cgje,0 cgsllj,0 cgjelj,0 dbrksl,0 dbrkje,0 dbrksllj,0 dbrkjelj,
                0 xhsl,0 xhje,0 xhsllj,0 xhjelj,0 htsl,0 htje,0 zrcbsl,0 zrcbje
                from thd_stkstockin a,thd_stkstockindtl b where a.id=b.parentid and a.istally=1 and a.stockinmanner=10 " + conBq + @"
                group by b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /***累计采购数量、金额****/
                union all
                select b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname, 
                0 zjhsl,0 yjhsl,0 rjhsl,0 cgsl,0 cgje,sum(b.quantity) cgsllj,sum(b.money) cgjelj,0 dbrksl,0 dbrkje,0 dbrksllj,0 dbrkjelj,
                0 xhsl,0 xhje,0 xhsllj,0 xhjelj,0 htsl,0 htje,0 zrcbsl,0 zrcbje
                from thd_stkstockin a,thd_stkstockindtl b where a.id=b.parentid and a.istally=1 and a.stockinmanner=10 " + conLj + @"
                group by b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /***调拨入库数量、金额****/
                union all
                select b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname, 
                0 zjhsl,0 yjhsl,0 rjhsl,0 cgsl,0 cgje,0 cgsllj,0 cgjelj,sum(b.quantity) dbrksl,sum(b.money) dbrkje,0 dbrksllj,0 dbrkjelj,
                0 xhsl,0 xhje,0 xhsllj,0 xhjelj,0 htsl,0 htje,0 zrcbsl,0 zrcbje
                from thd_stkstockin a,thd_stkstockindtl b where a.id=b.parentid and a.istally=1 and a.stockinmanner=11 " + conBq + @"
                group by b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /***累计调拨入库数量、金额****/
                union all
                select b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname, 
                0 zjhsl,0 yjhsl,0 rjhsl,0 cgsl,0 cgje,0 cgsllj,0 cgjelj,0 dbrksl,0 dbrkje,sum(b.quantity) dbrksllj,sum(b.money) dbrkjelj,
                0 xhsl,0 xhje,0 xhsllj,0 xhjelj,0 htsl,0 htje,0 zrcbsl,0 zrcbje
                from thd_stkstockin a,thd_stkstockindtl b where a.id=b.parentid and a.istally=1 and a.stockinmanner=11 " + conLj + @"
                group by b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /***领料出库数量、金额****/
                union all
                select b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname, 
                0 zjhsl,0 yjhsl,0 rjhsl,0 cgsl,0 cgje,0 cgsllj,0 cgjelj,0 dbrksl,0 dbrkje,0 dbrksllj,0 dbrkjelj,
                sum(b.quantity) xhsl,sum(b.money) xhje,0 xhsllj,0 xhjelj,0 htsl,0 htje,0 zrcbsl,0 zrcbje
                from thd_stkstockOut a,thd_stkstockOutdtl b where a.id=b.parentid and a.istally=1 and a.stockOutManner=20 " + conBq + @"
                group by b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /***领料出库累计数量、金额****/
                union all
                select b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname, 
                0 zjhsl,0 yjhsl,0 rjhsl,0 cgsl,0 cgje,0 cgsllj,0 cgjelj,0 dbrksl,0 dbrkje,0 dbrksllj,0 dbrkjelj,
                0 xhsl,0 xhje,sum(b.quantity) xhsllj,sum(b.money) xhjelj,0 htsl,0 htje,0 zrcbsl,0 zrcbje
                from thd_stkstockOut a,thd_stkstockOutdtl b where a.id=b.parentid and a.istally=1 and a.stockOutManner=20 " + conLj + @"
                group by b.material,b.materialcode,b.materialname,b.materialspec,b.matstandardunitname
                /***合同数量、金额 责任成本数量、金额***/
                union all
                select d.materialid,to_char(d.matcode),to_char(d.matname),to_char(d.matspecification),to_char(b.quantityunitname),
                0 zjhsl,0 yjhsl,0 rjhsl,0 cgsl,0 cgje,0 cgsllj,0 cgjelj,0 dbrksl,0 dbrkje,0 dbrksllj,0 dbrkjelj,
                0 xhsl,0 xhje,0 xhsllj,0 xhjelj,sum(b.rescontractquantity) htsl,sum(b.contractincometotal) htje,
                sum(b.responsiblequantity) zrcbsl,sum(b.responsibleusagetotal) zrcbje
                from thd_projecttaskaccountbill a,thd_projecttaskdtlacctsubject b,thd_projecttaskdetailaccount c,resmaterial d
                where a.id=c.parentid and c.id=b.parentid and b.resourcetypeguid=d.materialid and a.state=5 " + conGcrwhs + @"
                group by d.materialid,d.matcode,d.matname,d.matspecification,b.quantityunitname
                ) group by material,materialcode,materialname,materialspec,matstandardunitname order by materialName,materialspec
            ";

            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }
        #endregion
        /// <summary>
        /// 判断该结算是否核算 如果true表示结算已经在商务那边核算了 false表示结算在商务那边还没核算了
        /// </summary>
        /// <param name="sStockInOutID">结算ID</param>
        /// <returns></returns>
        public bool IsAccounted(string sStockInOutID)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sSQL = "select count(*)  from thd_materialsettlemaster t  join thd_stkstockinout t1 on t.monthlysettlment=t1.id where  t.monthaccountbill is not null and t1.id='{0}'  ";
            sSQL = string.Format(sSQL, sStockInOutID);
            command.CommandText = sSQL;
            int iCount = int.Parse(command.ExecuteScalar().ToString());
            return iCount > 0 ? true : false;
        }
    }
}
