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
using System.Text;


public partial class Rpt_MoneyManage_FactoringDataManage_FactoringDataRpt : System.Web.UI.Page
{
    IFinanceMultDataSrv service = GlobalClass.FinanceMultDataSrv;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 默认查询当前月份
            this.txtStartTime.Text = DateTime.Now.Year + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-01";
            this.txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            DataBind();
        }
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void DataBind()
    {
        DataBind(pager.CurrentPageIndex, pager.PageSize);
    }

    private void DataBind(int pageIndex,int pageSize)
    {
        // 查询开始与结束记录序号
        int startPageIndex = (pageIndex - 1) * pageSize + 1;
        int endPageIndex = pageIndex * pageSize;

        // 查询开始日期与结束日期
        string startDate = string.IsNullOrEmpty(txtStartTime.Text) ? "1900-01-01" : txtStartTime.Text;          // 开始日期
        string endDate = string.IsNullOrEmpty(txtEndTime.Text) ? "2044-01-01" : txtEndTime.Text;                // 结束日期

        // 时间格式化
        StringBuilder sb = new StringBuilder();
        string dateFormat = "{0} between to_date('{1} 00:00:00','yyyy-mm-dd hh24:mi:ss') AND to_date('{2} 23:59:59','yyyy-mm-dd hh24:mi:ss')";
        sb.Append(string.Format(dateFormat, "startdate", startDate, endDate));

        // 查询记录总数
        string sqlCount = string.Format("select count(*) as recordcount from thd_factoringdatadetail where {0}", sb.ToString());
        var dtRecord = GlobalClass.CommonMethodSrv.GetData(sqlCount).Tables[0];
        pager.RecordCount = dtRecord.Rows.Count > 0 ? Convert.ToInt32(dtRecord.Rows[0]["recordcount"]) : 0;

        // 查询数据
        string sqlText = string.Format("select * from (select rownum as rn,t.* from (select projectname, bankname, balance, rate, startdate, enddate, paytype, startchargingdate, endchargingdate, totalday, amountpayable from thd_factoringdatadetail where {0} order by startdate desc)t) where rn between {1} and {2}", sb.ToString(), startPageIndex, endPageIndex);
        var table = GlobalClass.CommonMethodSrv.GetData(sqlText).Tables[0];
        gvTable.DataSource = table;
        gvTable.DataBind();
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        pager.CurrentPageIndex = 1;
        DataBind();
    }

    protected void pager_PageChanged(object sender, EventArgs e)
    {
        DataBind();
    }


    /// <summary>
    /// 导出excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        // 查询出所有数据
        DataBind(1, pager.RecordCount);

        // 设置搜索栏与分页不可见
        panelSearch.Visible = false;
        panelPager.Visible = false;

        // 输出页面
        string fileName = "保理业务台帐" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).ToString());
        Response.ContentEncoding = System.Text.Encoding.UTF7;  //必须写，否则会有乱码
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "gb2312";

        Response.AppendHeader("Content-Disposition ", "attachment;filename=" + fileName);
    }

    /// <summary>
    /// 打印
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        // 查询出所有数据
        DataBind(1, pager.RecordCount);

        // 设置搜索栏与分页不可见
        panelSearch.Visible = false;
        panelPager.Visible = false;

        Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "print", "<script>window.print()</script>");
    }
    


}
