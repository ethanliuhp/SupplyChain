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
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.Collections.Generic;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using System.IO;
using System.Text;
using System.Diagnostics;
using NPOI.SS.UserModel;

public partial class MoneyManage_CompanyAccountMng_detailInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        IntialPage();
    }
    #region

    private IIndirectCostSvr _model;
    private IndirectCostMaster _curMaster = null;
    private SessionInfo userInfo = null;

    /// <summary>
    /// 获取对应服务
    /// </summary>
    public IIndirectCostSvr Model
    {
        get
        {
            if (_model == null)
            {
                this._model = GlobalClass.IndirectCostSvr;
            }
            return this._model;
        }
    }


    public IndirectCostMaster CurMaster
    {
        get
        {
            if (this._curMaster == null)
            {
                if (IsPostBack)
                {
                    this._curMaster = ViewState["Master"] as IndirectCostMaster;
                }
                else
                {
                    if (!string.IsNullOrEmpty(hdMasterID.Value))
                    {
                        this._curMaster = Model.GetIndirectCostMasterByID(hdMasterID.Value);
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
    public IndirectCostMaster NewMaster()
    {
        AccountTitleTree oAccountTitleTree=null;
        IndirectCostMaster oMaster = new IndirectCostMaster();

        oMaster.CreatePerson = UserInfo.CurrentPersinInfo;
        oMaster.CreatePersonName = UserInfo.CurrentPersinInfo.Name;
        oMaster.CreateDate = DateTime.Now;
        oMaster.OperOrgInfo = UserInfo.CurrentOrgInfo;
        oMaster.OperOrgInfoName = UserInfo.CurrentOrgInfo.Name;
        oMaster.OpgSysCode = UserInfo.CurrentOrgInfo.SysCode;
        oMaster.HandlePerson = UserInfo.CurrentPersinInfo;
        oMaster.HandlePersonName = UserInfo.CurrentPersinInfo.Name;
        oMaster.DocState = DocumentState.Edit;

        Hashtable ht=GlobalClass.AccountTitleTreeSvr.GetBasicAccountTitleTree();
        //借款
        oMaster.BorrowSymbol = new IndirectCostDetail() { Master = oMaster,  AccountSymbol = EnumAccountSymbol.借款标志, CostType = EnumCostType.其他 };
        if(ht["短期借款"]!=null){
            oMaster.BorrowSymbol.AccountTitle =ht["短期借款"] as AccountTitleTree;
            oMaster.BorrowSymbol.AccountTitleCode = oMaster.BorrowSymbol.AccountTitle.Code;
            oMaster.BorrowSymbol.AccountTitleID = oMaster.BorrowSymbol.AccountTitle.Id;
            oMaster.BorrowSymbol.AccountTitleName = oMaster.BorrowSymbol.AccountTitle.Name;
        }
        
        //利润
        //oMaster.ProfitSymbol = new IndirectCostDetail() { Master = oMaster, AccountSymbol = EnumAccountSymbol.利润标志, CostType = EnumCostType.其他 };
        //if (ht["利润"] != null)
        //{
        //    oMaster.ProfitSymbol.AccountTitle = ht["利润"] as AccountTitleTree;
        //    oMaster.ProfitSymbol.AccountTitleCode = oMaster.ProfitSymbol.AccountTitle.Code;
        //    oMaster.ProfitSymbol.AccountTitleID = oMaster.ProfitSymbol.AccountTitle.Id;
        //    oMaster.ProfitSymbol.AccountTitleName = oMaster.ProfitSymbol.AccountTitle.Name;
        //}
        //财务
        oMaster.FinanceCostSymbol=new   IndirectCostDetail() { Master = oMaster, AccountSymbol = EnumAccountSymbol.财务费用标志, CostType = EnumCostType.其他 };
        if (ht["财务费用"] != null)
        {
            oMaster.FinanceCostSymbol.AccountTitle = ht["财务费用"] as AccountTitleTree;
            oMaster.FinanceCostSymbol.AccountTitleCode = oMaster.FinanceCostSymbol.AccountTitle.Code;
            oMaster.FinanceCostSymbol.AccountTitleID = oMaster.FinanceCostSymbol.AccountTitle.Id;
            oMaster.FinanceCostSymbol.AccountTitleName = oMaster.FinanceCostSymbol.AccountTitle.Name;
        }
        //上交
        oMaster.HandOnSymbol = new IndirectCostDetail() { Master = oMaster, AccountSymbol = EnumAccountSymbol.上交标志, CostType = EnumCostType.其他 };
        if (ht["货币上交"] != null)
        {
            oMaster.HandOnSymbol.AccountTitle = ht["货币上交"] as AccountTitleTree;
            oMaster.HandOnSymbol.AccountTitleCode = oMaster.HandOnSymbol.AccountTitle.Code;
            oMaster.HandOnSymbol.AccountTitleID = oMaster.HandOnSymbol.AccountTitle.Id;
            oMaster.HandOnSymbol.AccountTitleName = oMaster.HandOnSymbol.AccountTitle.Name;
        }
        return oMaster;
    }
    /// <summary>
    /// 新建明细
    /// </summary>
    /// <returns></returns>
    public IndirectCostDetail NewDetail()
    {
        IndirectCostDetail oDetail = new IndirectCostDetail();
        return oDetail;
    }
    /// <summary>
    /// 删除主表
    /// </summary>
    public void DeleteMaster()
    {

        Model.Delete(CurMaster);
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
            this.CurMaster.Year = int.Parse(this.dpLstYear.Text);
            this.CurMaster.Month = int.Parse(this.dpLstMonth.Text);
           // this.CurMaster.Descript = this.txtDescript.Text;
            this.CurMaster.EndTime = UtilClass.ToDateTime(this.txtEndTime.Text);
            CurMaster.CreateDate = UtilClass.ToDateTime(this.txtEndTime.Text);
            this.CurMaster.FinanceCostSymbol.Money = UtilClass.ToDecimal(txtFinanceCostSymbolMoney.Text);
            //if (!string.IsNullOrEmpty(this.hdFinanceCostSymbolAccountTtileTreeID.Value) &&
            //    ((this.CurMaster.FinanceCostSymbol.AccountTitle == null) ||
            //    (!string.Equals(this.CurMaster.FinanceCostSymbol.AccountTitle.Id, this.hdFinanceCostSymbolAccountTtileTreeID.Value))))
            //{
           
               // this.CurMaster.FinanceCostSymbol.AccountTitle = GetAccountTitleTree(this.hdFinanceCostSymbolAccountTtileTreeID.Value);
               // this.CurMaster.FinanceCostSymbol.AccountTitleCode = this.CurMaster.FinanceCostSymbol.AccountTitle == null ? "" : this.CurMaster.FinanceCostSymbol.AccountTitle.Code;
               // this.CurMaster.FinanceCostSymbol.AccountTitleName = this.CurMaster.FinanceCostSymbol.AccountTitle == null ? "" : this.CurMaster.FinanceCostSymbol.AccountTitle.Name;
                //this.CurMaster.FinanceCostSymbol.AccountTitleID = this.CurMaster.FinanceCostSymbol.AccountTitle == null ? "" : this.CurMaster.FinanceCostSymbol.AccountTitle.Id;
           // }
            this.CurMaster.BorrowSymbol.Money = UtilClass.ToDecimal(this.txtBorrowSymbolMoney.Text);
            //if (!string.IsNullOrEmpty(this.hdBorrowSymbolAccountTtileTreeID.Value) &&
            //    ((this.CurMaster.BorrowSymbol.AccountTitle == null) ||
            //    (!string.Equals(this.CurMaster.BorrowSymbol.AccountTitle.Id, this.hdBorrowSymbolAccountTtileTreeID.Value))))
            //{
            //    this.CurMaster.BorrowSymbol.AccountTitle = GetAccountTitleTree(this.hdBorrowSymbolAccountTtileTreeID.Value);
            //    this.CurMaster.BorrowSymbol.AccountTitleCode = this.CurMaster.BorrowSymbol.AccountTitle == null ? "" : this.CurMaster.BorrowSymbol.AccountTitle.Code;
            //    this.CurMaster.BorrowSymbol.AccountTitleName = this.CurMaster.BorrowSymbol.AccountTitle == null ? "" : this.CurMaster.BorrowSymbol.AccountTitle.Name;
            //    this.CurMaster.BorrowSymbol.AccountTitleID = this.CurMaster.BorrowSymbol.AccountTitle == null ? "" : this.CurMaster.BorrowSymbol.AccountTitle.Id;
            //}
            this.CurMaster.HandOnSymbol.Money = UtilClass.ToDecimal(txtHandOnSymbolMoney.Text);
            //if (!string.IsNullOrEmpty(this.hdHandOnSymbolAccountTtileTreeID.Value) &&
            //  ((this.CurMaster.HandOnSymbol.AccountTitle == null) ||
            //  (!string.Equals(this.CurMaster.HandOnSymbol.AccountTitle.Id, this.hdHandOnSymbolAccountTtileTreeID.Value))))
            //{
            //    this.CurMaster.HandOnSymbol.AccountTitle = GetAccountTitleTree(this.hdHandOnSymbolAccountTtileTreeID.Value);
            //    this.CurMaster.HandOnSymbol.AccountTitleCode = this.CurMaster.HandOnSymbol.AccountTitle == null ? "" : this.CurMaster.HandOnSymbol.AccountTitle.Code;
            //    this.CurMaster.HandOnSymbol.AccountTitleName = this.CurMaster.HandOnSymbol.AccountTitle == null ? "" : this.CurMaster.HandOnSymbol.AccountTitle.Name;
            //    this.CurMaster.HandOnSymbol.AccountTitleID = this.CurMaster.HandOnSymbol.AccountTitle == null ? "" : this.CurMaster.HandOnSymbol.AccountTitle.Id;
            //}
            //this.CurMaster.ProfitSymbol.Money = UtilClass.ToDecimal(txtProfitSymbolMoney.Text);
            //if (!string.IsNullOrEmpty(this.hdProfitSymbolAccountTtileTreeID.Value) &&
            //  ((this.CurMaster.ProfitSymbol.AccountTitle == null) ||
            //  (!string.Equals(this.CurMaster.ProfitSymbol.AccountTitle.Id, this.hdProfitSymbolAccountTtileTreeID.Value))))
            //{
            //    this.CurMaster.ProfitSymbol.AccountTitle = GetAccountTitleTree(this.hdProfitSymbolAccountTtileTreeID.Value);
            //    this.CurMaster.ProfitSymbol.AccountTitleCode = this.CurMaster.ProfitSymbol.AccountTitle == null ? "" : this.CurMaster.ProfitSymbol.AccountTitle.Code;
            //    this.CurMaster.ProfitSymbol.AccountTitleName = this.CurMaster.ProfitSymbol.AccountTitle == null ? "" : this.CurMaster.ProfitSymbol.AccountTitle.Name;
            //    this.CurMaster.ProfitSymbol.AccountTitleID = this.CurMaster.ProfitSymbol.AccountTitle == null ? "" : this.CurMaster.ProfitSymbol.AccountTitle.Id;
            //}
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
        if (!IsPostBack)
        {
            //this.txtCode.Text = this.CurMaster.Code;
            this.dpLstYear.Text = this.CurMaster.Year.ToString();
            this.dpLstMonth.Text = this.CurMaster.Month.ToString();
            //this.txtDescript.Text = this.CurMaster.Descript;
            this.txtEndTime.Text = this.CurMaster.CreateDate.ToString("yyyy-MM-dd");
            //借款标志
            //oAccountTitleTree = this.CurMaster.BorrowSymbol.AccountTitle == null ? null : GetAccountTitleTree(this.CurMaster.BorrowSymbol.AccountTitle.Id);
            this.txtBorrowSymbolMoney.Text = this.CurMaster.BorrowSymbol.Money.ToString("#0.00");
            //this.txtBorrowSymbolAccountTtileTreeName.Text = oAccountTitleTree == null ? "" : oAccountTitleTree.Name;
            //this.hdBorrowSymbolAccountTtileTreeID.Value = oAccountTitleTree == null ? "" : oAccountTitleTree.Id;
            //财务费用
            //oAccountTitleTree = this.CurMaster.FinanceCostSymbol.AccountTitle == null ? null : GetAccountTitleTree(this.CurMaster.FinanceCostSymbol.AccountTitle.Id);
            this.txtFinanceCostSymbolMoney.Text = this.CurMaster.FinanceCostSymbol.Money.ToString("#0.00");
            //this.txtFinanceCostSymbolAccountTtileTreeName.Text = oAccountTitleTree == null ? "" : oAccountTitleTree.Name;
            //this.hdFinanceCostSymbolAccountTtileTreeID.Value = oAccountTitleTree == null ? "" : oAccountTitleTree.Id;
            //利润
            //oAccountTitleTree = this.CurMaster.ProfitSymbol.AccountTitle == null ? null : GetAccountTitleTree(this.CurMaster.ProfitSymbol.AccountTitle.Id);
            // this.txtProfitSymbolMoney.Text = this.CurMaster.ProfitSymbol.Money.ToString("#0.00");
            //this.txtProfitSymbolAccountTtileTreeName.Text = oAccountTitleTree == null ? "" : oAccountTitleTree.Name;
            //this.hdProfitSymbolAccountTtileTreeID.Value = oAccountTitleTree == null ? "" : oAccountTitleTree.Id;
            //上交
            // oAccountTitleTree = this.CurMaster.HandOnSymbol.AccountTitle == null ? null : GetAccountTitleTree(this.CurMaster.HandOnSymbol.AccountTitle.Id);
            this.txtHandOnSymbolMoney.Text = this.CurMaster.HandOnSymbol.Money.ToString("#0.00");
            //this.txtHandOnSymbolAccountTtileTreeName.Text = oAccountTitleTree == null ? "" : oAccountTitleTree.Name;
            //this.hdHandOnSymbolAccountTtileTreeID.Value = oAccountTitleTree == null ? "" : oAccountTitleTree.Id;
        }
        #endregion
        #region 明细
  
         

        //管理费用
        this.gvManageCostSource.DataSource = this.CurMaster.ManageCost;
        if (!IsReadyOnly)
        {
            this.gvManageCostSource.EnabledClientDeleteClick = true;
            //this.gvManageCostSource.ClientDeleteClick = "operateGrid";
            this.gvManageCostSource.EnabledClientNewClick = true;
            //this.gvManageCostSource.ClientNewClick = "operateGrid";
            this.gvManageCostSource.EnabledClientUpdateClick = true;
            //this.gvManageCostSource.ClientUpdateClick = "operateGrid";
        }
        this.gvManageCostSource.EnabledClientDoubleClick = true;
        this.gvManageCostSource.ClientClick = "operateGrid";
        this.gvManageCostSource.DataBind();

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
                 this.gvManageCostSource.ClearNewBillID(CurMaster,  removeDetailList);
                 //this.gvOtherPayoutSource.ClearNewBillID(CurMaster, removeDetailList);
                 //this.gvOtherReceiveSource.ClearNewBillID(CurMaster, removeDetailList);
                if (string.IsNullOrEmpty(CurMaster.Id))
                {
                    CurMaster = Model.Save(CurMaster) as IndirectCostMaster;
                }
                else
                {
                   
                    CurMaster = Model.Update(CurMaster, removeDetailList) as IndirectCostMaster;
                    //CurMaster = Model.Update(CurMaster, this.gvMasterSource.RemoveList) as IndirectCostMaster;
                }
                this.gvManageCostSource.ClearUpdate();
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
            
            this.gvManageCostSource.DestControl = this.gvManageCost;
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
        this.dpLstMonth.Items.Clear();
        for (int iMonth = 1; iMonth < 13; iMonth++)
        {
            this.dpLstMonth.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
        }
        this.dpLstMonth.SelectedValue = DateTime.Now.Month.ToString();
        this.dpLstYear.Items.Clear();
        for (int iYear = 2012; iYear < DateTime.Now.Year + 1; iYear++)
        {
            this.dpLstYear.Items.Add(new ListItem(iYear.ToString(), iYear.ToString()));
        }
        this.dpLstYear.SelectedValue = DateTime.Now.Year.ToString();
        this.selPartnerType.Items.Clear();
        this.selPartnerType.Items.Insert(this.selPartnerType.Items.Count, new ListItem("",""));
        this.selPartnerType.Items.Insert(this.selPartnerType.Items.Count, new ListItem("甲方", "甲方"));
        this.selPartnerType.Items.Insert(this.selPartnerType.Items.Count, new ListItem("分供方", "分供方"));
        this.selPartnerType.SelectedIndex = 0;
    }
    public void InitalControl()
    {
        Control[] arrControl = new Control[]{this.dpLstYear,this.dpLstMonth,this.txtEndTime,
                                             this.txtBorrowSymbolMoney,this.txtFinanceCostSymbolMoney,
                                             this.txtHandOnSymbolMoney };
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
        IndirectCostDetail oDetail = null;
       
        if (string.IsNullOrEmpty(hdDetailID.Value))
        {
            oDetail = NewDetail();
            oDetail.Master = CurMaster;
            CurMaster.Details.Add(oDetail);
            oDetail.Id = Guid.NewGuid().ToString();
            IsNew = true;
            oDetail.CostType = GetEnumCostType();
            oDetail.AccountSymbol = EnumAccountSymbol.其他;
            switch (oDetail.CostType)
            {
                case EnumCostType.管理费用: { this.gvManageCostSource.NewBillList.Add(oDetail);break; }
                //case EnumCostType.其他应付: { this.gvOtherPayoutSource.NewBillList.Add(oDetail); break; }
                //case EnumCostType.其他应收: { this.gvOtherReceiveSource.NewBillList.Add(oDetail); break; }
            }
        }
        else
        {
            oDetail = CurMaster.Details.OfType<IndirectCostDetail>().FirstOrDefault(item => item.Id == hdDetailID.Value);
            
        }
        if (!string.IsNullOrEmpty(hdAccountTitleID.Value) && (IsNew || oDetail.AccountTitle == null||(oDetail.AccountTitle != null && oDetail.AccountTitle.Id != hdAccountTitleID.Value)))
        {
            oDetail.AccountTitle = GetAccountTitleTree(hdAccountTitleID.Value);//需要查询
            oDetail.AccountTitleCode = oDetail.AccountTitle == null ? "" : oDetail.AccountTitle.Code;
            oDetail.AccountTitleID = oDetail.AccountTitle == null ? "" : oDetail.AccountTitle.Id;
            oDetail.AccountTitleName = oDetail.AccountTitle == null ? "" : oDetail.AccountTitle.Name;
        }
        if (!string.IsNullOrEmpty(this.hdOrgID.Value) && (IsNew || oDetail.OrgInfo == null||(oDetail.OrgInfo != null && oDetail.OrgInfo.Id != hdOrgID.Value)))
        {
            oDetail.OrgInfo = GetOperationOrgInfo(this.hdOrgID.Value);
            oDetail.OrgInfoID = oDetail.OrgInfo == null ? "" : oDetail.OrgInfo.Id;
            oDetail.OrgInfoName = oDetail.OrgInfo == null ? "" : oDetail.OrgInfo.Name;
            oDetail.OrgInfoSysCode = oDetail.OrgInfo == null ? "" : oDetail.OrgInfo.SysCode;
        }
        oDetail.PartnerType = selPartnerType.SelectedValue;
        oDetail.Descript = txtDescript.Text;
        oDetail.Money = UtilClass.ToDecimal(txtMoney.Text);
        oDetail.BudgetMoney = UtilClass.ToDecimal(this.txtBudgetMoney.Text);
        ModelToView();
    }
    public void btnImportExcel_Click(object sender, EventArgs e)
    {
        string sPathFile = this.hdFilePath.Value;
        if (!string.IsNullOrEmpty(sPathFile))
        {
           
          OpenExcel(sPathFile);
         
          //foreach (IndirectCostDetail oDetail in lstDetail)
          //{
              
          //    if (!string.IsNullOrEmpty(oDetail.AccountTitleCode) && lstAccount!=null && lstAccount.Count>0)
          //    {
          //        oDetail.AccountTitle = GetAccountTitleTree(lstAccount, oDetail.AccountTitleCode);
          //        if (oDetail.AccountTitle != null)
          //        {
          //            oDetail.AccountTitleID = oDetail.AccountTitle.Id;
          //            oDetail.AccountTitleName = oDetail.AccountTitle.Name;
          //            oDetail.AccountTitleSyscode = oDetail.AccountTitle.SysCode;
          //        }
          //    }
          //    //所属组织设置
          //    CurMaster.Details.Add(oDetail);
          //    this.gvManageCostSource.NewBillList.Add(oDetail);
           
          //}
          ModelToView();
        }
    }
    private void  OpenExcel(string strFileName)
    {

        //IList<IndirectCostDetail> lstDetail = null;
        try
        {
            string sFlag = string.Empty;
            string sAccountTitleCode = string.Empty;
            string sOrgName = string.Empty;
            string sMoney = string.Empty;
            double  dMoney = 0;
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
                        oDetail = NewDetail();
                        oDetail.Master = CurMaster;
                        oDetail.Id = Guid.NewGuid().ToString();
                        oDetail.CostType = EnumCostType.管理费用;
                        oDetail.AccountSymbol = EnumAccountSymbol.其他;
                        sAccountTitleCode = oRow.GetCell( 1).StringCellValue;
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
 
    public string GetRate(object ActualMoney, object BudgetMoney)
    {
      decimal dActualMoney=  UtilClass.ToDecimal(ActualMoney);
      decimal dBudgetMoney = UtilClass.ToDecimal(BudgetMoney);
        return dActualMoney==0? "--":(Math.Round( dBudgetMoney/dActualMoney,2)*100).ToString();
    }
    public AccountTitleTree GetAccountTitleTree(string sID)
    {
      return   GlobalClass.AccountTitleTreeSvr.GetAccountTitleTreeById(sID);
    }
    public AccountTitleTree GetAccountTitleTree(IList lstAccountTitle,string sCode)
    {
        AccountTitleTree oAccountTitleTree=null;
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
    public OperationOrgInfo GetOperationOrgInfo(string sID)
    {
       return  GlobalClass.CommonMethodSrv.GetOperationOrgInfo(sID);
    }
    public EnumCostType GetEnumCostType()
    {
        EnumCostType costType=EnumCostType.其他;
        string sValue = hdCostType.Value;
        if (!string.IsNullOrEmpty(sValue))
        {
            costType = (EnumCostType)Enum.Parse(typeof(EnumCostType), sValue);
        }
        return costType;
    }
    #endregion
    #endregion
}
