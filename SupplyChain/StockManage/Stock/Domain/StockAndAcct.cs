using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Domain
{
    /// <summary>
    /// 财务存货盘点
    /// </summary>
    [Serializable]
    public class StockAndAcct
    {
        private long id = 0;
        private int kjn = 0;
        private int kjy = 0;
        private long stockRelId = 0;//库存关系ID
        private string ckCode = "";//仓库编码
        private string ckName = "";//仓库名称
        private long ckId = 0;//仓库ID
        private string supplierName = "";//供应商
        private long supplierId = 0;
        private long materialId = 0;
        private string materialCode = "";
        private string materialName = "";
        private string materialStuff = "";
        private string materialSpec = "";
        private string factory = "";//厂家
        private string contractNo = "";//采购合同号

        private decimal firstRealStock = 0;//期初库存
        private decimal firstAcctStock = 0;//期初帐存
        private decimal lastRealStock = 0;//期末库存
        private decimal lastAcctStock = 0;//期末帐存
        private decimal stockInQty = 0;//入库数
        private decimal stockMoveInQty = 0;//转仓入库
        private decimal invoiceAuditQty = 0;//发票已审
        private decimal invoiceNoAuditQty = 0;//发票未审
        private decimal lastAuditInvoiceQty = 0;//已审前期发票
        private decimal lastNoAuditInvoiceQty = 0;//未审前期发票
        private decimal stockOutQty = 0;//出库数
        private decimal stockMoveOutQty = 0;//转仓出库
        private decimal balAuditQty = 0;//已审核结算数
        private decimal balNoAuditQty = 0;//未审核结算数
        private decimal lastAuditBalQty = 0;//已审前期结算数
        private decimal lastNoAuditBalQty = 0;//未审前期结算数

        private string stockInMxIdStr = "";//入库明细联接
        private string invoiceAuditMxIdStr = "";//入库已审发票明细联接
        private string invoiceNoAuditMxIdStr = "";//入库未审发票明细联接
        private string stockOutMxIdStr = "";//出库明细联接
        private string balAuditMxIdStr = "";//结算已审明细联接
        private string balNoAuditMxIdStr = "";//结算未审明细联接

        private decimal stockProfitQty = 0;//盘赢数量
        private decimal stockLossQty = 0;//盘亏数量
        private string stockProfitMxIdStr = "";//盘赢明细联接
        private string stockLossMxIdStr = "";//盘亏明细联接

        virtual public long Id
        {
            get { return id; }
            set { id = value; }
        }

        virtual public int Kjn
        {
            get { return kjn; }
            set { kjn = value; }
        }

        virtual public int Kjy
        {
            get { return kjy; }
            set { kjy = value; }
        }

        virtual public long StockRelId
        {
            get { return stockRelId; }
            set { stockRelId = value; }
        }

        virtual public long CkId
        {
            get { return ckId; }
            set { ckId = value; }
        }

        virtual public string CkCode
        {
            get { return ckCode; }
            set { ckCode = value; }
        }

        virtual public string CkName
        {
            get { return ckName; }
            set { ckName = value; }
        }

        virtual public string Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        virtual public string ContractNo
        {
            get { return contractNo; }
            set { contractNo = value; }
        }

        /// <summary>
        /// 供应商ID
        /// </summary>
        virtual public long SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }

        /// <summary>
        /// 物料Id
        /// </summary>
        virtual public long MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        virtual public string MaterialCode
        {
            get { return materialCode; }
            set { materialCode = value; }
        }

        /// <summary>
        /// 物料名称
        /// </summary>
        virtual public string MaterialName
        {
            get { return materialName; }
            set { materialName = value; }
        }

        /// <summary>
        /// 物料材质
        /// </summary>
        virtual public string MaterialStuff
        {
            get { return materialStuff; }
            set { materialStuff = value; }
        }

        /// <summary>
        /// 物料规格
        /// </summary>
        virtual public string MaterialSpec
        {
            get { return materialSpec; }
            set { materialSpec = value; }
        }

        virtual public decimal FirstRealStock
        {
            get { return firstRealStock; }
            set { firstRealStock = value; }
        }

        virtual public decimal FirstAcctStock
        {
            get { return firstAcctStock; }
            set { firstAcctStock = value; }
        }

        virtual public decimal LastRealStock
        {
            get { return lastRealStock; }
            set { lastRealStock = value; }
        }

        virtual public decimal LastAcctStock
        {
            get { return lastAcctStock; }
            set { lastAcctStock = value; }
        }

        virtual public decimal StockInQty
        {
            get { return stockInQty; }
            set { stockInQty = value; }
        }

        virtual public decimal StockMoveInQty
        {
            get { return stockMoveInQty; }
            set { stockMoveInQty = value; }
        }

        virtual public decimal InvoiceAuditQty
        {
            get { return invoiceAuditQty; }
            set { invoiceAuditQty = value; }
        }

        virtual public decimal InvoiceNoAuditQty
        {
            get { return invoiceNoAuditQty; }
            set { invoiceNoAuditQty = value; }
        }

        virtual public decimal LastAuditInvoiceQty
        {
            get { return lastAuditInvoiceQty; }
            set { lastAuditInvoiceQty = value; }
        }

        virtual public decimal LastNoAuditInvoiceQty
        {
            get { return lastNoAuditInvoiceQty; }
            set { lastNoAuditInvoiceQty = value; }
        }

        virtual public decimal StockOutQty
        {
            get { return stockOutQty; }
            set { stockOutQty = value; }
        }

        virtual public decimal StockMoveOutQty
        {
            get { return stockMoveOutQty; }
            set { stockMoveOutQty = value; }
        }

        virtual public decimal BalAuditQty
        {
            get { return balAuditQty; }
            set { balAuditQty = value; }
        }

        virtual public decimal BalNoAuditQty
        {
            get { return balNoAuditQty; }
            set { balNoAuditQty = value; }
        }

        virtual public decimal LastAuditBalQty
        {
            get { return lastAuditBalQty; }
            set { lastAuditBalQty = value; }
        }

        virtual public decimal LastNoAuditBalQty
        {
            get { return lastNoAuditBalQty; }
            set { lastNoAuditBalQty = value; }
        }

        virtual public string StockInMxIdStr
        {
            get { return stockInMxIdStr; }
            set { stockInMxIdStr = value; }
        }

        virtual public string InvoiceAuditMxIdStr
        {
            get { return invoiceAuditMxIdStr; }
            set { invoiceAuditMxIdStr = value; }
        }

        virtual public string InvoiceNoAuditMxIdStr
        {
            get { return invoiceNoAuditMxIdStr; }
            set { invoiceNoAuditMxIdStr = value; }
        }

        virtual public string StockOutMxIdStr
        {
            get { return stockOutMxIdStr; }
            set { stockOutMxIdStr = value; }
        }

        virtual public string BalAuditMxIdStr
        {
            get { return balAuditMxIdStr; }
            set { balAuditMxIdStr = value; }
        }

        virtual public string BalNoAuditMxIdStr
        {
            get { return balNoAuditMxIdStr; }
            set { balNoAuditMxIdStr = value; }
        }

        virtual public decimal StockProfitQty
        {
            get { return stockProfitQty; }
            set { stockProfitQty = value; }
        }

        virtual public decimal StockLossQty
        {
            get { return stockLossQty; }
            set { stockLossQty = value; }
        }

        virtual public string StockProfitMxIdStr
        {
            get { return stockProfitMxIdStr; }
            set { stockProfitMxIdStr = value; }
        }

        virtual public string StockLossMxIdStr
        {
            get { return stockLossMxIdStr; }
            set { stockLossMxIdStr = value; }
        }
    }
}
