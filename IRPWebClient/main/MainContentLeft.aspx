<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainContentLeft.aspx.cs"
    Inherits="Main_MainContentLeft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">

        //分隔面板
        function imgLeftMouseOver(obj) {
            obj.style.cursor = "pointer";
        }
        function imgLeftMouseOut(obj) {
            obj.style.cursor = "default";
        }
        function imgRightMouseOver(obj) {
            obj.style.cursor = "pointer";
        }
        function imgRightMouseOut(obj) {
            obj.style.cursor = "default";
        }
        function imgLeftClick(obj) {
            var mainFrameSet = window.parent.window.parent.document.getElementById("mainFrameSet");

            if (mainFrameSet) {
                mainFrameSet.setAttribute("cols", "0,*");
                obj.style.display = "none";
                document.getElementById("imgRight").style.display = "block";
            }
        }
        function imgRightClick(obj) {
            var mainFrameSet = window.parent.window.parent.document.getElementById("mainFrameSet");

            if (mainFrameSet) {
                mainFrameSet.setAttribute("cols", "15%,*");
                obj.style.display = "none";
                document.getElementById("imgLeft").style.display = "block";
            }
        }
        window.onload = function() {
            SetSplitPanelHeight();
        }
        window.onresize = function() {
            SetSplitPanelHeight();
        }

        function SetSplitPanelHeight() {
            document.getElementById("tbMain").setAttribute("height", document.documentElement.clientHeight);
        }

        function GetParentObject(tagName, objId) {
            var inputs = parent.parent.document.getElementsByTagName(tagName);
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
<body style="  background-color:#CCE6F3; margin: 0px; padding: 0px ;">
    <table id="tbMain" style="margin: 0px auto" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 8px; background-image: url(../images/FrameConfig/vpagebg.gif);" valign="middle">
                <img id="imgLeft" alt="" title="点击隐藏菜单栏" width="8px;" height="42px" src="../images/FrameConfig/varrowleft.png"
                    onclick="return imgLeftClick(this);" onmouseover="return imgLeftMouseOver(this);"
                    onmouseout="return imgLeftMouseOut(this);" /><img id="imgRight" alt="" title="点击显示菜单栏"
                        width="8px;" style="display: none;" height="42px" src="../images/FrameConfig/varrowright.gif"
                        onclick="return imgRightClick(this);" onmouseover="return imgRightMouseOver(this);"
                        onmouseout="return imgRightMouseOut(this);" />
            </td>
        </tr>
    </table>
</body>
</html>
