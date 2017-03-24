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

/// <summary>
/// Summary description for GridViewTemplateState
/// </summary>
public class GridViewTemplateState : ITemplate
{
    private DataControlRowType templateType;
    private static string headName = "操作";
    private GridView DestControl;
    private bool IsDelete = false;
    private string DeleteClick = string.Empty;
    private bool IsNew = false;
    private string NewClick = string.Empty;
    private bool IsUpdate = false;
    private string UpdateClick = string.Empty;
    private string deleteRowClientID = string.Empty;
    public GridViewTemplateState(GridView DestControl, DataControlRowType type)
    {
        this.DestControl = DestControl;
        templateType = type;
    }
    public GridViewTemplateState(GridView DestControl, DataControlRowType type, bool IsDelete, string DeleteClick,
        bool IsNew, string NewClick,
        bool IsUpdate, string UpdateClick,string sDeleteRowIDClientID)
    {
        this.DestControl = DestControl;
        templateType = type;
        this.IsDelete = IsDelete;
        this.DeleteClick = DeleteClick;
        this.IsUpdate = IsUpdate;
        this.UpdateClick = UpdateClick;
        this.IsNew = IsNew;
        this.NewClick = NewClick;
        deleteRowClientID = sDeleteRowIDClientID;
    }
    public static bool Existed(GridView oGrid)
    {
        if (oGrid != null)
        {
            int iColumnIndex = UtilClass.GetColumnIndex(oGrid, headName);
            return iColumnIndex > -1;
        }
        return false;
    }
    public static bool Delete(GridView oGrid)
    {
        if (Existed(oGrid))
        {
            int iColumnIndex = UtilClass.GetColumnIndex(oGrid, headName);
            oGrid.Columns.RemoveAt(iColumnIndex);
        }
        return true;
    }
    public void InstantiateIn(System.Web.UI.Control container)
    {
        switch (templateType)
        {
            case DataControlRowType.Header:
                Literal lc = new Literal();
                lc.Text = headName;
                container.Controls.Add(lc);
                break;
            case DataControlRowType.DataRow:

                //TextBox tb = new TextBox();
                //tb.ID = container.ClientID;
                LiteralControl lit = new LiteralControl();
                lit.DataBinding += new EventHandler(DataBinding);
                container.Controls.Add(lit);
                break;
            default:
                break;
        }
    }
    public void DataBinding(object sender, EventArgs e)
    {
        LiteralControl lit = (LiteralControl)sender;
        GridViewRow container = (GridViewRow)lit.NamingContainer;
        StringBuilder oBuilder = new StringBuilder();
        string strClick = string.Empty;
        if (this.IsNew || this.IsDelete || this.IsUpdate)
        {
            oBuilder.Append("<div style=' width:100px; height:100%; '>");
            if (this.IsNew)
            {
                oBuilder.AppendFormat("<img title='新增' style='Cursor:pointer' src='{0}'  onclick='return _Oprate(\"{1}\",\"{2}\",{3},{4},\"{5}\"); ' />&nbsp;", lit.Page.ResolveUrl("~/images/grid/add1.png"),
                    this.DestControl.ClientID, "New", container.RowIndex + 1, string.IsNullOrEmpty(this.NewClick) ? "null" : this.NewClick,this.deleteRowClientID);
            }
            if (this.IsUpdate)
            {
                oBuilder.AppendFormat("<img title='修改'style='Cursor:pointer' src='{0}'  onclick='return _Oprate(\"{1}\",\"{2}\",{3},{4},\"{5}\"); ' />&nbsp;", lit.Page.ResolveUrl("~/images/grid/edit1.png"),
                    this.DestControl.ClientID, "Update", container.RowIndex + 1, string.IsNullOrEmpty(this.UpdateClick) ? "null" : this.UpdateClick, this.deleteRowClientID);
            }
            if (this.IsDelete)
            {
                oBuilder.AppendFormat("<img title='删除' style='Cursor:pointer' src='{0}'  onclick='return _Oprate(\"{1}\",\"{2}\",{3},{4},\"{5}\"); ' />", lit.Page.ResolveUrl("~/images/grid/del1.png"),
                    this.DestControl.ClientID, "Delete", container.RowIndex + 1, string.IsNullOrEmpty(this.DeleteClick) ? "null" : this.DeleteClick, this.deleteRowClientID);
            }
            oBuilder.Append("</div>");
            lit.Text = oBuilder.ToString();
        }




        //tb.Text = ((DataRowView)container.DataItem)[dataField].ToString();

    }
}
