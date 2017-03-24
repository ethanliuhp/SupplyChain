<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPageTop.aspx.cs" Inherits="MainPage_MainPageTop" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/samples.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/MainWeb.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        window.onload = function() {
            parent.document.getElementById("frmTop").setAttribute("height", document.body.clientHeight + "px");
        }

    </script>

</head>
<body id="bodyTop" style="background-color: White; height: 100%;">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; margin: 0px auto;">
        <tr>
            <td colspan="3" style="padding-top: 0px;">
                <h1>
                </h1>
            </td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:Chart ID="Chart2" runat="server" EnableViewState="true" BackColor="#D3DFF0"
                    Palette="BrightPastel" BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
                    BorderWidth="2" BorderColor="26, 59, 105" Height="350px" Width="500px" IsSoftShadows="False"
                    ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
                    <Titles>
                        <asp:Title ShadowColor="32, 0, 0, 0" ShadowOffset="3" Font="Trebuchet MS, 14.25pt, style=Bold"
                            Text="成本、工期、质量、安全预警" Name="Title1" ForeColor="26, 59, 105">
                        </asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent"
                            IsEquallySpacedItems="True" Font="Trebuchet MS, 8pt, style=Bold" IsTextAutoFit="False"
                            Name="Default" LegendStyle="Column" Enabled="false">
                        </asp:Legend>
                    </Legends>
                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                    <Series>
                        <asp:Series ChartArea="ChartArea1" Name="Default" ChartType="Doughnut" XValueType="Auto"
                            YValueType="Double" Font="Trebuchet MS, 8.25pt, style=Bold" CustomProperties="DoughnutRadius=40, PieDrawingStyle=Concave, CollectedLabel=Other, MinimumRelativePieSize=20"
                            MarkerStyle="Circle" BorderColor="64, 64, 64, 64" Color="180, 65, 140, 240">
                            <Points>
                                <asp:DataPoint AxisLabel="安全" YValues="25" BorderColor="#DCE5F3" />
                                <asp:DataPoint AxisLabel="质量" YValues="25" BorderColor="#DCE5F3" />
                                <asp:DataPoint AxisLabel="成本" YValues="25" BorderColor="#DCE5F3" />
                                <asp:DataPoint AxisLabel="工期" YValues="25" BorderColor="#DCE5F3" />
                            </Points>
                        </asp:Series>
                    </Series>
                    <Annotations>
                        <asp:TextAnnotation Name="txtProjectCount" AnchorX="50" AnchorY="55" Alignment="MiddleCenter"
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
            <td valign="top" style="display: none;">
                <table class="controls" cellpadding="4">
                    <tr>
                        <td class="label">
                            成本模块颜色设置:
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListColor" runat="server" AutoPostBack="True" CssClass="spaceright"
                                OnSelectedIndexChanged="DropDownListColor_SelectedIndexChanged">
                                <asp:ListItem Value="Red">赤</asp:ListItem>
                                <asp:ListItem Value="Orange">橙</asp:ListItem>
                                <asp:ListItem Value="Yellow">黄</asp:ListItem>
                                <asp:ListItem Value="Green">绿</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" style="display: none;">
                <table class="controls" cellpadding="4">
                    <tr>
                        <td class="label">
                            图形种类:
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListPieType" runat="server" AutoPostBack="True" CssClass="spaceright"
                                Width="112px" OnSelectedIndexChanged="DropDownListPieType_SelectedIndexChanged">
                                <asp:ListItem Value="Doughnut">Doughnut</asp:ListItem>
                                <asp:ListItem Value="Pie">Pie</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            圆环半径(%):
                        </td>
                        <td>
                            <asp:DropDownList ID="HoleSizeList" runat="server" AutoPostBack="True" Enabled="False"
                                CssClass="spaceright" Width="112px" OnSelectedIndexChanged="HoleSizeList_SelectedIndexChanged">
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                                <asp:ListItem Value="40">40</asp:ListItem>
                                <asp:ListItem Value="50">50</asp:ListItem>
                                <asp:ListItem Value="60" Selected="True">60</asp:ListItem>
                                <asp:ListItem Value="70">70</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            标注文本的样式:
                        </td>
                        <td>
                            <asp:DropDownList ID="LabelStyleList" runat="server" AutoPostBack="True" CssClass="spaceright"
                                Width="112px" OnSelectedIndexChanged="LabelStyleList_SelectedIndexChanged">
                                <asp:ListItem Value="Inside" Selected="True">Inside</asp:ListItem>
                                <asp:ListItem Value="Outside">Outside</asp:ListItem>
                                <asp:ListItem Value="Disabled">Disabled</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            突出显示的地区:
                        </td>
                        <td>
                            <asp:DropDownList ID="ExplodedPointList" runat="server" AutoPostBack="True" CssClass="spaceright"
                                Width="112px" OnSelectedIndexChanged="ExplodedPointList_SelectedIndexChanged">
                                <asp:ListItem Value="None" Selected="True">None</asp:ListItem>
                                <asp:ListItem Value="武汉">武汉</asp:ListItem>
                                <asp:ListItem Value="北京">北京</asp:ListItem>
                                <asp:ListItem Value="上海">上海</asp:ListItem>
                                <asp:ListItem Value="天津">天津</asp:ListItem>
                                <asp:ListItem Value="济南">济南</asp:ListItem>
                                <asp:ListItem Value="郑州">郑州</asp:ListItem>
                                <asp:ListItem Value="西安">西安</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            饼图样式:
                        </td>
                        <td>
                            <asp:DropDownList ID="DropdownlistPieDrawingStyle" runat="server" Width="112px" CssClass="spaceright"
                                AutoPostBack="True" OnSelectedIndexChanged="DropdownlistPieDrawingStyle_SelectedIndexChanged">
                                <asp:ListItem Value="Default">Default</asp:ListItem>
                                <asp:ListItem Value="SoftEdge" Selected="True">SoftEdge</asp:ListItem>
                                <asp:ListItem Value="Concave">Concave</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            3D显示:
                        </td>
                        <td>
                            <asp:CheckBox ID="CheckboxShow3D" runat="server" AutoPostBack="True" Text="" OnCheckedChanged="CheckboxShow3D_CheckedChanged">
                            </asp:CheckBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
