<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPageLeft.aspx.cs" Inherits="MainPage_MainPageLeft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/MainWeb.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        window.onload = function() {
            createTreeOption();
        }

        function createTreeOption() {

            var ProjectId = document.getElementById("txtProjectId").value;

            var tree = document.getElementById("NavigateTree");
            var links = tree.getElementsByTagName("a");
            for (var i = 0; i < links.length; i++) {
                if (links[i].id.indexOf("NavigateTreet") > -1) {
                    links[i].onclick = nodeOption;

                    if (links[i].href.indexOf(ProjectId) > -1)
                        link = links[i];
                }
            }

            if (link != false) {
                link.click();
            }
        }
        var link = false;
        function nodeOption() {
            var obj = this;
            if (link && link != obj) {
                link.style.backgroundColor = "";
            }
            var rand = Math.random(); 
            if (obj.style.backgroundColor == "") {
                obj.style.backgroundColor = "#94CCFF";
            }

            var frmContentChild = window.parent.document.getElementById("frmContent").contentWindow.document.getElementById("frmContentChild");

            if (frmContentChild)
                frmContentChild.src = obj.href + "&userCode=" + escape(document.getElementById("txtUserCode").value)
                    + "&proInfoAuth=" + escape(document.getElementById("txtProInfoAuth").value) + "&rand=" + Math.random();


            //window.parent.document.getElementById("frmContent").src = obj.href + "&rand=" + Math.random();

            link = obj;

            //document.getElementById("txtProjectId").value = obj.id;

            return false;
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style='width: 100%;'>
        <table style='width: 100%;' cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <div id="divMessage" runat="server" style="color: Blue;">
                    </div>
                    <asp:TreeView ID="NavigateTree" runat="server" Font-Size="9pt" ShowLines="True" ExpandDepth="1"
                        ForeColor="Blue" Style="margin-left: -4px;" EnableViewState="false">
                    </asp:TreeView>
                </td>
            </tr>
        </table>
    </div>
    <div style='display: none;'>
        <input type="hidden" id="txtProjectId" runat="server" />
        <input type="hidden" id="txtTargetPageType" runat="server" />
        <input type="hidden" id="txtIsPageLoad" runat="server" />
        <input type="hidden" id="txtUserCode" runat="server" />
        <input type="hidden" id="txtProInfoAuth" runat="server" />
    </div>
    </form>
</body>
</html>
