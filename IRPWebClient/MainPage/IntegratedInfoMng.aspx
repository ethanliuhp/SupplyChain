<%@ Page Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true" CodeFile="IntegratedInfoMng.aspx.cs" Inherits="IntegratedInfoMng_" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ubsContentPage" runat="Server">

    <script type="text/javascript">
        window.onload = function() {
            var frmContentMain = document.getElementById('<%=frmContentMain.ClientID %>');
            frmContentMain.setAttribute("height", (document.documentElement.clientHeight + 100) + "px");

            //document.getElementById('<%=frmContentMain.ClientID %>').setAttribute("height", document.body.clientHeight + "px");

            var frmLeftChild = frmContentMain.contentWindow.document.getElementById("frmContent").contentWindow.document.getElementById("frmLeftChild");
            if (frmLeftChild) {
                var tbMain = frmLeftChild.contentWindow.document.getElementById("tbMain");
                tbMain.setAttribute("height", (document.documentElement.clientHeight + 100) + "px");
            }

//            var frmContentChild = frmContentMain.contentWindow.document.getElementById("frmContent").contentWindow.document.getElementById("frmContentChild");
//            if (frmContentChild) {
//                var tbContentRight = frmContentChild.contentWindow.document.getElementById("tbContentRight");
//                tbContentRight.setAttribute("height", (document.documentElement.clientHeight + 100 - 44 - 23 - 6) + "px");
//            }
        }
    </script>

    <iframe id="frmContentMain" runat="server" frameborder="0" scrolling="no" style="width: 100%;
        border: none; padding: 0px auto; margin: 0px auto;"></iframe>
</asp:Content>
