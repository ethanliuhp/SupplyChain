<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockInList.aspx.cs" Inherits="StockIn_StockInList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
   <style type="text/css">
    body,form{ margin:0px; padding:0px;  height:100%;  width:100%; }
   .mainDiv{ margin:0px; padding:0px;  width:100%; height:100%; text-align:center;}
   #master{ margin:0px; padding:0px; width:100%; height:30%; border:solid 0px blue;}
   #detial{ margin:0px; padding:0px; width:100%; height:70%; border:solid 0px black;}
   fieldset{width:100%;margin:10px; height:100%; font-size: 12px;border:1px dotted #f0f;  padding:0px;  }
   legend{color: #f0f;font-size: 14px;}
   
 .datable {background-color: #9FD6FF; color:#333333; font-size:12px; width:100%; height:100%;}
.datable tr {height:20px;}
.datable .lup {background-color: #C8E1FB;font-size: 12px;color: #014F8A;}
.datable .lup th {border-top: 1px solid #FFFFFF; border-left: 1px solid #FFFFFF;font-weight: normal;}
.datable .lupbai {background-color: #FFFFFF;}
.datable .trnei {background-color: #F2F9FF;}
.datable td {border-top: 1px solid #FFFFFF; border-left: 1px solid #FFFFFF;}
iframe{ width:100%; height:100%; border:solid 0px black; margin:0px; padding:0px;}
   </style>
     <script type="text/javascript" src="../JavaScript/jquery/jquery-1.4.2.min.js"></script>
   <script type="text/javascript">
   $(document).ready(function(){
      var win=$(window);
     var wDiff=40;
     var hDiff=50;
      win.resize=function(){
       var w=win.width();
      var h=win.height();
        $("#main").width(w-wDiff);
        $("#main").height(h-hDiff);
//         $("#master").height(h*0.3-diff);
//         $("#detial").height(h-h*0.3-diff);
//         $("#detial iframe").height(h-h*0.3-diff);
      };
      win.resize();
   });
   </script>
</head>

<body>
    <form id="form1" runat="server">
    <div id="main" class="mainDiv">
        <div id="search">
         <fieldset>
               <legend>查询条件</legend>
               <div style=" width:100%; height:100%">
                  <table style=" width:100% ; height:100%;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>23432</td>
                    </tr>
                  </table>
                  
               </div>
         </fieldset>
        </div>
        <div id="master">
            <fieldset>
                <legend>入库主表信息</legend>
                <div style=" width:100%; height:100% ">
                    <asp:GridView ID="gridMaster" runat="server"  CssClass="datable" AutoGenerateColumns="false" EmptyDataText="没有查询到入库单明细">
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
            </fieldset>
        </div>
        <div id="detial">
            <iframe  frameborder="0" style=" width:100%; height:100%;" src="stockDetial.aspx" ></iframe>
        </div>
    </div>
    </form>
</body>
</html>
