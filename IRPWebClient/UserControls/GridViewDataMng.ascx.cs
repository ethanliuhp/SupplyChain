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
using System.Text;
using System.Collections.Generic;
using Application.Business.Erp.SupplyChain.Base.Domain;

public partial class UserControls_GridViewDataMng : System.Web.UI.UserControl
{
    private int pageCount = 0;

    public event PublicClass.Method GoPageBefore;

    public event PublicClass.Method GoPageAfter;
    public event PublicClass.SelectedIndexChanged SelectedIndexChanged;
    public bool IsDataBind = false;
    private IList newBillList;
    private IList billList;
    private IList removeList;
    /// <summary>
    /// 控件唯一ID
    /// </summary>
    public string ControlKey
    {
        get
        {
            if (string.IsNullOrEmpty(hdUserControlKey.Value))
            {
                hdUserControlKey.Value = Guid.NewGuid().ToString() + "_";
            }
            return hdUserControlKey.Value;
        }
    }

    /// <summary>
    /// 新增单据
    /// </summary>
    public IList NewBillList
    {
        get
        {
            if (this.newBillList == null)
            {
                this.newBillList = ViewState[this.ControlKey + "NewBillList"] as IList;
                if (this.newBillList == null)
                {
                    this.newBillList = new ArrayList();
                    ViewState[this.ControlKey + "NewBillList"] = this.newBillList;
                }
            }
            return this.newBillList;
        }

    }
    /// <summary>
    /// 单据 数据
    /// </summary>
    public IList BillList
    {
        get
        {
            if (this.billList == null)
            {
                this.billList = ViewState[this.ControlKey + "BillList"] as IList;
                if (this.billList == null)
                {
                    this.billList = new ArrayList();
                    ViewState[this.ControlKey + "BillList"] = this.billList;
                }
            }
            return billList;
        }
        set
        {
            ViewState[this.ControlKey + "BillList"] = value;
            billList = value;
        }
    }
    public IList RemoveList
    {
        get
        {
            if (this.removeList == null)
            {
                this.removeList = ViewState[this.ControlKey + "RemoveList"] as IList;
                if (this.removeList == null)
                {
                    this.removeList = new ArrayList();
                    ViewState[this.ControlKey + "RemoveList"] = this.removeList;
                }
            }
            return this.removeList;
        }
    }
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
    /// 数据源
    /// </summary>
    public object DataSource
    {
        set;
        get;
    }
    #region 客户端单击
    /// <summary>
    /// 客户端事件脚本函数名  
    /// </summary>
    public string ClientClick
    {
        get { return this.hdClientClick.Value; }
        set { hdClientClick.Value = value; }
    }
    /// <summary>
    /// 启动客户端单击事件 
    /// </summary>
    public bool EnabledClientSingleClick
    {
        get { return bool.Parse(this.hdEnabledClientSingleClick.Value); }
        set { hdEnabledClientSingleClick.Value = value.ToString(); }
    }
    #endregion
    #region 客户端双击
    
    /// <summary>
    /// 启动客户端双击事件 
    /// </summary>
    public bool EnabledClientDoubleClick
    {
        get { return bool.Parse(this.hdEnabledClientDoubleClick.Value); }
        set { hdEnabledClientDoubleClick.Value = value.ToString(); }
    }
    #endregion
    #region 客户端修改
   
    /// <summary>
    /// 启动客户端修改事件 
    /// </summary>
    public bool EnabledClientUpdateClick
    {
        get { return bool.Parse(this.hdEnabledClientUpdateClick.Value); }
        set { hdEnabledClientUpdateClick.Value = value.ToString(); }
    }
    #endregion
    #region 客户端新增
   
    /// <summary>
    /// 启动客户端新增事件 
    /// </summary>
    public bool EnabledClientNewClick
    {
        get { return bool.Parse(this.hdEnabledClientNewClick.Value); }
        set { hdEnabledClientNewClick.Value = value.ToString(); }
    }
    #endregion
    #region 客户端删除
   
    /// <summary>
    /// 启动客户端删除事件 
    /// </summary>
    public bool EnabledClientDeleteClick
    {
        get { return bool.Parse(this.hdEnabledClientDeleteClick.Value); }
        set { hdEnabledClientDeleteClick.Value = value.ToString(); }
    }
    #endregion
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
    public int SelectRowIndex
    {
        get { return int.Parse(hdSelectRowIndex.Value); }
        set { hdSelectRowIndex.Value = value.ToString(); }
    }
    public void btnSelectClick(object sender, EventArgs e)
    {
        if (IsSelect && SelectedIndexChanged != null)
        {
            DestControl.SelectedIndex = this.SelectRowIndex;
            if (this.SelectRowIndex > -1)
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
            DestControl.HeaderStyle.Height = Unit.Pixel(30);
           // DestControl.HeaderStyle.Font.Bold = true;
            //DestControl.HeaderStyle.Font.Bold = true;
            DestControl.AlternatingRowStyle.BackColor = System.Drawing.Color.FromName("#F7FAFF");//B4D8E2
            DestControl.RowStyle.Height = Unit.Pixel(24);
            //DestControl.SelectedRowStyle.CssClass = "text-align: center";
            //DestControl.Style.Add("table-layout", "fixed;");
            DestControl.Style.Add("word-break","break-all");
            //DestControl.RowStyle.Wrap = false;
        }
    }
    /// <summary>
    /// 查询数据的SQL 
    /// 请在SQL语句前加 select  ROW_NUMBER() over (order by  t.OPGCODE) as rownum ,[列]|[*] from 表 [条件]
    /// 例如select  ROW_NUMBER() over (order by  t.OPGCODE) as rownum ,* from RESOPERATIONORG t   where t.OPGID>0
    /// </summary>
    public string DataSQL
    {
        get { return UtilClass.DecodeURIComponent(this.hdSQL.Value); }
        set
        {
            this.CurrentPage = 1;
            this.hdSQL.Value = UtilClass.EncodeURIComponent(value);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (IsPostBack)
        //{
        //    DataBind();
        //}
        DeleteBill();
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
    #region 数据源绑定
    /// <summary>
    /// 绑定数据源
    /// </summary>
    public override void DataBind()
    {
     
        base.DataBind();
        string sMessage = string.Empty;
        try
        {
            if (this.DestSource != null)
            {
                if (string.IsNullOrEmpty(this.DestSource.EmptyDataText))
                {
                    this.DestSource.EmptyDataText = "数据为空";
                }
                AddOperateTemplate();
                if (this.DataSource == null)
                {
                    ExecuteSql();
                }
                if (this.DataSource != null)
                {
                    if ((this.DataSource is DataTable) || (this.DataSource is Iesi.Collections.Generic.ISet<BaseDetail>) || (this.DataSource is Iesi.Collections.Generic.ISet<BaseMaster>))
                    {
                        if ((this.DataSource is Iesi.Collections.Generic.ISet<BaseDetail>) || (this.DataSource is Iesi.Collections.Generic.ISet<BaseMaster>))
                        {

                            if (this.DataSource is Iesi.Collections.Generic.ISet<BaseDetail>)
                            {
                                this.BillList = (this.DataSource as Iesi.Collections.Generic.ISet<BaseDetail>).ToList();
                            }
                            if (this.DataSource is Iesi.Collections.Generic.ISet<BaseMaster>)
                            {
                                this.BillList = (this.DataSource as Iesi.Collections.Generic.ISet<BaseMaster>).ToList();
                            }
                            object oBill = null;
                            foreach (object obj in RemoveList)
                            {
                                oBill = UtilClass.GetBill(this.BillList, UtilClass.GetBillID(obj));
                                if (oBill != null)
                                {
                                    this.BillList.Remove(oBill);
                                }
                            }
                            this.DataSource = this.BillList;
                        }
                        if (((this.DataSource is DataTable) && (this.DataSource as DataTable).Rows.Count > 0) || ((this.DataSource is IList) && (this.DataSource as IList).Count > 0) || ((this.DataSource is IList) && (this.DataSource as IList).Count > 0))
                        {
                            if (IsSelect)
                            {
                                this.DestSource.RowDataBound += new GridViewRowEventHandler(RowDataBound);

                            }
                        }
                        this.DestSource.DataSource = this.DataSource;
                        this.DestSource.DataBind();

                       
                        if (this.DestControl.Rows.Count > 0)
                        {
                            SetPage();
                        }
                        else
                        {
                            AddEmptyRow();
                            //DataTable oTable = this.DataSource as DataTable;
                            //oTable.Rows.Add(oTable.NewRow());
                            //DestSource.DataSource = oTable.DefaultView;
                            //DestSource.DataBind();
                            //int columnCount = this.DestSource.Columns.Count ;
                            ////DestSource.Rows[0].Cells.Clear();
                            //DestSource.Rows[0].Cells.Add(new TableCell());
                            //for (int i = 1; i < DestSource.Rows[0].Cells.Count; i++)
                            //{
                            //    DestSource.Rows[0].Cells[i].Visible = false;
                            //}
                            //DestSource.Rows[0].Cells[0].ColumnSpan = columnCount;

                            //DestSource.Rows[0].Cells[0].Text = string.IsNullOrEmpty(DestSource.EmptyDataText) ? "没有数据" : DestSource.EmptyDataText+"1231";
                            //DestSource.Rows[0].Cells[0].Style.Add("text-align", "center");
                            //DestSource.Rows[0].Cells[0].Visible = true;
                        }
                    }

                }
            }
            else
            {
                throw new Exception("请给[" + this.ClientID + "]绑定对应的列表控件");
            }
        }
        catch (Exception ex)
        {
            sMessage = "绑定数据元出错:[" + ex.Message + "]";
        }
        Message(sMessage);
       

    }
    private void ExecuteSql()
    {
        try
        {
            int rowCount = 0;
            DataSet ds = GlobalClass.CommonMethodSrv.GetDataAndRowCount(DataSQL, CurrentPage, PageSize, ref  rowCount);
            RowCount = rowCount;
            // this.pageCount = (RowCount + PageSize - 1) / PageSize;
            if (ds != null && ds.Tables.Count > 0)
            {
                this.DataSource = ds.Tables[0];
            }
            else
            {
                this.DataSource = null;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("SQL语句查询失败({0})", ex.Message));
        }

    }
    public void GetData()
    {
        string sMsg = string.Empty;
        DataSet ds = null;
        DataTable oTable = null;
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
                    this.DestSource.DataSource = oTable;
                    // this.DestSource.DataKeyNames = new string[] { "Id"};
                    if (IsSelect)
                    {
                        this.DestSource.RowDataBound += new GridViewRowEventHandler(RowDataBound);
                    }
                    this.DestSource.DataBind();
                    this.SelectRowIndex = -1;
                    this.DestControl.SelectedIndex = -1;
                }
                else
                {
                    AddEmptyRow();
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
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
        {
            object oRowData = e.Row.DataItem;
            string sRowData = string.Empty;
            int iRowIndex = e.Row.RowIndex;
            int iColumnIndex = -1;
            iColumnIndex = UtilClass.GetColumnIndex(this.DestControl, "序号");
            if (iColumnIndex > -1)
            {
                e.Row.Cells[iColumnIndex].Text = (iRowIndex + 1).ToString();
            }
            if (oRowData is DataRowView)
            {
                sRowData = UtilClass.DataRowToJosn((oRowData as DataRowView).Row);
            }
            else
            {
                sRowData = UtilClass.ObjectToJson(oRowData);
            }
            if (!string.IsNullOrEmpty(sRowData))
            {
                e.Row.Attributes.Add("Data", sRowData);
                //当鼠标停留时更改背景色
                //e.Row.Attributes.Add("onmouseover", " debugger;if(this.parentNode.parentNode.selectRow && this.parentNode.parentNode.selectRow.index==1)  curBak=this.style.backgroundColor;this.style.color='#116a79';this.style.backgroundColor='#cef97b'");
                e.Row.Attributes.Add("onmouseover", string.Format("_MouseOver(this,'{0}',{1})",this.DestControl.ClientID,iRowIndex+1));
                //当鼠标移开时还原背景色
                e.Row.Attributes.Add("onmouseout", string.Format("_MouseOut(this,'{0}',{1})", this.DestControl.ClientID, iRowIndex + 1));
                e.Row.Attributes["style"] = "Cursor:pointer";
                // e.Row.Attributes.Add("onmouseover", string.Format("mouseOver({0})", iRowIndex));
                // e.Row.Attributes.Add("onclick", GetClickScript( this.ClientClick,iRowIndex));
                // _Oprate(gridID,flag,iRowIndex,_fun)
                string sClick = string.Format("return _Oprate('{0}','{1}',{2},{3},'{4}');", this.DestSource.ClientID, "SingleClick", iRowIndex + 1, string.IsNullOrEmpty(this.ClientClick) && !this.EnabledClientSingleClick ? "null" : this.ClientClick,this.hdPageDeleteBillIDs.ClientID);
                e.Row.Attributes.Add("onclick", sClick);
                sClick = string.Format("return _Oprate('{0}','{1}',{2},{3},'{4}');", this.DestSource.ClientID, "DoubleClick", iRowIndex + 1, string.IsNullOrEmpty(this.ClientClick) && !this.EnabledClientDoubleClick ? "null" : this.ClientClick, this.hdPageDeleteBillIDs.ClientID);
                e.Row.Attributes.Add("ondblclick", sClick);
                //e.Row.Attributes.Add("onmouseout", string.Format("mouseOut({0})", iRowIndex));

            }
 
        }

    }
   
    //public string GetClickScript(string sClientClick, int iIndex)
    //{
    //    StringBuilder oBuild = new StringBuilder();
    //    oBuild.Append(" debugger ; var selectRowData=getDecodeRowData(this.Data); ");//获取行数据
    //    oBuild.AppendFormat(" var selectRowIndex={0};", iIndex);//选中的行
    //    oBuild.AppendFormat(" var oGrid= document.getElementById('{0}');", this.DestSource.ClientID);//获取列表对象
    //    oBuild.Append(" oGrid.selectRow={index:selectRowIndex,data:selectRowData};");
    //    oBuild.AppendFormat("  oGrid.deleteRow= function(rowIndex){{ var oGrid= document.getElementById('{0}'); if( oGrid.rows.length >rowIndex){{oGrid.rows[rowIndex].style.display='none';var oDeleteDetailIds=document.getElementById('{1}'); var sIDs=oDeleteDetailIds.value; var arrIDs=[];if(sIDs){{arrIDs=sIDs.split(';'); }}arrIDs.push();oDeleteDetailIds.value= arrIDs.join(';');  }} }}; ", this.DestSource.ClientID, this.hdPageDeleteBillIDs.ClientID);
    //    if (!string.IsNullOrEmpty(sClientClick))
    //    {
    //        oBuild.AppendFormat(" if({0}){{ {0}(selectRowIndex,selectRowData);  }} ", sClientClick);
    //    }
    //    return oBuild.ToString();
    //}
    #endregion
    #region 数据操作
    /// <summary>
    /// 处理页面删除行
    /// </summary>
    private void DeleteBill()
    {
        object oBill = null;
        object oTempBill = null;
        IList lstRemove;
        if (!string.IsNullOrEmpty(hdPageDeleteBillIDs.Value))
        {
            lstRemove = new ArrayList();
            string[] arrRemoveID = hdPageDeleteBillIDs.Value.Split(';');
            foreach (string sID in arrRemoveID)
            {
                oBill = UtilClass.GetBill(this.BillList, sID);//查找 该ID对应的单据对象
                if (oBill != null)
                {
                    this.BillList.Remove(oBill);
                    oTempBill = UtilClass.GetBill(this.NewBillList, sID);//查找该ID是否为新建单据 如果是 不需要添加到删除集合中 如果不是(历史单据)  需要存放在删除集合中 等待后台删除
                    if (oTempBill != null)
                    {
                        this.NewBillList.Remove(oTempBill);
                    }
                    //else
                    //{
                        this.RemoveList.Add(oBill);
                   // }
                }
            }
        }
        hdPageDeleteBillIDs.Value = "";
    }
    /// <summary>
    /// 清空新增的单据的ID
    /// </summary>
    public IList ClearNewBillID(IList lstDetail)
    {
        object oBill = null;
        foreach (object obj in RemoveList)
        {
            oBill = UtilClass.GetBill(lstDetail, UtilClass.GetBillID(obj));
            if (oBill != null)
            {
                lstDetail.Remove(oBill);
            }
        }
        foreach (object obj in lstDetail)
        {
            //if (this.NewBillList.Contains(obj))

            if (UtilClass.GetBill(this.NewBillList, UtilClass.GetBillID(obj)) != null)
            {
                UtilClass.ClearBillID(obj);
            }
        }

        return lstDetail;
    }
    /// <summary>
    /// 清除id和获取删除的明细
    /// </summary>
    /// <param name="oMaster"></param>
    /// <returns></returns>
    public void ClearNewBillID(BaseMaster oMaster, IList removeDetailList)
    {
        BaseDetail oDetailTemp = null;
        foreach (BaseDetail oDetail in this.RemoveList)
        {
            oDetailTemp = oMaster.Details.FirstOrDefault(item => item.Id == oDetail.Id);
            if (oDetailTemp != null)
            {
                removeDetailList.Add(oDetailTemp);
                oMaster.Details.Remove(oDetailTemp);
            }
        }
        foreach (BaseDetail oDetail in this.NewBillList)
        {
            oDetailTemp = oMaster.Details.FirstOrDefault(item => item.Id == oDetail.Id);
            if (oDetailTemp != null)
            {
                oMaster.Details.Remove(oDetailTemp);
                oDetailTemp.Id = null;
                oMaster.Details.Add(oDetailTemp);
            }
        }
    }
    /// <summary>
    /// 添加新增单据
    /// </summary>
    /// <param name="oBill"></param>
    public void AddNewBill(object oBill)
    {
        if (oBill != null)
        {
            this.NewBillList.Add(oBill);
            this.BillList.Add(oBill);
        }
    }
    /// <summary>
    /// 清除修改标记
    /// </summary>
    public void ClearUpdate()
    {
        this.NewBillList.Clear();
        this.RemoveList.Clear();
    }
    #endregion
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

        UtilClass.MessageBox(this.DestControl.Page, sMsg);
        
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

        // GetData();
        this.DataBind();

    }
    protected void selPager_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hdShowPageCount.Value = selPager.SelectedValue;
        this.CurrentPage = selPager.SelectedIndex + 1;
        //GetData();
        this.DataBind();
    }
    protected void AddOperateTemplate()
    {
        if (this.EnabledClientDeleteClick || this.EnabledClientNewClick || this.EnabledClientUpdateClick)
        {
            GridViewTemplateState.Delete(this.DestControl);
            TemplateField oTemplate = new TemplateField();

            oTemplate.ShowHeader = true;
            oTemplate.HeaderText = "操作";
            //oTemplate.ItemTemplate=new GridViewTemplate()
            oTemplate.HeaderTemplate = new GridViewTemplateState(this.DestControl, DataControlRowType.Header);
            oTemplate.ItemTemplate = new GridViewTemplateState(this.DestControl, DataControlRowType.DataRow,
                this.EnabledClientDeleteClick, this.ClientClick,
                this.EnabledClientNewClick, this.ClientClick,
                this.EnabledClientUpdateClick,  this.ClientClick,this.hdPageDeleteBillIDs.ClientID);
            oTemplate.ItemStyle.Width = Unit.Pixel(100);
            this.DestControl.Columns.Insert(0, oTemplate);
        }

    }
    protected void AddEmptyRow()
    {
        SetStyle();
        GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
        foreach (DataControlField field in this.DestControl.Columns)
        {
             
            TableCell cell = new TableCell();
            cell.Visible = field.Visible;
            cell.Text = field.HeaderText;
            cell.Width = field.HeaderStyle.Width;
            cell.Height = field.HeaderStyle.Height;
            cell.ForeColor = field.HeaderStyle.ForeColor;
            cell.Font.Size = field.HeaderStyle.Font.Size;
            cell.Font.Bold = true;// field.HeaderStyle.Font.Bold;
            cell.Font.Name = field.HeaderStyle.Font.Name;
            cell.Font.Strikeout = field.HeaderStyle.Font.Strikeout;
            cell.Font.Underline = field.HeaderStyle.Font.Underline;
            cell.BackColor = field.HeaderStyle.BackColor;
            cell.VerticalAlign = field.HeaderStyle.VerticalAlign;
            cell.HorizontalAlign = field.HeaderStyle.HorizontalAlign;
            cell.CssClass = field.HeaderStyle.CssClass;
            cell.BorderColor = field.HeaderStyle.BorderColor;
            cell.BorderStyle = field.HeaderStyle.BorderStyle;
            cell.BorderWidth = field.HeaderStyle.BorderWidth;
            row.Cells.Add(cell);
        }
        TableItemStyle headStyle = this.DestControl.HeaderStyle;
        TableItemStyle emptyStyle = DestControl.EmptyDataRowStyle;
        emptyStyle.Width = headStyle.Width;
        emptyStyle.Height = headStyle.Height;
        emptyStyle.ForeColor = headStyle.ForeColor;
        emptyStyle.Font.Size = headStyle.Font.Size;
        emptyStyle.Font.Bold = headStyle.Font.Bold;
        emptyStyle.Font.Name = headStyle.Font.Name;
        emptyStyle.Font.Strikeout = headStyle.Font.Strikeout;
        emptyStyle.Font.Underline = headStyle.Font.Underline;
        emptyStyle.BackColor = headStyle.BackColor;
        emptyStyle.VerticalAlign = headStyle.VerticalAlign;
        emptyStyle.HorizontalAlign = headStyle.HorizontalAlign;
        emptyStyle.CssClass = headStyle.CssClass;
        emptyStyle.BorderColor = headStyle.BorderColor;
        emptyStyle.BorderStyle = headStyle.BorderStyle;
        emptyStyle.BorderWidth = headStyle.BorderWidth;
        //空白行的设置
        GridViewRow row1 = new GridViewRow(0, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
     
        TableCell cell1 = new TableCell();
        StringBuilder oBuilder = new StringBuilder();
        if (this.EnabledClientNewClick)
        {
            oBuilder.AppendFormat("<img title='新增' style='Cursor:pointer' src='{0}'  onclick='return _Oprate(\"{1}\",\"{2}\",{3},{4},\"{5}\"); ' />", this.DestControl.Page.ResolveUrl("~/images/grid/add1.png"),
               this.DestControl.ClientID, "New", -1, string.IsNullOrEmpty(this.ClientClick) ? "null" : this.ClientClick, this.hdPageDeleteBillIDs.ClientID);
        }
        else
        {
            oBuilder.Append("<strong>没有任何数据可以显示</strong> ");
        }
       // cell1.Text = "没有任何数据可以显示 <input type='button' value='121' />";
        cell1.Text = oBuilder.ToString();
        cell1.BackColor = System.Drawing.Color.White;
        row1.Cells.Add(cell1);
        cell1.ColumnSpan = this.DestControl.Columns.Count;


        //if (this.DestControl.Controls.Count == 0)
        //{
        //    DestControl.Page.Response.Write("<script language='javascript'>alert('必须在初始化表格类之前执行DataBind方法并设置EmptyDataText属性不为空!');</script>");
        //}
        //else
        //{
            DestControl.Controls[0].Controls.Clear();
            this.DestControl.Controls[0].Controls.AddAt(0, row);
            this.DestControl.Controls[0].Controls.AddAt(1, row1);
        //}
    }
    
}

 
 
