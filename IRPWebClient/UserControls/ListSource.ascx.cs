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
using ComponentArt.Web.UI;

public partial class UserControls_ListSource : System.Web.UI.UserControl
{

    private int pageCount = 0;
  
    public event PublicClass.Method GoPageBefore;

    public event PublicClass.Method GoPageAfter;
    public event PublicClass.SelectedIndexChanged SelectedIndexChanged;
    // private int rowCount = 0;
    public EnumAlign Align
    {
        get
        {
            string sValue = hdAlign.Value.Trim();
            EnumAlign align = EnumAlign.Left;
            if (Enum.Parse(typeof(EnumAlign), sValue, true) != null)
            {
                align = (EnumAlign)Enum.Parse(typeof(EnumAlign), sValue, true);
            }

            return align;
        }
        set
        {

            string sName = value.ToString();
            hdAlign.Value = sName;
        }
    }

   
    /// <summary>
    /// 页面条数
    /// </summary>
    /// 
    public int PageSize
    {
        get { return int.Parse(hdPageSize.Value); }
        set { hdPageSize.Value = value.ToString(); }

    }
    /// <summary>
    /// 开关
    /// </summary>
    public bool IsLoadData
    {
        get { return bool.Parse(this.hdState.Value); }
        set { this.hdState.Value = value.ToString(); }
    }
    /// <summary>
    /// 共页数
    /// </summary>
    public int PageCount
    {
        get
        {
            if (this.PageSize == 0)
            {
                pageCount = 0;
            }
            else
            {
                pageCount = (this.RowCount + this.PageSize - 1) / this.PageSize;
            }
            return pageCount;
        }
    }
    /// <summary>
    /// 是否开启选择
    /// </summary>
    public bool IsSelect
    {
        get { return string.Equals(hdIsSelect.Value, "1") ? true : false; }
        set { hdIsSelect.Value = value == true ? "1" : "0"; }
    }
    public int SelectRow
    {
        get { return int.Parse(hdSelectRow.Value); }
        set { hdSelectRow.Value = value.ToString(); }
    }
    public void btnSelectClick(object sender, EventArgs e)
    {
        if (IsSelect && SelectedIndexChanged != null)
        {
            DestControl.SelectedIndex = SelectRow;
            if (SelectRow > -1)
            {
                SelectedIndexChanged(DestControl, e);
            }
        }

    }
    /// <summary>
    /// 行数
    /// </summary>
    public int RowCount
    {
        get
        {
            return int.Parse(hidRowCount.Value);

        }
        set { hidRowCount.Value = value.ToString(); }

    }
    public int CurrentPage
    {
        get { return int.Parse(hdShowPageCount.Value); }
        set
        {
            if (value < 1)
            {
                hdShowPageCount.Value = "1";
            }
            else
            {
                hdShowPageCount.Value = value.ToString();
            }
        }
    }

    public GridView DestControl
    {
        get { return DestSource; }
        set
        {
            _DestSource = value;
            DestGridViewControlName = value.ID;
            DestGridViewControlClientID = value.ClientID;
        }
    }
    /// <summary>
    ///需要绑定的数据库
    /// </summary>
    public string DestGridViewControlName
    {
        get
        {
            string id = "";
            if (this.ViewState["DestGridViewControlName"] != null)
                id = this.ViewState["DestGridViewControlName"].ToString();

            return id;
        }
        set
        {
            this.ViewState["DestGridViewControlName"] = value;
        }
    }

    public string DestGridViewControlClientID
    {
        get
        {
            string clientID = "";
            if (this.ViewState["DestGridViewControlClientID"] != null)
                clientID = this.ViewState["DestGridViewControlClientID"].ToString();

            return clientID;
        }
        set
        {
            this.ViewState["DestGridViewControlClientID"] = value;
            this.hdControl.Value = value;
        }
    }
    public void SetStyle()
    {
        if (DestControl != null)
        {
            DestControl.RowStyle.BackColor = System.Drawing.Color.FromName("#FFFFFF");
            DestControl.RowStyle.ForeColor = System.Drawing.Color.Black;
            DestControl.FooterStyle.BackColor = System.Drawing.Color.FromName("#F7FAFF");//B4D8E2
            DestControl.FooterStyle.ForeColor = System.Drawing.Color.Black;
            DestControl.HeaderStyle.BackColor = System.Drawing.Color.FromName("#F2F4F6");//3778A9
            DestControl.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            //DestControl.HeaderStyle.Font.Bold = true;
            DestControl.AlternatingRowStyle.BackColor = System.Drawing.Color.FromName("#F7FAFF");//B4D8E2
            //DestControl.SelectedRowStyle.CssClass = "text-align: center";
        }
    }
    /// <summary>
    /// 查询数据的SQL 
    /// 请在SQL语句前加 select  ROW_NUMBER() over (order by  t.OPGCODE) as rownum ,[列]|[*] from 表 [条件]
    /// 例如select  ROW_NUMBER() over (order by  t.OPGCODE) as rownum ,* from RESOPERATIONORG t   where t.OPGID>0
    /// </summary>
    public string DataSQL
    {
        get { return   UtilClass.DecodeURIComponent( this.hdSQL.Value); }
        set
        {
            this.CurrentPage = 1;
            this.hdSQL.Value = UtilClass.EncodeURIComponent(value);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetData();
        }
        SetStyle();
    }
    private GridView _DestSource;
    protected GridView DestSource
    {
        get
        {
            if (_DestSource == null)//页面
                _DestSource = Page.FindControl(this.DestGridViewControlName) as GridView;
            if (_DestSource == null)//用户控件
                _DestSource = this.Parent.FindControl(this.DestGridViewControlName) as GridView;
            if (_DestSource == null)//
            {
                if (this.Parent.Parent != null && this.Parent.Parent.Parent != null)
                    foreach (Control cp in this.Parent.Parent.Parent.Controls)
                    {
                        if ((cp as PageView) == null)
                            continue;

                        PageView pv = cp as PageView;
                        foreach (Control c in pv.Controls)
                        {
                            UserControl uc = c as UserControl;
                            if (uc != null)
                            {
                                _DestSource = uc.FindControl(this.DestGridViewControlName) as GridView;
                                if (_DestSource != null)
                                    break;
                            }
                        }
                    }
            }
            return _DestSource;
        }
    }
    private DataTable oDataTable = null;
    public void GetData()
    {
        string sMsg = string.Empty;
        DataSet ds = null;
        DataTable oTable = null;
        oDataTable = null;
        try
        {
            //SetStyle();
            if (this.GoPageBefore != null)
            {
                GoPageBefore();
            }
            if (IsLoadData)
            {
                try
                {
                    if (!string.IsNullOrEmpty(DataSQL))
                    {
                        if (DestSource != null)
                        {
                            int rowCount = 0;
                            ds = GlobalClass.CommonMethodSrv.GetDataAndRowCount(DataSQL, CurrentPage, PageSize, ref  rowCount);
                            RowCount = rowCount;
                            // this.pageCount = (RowCount + PageSize - 1) / PageSize;
                            if (ds != null && ds.Tables.Count > 0)
                            {
                                oTable = ds.Tables[0];
                            }
                        }
                        else
                        {
                            sMsg = "数据源的DestGridViewControlName属性未赋值";
                        }

                    }
                    else
                    {
                        sMsg = "数据源的DataSQL属性未赋值";
                    }
                }
                catch (Exception ex)
                {
                    sMsg = "请检查SQL语句";
                    RowCount = 0;
                    CurrentPage = 1;
                    this.pageCount = 0;

                }
                if (!string.IsNullOrEmpty(sMsg))
                {
                    Message(sMsg);
                }
                if (oTable.Rows.Count > 0)
                {
                    oDataTable = oTable;
                    this.DestSource.DataSource = oTable;
                    // this.DestSource.DataKeyNames = new string[] { "Id"};
                    if (IsSelect)
                    {
                        this.DestSource.RowDataBound += new GridViewRowEventHandler(RowDataBound);
                    }
                    this.DestSource.DataBind();
                    this.SelectRow = -1;
                    this.DestControl.SelectedIndex = -1;
                    

                }
                else
                {
                    oTable.Rows.Add(oTable.NewRow());
                    DestSource.DataSource = oTable;
                    DestSource.DataBind();
                    int columnCount = oTable.Columns.Count;
                    //DestSource.Rows[0].Cells.Clear();
                    DestSource.Rows[0].Cells.Add(new TableCell());
                    for (int i = 1; i < DestSource.Rows[0].Cells.Count; i++)
                    {
                        DestSource.Rows[0].Cells[i].Visible = false;
                    }
                    DestSource.Rows[0].Cells[0].ColumnSpan = columnCount;

                    DestSource.Rows[0].Cells[0].Text = string.IsNullOrEmpty(DestSource.EmptyDataText) ? "没有数据" : DestSource.EmptyDataText;
                    DestSource.Rows[0].Cells[0].Style.Add("text-align", "center");
                    DestSource.Rows[0].Cells[0].Visible = true;
                }
            }
            else
            {
                RowCount = 0;
                pageCount = 0;
            }
            SetPage();
            if (this.GoPageAfter != null)
            {
                GoPageAfter();
            }
        }

        catch (Exception ex)
        {
            sMsg = "请检查SQL语句和绑定字段，错误原因:" + ex.Message;
        }
        Message(sMsg);
    }


    public void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //e.Row.Cells[0].
        if (oDataTable!=null && oDataTable.Rows.Count>0&& e.Row.RowType == DataControlRowType.DataRow)
        {
            string sRowData=string.Empty ;
            int iRowIndex = e.Row.RowIndex;
            int iColumnIndex=-1;
            if (oDataTable.Rows.Count > iRowIndex)
            {
                iColumnIndex = UtilClass.GetColumnIndex(this.DestControl, "序号");
                if (iColumnIndex > -1)
                    e.Row.Cells[iColumnIndex].Text = (iRowIndex+1).ToString();
                sRowData=UtilClass.DataRowToJosn( oDataTable.Rows[iRowIndex]);
                if(!string.IsNullOrEmpty(sRowData)){
                    //e.Row.Attributes.Add("Data",  sRowData );
                    //当鼠标停留时更改背景色
                    e.Row.Attributes.Add("onmouseover", "curBak=this.style.backgroundColor;this.style.color='#116a79';this.style.backgroundColor='#cef97b'");
                    //当鼠标移开时还原背景色
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=curBak;this.style.color='#3a6381'");
                    e.Row.Attributes["style"] = "Cursor:pointer";
                   // e.Row.Attributes.Add("onmouseover", string.Format("mouseOver({0})", iRowIndex));
                    e.Row.Attributes.Add("onclick", string.Format("singleClick({0},decodeComponent('{1}'))", iRowIndex, sRowData));
                    e.Row.Attributes.Add("ondblclick", string.Format("dbClick({0},decodeComponent('{1}'))", iRowIndex, sRowData));
                    //e.Row.Attributes.Add("onmouseout", string.Format("mouseOut({0})", iRowIndex));
                    
                }
            }
        }
    }
    
    public void SetPage()
    {

        if (this.PageCount == 0)
        {
            this.linkFirst.Enabled = false;
            this.linkLast.Enabled = false;
            this.linkNext.Enabled = false;
            this.linkPrev.Enabled = false;
            this.selPager.Items.Clear();
            this.selPager.Enabled = false;
        }
        else
        {
            this.selPager.Items.Clear();
            for (int i = 1; i <= this.PageCount; i++)
            {
                this.selPager.Items.Insert(this.selPager.Items.Count, i.ToString());
            }
            this.selPager.Items[this.CurrentPage - 1].Selected = true;


            this.linkFirst.Enabled = this.CurrentPage == 1 ? false : true;
            if (this.linkFirst.Enabled)
            {
                linkFirst.Style.Add("cursor", "pointer");
                linkFirst.Style.Add("color", "white");
            }
            else
            {
                linkFirst.Style.Add("cursor", "text");
                linkFirst.Style.Add("color", "black");
            }

            this.linkPrev.Enabled = this.linkFirst.Enabled;
            if (this.linkPrev.Enabled)
            {
                linkPrev.Style.Add("cursor", "pointer");
                linkPrev.Style.Add("color", "white");
            }
            else
            {
                linkPrev.Style.Add("cursor", "text");
                linkPrev.Style.Add("color", "black");
            }

            this.linkLast.Enabled = this.CurrentPage == this.pageCount ? false : true;
            if (this.linkLast.Enabled)
            {
                linkLast.Style.Add("cursor", "pointer");
                linkLast.Style.Add("color", "white");
            }
            else
            {
                linkLast.Style.Add("cursor", "text");
                linkLast.Style.Add("color", "black");
            }

            this.linkNext.Enabled = this.linkLast.Enabled;
            if (this.linkNext.Enabled)
            {
                linkNext.Style.Add("cursor", "pointer");
                linkNext.Style.Add("color", "white");
            }
            else
            {
                linkNext.Style.Add("cursor", "text");
                linkNext.Style.Add("color", "black");
            }
        }
    }
    public void Message(string sMsg)
    {
        if (!string.IsNullOrEmpty(sMsg))
        {
            string sTemp = string.Format("数据源控件出错:{0}", sMsg);
            sTemp = string.Format("<script>alert('{0}');</script>", sTemp);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", sTemp);
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Button lb = sender as Button;
        if (lb == null)
            return;

        if (lb.CommandName == "first")
            hdShowPageCount.Value = "1";
        if (lb.CommandName == "prev")
            hdShowPageCount.Value = (this.CurrentPage - 1).ToString();
        if (lb.CommandName == "next")
            hdShowPageCount.Value = (this.CurrentPage + 1).ToString();
        if (lb.CommandName == "last")
            hdShowPageCount.Value = (this.PageCount).ToString();

        GetData();

    }
    protected void selPager_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hdShowPageCount.Value = selPager.SelectedValue;
        this.CurrentPage = selPager.SelectedIndex+1;
        GetData();
    }
}
