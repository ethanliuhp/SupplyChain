using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinControls.Controls;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public class FundPlanFlexGridOperate
    {
        string id;
        string parentId;
        int currentRowNumber;
        int costType;
        string quota;
        decimal subtotalQuota;
        Boolean dataRow;//判断是数据行，还是其他(小计/总计行)
        string moneyType;//费用类型：计划支出
        decimal allTypeTotalSumMoeny;
        CustomFlexGrid flexGrid;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        public int CurrentRowNumber
        {
            get { return currentRowNumber; }
            set { currentRowNumber = value; }
        }

        public int CostType
        {
            get { return costType; }
            set { costType = value; }
        }

        public Boolean DataRow
        {
            get { return dataRow; }
            set { dataRow = value; }
        }

        public string Quota
        {
            get { return quota; }
            set { quota = value; }
        }

        public decimal SubtotalQuota
        {
            get { return subtotalQuota; }
            set { subtotalQuota = value; }
        }

        public decimal AllTypeTotalSumMoeny
        {
            get { return allTypeTotalSumMoeny; }
            set { allTypeTotalSumMoeny = value; }
        }

        public string MoneyType
        {
            get { return moneyType; }
            set { moneyType = value; }
        }

        public CustomFlexGrid FlexGrid
        {
            get { return flexGrid; }
            set { flexGrid = value; }
        }

        public FundPlanFlexGridOperate(string id,string parentId ,int currentRowNumber,Boolean dataRow, CustomFlexGrid flexGrid)
        {
            this.id = id;
            this.parentId = parentId;
            this.currentRowNumber = currentRowNumber;
            this.dataRow = dataRow;
            this.flexGrid = flexGrid;
        }

        public FundPlanFlexGridOperate(int costType, decimal subtotalQuota, CustomFlexGrid flexGrid)
        {
            this.costType = costType;
            this.subtotalQuota = subtotalQuota;
            this.flexGrid = flexGrid;
        }

        public FundPlanFlexGridOperate(string id, int currentRowNumber, decimal subtotalQuota, Boolean dataRow, CustomFlexGrid flexGrid)
        {
            this.id = id;
            this.currentRowNumber = currentRowNumber;
            this.subtotalQuota = subtotalQuota;
            this.dataRow = dataRow;
            this.flexGrid = flexGrid;
        }

        public FundPlanFlexGridOperate(int currentRowNumber, decimal subtotalQuota, Boolean dataRow, CustomFlexGrid flexGrid)
        {
            this.currentRowNumber = currentRowNumber;
            this.subtotalQuota = subtotalQuota;
            this.dataRow = dataRow;
            this.flexGrid = flexGrid;
        }

        public FundPlanFlexGridOperate(string moneyType, decimal allTypeTotalSumMoeny, Boolean dataRow, CustomFlexGrid flexGrid)
        {
            this.moneyType = moneyType;
            this.allTypeTotalSumMoeny = allTypeTotalSumMoeny;
            this.dataRow = dataRow;
            this.flexGrid = flexGrid;
        }
    }
}
