<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Map_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>

    <script type='text/javascript'>


        // window.moveTo(0, 0);
        // window.resizeTo(screen.availWidth, screen.availHeight);
        // var width=screen.availWidth;
        //  var height=screen.availHeight;

        //document.getElementById("form1").style.width=width*0.1;
        //document.getElementById("form1").style.height=height;

        // document.getElementById("ctlPanel").style.width=width*0.1;
        ///document.getElementById("ctlPanel").style.height=height;

        // document.getElementById("ctlPanel").style.width=width*0.1;
        // document.getElementById("ctlPanel").style.height=height;

        // document.getElementById("mapPanel").style.width=width*0.9;
        // document.getElementById("mapPanel").style.height=height;

        function Load() {

            createTreeOption();

            var height = document.documentElement.clientHeight;
            var oForm = document.getElementById("main");
            oForm.setAttribute("height", height + "px");

            document.getElementById("ifrmMap").setAttribute("height", height + "px");

            if (parent && parent.document) {
                var frmContent = GetParentObject("iframe", "frmContent");
                if (frmContent) {
                    frmContent.setAttribute("height", document.body.clientHeight + "px");
                }
            }
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
                obj.style.backgroundColor = "#CCCCCC";
            }

            var ifrmMap = document.getElementById("ifrmMap");

            if (ifrmMap)
                ifrmMap.src = obj.href + "&rand=" + Math.random();

            link = obj;

            return false;
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
        function switchSysBar() {
            // debugger;
            if (switchPoint.innerText == 3) {
                switchPoint.innerText = 4
                document.all("ctlPanel").style.display = "none";
                //document.all("mapPanel").style.width = screen.availWidth * 0.99;
            } else {
                switchPoint.innerText = 3
                document.all("ctlPanel").style.display = "block";
                //document.all("mapPanel").style.width = screen.availWidth * 0.89;
            }
        }

        function LoadMap() {
            document.getElementById("swichtPanel").style.height = document.getElementById("mapPanel").style.height;
        }
        
    </script>

    <style type="text/css">
        #main
        {
            padding: 0px auto;
            margin: 0px auto;
            height: 100%;
            width: 100%;
        }
        #ctlPanel
        {
            width: 250px;
            height: 500px;
            overflow: auto;
            padding: 0px;
            margin: 0px;
            vertical-align: top;
        }
        #midPanel
        {
            width: 1%;
            height: 100%;
            padding: 0px;
            margin: 0px;
            background-color: #708EC7;
        }
        #mapPanel
        {
            width: 100%;
            height: 100%;
            text-align: left;
            padding: 0px;
            margin: 0px;
        }
        #swichtPanel
        {
            width: 100%;
            height: 100%;
            padding: 0px;
            margin: 0px;
            background-color: Black;
            visibility: inherit;
            z-index: 2;
        }
        #ifrmMap
        {
            width: 100%;
            margin: 0px;
            padding: 0px;
        }
        #form1
        {
            padding: 0px;
            margin: 0px;
        }
        .navPoint
        {
            color: white;
            cursor: hand;
            font-family: Webdings;
            font-size: 9pt;
            width: 100%;
            height: 100%;
            padding: 0;
            margin: 0;
        }
    </style>
</head>
<body onload="Load();" style="padding: 0px; margin: 0px">
    <form id="form1" runat="server">
    <table id="main" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td id="ctlPanel">
                <asp:TreeView ID="NavigateTree" runat="server" Font-Size="9pt" ShowLines="True" ExpandDepth="1"
                    Style="margin-left: -4px;" EnableViewState="false">
                </asp:TreeView>
                <div style='display: none;'>
                    <input type="hidden" id="txtProjectId" runat="server" />
                </div>
            </td>
            <td id="midPanel">
                <table style="height: 100%; width: 100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="200" style="height: 100%" onclick="switchSysBar()">
                            <!--这里调用上面的switchSysBar过程-->
                            <font style="font-size: 9pt; cursor: default; color: #ffffff"><span class="navPoint"
                                id="switchPoint" title="关闭/打开左栏">3</span> 屏幕切换</font>
                        </td>
                    </tr>
                </table>
            </td>
            <td id="mapPanel" valign="top">
                <iframe scrolling="no" id="ifrmMap" runat="server" frameborder="0" marginwidth="0"
                    marginheight="0"></iframe>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
