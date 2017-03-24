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
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using System.Collections.Generic;

public partial class MoneyManage_FactoringDataDetail : System.Web.UI.Page
{
    IFinanceMultDataSrv service = GlobalClass.FinanceMultDataSrv;

    private List<FactoringDataDetail> _listData;
    /// <summary> 
    /// 单据详情列表
    /// </summary>
    public List<FactoringDataDetail> ListData
    {
        get
        {
            if (_listData == null)
            {
                _listData = GetData();
            }
            return _listData;
        }
        set
        {
            _listData = value;
        }
    }
    private string code;
    public string Code
    {
        get
        {
            if (code == null)
            {
                code = Request["id"];
            }
            return code;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 获取单据详情
    /// </summary>
    /// <returns></returns>
    private List<FactoringDataDetail> GetData()
    {
        string id = Request["id"];

        var model = service.GetFactoringDataById(id);
        var detail = model.Details.Select(a => (FactoringDataDetail)a).ToList();

        return detail;
    }
}
