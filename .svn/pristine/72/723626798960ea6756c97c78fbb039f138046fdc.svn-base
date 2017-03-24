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
using VirtualMachine.Patterns.BusinessEssence.Domain;
public partial class MoneyManage_FactoringDataMng_DetailInfo : System.Web.UI.Page
{
    IFinanceMultDataSrv service = GlobalClass.FinanceMultDataSrv;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PageInit();
        }
    }

    private FactoringDataMaster model;
    /// <summary>
    /// 主表
    /// </summary>
    public FactoringDataMaster Model
    {
        get
        {
            if (model == null)
            {
                model = (FactoringDataMaster)ViewState["MasterInfo"];
            }
            return model;
        }
        set
        {
            model = value;
            ViewState["MasterInfo"] = value;
        }
    }

    /// <summary>
    /// 页面初始化
    /// </summary>
    private void PageInit()
    {
        txtCreatePerson.ReadOnly = true;
        txtCreateDate.Attributes.Add("onfocus", "new WdatePicker(this,'%Y-%M-%D',true)");
        if (string.IsNullOrEmpty(Request["id"]) && Model == null)
        {
            Model = new FactoringDataMaster() { DocState = DocumentState.Edit };
            hidIsNew.Value = "True";
            hidIsEdit.Value = "True";
        }
        else
        {
            string masterId = string.IsNullOrEmpty(Request["id"]) ? "1thVqInvX9KR74bWUvsqP0" : Request["id"];
            string pageState = Request["pageState"];
            Model = service.GetFactoringDataById(masterId);
            if (Model == null)
            {
                // 没有获取到数据
                hidIsDeleted.Value = "True";
                Model = new FactoringDataMaster();
                return;
            }
            hidIsNew.Value = "False";
            InitModel();
        }
    }

    private void InitModel()
    {
        txtCode.ReadOnly = true;
        txtCode.Text = Model.Code;
        txtCreatePerson.Text = Model.CreatePersonName;
        txtHandlerPerson.Text = Model.HandlePersonName;
        //txtCreateDate.Text = Model.CreateDate.ToString("yyyy-MM-dd");
        txtCreateDate.Text = Model.RealOperationDate.ToString("yyyy-MM-dd");
        txtRemark.Text = Model.Descript;
        if (Model.DocState != DocumentState.Edit)
        {
            // 如果主表数据不是编辑状态
            txtCode.ReadOnly = true;
            txtHandlerPerson.ReadOnly = true;
            txtRemark.ReadOnly = true;
            txtCreateDate.ReadOnly = true;
            txtCreateDate.Attributes.Remove("onfocus");
        }
        DataBind(Model.Details);
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    /// <param name="source"></param>
    private void DataBind(object source)
    {
        gvDetail.DataSource = source;
        gvDetail.DataBind();
    }

    /// <summary>
    /// 添加或编辑
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var editId = hidEditId.Value;
        FactoringDataDetail detail;
        if (hidEditState.Value == "add")
        {
            detail = new FactoringDataDetail() { Id = Guid.NewGuid().ToString() };
            detail.Master = Model;
            detail.TempData = "add";
        }
        else
        {
            detail = (FactoringDataDetail)Model.Details.Where(a => a.Id == editId).FirstOrDefault();
        }
        detail.DepartmentName = txtDepartmentName.Text;
        detail.ProjectName = txtProjectName.Text;
        detail.BankName = txtBankName.Text;
        detail.Balance = string.IsNullOrEmpty(txtBalance.Text) ? 0 : Convert.ToDecimal(txtBalance.Text);
        detail.Rate = string.IsNullOrEmpty(txtRate.Text) ? 0 : Convert.ToDecimal(txtRate.Text);
        detail.StartDate = string.IsNullOrEmpty(txtStartDate.Text) ? DateTime.Now : Convert.ToDateTime(txtStartDate.Text);
        detail.EndDate = string.IsNullOrEmpty(txtEndDate.Text) ? DateTime.Now : Convert.ToDateTime(txtEndDate.Text);
        detail.PayType = ddlPayType.SelectedValue;
        detail.StartChargingDate = string.IsNullOrEmpty(txtStartChargingDate.Text) ? DateTime.Now : Convert.ToDateTime(txtStartChargingDate.Text);
        detail.EndChargingDate = string.IsNullOrEmpty(txtEndChargingDate.Text) ? DateTime.Now : Convert.ToDateTime(txtEndChargingDate.Text);
        detail.AmountPayable = string.IsNullOrEmpty(txtAmountPayable.Text) ? 0 : Convert.ToDecimal(txtAmountPayable.Text);
        detail.TotalDay = (detail.EndChargingDate - detail.StartChargingDate).Days;
        Model.AddDetail(detail);
        DataBind(Model.Details);
        ViewState["MasterInfo"] = Model;
        hidIsEdit.Value = "True";
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        foreach (var item in Model.Details)
        {
            if (item.TempData == "add")
            {
                // 保存子表数据时，将新增加的子表项id清空
                item.Id = null;
            }
            item.Money = ((FactoringDataDetail)item).AmountPayable;       // 设置Money属性，自动汇总主表金额
        }
        var realOperationDate = new DateTime();
        DateTime.TryParse(txtCreateDate.Text, out realOperationDate);
        Model.RealOperationDate = realOperationDate;
        Model.HandlePersonName = txtHandlerPerson.Text;
        Model.Descript = txtRemark.Text;
        if (string.IsNullOrEmpty(Model.Code))
        {
            var user = Session[PublicClass.sUserInfoID] as SessionInfo;
            Model.LastModifyDate = DateTime.Now;
            Model.Code = txtCode.Text;
            Model.CreateDate = DateTime.Now;
            Model.CreateYear = DateTime.Now.Year;
            Model.CreateMonth = DateTime.Now.Month;
            Model.CreatePerson = user.CurrentPersinInfo;
            Model.CreatePersonName = user.CurrentPerson.Name;
            Model.OperOrgInfo = user.CurrentOrgInfo;
            Model.OperOrgInfoName = user.CurrentOrgInfo.Name;
            service.Save(Model);
            InitModel();
        }
        else
        {
            service.Update(Model);
        }
        hidIsEdit.Value = "False";
        hidIsNew.Value = "False";
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        var id = hidEditId.Value;
        var detail = (FactoringDataDetail)Model.Details.Where(a => a.Id == id).FirstOrDefault();
        txtDepartmentName.Text = detail.DepartmentName;
        txtProjectName.Text = detail.ProjectName;
        txtBankName.Text = detail.BankName;
        txtBalance.Text = detail.Balance.ToString();
        txtRate.Text = detail.Rate.ToString();
        txtStartDate.Text = detail.StartDate.ToString("yyyy-MM-dd");
        txtEndDate.Text = detail.EndDate.ToString("yyyy-MM-dd");
        ddlPayType.Text = detail.PayType;
        txtStartChargingDate.Text = detail.StartChargingDate.ToString("yyyy-MM-dd");
        txtEndChargingDate.Text = detail.EndChargingDate.ToString("yyyy-MM-dd");
        txtAmountPayable.Text = detail.AmountPayable.ToString();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        var id = hidDeleteId.Value;
        var detail = Model.Details.Where(a => a.Id == id).FirstOrDefault();
        Model.Details.Remove(detail);
        DataBind(Model.Details);
        hidIsEdit.Value = "True";
    }

    /// <summary>
    /// 提交数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void submit_Click(object sender, EventArgs e)
    {
        Model.DocState = DocumentState.Completed;
        btnSave_Click(null, null);
        PageInit();
    }

    /// <summary>
    /// 删除主表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnMasterDelete_Click(object sender, EventArgs e)
    {
        service.Delete(Model);
        hidIsDeleted.Value = "True";
    }


}