using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using Microsoft.JScript;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
/// <summary>
/// Summary description for UtilClass
/// </summary>
public class UtilClass
{
    public UtilClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static void MessageBox(System.Web.UI.Page obj, string sMessage)
    {
        string sMsg = string.Empty;
        if (!string.IsNullOrEmpty(sMessage))
        {
            sMessage=UtilClass.EncodeURIComponent(sMessage);
            sMsg = string.Format("<script>showMessage(decodeComponent('{0}'));</script>", sMessage);
            obj.ClientScript.RegisterStartupScript(obj.GetType(), "", sMsg);
        }
    }
    public static void ExecuteScript(Page obj, string strScript)
    {
        if (!string.IsNullOrEmpty(strScript))
        {
            obj.ClientScript.RegisterStartupScript(obj.GetType(), obj.GetType().ToString(), strScript, true);
        }
    }
    /// <summary>
    /// 将行转化为Json格式
    /// </summary>
    /// <param name="oRow"></param>
    /// <returns></returns>
    public static string DataRowToJosn(DataRow oRow)
    {
        StringBuilder oBuilder = new StringBuilder();
        string sName = string.Empty;
        string sValue = string.Empty;
        object objValue = null;
        if (oRow != null)
        {
            oBuilder.Append("[{");
            foreach (DataColumn oColumn in oRow.Table.Columns)
            {
                sName = oColumn.ColumnName;
                objValue = oRow[sName];
                sValue = objValue == DBNull.Value ? "" : objValue.ToString();
                if (oBuilder.Length > 2)
                {
                    oBuilder.Append(",");
                }
                oBuilder.AppendFormat("{0}:'{1}'", sName.ToUpper(), EncodeURIComponent(sValue));
            }
            oBuilder.Append("}]");
        }
        return oBuilder.ToString();
    }
    /// <summary>
    /// 将表转化为Json格式
    /// </summary>
    /// <param name="oRow"></param>
    /// <returns></returns>
    public static string DataTableToJosn(DataTable oTable)
    {
        StringBuilder oBuilder = new StringBuilder();
        string sName = string.Empty;
        string sValue = string.Empty;
        object objValue = null;
        if (oTable != null && oTable.Rows.Count > 0)
        {
            oBuilder.Append("[");
            foreach (DataRow oRow in oTable.Rows)
            {
                if (oBuilder.Length > 1)
                {
                    oBuilder.Append(",{");
                }
                else
                {
                    oBuilder.Append("{");
                }
                foreach (DataColumn oColumn in oRow.Table.Columns)
                {
                    sName = oColumn.ColumnName;
                    objValue = oRow[sName];
                    sValue = objValue == DBNull.Value ? "" : objValue.ToString();
                    if (oBuilder.Length > 2)
                    {
                        oBuilder.Append(",");
                    }
                    oBuilder.AppendFormat("{0}:'{1}'", sName.ToUpper(), EncodeURIComponent(sValue));
                }
                oBuilder.Append("}");
               
            }
            oBuilder.Append("]");
        }
        return oBuilder.ToString();
    }
    public static string ObjectToJson(  object oBill)
    {
        Type oType = oBill.GetType();
        Type oTypeChild;
        object oValue;
        StringBuilder oBuilder = new StringBuilder();
        string sName = string.Empty;
        string sValue = string.Empty;
        PropertyInfo oChildProperty;
        oBuilder.Append("[{");
        foreach (PropertyInfo oProperty in oType.GetProperties())
        {
            sName = oProperty.Name;
            oValue = oProperty.GetValue(oBill,null);
            if (!(oValue is IList))
            {
                if (oValue != null)
                {
                    oTypeChild = oValue.GetType();
                    oChildProperty = oTypeChild.GetProperty("ID");
                    if (oChildProperty == null)
                    {
                        oChildProperty = oTypeChild.GetProperty("Id");
                    }
                    if (oChildProperty != null)
                    {
                        oValue = oChildProperty.GetValue(oValue, null);
                    }


                }
                sValue = oValue == null ? "" : oValue.ToString();

                if (oBuilder.Length > 2)
                {
                    oBuilder.Append(",");
                }
                oBuilder.AppendFormat("{0}:'{1}'", sName.ToUpper(), EncodeURIComponent(sValue));
            }
        }
        oBuilder.Append("}]");
        return oBuilder.ToString();
    }

    public static string ObjectToJson(IList lstBill)
    {
       
        Type oTypeChild;
        object oValue;
        StringBuilder oBuilder = new StringBuilder();
        string sName = string.Empty;
        string sValue = string.Empty;
        PropertyInfo oChildProperty;
        if (lstBill != null && lstBill.Count > 0)
        {
            oBuilder.Append("[");
            foreach (object oBill in lstBill)
            {
                Type oType = oBill.GetType();
                if (oBuilder.Length == 1)
                {
                    oBuilder.Append("{");
                }
                else
                {
                    oBuilder.Append(",{");
                }
                foreach (PropertyInfo oProperty in oType.GetProperties())
                {
                    sName = oProperty.Name;
                    oValue = oProperty.GetValue(oBill, null);
                    if (!(oValue is IList))
                    {
                        if (oValue != null)
                        {
                            oTypeChild = oValue.GetType();
                            oChildProperty = oTypeChild.GetProperty("ID");
                            if (oChildProperty == null)
                            {
                                oChildProperty = oTypeChild.GetProperty("Id");
                            }
                            if (oChildProperty != null)
                            {
                                oValue = oChildProperty.GetValue(oValue, null);
                            }


                        }
                        sValue = oValue == null ? "" : oValue.ToString();

                        if (oBuilder.Length > 2)
                        {
                            oBuilder.Append(",");
                        }
                        oBuilder.AppendFormat("{0}:'{1}'", sName.ToUpper(), EncodeURIComponent(sValue));
                    }
                }
                oBuilder.Append("}");
            }
            oBuilder.Append("]");
        }
        return oBuilder.ToString();
    }
  
    public static string EncodeURIComponent(string sValue)
    {
        return GlobalObject.encodeURIComponent(sValue);
    }
    public static string DecodeURIComponent(string sValue)
    {
        return GlobalObject.decodeURIComponent(sValue);
    }
    public static int GetColumnIndex(GridView oGird, string sHeaderText)
    {
        int iColumnIndex = -1;
        DataControlField oField=null;
       for(int i=0;i<oGird.Columns.Count;i++)
        {
           oField=oGird.Columns[i];
            if(string.Equals(oField.HeaderText,sHeaderText)){
                iColumnIndex = i;
                break;
            }
       }
       return iColumnIndex;
    }
    public static string QueryString(Page oPage, string sKey)
    {
        string sValue = oPage.Request[sKey];
        if (!string.IsNullOrEmpty(sValue))
        {
            sValue = GlobalObject.decodeURIComponent(sValue);
        }
        return sValue;
    }
    public static bool GetPageReadOnly(PageState pageState)
    {
        bool readOnly = true;
        if (pageState == PageState.Show)
        {
            readOnly = true;
        }
        else if (pageState == PageState.New || pageState == PageState.Update)
        {
            readOnly = false;
        }
        return readOnly;
    }
    public static void SetControl(Control[] arrControls, PageState pageState)
    {
        bool readOnly = GetPageReadOnly(pageState);
        Type oType;
        Type textBoxType = typeof(TextBox);
        Type dpLstType = typeof(DropDownList);
        Type btnType = typeof(Button);
        TextBox oText;
        DropDownList dpLst;
        Button btn;
        
        foreach (Control oControl in arrControls)
        {
            oType = oControl.GetType();
            if (oType == textBoxType)
            {
                oText = oControl as TextBox;
                //oText.ReadOnly = readOnly;
                oText.Enabled = !readOnly;
            }
            else if (oType == dpLstType)
            {
                dpLst = oControl as DropDownList;
                dpLst.Enabled = !readOnly;
            }
            else if (oType == btnType)
            {
                btn = oControl as Button;
                btn.Enabled = !readOnly;
            }
            else
            {
            }

        }
    }
    public static decimal ToDecimal(object obj)
    {
        decimal dResult=0;
        if(obj==null){
        }
        else{
            decimal.TryParse(obj.ToString(),out dResult);
        }
        return dResult;
    }
    public static DateTime ToDateTime(object obj)
    {
        DateTime dTime = DateTime.MinValue;
        if (obj != null)
        {
            DateTime.TryParse(obj.ToString(), out dTime);
        }
        return dTime;
    }
    /// <summary>
    /// 根据id查找对应的单据对象
    /// </summary>
    /// <param name="lstBill"></param>
    /// <param name="sID"></param>
    /// <returns></returns>
    public static object GetBill(IList lstBill, string sID)
    {
        object oResult = null;
        string sTempID = string.Empty;
        if (lstBill == null || lstBill.Count == 0)
        {
            oResult = null;
        }
        else
        {
            foreach (object oBill in lstBill)
            {
                sTempID = GetBillID(oBill);
                if (string.Equals(sTempID, sID))
                {
                    oResult = oBill;
                    break;
                }
            }
        }
        return oResult;
    }
    /// <summary>
    /// 获取单据唯一编号
    /// </summary>
    /// <param name="oBill"></param>
    /// <returns></returns>
    public static   string GetBillID(object oBill)
    {
        string sID = string.Empty;
        if (oBill != null)
        {
            if (oBill is BaseMaster)
            {
                sID = (oBill as BaseMaster).Id;
            }
            else if (oBill is BaseDetail)
            {
                sID = (oBill as BaseDetail).Id;
            }
            else
            {
                sID = string.Empty;
            }
        }
        return sID;
    }
    public static void ClearBillID(object oBill)
    {
        if (oBill != null)
        {
            if (oBill is BaseMaster)
            {
                (oBill as BaseMaster).Id = null; ;
            }
            else if (oBill is BaseDetail)
            {
                (oBill as BaseDetail).Id = null ;
            }
        }
    }
    public static string GetDocumentStateName( string sState)
    {
       
        DocumentState state = (DocumentState)Enum.Parse(typeof(DocumentState), sState);
        string sResult = string.Empty;
        switch (state)
        {
            case DocumentState.Edit: { sResult = "编辑"; break; }
            case DocumentState.Valid: { sResult = "有效"; break; }
            case DocumentState.Invalid: { sResult = "无效"; break; }
            case DocumentState.InAudit: { sResult = "审批中"; break; }
            case DocumentState.Suspend: { sResult = "挂起"; break; }
            case DocumentState.InExecute: { sResult = "执行中"; break; }
            case DocumentState.Freeze: { sResult = "冻结"; break; }
            case DocumentState.Completed: { sResult = "结束"; break; }
            case DocumentState.Blankness: { sResult = ""; break; }
            default: { sResult = ""; break; }
        }
        return sResult;
    }
}
