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
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using NHibernate.Criterion;
using VirtualMachine.Core;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;



public partial class MoneyManage_GatheringMng_detailInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        IntialPage();
    }
    #region

    private IFinanceMultDataSrv _model;
    private GatheringMaster _curMaster = null;
    private SessionInfo userInfo = null;
    /// <summary>
    /// 收款类型,0:工程款,1:其他
    /// </summary>
    public int GatheringType
    {
        get { return int.Parse(hdGatheringType.Value); }
        set { hdGatheringType.Value = value.ToString(); }
    }
    /// <summary>
    /// 获取对应服务
    /// </summary>
    public IFinanceMultDataSrv Model
    {
        get
        {
            if (_model == null)
            {
                this._model = GlobalClass.FinanceMultDataSrv;
            }
            return this._model;
        }
    }


    public GatheringMaster CurMaster
    {
        get
        {
            if (this._curMaster == null)
            {
                if (IsPostBack)
                {
                    this._curMaster = ViewState["Master"] as GatheringMaster;
                }
                else
                {
                    if (!string.IsNullOrEmpty(hdMasterID.Value))
                    {
                        this._curMaster = Model.GetGatheringMasterById(hdMasterID.Value);
                    }
                    if (this._curMaster == null)
                    {
                        this._curMaster = NewMaster();
                    }
                    ViewState["Master"] = this._curMaster;
                }
            }
            return this._curMaster;
        }
        set
        {
            ViewState["Master"] = value;
            this._curMaster = value;
        }
    }
    /// <summary>
    /// 新建主表
    /// </summary>
    /// <returns></returns>
    public GatheringMaster NewMaster()
    {
        AccountTitleTree oAccountTitleTree = null;
        GatheringMaster oMaster = new GatheringMaster();

        oMaster.CreatePerson = UserInfo.CurrentPersinInfo;
        oMaster.CreatePersonName = UserInfo.CurrentPersinInfo.Name;
        oMaster.CreateDate = DateTime.Now;
        oMaster.OperOrgInfo = UserInfo.CurrentOrgInfo;
        oMaster.OperOrgInfoName = UserInfo.CurrentOrgInfo.Name;
        oMaster.OpgSysCode = UserInfo.CurrentOrgInfo.SysCode;
        oMaster.HandlePerson = UserInfo.CurrentPersinInfo;
        oMaster.HandlePersonName = UserInfo.CurrentPersinInfo.Name;
        //oMaster.ProjectId = UserInfo.CurrentProjectInfo.Id;
        //oMaster.ProjectName = UserInfo.CurrentProjectInfo.Name;
        oMaster.DocState = DocumentState.Edit;

        return oMaster;
    }
    /// <summary>
    /// 新建明细
    /// </summary>
    /// <returns></returns>
    public GatheringDetail NewDetail()
    {
        GatheringDetail oDetail = new GatheringDetail();
        return oDetail;
    }
    /// <summary>
    /// 删除主表
    /// </summary>
    public void DeleteMaster()
    {

        Model.DeleteGatheringMaster(CurMaster);
        chkIsRefresh.Checked = true;
        //UtilClass.ExecuteScript(this, "parent.window.showList(true);");
        UtilClass.ExecuteScript(this, "alert('删除成功'); parent.window.Operate('back');");
    }

    public void ViewToModel()
    {
        if (ValidateData())
        {
            #region 主表
            // this.CurMaster.Code = this.txtCode.Text;
           
            // this.CurMaster.Descript = this.txtDescript.Text;
            this.CurMaster.CreateDate = UtilClass.ToDateTime(this.txtCreateDate.Text);
            this.CurMaster.Descript = this.txtRemark.Text;
            CurMaster.IfProjectMoney = GatheringType;
            //单位选择
            if (!string.IsNullOrEmpty(this.hdTheCustomerID.Value))
            {
                ObjectQuery oQuery = new ObjectQuery();
                oQuery.AddCriterion(Expression.Eq("Id", this.hdTheCustomerID.Value));
                IList lst = GlobalClass.CommonMethodSrv.Query(typeof(CustomerRelationInfo), oQuery);
                CurMaster.TheCustomerRelationInfo = (lst != null && lst.Count > 0) ? (lst[0] as CustomerRelationInfo).Id : null;
                //CurMaster.TheCustomerName = txtTheCustomerName.Text;
            }
            
                CurMaster.TheCustomerName = txtTheCustomerName.Text;
            
            AccountTitleTree currAccountTitle = string.IsNullOrEmpty(this.cmbGatheringType.SelectedValue) ? null : GetAccountTitleTree(this.cmbGatheringType.SelectedValue);
            CurMaster.AccountTitleID =currAccountTitle==null?"": currAccountTitle.Id;
            CurMaster.AccountTitleName = currAccountTitle == null ? "" : currAccountTitle.Name;
            CurMaster.AccountTitleCode = currAccountTitle == null ? "" : currAccountTitle.Code;
            CurMaster.AccountTitleSyscode = currAccountTitle == null ? "" : currAccountTitle.SysCode;
            #endregion
            #region 明细
            //this.gvDetail.DataSource = this.CurMaster.Details;
            //this.gvDetail.DataBind();

            #endregion

        }
    }
    public void ModelToView()
    {
        AccountTitleTree oAccountTitleTree = null;
        #region 主表
        
        this.txtCode.Text = CurMaster.Code;
        if (!IsPostBack)
        {
            this.txtCreateDate.Text = this.CurMaster.CreateDate.ToString("yyyy-MM-dd");

            txtRemark.Text = CurMaster.Descript;
            txtTheCustomerName.Text = CurMaster.TheCustomerName;
            if (CurMaster.TheCustomerRelationInfo != null)
            {
                hdTheCustomerID.Value = CurMaster.TheCustomerRelationInfo;
            }
            if (GatheringType == 1 && !string.IsNullOrEmpty(CurMaster.AccountTitleID))
            {
                this.cmbGatheringType.SelectedValue = CurMaster.AccountTitleID;
            }
        }
       
        #endregion
        #region 明细



        //管理费用
        this.gvDetailSource.DataSource = this.CurMaster.Details;
        if (!IsReadyOnly)
        {
            this.gvDetailSource.EnabledClientDeleteClick = true;
            //this.gvDetailSource.ClientDeleteClick = "operateGrid";
            this.gvDetailSource.EnabledClientNewClick = true;
            //this.gvDetailSource.ClientNewClick = "operateGrid";
            this.gvDetailSource.EnabledClientUpdateClick = true;
            //this.gvDetailSource.ClientUpdateClick = "operateGrid";
        }
        this.gvDetailSource.EnabledClientDoubleClick = true;
        this.gvDetailSource.ClientClick = "operateGrid";
        this.gvDetailSource.DataBind();

        //this.gvMasterSource.IsSelect = true;
        //this.gvMasterSource.DataSource = this.CurMaster.Details;
        //this.gvMasterSource.ClientDoubleClick = "dbDetailClick";
        //this.gvMasterSource.DataBind();
        //this.gvDetail.DataSource = this.CurMaster.Details;
        //this.gvDetail.DataBind();
        #endregion
    }
    #region
    //public void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        string sRowData = string.Empty;
    //        int iRowIndex = e.Row.RowIndex;
    //        int iColumnIndex = -1;
    //        IndirectCostDetail oDetail = e.Row.DataItem as IndirectCostDetail;
    //        if (oDetail != null)
    //        {
    //            iColumnIndex = UtilClass.GetColumnIndex(this.gvDetail, "序号");
    //            if (iColumnIndex > -1)
    //                e.Row.Cells[iColumnIndex].Text = (iRowIndex + 1).ToString();
    //            sRowData = UtilClass.ObjectToJson(oDetail);
    //            if (!string.IsNullOrEmpty(sRowData))
    //            {
    //                //e.Row.Attributes.Add("Data",  sRowData );
    //                //当鼠标停留时更改背景色
    //                e.Row.Attributes.Add("onmouseover", "curBak=this.style.backgroundColor;this.style.color='#116a79';this.style.backgroundColor='#cef97b'");
    //                //当鼠标移开时还原背景色
    //                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=curBak;this.style.color='#3a6381'");
    //                e.Row.Attributes["style"] = "Cursor:pointer";
    //                // e.Row.Attributes.Add("onmouseover", string.Format("mouseOver({0})", iRowIndex));
    //                e.Row.Attributes.Add("onclick", string.Format("singleDetailClick({0},decodeComponent('{1}'))", iRowIndex, sRowData));
    //                e.Row.Attributes.Add("ondblclick", string.Format("dbDetailClick({0},decodeComponent('{1}'))", iRowIndex, sRowData));
    //                //e.Row.Attributes.Add("onmouseout", string.Format("mouseOut({0})", iRowIndex));

    //            }
    //        }
    //    }

    //}
    #endregion

    public bool ValidateData()
    {
        return true;
    }
    public SessionInfo UserInfo
    {
        get
        {
            if (userInfo == null)
            {
                userInfo = PublicClass.GetUseInfo(Session);
            }
            return userInfo;
        }
    }
    public PageState PageState
    {
        get
        {
            if (string.IsNullOrEmpty(hdPageState.Value))
            {
                hdPageState.Value = PageState.Empty.ToString();
            }
            return (PageState)Enum.Parse(typeof(PageState), hdPageState.Value);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public bool IsReadyOnly
    {
        get { return UtilClass.GetPageReadOnly(PageState); }
    }
    public void Save()
    {
        IList removeDetailList = new ArrayList();
        if (CurMaster != null)
        {
            if (this.ValidateData())
            {
                ViewToModel();
                this.gvDetailSource.ClearNewBillID(CurMaster, removeDetailList);
                //this.gvOtherPayoutSource.ClearNewBillID(CurMaster, removeDetailList);
                //this.gvOtherReceiveSource.ClearNewBillID(CurMaster, removeDetailList);
                if (string.IsNullOrEmpty(CurMaster.Id))
                {
                    CurMaster = Model.SaveWebGatheringMaster(CurMaster) as GatheringMaster;
                }
                else
                {

                    CurMaster = Model.SaveWebGatheringMaster(CurMaster) as GatheringMaster;
                    //CurMaster = Model.Update(CurMaster, this.gvMasterSource.RemoveList) as IndirectCostMaster;
                }
                this.gvDetailSource.ClearUpdate();
                //this.gvOtherPayoutSource.ClearUpdate();
                //this.gvOtherReceiveSource.ClearUpdate();
                //RemoveDetailList.Clear();
                //NewDetailList.Clear();
                chkIsRefresh.Checked = true;
                if (CurMaster.DocState == DocumentState.InExecute)
                {
                    UtilClass.ExecuteScript(this, "alert('提交成功'); parent.window.Operate('back');");
                }
                else
                {
                    UtilClass.ExecuteScript(this, "alert('保存成功');");
                }
            }
        }

    }
    public void IntialPage()
    {
        if (!IsPostBack)
        {

            this.gvDetailSource.DestControl = this.gvDetail;
            hdMasterID.Value = UtilClass.QueryString(this, "ID");
            hdPageState.Value = UtilClass.QueryString(this, "PageState");
            if (PageState == PageState.Empty)
            {
                return;
            }
            else
            {
                if (PageState.Delete == PageState)
                {
                    DeleteMaster();
                    return;
                }
                else
                {
                    InitalData();
                    InitalControl();
                    InitalEvent();
                    ModelToView();
                }
            }
        }
        else
        {
            if (PageState.New == PageState)
            {
                ViewToModel();
            }
            InitalEvent();

        }

    }
    public void InitalData()
    {
        ObjectQuery oq = new ObjectQuery();
        oq.AddCriterion(Expression.Eq("BusinessFlag", "01"));//获取收款类别的会计科目
       IList acctTitleList =  GlobalClass.AccountTitleTreeSvr.GetAccountTitleTreeByQuery(oq);
        IList otherGatherList = new ArrayList();
        foreach (AccountTitleTree title in acctTitleList)
        {
            if (title.Code == "112201" || title.Code == "112206")
            {
               // gathingAcctTitle = title;
            }
            else
            {
                otherGatherList.Add(title);
            }
        }
        otherGatherList.Insert(0, new AccountTitleTree());
        cmbGatheringType.DataSource = otherGatherList;
        cmbGatheringType.DataTextField = "Name";
        cmbGatheringType.DataValueField = "Id";
        cmbGatheringType.DataBind();
    }
    public void InitalControl()
    {
        Control[] arrControl = new Control[]{this.txtCreateDate,this.cmbGatheringType,this.txtRemark};
        UtilClass.SetControl(arrControl, PageState);
        //this.txtAccountTitle.ReadOnly = true;
        //this.txtOrgName.ReadOnly = true;
    }
    public void InitalEvent()
    {

        this.btnDeleteMaster.Click += new EventHandler(this.btnDeleteMasterClick);
        this.btnSaveMaster.Click += new EventHandler(this.btnSaveMasterClick);
        this.btnSubmitMaster.Click += new EventHandler(this.btnSubmitMasterClick);
        //this.gvDetail.RowDataBound += new GridViewRowEventHandler(this.gvDetail_RowDataBound);
        this.btnAddOrUpdateDetail.Click += new EventHandler(this.btnAddOrUpdateDetailClick);
    }
    #region 主表增删改
    public void btnSaveMasterClick(object sender, EventArgs e)
    {

        Save();
        ModelToView();

    }
    public void btnSubmitMasterClick(object sender, EventArgs e)
    {
        CurMaster.DocState = DocumentState.InExecute;
        //this.btnSave.Click();
        btnSaveMasterClick(sender, e);
    }
    public void btnDeleteMasterClick(object sender, EventArgs e)
    {
        DeleteMaster();
    }
    #endregion
    #region 明细

    public void btnAddOrUpdateDetailClick(object sender, EventArgs e)
    {
        bool IsNew = false;
        GatheringDetail oDetail = null;

        if (string.IsNullOrEmpty(hdDetailID.Value))
        {
            oDetail = NewDetail();
            oDetail.Master = CurMaster;
            CurMaster.Details.Add(oDetail);
            oDetail.Id = Guid.NewGuid().ToString();
            IsNew = true;
            this.gvDetailSource.NewBillList.Add(oDetail);
           
        }
        else
        {
            oDetail = CurMaster.Details.OfType<GatheringDetail>().FirstOrDefault(item => item.Id == hdDetailID.Value);
        }
         
        oDetail.Descript = txtDescript.Text;
        oDetail.Money = UtilClass.ToDecimal(txtMoney.Text);
        ModelToView();
    }

    public string GetRate(object ActualMoney, object BudgetMoney)
    {
        decimal dActualMoney = UtilClass.ToDecimal(ActualMoney);
        decimal dBudgetMoney = UtilClass.ToDecimal(BudgetMoney);
        return dActualMoney == 0 ? "--" : (Math.Round(dBudgetMoney / dActualMoney, 2) * 100).ToString();
    }
    public AccountTitleTree GetAccountTitleTree(string sID)
    {
        return GlobalClass.AccountTitleTreeSvr.GetAccountTitleTreeById(sID);
    }
   
    #endregion
    #endregion
}
