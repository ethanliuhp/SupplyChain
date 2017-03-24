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
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

public partial class MoneyManage_PaymentMng_detailInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        IntialPage();
    }
    #region

    private IFinanceMultDataSrv _model;
    private PaymentMaster _curMaster = null;
    private SessionInfo userInfo = null;
    /// <summary>
    /// 付款类型,0:工程款,1:其他
    /// </summary>
    public int PaymentType
    {
        get { return int.Parse(hdPaymentType.Value); }
        set { hdPaymentType.Value = value.ToString(); }
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


    public PaymentMaster CurMaster
    {
        get
        {
            if (this._curMaster == null)
            {
                if (IsPostBack)
                {
                    this._curMaster = ViewState["Master"] as PaymentMaster;
                }
                else
                {
                    if (!string.IsNullOrEmpty(hdMasterID.Value))
                    {
                        this._curMaster = Model.GetPaymentMasterById(hdMasterID.Value);
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
    public PaymentMaster NewMaster()
    {
        AccountTitleTree oAccountTitleTree = null;
        PaymentMaster oMaster = new PaymentMaster();

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
        oMaster.IfProjectMoney = this.PaymentType;
        return oMaster;
    }
    /// <summary>
    /// 新建明细
    /// </summary>
    /// <returns></returns>
    public PaymentDetail NewDetail()
    {
        PaymentDetail oDetail = new PaymentDetail();
        return oDetail;
    }
    /// <summary>
    /// 删除主表
    /// </summary>
    public void DeleteMaster()
    {

        Model.DeletePaymentMaster(CurMaster);
        chkIsRefresh.Checked = true;
        //UtilClass.ExecuteScript(this, "parent.window.showList(true);");
        UtilClass.ExecuteScript(this, "alert('删除成功'); parent.window.Operate('back');");
    }

    public void ViewToModel()
    {
        if (ValidateData())
        {
            #region 主表
            CurMaster.CreateDate = ClientUtil.ToDateTime(txtCreateDate.Text );
            CurMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
            
           // CurMaster.BankAddress = ClientUtil.ToString(this.hdBankAddress.Value);
            CurMaster.IfProjectMoney = PaymentType;
            if (!string.IsNullOrEmpty(this.hdSupplyID.Value))
            {
                ObjectQuery oQuery=new ObjectQuery ();
                oQuery.AddCriterion(Expression.Eq("Id",this.hdSupplyID.Value));
                IList lst=GlobalClass.CommonMethodSrv.Query(typeof(SupplierRelationInfo),oQuery); 
                SupplierRelationInfo oTheSupplierRelationInfo=(lst!=null && lst.Count>0) ? (lst[0] as SupplierRelationInfo):null;

                CurMaster.TheSupplierRelationInfo =oTheSupplierRelationInfo==null?"":oTheSupplierRelationInfo.Id;
                CurMaster.BankAccountNo = oTheSupplierRelationInfo == null ? "" : oTheSupplierRelationInfo.BankAccount;
                    CurMaster.BankName = oTheSupplierRelationInfo==null?"":oTheSupplierRelationInfo.BankName;
                    //CurMaster.BankAddress=CurMaster.TheSupplierRelationInfo.
                
            }
            CurMaster.BankAccountNo = ClientUtil.ToString(this.txtBankAcctNo.Text);
            CurMaster.BankName = ClientUtil.ToString(this.txtBankNo.Text);
            CurMaster.TheSupplierName = txtSupply.Text;


            if (!string.IsNullOrEmpty(cmbPaymentType.SelectedValue))
            {
                AccountTitleTree currAccountTitle = GetAccountTitleTree(cmbPaymentType.SelectedValue);
                CurMaster.AccountTitleID = currAccountTitle == null ? "" : currAccountTitle.Id;
                CurMaster.AccountTitleName = currAccountTitle == null ? "" : currAccountTitle.Name;
                CurMaster.AccountTitleCode = currAccountTitle == null ? "" : currAccountTitle.Code;
                CurMaster.AccountTitleSyscode = currAccountTitle == null ? "" : currAccountTitle.SysCode;
            }
           
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
                txtCreateDate.Text = CurMaster.CreateDate.ToString("yyyy-MM-dd");
                this.txtRemark.Text = CurMaster.Descript;
                this.cmbPaymentType.SelectedValue = CurMaster.AccountTitleID;
                //this.hdBankAddress.Value = CurMaster.BankAddress;
                if (CurMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Text = CurMaster.TheSupplierName;
                    hdSupplyID.Value = CurMaster.TheSupplierRelationInfo;
                    this.txtBankAcctNo.Text = CurMaster.BankAccountNo;// CurMaster.TheSupplierRelationInfo.BankAccount;
                    this.txtBankNo.Text = CurMaster.BankName;//CurMaster.TheSupplierRelationInfo.BankName;
                }
                else
                {
                    txtSupply.Text = CurMaster.TheSupplierName;
                }
                this.txtBankAcctNo.Text = CurMaster.BankAccountNo;
                this.txtBankNo.Text = CurMaster.BankName;
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
        #endregion
    }
  

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
                    CurMaster = Model.SaveWebPaymentMaster(CurMaster) as PaymentMaster;
                }
                else
                {

                    CurMaster = Model.SaveWebPaymentMaster(CurMaster) as PaymentMaster;
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
        oq.AddCriterion(Expression.Eq("BusinessFlag", "02"));
       IList acctTitleList = GlobalClass.AccountTitleTreeSvr.GetAccountTitleTreeByQuery(oq);
        IList otherPaymentList = new ArrayList();
        foreach (AccountTitleTree title in acctTitleList)
        {
            if (title.Code == "220202" || title.Code == "220203" || title.Code == "220204" || title.Code == "220205" || title.Code == "220299"
                    || title.Code == "22020801" || title.Code == "22020802" || title.Code == "22020803" || title.Code == "22020804")
            {
                //paymentAcctTitle = title;
            }
            else
            {
                otherPaymentList.Add(title);
            }
        }
        otherPaymentList.Insert(0, new AccountTitleTree());
        this.cmbPaymentType.DataSource = otherPaymentList;
        cmbPaymentType.DataTextField = "Name";
        cmbPaymentType.DataValueField = "Id";
        this.cmbPaymentType.DataBind();
    }
    public void InitalControl()
    {
        Control[] arrControl = new Control[] { this.txtCreateDate,this.txtRemark, this.cmbPaymentType,this.txtBankAcctNo,this.txtBankNo
        };
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
        PaymentDetail oDetail = null;

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
            oDetail = CurMaster.Details.OfType<PaymentDetail>().FirstOrDefault(item => item.Id == hdDetailID.Value);
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
