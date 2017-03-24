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
using IRPServiceModel.Services.PaymentOrder;
using VirtualMachine.Component.Util;
using IRPServiceModel.Domain.PaymentOrder;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using NHibernate.Criterion;
using System.Collections.Generic;
using  VirtualMachine.Core;

public partial class PaymentOrder_PaymentOrderMaster : System.Web.UI.Page
{
    IPaymentOrderSvr model = GlobalClass.PaymentOrderSvr;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public void bind()
    {
        if (!IsPostBack)
        {
            int iIndex = 0;
            dpLstPaymentType_Search.Items.Clear();
            dpLstPaymentType_Search.Items.Insert(iIndex,new ListItem("所有",""));
            foreach (string item in Enum.GetNames(typeof(EnumPaymentType)))
            {
                ddlPaymentTypeName.Items.Insert(iIndex,new ListItem(item, ((int)Enum.Parse(typeof(EnumPaymentType), item)).ToString()));
                iIndex++;
                dpLstPaymentType_Search.Items.Insert(iIndex,new ListItem(item, ((int)Enum.Parse(typeof(EnumPaymentType), item)).ToString()));
            }
            ddlPaymentTypeName.SelectedIndex = 0;
        }
        StringBuilder condition = new StringBuilder();
        if (dpLstPaymentType_Search.SelectedIndex > -1 && !string.IsNullOrEmpty(dpLstPaymentType_Search.SelectedValue))
        {
            condition.AppendFormat(" AND T.PAYMENTTYPE={0} ", dpLstPaymentType_Search.SelectedValue);
        }
        if (!string.IsNullOrEmpty(txtBeginDate_Search.Text))
        {
            DateTime beginDate;
            if (DateTime.TryParse(txtBeginDate_Search.Text, out beginDate) && beginDate > DateTime.Parse("1900-1-1"))
            {
                condition.AppendFormat("AND T.CREATEDATE>='{0}'", beginDate.ToString());
            }
        }
        if (!string.IsNullOrEmpty(txtEndDate_Search.Text))
        {
            DateTime endDate;
            if (DateTime.TryParse(txtEndDate_Search.Text, out endDate) && endDate > DateTime.Parse("1900-1-1"))
            {
                condition.AppendFormat("AND T.CREATEDATE<='{0}'", endDate.ToString());
            }
        }
        if (!string.IsNullOrEmpty(txtPayee_Search.Text))
        {
            condition.AppendFormat(" T.PAYEENAME LIKE '%{0}%'", txtPayee_Search.Text);
        }
        string sql = "select    rownum num , t.* from IRP_PaymentOrderMaster t where 1 = 1" + condition;
        this.GridViewSource1.DestControl = this.GridView1;
        this.GridViewSource1.DataSQL = sql;
        this.GridViewSource1.PageSize = 15;
        this.GridViewSource1.GetData();
    }
  
   
    protected void btnAddNew_Click(object sender, EventArgs e)
    {

        checkBox.Checked = true;
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        PaymentOrderMaster master = model.GetPaymentOrderById(txtIdHidden.Value);
        if (master == null)
        {
            UtilClass.MessageBox(this, "该单据已经删除，不能修改！");
            return;
        }
        if (master.DocState != DocumentState.Edit)
        {
            UtilClass.MessageBox(this, "该单据已提交，不能修改！");
            return;
        }
        ddlPaymentTypeName.SelectedValue =((int) master.PaymentType).ToString();
        txtPayeeName.Text = master.Payee == null ? "" : master.Payee.Name;
        txtPayeeId.Value = master.Payee == null ? "" : master.Payee.Id;
        txtTheBankName.Text = master.TheBankName;
        txtTheBankCode.Text = master.TheBankCode;
        this.txtDescript.Text = master.Describe;
        checkBox.Checked = true;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string[] idlst = txtIdListHidden.Value.Split(new char[] { '|' });
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (string id in idlst)
            {
                dis.Add( Expression.Eq("Id", id));
            }
            oq.AddCriterion(dis);
            IList<PaymentOrderMaster> list = model.Query( oq);
            foreach (PaymentOrderMaster master in list)
            {
                if (master.DocState != DocumentState.Edit)
                {
                    UtilClass.MessageBox(this,"该单据已提交，不能删除！");
                    return;
                }
            }
            if (model.Delete(list))
            {
                UtilClass.MessageBox(this, "删除成功！");
                GridViewSource1.GetData();
            }
            else
            {
                UtilClass.MessageBox(this, "删除失败！");
            }
        }
        catch (Exception)
        {
            UtilClass.MessageBox(this, "操作失误，请重试！");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        PaymentOrderMaster master = new PaymentOrderMaster();
        if (txtIdHidden.Value != "")
        {
            master = model.GetPaymentOrderById(txtIdHidden.Value);
        }
        master.PaymentType= (EnumPaymentType) Int32.Parse( ddlPaymentTypeName.SelectedValue);
        PersonInfo person = GlobalClass.CommonMethodSrv.QueryById(typeof(PersonInfo), txtPayeeId.Value)as PersonInfo  ;
        master.PayeeName = person == null ? "" : person.Name;
        master.TheBankCode = txtTheBankCode.Text;
        master.TheBankName = txtTheBankName.Text;
        master.Payee = person;
        master.Describe = txtDescript.Text;
        model.SaveOrUpdate(master);
       UtilClass.MessageBox(this,"保存成功！");
        GridViewSource1.GetData();
        checkBox.Checked = false;
    }
    protected void btnCanel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    public void Clear()
    {
        txtPayeeName.Text = "";
        txtPayeeId.Value = "";
        txtTheBankName.Text = "";
        txtTheBankCode.Text = "";
        txtDescript.Text = "";
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bind();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string[] idlst = txtIdListHidden.Value.Split(new char[] { '|' });
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (string id in idlst)
            {
                dis.Add(Expression.Eq("Id", id));
            }
            oq.AddCriterion(dis);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList<PaymentOrderMaster> list = model.Query(oq);
            foreach (PaymentOrderMaster master in list)
            {
                if (master.DocState != DocumentState.Edit)
                {
                   UtilClass. MessageBox(this,"该单据已提交，不能重复提交！");
                    return;
                }
                if (master.Details.Count == 0)
                {
                    UtilClass.MessageBox(this,"该单据没有明细，不能提交！");
                    return;
                }
                master.DocState = DocumentState.InExecute;
            }
            if (model.SaveOrUpdate(list))
            {
                UtilClass.MessageBox(this,"提交成功");
                GridViewSource1.GetData();
            }
        }
        catch (Exception ex)
        {
            UtilClass.MessageBox(this,"操作失败！" + ex.Message);
        }
    }
}