﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefaultHeaderBak.aspx.cs" Inherits="DefaultHeader" %>

<%@ Register Src="~/UserControls/ubsWebHeader.ascx" TagName="ubsWebHeader" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/WebMenuControl.ascx" TagName="webmenucontrol" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>IRP</title>
    <link href="CSS/MainWeb.CSS" rel="Stylesheet" type="text/css" />
    <link rel="stylesheet" href="CSS/jwindow.css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 100%;" id="headerTable" runat="server" >
        <tr id="trWebHeader" runat="server">
            <td valign="top" style="height: 71px">
                <uc1:ubsWebHeader ID="ubsHeader" runat="server"></uc1:ubsWebHeader>
            </td>
        </tr>
        <tr id="trLoginLogInfo">
            <td align="right" class="Header_Job">
                当前岗位：<asp:Label ID="lblCurrJobName" runat="server"></asp:Label>&nbsp;&nbsp;
                <button id="btnChangeJob" class="LocalizedButton" onclick="popForm();">
                    更换岗位</button>&nbsp;&nbsp;
            </td>
        </tr>
         <tr id="trWebMenu" runat="server">
            <td valign="top" style="width: 100%; height: 29px">
                <uc3:webmenucontrol ID="WebMenuControl1" runat="server" />
            </td>
        </tr>
    </table>
    <%--选择岗位--%>

    <script type="text/javascript" language="javascript" src='<%=ResolveClientUrl("JavaScript/jquery/jquery.js") %>'></script>

    <script type="text/javascript" language="javascript" src='<%=ResolveClientUrl("JavaScript/jquery/jquery.jwindow.js") %>'></script>

    <script type="text/javascript" language="javascript" src='<%=ResolveClientUrl("JavaScript/jquery/jquery.interface.js") %>'></script>

    <script type="text/javascript" language="javascript">
        //弹窗
        function popForm() {
            $("#panelWindow").jWindowOpen({
                modal: true,
                center: true,
                drag: ".title",
                close: "#panelWindow #popupContactClose",
                closeHoverClass: "hover",
                transfererFrom: "#btnChangeJob",
                transfererClass: "transferer"
            });
        }

        function ImgClose() {
            document.getElementById("popupContactClose").click();
        }
        function popupContactCloseClick() {

        }
        function setLocation(obj) {

            var tempObj = obj;

            var txtCurrLocation;
            while (!txtCurrLocation || txtCurrLocation.type != "hidden") {

                txtCurrLocation = tempObj.nextSibling;

                tempObj = tempObj.nextSibling;
            }

            txtCurrLocation.value = window.location.href;
        }
    </script>

    <div class="overlayer">
    </div>
    <div class="window" id="panelWindow">
        <table style="width: 100%;" cellpadding="0" cellspacing="0">
            <tr>
                <td id="divTitle" class="title" style="text-align: center; text-indent: 20px;">
                    选择岗位
                </td>
                <td style="width: 23px; background: #E2EFFC;">
                    <img id="imgClose" title="关闭" alt='' src='<%=ResolveClientUrl("images/login/close1.bmp") %>'
                        style="cursor: pointer;" onclick="return ImgClose();" />
                </td>
            </tr>
        </table>
        <div class="content">
            <div style="display: none;">
                <input id="targetDefineTypeEdit" value="弹出窗口" onclick="return popForm();" />
                <a id="popupContactClose" onclick="return popupContactCloseClick();">【关闭】</a>
            </div>
            <table style="width: 100%; margin: 0px atuo;" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="text-align: center;">
                        <asp:ListBox ID="listGroupJob" runat="server">
                            <asp:ListItem Text="测试员" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="测试员" Value=""></asp:ListItem>
                            <asp:ListItem Text="测试员" Value=""></asp:ListItem>
                            <asp:ListItem Text="测试员" Value=""></asp:ListItem>
                        </asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 20px;">
                        <asp:Button ID="btnEnter" runat="server" Text="确定" Width="100px" CssClass="LocalizedButton"
                            OnClick="btnEnter_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="status">
            <span class="resize"></span>
        </div>
    </div>
    </form>
</body>
</html>