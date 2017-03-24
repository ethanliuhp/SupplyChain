<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main_bak.aspx.cs" Inherits="main_main_bak" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript" src="../javascript/jquery-1.10.2.js"></script>

    <script type="text/javascript">
        function SetOperationSrc(sUrl) {

            //            var opt = $("#frOperate");
            //            if (opt)
            //                opt.attr("src", sUrl);

            var frOperate = document.getElementById("frOperate");
            if (!frOperate)
                return false;

            var frmContentChild = frOperate.contentWindow.document.getElementById("frmContentChild");
            if (!frmContentChild)
                return false;

            frmContentChild.src = sUrl;

            //显示进度条
            var tmpUrl = sUrl.toLowerCase();
            if (tmpUrl.indexOf("__dopostback") == -1 && tmpUrl.indexOf(".aspx") > 0) {

                var mainFramesetChildCon = frOperate.contentWindow.document.getElementById("mainFramesetChildCon");
                if (mainFramesetChildCon)
                    mainFramesetChildCon.setAttribute("rows", "30,*");

                var frmHeaderChild = frOperate.contentWindow.document.getElementById("frmHeaderChild");
                if (frmHeaderChild) {
                    var divHeadLoader = frmHeaderChild.contentWindow.document.getElementById("divHeadLoader");
                    if (divHeadLoader) {
                        divHeadLoader.style.display = "block";
                    }
                }
            }

            return false;
        }
    </script>

</head>
<%--<frameset rows="80px,*" framespacing="0" frameborder="0">
    <frame src="Top.aspx" id="frmTop" scrolling="no" marginwidth="0" marginheight="0" framespacing="0"  style="border-bottom:solid 2px #388CEB;" ></frame>--%>
   <frameset id="mainFrameSet" cols="15%,*" framespacing="4" frameborder="0"> 
        <frame src="Menu.aspx" id="frMenu" scrolling="auto" marginwidth="0" marginheight="0" framespacing="0" style="border-right:solid 0px #388CEB;"  ></frame>
        <frame src="mainContent.htm" id="frOperate" scrolling="auto" marginwidth="0" marginheight="0" framespacing="0" ></frame>
    </frameset>
    <%--<frame src="Botton.aspx" id="frBotton" scrolling="no" marginwidth="0" marginheight="0" framespacing="0" style="border:solid 1px #BFBFBF;"></frame>--%>
<%--</frameset>--%>
<%--<frameset rows="95%,5%">
            <frameset cols="20%,80%">
                <frame src="Menu.aspx" id="frMenu"></frame>
                <frame src="Operation.aspx" id="frOperate" ></frame>
            </frameset>
            <frame src="Botton.aspx" id="frBotton"  frameborder="0" scrolling="no" ></frame>
        </frameset>--%>
</html>

