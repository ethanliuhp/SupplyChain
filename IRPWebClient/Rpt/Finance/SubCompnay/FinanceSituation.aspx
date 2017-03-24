<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinanceSituation.aspx.cs" Inherits="Rpt_Finance_SubCompnay_FinanceSituation" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>分公司财务情况表</title>
    
    <link href="../../../css/cscec-reporter.css" rel="Stylesheet" />
    <script src="../../../JavaScript/My97DatePicker/WdatePicker.js"></script>
    <script src="../../../JavaScript/jquery-1.10.2.min.js"></script>
    <script src="<%=ResolveUrl("../../../JavaScript/progressLoader.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(window).ready(function(){loadProgress();});
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <div class="container">
        <div class="panel">
            <div class="panel-search" runat="server" id="panelSearch">
                <div class="row">
                    <label class="label">
                        起始日期查询：
                        <asp:TextBox ID="txtStartTime" class="text" runat="server" onfocus="new WdatePicker(this,'%Y-%M-%D',true)"></asp:TextBox>
                        至
                        <asp:TextBox ID="txtEndTime" class="text" runat="server" onfocus="new WdatePicker(this,'%Y-%M-%D',true)"></asp:TextBox>
                    </label>
                    <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" class="button button-default" />
                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="../../../images/tool/excel.png" CssClass="image" style="float:right;" title="导出excel" OnClick="btnExcel_Click" />
                    <asp:ImageButton ID="btnPrint" runat="server" ImageUrl="../../../images/tool/print.gif" CssClass="image" style="float:right;" title="打印" OnClick="btnPrint_Click" />
                </div>
            </div>
            <div class="panel-title">
                <h3>分公司财务情况表</h3>
            </div>
            <div class="panel-body">
                <asp:GridView ID="gvTable" runat="server" AutoGenerateColumns="false" class="table">
                    <Columns>
                        <asp:BoundField HeaderText="序号" DataField="RowNo"/>
                        <asp:BoundField HeaderText="考核指标" DataField="AssessmentIndex" />
                        <asp:BoundField HeaderText="目标" DataField="Target" />
                        <asp:BoundField HeaderText="实际" DataField="Actual" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="panel-pager" runat="server" id="panelPager">
                <webdiyer:AspNetPager ID="pager" runat="server" OnPageChanged="pager_PageChanged"
                      FirstPageText="首页" LastPageText="尾页" NextPageText="&raquo;" PrevPageText="&laquo;" PageSize="30" CssClass="pager">
                </webdiyer:AspNetPager>
            </div>
            <div class="panel-footer"></div>
        </div>
    </div>
    </form>
</body>
</html>
