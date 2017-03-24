<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPageBottom.aspx.cs" Inherits="MainPage_MainPageBottom" %>

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
            $("#tdPic").mouseover(function() {
                DialogSwitch1('showPic', 'block');
            });
            $("#tdPic").mouseout(function() {
                DialogSwitch1('showPic', 'none');
            });
            $("#imgPic").click(function() {
                $("#btnPicUrl").click();

            });
            //添加鼠标动作监听
            //            $('#tdPic').mouseover(function() {
            //                DialogSwitch('showPic', 'block', mouseOffset(e).left - 110, getOffset($(this).attr('id')).top - ($("#showPic").height() / 2));
            //            }, function() {
            //                DialogSwitch('showPic', 'none', 0, 0);
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
        //        function getText(id) {
        //            return $(id).find('td:eq(0)').html();
        //        }
        function getText(id) {
            return $("#showPic").html();
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

        //弹出框开关控制
        function DialogSwitch1(id, sty) {
            var showDiv = document.getElementById(id);

            //此属性一定要设置
            showDiv.style.position = "absolute";

            //显示隐藏控制
            showDiv.style.display = sty;

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
                document.getElementById("trProjectBaseInfo").style.display = "none";
                document.getElementById("trMonthCostAnalysis").style.display = "block";
                document.getElementById("trWarnProjectBaseInfo").style.display = "none";
                document.getElementById("trWarnProjectBaseInfoCost").style.display = "none";
                document.getElementById("trWarn").style.display = "none";

                document.getElementById("tdAddCostAnalysis").style.display = "none";
                document.getElementById("tdProjectBaseInfo").style.display = "none";
                document.getElementById("tdMonthCostAnalysis").style.display = "block";
                document.getElementById("tdWarnPic").style.display = "none";
            }
            else if (obj.indexOf("AddCostAnalysis") > -1) {

                if (optFlag == "false") {
                    alert("您没有访问“" + orgName + "”该选项卡的权限!");
                    return false;
                }

                document.getElementById("trMonthCostAnalysis").style.display = "none";
                document.getElementById("trProjectBaseInfoCostAuth").style.display = "none";
                document.getElementById("trProjectBaseInfo").style.display = "none";
                document.getElementById("trAddCostAnalysis").style.display = "block";
                document.getElementById("trWarnProjectBaseInfo").style.display = "none";
                document.getElementById("trWarnProjectBaseInfoCost").style.display = "none";
                document.getElementById("trWarn").style.display = "none";

                document.getElementById("tdMonthCostAnalysis").style.display = "none";
                document.getElementById("tdProjectBaseInfo").style.display = "none";
                document.getElementById("tdAddCostAnalysis").style.display = "block";
                document.getElementById("tdWarnPic").style.display = "none";
            }
            else if (obj.indexOf("ProjectBaseInfo") > -1) {
                document.getElementById("trAddCostAnalysis").style.display = "none";
                document.getElementById("trMonthCostAnalysis").style.display = "none";
                if (document.getElementById("txtBaseinfo").value == "false") {
                    document.getElementById("trProjectBaseInfoCostAuth").style.display = "block";
                    document.getElementById("trProjectBaseInfo").style.display = "none";
                }
                else {
                    document.getElementById("trProjectBaseInfo").style.display = "block";
                    document.getElementById("trProjectBaseInfoCostAuth").style.display = "none";
                }
                document.getElementById("trWarnProjectBaseInfo").style.display = "none";
                document.getElementById("trWarnProjectBaseInfoCost").style.display = "none";
                document.getElementById("trWarn").style.display = "none";

                document.getElementById("tdAddCostAnalysis").style.display = "none";
                document.getElementById("tdMonthCostAnalysis").style.display = "none";
                document.getElementById("tdProjectBaseInfo").style.display = "block";
                document.getElementById("tdWarnPic").style.display = "none";
            }
            else if (obj.indexOf("Warning") > -1) {
                document.getElementById("trMonthCostAnalysis").style.display = "none";
                document.getElementById("trProjectBaseInfoCostAuth").style.display = "none";
                document.getElementById("trProjectBaseInfo").style.display = "none";
                document.getElementById("trAddCostAnalysis").style.display = "none";
                document.getElementById("trWarnProjectBaseInfo").style.display = "none";
                document.getElementById("trWarnProjectBaseInfoCost").style.display = "none";
                document.getElementById("trWarn").style.display = "block";

                document.getElementById("tdAddCostAnalysis").style.display = "none";
                document.getElementById("tdMonthCostAnalysis").style.display = "none";
                document.getElementById("tdProjectBaseInfo").style.display = "none";
                document.getElementById("tdWarnPic").style.display = "block";
            }
            else if (obj.indexOf("WarnBaseInfo") > -1) {
                document.getElementById("trMonthCostAnalysis").style.display = "none";
                document.getElementById("trProjectBaseInfoCostAuth").style.display = "none";
                document.getElementById("trProjectBaseInfo").style.display = "none";
                document.getElementById("trAddCostAnalysis").style.display = "none";
                document.getElementById("trWarnProjectBaseInfo").style.display = "block";
                document.getElementById("trWarnProjectBaseInfoCost").style.display = "none";
                document.getElementById("trWarn").style.display = "none";

                document.getElementById("tdAddCostAnalysis").style.display = "none";
                document.getElementById("tdMonthCostAnalysis").style.display = "none";
                document.getElementById("tdProjectBaseInfo").style.display = "none";
                document.getElementById("tdWarnPic").style.display = "block";
            }
            else if (obj.indexOf("WarnCostBaseInfo") > -1) {
                document.getElementById("trMonthCostAnalysis").style.display = "none";
                document.getElementById("trProjectBaseInfoCostAuth").style.display = "none";
                document.getElementById("trProjectBaseInfo").style.display = "none";
                document.getElementById("trAddCostAnalysis").style.display = "none";
                document.getElementById("trWarnProjectBaseInfo").style.display = "none";
                document.getElementById("trWarnProjectBaseInfoCost").style.display = "block";
                document.getElementById("trWarn").style.display = "none";

                document.getElementById("tdAddCostAnalysis").style.display = "none";
                document.getElementById("tdMonthCostAnalysis").style.display = "none";
                document.getElementById("tdProjectBaseInfo").style.display = "none";
                document.getElementById("tdWarnPic").style.display = "block";
            }
        }
    </script>

</head>
<body class="login" style="background-color: White; height: 100%; font-family: Trebuchet MS;">
    <form id="form1" runat="server">
    <div id="showPic" style="position: absolute; z-index: 1000; display: none; border: 1px solid #FFFFFF;
        background-color: #FFFFFF; z-index: 999; margin-top: 100px; margin-left: 280px;">
        <asp:Image ID="imgPicBig" runat="server" />
    </div>
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
                            <table style="margin: 0px auto; text-align: center; margin-top: 4px;">
                                <tr>
                                    <td>
                                        <table class="tbWarn">
                                            <tr>
                                                <td colspan="4" style="text-align: center">
                                                    预警顺序：
                                                </td>
                                            </tr>
                                            <tr style="height: 5px">
                                                <td class="tdWarn" style="background-color: Green; width: 35px; height: 6px;">
                                                </td>
                                                <td class="tdWarn" style="background-color: Yellow; width: 35px; height: 6px;">
                                                </td>
                                                <td class="tdWarn" style="background-color: orange; width: 35px; height: 6px;">
                                                </td>
                                                <td class="tdWarn" style="background-color: Red; width: 35px; height: 6px;">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td>
                                        <table class="tbWarn">
                                            <tr>
                                                <td>
                                                    预警规则：
                                                </td>
                                                <td class="tdWarn" style="border-left: solid 5px #FFFFFF;">
                                                    <table class="tbChildWarn" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td id="tdCost">
                                                                成本
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="tdWarn">
                                                    <table class="tbChildWarn" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td id="tdDuration">
                                                                工期
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="tdWarn">
                                                    <table class="tbChildWarn" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td id="tdSafe">
                                                                安全
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="tdWarn">
                                                    <table class="tbChildWarn" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td id="tdQuality">
                                                                质量
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>--%>
                            </table>
                            <%--<div id="showQuality" style="position: absolute; z-index: 1000; display: none; border: 1px solid #FFFFFF;
                                background-color: #FFFFFF;">
                                <img alt="" src="../images/warn/quality.bmp" />
                            </div>
                            <div id="showSafe" style="position: absolute; z-index: 1000; display: none; border: 1px solid #FFFFFF;
                                background-color: #FFFFFF;">
                                <img alt="" src="../images/warn/safe.bmp" />
                            </div>
                            <div id="showDuration" style="position: absolute; z-index: 1000; display: none; border: 1px solid #FFFFFF;
                                background-color: #FFFFFF;">
                                <img alt="" src="../images/warn/duration.bmp" />
                            </div>--%>
                            <div style="display: none;">
                                <div id="lblCostDesc" runat="server" style="font-size: 10pt; margin-top: 10px; padding-left: 60px;
                                    word-break: break-all;">
                                </div>
                                <div id="lblDurationDesc" runat="server" style="font-size: 10pt; margin-top: 10px;
                                    padding-left: 60px; word-break: break-all;">
                                </div>
                                <div id="lblQualityDesc" runat="server" style="font-size: 10pt; margin-top: 10px;
                                    padding-left: 60px; word-break: break-all;">
                                </div>
                                <div id="lblSafeDesc" runat="server" style="font-size: 10pt; margin-top: 10px; padding-left: 60px;
                                    word-break: break-all;">
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="margin-left: 10px;" id="tdPic">
                            <asp:Image ID="imgPic" runat="server" Width="255" Height="220" 
                                ToolTip="点击查看远程生产视频" />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" align="left">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="4">
                            <table id="tbTabPage" border="0" cellspacing="0" cellpadding="0" width="100%" style="margin-top: 13px;">
                                <tr id="trProjectBaseInfo" runat="server" style="display: block;">
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Background.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGActive.png); background-position: top;
                                        background-repeat: repeat-x; cursor: pointer; vertical-align: middle; white-space: nowrap;"
                                        onclick="return TabPageClick('ProjectBaseInfo');">
                                        项目基本信息
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightActive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('WarnBaseInfo');">
                                        预警规则
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td colspan="6" class="tdTab" width="100%" background="../images/tabs/Tab-Background.png">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trProjectBaseInfoCostAuth" runat="server" style="display: none;">
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Background.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGActive.png); background-position: top;
                                        background-repeat: repeat-x; cursor: pointer; vertical-align: middle; white-space: nowrap;"
                                        onclick="return TabPageClick('ProjectBaseInfo');">
                                        项目基本信息
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-LeftisActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('MonthCostAnalysis');">
                                        月度成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('AddCostAnalysis');">
                                        累计成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('WarnCostBaseInfo');">
                                        预警规则
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab" width="100%" background="../images/tabs/Tab-Background.png">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trMonthCostAnalysis" runat="server" style="display: none;">
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Background.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('ProjectBaseInfo');">
                                        项目基本信息
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-RightisActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGActive.png); background-position: top;
                                        background-repeat: repeat-x; cursor: pointer; vertical-align: middle; white-space: nowrap;"
                                        onclick="return TabPageClick('MonthCostAnalysis');">
                                        月度成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightActive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('AddCostAnalysis');">
                                        累计成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('Warning');">
                                        预警规则
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab" width="100%" background="../images/tabs/Tab-Background.png">
                                        &nbsp;
                                    </td>
                                    <%--<td class="tdTab">
                                        <img src="../images/tabs/Tab-Background.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGActive.png); background-position: top;
                                        background-repeat: repeat-x; cursor: pointer; vertical-align: middle; white-space: nowrap;"
                                        onclick="return TabPageClick('MonthCostAnalysis');">
                                        月度成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-LeftisActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('AddCostAnalysis');">
                                        累计成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('ProjectBaseInfo');">
                                        项目基本信息
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab" width="100%" background="../images/tabs/Tab-Background.png">
                                        &nbsp;
                                    </td>--%>
                                </tr>
                                <tr id="trAddCostAnalysis" runat="server" style="display: none;">
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Background.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('ProjectBaseInfo');">
                                        项目基本信息
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('MonthCostAnalysis');">
                                        月度成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGActive.png); background-position: top;
                                        background-repeat: repeat-x; cursor: pointer; vertical-align: middle; white-space: nowrap;"
                                        onclick="return TabPageClick('AddCostAnalysis');">
                                        累计成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightActive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('Warning');">
                                        预警规则
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab" width="100%" background="../images/tabs/Tab-Background.png">
                                        &nbsp;
                                    </td>
                                    <%--<td class="tdTab">
                                        <img src="../images/tabs/Tab-Background.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('MonthCostAnalysis');">
                                        月度成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-RightisActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGActive.png); background-position: top;
                                        background-repeat: repeat-x; cursor: pointer; vertical-align: middle; white-space: nowrap;"
                                        onclick="return TabPageClick('AddCostAnalysis');">
                                        累计成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightActive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('ProjectBaseInfo');">
                                        项目基本信息
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab" width="100%" background="../images/tabs/Tab-Background.png">
                                        &nbsp;
                                    </td>--%>
                                </tr>
                                <tr id="trWarnProjectBaseInfo" runat="server" style="display: none;">
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Background.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('ProjectBaseInfo');">
                                        项目基本信息
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGActive.png); background-position: top;
                                        background-repeat: repeat-x; cursor: pointer; vertical-align: middle; white-space: nowrap;"
                                        onclick="return TabPageClick('WarnBaseInfo');">
                                        预警规则
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightActive.png" />
                                    </td>
                                    <td colspan="6" class="tdTab" width="100%" background="../images/tabs/Tab-Background.png">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trWarnProjectBaseInfoCost" runat="server" style="display: none;">
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Background.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('ProjectBaseInfo');">
                                        项目基本信息
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('MonthCostAnalysis');">
                                        月度成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('AddCostAnalysis');">
                                        累计成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGActive.png); background-position: top;
                                        background-repeat: repeat-x; cursor: pointer; vertical-align: middle; white-space: nowrap;"
                                        onclick="return TabPageClick('WarnCostBaseInfo');">
                                        预警规则
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightActive.png" />
                                    </td>
                                    <td class="tdTab" width="100%" background="../images/tabs/Tab-Background.png">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trWarn" runat="server" style="display: none;">
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Background.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('ProjectBaseInfo');">
                                        项目基本信息
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('MonthCostAnalysis');">
                                        月度成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftInactive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGInactive.png);
                                        background-position: top; background-repeat: repeat-x; cursor: pointer; vertical-align: middle;
                                        white-space: nowrap;" onclick="return TabPageClick('AddCostAnalysis');">
                                        累计成本情况分析图
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightInactive.png" />
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndLeftActive.png" />
                                    </td>
                                    <td class="tdTab" style="background: url(../images/tabs/Tab-Body-BGActive.png); background-position: top;
                                        background-repeat: repeat-x; cursor: pointer; vertical-align: middle; white-space: nowrap;"
                                        onclick="return TabPageClick('Warning');">
                                        预警规则
                                    </td>
                                    <td class="tdTab">
                                        <img src="../images/tabs/Tab-Body-EndRightActive.png" />
                                    </td>
                                    <td class="tdTab" width="100%" background="../images/tabs/Tab-Background.png">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td id="tdMonthCostAnalysis" style="border-left: solid 1px #3F8BBC; border-right: solid 1px #3F8BBC;
                            border-bottom: solid 1px #3F8BBC; display: none;">
                            <table style="height: 200px; margin: 3px; padding: 0px auto;">
                                <tr>
                                    <td>
                                        <asp:Chart ID="ChartMonthCostAnalysis" runat="server" Height="330px" Width="700px"
                                            EnableViewState="true" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" ImageType="Png"
                                            BackColor="#D3DFF0" Palette="BrightPastel" BorderDashStyle="Solid" BackSecondaryColor="White"
                                            BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105">
                                            <Titles>
                                                <asp:Title ShadowColor="32, 0, 0, 0" ShadowOffset="3" Font="Trebuchet MS, 14.25pt, style=Bold"
                                                    Text="月度成本分析图" Name="Title1" ForeColor="26, 59, 105">
                                                </asp:Title>
                                            </Titles>
                                            <Legends>
                                                <asp:Legend Enabled="true" IsTextAutoFit="true" Name="Default" BackColor="Transparent"
                                                    Alignment="Near" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold">
                                                </asp:Legend>
                                            </Legends>
                                            <%--<BorderSkin SkinStyle="Emboss"></BorderSkin>--%>
                                            <Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series1" LegendText="合同收入"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Blue" BorderWidth="2">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series2" LegendText="责任成本"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Orange" BorderWidth="2">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series3" LegendText="实际成本"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Red" BorderWidth="2">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series4" LegendText="实际收款"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Green" BorderWidth="2">
                                                </asp:Series>
                                            </Series>
                                            <ChartAreas>
                                                <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                                                    BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                                                    BackGradientStyle="TopBottom">
                                                    <Area3DStyle Rotation="9" Perspective="5" LightStyle="Realistic" Inclination="38"
                                                        PointDepth="200" IsRightAngleAxes="False" WallWidth="0" IsClustered="False" />
                                                    <Position X="0" Y="12" Height="85" Width="84"></Position>
                                                    <AxisY LineColor="64, 64, 64, 64" Title="万元" TextOrientation="Horizontal" TitleAlignment="Far">
                                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                                    </AxisY>
                                                    <AxisX LineColor="64, 64, 64, 64" Title="年/月" Interval="1" TitleAlignment="Far" LabelAutoFitStyle="IncreaseFont"
                                                        Maximum="12" Minimum="1">
                                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                                    </AxisX>
                                                </asp:ChartArea>
                                            </ChartAreas>
                                        </asp:Chart>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" style="padding-bottom: 10px; display: none;">
                                        <fieldset>
                                            <legend id="lblCostInfoTitle" runat="server">成本信息</legend>
                                            <table style="width: 100%; margin: 0px auto; text-align: left;" cellspacing="5" cellpadding="5">
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        合同收入(开工累计)：
                                                    </td>
                                                    <td>
                                                        <label id="lblContractTotalPrice" runat="server" class="tdBaseRight">
                                                        </label>
                                                        万元
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        责任成本(开工累计)：
                                                    </td>
                                                    <td>
                                                        <label id="lblResonsibleTotalPrice" runat="server" class="tdBaseRight">
                                                        </label>
                                                        万元
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        实际成本(开工累计)：
                                                    </td>
                                                    <td>
                                                        <label id="lblRealTotalPrice" runat="server" class="tdBaseRight">
                                                        </label>
                                                        万元
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        实际收款(开工累计)：
                                                    </td>
                                                    <td>
                                                        <label id="lblRealColltionsummoney" runat="server" class="tdBaseRight">
                                                        </label>
                                                        万元
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        利润率：
                                                    </td>
                                                    <td>
                                                        <label id="lblProfit" runat="server" class="tdBaseRight">
                                                        </label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td id="tdAddCostAnalysis" style="border-left: solid 1px #3F8BBC; border-right: solid 1px #3F8BBC;
                            border-bottom: solid 1px #3F8BBC; display: none;">
                            <table style="height: 200px; margin: 3px; padding: 0px auto;">
                                <tr>
                                    <td>
                                        <asp:Chart ID="ChartAddCostAnalysis" runat="server" Height="330px" Width="700px"
                                            EnableViewState="true" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" ImageType="Png"
                                            BackColor="#D3DFF0" Palette="BrightPastel" BorderDashStyle="Solid" BackSecondaryColor="White"
                                            BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105">
                                            <Titles>
                                                <asp:Title ShadowColor="32, 0, 0, 0" ShadowOffset="3" Font="Trebuchet MS, 14.25pt, style=Bold"
                                                    Text="累计成本分析图" Name="Title1" ForeColor="26, 59, 105">
                                                </asp:Title>
                                            </Titles>
                                            <Legends>
                                                <asp:Legend Enabled="true" IsTextAutoFit="true" Name="Default" BackColor="Transparent"
                                                    Alignment="Near" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold">
                                                </asp:Legend>
                                            </Legends>
                                            <%--<BorderSkin SkinStyle="Emboss"></BorderSkin>--%>
                                            <Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series1" LegendText="2012累计合同收入"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Blue" BorderWidth="1">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series2" LegendText="2012累计责任成本"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Orange" BorderWidth="1"
                                                    Enabled="false">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series3" LegendText="2012累计实际成本"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Red" BorderWidth="1">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series4" LegendText="2012累计收款"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Green" BorderWidth="1">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series20" LegendText=" "
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Transparent" BorderWidth="2">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series5" LegendText="2013累计合同收入"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Black" BorderWidth="2">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series6" LegendText="2013累计责任成本"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Orange" BorderWidth="2"
                                                    Enabled="false">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series7" LegendText="2013累计实际成本"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Yellow" BorderWidth="2">
                                                </asp:Series>
                                                <asp:Series XValueType="String" YValueType="String" Name="Series8" LegendText="2013累计收款"
                                                    ChartType="Line" BorderColor="180, 26, 59, 105" Color="Purple" BorderWidth="2">
                                                </asp:Series>
                                            </Series>
                                            <ChartAreas>
                                                <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                                                    BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                                                    BackGradientStyle="TopBottom">
                                                    <Area3DStyle Rotation="9" Perspective="5" LightStyle="Realistic" Inclination="38"
                                                        PointDepth="200" IsRightAngleAxes="False" WallWidth="0" IsClustered="False" />
                                                    <Position X="0" Y="12" Height="85" Width="76"></Position>
                                                    <AxisY LineColor="64, 64, 64, 64" LineDashStyle="Dash" Title="万元" TextOrientation="Stacked"
                                                        TitleAlignment="Far">
                                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                                    </AxisY>
                                                    <AxisX LineColor="64, 64, 64, 64" Title="月份" Interval="1" TitleAlignment="Far" LabelAutoFitStyle="IncreaseFont"
                                                        Maximum="12" Minimum="1">
                                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                                    </AxisX>
                                                </asp:ChartArea>
                                            </ChartAreas>
                                        </asp:Chart>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td id="tdProjectBaseInfo" style="border-left: solid 1px #3F8BBC; border-right: solid 1px #3F8BBC;
                            border-bottom: solid 1px #3F8BBC; display: block;">
                            <table style="height: 300px; width: 700px; margin: 3px;">
                                <tr>
                                    <td valign="top">
                                        <fieldset>
                                            <legend>工程简介</legend>
                                            <table style="width: 100%; margin: 0px auto; line-height: 20px; text-align: left;">
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        工程名称：
                                                    </td>
                                                    <td id="txtProjectName" runat="server" class="tdBaseRight">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        项目名称：
                                                    </td>
                                                    <td id="txtProName" runat="server" class="tdBaseRight">
                                                    </td>
                                                </tr>
                                                <%--    <tr>
                        <td class="tdBaseLeft">
                            项目类型：
                        </td>
                        <td id="txtProjectType" runat="server" class="tdBaseRight">
                        </td>
                        <td class="tdBaseLeft">
                            承包方式：
                        </td>
                        <td id="txtContractWay" runat="server" class="tdBaseRight">
                        </td>
                    </tr>--%>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        工程地点：
                                                    </td>
                                                    <td id="txtProvince" runat="server" class="tdBaseRight">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        承包范围：
                                                    </td>
                                                    <td id="txtContracte" runat="server" class="tdBaseRight">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--          <td class="tdBaseLeft">
                            结构类型：
                        </td>
                        <td id="txtStructType" runat="server" class="tdBaseRight">
                        </td>--%>
                                                    <td class="tdBaseLeft">
                                                        基础形式：
                                                    </td>
                                                    <td id="txtBace" runat="server" class="tdBaseRight">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        施工阶段：
                                                    </td>
                                                    <td id="txtLifeCycleState" runat="server" class="tdBaseRight">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        开工时间：
                                                    </td>
                                                    <td id="txtCreateDate" runat="server" class="tdBaseRight">
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" style="padding-top: 15px; padding-bottom: 15px;">
                                        <fieldset>
                                            <legend>商务信息</legend>
                                            <table style="width: 100%; margin: 0px auto; line-height: 20px; text-align: left;">
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        资金来源：
                                                    </td>
                                                    <td id="txtMoneySource" runat="server" class="tdBaseRight">
                                                    </td>
                                                </tr>
                                                <%--<tr>
                        <td class="tdBaseLeft">
                            资金到位情况：
                        </td>
                        <td id="txtMoneyStates" runat="server" class="tdBaseRight">
                        </td>
                    </tr>--%>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        工程造价：
                                                    </td>
                                                    <td>
                                                        <label id="txtProjectCost" runat="server" class="tdBaseRight">
                                                        </label>
                                                        万元
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td class="tdBaseLeft">
                                                        实际预算总额：
                                                    </td>
                                                    <td>
                                                        <label id="txtRealPreMoney" runat="server" class="tdBaseRight">
                                                        </label>
                                                        万元
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        土建合同总额：
                                                    </td>
                                                    <td>
                                                        <label id="txtConstractMoney" runat="server" class="tdBaseRight">
                                                        </label>
                                                        万元
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        安装合同总额：
                                                    </td>
                                                    <td>
                                                        <label id="txtInstallOrderMoney" runat="server" class="tdBaseRight">
                                                        </label>
                                                        万元
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        合同收款比例：
                                                    </td>
                                                    <td>
                                                        <label id="txtCollectProport" runat="server" class="tdBaseRight">
                                                        </label>
                                                        %
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                                    <td class="tdBaseLeft">
                                                        责任上缴比例：
                                                    </td>
                                                    <td>
                                                        <label id="txtTurnProport" runat="server" class="tdBaseRight">
                                                        </label>
                                                        %
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        大包模板木枋地上造价：
                                                    </td>
                                                    <td>
                                                        <label id="txtGroundPrice" runat="server" class="tdBaseRight">
                                                        </label>
                                                        元
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        大包模板木枋地下造价：
                                                    </td>
                                                    <td>
                                                        <label id="txtUnderPrice" runat="server" class="tdBaseRight">
                                                        </label>
                                                        元
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td class="tdBaseLeft">
                                                        备注信息：
                                                    </td>
                                                    <td id="txtExplain" runat="server" class="tdBaseRight">
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td id="tdWarnPic" style="border-left: solid 1px #3F8BBC; border-right: solid 1px #3F8BBC;
                            border-bottom: solid 1px #3F8BBC; display: none;">
                            <table>
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>成本</legend>
                                            <img alt="" src="../images/warn/cost.bmp" />
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>工期</legend>
                                            <img alt="" src="../images/warn/duration.bmp" />
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>安全</legend>
                                            <img alt="" src="../images/warn/safe.bmp" />
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>质量</legend>
                                            <img alt="" src="../images/warn/quality.bmp" />
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div>
        <asp:HiddenField ID="txtOrgName" runat="server" />
        <asp:HiddenField ID="txtOptFlag" runat="server" Value="false" />
        <asp:HiddenField ID="txtBaseinfo" runat="server" Value="true" />
    </div>
    <%--<div id="show">
    </div>--%>
    <div style=" display:none">
        <asp:Button ID="btnPicUrl" runat="server" onclick="btnPicUrl_Click" 
            Text="Button" />
    </div>
    </form>
</body>
</html>
