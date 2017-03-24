<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="MainPage"
    Title="ึ๗าณ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ึ๗าณ</title>
    <link href="CSS/MainWeb.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        window.onload = function() {
            //            if (parent && parent.document) {
            //                var frmContent = GetParentObject("iframe", "frmContent");
            //                if (frmContent) {
            //                    frmContent.setAttribute("height", "600px");
            //                }
            //            }
        }
        function GetObject(tagName, objId) {
            var inputs = document.getElementsByTagName(tagName);
            for (var i = 0; i < inputs.length; i++) {
                var inp = inputs[i];
                if (inp.id.indexOf(objId) > -1) {
                    return inp;
                    break;
                }
            }
            return null;
        }
        function GetParentObject(tagName, objId) {
            var inputs = parent.document.getElementsByTagName(tagName);
            for (var i = 0; i < inputs.length; i++) {
                var inp = inputs[i];
                if (inp.id.indexOf(objId) > -1) {
                    return inp;
                    break;
                }
            }
            return null;
        }
    </script>

</head>
<frameset cols="175,*" id="mainFrameSet" framespacing="4" frameborder="0">
        <frame scrolling="auto" id="frmLeft" runat="server"  marginwidth="0" marginheight="0" framespacing="0" style="border-right:solid 1px #BFBFBF;"/>
        <frame scrolling="auto" id="frmContent" runat="server" src="MainPage/MainPageContent.htm" marginwidth="0" marginheight="0" framespacing="0" />
</frameset>
</html>
