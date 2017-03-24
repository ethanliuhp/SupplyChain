﻿using System;
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

public partial class MoneyManage_PaymentMng_master : System.Web.UI.Page
{

    string sMasterSQL;
    string sOrderBySQL = " ORDER BY RealOperationdate DESC";
    protected void Page_Load(object sender, EventArgs e)
    {

        sMasterSQL = string.Format("select  t.* from THD_PAYMENTMASTER t where CreatePerson='{0}' and IfProjectMoney={1} ", PublicClass.GetUseInfo(Session).CurrentPersinInfo.Id, PaymentType);

        if (!IsPostBack)
        {
            DataBind(sMasterSQL + sOrderBySQL);
            //this.gvMasterSource.PageSize = 10;

            this.txtStartDate.Text = (DateTime.Now.AddDays(-7)).ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

    }
    public void DataBind(string sSQL)
    {
        this.gvMasterSource.DataSQL = string.Format("select rownum num ,t.* from ({0}) t", sSQL);
        this.gvMasterSource.DestControl = this.gvMaster;
        this.gvMasterSource.IsSelect = true;
        //this.gvMasterSource.ClientSingleClick = "click1";
        this.gvMasterSource.ClientClick = "operateMaster";
        this.gvMasterSource.EnabledClientDeleteClick = false;
        //this.gvMasterSource.ClientDeleteClick = "operateMaster";
        this.gvMasterSource.EnabledClientNewClick = false;
        //this.gvMasterSource.ClientNewClick = "operateMaster";
        this.gvMasterSource.EnabledClientUpdateClick = false;
        //this.gvMasterSource.ClientUpdateClick = "operateMaster";
        this.gvMasterSource.DataBind();
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        try
        {
            string sSQL = sMasterSQL;
            DateTime dateStart = DateTime.Now;
            DateTime dateEnd = DateTime.Now.AddDays(-1);
            if (string.IsNullOrEmpty(this.txtStartDate.Text) && string.IsNullOrEmpty(this.txtEndDate.Text))
            {
                txtEndDate.Focus();
                throw new Exception("请输入查询条件");
            }
            else
            {

                if (!string.IsNullOrEmpty(this.txtStartDate.Text))
                {
                    if (DateTime.TryParse(this.txtStartDate.Text, out dateStart))
                    {
                        sSQL += string.Format(" and createdate>to_date('{0}','yyyymmdd')-1 ", dateStart.ToString("yyyyMMdd"));
                    }
                }
                if (!string.IsNullOrEmpty(this.txtEndDate.Text))
                {
                    if (DateTime.TryParse(this.txtEndDate.Text, out dateEnd))
                    {
                        sSQL += string.Format(" and createdate<to_date('{0}','yyyymmdd')+1 ", dateEnd.ToString("yyyyMMdd"));
                    }
                }
                if (!string.IsNullOrEmpty(this.txtStartDate.Text) && !string.IsNullOrEmpty(this.txtEndDate.Text))
                {
                    if (dateEnd < dateStart)
                    {
                        txtEndDate.Focus();
                        throw new Exception("请正确输入([起始日期]应该小于等于[结束日期])");

                    }
                }
                // this.gvMasterSource.DataSQL = sMasterSQL + sOrderBySQL;
                // this.gvMasterSource.GetData();
                DataBind(sSQL + sOrderBySQL);
            }
        }
        catch (Exception ex)
        {
            UtilClass.MessageBox(this, ex.Message);
            DataBind(sMasterSQL + sOrderBySQL);
        }
    }
    /// <summary>
    /// 收款类型,0:工程款,1:其他
    /// </summary>
    public int PaymentType
    {
        get { return int.Parse(this.hdPaymentType.Value); }
        set { hdPaymentType.Value = value.ToString(); }
    }
}