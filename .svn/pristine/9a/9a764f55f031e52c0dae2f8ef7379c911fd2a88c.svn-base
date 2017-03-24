using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Collections;


public partial class Resource_ShowPerson : System.Web.UI.Page
{
    public bool IsSingle = true;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            GetQuery();
            Query("");
            SelectMethod(IsSingle);
            ClearPersonID();
        }
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
                IsSingle = false;
            }
        }

        catch
        {
            IsSingle = false;
        }
    }
  
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string sCode = this.txtCode.Text.Trim();
        string sName = this.txtName.Text.Trim();
        string sWhere = string.Empty;
        sWhere = string.IsNullOrEmpty(sCode) ? "   1=1 " : string.Format("     t.PERCODE like '%{0}%' ", sCode);
        sWhere += string.IsNullOrEmpty(sName) ? " and  1=1 " : string.Format(" and t.perNAME like '%{0}%' ", sName);
        Query(sWhere);
        GridViewSource_person.GetData();
        ClearPersonID();
    }
    public void Query(string sWhere)
    {
 

       
        GridViewSource_person.DataSQL = "select   rownum num,t.* from resperson t " ;
 

        GridViewSource_person.DestControl = gvPerson;
        GridViewSource_person.PageSize = 10;
        GridViewSource_person.CurrentPage = 1;
        GridViewSource_person.DataBind();
    }
    public string GetPersonTypeWhere()
    {
        string sWhere = string.Empty;
        string sValue = this.hdPersonType.Value;
        if (!string.IsNullOrEmpty(sValue))
        {
            string[] arr = sValue.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                if (!string.IsNullOrEmpty(arr[i]))
                {
                    if (i != 0)
                    {
                        sWhere += " or ";
                    }
                    sWhere += string.Format(" t.PersonType={0} ", arr[i]);
                }
            }
            if (arr.Length > 1)
            {
                sWhere = " (" + sWhere + ")";
            }

        }

        string sTemp = string.Empty;
        sValue = hdTeachType.Value;
        if (!string.IsNullOrEmpty(sValue))
        {
            string[] arr = sValue.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                if (!string.IsNullOrEmpty(arr[i]))
                {
                    if (i != 0)
                    {
                        sTemp += " or ";
                    }
                    sTemp += string.Format(" t.TeachType={0} ", arr[i]);
                }
            }
            if (arr.Length > 1)
            {
                sTemp = " (" + sWhere + ")";
            }

        }
        if (!string.IsNullOrEmpty(sTemp))
        {
            if (!string.IsNullOrEmpty(sWhere))
            {
                sWhere += " and " + sTemp;
            }
            else
            {
                sWhere = sTemp;
            }
        }
        return sWhere;
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
        ObjectQuery oQuery = new ObjectQuery();
        oQuery.AddCriterion(Expression.In("Id", arrIDs));
        IList lst = ResourceSvr.PersonManager.GetPerson(typeof(StandardPerson), oQuery);
        sJosn = UtilClass.ObjectToJson(lst);
        this.hdJosn.Value = string.IsNullOrEmpty(sJosn) ? string.Empty : sJosn;
        UtilClass.ExecuteScript(this, "Sure();");
        //string sMsg = string.Format(" <script>  Close(\"{0}\"); </script> ", this.hdJosn.Value);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "232", sMsg);
    }
   

}
