using System;
using System.Collections;
using System.Collections.Generic;
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
using NHibernate.Criterion;
using VirtualMachine.Core;
using System.Text;

public partial class MoneyManage_FactoringDataManage : System.Web.UI.Page
{
    IFinanceMultDataSrv service = GlobalClass.FinanceMultDataSrv;
    public string Condition { get; set; }

    private DataTable _listData;
    public DataTable ListData
    {
        get
        {
            if (_listData == null)
            {
                _listData = GetData(Condition);
            }
            return _listData;
        }
        set
        {
            _listData = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


    }

    private DataTable GetData(string condition)
    {
        DataSet ds = GlobalClass.CommonMethodSrv.GetData("select id, version, code, createperson, createpersonname, handleperson, handlepersonname, createyear, createmonth, createdate, businessdate, state, operationorg, operorgname, opgsyscode, summoney, descript, realoperationdate from thd_factoringdatamaster where 1=1 " + condition);

        return ds.Tables[0];
    }

    protected void btn_Add_Master_Click(object sender, EventArgs args)
    {
        if (hid_operateState.Value == "Edit")
        {
            var model = service.GetFactoringDataById(hid_edit_id.Value);
            model.Code = txtCode.Text;
            model.Descript = txtDescrip.Text;
            model.LastModifyDate = DateTime.Now;
            model.HandlePersonName = txtHandler.Text;
            service.Update(model);
        }
        else
        {
            var model = new FactoringDataMaster();
            var user = Session[PublicClass.sUserInfoID] as SessionInfo;
            model.Descript = txtDescrip.Text;
            model.LastModifyDate = DateTime.Now;
            model.HandlePersonName = txtHandler.Text;
            model.Code = txtCode.Text;
            model.CreateDate = DateTime.Now;
            model.CreateYear = DateTime.Now.Year;
            model.CreateMonth = DateTime.Now.Month;
            model.CreatePerson = user.CurrentPersinInfo;
            model.OperOrgInfo = user.CurrentOrgInfo;

            service.Save(model);
        }
    }

    protected void btn_Delete_Click(object sender, EventArgs args)
    {
        //FactoringDataMaster model = new FactoringDataMaster() { Id = hid_delete_id.Value };
        var model = service.GetFactoringDataById(hid_delete_id.Value);
        service.Delete(model);
    }

    protected void btn_Search_Click(object sender, EventArgs args)
    {
        var startDate = string.IsNullOrEmpty(txtStartDate.Text) ? "1900-01-01" : txtStartDate.Text;
        var endDate = string.IsNullOrEmpty(txtEndDate.Text) ? "2060-01-01" : txtEndDate.Text;
        var orderNumber = txtNumber.Text;
        StringBuilder sb = new StringBuilder();
        string dateFormat = "{0} BETWEEN to_date('{1} 00:00:00','yyyy-mm-dd hh24:mi:ss') AND to_date('{2} 23:59:59','yyyy-mm-dd hh24:mi:ss')";
        sb.Append("and ");
        sb.Append(string.Format(dateFormat, "CREATEDATE", startDate, endDate));
        if (!string.IsNullOrEmpty(orderNumber))
        {
            sb.Append("and ");
            sb.Append("CODE='" + orderNumber + "'");
        }
        Condition = sb.ToString();
    }


}
