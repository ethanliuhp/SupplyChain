<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPageContent.aspx.cs"
    Inherits="MainPage_MainPageContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<frameset id="mainFramesetChild" cols="8,*" framespacing="0" frameborder="0">
    <frame scrolling="no" id="frmLeftChild" runat="server" src="MainPageContentLeft.aspx" marginwidth="0"
        marginheight="0" framespacing="0" />
    <frame scrolling="auto" id="frmContentChild" runat="server" src="MainPageBottom.aspx" marginwidth="0"
        marginheight="0" framespacing="0" />
</frameset>
</html>
