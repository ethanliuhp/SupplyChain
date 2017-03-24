<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Master.aspx.cs" Inherits="MoneyManage_FactoringDataMng_Master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>主表</title>
    <link href="../../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Util.js"></script>
    <script src="../../JavaScript/jquery-1.10.2.min.js"></script>
    <script src="../../JavaScript/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        var masterTable;
        $(function(){
            masterTable = $(".table");
            masterTable
            .on("dblclick","tr:not(:first)",function(){
                // 数据行双击事件
                var data = $(this).find(".dataStoragy").val().split("|");
                var id = data[0];
                var pageState = data[1];
                window.parent.reloadDetail(id,pageState);
                
            })
            .on("click","tr:not(:first)",function(){
                masterTable.find("tr:not(:first)").removeClass("_SelectRowStyle");
                $(this).addClass("_SelectRowStyle");
                // 控制删除按钮
                var data = $(this).find(".dataStoragy").val().split("|");
                var id = data[0];
                var pageState = data[1];
                window.parent.delVisible(pageState);
                // 给记录删除id的隐藏控件赋值
                $("#<%=hidDeleteId.ClientID %>").val(id);
            })
            .on("click",".delete",function(){
                // 删除
                if(!confirm("确定删除吗？"))return false;
                var data = $(this).data("id");
                $("#<%=hidDeleteId.ClientID %>").val(data);
                __doPostBack("btnDelete","");
            })
            // 新增
            $("#btnAdd").click(function(){
                window.parent.reloadDetail("","add");
            });
            $("#btnSubmit").click(function(){
                if(!confirm("此操作不可逆，确定提交吗？"))return false;
                var data = masterTable.find(".selector .dataStoragy").val().split("|");
                var id = data[0];
                var pageState = data[1];
                $("#<%=hidSubmitId.ClientID %>").val(id);
            });
            
        });
        
        // 删除主数据
        function deleteMaster(){
            if(!confirm("确定删除吗？"))return false;
            __doPostBack("btnDelete","");
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divMain" style=" padding:0px; margin:0px; text-align:center;  ">
        <div id="divQuery" style=" width:100%; height:30px">
            <table cellpadding="0" cellspacing="0" class="QueryTable">
                <tr class="QueryTableTr">
                    <td class="QueryTableTdLabel">截至日期:</td>
                    <td class=" QueryTableTdText"> 
                        <asp:TextBox runat="server" ID="txtStartDate" class="Wdate" onfocus="new WdatePicker(this,'%Y-%M-%D',true)"></asp:TextBox>
                        至
                        <asp:TextBox runat="server" ID="txtEndDate" class="Wdate" onfocus="new WdatePicker(this,'%Y-%M-%D',true)"></asp:TextBox>
                    </td>
                    <td class="QueryTableTdLabel" style=" text-align:left;">
                        <asp:Button runat="server" ID="btnQuery" Text="查询" OnClick="btnQuery_Click" CssClass="panel-button"></asp:Button>
                        <asp:LinkButton ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click" style="display:none;" />
                    </td>
                    <td class=" QueryTableTdText"></td>
                </tr>
            </table>
        </div>
        <div id="divLst">
            <asp:GridView ID="gvMaster" runat="server" AutoGenerateColumns="false" EmptyDataText="没有符合条件数据..." CssClass="table">
                <AlternatingRowStyle BackColor="#F7FAFF" />
                <Columns>
                    <asp:BoundField DataField="rn" HeaderText="序号" HeaderStyle-Width="40px"></asp:BoundField>
                    <asp:BoundField DataField="code" HeaderText="编号"></asp:BoundField>
                    <asp:BoundField DataField="createpersonname" HeaderText="制单人"></asp:BoundField>
                    <asp:BoundField DataField="handlepersonname" HeaderText="业务员"></asp:BoundField>
                    <asp:TemplateField HeaderText="创建日期" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <%#Convert.ToDateTime(Eval("createdate")).ToString("yyyy-MM-dd")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="summoney" HeaderText="总金额" DataFormatString="{0:N2}"></asp:BoundField>
                    <asp:BoundField DataField="descript" HeaderText="备注"></asp:BoundField>
                    <asp:TemplateField HeaderText="状态" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <%#CommonHelper.stateDescripe[Convert.ToInt32(Eval("state"))]%>
                            <input type="hidden" class="dataStoragy" value='<%#Eval("id") %>|<%#Eval("state") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <webdiyer:AspNetPager ID="pager" runat="server" OnPageChanged="pager_PageChanged"
                  FirstPageText="首页" LastPageText="尾页" NextPageText=">>" PrevPageText="<<" ShowPageIndexBox="Never"
                   AlwaysShow="true" ReverseUrlPageIndex="True" PageSize="15" CssClass="pager">
            </webdiyer:AspNetPager>
        </div>
    </div>
    <asp:HiddenField ID="hidDeleteId" runat="server" />
    <asp:HiddenField ID="hidSubmitId" runat="server" />
    </form>
</body>
</html>
