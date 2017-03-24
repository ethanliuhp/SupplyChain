<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户登录</title>
    <link href="CSS/MainWeb.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="CSS/jwindow.css" media="all" />
    <style type="text/css">
        body
        {
            font-size: 9pt;
        }
        .divMain
        {
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='Images/login/login1.jpg',sizingMethod='scale');
            background-repeat: no-repeat;
            background-positon: 100%, 100%;
            margin: 0;
            padding: 0;
            border: 0 none;
            overflow: hidden;
            height: 100%;
        }
        .inputLogin
        {
            width: 193px;
            height: 18px;
            background: #ffffff url(Images/login/loginInputBg.jpg) repeat-x;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 13px;
            border: 1px solid #b2bec5;
            position: relative;
        }
    </style>

    <script type="text/javascript" src="JavaScript/jquery/jquery.js"></script>

    <script type="text/javascript" src="JavaScript/jquery/jquery.jwindow.js"></script>

    <script type="text/javascript" src="JavaScript/jquery/jquery.interface.js"></script>

    <script type="text/javascript">
        window.onload = function() {
            //浏览器兼容
            //            var height = document.documentElement.clientHeight;
            //            var loginHeight = document.getElementById('tbLogin').clientHeight;
            //            if (height > loginHeight) {
            //                document.getElementById('tbLogin').style.marginTop = (height - loginHeight) / 2 + 'px';
            //            }
            //            else {
            //                document.getElementById('tbLogin').style.marginTop = loginHeight / 2 + 'px';
            //            }

            //            document.getElementById("myLogin_UserName").focus();

            focusUserName();
        }


        function ResetLoginInfo() {
            document.getElementById("myLogin_UserName").value = "";
            document.getElementById("myLogin_Password").value = "";
            document.getElementById("tdMessage").innerText = "";
            document.getElementById("myLogin_UserName").focus();
            return false;
        }

        //切换验证码
        function refCode() {
            var rand = Math.random();
            document.getElementById("imgCode").src = "Code.aspx?rand=" + rand;
            return false;
        }

        //选择岗位弹窗
        function popForm() {
            $("#panelWindow").jWindowOpen({
                modal: true,
                center: true,
                drag: ".title",
                close: "#panelWindow #popupContactClose",
                closeHoverClass: "hover",
                transfererFrom: "#form1",
                transfererClass: "transferer"
            });
        }

        function ImgClose() {
            document.getElementById("popupContactClose").click();
        }

        function popupContactCloseClick() {

        }

        function getUserLocation() {
            var curUname = document.getElementById('txtUserName').value;
            if (curUname == "") {
                return false;
            }

            //            var lastUname = document.getElementById('LastUserName').value;
            //            if (curUname != lastUname) //当用户名文本框失去焦点的时候如果值没有改变不触发重新加载岗位信息
            //            {
            //                document.getElementById('LastUserName').value = curUname;
            //            }
        }

        function focusUserName() {

            if (document.getElementById('txtUserName').value == "") {
                document.getElementById('txtUserName').focus();
            }
            else {
                document.getElementById('txtPassword').focus();
            }
        }

        function ChangeGroupRoleJS() {
            document.getElementById('btnChangeGroupRole').click();
        }

    </script>

</head>
<body class="login">
    <form id="form1" runat="server">
    <asp:TextBox ID="LastUserName" runat="server" Style="display: none"></asp:TextBox>
    <table id="tbMain" runat="server" class="login_content" >
        <tr>
            <td colspan="3" style="padding: 0px 20px 0px 20px; text-align: center">
                <span id="LocalizedLabel6" title="欢迎使用IRP。" class="login_welcom">欢迎使用IRP</span>
            </td>
        </tr>
        <tr>
            <td class="login_td">
                <span id="LocalizedLabel2" title="输入用户名。 " class="login_label">用户名: </span>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtUserName" runat="server" onblur="getUserLocation();ChangeGroupRoleJS();"
                    TabIndex="1" CssClass="login_input"></asp:TextBox>
            </td>
            <td style="width: 200px;">
                <%--<asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" Display="Dynamic"
                    ErrorMessage="请输入用户名." ControlToValidate="txtUserName"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr>
            <td class="login_td">
                <span id="LocalizedLabel3" title="输入密码。" class="login_label">密码: </span>
            </td>
            <td style="text-align: left">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="login_input"
                    TabIndex="2"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="login_td">
                <span id="LocalizedLabel1" title="岗位" class="login_label">岗位:</span>
            </td>
            <td style="text-align: left">
                <asp:DropDownList ID="cbUserJobs" runat="server" AutoPostBack="false" TabIndex="3"
                    CssClass="login_droplist" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="text-align: left; padding-left: 20px; padding-top: 5px;">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="left" style="text-align: center;">
                <asp:Button ID="btnSubmit" runat="server" Text="登录" ToolTip="登录" OnClick="cmdSubmitButton_click"
                    TabIndex="6" CssClass="login_button" Width="80" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnReset" runat="server" Text="重置" ToolTip="重置" TabIndex="6" CssClass="login_button"
                    Width="80" OnClick="btnReset_Click" />
                <div style="display: none;">
                    <asp:Button ID="btnChangeGroupRole" runat="server" Text="" Width="0" OnClick="btnChangeGroupRole_Click" />
                </div>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <div class="overlayer">
    </div>
    <div class="window" id="panelWindow">
        <table style="width: 100%;" cellpadding="0" cellspacing="0">
            <tr>
                <td id="divTitle" class="title" style="text-align: center; text-indent: 20px;">
                    选择岗位
                </td>
                <td style="width: 23px; background: #E2EFFC;">
                    <img id="imgClose" title="关闭" alt='' src="images/login/close1.bmp" style="cursor: pointer;"
                        onclick="return ImgClose();" />
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
                    <td>
                        <asp:ListBox ID="listGroupJob" runat="server">
                            <asp:ListItem Text="系统管理员" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="测试员" Value=""></asp:ListItem>
                            <asp:ListItem Text="测试员" Value=""></asp:ListItem>
                            <asp:ListItem Text="测试员" Value=""></asp:ListItem>
                        </asp:ListBox>
                        <input id="txtSessionKeyHidden" runat="server" type="hidden" />
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
