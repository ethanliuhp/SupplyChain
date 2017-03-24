<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SSOLogin.aspx.cs" Inherits="Login_SSOLogin"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>单点登录</title>
    <style type="text/css" >
        #divMain 
        {
        	width:100%; 
        	height:100%; 
        	margin:0px 0px;
        	padding:0px 0px;
        	overflow: hidden;
        	background-image:url(../images/login/login.png);
            text-align:center;
            vertical-align:middle;
            min-width:627px;
            min-height:348px;
             border:solid 1px black;
        }
        #divMiddle
        {
        	margin:auto auto;
        	padding:0px 0px;
        	background:url(../images/login/loginTable.PNG);
        	width:627px;
            height:348px;
            border:0px;
            overflow:hidden;
           
        }
        #divLeft
        {
        	border:0px;
        	padding:0px 0px;
        	margin:0px 0px;
        	width:240px;
        	float:left;
        }
        #divRight
        {
        	border:0px;
        	padding:0px 0px;
        	margin:0px 0px;
        	width:387px;
        	height:348px;
        	float:right;
        }
        
        #divContentTop
        {
        	width:100%;
        	height:120px;
            border:0px;
            margin:0px 0px;
            padding:0xp 0px;
        }
        #divContentMiddle
        {
        	width:100%;
            border:0px;
            margin:0px 0px;
            padding:0xp 0px;
        }
        .tableContent
        {
        	width:100%;
        }
        .tdLable
        {
        	margin:0px  0px;
        	height:30px;
        	text-align:right;
        	width:20%;
        	white-space: nowrap;
        }
        .tdText
        {
        	margin:0px  0px;
        	height:30px;
        	text-align:left;
        	width:40%;
        	white-space: nowrap;
        }
        .tdEmpty
        {
        	margin:0px  0px;
        	height:30px;
        	text-align:left;
        	width:40%;
        	white-space: nowrap;
        }
    </style>
    <script type="text/javascript" src="../JavaScript/jquery/jquery-1.4.2.min.js"></script>
    <script type="text/javascript">
    var iCount=0;
    var divBottom='<%=divBottom.ClientID %>';
    $(document).ready(function(){
        $(window).resize(function(){
          var w=$(window).width();
          var h=$(window).height();
          $("#divMain").width(w).height(h);
          var middle=$("#divMiddle");
          middle.css({"margin-top":(h-middle.height())/2,"margin-left":(w-middle.width())/2 }); 
        });
        $(window).resize();
    });
     function showMessage(sMsg)
    {
      //  alert(sMsg);
      //$("#"+divBottom).innerText=sMsg;
       // document.getElementById("divBottom").innerText=sMsg;
      //s $("#divBottom").text(sMsg);
    }
    </script>
</head>
<body style=" margin:0px 0px; padding:0px 0px; width:100%; height:100%; overflow:hidden;">
    <form id="form1" runat="server"  style=" margin:0px 0px; padding:0px 0px; width:100%; height:100%; overflow:hidden;">
        <div id="divMain">
            <div id="divMiddle"     align="center" >
                <div id="divLeft"></div>
                <div id="divRight">
                    <div id="divContentTop"></div>
                    <div id="divContentMiddle">
                         <table  class="tableContent" >
                            <tr   <%=this.IsSingleLogin ? "style='display:none'" : "style=''" %> >
                                <td  class="tdLable">用户名</td>
                                <td  class="tdText">
                                    <asp:TextBox  runat="server" Width="99%" ID="txtUserName"  AutoPostBack="true"  ontextchanged="txtUserName_TextChanged"     ></asp:TextBox>
                                </td>
                                <td class=" tdEmpty"></td>
                            </tr>
                            <tr <%=this.IsSingleLogin ? "style='display:none'" : "style=''" %> >
                                <td   class="tdLable">密码</td>
                                <td  class="tdText">
                                    <asp:TextBox  runat="server" ID="txtPwd"   Width="99%" TextMode="Password"   ></asp:TextBox>
                                </td>
                                <td class=" tdEmpty"></td>
                            </tr>
                             <tr>
                                <td  class="tdLable">岗位</td>
                                <td  class="tdText"  nowrap="nowrap">
                                    <asp:DropDownList runat="server" ID="dpLst" Width="60%"></asp:DropDownList>
                                    <asp:Button runat="server" ID="btnComeIn" Text="进 入" onclick="btnComeIn_Click"  style="width: 38%" Visible="false" />
                                </td>
                                <td class=" tdEmpty"></td>
                            </tr>
                        <tr>
                            <td></td>
                            <td colspan="2"   >
                                <div id="divBottom" runat="server" style="text-align:left; vertical-align:top; padding-top:10px; width:100%; height:30px;  font-weight:bold;"></div>
                            </td>
                        </tr>
                 </table>
                    </div>
                   
                </div>
                
                <asp:Button runat="server" ID="btnGetJob" onclick="btnGetJob_Click" style=" display:none"  />
            </div>
        </div>
    
   
    
    </form>
     
</body>
 
</html>
