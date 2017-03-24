<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Main_Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/MainWeb.css" rel="stylesheet" type="text/css" />
    <link href="../css/xforms.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tree
        {
        }
    </style>

    <script type="text/javascript" src="../JavaScript/jquery-1.10.2.js"></script>

    <script type="text/javascript">

        var oMenuID = "<%=trvMenu.ClientID %>";
        $(document).ready(function() {

            //有浏览器兼容问题，ie10用不了
            //            $("div #" + omenuid + "  table td a").click(function() {
            //                return clicknode(this);
            //            });

            createTreeOption();
        });

        function ClickNode(t) {
            try {
                // debugger;
                var oImg = $(t).children("img");
                if (oImg != null && oImg.length > 0) {
                    return true;
                }
                else {

                    var sUrl = t;  // GetUrl($(t).html());
                    if (sUrl == null || sUrl == '') {
                        //alert("");
                    }
                    else {
                        window.parent.SetOperationSrc(sUrl);
                    }

                }
                //  alert($(t).html());
            }
            catch (e) {
                alert("出错");
            }
            return false;
        }
        function GetUrl(sName) {
            //debugger;
            var sUrl = "";
            var val = $("#<%=hdMenuArr.ClientID %>").val();
            if (val != '' && val != null) {
                var arr = eval(val);
                for (var index in arr) {
                    var o = arr[index];
                    if (o.Name == sName) {
                        sUrl = o.Url;
                        break;
                    }
                }
            }
            return sUrl;
        }


        function createTreeOption() {
            var tree = document.getElementById("trvMenu");
            var links = tree.getElementsByTagName("a");
            for (var i = 0; i < links.length; i++) {
                if (links[i].id.indexOf("trvMenut") > -1) {
                    links[i].onclick = nodeOption;
                }
            }
        }
        var link = false;
        function nodeOption() {
            var obj = this;
            if (link && link != obj) {
                link.style.backgroundColor = "";
                link.style.color = "#000000";
            }

            if (obj.style.backgroundColor == "") {
                obj.style.backgroundColor = "#000080";
                obj.style.color = "#FFFFFF";
            }
            if(obj && obj.href && obj.href.indexOf("javascript:__doPostBac")<0){
                window.parent.SetOperationSrc(obj.href);
            }

            link = obj;

            return false;
        }
    </script>

</head>
<body style=" background-color:#CCE6F3" >
    <form id="form1" runat="server">
    <div>
        <asp:TreeView runat="server" ID="trvMenu" ShowLines="True" ExpandDepth="1" EnableViewState="true"
            CssClass="tree" ForeColor="Black"  NoExpandImageUrl="~/images/tree/plus3.gif"
            CollapseImageUrl="~/images/tree/plus3.gif" ExpandImageUrl="~/images/tree/minus2.gif"
            NodeStyle-ImageUrl="~/images/tree/folderClosed.gif" LeafNodeStyle-ImageUrl="~/images/tree/treeNodeNew.gif"
            SelectedNodeStyle-ImageUrl="~/images/tree/folderOpen.gif" 
            onselectednodechanged="trvMenu_SelectedNodeChanged" >
<SelectedNodeStyle ImageUrl="~/images/tree/folderOpen.gif"></SelectedNodeStyle>

<NodeStyle ImageUrl="~/images/tree/folderClosed.gif"></NodeStyle>

<LeafNodeStyle ImageUrl="~/images/tree/treeNodeNew.gif"></LeafNodeStyle>
        </asp:TreeView>
    </div>
    <asp:HiddenField Value="" ID="hdMenuArr" runat="server" />
    </form>
</body>
</html>
