<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPageBottomMap.aspx.cs" Inherits="MainPage_MainPageBottomMap" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/MainWeb.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .CurrMultiPage
        {
            border-style: solid;
            border-width: 2px 2px 2px 2px;
            border-color: #3F8BBC #3F8BBC #3F8BBC #3F8BBC;
            background-color: #FFFFFF;
            width: 100%;
            height: 100%;
            position: relative; /*-top:-1px;-*/
            z-index: 98;
            padding: 2px;
        }
        .tbHeaderTitle
        {
            background: url(../images/mainPage/header.bmp); /*filter: progid:DXImageTransform.Microsoft.gradient(enable=true,startColorstr=#EAF6FD, endColorstr=#A7D9F5);*/
        }
        .tbWarn
        {
            margin-left: 5px;
        }
        .tbChildWarn
        {
            margin: 0px auto;
            width: 35px;
            height: 32px;
            cursor: pointer;
            background-color: #D6E1F1;
        }
       .tdWarn
        {
            border: solid 10px #FFFFFF;
        }
        .tdTab
        {
            color: #1A3B69;
            font-family: Verdana;
            font-size: 9pt;
            font-weight: bold;
            vertical-align: top;
        }
    </style>

    <script type="text/javascript" src="../JavaScript/jquery/jquery-1.4.2.min.js"></script>

    <script src="../JavaScript/Js.js" type="text/javascript"></script>

    <script type="text/javascript">
        window.onload = function() {
            //parent.document.getElementById("frmBottom").setAttribute("height", document.body.clientHeight + "px");

            //            var height = window.parent.document.getElementById("frmLeftChild").clientHeight - 44 - 23 - 6;
            //            document.getElementById("tbContentRight").setAttribute("height", height + "px");

            //            if (window.parent && window.parent.document) {
            //                var frmMapChart = window.parent.document.getElementById("frmMapChart");
            //                if (frmMapChart) {
            //                    frmMapChart.setAttribute("width", "800px");
            //                    frmMapChart.setAttribute("height", "500px");
            //                }
            //            }

            var navigator = checkNavigator();
            if (navigator == "IE6" || navigator == "IE7") {
                document.getElementById("tbTabPage").style.marginBottom = "-4px";
            }
        }

        $(function() {
            $("#imgPic").click(function() {
                $("#btnPicUrl").click();
                
            });
            //            //添加鼠标动作监听
            //            $('#tdCost').hover(function(e) {
            //                $('#show').html(getText(this));
            //                DialogSwitch('showCost', 'block', mouseOffset(e).left - 110, getOffset($(this).attr('id')).top - ($("#showCost").height() / 2));
            //            }, function() {
            //                $('#show').html(getText(this));
            //                DialogSwitch('showCost', 'none', 0, 0);
            //            });

            //            //添加鼠标动作监听
            //            $('#tdDuration').hover(function(e) {
            //                $('#show').html(getText(this));
            //                DialogSwitch('showDuration', 'block', mouseOffset(e).left - 110, getOffset($(this).attr('id')).top - ($("#showDuration").height() / 2));
            //            }, function() {
            //                $('#show').html(getText(this));
            //                DialogSwitch('showDuration', 'none', 0, 0);
            //            });

            //            //添加鼠标动作监听
            //            $('#tdSafe').hover(function(e) {
            //                $('#show').html(getText(this));
            //                DialogSwitch('showSafe', 'block', mouseOffset(e).left - 110, getOffset($(this).attr('id')).top - ($("#showSafe").height() / 2));
            //            }, function() {
            //                $('#show').html(getText(this));
            //                DialogSwitch('showSafe', 'none', 0, 0);
            //            });

            //            //添加鼠标动作监听
            //            $('#tdQuality').hover(function(e) {
            //                $('#show').html(getText(this));
            //                DialogSwitch('showQuality', 'block', mouseOffset(e).left - 110, getOffset($(this).attr('id')).top - ($("#showQuality").height() / 2));
            //            }, function() {
            //                $('#show').html(getText(this));
            //                DialogSwitch('showQuality', 'none', 0, 0);
            //            });
        });
        function getText(id) {
            return $(id).find('td:eq(0)').html();
        }

        //弹出框开关控制
        function DialogSwitch(id, sty, l, t) {
            var showDiv = document.getElementById(id);

            //此属性一定要设置
            showDiv.style.position = "absolute";

            //显示隐藏控制
            showDiv.style.display = sty;
            //设置DIV显示位置
            showDiv.style.marginLeft = l;
            showDiv.style.marginTop = (t - getOffset(id).top);
        }

        //获取鼠标所在元素的位移
        function getOffset(id) {
            var left = $('#' + id).offset().left;
            var top = $('#' + id).offset().top;
            return { left: left, top: top };
        }
        var Mouse = function(e) {
            mouse = new MouseEvent(e);
            var leftpos = mouse.x;
            var toppos = mouse.y;
            return { left: leftpos, top: toppos };
        }

        //获取鼠标位置
        function mouseOffset(e) {
            var mouse = new MouseEvent(e);
            var leftpos = mouse.x;
            var toppos = mouse.y;
            return { left: leftpos, top: toppos };
        }
        //获取鼠标坐标函数
        var MouseEvent = function(e) {
            this.x = e.pageX
            this.y = e.pageY
        }

        function TabPageClick(obj) {

            var orgName = document.getElementById("txtOrgName").value;
            var optFlag = document.getElementById("txtOptFlag").value;

            if (obj.indexOf("MonthCostAnalysis") > -1) {

                if (optFlag == "false") {
                    alert("您没有访问“" + orgName + "”该选项卡的权限!");
                    return false;
                }

                document.getElementById("trAddCostAnalysis").style.display = "none";
                document.getElementById("trProjectBaseInfoCostAuth").style.display = "none";
                document.getElementById("trMonthCostAnalysis").style.display = "block";

                document.getElementById("tdAddCostAnalysis").style.display = "none";
                document.getElementById("tdProjectBaseInfo").style.display = "none";
                document.getElementById("tdMonthCostAnalysis").style.display = "block";
            }
            else if (obj.indexOf("AddCostAnalysis") > -1) {

                if (optFlag == "false") {
                    alert("您没有访问“" + orgName + "”该选项卡的权限!");
                    return false;
                }

                document.getElementById("trMonthCostAnalysis").style.display = "none";
                document.getElementById("trProjectBaseInfoCostAuth").style.display = "none";
                document.getElementById("trAddCostAnalysis").style.display = "block";

                document.getElementById("tdMonthCostAnalysis").style.display = "none";
                document.getElementById("tdProjectBaseInfo").style.display = "none";
                document.getElementById("tdAddCostAnalysis").style.display = "block";
            }
            else if (obj.indexOf("ProjectBaseInfo") > -1) {
                document.getElementById("trAddCostAnalysis").style.display = "none";
                document.getElementById("trMonthCostAnalysis").style.display = "none";
                document.getElementById("trProjectBaseInfoCostAuth").style.display = "block";

                document.getElementById("tdAddCostAnalysis").style.display = "none";
                document.getElementById("tdMonthCostAnalysis").style.display = "none";
                document.getElementById("tdProjectBaseInfo").style.display = "block";
            }
        }
    </script>

</head>
<body class="login" style="background-color: White; height: 100%; font-family: Trebuchet MS;">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td valign="top" style="width: 270px;">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Chart ID="Chart2" runat="server" EnableViewState="true" BackColor="#FFFFFF"
                                Palette="BrightPastel" BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
                                BorderWidth="2" BorderColor="26, 59, 105" Height="260px" Width="270px" IsSoftShadows="False"
                                ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
                                <Titles>
                                    <asp:Title ShadowColor="32, 0, 0, 0" ShadowOffset="3" Font="Trebuchet MS, 14.25pt, style=Bold"
                                        Visible="false" Text="成本、工期、质量、安全预警" Name="Title1" ForeColor="26, 59, 105">
                                    </asp:Title>
                                </Titles>
                                <Legends>
                                    <asp:Legend TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent"
                                        IsEquallySpacedItems="True" Font="Trebuchet MS, 8pt, style=Bold" IsTextAutoFit="False"
                                        Name="Default" LegendStyle="Column" Enabled="false">
                                    </asp:Legend>
                                </Legends>
                                <%--<BorderSkin SkinStyle="Emboss"></BorderSkin>--%>
                                <Series>
                                    <asp:Series ChartArea="ChartArea1" Name="Default" ChartType="Doughnut" XValueType="Auto"
                                        YValueType="Double" Font="Trebuchet MS, 8.25pt, style=Bold" CustomProperties="DoughnutRadius=40, PieDrawingStyle=Concave, CollectedLabel=Other, MinimumRelativePieSize=20"
                                        MarkerStyle="Circle" BorderColor="64, 64, 64, 64" Color="180, 65, 140, 240">
                                        <Points>
                                            <asp:DataPoint AxisLabel="安全" YValues="25" BorderColor="#DCE5F3" ToolTip="" />
                                            <asp:DataPoint AxisLabel="质量" YValues="25" BorderColor="#DCE5F3" ToolTip="" />
                                            <asp:DataPoint AxisLabel="成本" YValues="25" BorderColor="#DCE5F3" ToolTip="" />
                                            <asp:DataPoint AxisLabel="工期" YValues="25" BorderColor="#DCE5F3" ToolTip="" />
                                        </Points>
                                    </asp:Series>
                                </Series>
                                <Annotations>
                                    <asp:TextAnnotation Name="txtProjectCount" AnchorX="50" AnchorY="50" Alignment="MiddleCenter"
                                        Text="成本、工期、质量、安全\n\n批注信息" AnchorAlignment="MiddleCenter">
                                    </asp:TextAnnotation>
                                </Annotations>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent"
                                        BackColor="Transparent" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                        <Area3DStyle Enable3D="true" />
                                        <AxisY2>
                                            <MajorGrid Enabled="False" />
                                            <MajorTickMark Enabled="False" />
                                        </AxisY2>
                                        <AxisX2>
                                            <MajorGrid Enabled="False" />
                                            <MajorTickMark Enabled="False" />
                                        </AxisX2>
                                        <Area3DStyle PointGapDepth="900" Rotation="162" IsRightAngleAxes="False" WallWidth="25"
                                            IsClustered="False" />
                                        <AxisY LineColor="64, 64, 64, 64">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                            <MajorTickMark Enabled="False" />
                                        </AxisY>
                                        <AxisX LineColor="64, 64, 64, 64">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                            <MajorTickMark Enabled="False" />
                                        </AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom" align="center">
                            <%--<BorderSkin SkinStyle="Emboss"></BorderSkin>--%>
                                        <table class="tbWarn" style=" height:20px;">
                                            <tr style=" height:16px;">
                                                <td colspan="4" style="text-align: center">
                                                    预警顺序：
                                                </td>
                                            </tr>
                                            <tr style=" height:4px;">
                                                <td class="tdWarn" style="background-color: Green; width: 35px; height:6px;">
                                                </td>
                                                <td class="tdWarn" style="background-color: Yellow; width: 35px; height:6px;">
                                                </td>
                                                <td class="tdWarn" style="background-color: orange; width: 35px; height:6px;">
                                                </td>
                                                <td class="tdWarn" style="background-color: Red; width: 35px; height:6px;">
                                                </td>
                                            </tr>
                                        </table>
                            <%-- <table style="margin: 0px auto; text-align: center; margin-top: 4px;">
                                <tr>
                                    <td>--%>
                            <div id="show">
                            </div>
                           
                           <%-- </td>
                                </tr>
                            </table>--%>
                        </td>
                    </tr>
                </table>
            </td>
            <td runat="server">
                <asp:Image ID="imgPic" runat="server" ImageAlign="Middle" 
                    ToolTip="点击查看远程生产视频" />
            </td>      
        </tr>
    </table>
    <div style=" display:none">
        <asp:HiddenField ID="txtOrgName" runat="server" />
        <asp:HiddenField ID="txtOptFlag" runat="server" Value="false" />
        <asp:Button ID="btnPicUrl" runat="server" onclick="btnPicUrl_Click" 
            Text="Button" />
    </div>
    </form>
</body>
</html>
