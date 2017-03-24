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
using IRPServiceModel.Domain.PaymentOrder;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections.Generic;

public partial class PaymentOrder_PaymentOrderDetail : System.Web.UI.Page
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
        string id = Request.QueryString["Id"].ToString();
        string sql = "select   rownum num, t.* from IRP_PaymentOrderDetial t where 1 = 1 and t.ParentId = '" + id + "'";
        this.GridViewSource1.DataSQL = sql;
        this.GridViewSource1.DestControl = this.GridView1;
        this.GridViewSource1.PageSize = 10;
        this.GridViewSource1.GetData();
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string id = Request.QueryString["Id"].ToString();
        PaymentOrderMaster master = model.GetPaymentOrderById(id);
        PaymentOrderDetial dtl = new PaymentOrderDetial();
        if (txtIdHidden.Value != "")
        {
            dtl = model.GetPaymentOrderDetailById(txtIdHidden.Value);
        }
        dtl.Money = Convert.ToDecimal(txtMoney.Text);
        dtl.PaymentItemName = txtPaymentItemName.Text;
        dtl.Describe = this.txtDescribe.Text;
        dtl.Master = master;
        master.Money = dtl.Money;
        foreach (PaymentOrderDetial oDetail in master.Details)
        {
            master.Money += oDetail.Money;
        }
        model.SaveOrUpdate(dtl);
       // master.Money += dtl.PaymentMoney;
        
       // model.SaveOrUpdate(master);
        SumMoney();
       UtilClass. MessageBox(this,"保存成功！");
        this.GridViewSource1.GetData();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        PaymentOrderDetial dtl = model.GetPaymentOrderDetailById(txtIdHidden.Value);
        this.txtMoney.Text = dtl.Money.ToString();
        this.txtPaymentItemName.Text = dtl.PaymentItemName;
        this.txtDescribe.Text = dtl.Describe;
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
                dis.Add(Expression.Eq("Id", id));
            }
            oq.AddCriterion(dis);
            IList<PaymentOrderDetial> list = model.QueryDetial(  oq);
            if (model.Delete(list as IList))
            {
                UtilClass.MessageBox(this, "删除成功！");
                GridViewSource1.GetData();
                SumMoney();
                Clear();
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
    public void SumMoney(   )
    {
        string id = Request.QueryString["Id"].ToString();
        PaymentOrderMaster master = model.GetPaymentOrderById(id);
        master.Money = 0;
        if (master!=null)
        {
            foreach (PaymentOrderDetial oDetail in master.Details)
            {
                master.Money += oDetail.Money;
            }
            model.SaveOrUpdate(master);
        }
      
    }
    public void Clear()
    {
        txtMoney.Text = "";
        this.txtDescribe.Text = "";
        this.txtPaymentItemName.Text = "";
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Clear();
    }

}
