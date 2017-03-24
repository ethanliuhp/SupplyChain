using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using NPOI.SS.UserModel;

public partial class test_menu_detail : System.Web.UI.Page
{
    IndirectCostMaster CurMaster = new IndirectCostMaster();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ModelToView();
        }
    }
    public void btnClick_Click(object sender, EventArgs e)
    {

    }
    public void ModelToView()
    {
        this.gvManageCostSource.DestControl = this.gvManageCost;
        this.gvManageCostSource.DataSource = CurMaster.Details;
        this.gvManageCostSource.DataBind();
    }
    public void btnImportExcel_Click(object sender, EventArgs e)
    {
        string sPathFile = this.txtFilePath.Text;
        if (!string.IsNullOrEmpty(sPathFile))
        {

            OpenExcel(sPathFile);

            ModelToView();
        }
    }
  
    private void OpenExcel(string strFileName)
    {

        //IList<IndirectCostDetail> lstDetail = null;
        try
        {
            string sFlag = string.Empty;
            string sAccountTitleCode = string.Empty;
            string sOrgName = string.Empty;
            string sMoney = string.Empty;
            double dMoney = 0;
            IRow oRow = null;
            IList lstAccount = GlobalClass.AccountTitleTreeSvr.GetAccountTitleTreeByInstance();
            using (ExcelHelper oExcelHelper = new ExcelHelper(strFileName))
            {
                ISheet oSheet = oExcelHelper.GetSheet("");
                //lstDetail = new List<IndirectCostDetail>();
                IndirectCostDetail oDetail = null;
                for (int iStartRow = 10; iStartRow <= oSheet.LastRowNum; iStartRow++)
                {
                    oRow = oSheet.GetRow(iStartRow);
                    sFlag = oRow.GetCell(0).StringCellValue.Trim();
                    if (string.IsNullOrEmpty(sFlag))
                    {
                        oDetail = new IndirectCostDetail();
                        oDetail.Master = CurMaster;
                        oDetail.Id = Guid.NewGuid().ToString();
                        oDetail.CostType = EnumCostType.管理费用;
                        oDetail.AccountSymbol = EnumAccountSymbol.其他;
                        sAccountTitleCode = oRow.GetCell(1).StringCellValue;
                        if (!string.IsNullOrEmpty(sAccountTitleCode) && sAccountTitleCode.IndexOf('\\') > 0)
                        {
                            sAccountTitleCode = sAccountTitleCode.Substring(0, sAccountTitleCode.IndexOf("\\"));
                            oDetail.AccountTitle = GetAccountTitleTree(lstAccount, sAccountTitleCode);
                            if (oDetail.AccountTitle != null)
                            {
                                oDetail.AccountTitleCode = oDetail.AccountTitle.Code;
                                oDetail.AccountTitleID = oDetail.AccountTitle.Id;
                                oDetail.AccountTitleName = oDetail.AccountTitle.Name;
                                oDetail.AccountTitleSyscode = oDetail.AccountTitle.SysCode;
                            }
                        }
                        else
                        {
                            sAccountTitleCode = string.Empty;
                        }

                        sOrgName = oRow.GetCell(3).StringCellValue.Trim();
                        dMoney = oRow.GetCell(6).NumericCellValue;
                        oDetail.AccountTitleCode = sAccountTitleCode;
                        oDetail.OrgInfoName = sOrgName;
                        oDetail.Money = (decimal)dMoney;
                        // lstDetail.Add(oDetail);
                        CurMaster.Details.Add(oDetail);
                        this.gvManageCostSource.NewBillList.Add(oDetail);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        catch
        {

        }
        //return lstDetail;
    }
    public AccountTitleTree GetAccountTitleTree(IList lstAccountTitle, string sCode)
    {
        AccountTitleTree oAccountTitleTree = null;
        foreach (AccountTitleTree oAccount in lstAccountTitle)
        {
            if (string.Equals(oAccount.Code, sCode))
            {
                oAccountTitleTree = oAccount;
                break;
            }
        }
        return oAccountTitleTree;
    }
}
