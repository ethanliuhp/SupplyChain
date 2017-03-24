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
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Core;
using NHibernate.Criterion;

public partial class Show_ShowSupply : System.Web.UI.Page
{
    public bool IsSingle = true;
    public string _SQL = "select rownum num, t.suprelid ,t.code,t1.orgname,t.address,t.linkman  from ressupplierrelation t join resorganization t1 on t.orgid=t1.orgid where 1=1 ";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IntialData();
            GetQuery();
            Query("");
            SelectMethod(IsSingle);
            ClearPersonID();
        }
        
    }
    public void IntialData()
    {
        string sSupplyCateSQL = "select t.catid id,t.catname name from RESSUPRELCATEGORY t where t.catnodetype=2 order by t.orderno asc";
        DataTable oTable = GlobalClass.CommonMethodSrv.GetData(sSupplyCateSQL).Tables[0];
        DataRow oRow = oTable.NewRow();
        oRow["name"] = "-所有-";
        oRow["id"] = "";
        oTable.Rows.InsertAt(oRow, 0);
        dpLstCata.Items.Clear();
        dpLstCata.DataSource = oTable;
        dpLstCata.DataTextField = "name";
        dpLstCata.DataValueField = "id";
        dpLstCata.DataBind();
    }
    public void GetQuery()
    {
        try
        {
            string sValue = Request.QueryString["IsSingle"];
            if (!string.IsNullOrEmpty(sValue))
            {
                IsSingle = bool.Parse(sValue);
            }
            else
            {
                IsSingle = true;
            }
        }

        catch
        {
            IsSingle = true;
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //t.supplierrelationcategoryid='' and t.code='' and t1.orgname like ''
        string sCode = this.txtCode.Text.Trim();
        string sName = this.txtName.Text.Trim();
        string sCate=this.dpLstCata.SelectedValue;
        string sWhere = string.Empty;
        sWhere += string.IsNullOrEmpty(sCode) ? "" : string.Format(" and t.code like '%{0}%' ", sCode);
        sWhere += string.IsNullOrEmpty(sName) ? "" : string.Format(" and t1.orgname like '%{0}%' ", sName);
        sWhere += string.IsNullOrEmpty(sCate) ? "" : string.Format(" and t.supplierrelationcategoryid='{0}' ", sCate);
        Query(sWhere);
        GridViewSource_person.GetData();
        ClearPersonID();
    }
    public void Query(string sWhere)
    {
        GridViewSource_person.DataSQL = _SQL+sWhere;
        GridViewSource_person.DestControl = gvPerson;
        GridViewSource_person.PageSize = 10;
        GridViewSource_person.CurrentPage = 1;
        GridViewSource_person.DataBind();
    }
    
    public void ClearPersonID()
    {
        this.hdPersonID.Value = string.Empty;

    }
    /// <summary>
    /// true是选择一个
    /// </summary>
    /// <param name="isSingle"></param>
    public void SelectMethod(bool isSingle)
    {
        this.hdSelectMethod.Value = (isSingle ? 1 : 0).ToString();
    }
    protected void btnSure_Click(object sender, EventArgs e)
    {
        string sIDs = this.hdPersonID.Value.Trim();
        string sMethod = this.hdSelectMethod.Value;
        string sJosn = string.Empty;
        string[] arrIDs = sIDs.Split(',');
        sIDs = string.Empty;
        for (int i = 0; i < arrIDs.Length; i++)
        {
            sIDs += string.IsNullOrEmpty(sIDs) ? string.Format("'{0}'", arrIDs[i]) : string.Format(",'{0}'", arrIDs[i]);
        }
        string sSQL = string.Format("select t.*,t1.orgname name from ressupplierrelation t ,resorganization t1 where t.orgid=t1.orgid and t.suprelid in ({0})", sIDs);
        DataTable oTable = GlobalClass.CommonMethodSrv.GetData(sSQL).Tables[0];
        sJosn = UtilClass.DataTableToJosn(oTable);
        this.hdJosn.Value = string.IsNullOrEmpty(sJosn) ? string.Empty : sJosn;
        UtilClass.ExecuteScript(this, "Sure();");
        //string sMsg = string.Format(" <script>  Close(\"{0}\"); </script> ", this.hdJosn.Value);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "232", sMsg);
    }


}
