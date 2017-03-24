<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Message.aspx.cs" Inherits="Main_Message" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="<%=ResolveUrl("../JavaScript/jquery-1.10.2-vsdoc.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jquery-1.10.2.js")%>" type="text/javascript"></script>

    <%--弹出层js css --%>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.core.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.widget.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.button.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.mouse.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.draggable.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.resizable.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.position.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.dialog.js")%>" type="text/javascript"></script>

    <link href="<%=ResolveUrl("../css/jquery-ui[1].css")%>" rel="stylesheet" type="text/css" />

    <script src="<%=ResolveUrl("../JavaScript/My97DatePicker/WdatePicker.js")%>" type="text/javascript"></script>

    <link href="../css/MainWeb.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript">
        window.onload = function() {
            $(function() {
                $("#<%=frameMessage.ClientID %>").dialog({
                    open: function() {
                        $("#<%=frameMessage.ClientID %>").addClass("dialog");
                    },
                    autoOpen: document.getElementById("<%=checkBox.ClientID %>").checked,
                    width: 550,
                    height: 200,
                    modal: true,
                    bgiframe: true,
                    zIndex: 990,
                    close: function() {
                        $("#<%=frameMessage.ClientID %>").removeClass("dialog");
                        document.getElementById("<%=checkBox.ClientID %>").checked = false;
                        $("#<%=btnChangeState.ClientID %>").click();
                    }
                });
            });
        }

        function OpenLink(sUrl) {
            var str = sUrl.toLowerCase().replace('http://', '');
            str = "http://" + str;
            window.open(str);
            
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="width: 400px; margin-top: 10px">
            <fieldset>
                <legend>消息记录</legend>
                <asp:GridView ID="gvMessages" runat="server" AutoGenerateColumns="False" DataKeyNames="SendPerson"
                    EmptyDataText="当前没有相关数据显示" Width="100%" OnRowCommand="gvMessages_RowCommand">
                    <RowStyle BackColor="#FFFFFF" ForeColor="Black" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B4D8E2" ForeColor="Black" />
                    <HeaderStyle BackColor="#3778A9" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#B4D8E2" />
                    <Columns>
                        <asp:BoundField HeaderText="发送人" DataField="SendPersonName" />
                        <asp:BoundField HeaderText="条数" DataField="num" />
                        <asp:TemplateField HeaderText="查看">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkShow" runat="server" CommandName="message" CommandArgument='<%#Eval("SendPerson") %>'>查看</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
        </div>
         <div style="width: 400px; margin-top: 10px">
  
           <fieldset>
                <legend>友情链接</legend>
                <asp:GridView ID="gvFriendLink" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="当前没有相关数据显示" Width="100%" OnRowCommand="gvMessages_RowCommand">
                    <RowStyle BackColor="#FFFFFF" ForeColor="Black" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B4D8E2" ForeColor="Black" />
                    <HeaderStyle BackColor="#3778A9" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#B4D8E2" />
                    <Columns>
                       <asp:TemplateField HeaderText="友情链接">
                            <ItemTemplate>
                                <a  style="  font-size:13px" onclick="OpenLink('<%#Eval("LinkAddress") %>')" href=""><%#Eval("LinkName")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="描述" DataField="Descript" />
                        
                    </Columns>
                </asp:GridView>
            
                
            </fieldset>
        </div>
        <div id="frameMessage" title="消息" style="width: 100%; display: none;" runat="server">
            <iframe id="frameMessage1" style="display: block" width="100%" height="100%" frameborder="0"
                runat="server"></iframe>
        </div>
       
        <div style="display: none">
            <asp:CheckBox ID="checkBox" runat="server" />
            <asp:Button ID="btnChangeState" runat="server" Text="查看状态" OnClick="btnChangeState_Click" />
            <asp:HiddenField ID="SendPersonId" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
