<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stockDetial.aspx.cs" Inherits="StockIn_stockDetial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        body,form{ margin:0px; padding:0px;  height:100%;  width:100%; }
        
    </style>
     <script type="text/javascript" src="../JavaScript/jquery/jquery-1.4.2.min.js"></script>
     <script type="text/javascript">
         $(document).ready(function(){
      var win=$(window);
     var diff=0;
      win.resize=function(){
       var w=win.width();
      var h=win.height();
       // $("#main").width(w);
        $("#Content").height(h);
         
      };
      win.resize();
   });
     </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="Content">
     <asp:GridView ID="gridDetial" runat="server"  CssClass="datable" AutoGenerateColumns="false"  EmptyDataText="没有查询到入库单明细" >
                        <Columns>
                       <%-- 物资编码 物资名称 规格型号 材质 计量单位 可用数量 剩余数量 数量 临时数量 
                        采购单价 认价单价 金额 认价金额 图号 计算式 外观质量 专业分类 备注--%>
                            <asp:BoundField  HeaderText="物资编码" Visible="true" DataField=""  ItemStyle-Width="50px"  />
                            <asp:BoundField  HeaderText="物资名称" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="规格型号" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="材质" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="计量单位" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="可用数量" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="剩余数量" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="数量" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="临时数量" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="采购单价" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="认价单价" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="金额" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="图号" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="计算式" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="外观质量" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="专业分类" Visible="true" DataField=""  ItemStyle-Width="50px" />
                            <asp:BoundField  HeaderText="备注" Visible="true" DataField=""  ItemStyle-Width="50px" />
                        </Columns>
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                    </asp:GridView>
    </div>
    </form>
</body>
</html>
