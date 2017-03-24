using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using CommonSearchLib.BillCodeMng.Service;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.ConcreteManage.PouringNoteMng.Domain;
using NHibernate.Criterion;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.ConcreteManage.PumpingPoundsMng.Domain;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteCheckingMng.Domain;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteBalanceMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Service;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Service;

namespace Application.Business.Erp.SupplyChain.ConcreteManage.Service
{
    /// <summary>
    /// 商品砼管理服务
    /// </summary>
    public class ConcreteManSrv : BaseService, IConcreteManSrv
    {
        #region 注入服务
        private IStockInSrv refStockInSrv;

        public IStockInSrv RefStockInSrv
        {
            get { return refStockInSrv; }
            set { refStockInSrv = value; }
        }

        private IStockOutSrv refStockOutSrv;
        virtual public IStockOutSrv RefStockOutSrv
        {
            get { return refStockOutSrv; }
            set { refStockOutSrv = value; }
        }
        private IStockMoveSrv refStockMoveSrv;
        virtual public IStockMoveSrv RefStockMoveSrv
        {
            get { return refStockMoveSrv; }
            set { refStockMoveSrv = value; }
        }
        #endregion

        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        private string GetCode(Type type,string projectId)
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

        #region 商品砼浇筑记录单方法
        [TransManager]
        public PouringNoteMaster SavePouringNoteMaster(PouringNoteMaster obj, IList moveDtlList)
        {
            obj.LastModifyDate = DateTime.Now;

            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(PouringNoteMaster), obj.ProjectId);
                obj = SaveByDao(obj) as PouringNoteMaster;
                //新增时修改前驱单据引用数量
                foreach (PouringNoteDetail dtl in obj.Details)
                {
                    DailyPlanDetail forwardDtl = GetDailyPlanDetail(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                    dao.SaveOrUpdate(forwardDtl);
                }
                obj.RealOperationDate = DateTime.Now;
            }
            else
            {
                
                obj = SaveOrUpdateByDao(obj) as PouringNoteMaster;
                foreach (PouringNoteDetail dtl in obj.Details)
                {
                    DailyPlanDetail forwardDtl = GetDailyPlanDetail(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                    }
                    else
                    {
                        //修改时修改前续单据的引用数量为 引用数量+当前浇筑数量与上次浇筑数量的差值
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(TransUtil.ToDecimal(dtl.TempData));
                    }
                    dao.SaveOrUpdate(forwardDtl);
                }
                //修改时对于删除的明细 删除引用数量
                foreach (PouringNoteDetail dtl in moveDtlList)
                {
                    DailyPlanDetail forwardDtl = GetDailyPlanDetail(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.SaveOrUpdate(forwardDtl);
                }
            }
            if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
            {
                obj.SubmitDate = DateTime.Now;
            }
            CreateStockInOut(obj);
            return obj;
        }
        [TransManager]
        public void CreateStockInOut(PouringNoteMaster oPouringNoteMaster)
        {

            if (oPouringNoteMaster != null)
            {
                #region"调入"
                StockMoveIn oStockMoveIn = CreateStockMoveIn(oPouringNoteMaster);
                if (oStockMoveIn != null)
                {
                    oStockMoveIn = refStockMoveSrv.SaveStockMoveIn1(oStockMoveIn, new ArrayList());
                }
                #endregion

                #region"调出"
                StockMoveOut oStockMoveOut = CreateStockMoveOut(oPouringNoteMaster);
                if (oStockMoveOut != null)
                {
                    oStockMoveOut = refStockMoveSrv.SaveStockMoveOut1(oStockMoveOut);
                }
                #endregion

                #region"领料"
                StockOut oStockOut = CreateStockOut(oPouringNoteMaster);
                if (oStockOut != null)
                {
                    oStockOut = refStockOutSrv.SaveStockOut1(oStockOut);
                }
                #endregion
            }
        }
        /// <summary>
        /// 由浇筑记录单产生调拨入库单
        /// </summary>
        /// <param name="oPouringNoteMaster"></param>
        /// <returns></returns>
        public StockMoveIn CreateStockMoveIn(PouringNoteMaster oPouringNoteMaster)
        {
            StockMoveIn oStockMoveIn = null;
            StockMoveInDtl oStockMoveInDtl = null;
            decimal sumImportMoney=0;
            decimal sumImportQty=0;
            DateTime mainDate = new DateTime();
            if (!string.IsNullOrEmpty(oPouringNoteMaster.InportSupplierName ))
            {
                oStockMoveIn = new StockMoveIn();
                foreach (PouringNoteDetail oPouringNoteDetail in oPouringNoteMaster.Details)
                {
                    if (oPouringNoteDetail.ImportQty != 0  )
                    {
                        oStockMoveInDtl = new StockMoveInDtl();
                        sumImportMoney += oPouringNoteDetail.ImportMoney;
                        sumImportQty += oPouringNoteDetail.ImportQty;
                        oStockMoveInDtl.ConfirmMoney = oPouringNoteDetail.ImportMoney;
                        oStockMoveInDtl.ConfirmPrice = oPouringNoteDetail.ImportPrice;
                        oStockMoveInDtl.Descript = oPouringNoteMaster.Descript + "【浇筑记录单产生】";
                        oStockMoveInDtl.DiagramNumber = oPouringNoteDetail.DiagramNumber;
                        oStockMoveInDtl.ForwardDetailId = oPouringNoteDetail.Id;
                        oStockMoveInDtl.MaterialCode = oPouringNoteDetail.MaterialCode;
                        oStockMoveInDtl.MaterialName = oPouringNoteDetail.MaterialName;
                        oStockMoveInDtl.MaterialResource = oPouringNoteDetail.MaterialResource;
                        oStockMoveInDtl.MaterialSpec = oPouringNoteDetail.MaterialSpec;
                        oStockMoveInDtl.MaterialStuff = oPouringNoteDetail.MaterialStuff;
                        oStockMoveInDtl.MaterialSysCode = oPouringNoteDetail.MaterialSysCode;
                        oStockMoveInDtl.MatStandardUnit = oPouringNoteDetail.MatStandardUnit;
                        oStockMoveInDtl.MatStandardUnitName = oPouringNoteDetail.MatStandardUnitName;
                        oStockMoveInDtl.Money = oPouringNoteDetail.ImportMoney;
                        oStockMoveInDtl.Price = oPouringNoteDetail.ImportPrice;
                        oStockMoveInDtl.ProfessionalCategory = "";
                        oStockMoveInDtl.Quantity = oPouringNoteDetail.ImportQty;
                        oStockMoveInDtl.RefQuantity = oPouringNoteDetail.ImportQty;
                        oStockMoveInDtl.UsedPart = oPouringNoteDetail.UsedPart;
                        oStockMoveInDtl.UsedPartName = oPouringNoteDetail.UsedPartName;
                        oStockMoveInDtl.UsedPartSysCode = oPouringNoteDetail.UsedPartSysCode;
                        mainDate = oPouringNoteDetail.PouringDate;
                        oStockMoveIn.AddDetail(oStockMoveInDtl);
                        oStockMoveInDtl.Master = oStockMoveIn;
                    }
                }
                if (oStockMoveIn.Details.Count > 0)
                {
                    oStockMoveIn.AuditDate = oPouringNoteMaster.AuditDate;
                    oStockMoveIn.AuditMonth = oPouringNoteMaster.AuditMonth;
                    oStockMoveIn.AuditPerson = oPouringNoteMaster.AuditPerson;
                    oStockMoveIn.AuditPersonName = oPouringNoteMaster.AuditPersonName;
                    oStockMoveIn.AuditRoles = oPouringNoteMaster.AuditRoles;
                    oStockMoveIn.Audits = oPouringNoteMaster.Audits;
                    oStockMoveIn.AuditYear = oPouringNoteMaster.AuditYear;
                    oStockMoveIn.CreateDate = mainDate;
                    oStockMoveIn.CreateMonth = oPouringNoteMaster.CreateMonth;
                    oStockMoveIn.CreatePerson = oPouringNoteMaster.CreatePerson;
                    oStockMoveIn.CreatePersonName = oPouringNoteMaster.CreatePersonName;
                    oStockMoveIn.CreateYear = oPouringNoteMaster.CreateYear;
                    oStockMoveIn.Descript = oPouringNoteMaster.Descript + "【浇筑记录单产生】";
                    oStockMoveIn.DocState = oPouringNoteMaster.DocState;
                    oStockMoveIn.MoveOutProjectName = oPouringNoteMaster.InportSupplierName;
                    oStockMoveIn.OperOrgInfo = oPouringNoteMaster.OperOrgInfo;
                    oStockMoveIn.OperOrgInfoName = oPouringNoteMaster.OperOrgInfoName;
                    oStockMoveIn.OpgSysCode = oPouringNoteMaster.OpgSysCode;
                    oStockMoveIn.Special = "土建";
                    oStockMoveIn.ProjectId = oPouringNoteMaster.ProjectId;
                    oStockMoveIn.ProjectName = oPouringNoteMaster.ProjectName;
                    oStockMoveIn.ForwardBillCode = oPouringNoteMaster.Code;
                    oStockMoveIn.ForwardBillId = oPouringNoteMaster.Id;
                    oStockMoveIn.RealOperationDate = oPouringNoteMaster.RealOperationDate;
                    oStockMoveIn.StockInManner = EnumStockInOutManner.调拨入库;
                    oStockMoveIn.SubmitDate = oPouringNoteMaster.SubmitDate;
                    oStockMoveIn.SumConfirmMoney = sumImportMoney;
                    oStockMoveIn.SumMoney = sumImportMoney;
                    oStockMoveIn.SumQuantity = sumImportQty;
                    oStockMoveIn.TheStockInOutKind = 11;
                    oStockMoveIn.TheSupplierName = oPouringNoteMaster.InportSupplierName;
                    oStockMoveIn.MatCatName = "砼及外加剂";
                    oStockMoveIn.IsTally = 1;
                }
                else
                {
                    oStockMoveIn = null;
                }
            }
            return oStockMoveIn;
        }
        /// <summary>
        /// 由浇筑记录单调拨出库单
        /// </summary>
        /// <param name="oPouringNoteMaster"></param>
        /// <returns></returns>
        public StockMoveOut CreateStockMoveOut(PouringNoteMaster oPouringNoteMaster)
        {
            StockMoveOut oStockMoveOut = null;
            StockMoveOutDtl oStockMoveOutDtl = null;
            decimal sumImportMoney = 0;
            decimal sumImportQty = 0;
            DateTime mainDate = new DateTime();
            if (!string.IsNullOrEmpty(oPouringNoteMaster.ExportSupplierName)   )
            {
                oStockMoveOut = new  StockMoveOut();
                foreach (PouringNoteDetail oPouringNoteDetail in oPouringNoteMaster.Details)
                {
                    if (oPouringNoteDetail.ExportQty != 0  )
                    {
                        oStockMoveOutDtl = new  StockMoveOutDtl();
                        sumImportMoney += oPouringNoteDetail.ExportMoney;
                        sumImportQty += oPouringNoteDetail.ExportQty;
                        oStockMoveOutDtl.ConfirmMoney = oPouringNoteDetail.ExportMoney;
                        oStockMoveOutDtl.ConfirmPrice = oPouringNoteDetail.ExportQty;
                        oStockMoveOutDtl.Descript = oPouringNoteMaster.Descript+"【浇筑记录单产生】";
                        oStockMoveOutDtl.DiagramNumber = oPouringNoteDetail.DiagramNumber;
                        oStockMoveOutDtl.ForwardDetailId = oPouringNoteDetail.Id;
                        oStockMoveOutDtl.MaterialCode = oPouringNoteDetail.MaterialCode;
                        oStockMoveOutDtl.MaterialName = oPouringNoteDetail.MaterialName;
                        oStockMoveOutDtl.MaterialResource = oPouringNoteDetail.MaterialResource;
                        oStockMoveOutDtl.MaterialSpec = oPouringNoteDetail.MaterialSpec;
                        oStockMoveOutDtl.MaterialStuff = oPouringNoteDetail.MaterialStuff;
                        oStockMoveOutDtl.MaterialSysCode = oPouringNoteDetail.MaterialSysCode;
                        oStockMoveOutDtl.MatStandardUnit = oPouringNoteDetail.MatStandardUnit;
                        oStockMoveOutDtl.MatStandardUnitName = oPouringNoteDetail.MatStandardUnitName;
                        oStockMoveOutDtl.Money = oPouringNoteDetail.ExportMoney;
                        oStockMoveOutDtl.Price = oPouringNoteDetail.ExportPrice;
                        oStockMoveOutDtl.MoveMoney = oPouringNoteDetail.ExportMoney;
                        oStockMoveOutDtl.MovePrice = oPouringNoteDetail.ExportPrice;
                         
                        oStockMoveOutDtl.ProfessionalCategory = "";
                        oStockMoveOutDtl.Quantity = oPouringNoteDetail.ExportQty;
                        oStockMoveOutDtl.RefQuantity = oPouringNoteDetail.ExportQty;
                        oStockMoveOutDtl.UsedPart = oPouringNoteDetail.UsedPart;
                        oStockMoveOutDtl.UsedPartName = oPouringNoteDetail.UsedPartName;
                        oStockMoveOutDtl.UsedPartSysCode = oPouringNoteDetail.UsedPartSysCode;
                        mainDate = oPouringNoteDetail.PouringDate;
                        oStockMoveOut.AddDetail(oStockMoveOutDtl);
                        oStockMoveOutDtl.Master = oStockMoveOut;
                    }
                }
                if (oStockMoveOut.Details.Count > 0)
                {
                    oStockMoveOut.AuditDate = oPouringNoteMaster.AuditDate;
                    oStockMoveOut.AuditMonth = oPouringNoteMaster.AuditMonth;
                    oStockMoveOut.AuditPerson = oPouringNoteMaster.AuditPerson;
                    oStockMoveOut.AuditPersonName = oPouringNoteMaster.AuditPersonName;
                    oStockMoveOut.AuditRoles = oPouringNoteMaster.AuditRoles;
                    oStockMoveOut.Audits = oPouringNoteMaster.Audits;
                    oStockMoveOut.AuditYear = oPouringNoteMaster.AuditYear;
                    oStockMoveOut.CreateDate = mainDate;
                    oStockMoveOut.CreateMonth = oPouringNoteMaster.CreateMonth;
                    oStockMoveOut.CreatePerson = oPouringNoteMaster.CreatePerson;
                    oStockMoveOut.CreatePersonName = oPouringNoteMaster.CreatePersonName;
                    oStockMoveOut.CreateYear = oPouringNoteMaster.CreateYear;
                    oStockMoveOut.Descript = oPouringNoteMaster.Descript + "【浇筑记录单产生】";
                    oStockMoveOut.DocState = oPouringNoteMaster.DocState;
                    oStockMoveOut.MoveInProjectName = oPouringNoteMaster.ExportSupplierName;
                    oStockMoveOut.OperOrgInfo = oPouringNoteMaster.OperOrgInfo;
                    oStockMoveOut.OperOrgInfoName = oPouringNoteMaster.OperOrgInfoName;
                    oStockMoveOut.OpgSysCode = oPouringNoteMaster.OpgSysCode;
                    oStockMoveOut.Special = "土建";
                    oStockMoveOut.ProjectId = oPouringNoteMaster.ProjectId;
                    oStockMoveOut.ProjectName = oPouringNoteMaster.ProjectName;
                    oStockMoveOut.RealOperationDate = oPouringNoteMaster.RealOperationDate;
                    oStockMoveOut.StockOutManner = EnumStockInOutManner.调拨出库;
                    oStockMoveOut.SubmitDate = oPouringNoteMaster.SubmitDate;
                    oStockMoveOut.ForwardBillCode = oPouringNoteMaster.Code;
                    oStockMoveOut.ForwardBillId = oPouringNoteMaster.Id;
                    oStockMoveOut.SumMoney = sumImportMoney;
                    oStockMoveOut.SumQuantity = sumImportQty;
                    oStockMoveOut.TheStockInOutKind = 21;
                    oStockMoveOut.TheSupplierName = oPouringNoteMaster.ExportSupplierName;
                    oStockMoveOut.IsTally = 1;
                    oStockMoveOut.MatCatName = "砼及外加剂";
                }
                else
                {
                    oStockMoveOut = null;
                }
            }
            return oStockMoveOut;
        }
        public StockOut CreateStockOut(PouringNoteMaster oPouringNoteMaster)
        {
            StockOut oStockOut = null;
            StockOutDtl oStockOutDtl = null;
            decimal sumImportMoney = 0;
            decimal sumImportQty = 0;
            decimal money = 0;
            decimal quality = 0;
            decimal price = 0;
            DateTime mainDate = new DateTime();
            if (!string.IsNullOrEmpty(oPouringNoteMaster.ExportSupplierName) || !string.IsNullOrEmpty(oPouringNoteMaster.InportSupplierName))
            {
                oStockOut = new StockOut();
                foreach (PouringNoteDetail oPouringNoteDetail in oPouringNoteMaster.Details)
                {
                    quality = oPouringNoteDetail.ImportQty - oPouringNoteDetail.ExportQty;
                    money = oPouringNoteDetail.ImportMoney - oPouringNoteDetail.ExportMoney;
                    if (quality != 0)
                    {
                        price = money / quality;
                        oStockOutDtl = new StockOutDtl();
                        sumImportMoney += money;
                        sumImportQty += quality;
                        oStockOutDtl.ConfirmMoney = money;
                        oStockOutDtl.ConfirmPrice = price;
                        oStockOutDtl.Descript = oPouringNoteMaster.Descript + "【浇筑记录单产生】"; ;
                        oStockOutDtl.DiagramNumber = oPouringNoteDetail.DiagramNumber;
                        oStockOutDtl.ForwardDetailId = oPouringNoteDetail.Id;
                        oStockOutDtl.MaterialCode = oPouringNoteDetail.MaterialCode;
                        oStockOutDtl.MaterialName = oPouringNoteDetail.MaterialName;
                        oStockOutDtl.MaterialResource = oPouringNoteDetail.MaterialResource;
                        oStockOutDtl.MaterialSpec = oPouringNoteDetail.MaterialSpec;
                        oStockOutDtl.MaterialStuff = oPouringNoteDetail.MaterialStuff;
                        oStockOutDtl.MaterialSysCode = oPouringNoteDetail.MaterialSysCode;
                        oStockOutDtl.MatStandardUnit = oPouringNoteDetail.MatStandardUnit;
                        oStockOutDtl.MatStandardUnitName = oPouringNoteDetail.MatStandardUnitName;
                        oStockOutDtl.Money = money;
                        oStockOutDtl.Price = price;
                        oStockOutDtl.ProfessionalCategory = "";
                        oStockOutDtl.Quantity = quality;
                        oStockOutDtl.RefQuantity = quality;
                        oStockOutDtl.UsedPart = oPouringNoteDetail.UsedPart;
                        oStockOutDtl.UsedPartName = oPouringNoteDetail.UsedPartName;
                        oStockOutDtl.UsedPartSysCode = oPouringNoteDetail.UsedPartSysCode;
                        oStockOutDtl.SubjectGUID = oPouringNoteDetail.SubjectGUID;
                        oStockOutDtl.SubjectName = oPouringNoteDetail.SubjectName;
                        oStockOutDtl.SubjectSysCode = oPouringNoteDetail.SubjectSysCode;
                        mainDate = oPouringNoteDetail.PouringDate;

                        oStockOut.AddDetail(oStockOutDtl);
                        oStockOutDtl.Master = oStockOut;
                    }
                }
                if (oStockOut.Details.Count > 0)
                {
                    oStockOut.AuditDate = oPouringNoteMaster.AuditDate;
                    oStockOut.AuditMonth = oPouringNoteMaster.AuditMonth;
                    oStockOut.AuditPerson = oPouringNoteMaster.AuditPerson;
                    oStockOut.AuditPersonName = oPouringNoteMaster.AuditPersonName;
                    oStockOut.AuditRoles = oPouringNoteMaster.AuditRoles;
                    oStockOut.Audits = oPouringNoteMaster.Audits;
                    oStockOut.AuditYear = oPouringNoteMaster.AuditYear;
                    oStockOut.CreateDate = mainDate;
                    oStockOut.CreateMonth = oPouringNoteMaster.CreateMonth;
                    oStockOut.CreatePerson = oPouringNoteMaster.CreatePerson;
                    oStockOut.CreatePersonName = oPouringNoteMaster.CreatePersonName;
                    oStockOut.CreateYear = oPouringNoteMaster.CreateYear;
                    oStockOut.Descript = oPouringNoteMaster.Descript + "【浇筑记录单产生】"; ;
                    oStockOut.DocState = oPouringNoteMaster.DocState;
                    oStockOut.ForwardBillCode = oPouringNoteMaster.Code;
                    oStockOut.ForwardBillId = oPouringNoteMaster.Id;
                    oStockOut.OperOrgInfo = oPouringNoteMaster.OperOrgInfo;
                    oStockOut.OperOrgInfoName = oPouringNoteMaster.OperOrgInfoName;
                    oStockOut.OpgSysCode = oPouringNoteMaster.OpgSysCode;
                    oStockOut.Special = "土建";
                    oStockOut.ProjectId = oPouringNoteMaster.ProjectId;
                    oStockOut.ProjectName = oPouringNoteMaster.ProjectName;
                    oStockOut.RealOperationDate = oPouringNoteMaster.RealOperationDate;
                    oStockOut.StockOutManner = EnumStockInOutManner.领料出库;
                    oStockOut.SubmitDate = oPouringNoteMaster.SubmitDate;

                    oStockOut.SumMoney = sumImportMoney;
                    oStockOut.SumQuantity = sumImportQty;
                    oStockOut.TheStockInOutKind = 20;
                    oStockOut.TheSupplierName = oPouringNoteMaster.ExportSupplierName;
                    oStockOut.IsTally = 1;
                    oStockOut.MatCatName = "砼及外加剂";
                }
                else
                {
                    oStockOut = null;
                }
            }
            return oStockOut;
        }
        /// <summary>
        /// 删除浇筑记录单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeletePouringNoteMaster(PouringNoteMaster obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (PouringNoteDetail dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    DailyPlanDetail forwardDtl = GetDailyPlanDetail(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.SaveOrUpdate(forwardDtl);
                }
            }
            return dao.Delete(obj);
        }

        [TransManager]
        public DailyPlanDetail GetDailyPlanDetail(string dailyPlanDetailId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", dailyPlanDetailId));
            IList list = dao.ObjectQuery(typeof(DailyPlanDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as DailyPlanDetail;
            }
            return null;
        }

        /// <summary>
        /// 通过ID查询浇筑记录单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PouringNoteMaster GetPouringNoteMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetPouringNoteMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as PouringNoteMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询浇筑记录单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public PouringNoteMaster GetPouringNoteMasterByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetPouringNoteMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as PouringNoteMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询浇筑记录单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetPouringNoteMaster(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.SubjectGUID", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.UsedPart", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(PouringNoteMaster), objectQuery);
        }

        public IList GetPouringNoteDetail(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(PouringNoteDetail), objectQuery);
        }

        /// <summary>
        /// 浇筑记录单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet GetPouringNoteMasterQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t1.State,t1.SupplierRelation,t1.SupplierName,t1.InportSupplier,t1.InportSupplierName,t1.PrintTimes,t1.Descript,
                  t1.ExportSupplier,t1.ExportSupplierName,t2.UsedPartName,t2.IsPump,t2.Quantity,t2.Price,t2.Money, 
                  t2.ImportQty,t2.ImportPrice,t2.ImportMoney,t2.ExportQty,t2.ExportPrice,t2.ExportMoney,t2.ConsumeQty,t2.ConsumePrice,t2.ConsumeMoney,
                  t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.MatStandardUnitName,t2.PouringDate,t2.subjectname
                  FROM THD_PouringNoteMaster t1 left join THD_PouringNoteDetail t2 ON t2.ParentId=t1.Id";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 通过供应商得到泵送费
        /// </summary>
        /// <returns></returns>
        public decimal GetPumpMoneyByContract(string supplyRelId)
        {
            decimal pumpMoney = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = " select t1.pumpmoney From thd_supplyordermaster t1 where t1.supplierrelation='" + supplyRelId + "' ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    pumpMoney = TransUtil.ToDecimal(dataRow["pumpmoney"]);
                }
            }
            return pumpMoney;
        }
        #endregion

        #region 抽磅单方法
        [TransManager]
        public PumpingPoundsMaster SavePumpingPoundsMaster(PumpingPoundsMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(PumpingPoundsMaster), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as PumpingPoundsMaster;
        }

        /// <summary>
        /// 通过ID查询抽磅单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PumpingPoundsMaster GetPumpingPoundsMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetPumpingPoundsMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as PumpingPoundsMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询抽磅单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public PumpingPoundsMaster GetPumpingPoundsMasterByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetPumpingPoundsMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as PumpingPoundsMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询抽磅单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetPumpingPoundsMaster(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("UsedPart", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(PumpingPoundsMaster), objectQuery);
        }

        /// <summary>
        /// 抽磅单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet GetPumpingPoundsMasterQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,Code,STATE,SupplierName,MaterialCode,MaterialName,MaterialSpec,PlateNumber,DiffAmount,NetWeight,t1.Descript,
                         GrossWeight,TareWeight,TicketVolume,TicketWeight,t1.UsedPart,t1.UsedPartName,t1.PouringDate,t1.PrintTimes
                         FROM THD_PumpingPoundsMaster t1 LEFT JOIN THD_PumpingPoundsDetail t2 ON t2.ParentId=t1.Id";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        #endregion

        #region 商品砼对账单
        public ConcreteCheckingMaster SaveConcreteCheck(ConcreteCheckingMaster obj)
        {
            bool value = true;
            if (obj.Id == null)
            {
                value = true;
                obj.RealOperationDate = DateTime.Now;

            }
            else
            {
                value = false;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj = SaveConCheckingMaster(obj);
            if (value == true)
            {
                UpdatePouringNoteDtl(obj);
            }
            return obj;
        }
        [TransManager]
        private ConcreteCheckingMaster SaveConCheckingMaster(ConcreteCheckingMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(ConcreteCheckingMaster), obj.ProjectId);
            }

            obj.RealOperationDate = DateTime.Now;
            obj.LastModifyDate = DateTime.Now;
            obj = SaveOrUpdateByDao(obj) as ConcreteCheckingMaster;
            return obj;
        }
        /// <summary>
        /// 更新浇筑记录明细(回写对账单ID和更新对账状态：0：未结算 1：已结算)
        /// </summary>
        /// <param name="IdStr"></param>
        /// <param name="ConCheckId"></param>
        private void UpdatePouringNoteDtl(ConcreteCheckingMaster obj)
        {
            try
            {
                //更新浇筑记录明细
                string pouringNoteDtlIdStr = "";
                foreach (ConcreteCheckingDetail detail in obj.Details)
                {
                    pouringNoteDtlIdStr = pouringNoteDtlIdStr + detail.TempData + ",";
                }
                //给GUID加上单引号
                pouringNoteDtlIdStr = "'" + pouringNoteDtlIdStr + "'";
                pouringNoteDtlIdStr = pouringNoteDtlIdStr.Replace(",", "','");
                pouringNoteDtlIdStr = pouringNoteDtlIdStr.Substring(0, pouringNoteDtlIdStr.Length - 3);

                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                string sql = "update THD_PouringNoteDetail set ConcreteCheckId='" + obj.Id + "', ConcreteCheckState=" + 1 + " where Id in(" + pouringNoteDtlIdStr + ")";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                DeleteByDao(obj);
                throw;
            }

        }
        /// <summary>
        /// 删除对账单 同时清除该对账单关联的浇筑记录明细的对账单ID
        /// </summary>
        /// <param name="obj"></param>
        public bool DeleteConCheckMaster(ConcreteCheckingMaster obj)
        {
            if (obj.Id == null) return true;
            //清除该对账单关联的浇筑记录明细的对账单ID
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ConcreteCheckId", obj.Id));

            IList lst = GetPouringNoteDetail(oq);
            foreach (PouringNoteDetail detail in lst)
            {
                detail.ConcreteCheckId = null;
                detail.ConcreteCheckState = 0;
                UpdateByDao(detail);
            }
            return DeleteByDao(obj);
        }
        /// <summary>
        /// 通过ID查询商品砼对账单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConcreteCheckingMaster GetConCheckingMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetConCheckingMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConcreteCheckingMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询商品砼对账单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ConcreteCheckingMaster GetConCheckingMasterByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));

            IList list = GetConCheckingMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConcreteCheckingMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询商品砼对账单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetConCheckingMaster(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("CreatePerson", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ConcreteCheckingMaster), objectQuery);
        }

        /// <summary>
        /// 商品砼对账单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet GetConCheckingMasterQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t1.State,t1.SupplierName,t1.CreateDate,t1.CreatePersonName,t1.Descript,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,
                  t2.Price,t2.Money,t2.TicketVolume,t2.DeductionVolume,t2.LessPumpVolume,t2.BalVolume,t2.ConversionVolume,t2.UsedPartName,t2.IsPump,t2.subjectname
                  FROM THD_ConcreteCheckingMaster t1 LEFT JOIN THD_ConcreteCheckingDetail t2 ON t2.ParentId=t1.Id";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        public ConcreteCheckingDetail GetConCkeckingDetailById(string detailId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", detailId));
            IList list = dao.ObjectQuery(typeof(ConcreteCheckingDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConcreteCheckingDetail;
            }
            return null;
        }

        /// <summary>
        /// 查询最后一次对账单的结束日期
        /// </summary>
        /// <returns></returns>
        public string GetLastEndDate(SupplierRelationInfo theSupplier)
        {
            string lastEndDate = "";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "select max(EndDate) as EndDate from THD_ConcreteCheckingMaster where SupplierRelation='" + theSupplier.Id + "'";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            DataTable table = dataSet.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                lastEndDate = row["EndDate"].ToString();
            }
            return lastEndDate;
        }
        #endregion

        #region 商品砼结算单
        [TransManager]
        public ConcreteBalanceMaster SaveConcreteBalanceMaster(ConcreteBalanceMaster obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
          
            if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
            {
                obj.SubmitDate = DateTime.Now;
            }
            if (obj.Id == null)
            {
                obj.RealOperationDate = DateTime.Now;
                obj.Code = GetCode(typeof(ConcreteBalanceMaster), obj.ProjectId);
                obj = SaveByDao(obj) as ConcreteBalanceMaster;
                //新增时修改前续单据的引用数量
                foreach (ConcreteBalanceDetail dtl in obj.Details)
                {
                    ConcreteCheckingDetail ConcreteCheckingDetail = GetConCkeckingDetailById(dtl.ForwardDetailId);
                    ConcreteCheckingDetail.RefQuantity = ConcreteCheckingDetail.RefQuantity + dtl.BalanceVolume;//RefQuery为引用数量

                    dao.Save(ConcreteCheckingDetail);
                }
            }
            else
            {
                obj = SaveOrUpdateByDao(obj) as ConcreteBalanceMaster;
                foreach (ConcreteBalanceDetail dtl in obj.Details)
                {
                    ConcreteCheckingDetail ConcreteCheckingDetail = GetConCkeckingDetailById(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        ConcreteCheckingDetail.RefQuantity = ConcreteCheckingDetail.RefQuantity + dtl.BalanceVolume;
                    }
                    else
                    {
                        ConcreteCheckingDetail.RefQuantity = ConcreteCheckingDetail.RefQuantity + dtl.BalanceVolume - Convert.ToDecimal(dtl.TempData);
                    }
                    dao.Save(ConcreteCheckingDetail);
                }

                //修改时对于删除的明细 删除引用数量
                foreach (ConcreteBalanceDetail dtl in movedDtlList)
                {
                    ConcreteCheckingDetail ConcreteCheckingDetail = GetConCkeckingDetailById(dtl.ForwardDetailId);
                    ConcreteCheckingDetail.RefQuantity = ConcreteCheckingDetail.RefQuantity - dtl.BalanceVolume;
                    dao.Save(ConcreteCheckingDetail);
                }
            }
            //修改结算单明细的计量单位
            UpdateConcreteBalanceDetailUnit();
            if (obj.DocState == DocumentState.InExecute)
            {
                TallyConcreteBalance(obj, obj.ProjectId);
            }
            
            return obj;
        }
        //获取供应商的累计结算金额
        public decimal GetAddSumMoney(string projectID, string supprelid)
        {
            decimal addSumMoney = 0;
            string sSQL = "select sum(t1.summoney) addsummoney from thd_concretebalancemaster t1 where "+
                            " t1.projectid='" + projectID + "' and t1.supplierrelation='" + supprelid + "' and t1.state=5 ";
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = sSQL;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                {
                    addSumMoney = TransUtil.ToDecimal(oDataRow["addsummoney"]);
                }
            }
            return addSumMoney;
        }

        /// <summary>
        /// 更新结算单的累计金额
        /// </summary>
        /// <param name="MasterProperty"></param>
        private void UpdateConcreteBalanceAddMoney(string id,decimal addMoney)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand cmd = cnn.CreateCommand();
            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(cmd);
            string sql = " update thd_concretebalancemaster t1 set t1.state=5 ,t1.addsummoney = " + addMoney + " where t1.id = '" + id + "'";
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 更新结算单明细的计量单位
        /// </summary>
        /// <param name="MasterProperty"></param>
        private void UpdateConcreteBalanceDetailUnit()
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            string sql = "";
            IDbCommand cmd = cnn.CreateCommand();
            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(cmd);
            sql = " update thd_concretebalancedetail t1 set (t1.matstandardunit,t1.matstandardunitname)=( " +
                        " select k2.standunitid,k2.standunitname from resmaterial k1,resstandunit " +
                       " k2 where k1.standardunitid=k2.standunitid     and t1.material=k1.materialid ) where t1.matstandardunit is null";
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 通过ID查询商品砼结算单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConcreteBalanceMaster GetConcreteBalanceMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("Details.SubjectGUID", NHibernate.FetchMode.Eager);
            IList list = GetConcreteBalanceMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConcreteBalanceMaster;
            }
            return null;
        }
        public ConcreteBalanceDetail GetConcreteBalanceDetailByConcreteCheckingDetailID(string ConcreteCheckingDetailID)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq  ("ForwardDetailId",ConcreteCheckingDetailID));
            IList lst = Dao.ObjectQuery(typeof(ConcreteBalanceDetail), oQuery);
            return (lst != null && lst.Count > 0) ? lst[0] as ConcreteBalanceDetail : null;
        }
        /// <summary>
        /// 通过Code查询商品砼结算单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ConcreteBalanceMaster GetConcreteBalanceMasterByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetConcreteBalanceMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConcreteBalanceMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询商品砼结算单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetConcreteBalanceMaster(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("CreatePerson", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ConcreteBalanceMaster), objectQuery);
        }

        /// <summary>
        /// 商品砼结算单删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteConcreteBalance(ConcreteBalanceMaster obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (ConcreteBalanceDetail dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    ConcreteCheckingDetail forwardDtl = GetConCkeckingDetailById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.BalanceVolume);
                    dao.Save(forwardDtl);
                }
            }
            return dao.Delete(obj);
        }
        /// <summary>
        /// 提交结算单后生成一对出入库单，并记账，审批平台调用
        /// </summary>
        [TransManager]
        public void TallyConcreteBalanceByApproval(string billId)
        {
            ConcreteBalanceMaster master = this.GetConcreteBalanceMasterById(billId);
            this.TallyConcreteBalance(master, master.ProjectId);
        }

        /// <summary>
        /// 提交结算单后生成一对出入库单，并记账
        /// </summary>
        /// <param name="obj"></param>
        public void TallyConcreteBalance(ConcreteBalanceMaster obj, string projectId)
        {
            //补入累计结算金额
            decimal addSumMoney = this.GetAddSumMoney(obj.ProjectId, obj.TheSupplierRelationInfo.Id) + obj.SumMoney;
            this.UpdateConcreteBalanceAddMoney(obj.Id, addSumMoney);
            //判断状态是否提交，提交时生成一对出入库单，并且记账
            //生成入库单，并记账
            StockIn theStockIn = CreateStockInByBalance(obj);
            IList lstStockRelation = new ArrayList();
            if (theStockIn != null)
            {
                #region 入库单记账
                theStockIn.AuditDate = obj.CreateDate;
                theStockIn.AuditYear = obj.CreateYear;
                theStockIn.AuditMonth = obj.CreateMonth;
                theStockIn.AuditPersonName = obj.AuditPersonName;
                theStockIn.AuditPerson = obj.AuditPerson;
                theStockIn.SubmitDate = obj.SubmitDate;
                theStockIn.DocState = DocumentState.InExecute;
                theStockIn.IsTally = 1;
                dao.SaveOrUpdate(theStockIn);

                foreach (StockInDtl oStockInDtl in theStockIn.Details)
                {
                    Application.Business.Erp.SupplyChain.StockManage.Stock.Domain.StockRelation oStockRelation = new Application.Business.Erp.SupplyChain.StockManage.Stock.Domain.StockRelation();
                    oStockRelation.AccountMonth = obj.CreateMonth;
                    oStockRelation.AccountYear = obj.CreateYear;
                    oStockRelation.State = 0;
                    oStockRelation.ProjectId = theStockIn.ProjectId;
                    oStockRelation.ProjectName = theStockIn.ProjectName;
                    oStockRelation.OperOrgInfo = theStockIn.OperOrgInfo.Id;
                    oStockRelation.OperOrgInfoName = theStockIn.OperOrgInfoName;
                    oStockRelation.StockInId = theStockIn.Id;
                    oStockRelation.StockInDtlId = oStockInDtl.Id;
                    oStockRelation.Price = oStockInDtl.Price;
                    oStockRelation.Quantity = oStockInDtl.Quantity;
                    oStockRelation.Money = oStockInDtl.Money;
                    oStockRelation.RemainMoney = oStockInDtl.Money; ;
                    oStockRelation.RemainQuantity = oStockInDtl.Quantity;
                    oStockRelation.Material = oStockInDtl.MaterialResource.Id;
                    oStockRelation.MaterialName = oStockInDtl.MaterialName;

                    oStockRelation.MaterialCode = oStockInDtl.MaterialCode;
                    oStockRelation.MaterialSpec = oStockInDtl.MaterialSpec;
                    oStockRelation.Special = theStockIn.Special;
                    oStockRelation.ProfessionCategory = theStockIn.ProfessionCategory;
                    oStockRelation.SupplierRelationInfo = theStockIn.TheSupplierRelationInfo;
                    oStockRelation.SupplierName = theStockIn.TheSupplierName;
                    oStockRelation.MaterialStuff = oStockInDtl.MaterialStuff;
                    oStockRelation.Confirmprice = oStockInDtl.ConfirmPrice;
                    lstStockRelation.Add(oStockRelation);
                }
                dao.SaveOrUpdate(lstStockRelation);
                #endregion
            }
            //生成出库单，并记账
            StockOut theStockOut = CreateStockOutByBalance(obj);
            theStockOut.IsTally = 1;
            #region 出库单
            theStockOut.AuditDate = obj.CreateDate;
            theStockOut.AuditYear = obj.CreateYear;
            theStockOut.AuditMonth = obj.CreateMonth;
            theStockOut.AuditPersonName = obj.AuditPersonName;
            theStockOut.AuditPerson = obj.AuditPerson;
            theStockOut.SubmitDate = DateTime.Now;
            theStockOut.DocState = DocumentState.InExecute;
            theStockOut.IsTally = 1;
            dao.SaveOrUpdate(theStockOut);
            IList lstStkStockOutDtlSeq = new ArrayList();
            foreach (StockRelation oStockRelation in lstStockRelation)
            {
                StockOutDtlSeq oStockOutDtlSeq = new StockOutDtlSeq();
                //StockInDtl oStockInDtl = GetStockInDtl( oStockRelation,theStockIn);
                StockOutDtl oStockOutDtl = GetStockOutDtl(oStockRelation, theStockOut);
                oStockOutDtlSeq.StockOutDtl = oStockOutDtl;
                //oStockOutDtlSeq.StockInDtlId = oStockInDtl.Id ;
                oStockOutDtlSeq.StockInDtlId = oStockRelation.StockInDtlId;
                oStockOutDtlSeq.Price = oStockRelation.Price;
                oStockOutDtlSeq.Quantity = oStockRelation.Quantity;
                oStockOutDtlSeq.RemainQuantity = oStockRelation.RemainQuantity;
                oStockOutDtlSeq.CreateDate = DateTime.Now;
                oStockOutDtlSeq.SeqCreateDate = oStockRelation.SeqCreateDate;
                oStockOutDtlSeq.StockRelId = oStockRelation.Id;
                lstStkStockOutDtlSeq.Add(oStockOutDtlSeq);
            }
            dao.SaveOrUpdate(lstStkStockOutDtlSeq);
            for (int i = 0; i < lstStockRelation.Count; i++)
            {
                StockRelation oStockRelation = lstStockRelation[i] as StockRelation;
                if (oStockRelation != null)
                {
                    oStockRelation.RemainQuantity = 0;
                    oStockRelation.RemainMoney = 0;
                    oStockRelation.IdleQuantity = 0;
                    dao.SaveOrUpdate(oStockRelation);
                }
            }
            #endregion
        }
        public StockInDtl GetStockInDtl(StockRelation oStockRelation, StockIn oStockIn)
        {
            StockInDtl oStockInDtl = null;
            foreach (StockInDtl obj in oStockIn.Details)
            {
                if (oStockRelation.Material == obj.MaterialResource.Id)
                {
                    oStockInDtl = obj;
                }
            }
            return oStockInDtl;
        }
        public StockOutDtl GetStockOutDtl(StockRelation oStockRelation, StockOut oStockOut)
        {
            StockOutDtl oStockOutDtl = null;
            foreach (StockOutDtl obj in oStockOut.Details)
            {
                if (oStockRelation.Material == obj.MaterialResource.Id)
                {
                    oStockOutDtl = obj;
                }
            }
            return oStockOutDtl;
        }
        public string GetDetialID(StockIn oStockIn)
        {
            string sID = string.Empty;
            foreach (StockInDtl oStockInDtl in oStockIn.Details)
            {
                if (string.IsNullOrEmpty(sID))
                {
                    sID = oStockInDtl.Id;
                }
                else
                {
                    sID = "," + oStockInDtl.Id;
                }
            }
            return sID;
        }
        /// <summary>
        /// 由结算单生成入库单
        /// </summary>
        /// <param name="obj"></param>
        private StockIn CreateStockInByBalance(ConcreteBalanceMaster obj)
        {
            StockIn theStockIn = new StockIn();
            //入库主表信息
            theStockIn.Code = GetCode(typeof(StockIn), obj.ProjectId, "商品砼");
            theStockIn.CreateDate = obj.CreateDate;
            theStockIn.CreateYear = obj.CreateYear;
            theStockIn.CreateMonth = obj.CreateMonth;
            theStockIn.CreatePerson = obj.CreatePerson;
            theStockIn.CreatePersonName = obj.CreatePersonName;
            theStockIn.OperOrgInfo = obj.OperOrgInfo;
            theStockIn.OperOrgInfoName = obj.OperOrgInfoName;
            theStockIn.OpgSysCode = obj.OpgSysCode;
            theStockIn.TheSupplierRelationInfo = obj.TheSupplierRelationInfo;
            theStockIn.TheSupplierName = obj.SupplierName;
            theStockIn.DocState = obj.DocState;
            theStockIn.IsTally = 1;
            theStockIn.HandlePerson = obj.HandlePerson;
            theStockIn.HandlePersonName = obj.HandlePersonName;
            theStockIn.ProjectId = obj.ProjectId;
            theStockIn.ProjectName = obj.ProjectName;
            theStockIn.RealOperationDate = obj.RealOperationDate;
            theStockIn.MatCatName = "砼及外加剂";
            theStockIn.TheStationCategory = refStockInSrv.GetStationCategory(obj.ProjectId);
            theStockIn.Descript = "商品砼结算单生成入库单";
            theStockIn.Special = TransUtil.ConSpecialTJ;

            theStockIn.StockInManner = EnumStockInOutManner.收料入库;
            theStockIn.SumMoney = obj.SumMoney;
            theStockIn.SumQuantity = obj.SumVolumeQuantity;
            theStockIn.ConcreteBalID = obj.Id;

            //入库明细信息
            foreach (ConcreteBalanceDetail theBalDetail in obj.Details)
            {
                StockInDtl theStockInDtl = new StockInDtl();
                theStockInDtl.MaterialResource = theBalDetail.MaterialResource;
                theStockInDtl.MaterialCode = theBalDetail.MaterialCode;
                theStockInDtl.MaterialName = theBalDetail.MaterialName;
                theStockInDtl.MaterialSpec = theBalDetail.MaterialSpec;
                theStockInDtl.MatStandardUnit = theBalDetail.MatStandardUnit;
                theStockInDtl.MatStandardUnitName = theBalDetail.MatStandardUnitName;
                theStockInDtl.Money = theBalDetail.Money;
                theStockInDtl.Price = theBalDetail.Price;
                theStockInDtl.Quantity = theBalDetail.BalanceVolume;
                theStockInDtl.BalQuantity = theStockInDtl.Quantity;
                theStockInDtl.ConcreteBalDtlID = theBalDetail.Id;
                theStockInDtl.UsedPart = theBalDetail.UsedPart;
                theStockInDtl.UsedPartName = theBalDetail.UsedPartName;
                theStockIn.AddDetail(theStockInDtl);
            }
            return SaveByDao(theStockIn) as StockIn;
        }

        /// <summary>
        /// 由结算单生成出库单
        /// </summary>
        /// <param name="obj"></param>
        private StockOut CreateStockOutByBalance(ConcreteBalanceMaster obj)
        {
            StockOut theStockOut = new StockOut();
            //出库主表信息
            theStockOut.Code = GetCode(typeof(StockOut), obj.ProjectId, "商品砼");
            theStockOut.CreateDate = obj.CreateDate;
            theStockOut.CreateYear = obj.CreateYear;
            theStockOut.CreateMonth = obj.CreateMonth;
            theStockOut.CreatePerson = obj.CreatePerson;
            theStockOut.CreatePersonName = obj.CreatePersonName;
            theStockOut.OperOrgInfo = obj.OperOrgInfo;
            theStockOut.OperOrgInfoName = obj.OperOrgInfoName;
            theStockOut.OpgSysCode = obj.OpgSysCode;
            theStockOut.TheSupplierRelationInfo = obj.TheSupplierRelationInfo;
            theStockOut.TheSupplierName = obj.SupplierName;
            theStockOut.DocState = obj.DocState;
            theStockOut.IsTally = 1;
            theStockOut.HandlePerson = obj.HandlePerson;
            theStockOut.HandlePersonName = obj.HandlePersonName;
            theStockOut.ProjectId = obj.ProjectId;
            theStockOut.ProjectName = obj.ProjectName;
            theStockOut.RealOperationDate = obj.RealOperationDate;
            theStockOut.TheStationCategory = refStockInSrv.GetStationCategory(obj.ProjectId);
            theStockOut.Descript = "商品砼结算单生成出库单";
            theStockOut.Special = TransUtil.ConSpecialTJ ;
            theStockOut.MatCatName = "砼及外加剂";
            theStockOut.StockOutManner = EnumStockInOutManner.领料出库;
            theStockOut.SumQuantity = obj.SumVolumeQuantity;
            theStockOut.SumMoney = obj.SumMoney;
            theStockOut.ConcreteBalID = obj.Id;

            //出库明细信息
            foreach (ConcreteBalanceDetail theBalDetail in obj.Details)
            {
                StockOutDtl theStockOutDtl = new StockOutDtl();
                theStockOutDtl.MaterialResource = theBalDetail.MaterialResource;
                theStockOutDtl.MaterialCode = theBalDetail.MaterialCode;
                theStockOutDtl.MaterialName = theBalDetail.MaterialName;
                theStockOutDtl.MaterialSpec = theBalDetail.MaterialSpec;
                theStockOutDtl.MatStandardUnit = theBalDetail.MatStandardUnit;
                theStockOutDtl.MatStandardUnitName = theBalDetail.MatStandardUnitName;
                theStockOutDtl.Money = theBalDetail.Money;
                theStockOutDtl.Price = theBalDetail.Price;
                theStockOutDtl.Quantity = theBalDetail.BalanceVolume;
                theStockOutDtl.ConcreteBalDtlID = theBalDetail.Id;

                theStockOutDtl.UsedPart = theBalDetail.UsedPart;
                theStockOutDtl.UsedPartName = theBalDetail.UsedPartName;
                theStockOutDtl.UsedPartSysCode = theBalDetail.UsedPartSysCode;
                theStockOutDtl.SubjectGUID = theBalDetail.SubjectGUID;
                theStockOutDtl.SubjectName = theBalDetail.SubjectName;
                theStockOutDtl.SubjectSysCode = theBalDetail.SubjectSysCode;



                theStockOut.AddDetail(theStockOutDtl);
            }

            return SaveByDao(theStockOut) as StockOut;
        }

        public DataSet GetConereteBalanceByQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t1.State,t1.createdate,t1.realoperationdate,t1.createpersonname,
                  t1.SupplierName,t1.SumVolumeQuantity,t1.SumMoney,t2.MaterialCode,t2.MaterialName,
                  t2.MaterialSpec,t2.UsedPartName,t2.CheckingVolume,t2.BalanceVolume,t2.IsPump,t2.Money,t2.Price
                  FROM THD_ConcreteBalanceMaster t1 LEFT JOIN THD_ConcreteBalanceDetail t2 ON t2.ParentId=t1.Id";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        #endregion

        #region 商品砼结算红单

        [TransManager]
        public ConcreteBalanceRedMaster SaveConcreteBalanceRedMaster(ConcreteBalanceRedMaster obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(ConcreteBalanceRedMaster), obj.ProjectId);
                obj = SaveByDao(obj) as ConcreteBalanceRedMaster;
                //新增时修改前续单据的引用数量
                foreach (ConcreteBalanceRedDetail dtl in obj.Details)
                {
                    ConcreteBalanceDetail ConcreteBalanceDetail = GetConBalDetailById(dtl.ForwardDetailId);
                    ConcreteBalanceDetail.RefQuantity = ConcreteBalanceDetail.RefQuantity + Math.Abs(dtl.BalanceVolume);//RefQuery为引用数量

                    dao.Save(ConcreteBalanceDetail);
                }
            }
            else
            {
                obj = SaveOrUpdateByDao(obj) as ConcreteBalanceRedMaster;
                foreach (ConcreteBalanceRedDetail dtl in obj.Details)
                {
                    ConcreteBalanceDetail ConcreteBalanceDetail = GetConBalDetailById(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        ConcreteBalanceDetail.RefQuantity = ConcreteBalanceDetail.RefQuantity + Math.Abs(dtl.BalanceVolume);
                    }
                    else
                    {
                        ConcreteBalanceDetail.RefQuantity = ConcreteBalanceDetail.RefQuantity + Math.Abs(dtl.BalanceVolume) - Math.Abs(Convert.ToDecimal(dtl.TempData));
                    }
                    dao.Save(ConcreteBalanceDetail);
                }

                //修改时对于删除的明细 删除引用数量
                foreach (ConcreteBalanceRedDetail dtl in movedDtlList)
                {
                    ConcreteBalanceDetail ConcreteBalanceDetail = GetConBalDetailById(dtl.ForwardDetailId);
                    ConcreteBalanceDetail.RefQuantity = ConcreteBalanceDetail.RefQuantity - Math.Abs(dtl.BalanceVolume);
                    dao.Save(ConcreteBalanceDetail);
                }
            }
            return obj;
        }

        /// <summary>
        /// 商品砼结算单删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteConcreteBalanceRed(ConcreteBalanceRedMaster obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (ConcreteBalanceRedDetail dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    ConcreteBalanceDetail forwardDtl = GetConBalDetailById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.BalanceVolume);
                    dao.Save(forwardDtl);
                }
            }
            return dao.Delete(obj);
        }

        /// <summary>
        /// 通过ID查询商品砼结算红单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConcreteBalanceRedMaster GetConcreteBalanceRedMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetConcreteBalanceRedMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConcreteBalanceRedMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询商品砼结算红单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ConcreteBalanceRedMaster GetConcreteBalanceRedMasterByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetConcreteBalanceRedMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConcreteBalanceRedMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询商品砼结算红单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetConcreteBalanceRedMaster(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("CreatePerson", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ConcreteBalanceRedMaster), objectQuery);
        }

        public ConcreteBalanceDetail GetConBalDetailById(string detailId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", detailId));
            IList list = dao.ObjectQuery(typeof(ConcreteBalanceDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConcreteBalanceDetail;
            }
            return null;
        }

        /// <summary>
        /// 提交结算单后生成一对出入库单，并记账
        /// </summary>
        /// <param name="obj"></param>
        public void TallyConcreteBalanceRed(ConcreteBalanceRedMaster obj, string projectId)
        {
            //判断状态是否提交，提交时生成一对出入库红单，并且记账
            if (obj.DocState == DocumentState.InAudit)
            {
                try
                {
                    //生成入库单，并记账
                    StockInRed theStockInRed = CreateStockInRedByBalance(obj);
                    if (theStockInRed != null)
                    {
                        IList billIdList = new ArrayList();
                        billIdList.Add(theStockInRed.Id);

                        IList billCodeList = new ArrayList();
                        billCodeList.Add(theStockInRed.Code);

                        Hashtable hashBillId = new Hashtable();
                        hashBillId.Add("StockInRed", billIdList);

                        Hashtable hashBillCode = new Hashtable();
                        hashBillCode.Add("StockInRed", billCodeList);

                        refStockInSrv.TallyStockIn(hashBillId, hashBillCode, theStockInRed.CreateYear, theStockInRed.CreateMonth, theStockInRed.CreatePerson.ToString(), theStockInRed.CreatePersonName, projectId);
                    }
                    //生成出库单，并记账
                    StockOutRed theStockOutRed = CreateStockOutRedByBalance(obj);
                    if (theStockOutRed != null)
                    {
                        IList billIdList = new ArrayList();
                        billIdList.Add(theStockOutRed.Id);

                        IList billCodeList = new ArrayList();
                        billCodeList.Add(theStockOutRed.Code);

                        Hashtable hashBillId = new Hashtable();
                        hashBillId.Add("StockOutRed", billIdList);

                        Hashtable hashBillCode = new Hashtable();
                        hashBillCode.Add("StockOutRed", billCodeList);

                        refStockOutSrv.TallyStockOut(hashBillId, hashBillCode, theStockOutRed.CreateYear, theStockOutRed.CreateMonth, theStockOutRed.CreatePerson.ToString(), theStockOutRed.CreatePersonName, projectId);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 由结算单生成入库单
        /// </summary>
        /// <param name="obj"></param>
        private StockInRed CreateStockInRedByBalance(ConcreteBalanceRedMaster obj)
        {
            StockInRed theStockInRed = new StockInRed();
            //入库主表信息
            theStockInRed.Code = GetCode(typeof(StockIn), obj.ProjectId, "商品砼");
            theStockInRed.CreateDate = obj.CreateDate;
            theStockInRed.CreateYear = obj.CreateYear;
            theStockInRed.CreateMonth = obj.CreateMonth;
            theStockInRed.CreatePerson = obj.CreatePerson;
            theStockInRed.CreatePersonName = obj.CreatePersonName;
            theStockInRed.OperOrgInfo = obj.OperOrgInfo;
            theStockInRed.OperOrgInfoName = obj.OperOrgInfoName;
            theStockInRed.OpgSysCode = obj.OpgSysCode;
            theStockInRed.TheSupplierRelationInfo = obj.TheSupplierRelationInfo;
            theStockInRed.TheSupplierName = obj.SupplierName;
            theStockInRed.DocState = obj.DocState;
            theStockInRed.HandlePerson = obj.HandlePerson;
            theStockInRed.HandlePersonName = obj.HandlePersonName;
            theStockInRed.ProjectId = obj.ProjectId;
            theStockInRed.ProjectName = obj.ProjectName;
            theStockInRed.RealOperationDate = obj.RealOperationDate;
            theStockInRed.TheStationCategory = refStockInSrv.GetStationCategory(obj.ProjectId);
            theStockInRed.Descript = "商品砼结算单生成入库红单";


            theStockInRed.StockInManner = EnumStockInOutManner.收料入库;
            theStockInRed.SumMoney = obj.SumMoney;
            theStockInRed.SumQuantity = obj.SumVolumeQuantity;
            theStockInRed.ConcreteBalID = obj.Id;

            //入库明细信息
            foreach (ConcreteBalanceRedDetail theBalDetail in obj.Details)
            {
                StockInRedDtl theStockInRedDtl = new StockInRedDtl();
                theStockInRedDtl.MaterialResource = theBalDetail.MaterialResource;
                theStockInRedDtl.MaterialCode = theBalDetail.MaterialCode;
                theStockInRedDtl.MaterialName = theBalDetail.MaterialName;
                theStockInRedDtl.MaterialSpec = theBalDetail.MaterialSpec;
                theStockInRedDtl.MatStandardUnit = theBalDetail.MatStandardUnit;
                theStockInRedDtl.MatStandardUnitName = theBalDetail.MatStandardUnitName;
                theStockInRedDtl.Money = theBalDetail.Money;
                theStockInRedDtl.Price = theBalDetail.Price;
                theStockInRedDtl.Quantity = theBalDetail.BalanceVolume;
                theStockInRedDtl.ConcreteBalDtlID = theBalDetail.Id;
                theStockInRed.AddDetail(theStockInRedDtl);
            }
            return SaveByDao(theStockInRed) as StockInRed;
        }

        /// <summary>
        /// 由结算单生成出库单
        /// </summary>
        /// <param name="obj"></param>
        private StockOutRed CreateStockOutRedByBalance(ConcreteBalanceRedMaster obj)
        {
            StockOutRed theStockOutRed = new StockOutRed();
            //出库主表信息
            theStockOutRed.Code = GetCode(typeof(StockOut), obj.ProjectId, "商品砼");
            theStockOutRed.CreateDate = obj.CreateDate;
            theStockOutRed.CreateYear = obj.CreateYear;
            theStockOutRed.CreateMonth = obj.CreateMonth;
            theStockOutRed.CreatePerson = obj.CreatePerson;
            theStockOutRed.CreatePersonName = obj.CreatePersonName;
            theStockOutRed.OperOrgInfo = obj.OperOrgInfo;
            theStockOutRed.OperOrgInfoName = obj.OperOrgInfoName;
            theStockOutRed.OpgSysCode = obj.OpgSysCode;
            theStockOutRed.TheSupplierRelationInfo = obj.TheSupplierRelationInfo;
            theStockOutRed.TheSupplierName = obj.SupplierName;
            theStockOutRed.DocState = obj.DocState;
            theStockOutRed.HandlePerson = obj.HandlePerson;
            theStockOutRed.HandlePersonName = obj.HandlePersonName;
            theStockOutRed.ProjectId = obj.ProjectId;
            theStockOutRed.ProjectName = obj.ProjectName;
            theStockOutRed.RealOperationDate = obj.RealOperationDate;
            theStockOutRed.TheStationCategory = refStockInSrv.GetStationCategory(obj.ProjectId);
            theStockOutRed.Descript = "商品砼结算单生成出库红单";

            theStockOutRed.StockOutManner = EnumStockInOutManner.领料出库;
            theStockOutRed.SumQuantity = obj.SumVolumeQuantity;
            theStockOutRed.SumMoney = obj.SumMoney;
            theStockOutRed.ConcreteBalID = obj.Id;

            //出库明细信息
            foreach (ConcreteBalanceRedDetail theBalDetail in obj.Details)
            {
                StockOutRedDtl theStockOutRedDtl = new StockOutRedDtl();
                theStockOutRedDtl.MaterialResource = theBalDetail.MaterialResource;
                theStockOutRedDtl.MaterialCode = theBalDetail.MaterialCode;
                theStockOutRedDtl.MaterialName = theBalDetail.MaterialName;
                theStockOutRedDtl.MaterialSpec = theBalDetail.MaterialSpec;
                theStockOutRedDtl.MatStandardUnit = theBalDetail.MatStandardUnit;
                theStockOutRedDtl.MatStandardUnitName = theBalDetail.MatStandardUnitName;
                theStockOutRedDtl.Money = theBalDetail.Money;
                theStockOutRedDtl.Price = theBalDetail.Price;
                theStockOutRedDtl.Quantity = theBalDetail.BalanceVolume;
                theStockOutRedDtl.ConcreteBalDtlID = theBalDetail.Id;
                theStockOutRed.AddDetail(theStockOutRedDtl);
            }

            return SaveByDao(theStockOutRed) as StockOutRed;
        }

        #endregion

        #region 会计期
        public IList GetFiscalInfo(DateTime createDate)
        {
            IList list = new ArrayList();
            string sSQL = "select t1.begindate,t1.enddate from resfiscalperioddet t1 where t1.begindate<=to_date('" + createDate.ToShortDateString() + "','yyyy-mm-dd') " +
                            " and t1.enddate>=to_date('" + createDate.ToShortDateString()+ "','yyyy-mm-dd') ";
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sSQL;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                {
                    list.Add(TransUtil.ToDateTime(oDataRow["begindate"]));
                    list.Add(TransUtil.ToDateTime(oDataRow["enddate"]));
                }
            }
            return list;
        }
        #endregion
    }
}
