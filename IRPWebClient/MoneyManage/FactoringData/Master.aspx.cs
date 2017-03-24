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
using System.Text;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;

public partial class MoneyManage_FactoringData_Master : System.Web.UI.Page
{
    IFinanceMultDataSrv service = GlobalClass.FinanceMultDataSrv;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtStartDate.Text = (DateTime.Now.AddDays(-30)).ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            BindData();
        }
    }

    private void BindData()
    {
        string startDate = string.IsNullOrEmpty(txtStartDate.Text) ? "1900-01-01" : txtStartDate.Text;          // 开始日期
        string endDate = string.IsNullOrEmpty(txtEndDate.Text) ? "2044-01-01" : txtEndDate.Text;                // 结束日期
        int startPageIndex = (pager.CurrentPageIndex - 1) * pager.PageSize + 1;                                 // 序号开始
        int endPageIndex = pager.CurrentPageIndex * pager.PageSize;                                             // 序号结束

        // 时间
        StringBuilder sb = new StringBuilder();
        string dateFormat = "{0} between to_date('{1} 00:00:00','yyyy-mm-dd hh24:mi:ss') AND to_date('{2} 23:59:59','yyyy-mm-dd hh24:mi:ss')";
        sb.Append(string.Format(dateFormat, "createdate", startDate, endDate));
        // 格式化sql查询
        string sqlText = string.Format("select * from (select rownum as rn, id, code, createpersonname, handlepersonname, createdate, summoney, state, descript from (select id, code, createpersonname, handlepersonname, createdate, summoney, state, descript from thd_factoringdatamaster where {0} order by createdate desc)) where rn between {1} and {2}", sb.ToString(), startPageIndex, endPageIndex);
        var table = GlobalClass.CommonMethodSrv.GetData(sqlText).Tables[0];
        gvMaster.DataSource = table;
        gvMaster.DataBind();
        // 查询总记录数
        string sqlRecord = string.Format("select count(*) as recordcount from thd_factoringdatamaster where {0}", sb.ToString());
        DataSet dsRecord = GlobalClass.CommonMethodSrv.GetData(sqlRecord);
        pager.RecordCount = dsRecord.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsRecord.Tables[0].Rows[0]["recordcount"]) : 0;
    }

    protected void pager_PageChanged(object sender, EventArgs e)
    {
        BindData();
    }
        
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BindData();
    }

    /// <summary>
    /// 删除主表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        var model = service.GetFactoringDataById(hidDeleteId.Value);
        service.Delete(model);
        BindData();
    }

    /// <summary>
    /// 提交主表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var id = hidSubmitId.Value;
        var master = service.GetFactoringDataById(id);
        master.DocState = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Completed;
        foreach (var item in master.Details)
        {
            item.Money = ((FactoringDataDetail)item).AmountPayable;
        }
        service.Update(master);
        hidSubmitId.Value = "";
        BindData();
    }

}
