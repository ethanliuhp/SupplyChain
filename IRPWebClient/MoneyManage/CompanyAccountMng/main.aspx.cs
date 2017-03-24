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

public partial class MoneyManage_CompanyAccountMng_main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sCompanyType = Request.QueryString["CompanyType"];
            try
            {
                CompanyAccountType = (EnumCompanyAccountMng)Enum.Parse(typeof(EnumCompanyAccountMng), sCompanyType);
            }
            catch
            {
                CompanyAccountType = EnumCompanyAccountMng.公司财务账面维护;
            }
            
        }
    }
    public EnumCompanyAccountMng CompanyAccountType
    {
        get { return (EnumCompanyAccountMng)Enum.Parse(typeof(EnumCompanyAccountMng), hdCompanyType.Value); }
        set { hdCompanyType.Value = (value).ToString(); }
    }
}
public enum EnumCompanyAccountMng
{
    公司财务账面维护=0,
    分公司财务账面维护= 1
}
